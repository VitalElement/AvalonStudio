using AsyncRpc;
using AsyncRpc.Routing;
using AsyncRpc.Transport.Tcp;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Languages.CSharp.OmniSharp;
using AvalonStudio.MSBuildHost;
using AvalonStudio.Packages;
using AvalonStudio.Platforms;
using AvalonStudio.Projects.OmniSharp.DotnetCli;
using AvalonStudio.Utils;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.Host.Mef;
using RoslynPad.Roslyn;
using RoslynPad.Roslyn.Diagnostics;
using System;
using System.Collections.Generic;
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
        public int CompareTo(ISolutionItem other)
        {
            return Name.CompareTo(other.Name);
        }

        public ObservableCollection<ISolutionItem> Items => null;

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
            Parent = Solution = this;
        }

        private async Task LoadSolution(string path)
        {
            await PackageManager.EnsurePackage("AvalonStudio.Languages.CSharp", IoC.Get<IConsole>());

            var dotnetDirectory = Path.Combine(PackageManager.GetPackageDirectory("AvalonStudio.Languages.CSharp"), "content");
            var dotnet = new DotNetCliService(Path.Combine(dotnetDirectory, "dotnet"));

            var dotnetInfo = dotnet.GetInfo();

            var currentDir = Platform.ExecutionPath;

            var loadedAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            var assemblies = new[]
            {
                AddItem(OmniSharpProject.Create(this, project.Path, project));
            }
            else if(Path.GetExtension(path) == ".csproj")
            {
                var roslynProject = await Workspace.AddProject("", path);

                var asProject = OmniSharpProject.Create(roslynProject.Item1, this, path, roslynProject.Item2);

                AddProject(asProject);

                asProject.LoadFiles();
            }

            Location = path;

            Name = Path.GetFileNameWithoutExtension(path);

           

            CurrentDirectory = Path.GetDirectoryName(path);
        }

        public void RemoveItem(ISolutionItem item)
        {
            throw new NotImplementedException();
        }

        public T AddItem<T>(T item, ISolutionFolder parent = null) where T : ISolutionItem
        {
            if (item is IProject project)
            {
                var currentProject = Projects.FirstOrDefault(p => p.Name == project.Name);

                if (currentProject != null) return (T)currentProject;

                //ProjectReferences.Add(CurrentDirectory.MakeRelativePath(project.Location));
                Items.InsertSorted(project);
                currentProject = project;

                return (T)currentProject;
            }

            return item;
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

        public bool CanRename => false;

        public string Name { get; set; }

        public IEnumerable<IProject> Projects { get; set; }

        public IProject StartupProject { get; set; }

        public string Location { get; private set; }
        public ISolution Solution { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISolutionFolder Parent { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Guid Id { get; set; } = Guid.NewGuid();

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

        public void Save()
        {
            //throw new NotImplementedException();
        }

        public void SetItemParent(ISolutionItem item, ISolutionFolder parent)
        {
            throw new NotImplementedException();
        }

        public void AddFolder(ISolutionFolder name)
        {
            throw new NotImplementedException();
        }

        public IProject FindProject(string name)
        {
            throw new NotImplementedException();
        }

        public void VisitChildren(Action<ISolutionItem> visitor)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(ISolutionItem item)
        {
            throw new NotImplementedException();
        }
    }
}