using AsyncRpc;
using AsyncRpc.Routing;
using AsyncRpc.Transport.Tcp;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Languages.CSharp.OmniSharp;
using AvalonStudio.MSBuildHost;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis.Host.Mef;
using RoslynPad.Roslyn;
using System;
using System.Collections.ObjectModel;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpSolution : ISolution
    {
        private CompositionHost _compositionContext;
        private MefHostServices _host;

        public static async Task<OmniSharpSolution> Create(string path)
        {
            OmniSharpSolution result = new OmniSharpSolution();

            await result.LoadSolution(path);

            return result;
        }

        public NuGetConfiguration NuGetConfiguration { get; }

        public RoslynWorkspace Workspace { get; private set; }

        private OmniSharpServer server;

        private OmniSharpSolution()
        {
            server = new OmniSharpServer(TcpUtils.FreeTcpPort());
            Projects = new ObservableCollection<IProject>();
        }

        private async Task LoadSolution(string path)
        {
            var currentDir = AvalonStudio.Platforms.Platform.ExecutionPath;

            var assemblies = new[]
            {
                Assembly.LoadFrom(Path.Combine(currentDir, "Roslyn", "Microsoft.CodeAnalysis.dll")),
                Assembly.LoadFrom(Path.Combine(currentDir, "Roslyn", "Microsoft.CodeAnalysis.CSharp.dll")),
                Assembly.LoadFrom(Path.Combine(currentDir, "Roslyn", "Microsoft.CodeAnalysis.Features.dll")),
                Assembly.LoadFrom(Path.Combine(currentDir, "Roslyn", "Microsoft.CodeAnalysis.CSharp.Features.dll")),
            };

            var partTypes = MefHostServices.DefaultAssemblies.Concat(assemblies)
                    .Distinct()
                    .SelectMany(x => x.GetTypes())
                    //.Concat(new[] { typeof(DocumentationProviderServiceFactory) })
                    .ToArray();

            _compositionContext = new ContainerConfiguration()
                .WithParts(partTypes)
                .CreateContainer();

            _host = MefHostServices.Create(_compositionContext);

            Workspace = new RoslynWorkspace(_host, NuGetConfiguration);

            var roslynProject = await Workspace.AddProject(path);

            Location = path;

            Name = Path.GetFileNameWithoutExtension(path);

            var project = OmniSharpProject.Create(roslynProject, this, path);

            AddProject(project);

            project.LoadFiles();

            CurrentDirectory = Path.GetDirectoryName(path);
        }

        public IProject AddProject(IProject project)
        {
            var currentProject = Projects.FirstOrDefault(p => p.Name == project.Name);

            if (currentProject != null) return currentProject;

            //ProjectReferences.Add(CurrentDirectory.MakeRelativePath(project.Location));
            Projects.InsertSorted(project);
            currentProject = project;

            project.FileAdded += Project_FileAdded;

            return currentProject;
        }

        private void Project_FileAdded(object sender, ISourceFile e)
        {
            switch (e.Extension)
            {
                case ".cs":
                    Workspace.AddDocument((sender as OmniSharpProject).RoslynProject, e);
                    break;
            }
        }

        public OmniSharpServer Server => server;

        public string CurrentDirectory { get; set; }

        public string Name { get; set; }

        public ObservableCollection<IProject> Projects { get; set; }

        public IProject StartupProject { get; set; }

        public string Location { get; private set; }

        public ISourceFile FindFile(string file)
        {
            ISourceFile result = null;

            foreach (var project in Projects)
            {
                result = project.FindFile(file);

                if (result != null)
                {
                    break;
                }
            }

            return result;
        }

        public void RemoveProject(IProject project)
        {
            project.FileAdded -= Project_FileAdded;
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }
    }
}