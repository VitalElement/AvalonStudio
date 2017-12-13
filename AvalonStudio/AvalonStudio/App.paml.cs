using Avalonia;
using Avalonia.Controls;
using Avalonia.Diagnostics;
using Avalonia.Logging.Serilog;
using Avalonia.Markup.Xaml;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility.Projects;
using AvalonStudio.Platforms;
using AvalonStudio.Repositories;
using Microsoft.TemplateEngine.Cli;
using Microsoft.TemplateEngine.Edge;
using Microsoft.TemplateEngine.Edge.TemplateUpdates;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects;
using Microsoft.TemplateEngine.Orchestrator.RunnableProjects.Config;
using Microsoft.TemplateEngine.Utils;
using Serilog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace AvalonStudio
{
    internal class App : Application
    {
        private const string HostIdentifier = "dotnetcli-preview";
        private const string HostVersion = "1.0.0";
        private const string CommandName = "new3";

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

        [STAThread]
        private static void Main(string[] args)
        {
            DefaultTemplateEngineHost host = CreateHost(false);            

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            var builder = AppBuilder.Configure<App>().UseReactiveUI().AvalonStudioPlatformDetect().AfterSetup(_ =>
            {
                Platform.Initialise();

                PackageSources.InitialisePackageSources();

                var container = CompositionRoot.CreateContainer();

                ShellViewModel.Instance = container.GetExport<ShellViewModel>();
            });

            InitializeLogging();

            builder.Start<MainWindow>();
        }

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>().UsePlatformDetect().UseReactiveUI();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);

           // DataTemplates.Add(new ViewLocatorDataTemplate());
        }

        private static void InitializeLogging()
        {
#if DEBUG
            SerilogLogger.Initialize(new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Trace(outputTemplate: "{Area}: {Message}")
                .CreateLogger());
#endif
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }
    }

    public static class AppBuilderExtensions
    {
        public static AppBuilder AvalonStudioPlatformDetect(this AppBuilder builder)
        {
            switch (Platform.PlatformIdentifier)
            {
                case Platforms.PlatformID.Win32NT:
                    return builder.UseWin32().UseSkia();

                case Platforms.PlatformID.Unix:
                    return builder.UseGtk3().UseSkia();

                default:
                    return builder.UsePlatformDetect();
            }
        }
    }
}