using Avalonia.Ide.LanguageServer.MSBuild;
using Avalonia.Ide.LanguageServer.MSBuild.Requests;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Process = System.Diagnostics.Process;

namespace AvalonStudio.Projects.OmniSharp.MSBuild
{
    public class MSBuildHost : IDisposable
    {
        private WireHelper _connection;
        private TcpClient _client;
        private object _lock = new object();
        private List<string> outputLines = new List<string>();
        private List<string> errorLines = new List<string>();
        private Process hostProcess;
        private string _sdkPath;
        private AutoResetEvent requestComplete = new AutoResetEvent(false);
        private int _id;

        public MSBuildHost(string sdkPath, int id = -1)
        {
            _sdkPath = sdkPath;

            _id = id;
        }

        public int Id => _id;

        public void Dispose()
        {
            _client.Dispose();

            hostProcess?.Kill();
        }

        public Task EnsureConnectionAsync()
        {
            return Task.Run(() =>
            {
                if (_connection == null || !_client.Connected)
                {
                    _client?.Dispose();

                    using (var l = new OneShotTcpServer())
                    {
                        var path = typeof(NextRequestType).Assembly.GetModules()[0].FullyQualifiedName;
                        path = Path.Combine(Path.GetDirectoryName(path), "host.csproj");

                        string args = $"msbuild /m:1 /p:AvaloniaIdePort={l.Port} \"{path}\"";                        

                        hostProcess = PlatformSupport.LaunchShellCommand("dotnet", args,
                        (sender, e) =>
                        {
                            if (e.Data != null)
                            {
                                lock (outputLines)
                                {
                                    outputLines.Add(e.Data);
                                }

                                if (e.Data == "*** Request Handled")
                                {
                                    requestComplete.Set();
                                }
                            }
                        },
                        (sender, e) =>
                        {
                            if (e.Data != null)
                            {
                                lock (errorLines)
                                {
                                    errorLines.Add(e.Data);
                                }
                            }
                        }, false, Platforms.Platform.ExecutionPath, false);

                        _client = l.WaitForOneConnection();
                        _connection = new WireHelper(_client.GetStream());
                    }
                }
            });
        }

        public TRes SendRequest<TRes>(RequestBase<TRes> req)
        {
            lock (_lock)
            {
                _connection.SendRequest(req);
                var e = _connection.Read<ResponseEnvelope<TRes>>();
                if (e.Exception != null)
                    throw new TargetInvocationException(e.Exception, null);
                return e.Response;
            }
        }

        private static List<string> GetTargetFrameworks(XDocument project)
        {
            var targetFramework = project.Descendants("TargetFramework").FirstOrDefault();
            var targetFrameworks = project.Descendants("TargetFrameworks");

            var result = new List<string>();

            if (targetFramework != null)
            {
                result.Add(targetFramework.Value);
            }

            if (targetFrameworks != null)
            {
                foreach (var framework in targetFrameworks)
                {
                    var frameworks = framework.Value.Split(';');

                    foreach (var value in frameworks)
                    {
                        result.Add(value);
                    }
                }
            }

            return result;
        }

        public Task<(bool result, List<string> outputAssemblies, string consoleOutput)> BuildProject(IProject project)
        {
            lock (outputLines)
            {
                outputLines.Clear();
                errorLines.Clear();
            }

            return Task.Run(() =>
            {
                var xproject = XDocument.Load(project.Location);

                var frameworks = GetTargetFrameworks(xproject);

                var targetFramework = frameworks.FirstOrDefault();

                if (targetFramework != null)
                {
                    Console.WriteLine($"Automatically selecting {targetFramework} as TargetFramework");
                }
                else
                {
                    //throw new Exception("Must specify target framework to load project.");
                    Console.WriteLine($"Non-Dotnet core project trying anyway.");
                    targetFramework = "";
                }

                var output = SendRequest(new BuildProjectRequest { SolutionDirectory = project.Solution.CurrentDirectory, FullPath = project.Location, TargetFramework = targetFramework });

                requestComplete.WaitOne();

                var builder = new StringBuilder();

                lock (outputLines)
                {
                    foreach (var line in outputLines.Take(outputLines.Count - 1))
                    {
                        if (Regex.IsMatch(line, ": (warning|error)"))
                        {
                            builder.AppendLine(line);
                        }
                    }

                    outputLines.Clear();
                    errorLines.Clear();
                }

                return (output.Success, output.OutputAssemblies, builder.ToString());
            });
        }

