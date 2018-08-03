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
using System.Composition;

namespace AvalonStudio.Extensibility.Templating
{
    public enum CreationResult
    {
        Success = 0,
        CreateFailed = unchecked((int)0x80020009),
        MissingMandatoryParam = unchecked((int)0x8002000F),
        InvalidParamValues = unchecked((int)0x80020005),
        OperationNotSpecified = unchecked((int)0x8002000E),
        NotFound = unchecked((int)0x800200006),
        Cancelled = unchecked((int)0x80004004)
    }

    [Export(typeof(TemplateManager))]
    [Shared]
    public class TemplateManager : IActivatableExtension
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

            _commandInput.ResetArgs();

            Initialise();
        }

        private void Initialise(bool forceCacheRebuild = false)
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
            catch (EngineInitializationException)
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

        public async Task<CreationResult> CreateTemplate(ITemplate template, string path, params (string name, string value)[] parameters )
        {
            if (template is DotNetTemplateAdaptor templateImpl)
            {
                var parameterList = new List<string>
                {
                    templateImpl.DotnetTemplate.ShortName,
                    "--output",
                    path
                };

                foreach (var (name, value) in parameters)
                {
                    parameterList.Add($"--{name}");
                    parameterList.Add(value);
                }

                _commandInput.ResetArgs(parameterList.ToArray());                

                string fallbackName = new DirectoryInfo(_commandInput.OutputPath ?? Directory.GetCurrentDirectory()).Name;

                var result = await _templateCreator.InstantiateAsync(templateImpl.DotnetTemplate, _commandInput.Name, fallbackName, _commandInput.OutputPath, parameters.ToDictionary(x => x.name, x=> x.value), _commandInput.SkipUpdateCheck, _commandInput.IsForceFlagSpecified, _commandInput.BaselineName).ConfigureAwait(false);
                return (CreationResult)result.Status;
            }

            return CreationResult.NotFound;
        }

        public IReadOnlyList<ITemplate> ListItemTemplates(string language)
        {
            if (!string.IsNullOrEmpty(language))
            {
                _commandInput.ResetArgs("--list", "--language", language, "--type", "item");
            }
            else
            {
                _commandInput.ResetArgs("--list", "--type", "item");
            }

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

            if (templateList.TryGetUnambiguousTemplateGroupToUse(out IReadOnlyList<ITemplateMatchInfo> unambiguousTemplateGroupForDetailDisplay, true))
            {
                return unambiguousTemplateGroupForDetailDisplay.Where(t => t.IsMatch).Select(ti => new DotNetTemplateAdaptor(ti.Info)).ToList().AsReadOnly();
            }

            var ambiguous = templateList.GetBestTemplateMatchList(true);

            var groups = HelpForTemplateResolution.GetLanguagesForTemplateInfoGroups(ambiguous, language, "C#");

            return groups.Keys.Where(t => t.IsMatch).Select(ti => new DotNetTemplateAdaptor(ti.Info)).ToList().AsReadOnly();
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
                ConfigureEnvironment();

                if (_settingsLoader.UserTemplateCache.TemplateInfo.Count > 0)
                {
                    _paths.WriteAllText(_paths.User.FirstRunCookie, "");
                }
            }

            return true;
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
            UpdatePackages(installer);
        }

        public void BeforeActivation()
        {            
        }

        public void Activation()
        {
        }

        public void UpdateDefaultTemplates()
        {
            UpdatePackages(Installer);
        }

        private static void UpdatePackages (IInstaller installer)
        {
            var packages = new List<string> { "VitalElement.AvalonStudio.Templates" };

            installer.InstallPackages(packages);
        }

        public IDictionary<string, IEnumerable<ITemplate>> GetProjectTemplates() => GetTemplates(TemplateKind.Project);
        public IDictionary<string, IEnumerable<ITemplate>> GetItemTemplates() => GetTemplates(TemplateKind.Item);

        private IDictionary<string, IEnumerable<ITemplate>> GetTemplates(TemplateKind kind)
        {
            var templates = new HashSet<ITemplateInfo>();
            _settingsLoader.GetTemplates(templates);

            return templates.Where(t => t.GetTemplateKind() == kind)
                .Select(t => new DotNetTemplateAdaptor(t))
                .GroupBy(t => t.Language)
                .ToDictionary(t => t.Key, t => t.AsEnumerable<ITemplate>());
        }
    }
}
