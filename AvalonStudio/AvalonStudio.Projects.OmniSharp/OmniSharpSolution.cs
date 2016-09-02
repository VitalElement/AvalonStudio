using AvalonStudio.Extensibility.Utils;
using AvalonStudio.Languages.CSharp.OmniSharp;
using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpSolution : ISolution
    {
        public static async Task<OmniSharpSolution> Create (string path)
        {
            OmniSharpSolution result = new OmniSharpSolution();

            await result.LoadSolution(path);

            return result;            
        }

        private OmniSharpServer server;

        private OmniSharpSolution()
        {
            server = new OmniSharpServer(TcpUtils.FreeTcpPort());
           
        }

        private async Task LoadSolution (string path)
        {
            await server.StartAsync(Path.GetDirectoryName(path));

            var workspace = await server.SendRequest(new WorkspaceInformationRequest() { ExcludeSourceFiles = false });

            foreach(var project in workspace.MsBuild.Projects)
            {
                
            }
        }


        public OmniSharpServer Server { get { return server; } }

        public string CurrentDirectory { get; set; }

        public string Name { get; set; }

        public ObservableCollection<IProject> Projects { get; set; }

        public IProject StartupProject { get; set; }

        public IProject AddProject(IProject project)
        {
            throw new NotImplementedException();
        }

        public ISourceFile FindFile(string path)
        {
            throw new NotImplementedException();
        }

        public void RemoveProject(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
