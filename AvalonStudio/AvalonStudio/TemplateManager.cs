using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Abstractions.Mount;
using Microsoft.TemplateEngine.Abstractions.PhysicalFileSystem;
using Microsoft.TemplateEngine.Cli;
using Microsoft.TemplateEngine.Cli.CommandParsing;
using Microsoft.TemplateEngine.Edge;
using Microsoft.TemplateEngine.Edge.Settings;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.TemplateEngine.Utils;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.TemplateEngine.Cli.HelpAndUsage;

namespace AvalonStudio
{
    internal static class HelpForTemplateResolution
    {
        public static CreationResultStatus CoordinateHelpAndUsageDisplay(TemplateListResolutionResult templateResolutionResult, IEngineEnvironmentSettings environmentSettings, INewCommandInput commandInput, IHostSpecificDataLoader hostDataLoader, ITelemetryLogger telemeteryLogger, TemplateCreator templateCreator, string defaultLanguage)
        {
            // this is just checking if there is an unambiguous group.
            // the called methods decide whether to get the default language filtered lists, based on what they're doing.
            if (templateResolutionResult.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroup)
                    && TemplateListResolver.AreAllTemplatesSameLanguage(unambiguousTemplateGroup))
            {
                // This will often show detailed help on the template group, which only makes sense if they're all the same language.
                return DisplayHelpForUnambiguousTemplateGroup(templateResolutionResult, environmentSettings, commandInput, hostDataLoader, templateCreator, defaultLanguage);
            }
            else
            {
                return DisplayHelpForAmbiguousTemplateGroup(templateResolutionResult, environmentSettings, commandInput, hostDataLoader, telemeteryLogger, defaultLanguage);
            }
        }

        private static CreationResultStatus DisplayHelpForUnambiguousTemplateGroup(TemplateListResolutionResult templateResolutionResult, IEngineEnvironmentSettings environmentSettings, INewCommandInput commandInput, IHostSpecificDataLoader hostDataLoader, TemplateCreator templateCreator, string defaultLanguage)
        {
            // filter on the default language if needed, the details display should be for a single language group
            if (!templateResolutionResult.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroupForDetailDisplay))
            {
                // this is really an error
                unambiguousTemplateGroupForDetailDisplay = new List<ITemplateMatchInfo>();
            }

            if (commandInput.IsListFlagSpecified)
            {
                // because the list flag is present, don't display help for the template group, even though an unambiguous group was resolved.
                if (!AreAllParamsValidForAnyTemplateInList(unambiguousTemplateGroupForDetailDisplay)
                    && TemplateListResolver.FindHighestPrecedenceTemplateIfAllSameGroupIdentity(unambiguousTemplateGroupForDetailDisplay) != null)
                {
                    DisplayHelpForAcceptedParameters(commandInput.CommandName);
                    return CreationResultStatus.InvalidParamValues;
                }

                // get the group without filtering on default language
                if (!templateResolutionResult.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroupForList, true))
                {
                    // this is really an error
                    unambiguousTemplateGroupForList = new List<ITemplateMatchInfo>();
                }

