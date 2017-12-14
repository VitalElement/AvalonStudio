using System;
using System.Collections.Generic;
using Microsoft.TemplateEngine.Abstractions;
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
using System.Globalization;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Config;
using Microsoft.TemplateEngine.Edge.TemplateUpdates;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.Extensibility.Templating
{
    public class TemplateManager : IExtension
    {
        private const string HostIdentifier = "AvalonStudio";
        private const string HostVersion = "1.0.0";
        private const string CommandNameString = "new3";

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

        public TemplateManager()
        {
            var host = new ExtendedTemplateEngineHost(CreateHost(false), this);
            EnvironmentSettings = new EngineEnvironmentSettings(host, x => new SettingsLoader(x), null);
            _settingsLoader = (SettingsLoader)EnvironmentSettings.SettingsLoader;
            Installer = new Installer(EnvironmentSettings);
            _templateCreator = new TemplateCreator(EnvironmentSettings);
            _aliasRegistry = new AliasRegistry(EnvironmentSettings);
            CommandName = CommandNameString;
            _paths = new Paths(EnvironmentSettings);
            _onFirstRun = FirstRun;
            _hostDataLoader = new HostSpecificDataLoader(EnvironmentSettings.SettingsLoader);
            _commandInput = new NewCommandInputCli(CommandNameString);

            if (!EnvironmentSettings.Host.TryGetHostParamDefault("prefs:language", out _defaultLanguage))
            {
                _defaultLanguage = null;
            }
        }

        public void Initialise(bool forceCacheRebuild = false)
        {
            if (!ConfigureLocale())
            {
                throw new Exception("TemplatingEngine: Unable to configure Locale");
            }

            if (!Initialize())
            {
                throw new Exception("TemplatingEngine: Unable to initialise");
            }

            try
            {
                _settingsLoader.RebuildCacheFromSettingsIfNotCurrent(forceCacheRebuild);
            }
            catch (EngineInitializationException eiex)
            {
                ////Reporter.Error.WriteLine(eiex.Message.Bold().Red());
                ////Reporter.Error.WriteLine(LocalizableStrings.SettingsReadError);
                throw new Exception("TemplateingEnging: Unable to rebuild cache.");
            }
        }

        public IReadOnlyList<ITemplate> ListProjectTemplates(string language)
        {
            _commandInput.ResetArgs("--list", "--language", language, "--type", "project");

            return ListTemplates(language, TemplateKind.Project);
        }

        public async Task<CreationResultStatus> CreateTemplate(ITemplate template, string path, string name = "")
        {
            if (template is DotNetTemplateAdaptor templateImpl)
            {
                if (string.IsNullOrEmpty(name))
                {
                    _commandInput.ResetArgs(templateImpl.DotnetTemplate.Info.ShortName, "--output", path);
                }
                else
                {
                    _commandInput.ResetArgs(templateImpl.DotnetTemplate.Info.ShortName, "--output", path, "--name", name);
                }

                string fallbackName = new DirectoryInfo(_commandInput.OutputPath ?? Directory.GetCurrentDirectory()).Name;

                var result = await _templateCreator.InstantiateAsync(templateImpl.DotnetTemplate.Info, _commandInput.Name, fallbackName, _commandInput.OutputPath, templateImpl.DotnetTemplate.GetValidTemplateParameters(), _commandInput.SkipUpdateCheck, _commandInput.IsForceFlagSpecified, _commandInput.BaselineName).ConfigureAwait(false);

                return result.Status;

            }

            return CreationResultStatus.NotFound;
        }

        public IReadOnlyList<ITemplate> ListItemTemplates(string language)
        {
            _commandInput.ResetArgs("--list", "--language", language, "--type", "item");

            return ListTemplates(language, TemplateKind.Item);
        }

        public void InstallTemplates(params string[] paths)
        {
            /*var args = paths.Prepend("--install");
            _commandInput.ResetArgs(args.ToArray());*/

            Installer.InstallPackages(paths);
        }

        private IReadOnlyList<ITemplate> ListTemplates(string language, TemplateKind kind)
        {
            var templateList = TemplateListResolver.GetTemplateResolutionResult(_settingsLoader.UserTemplateCache.TemplateInfo, _hostDataLoader, _commandInput, _defaultLanguage);

            if (templateList.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroupForDetailDisplay))
            {
                return unambiguousTemplateGroupForDetailDisplay.Where(t => t.IsMatch).Select(ti => new DotNetTemplateAdaptor(ti, kind)).ToList().AsReadOnly();
            }

            var ambiguous = templateList.GetBestTemplateMatchList(true);

            var groups = HelpForTemplateResolution.GetLanguagesForTemplateInfoGroups(ambiguous, language, "C#");

            return groups.Keys.Where(t => t.IsMatch).Select(ti => new DotNetTemplateAdaptor(ti, kind)).ToList().AsReadOnly();
        }

        private bool ConfigureLocale()
        {
            string newLocale = CultureInfo.CurrentCulture.IetfLanguageTag;

            if (!ValidateLocaleFormat(newLocale))
            {
                ////Reporter.Error.WriteLine(string.Format(LocalizableStrings.BadLocaleError, newLocale).Bold().Red());
                return false;
            }

            EnvironmentSettings.Host.UpdateLocale(newLocale);
            // cache the templates for the new locale
            _settingsLoader.Reload();

            return true;
        }

        private bool Initialize()
        {
            if (!_paths.Exists(_paths.User.BaseDir) || !_paths.Exists(_paths.User.FirstRunCookie))
            {
                if (!_commandInput.IsQuietFlagSpecified)
                {
                    ////Reporter.Output.WriteLine(LocalizableStrings.GettingReady);
                }

                ConfigureEnvironment();
                _paths.WriteAllText(_paths.User.FirstRunCookie, "");
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

        private TemplateListResolutionResult QueryForTemplateMatches()
        {
            return TemplateListResolver.GetTemplateResolutionResult(_settingsLoader.UserTemplateCache.TemplateInfo, _hostDataLoader, _commandInput, _defaultLanguage);
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

        private HashSet<string> AllTemplateShortNames
        {
            get
            {
                IReadOnlyCollection<ITemplateMatchInfo> allTemplates = TemplateListResolver.PerformAllTemplatesQuery(_settingsLoader.UserTemplateCache.TemplateInfo, _hostDataLoader);
                HashSet<string> allShortNames = new HashSet<string>(allTemplates.Select(x => x.Info.ShortName));
                return allShortNames;
            }
        }

        internal string OutputPath => _commandInput.OutputPath;

        internal string TemplateName => _commandInput.TemplateName;

        internal string CommandName { get; }

        internal static IInstaller Installer { get; set; }

        internal EngineEnvironmentSettings EnvironmentSettings { get; private set; }

        private static bool ValidateLocaleFormat(string localeToCheck)
        {
            return LocaleFormatRegex.IsMatch(localeToCheck);
        }

        private static DefaultTemplateEngineHost CreateHost(bool emitTimings)
        {
            var preferences = new Dictionary<string, string>
            {
                { "prefs:language", "C#" }
            };

            try
            {
                string versionString = Dotnet.Version().CaptureStdOut().Execute().StdOut;
                if (!string.IsNullOrWhiteSpace(versionString))
                {
                    preferences["dotnet-cli-version"] = versionString.Trim();
                }
            }
            catch
            { }

            var builtIns = new AssemblyComponentCatalog(new[]
            {
                typeof(RunnableProjectGenerator).GetTypeInfo().Assembly,
                typeof(ConditionalConfig).GetTypeInfo().Assembly,
                typeof(NupkgInstallUnitDescriptorFactory).GetTypeInfo().Assembly
            });

            DefaultTemplateEngineHost host = new DefaultTemplateEngineHost(HostIdentifier, HostVersion, CultureInfo.CurrentCulture.Name, preferences, builtIns, new[] { "dotnetcli" });

            if (emitTimings)
            {
                host.OnLogTiming = (label, duration, depth) =>
                {
                    string indent = string.Join("", Enumerable.Repeat("  ", depth));
                    Console.WriteLine($"{indent} {label} {duration.TotalMilliseconds}");
                };
            }


            return host;
        }

        private static void FirstRun(IEngineEnvironmentSettings environmentSettings, IInstaller installer)
        {
            string baseDir = Environment.ExpandEnvironmentVariables("%DN3%");

            if (baseDir.Contains('%'))
            {
                Assembly a = typeof(TemplateManager).GetTypeInfo().Assembly;
                string path = new Uri(a.CodeBase, UriKind.Absolute).LocalPath;
                path = Path.GetDirectoryName(path);
                Environment.SetEnvironmentVariable("DN3", path);
            }

            List<string> toInstallList = new List<string>();
            Paths paths = new Paths(environmentSettings);

            if (paths.FileExists(paths.Global.DefaultInstallPackageList))
            {
                toInstallList.AddRange(paths.ReadAllText(paths.Global.DefaultInstallPackageList).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
            }

            if (paths.FileExists(paths.Global.DefaultInstallTemplateList))
            {
                toInstallList.AddRange(paths.ReadAllText(paths.Global.DefaultInstallTemplateList).Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries));
            }

            if (toInstallList.Count > 0)
            {
                for (int i = 0; i < toInstallList.Count; i++)
                {
                    toInstallList[i] = toInstallList[i].Replace("\r", "")
                                                        .Replace('\\', Path.DirectorySeparatorChar);
                }

                installer.InstallPackages(toInstallList);
            }
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this);
        }

        public void Activation()
        {
            
        }
    }
}