        public async Task<(ProjectInfo info, List<string> projectReferences, string targetPath)> LoadProject(string solutionDirectory, string projectFile, ProjectId id = null)
        {
            lock (outputLines)
            {
                outputLines.Clear();
                errorLines.Clear();
            }

            return await Task.Run(() =>
            {
                var project = XDocument.Load(projectFile);

                var projectReferences = project.Descendants("ProjectReference").Select(e => e.Attribute("Include").Value).ToList();

                var frameworks = GetTargetFrameworks(project);

                var targetFramework = frameworks.FirstOrDefault();

                if (targetFramework != null)
                {
                    Console.WriteLine($"Automatically selecting {targetFramework} as TargetFramework");
                }
                else
                {
                    //throw new Exception("Must specify target framework to load project.");
                    Console.WriteLine($"Non-Dotnet core project trying anyway.");
                    targetFramework = "";
                }

                ProjectInfoResponse loadData = null;

                try
                {
                    loadData = SendRequest(new ProjectInfoRequest { SolutionDirectory = solutionDirectory, FullPath = projectFile, TargetFramework = targetFramework });
                }
                catch (Exception)
                {
                    return (null, null, null);
                }

                requestComplete.WaitOne();

                if (loadData.CscCommandLine != null && loadData.CscCommandLine.Count > 0)
                {
                    var projectOptions = ParseArguments(loadData.CscCommandLine.Skip(1));
                    
                    var projectInfo = ProjectInfo.Create(
                        id ?? ProjectId.CreateNewId(),
                        VersionStamp.Create(),
                        name: Path.GetFileNameWithoutExtension(projectFile),
                        assemblyName: Path.GetFileNameWithoutExtension(projectOptions.outputFile),
                        language: LanguageNames.CSharp,
                        filePath: projectFile,
                        outputFilePath: projectOptions.outputFile,
                        compilationOptions: projectOptions.compilationOptions,
                        parseOptions: projectOptions.parseOptions,
                        metadataReferences: loadData.MetaDataReferences.Where(ar => File.Exists(ar)).Select(ar => MetadataReference.CreateFromFile(ar, documentation: IoC.Get<AvalonStudio.Projects.OmniSharp.Roslyn.DocumentationProvider>()?.GetDocumentationProvider(ar))));

                    return (projectInfo, projectReferences, loadData.TargetPath);
                }
                else
                {
                    IoC.Get<IConsole>($"Project may have failed to load correctly: {Path.GetFileNameWithoutExtension(projectFile)}");

                    var projectInfo = ProjectInfo.Create(
                        id ?? ProjectId.CreateNewId(),
                        VersionStamp.Create(),
                        Path.GetFileNameWithoutExtension(projectFile), Path.GetFileNameWithoutExtension(projectFile),
                        LanguageNames.CSharp,
                        projectFile);

                    return (projectInfo, projectReferences, loadData?.TargetPath);
                }
            });
        }

        private static (string outputFile, CSharpCompilationOptions compilationOptions, CSharpParseOptions parseOptions) ParseArguments(IEnumerable<string> args)
        {
            var arguments = new Dictionary<string, string>();

            var compilationOptions = new CSharpCompilationOptions(OutputKind.ConsoleApplication);
            compilationOptions = compilationOptions.WithAssemblyIdentityComparer(DesktopAssemblyIdentityComparer.Default);
            string outputFile = "";

            var specificDiagnosticOptions = new Dictionary<string, ReportDiagnostic>()
            {
                // Ensure that specific warnings about assembly references are always suppressed.
                { "CS1701", ReportDiagnostic.Suppress },
                { "CS1702", ReportDiagnostic.Suppress },
                { "CS1705", ReportDiagnostic.Suppress }
            };

            compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(specificDiagnosticOptions);

            var parseOptions = new CSharpParseOptions(LanguageVersion.Latest);

            foreach (var arg in args)
            {
                var argParts = arg.Split(':');

                var argument = argParts[0].Replace("+", "").Replace("/", "");
                var value = "";

                if (argParts.Count() > 1)
                {
                    value = argParts[1];
                }

                switch (argument)
                {
                    case "target":
                        compilationOptions = compilationOptions.WithOutputKind(ParseTarget(value));
                        break;

                    case "unsafe":
                        compilationOptions = compilationOptions.WithAllowUnsafe(true);
                        break;

                    case "nowarn":
                        var warnings = value.Split(',');

                        if (warnings.Count() > 0)
                        {
                            compilationOptions = compilationOptions.WithSpecificDiagnosticOptions(warnings.Select(s => new KeyValuePair<string, ReportDiagnostic>(!s.StartsWith("CS") ? $"CS{s}" : s, ReportDiagnostic.Suppress)));
                        }
                        break;

                    case "define":
                        var defines = value.Split(';');

                        if (defines.Count() > 0)
                        {
                            parseOptions = parseOptions.WithPreprocessorSymbols(defines);
                        }
                        break;

                    case "optimize":
                        compilationOptions = compilationOptions.WithOptimizationLevel(OptimizationLevel.Release);
                        break;

                    case "platform":
                        compilationOptions = compilationOptions.WithPlatform(ParsePlatform(value));
                        break;

                    case "out":
                        outputFile = value;
                        break;

                }
            }

            return (outputFile, compilationOptions, parseOptions);
        }

        private static OutputKind ParseTarget(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "exe":
                    return OutputKind.ConsoleApplication;

                case "winexe":
                    return OutputKind.WindowsApplication;

                case "library":
                    return OutputKind.DynamicallyLinkedLibrary;

                case "module":
                    return OutputKind.NetModule;

                case "appcontainerexe":
                    return OutputKind.WindowsRuntimeApplication;

                case "winmdobj":
                    return OutputKind.WindowsRuntimeMetadata;

                default:
                    throw new Exception($"{value} is not a valid OutputKind");
            }
        }

        private static Platform ParsePlatform(string value)
        {
            switch (value.ToLowerInvariant())
            {
                case "x86":
                    return Platform.X86;
                case "x64":
                    return Platform.X64;
                case "itanium":
                    return Platform.Itanium;
                case "anycpu":
                    return Platform.AnyCpu;
                case "anycpu32bitpreferred":
                    return Platform.AnyCpu32BitPreferred;
                case "arm":
                    return Platform.Arm;
                default:
                    return Platform.AnyCpu;
            }
        }
    }
}