                DisplayTemplateList(unambiguousTemplateGroupForList, environmentSettings, commandInput.Language, defaultLanguage);
                // list flag specified, so no usage examples or detailed help
                return CreationResultStatus.Success;
            }
            else
            {
                // not in list context, but Unambiguous
                // this covers whether or not --help was input, they do the same thing in the unambiguous case
                return TemplateDetailedHelpForSingularTemplateGroup(unambiguousTemplateGroupForDetailDisplay, environmentSettings, commandInput, hostDataLoader, templateCreator);
            }
        }

        private static CreationResultStatus TemplateDetailedHelpForSingularTemplateGroup(IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroup, IEngineEnvironmentSettings environmentSettings, INewCommandInput commandInput, IHostSpecificDataLoader hostDataLoader, TemplateCreator templateCreator)
        {
            ShowUsageHelp(commandInput);

            // (scp 2017-09-06): parse errors probably can't happen in this context.
            foreach (string parseErrorMessage in unambiguousTemplateGroup.Where(x => x.HasParseError()).Select(x => x.GetParseError()).ToList())
            {
                //Reporter.Error.WriteLine(parseErrorMessage.Bold().Red());
            }

            GetParametersInvalidForTemplatesInList(unambiguousTemplateGroup, out IReadOnlyList<string> invalidForAllTemplates, out IReadOnlyList<string> invalidForSomeTemplates);

            if (invalidForAllTemplates.Count > 0 || invalidForSomeTemplates.Count > 0)
            {
                DisplayInvalidParameters(invalidForAllTemplates);
                DisplayParametersInvalidForSomeTemplates(invalidForSomeTemplates, LocalizableStrings.SingleTemplateGroupPartialMatchSwitchesNotValidForAllMatches);
            }

            bool showImplicitlyHiddenParams = unambiguousTemplateGroup.Count > 1;
            TemplateDetailsDisplay.ShowTemplateGroupHelp(unambiguousTemplateGroup, environmentSettings, commandInput, hostDataLoader, templateCreator, showImplicitlyHiddenParams);

            return invalidForAllTemplates.Count > 0 || invalidForSomeTemplates.Count > 0
                ? CreationResultStatus.InvalidParamValues
                : CreationResultStatus.Success;
        }

        private static CreationResultStatus DisplayHelpForAmbiguousTemplateGroup(TemplateListResolutionResult templateResolutionResult, IEngineEnvironmentSettings environmentSettings, INewCommandInput commandInput, IHostSpecificDataLoader hostDataLoader, ITelemetryLogger telemetryLogger, string defaultLanguage)
        {
            if (!string.IsNullOrEmpty(commandInput.TemplateName)
                && templateResolutionResult.GetBestTemplateMatchList(true).Count > 0
                && templateResolutionResult.GetBestTemplateMatchList(true).All(x => x.MatchDisposition.Any(d => d.Location == MatchLocation.Language && d.Kind == MatchKind.Mismatch)))
            {
                string errorMessage = GetLanguageMismatchErrorMessage(commandInput);
                //Reporter.Error.WriteLine(errorMessage.Bold().Red());
                //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, $"{commandInput.CommandName} {commandInput.TemplateName}").Bold().Red());
                return CreationResultStatus.NotFound;
            }

            // The following occurs when:
            //      --alias <value> is specifed
            //      --help is specified
            //      template (group) can't be resolved
            if (!string.IsNullOrWhiteSpace(commandInput.Alias))
            {
                //Reporter.Error.WriteLine(LocalizableStrings.InvalidInputSwitch.Bold().Red());
                //Reporter.Error.WriteLine("  " + commandInput.TemplateParamInputFormat("--alias").Bold().Red());
                return CreationResultStatus.NotFound;
            }

            bool hasInvalidParameters = false;
            IReadOnlyList<ITemplateMatchInfo> templatesForDisplay = templateResolutionResult.GetBestTemplateMatchList(true);
            GetParametersInvalidForTemplatesInList(templatesForDisplay, out IReadOnlyList<string> invalidForAllTemplates, out IReadOnlyList<string> invalidForSomeTemplates);
            if (invalidForAllTemplates.Any() || invalidForSomeTemplates.Any())
            {
                hasInvalidParameters = true;
                DisplayInvalidParameters(invalidForAllTemplates);
                DisplayParametersInvalidForSomeTemplates(invalidForSomeTemplates, LocalizableStrings.PartialTemplateMatchSwitchesNotValidForAllMatches);
            }

            if (commandInput.IsHelpFlagSpecified)
            {
                // usage help should show first (if it's being shown at all).
                telemetryLogger.TrackEvent(commandInput.CommandName + "-Help");
                ShowUsageHelp(commandInput);
            }

            ShowContextAndTemplateNameMismatchHelp(templateResolutionResult, commandInput.TemplateName, commandInput.TypeFilter);
            DisplayTemplateList(templatesForDisplay, environmentSettings, commandInput.Language, defaultLanguage);

            if (!commandInput.IsListFlagSpecified)
            {
                TemplateUsageHelp.ShowInvocationExamples(templateResolutionResult, hostDataLoader, commandInput.CommandName);
            }

            if (hasInvalidParameters)
            {
                return CreationResultStatus.NotFound;
            }
            else if (commandInput.IsListFlagSpecified || commandInput.IsHelpFlagSpecified)
            {
                return CreationResultStatus.Success;
            }
            else
            {
                return CreationResultStatus.OperationNotSpecified;
            }
        }

        // Returns true if any of the input templates has a valid parameter parse result.
        private static bool AreAllParamsValidForAnyTemplateInList(IReadOnlyList<ITemplateMatchInfo> templateList)
        {
            bool anyValidTemplate = false;

            foreach (ITemplateMatchInfo templateInfo in templateList)
            {
                if (templateInfo.GetInvalidParameterNames().Count == 0)
                {
                    anyValidTemplate = true;
                    break;
                }
            }

            return anyValidTemplate;
        }

        private static void DisplayHelpForAcceptedParameters(string commandName)
        {
            //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, commandName).Bold().Red());
        }

        // Displays the list of templates in a table, one row per template group.
        // 
        // The columns displayed are as follows
        // Note: Except language, the values are taken from one template in the group. The info could vary among the templates in the group, but shouldn't.
        // There is no check that the info doesn't vary.
        //  - Templates
        //  - Short Name
        //  - Language: All languages supported by the group are displayed, with the default language in brackets, e.g.: [C#]
        //  - Tags
        private static void DisplayTemplateList(IReadOnlyList<ITemplateMatchInfo> templates, IEngineEnvironmentSettings environmentSettings, string language, string defaultLanguage)
        {
            IReadOnlyDictionary<ITemplateInfo, string> templateGroupsLanguages = GetLanguagesForTemplateGroups(templates, language, defaultLanguage);

            HelpFormatter<KeyValuePair<ITemplateInfo, string>> formatter = HelpFormatter.For(environmentSettings, templateGroupsLanguages, 6, '-', false)
                .DefineColumn(t => t.Key.Name, LocalizableStrings.Templates)
                .DefineColumn(t => t.Key.ShortName, LocalizableStrings.ShortName)
                .DefineColumn(t => t.Value, out object languageColumn, LocalizableStrings.Language)
                .DefineColumn(t => t.Key.Classifications != null ? string.Join("/", t.Key.Classifications) : null, out object tagsColumn, LocalizableStrings.Tags)
                //.OrderByDescending(languageColumn, new NullOrEmptyIsLastStringComparer())
                .OrderBy(tagsColumn);
            //Reporter.Output.WriteLine(formatter.Layout());
        }

        private static IReadOnlyDictionary<ITemplateInfo, string> GetLanguagesForTemplateGroups(IReadOnlyList<ITemplateMatchInfo> templateList, string language, string defaultLanguage)
        {
            IEnumerable<IGrouping<string, ITemplateMatchInfo>> grouped = templateList.GroupBy(x => x.Info.GroupIdentity, x => !string.IsNullOrEmpty(x.Info.GroupIdentity));
            Dictionary<ITemplateInfo, string> templateGroupsLanguages = new Dictionary<ITemplateInfo, string>();

            foreach (IGrouping<string, ITemplateMatchInfo> grouping in grouped)
            {
                List<string> languageForDisplay = new List<string>();
                HashSet<string> uniqueLanguages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                string defaultLanguageDisplay = string.Empty;

                foreach (ITemplateMatchInfo template in grouping)
                {
                    if (template.Info.Tags != null && template.Info.Tags.TryGetValue("language", out ICacheTag languageTag))
                    {
                        foreach (string lang in languageTag.ChoicesAndDescriptions.Keys)
                        {
                            if (uniqueLanguages.Add(lang))
                            {
                                if (string.IsNullOrEmpty(language) && string.Equals(defaultLanguage, lang, StringComparison.OrdinalIgnoreCase))
                                {
                                    defaultLanguageDisplay = $"[{lang}]";
                                }
                                else
                                {
                                    languageForDisplay.Add(lang);
                                }
                            }
                        }
                    }
                }

                languageForDisplay.Sort(StringComparer.OrdinalIgnoreCase);
                if (!string.IsNullOrEmpty(defaultLanguageDisplay))
                {
                    languageForDisplay.Insert(0, defaultLanguageDisplay);
                }

                templateGroupsLanguages[grouping.First().Info] = string.Join(", ", languageForDisplay);
            }

            return templateGroupsLanguages;
        }

        public static void DisplayInvalidParameters(IReadOnlyList<string> invalidParams)
        {
            if (invalidParams.Count > 0)
            {
                //Reporter.Error.WriteLine(LocalizableStrings.InvalidInputSwitch.Bold().Red());
                foreach (string flag in invalidParams)
                {
                    //Reporter.Error.WriteLine($"  {flag}".Bold().Red());
                }
            }
        }

        private static void DisplayParametersInvalidForSomeTemplates(IReadOnlyList<string> invalidParams, string messageHeader)
        {
            if (invalidParams.Count > 0)
            {
                //Reporter.Error.WriteLine(messageHeader.Bold().Red());
                foreach (string flag in invalidParams)
                {
                    //Reporter.Error.WriteLine($"  {flag}".Bold().Red());
                }
            }
        }

        private static void ShowContextAndTemplateNameMismatchHelp(TemplateListResolutionResult templateResolutionResult, string templateName, string context)
        {
            if (string.IsNullOrEmpty(templateName))
            {
                return;
            }

            GetContextBasedAndOtherPartialMatches(templateResolutionResult, out IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> contextProblemMatchGroups, out IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> remainingPartialMatchGroups);
            DisplayPartialNameMatchAndContextProblems(templateName, context, contextProblemMatchGroups, remainingPartialMatchGroups);
        }

        private static void GetContextBasedAndOtherPartialMatches(TemplateListResolutionResult templateResolutionResult, out IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> contextProblemMatchGroups, out IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> remainingPartialMatchGroups)
        {
            Dictionary<string, List<ITemplateMatchInfo>> contextProblemMatches = new Dictionary<string, List<ITemplateMatchInfo>>();
            Dictionary<string, List<ITemplateMatchInfo>> remainingPartialMatches = new Dictionary<string, List<ITemplateMatchInfo>>();

            // this filtering / grouping ignores language differences.
            foreach (ITemplateMatchInfo template in templateResolutionResult.GetBestTemplateMatchList(true))
            {
                if (template.MatchDisposition.Any(x => x.Location == MatchLocation.Context && x.Kind != MatchKind.Exact))
                {
                    if (!contextProblemMatches.TryGetValue(template.Info.GroupIdentity, out List<ITemplateMatchInfo> templateGroup))
                    {
                        templateGroup = new List<ITemplateMatchInfo>();
                        contextProblemMatches.Add(template.Info.GroupIdentity, templateGroup);
                    }

                    templateGroup.Add(template);
                }
                else if (!templateResolutionResult.UsingContextMatches
                    && template.MatchDisposition.Any(t => t.Location != MatchLocation.Context && t.Kind != MatchKind.Mismatch && t.Kind != MatchKind.Unspecified))
                {
                    if (!remainingPartialMatches.TryGetValue(template.Info.GroupIdentity, out List<ITemplateMatchInfo> templateGroup))
                    {
                        templateGroup = new List<ITemplateMatchInfo>();
                        remainingPartialMatches.Add(template.Info.GroupIdentity, templateGroup);
                    }

                    templateGroup.Add(template);
                }
            }

            // context mismatches from the "matched" templates
            contextProblemMatchGroups = contextProblemMatches.Values.ToList();

            // other templates with anything matching
            remainingPartialMatchGroups = remainingPartialMatches.Values.ToList();
        }

        private static void DisplayPartialNameMatchAndContextProblems(string templateName, string context, IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> contextProblemMatchGroups, IReadOnlyList<IReadOnlyList<ITemplateMatchInfo>> remainingPartialMatchGroups)
        {
            if (contextProblemMatchGroups.Count + remainingPartialMatchGroups.Count > 1)
            {
                // Unable to determine the desired template from the input template name: {0}..
                //Reporter.Error.WriteLine(string.Format(LocalizableStrings.AmbiguousInputTemplateName, templateName).Bold().Red());
            }
            else if (contextProblemMatchGroups.Count + remainingPartialMatchGroups.Count == 0)
            {
                // No templates matched the input template name: {0}
                //Reporter.Error.WriteLine(string.Format(LocalizableStrings.NoTemplatesMatchName, templateName).Bold().Red());
                //Reporter.Error.WriteLine();
                return;
            }

            bool anythingReported = false;

            foreach (IReadOnlyList<ITemplateMatchInfo> templateGroup in contextProblemMatchGroups)
            {
                // all templates in a group should have the same context & name
                if (templateGroup[0].Info.Tags != null && templateGroup[0].Info.Tags.TryGetValue("type", out ICacheTag typeTag))
                {
                    MatchInfo? matchInfo = WellKnownSearchFilters.ContextFilter(context)(templateGroup[0].Info);
                    if ((matchInfo?.Kind ?? MatchKind.Mismatch) == MatchKind.Mismatch)
                    {
                        // {0} matches the specified name, but has been excluded by the --type parameter. Remove or change the --type parameter to use that template
                        //Reporter.Error.WriteLine(string.Format(LocalizableStrings.TemplateNotValidGivenTheSpecifiedFilter, templateGroup[0].Info.Name).Bold().Red());
                        anythingReported = true;
                    }
                }
                else
                {
                    // this really shouldn't ever happen. But better to have a generic error than quietly ignore the partial match.
                    //
                    //{0} cannot be created in the target location
                    //Reporter.Error.WriteLine(string.Format(LocalizableStrings.GenericPlaceholderTemplateContextError, templateGroup[0].Info.Name).Bold().Red());
                    anythingReported = true;
                }
            }

            if (remainingPartialMatchGroups.Count > 0)
            {
                // The following templates partially match the input. Be more specific with the template name and/or language
                //Reporter.Error.WriteLine(LocalizableStrings.TemplateMultiplePartialNameMatches.Bold().Red());
                anythingReported = true;
            }

            if (anythingReported)
            {
                //Reporter.Error.WriteLine();
            }
        }

        // Returns a list of the parameter names that are invalid for every template in the input group.
        public static void GetParametersInvalidForTemplatesInList(IReadOnlyList<ITemplateMatchInfo> templateList, out IReadOnlyList<string> invalidForAllTemplates, out IReadOnlyList<string> invalidForSomeTemplates)
        {
            IDictionary<string, int> invalidCounts = new Dictionary<string, int>();

            foreach (ITemplateMatchInfo template in templateList)
            {
                foreach (string paramName in template.GetInvalidParameterNames())
                {
                    if (!invalidCounts.ContainsKey(paramName))
                    {
                        invalidCounts[paramName] = 1;
                    }
                    else
                    {
                        invalidCounts[paramName]++;
                    }
                }
            }

            IEnumerable<IGrouping<string, string>> countGroups = invalidCounts.GroupBy(x => x.Value == templateList.Count ? "all" : "some", x => x.Key);
            invalidForAllTemplates = countGroups.FirstOrDefault(x => string.Equals(x.Key, "all", StringComparison.Ordinal))?.ToList();
            if (invalidForAllTemplates == null)
            {
                invalidForAllTemplates = new List<string>();
            }

            invalidForSomeTemplates = countGroups.FirstOrDefault(x => string.Equals(x.Key, "some", StringComparison.Ordinal))?.ToList();
            if (invalidForSomeTemplates == null)
            {
                invalidForSomeTemplates = new List<string>();
            }
        }

        public static void ShowUsageHelp(INewCommandInput commandInput)
        {
            //Reporter.Output.WriteLine(commandInput.HelpText);
            //Reporter.Output.WriteLine();
        }

        public static CreationResultStatus HandleParseError(INewCommandInput commandInput, ITelemetryLogger telemetryLogger)
        {
            TemplateListResolver.ValidateRemainingParameters(commandInput, out IReadOnlyList<string> invalidParams);
            DisplayInvalidParameters(invalidParams);

            // TODO: get a meaningful error message from the parser
            if (commandInput.IsHelpFlagSpecified)
            {
                telemetryLogger.TrackEvent(commandInput.CommandName + "-Help");
                ShowUsageHelp(commandInput);
            }
            else
            {
                //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, commandInput.CommandName).Bold().Red());
            }

            return CreationResultStatus.InvalidParamValues;
        }

        private static string GetLanguageMismatchErrorMessage(INewCommandInput commandInput)
        {
            string inputFlagForm;
            if (commandInput.Tokens.Contains("-lang"))
            {
                inputFlagForm = "-lang";
            }
            else
            {
                inputFlagForm = "--language";
            }

            string invalidLanguageErrorText = LocalizableStrings.InvalidTemplateParameterValues;
            invalidLanguageErrorText += Environment.NewLine + string.Format(LocalizableStrings.InvalidParameterDetail, inputFlagForm, commandInput.Language, "language");
            return invalidLanguageErrorText;
        }
    }

    internal static class TelemetryConstants
    {
        // event name suffixes
        public static readonly string InstallEventSuffix = "-install";
        public static readonly string HelpEventSuffix = "-help";
        public static readonly string CreateEventSuffix = "-create-template";
        public static readonly string CalledWithNoArgsEventSuffix = "-called-with-no-args";

        // install event args
        public static readonly string ToInstallCount = "CountOfThingsToInstall";

        // create event args
        public static readonly string Language = "language";
        public static readonly string ArgError = "argument-error";
        public static readonly string Framework = "framework";
        public static readonly string TemplateName = "template-name";
        public static readonly string IsTemplateThirdParty = "is-template-3rd-party";
        public static readonly string Auth = "auth";
        public static readonly string CreationResult = "create-success";
    }

    internal class ExtendedTemplateEngineHost : ITemplateEngineHost
    {
        private readonly TemplateManager _new3Command;
        private readonly ITemplateEngineHost _baseHost;

        public ExtendedTemplateEngineHost(ITemplateEngineHost baseHost, TemplateManager new3Command)
        {
            _baseHost = baseHost;
            _new3Command = new3Command;
        }

        public IPhysicalFileSystem FileSystem => _baseHost.FileSystem;

        public string Locale => _baseHost.Locale;

        public void UpdateLocale(string newLocale) => _baseHost.UpdateLocale(newLocale);

        public string HostIdentifier => _baseHost.HostIdentifier;

        public IReadOnlyList<string> FallbackHostTemplateConfigNames => _baseHost.FallbackHostTemplateConfigNames;

        public string Version => _baseHost.Version;

        public virtual IReadOnlyList<KeyValuePair<Guid, Func<Type>>> BuiltInComponents => _baseHost.BuiltInComponents;

        public virtual void LogMessage(string message) => _baseHost.LogMessage(message);

        public virtual void OnCriticalError(string code, string message, string currentFile, long currentPosition)
        {
            _baseHost.OnCriticalError(code, message, currentFile, currentPosition);
        }

        public virtual bool OnNonCriticalError(string code, string message, string currentFile, long currentPosition)
        {
            return _baseHost.OnNonCriticalError(code, message, currentFile, currentPosition);
        }

        public virtual bool OnParameterError(ITemplateParameter parameter, string receivedValue, string message, out string newValue)
        {
            return _baseHost.OnParameterError(parameter, receivedValue, message, out newValue);
        }

        public virtual void OnSymbolUsed(string symbol, object value) => _baseHost.OnSymbolUsed(symbol, value);

        public virtual bool TryGetHostParamDefault(string paramName, out string value)
        {
            switch (paramName)
            {
                case "GlobalJsonExists":
                    value = GlobalJsonFileExistsInPath.ToString();
                    return true;
                default:
                    return _baseHost.TryGetHostParamDefault(paramName, out value);
            }
        }

        public void VirtualizeDirectory(string path)
        {
            _baseHost.VirtualizeDirectory(path);
        }

        private static string GetChangeString(ChangeKind kind)
        {
            string changeType;

            switch (kind)
            {
                case ChangeKind.Change:
                    changeType = LocalizableStrings.Change;
                    break;
                case ChangeKind.Delete:
                    changeType = LocalizableStrings.Delete;
                    break;
                case ChangeKind.Overwrite:
                    changeType = LocalizableStrings.Overwrite;
                    break;
                default:
                    changeType = LocalizableStrings.UnknownChangeKind;
                    break;
            }

            return changeType;
        }

        public bool OnPotentiallyDestructiveChangesDetected(IReadOnlyList<IFileChange> changes, IReadOnlyList<IFileChange> destructiveChanges)
        {
            ////Reporter.Error.WriteLine(LocalizableStrings.DestructiveChangesNotification.Bold().Red());
            int longestChangeTextLength = destructiveChanges.Max(x => GetChangeString(x.ChangeKind).Length);
            int padLen = 5 + longestChangeTextLength;

            foreach (IFileChange change in destructiveChanges)
            {
                string changeKind = GetChangeString(change.ChangeKind);
                ////Reporter.Error.WriteLine(($"  {changeKind}".PadRight(padLen) + change.TargetRelativePath).Bold().Red());
            }

            ////Reporter.Error.WriteLine();
            ////Reporter.Error.WriteLine(LocalizableStrings.RerunCommandAndPassForceToCreateAnyway.Bold().Red());
            return false;
        }

        public bool OnConfirmPartialMatch(string name)
        {
            return true;
        }

        public void LogDiagnosticMessage(string message, string category, params string[] details)
        {
            _baseHost.LogDiagnosticMessage(message, category, details);
        }

        public virtual void LogTiming(string label, TimeSpan duration, int depth)
        {
            _baseHost.LogTiming(label, duration, depth);
        }

        private bool GlobalJsonFileExistsInPath
        {
            get
            {
                const string fileName = "global.json";

                string workingPath = Path.Combine(FileSystem.GetCurrentDirectory(), _new3Command.OutputPath);
                bool found = false;

                do
                {
                    string checkPath = Path.Combine(workingPath, fileName);
                    found = FileSystem.FileExists(checkPath);
                    if (!found)
                    {
                        workingPath = Path.GetDirectoryName(workingPath.TrimEnd('/', '\\'));

                        if (!FileSystem.DirectoryExists(workingPath))
                        {
                            workingPath = null;
                        }
                    }
                } while (!found && (workingPath != null));

                return found;
            }
        }
    }

    internal class Installer : IInstaller
    {
        private readonly IEngineEnvironmentSettings _environmentSettings;
        private readonly Paths _paths;

        public Installer(IEngineEnvironmentSettings environmentSettings)
        {
            _environmentSettings = environmentSettings;
            _paths = new Paths(environmentSettings);
        }

        public void AddInstallDescriptorForLocation(Guid mountPointId)
        {
            ((SettingsLoader)(_environmentSettings.SettingsLoader)).InstallUnitDescriptorCache.TryAddDescriptorForLocation(mountPointId);
        }

        public void InstallPackages(IEnumerable<string> installationRequests)
        {
            List<string> localSources = new List<string>();
            List<Package> packages = new List<Package>();

            foreach (string request in installationRequests)
            {
                string req = request;

                //If the request string doesn't have any wild cards or probable path indicators,
                //  and doesn't have a "::" delimiter either, try to convert it to "latest package"
                //  form
                if (OriginalRequestIsImplicitPackageVersionSyntax(request))
                {
                    req += "::*";
                }

                if (Package.TryParse(req, out Package package))
                {
                    packages.Add(package);
                }
                else
                {
                    localSources.Add(request);
                }
            }

            if (localSources.Count > 0)
            {
                InstallLocalPackages(localSources);
            }

            if (packages.Count > 0)
            {
                InstallRemotePackages(packages);
            }

            _environmentSettings.SettingsLoader.Save();
        }

        private bool OriginalRequestIsImplicitPackageVersionSyntax(string req)
        {
            if (req.IndexOfAny(new[] { '*', '?', '/', '\\' }) < 0 && req.IndexOf("::", StringComparison.Ordinal) < 0)
            {
                bool localFileSystemEntryExists = false;

                try
                {
                    localFileSystemEntryExists = _environmentSettings.Host.FileSystem.FileExists(req)
                                                 || _environmentSettings.Host.FileSystem.DirectoryExists(req);
                }
                catch
                {
                }

                return !localFileSystemEntryExists;
            }

            return false;
        }

        public IEnumerable<string> Uninstall(IEnumerable<string> uninstallRequests)
        {
            List<string> uninstallFailures = new List<string>();
            foreach (string uninstall in uninstallRequests)
            {
                string prefix = Path.Combine(_paths.User.Packages, uninstall);
                IReadOnlyList<MountPointInfo> rootMountPoints = _environmentSettings.SettingsLoader.MountPoints.Where(x =>
                {
                    if (x.ParentMountPointId != Guid.Empty)
                    {
                        return false;
                    }

                    if (uninstall.IndexOfAny(new[] { '/', '\\' }) < 0)
                    {
                        if (x.Place.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase) && x.Place.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase))
                        {
                            return true;
                        }
                    }

                    if (string.Equals(x.Place, uninstall, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                    else if (x.Place.Length > uninstall.Length)
                    {
                        string place = x.Place.Replace('\\', '/');
                        string match = uninstall.Replace('\\', '/');

                        if (match[match.Length - 1] != '/')
                        {
                            match += "/";
                        }

                        return place.StartsWith(match, StringComparison.OrdinalIgnoreCase);
                    }

                    return false;
                }).ToList();

                if (rootMountPoints.Count == 0)
                {
                    uninstallFailures.Add(uninstall);
                    continue;
                }

                HashSet<Guid> mountPoints = new HashSet<Guid>(rootMountPoints.Select(x => x.MountPointId));
                bool isSearchComplete = false;
                while (!isSearchComplete)
                {
                    isSearchComplete = true;
                    foreach (MountPointInfo possibleChild in _environmentSettings.SettingsLoader.MountPoints)
                    {
                        if (mountPoints.Contains(possibleChild.ParentMountPointId))
                        {
                            isSearchComplete &= !mountPoints.Add(possibleChild.MountPointId);
                        }
                    }
                }

                //Find all of the things that refer to any of the mount points we've got
                _environmentSettings.SettingsLoader.RemoveMountPoints(mountPoints);
                ((SettingsLoader)(_environmentSettings.SettingsLoader)).InstallUnitDescriptorCache.RemoveDescriptorsForLocationList(mountPoints);
                _environmentSettings.SettingsLoader.Save();

                foreach (MountPointInfo mountPoint in rootMountPoints)
                {
                    if (mountPoint.Place.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            _environmentSettings.Host.FileSystem.FileDelete(mountPoint.Place);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return uninstallFailures;
        }

        private void InstallRemotePackages(List<Package> packages)
        {
            const string packageRef = @"    <PackageReference Include=""{0}"" Version=""{1}"" />";
            const string projectFile = @"<Project ToolsVersion=""15.0"" Sdk=""Microsoft.NET.Sdk"">
  <PropertyGroup>
    <TargetFrameworks>netcoreapp1.0</TargetFrameworks>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Remove=""Microsoft.NETCore.App"" />
{0}
  </ItemGroup>
</Project>";

            _paths.CreateDirectory(_paths.User.ScratchDir);
            string proj = Path.Combine(_paths.User.ScratchDir, "restore.csproj");
            StringBuilder references = new StringBuilder();

            foreach (Package pkg in packages)
            {
                references.AppendLine(string.Format(packageRef, pkg.Name, pkg.Version));
            }

            string content = string.Format(projectFile, references.ToString());
            _paths.WriteAllText(proj, content);

            _paths.CreateDirectory(_paths.User.Packages);
            string restored = Path.Combine(_paths.User.ScratchDir, "Packages");
            Dotnet.Restore(proj, "--packages", restored).ForwardStdOut().ForwardStdErr().Execute();

            List<string> newLocalPackages = new List<string>();
            foreach (string packagePath in _paths.EnumerateFiles(restored, "*.nupkg", SearchOption.AllDirectories))
            {
                string path = Path.Combine(_paths.User.Packages, Path.GetFileName(packagePath));
                _paths.Copy(packagePath, path);
                newLocalPackages.Add(path);
            }

            _paths.DeleteDirectory(_paths.User.ScratchDir);
            InstallLocalPackages(newLocalPackages);
        }

        private void InstallLocalPackages(IReadOnlyList<string> packageNames)
        {
            List<string> toInstall = new List<string>();

            foreach (string package in packageNames)
            {
                if (package == null)
                {
                    continue;
                }

                string pkg = package.Trim();
                pkg = _environmentSettings.Environment.ExpandEnvironmentVariables(pkg);
                string pattern = null;

                int wildcardIndex = pkg.IndexOfAny(new[] { '*', '?' });

                if (wildcardIndex > -1)
                {
                    int lastSlashBeforeWildcard = pkg.LastIndexOfAny(new[] { '\\', '/' });
                    if (lastSlashBeforeWildcard >= 0)
                    {
                        pattern = pkg.Substring(lastSlashBeforeWildcard + 1);
                        pkg = pkg.Substring(0, lastSlashBeforeWildcard);
                    }
                }

                try
                {
                    if (pattern != null)
                    {
                        string fullDirectory = new DirectoryInfo(pkg).FullName;
                        string fullPathGlob = Path.Combine(fullDirectory, pattern);
                        ((SettingsLoader)(_environmentSettings.SettingsLoader)).UserTemplateCache.Scan(fullPathGlob, out IReadOnlyList<Guid> contentMountPointIds);

                        foreach (Guid mountPointId in contentMountPointIds)
                        {
                            AddInstallDescriptorForLocation(mountPointId);
                        }
                    }
                    else if (_environmentSettings.Host.FileSystem.DirectoryExists(pkg) || _environmentSettings.Host.FileSystem.FileExists(pkg))
                    {
                        string packageLocation = new DirectoryInfo(pkg).FullName;
                        ((SettingsLoader)(_environmentSettings.SettingsLoader)).UserTemplateCache.Scan(packageLocation, out IReadOnlyList<Guid> contentMountPointIds);

                        foreach (Guid mountPointId in contentMountPointIds)
                        {
                            AddInstallDescriptorForLocation(mountPointId);
                        }
                    }
                    else
                    {
                        _environmentSettings.Host.OnNonCriticalError("InvalidPackageSpecification", string.Format(LocalizableStrings.CouldNotFindItemToInstall, pkg), null, 0);
                    }
                }
                catch (Exception ex)
                {
                    _environmentSettings.Host.OnNonCriticalError("InvalidPackageSpecification", string.Format(LocalizableStrings.BadPackageSpec, pkg), null, 0);

                    if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_NEW_DEBUG")))
                    {
                        _environmentSettings.Host.OnNonCriticalError("InvalidPackageSpecificationDetails", ex.ToString(), null, 0);
                    }
                    else
                    {
                        _environmentSettings.Host.OnNonCriticalError("InvalidPackageSpecificationDetails", ex.Message, null, 0);
                    }
                }
            }
        }
    }

    public class TemplateManager
    {
        private readonly ITelemetryLogger _telemetryLogger;
        private readonly TemplateCreator _templateCreator;
        private readonly SettingsLoader _settingsLoader;
        private readonly AliasRegistry _aliasRegistry;
        private readonly Paths _paths;
        private readonly INewCommandInput _commandInput;    // It's safe to access template agnostic information anytime after the first parse. But there is never a guarantee which template the parse is in the context of.
        private readonly IHostSpecificDataLoader _hostDataLoader;
        private readonly string _defaultLanguage;
        private static readonly Regex LocaleFormatRegex = new Regex(@"
                    ^
                        [a-z]{2}
                        (?:-[A-Z]{2})?
                    $"
            , RegexOptions.IgnorePatternWhitespace);
        private readonly Action<IEngineEnvironmentSettings, IInstaller> _onFirstRun;
        private readonly Func<string> _inputGetter = () => Console.ReadLine();

        public TemplateManager(string commandName, ITemplateEngineHost host, ITelemetryLogger telemetryLogger, Action<IEngineEnvironmentSettings, IInstaller> onFirstRun, INewCommandInput commandInput, string hivePath)
        {
            _telemetryLogger = telemetryLogger;
            host = new ExtendedTemplateEngineHost(host, this);
            EnvironmentSettings = new EngineEnvironmentSettings(host, x => new SettingsLoader(x), hivePath);
            _settingsLoader = (SettingsLoader)EnvironmentSettings.SettingsLoader;
            Installer = new Installer(EnvironmentSettings);
            _templateCreator = new TemplateCreator(EnvironmentSettings);
            _aliasRegistry = new AliasRegistry(EnvironmentSettings);
            CommandName = commandName;
            _paths = new Paths(EnvironmentSettings);
            _onFirstRun = onFirstRun;
            _hostDataLoader = new HostSpecificDataLoader(EnvironmentSettings.SettingsLoader);
            _commandInput = commandInput;

            if (!EnvironmentSettings.Host.TryGetHostParamDefault("prefs:language", out _defaultLanguage))
            {
                _defaultLanguage = null;
            }

            var matches = QueryForTemplateMatches();
        }

        private bool ConfigureLocale()
        {
            if (!string.IsNullOrEmpty(_commandInput.Locale))
            {
                string newLocale = _commandInput.Locale;
                if (!ValidateLocaleFormat(newLocale))
                {
                    ////Reporter.Error.WriteLine(string.Format(LocalizableStrings.BadLocaleError, newLocale).Bold().Red());
                    return false;
                }

                EnvironmentSettings.Host.UpdateLocale(newLocale);
                // cache the templates for the new locale
                _settingsLoader.Reload();
            }

            return true;
        }

        private bool Initialize()
        {
            bool ephemeralHiveFlag = _commandInput.HasDebuggingFlag("--debug:ephemeral-hive");

            if (ephemeralHiveFlag)
            {
                EnvironmentSettings.Host.VirtualizeDirectory(_paths.User.BaseDir);
            }

            bool reinitFlag = _commandInput.HasDebuggingFlag("--debug:reinit");
            if (reinitFlag)
            {
                _paths.Delete(_paths.User.BaseDir);
            }

            // Note: this leaves things in a weird state. Might be related to the localized caches.
            // not sure, need to look into it.
            if (reinitFlag || _commandInput.HasDebuggingFlag("--debug:reset-config"))
            {
                _paths.Delete(_paths.User.AliasesFile);
                _paths.Delete(_paths.User.SettingsFile);
                _settingsLoader.UserTemplateCache.DeleteAllLocaleCacheFiles();
                _settingsLoader.Reload();
                return false;
            }

            if (!_paths.Exists(_paths.User.BaseDir) || !_paths.Exists(_paths.User.FirstRunCookie))
            {
                if (!_commandInput.IsQuietFlagSpecified)
                {
                    ////Reporter.Output.WriteLine(LocalizableStrings.GettingReady);
                }

                ConfigureEnvironment();
                _paths.WriteAllText(_paths.User.FirstRunCookie, "");
            }

            if (_commandInput.HasDebuggingFlag("--debug:showconfig"))
            {
                ShowConfig();
                return false;
            }

            return true;
        }

        private void ShowConfig()
        {
            
        }


        private void ConfigureEnvironment()
        {
            // delete everything from previous attempts for this install when doing first run setup.
            // don't want to leave partial setup if it's in a bad state.
            if (_paths.Exists(_paths.User.BaseDir))
            {
                _paths.DeleteDirectory(_paths.User.BaseDir);
            }

            _onFirstRun?.Invoke(EnvironmentSettings, Installer);
            EnvironmentSettings.SettingsLoader.Components.RegisterMany(typeof(New3Command).GetTypeInfo().Assembly.GetTypes());
        }

        private async Task<CreationResultStatus> ExecuteAsync()
        {
            // this is checking the initial parse, which is template agnostic.
            if (_commandInput.HasParseError)
            {
                throw new NotImplementedException();
                //return HelpForTemplateResolution.HandleParseError(_commandInput, _telemetryLogger);
            }

            if (_commandInput.IsHelpFlagSpecified)
            {
                _telemetryLogger.TrackEvent(CommandName + TelemetryConstants.HelpEventSuffix);
            }

            if (_commandInput.ShowAliasesSpecified)
            {
                return AliasSupport.DisplayAliasValues(EnvironmentSettings, _commandInput, _aliasRegistry, CommandName);
            }

            if (_commandInput.ExpandedExtraArgsFiles && string.IsNullOrEmpty(_commandInput.Alias))
            {   // Only show this if there was no alias expansion.
                // ExpandedExtraArgsFiles must be checked before alias expansion - it'll get reset if there's an alias.
                ////Reporter.Output.WriteLine(string.Format(LocalizableStrings.ExtraArgsCommandAfterExpansion, string.Join(" ", _commandInput.Tokens)));
            }

            if (string.IsNullOrEmpty(_commandInput.Alias))
            {
                // The --alias param is for creating / updating / deleting aliases.
                // If it's not present, try expanding aliases now.
                CreationResultStatus aliasExpansionResult = AliasSupport.CoordinateAliasExpansion(_commandInput, _aliasRegistry, _telemetryLogger);

                if (aliasExpansionResult != CreationResultStatus.Success)
                {
                    return aliasExpansionResult;
                }
            }

            if (!ConfigureLocale())
            {
                return CreationResultStatus.InvalidParamValues;
            }

            if (!Initialize())
            {
                return CreationResultStatus.Success;
            }

            bool forceCacheRebuild = _commandInput.HasDebuggingFlag("--debug:rebuildcache");
            try
            {
                _settingsLoader.RebuildCacheFromSettingsIfNotCurrent(forceCacheRebuild);
            }
            catch (EngineInitializationException eiex)
            {
                ////Reporter.Error.WriteLine(eiex.Message.Bold().Red());
                ////Reporter.Error.WriteLine(LocalizableStrings.SettingsReadError);
                return CreationResultStatus.CreateFailed;
            }

            try
            {
                if (!string.IsNullOrEmpty(_commandInput.Alias) && !_commandInput.IsHelpFlagSpecified)
                {
                    return AliasSupport.ManipulateAliasIfValid(_aliasRegistry, _commandInput.Alias, _commandInput.Tokens.ToList(), AllTemplateShortNames);
                }

                if (_commandInput.CheckForUpdates)
                {
                    // Don't return after updating. This way, if someone runs something like:
                    //      > dotnet new mvc --update-check
                    // we'll first check for updates, then try to invoke the template.
                    new TemplateUpdating(EnvironmentSettings, Installer, _inputGetter).Update(_settingsLoader.InstallUnitDescriptorCache.Descriptors.Values.ToList());
                }
                else if (_commandInput.CheckForUpdatesNoPrompt)
                {
                    new TemplateUpdating(EnvironmentSettings, Installer, _inputGetter).UpdateWithoutPrompting(_settingsLoader.InstallUnitDescriptorCache.Descriptors.Values.ToList());
                }

                if (string.IsNullOrWhiteSpace(TemplateName))
                {
                    return EnterMaintenanceFlow();
                }

                return await EnterTemplateManipulationFlowAsync().ConfigureAwait(false);
            }
            catch (TemplateAuthoringException tae)
            {
                ////Reporter.Error.WriteLine(tae.Message.Bold().Red());
                return CreationResultStatus.CreateFailed;
            }
        }

        private TemplateListResolutionResult QueryForTemplateMatches()
        {
            return TemplateListResolver.GetTemplateResolutionResult(_settingsLoader.UserTemplateCache.TemplateInfo, _hostDataLoader, _commandInput, _defaultLanguage);
        }

        private async Task<CreationResultStatus> EnterTemplateManipulationFlowAsync()
        {
            TemplateListResolutionResult templateResolutionResult = QueryForTemplateMatches();

            if (_commandInput.IsListFlagSpecified || _commandInput.IsHelpFlagSpecified)
            {
                return HelpForTemplateResolution.CoordinateHelpAndUsageDisplay(templateResolutionResult, EnvironmentSettings, _commandInput, _hostDataLoader, _telemetryLogger, _templateCreator, _defaultLanguage);
            }

            if (templateResolutionResult.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroup)
                && templateResolutionResult.TryGetSingularInvokableMatch(out ITemplateMatchInfo templateToInvoke)
                && !unambiguousTemplateGroup.Any(x => x.HasParameterMismatch())
                && !unambiguousTemplateGroup.Any(x => x.HasAmbiguousParameterValueMatch()))
            {
                // If any template in the group has any ambiguous params, then don't invoke.
                // The check for HasAmbiguousParameterValueMatch is for an example like:
                // "dotnet new mvc -f netcore"
                //      - '-f netcore' is ambiguous in the 1.x version (2 begins-with matches)
                //      - '-f netcore' is not ambiguous in the 2.x version (1 begins-with match)
                return await EnterTemplateInvocationFlowAsync(templateToInvoke).ConfigureAwait(false);
            }
            else
            {
                return HelpForTemplateResolution.CoordinateHelpAndUsageDisplay(templateResolutionResult, EnvironmentSettings, _commandInput, _hostDataLoader, _telemetryLogger, _templateCreator, _defaultLanguage);
            }
        }

        private bool CheckForArgsError(ITemplateMatchInfo template, out string commandParseFailureMessage)
        {
            bool argsError;

            if (template.HasParseError())
            {
                commandParseFailureMessage = template.GetParseError();
                argsError = true;
            }
            else
            {
                commandParseFailureMessage = null;
                IReadOnlyList<string> invalidParams = template.GetInvalidParameterNames();

                if (invalidParams.Count > 0)
                {
                    HelpForTemplateResolution.DisplayInvalidParameters(invalidParams);
                    argsError = true;
                }
                else
                {
                    argsError = false;
                }
            }

            return argsError;
        }

        private async Task<CreationResultStatus> EnterTemplateInvocationFlowAsync(ITemplateMatchInfo templateToInvoke)
        {
            templateToInvoke.Info.Tags.TryGetValue("language", out ICacheTag language);
            bool isMicrosoftAuthored = string.Equals(templateToInvoke.Info.Author, "Microsoft", StringComparison.OrdinalIgnoreCase);
            string framework = null;
            string auth = null;
            string templateName = TelemetryHelper.HashWithNormalizedCasing(templateToInvoke.Info.Identity);

            if (isMicrosoftAuthored)
            {
                _commandInput.InputTemplateParams.TryGetValue("Framework", out string inputFrameworkValue);
                framework = TelemetryHelper.HashWithNormalizedCasing(TelemetryHelper.GetCanonicalValueForChoiceParamOrDefault(templateToInvoke.Info, "Framework", inputFrameworkValue));

                _commandInput.InputTemplateParams.TryGetValue("auth", out string inputAuthValue);
                auth = TelemetryHelper.HashWithNormalizedCasing(TelemetryHelper.GetCanonicalValueForChoiceParamOrDefault(templateToInvoke.Info, "auth", inputAuthValue));
            }

            bool argsError = CheckForArgsError(templateToInvoke, out string commandParseFailureMessage);
            if (argsError)
            {
                _telemetryLogger.TrackEvent(CommandName + TelemetryConstants.CreateEventSuffix, new Dictionary<string, string>
                {
                    { TelemetryConstants.Language, language?.ChoicesAndDescriptions.Keys.FirstOrDefault() },
                    { TelemetryConstants.ArgError, "True" },
                    { TelemetryConstants.Framework, framework },
                    { TelemetryConstants.TemplateName, templateName },
                    { TelemetryConstants.IsTemplateThirdParty, (!isMicrosoftAuthored).ToString() },
                    { TelemetryConstants.Auth, auth }
                });

                if (commandParseFailureMessage != null)
                {
                  //  Reporter.Error.WriteLine(commandParseFailureMessage.Bold().Red());
                }

                //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, $"{CommandName} {TemplateName}").Bold().Red());
                return CreationResultStatus.InvalidParamValues;
            }
            else
            {
                bool success = true;

                try
                {
                    return await CreateTemplateAsync(templateToInvoke).ConfigureAwait(false);
                }
                catch (ContentGenerationException cx)
                {
                    success = false;
                    //Reporter.Error.WriteLine(cx.Message.Bold().Red());
                    if (cx.InnerException != null)
                    {
                      //  Reporter.Error.WriteLine(cx.InnerException.Message.Bold().Red());
                    }

                    return CreationResultStatus.CreateFailed;
                }
                catch (Exception ex)
                {
                    success = false;
                    //Reporter.Error.WriteLine(ex.Message.Bold().Red());
                }
                finally
                {
                    _telemetryLogger.TrackEvent(CommandName + TelemetryConstants.CreateEventSuffix, new Dictionary<string, string>
                    {
                        { TelemetryConstants.Language, language?.ChoicesAndDescriptions.Keys.FirstOrDefault() },
                        { TelemetryConstants.ArgError, "False" },
                        { TelemetryConstants.Framework, framework },
                        { TelemetryConstants.TemplateName, templateName },
                        { TelemetryConstants.IsTemplateThirdParty, (!isMicrosoftAuthored).ToString() },
                        { TelemetryConstants.CreationResult, success.ToString() },
                        { TelemetryConstants.Auth, auth }
                    });
                }

                return CreationResultStatus.CreateFailed;
            }
        }

        private async Task<CreationResultStatus> CreateTemplateAsync(ITemplateMatchInfo templateMatchDetails)
        {
            ITemplateInfo template = templateMatchDetails.Info;

            string fallbackName = new DirectoryInfo(_commandInput.OutputPath ?? Directory.GetCurrentDirectory()).Name;

            if (string.IsNullOrEmpty(fallbackName) || string.Equals(fallbackName, "/", StringComparison.Ordinal))
            {   // DirectoryInfo("/").Name on *nix returns "/", as opposed to null or "".
                fallbackName = null;
            }

            TemplateCreationResult instantiateResult;

            try
            {
                instantiateResult = await _templateCreator.InstantiateAsync(template, _commandInput.Name, fallbackName, _commandInput.OutputPath, templateMatchDetails.GetValidTemplateParameters(), _commandInput.SkipUpdateCheck, _commandInput.IsForceFlagSpecified, _commandInput.BaselineName).ConfigureAwait(false);
            }
            catch (ContentGenerationException cx)
            {
                //Reporter.Error.WriteLine(cx.Message.Bold().Red());
                if (cx.InnerException != null)
                {
                  //  Reporter.Error.WriteLine(cx.InnerException.Message.Bold().Red());
                }

                return CreationResultStatus.CreateFailed;
            }
            catch (TemplateAuthoringException tae)
            {
                //Reporter.Error.WriteLine(tae.Message.Bold().Red());
                return CreationResultStatus.CreateFailed;
            }

            string resultTemplateName = string.IsNullOrEmpty(instantiateResult.TemplateFullName) ? TemplateName : instantiateResult.TemplateFullName;

            switch (instantiateResult.Status)
            {
                case CreationResultStatus.Success:
                    //Reporter.Output.WriteLine(string.Format(LocalizableStrings.CreateSuccessful, resultTemplateName));

                    if (!string.IsNullOrEmpty(template.ThirdPartyNotices))
                    {
                      //  Reporter.Output.WriteLine(string.Format(LocalizableStrings.ThirdPartyNotices, template.ThirdPartyNotices));
                    }

                    //HandlePostActions(instantiateResult);
                    break;
                case CreationResultStatus.CreateFailed:
                    //Reporter.Error.WriteLine(string.Format(LocalizableStrings.CreateFailed, resultTemplateName, instantiateResult.Message).Bold().Red());
                    break;
                case CreationResultStatus.MissingMandatoryParam:
                    if (string.Equals(instantiateResult.Message, "--name", StringComparison.Ordinal))
                    {
                        //Reporter.Error.WriteLine(string.Format(LocalizableStrings.MissingRequiredParameter, instantiateResult.Message, resultTemplateName).Bold().Red());
                    }
                    else
                    {
                        // TODO: rework to avoid having to reparse.
                        // The canonical info could be in the ITemplateMatchInfo, but currently isn't.
                        TemplateListResolver.ParseTemplateArgs(template, _hostDataLoader, _commandInput);

                        IReadOnlyList<string> missingParamNamesCanonical = instantiateResult.Message.Split(new[] { ',' })
                            .Select(x => _commandInput.VariantsForCanonical(x.Trim())
                                                        .DefaultIfEmpty(x.Trim()).First())
                            .ToList();
                        string fixedMessage = string.Join(", ", missingParamNamesCanonical);
                        //Reporter.Error.WriteLine(string.Format(LocalizableStrings.MissingRequiredParameter, fixedMessage, resultTemplateName).Bold().Red());
                    }
                    break;
                case CreationResultStatus.OperationNotSpecified:
                    break;
                case CreationResultStatus.InvalidParamValues:
                    //TemplateUsageInformation usageInformation = TemplateUsageHelp.GetTemplateUsageInformation(template, EnvironmentSettings, _commandInput, _hostDataLoader, _templateCreator);
                    //string invalidParamsError = InvalidParameterInfo.InvalidParameterListToString(usageInformation.InvalidParameters);
                    //Reporter.Error.WriteLine(invalidParamsError.Bold().Red());
                    //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, $"{CommandName} {TemplateName}").Bold().Red());
                    break;
                default:
                    break;
            }

            return instantiateResult.Status;
        }


        private CreationResultStatus EnterMaintenanceFlow()
        {
            if (!TemplateListResolver.ValidateRemainingParameters(_commandInput, out IReadOnlyList<string> invalidParams))
            {
                HelpForTemplateResolution.DisplayInvalidParameters(invalidParams);
                if (_commandInput.IsHelpFlagSpecified)
                {
                    _telemetryLogger.TrackEvent(CommandName + "-Help");
                    HelpForTemplateResolution.ShowUsageHelp(_commandInput);
                }
                else
                {
                    //Reporter.Error.WriteLine(string.Format(LocalizableStrings.RunHelpForInformationAboutAcceptedParameters, CommandName).Bold().Red());
                }

                return CreationResultStatus.InvalidParamValues;
            }

            if (_commandInput.ToInstallList != null && _commandInput.ToInstallList.Count > 0 && _commandInput.ToInstallList[0] != null)
            {
                Installer.InstallPackages(_commandInput.ToInstallList.Select(x => x.Split(new[] { "::" }, StringSplitOptions.None)[0]));
            }

            if (_commandInput.ToUninstallList != null)
            {
                if (_commandInput.ToUninstallList.Count > 0 && _commandInput.ToUninstallList[0] != null)
                {
                    IEnumerable<string> failures = Installer.Uninstall(_commandInput.ToUninstallList);

                    foreach (string failure in failures)
                    {
                        Console.WriteLine(LocalizableStrings.CouldntUninstall, failure);
                    }
                }
                else
                {
                    Console.WriteLine(LocalizableStrings.CommandDescription);
                    Console.WriteLine();
                    Console.WriteLine(LocalizableStrings.InstalledItems);

                    foreach (string value in _settingsLoader.InstallUnitDescriptorCache.InstalledItems.Values)
                    {
                        Console.WriteLine($" {value}");
                    }

                    return CreationResultStatus.Success;
                }
            }

            if (_commandInput.ToInstallList != null && _commandInput.ToInstallList.Count > 0 && _commandInput.ToInstallList[0] != null)
            {
                CreationResultStatus installResult = EnterInstallFlow();

                if (installResult == CreationResultStatus.Success)
                {
                    _settingsLoader.Reload();
                    TemplateListResolutionResult resolutionResult = QueryForTemplateMatches();
                    HelpForTemplateResolution.CoordinateHelpAndUsageDisplay(resolutionResult, EnvironmentSettings, _commandInput, _hostDataLoader, _telemetryLogger, _templateCreator, _defaultLanguage);
                }

                return installResult;
            }

            //No other cases specified, we've fallen through to "Usage help + List"
            HelpForTemplateResolution.ShowUsageHelp(_commandInput);
            TemplateListResolutionResult templateResolutionResult = QueryForTemplateMatches();

            HelpForTemplateResolution.CoordinateHelpAndUsageDisplay(templateResolutionResult, EnvironmentSettings, _commandInput, _hostDataLoader, _telemetryLogger, _templateCreator, _defaultLanguage);

            return CreationResultStatus.Success;
        }

        private CreationResultStatus EnterInstallFlow()
        {
            _telemetryLogger.TrackEvent(CommandName + TelemetryConstants.InstallEventSuffix, new Dictionary<string, string> { { TelemetryConstants.ToInstallCount, _commandInput.ToInstallList.Count.ToString() } });

            Installer.InstallPackages(_commandInput.ToInstallList);

            //TODO: When an installer that directly calls into NuGet is available,
            //  return a more accurate representation of the outcome of the operation
            return CreationResultStatus.Success;
        }

        private HashSet<string> AllTemplateShortNames
        {
            get
            {
                IReadOnlyCollection<ITemplateMatchInfo> allTemplates = TemplateListResolver.PerformAllTemplatesQuery(_settingsLoader.UserTemplateCache.TemplateInfo, _hostDataLoader);
                HashSet<string> allShortNames = new HashSet<string>(allTemplates.Select(x => x.Info.ShortName));
                return allShortNames;
            }
        }

        public string OutputPath => _commandInput.OutputPath;

        public string TemplateName => _commandInput.TemplateName;

        public string CommandName { get; }

        public static IInstaller Installer { get; set; }

        public EngineEnvironmentSettings EnvironmentSettings { get; private set; }

        private static bool ValidateLocaleFormat(string localeToCheck)
        {
            return LocaleFormatRegex.IsMatch(localeToCheck);
        }
    }
}
