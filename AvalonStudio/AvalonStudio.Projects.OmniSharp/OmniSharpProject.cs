using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Debugging;
using AvalonStudio.TestFrameworks;
using AvalonStudio.Toolchains;

namespace AvalonStudio.Projects.OmniSharp
{
    public class OmniSharpProject : IProject
    {
        public IList<object> ConfigurationPages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string CurrentDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IDebugger Debugger
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public dynamic DebugSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Executable
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Extension
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool Hidden
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<IProjectItem> Items
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Location
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string LocationDirectory
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public IProjectFolder Parent
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IProject Project
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public ObservableCollection<IProject> References
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ISolution Solution
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ITestFramework TestFramework
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public IToolChain ToolChain
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public dynamic ToolchainSettings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public event EventHandler FileAdded;

        public void AddReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectFolder other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(string other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProject other)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(IProjectItem other)
        {
            throw new NotImplementedException();
        }

        public void ExcludeFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void ExcludeFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public ISourceFile FindFile(string path)
        {
            throw new NotImplementedException();
        }

        public IProject Load(ISolution solution, string filePath)
        {
            throw new NotImplementedException();
        }

        public void RemoveReference(IProject project)
        {
            throw new NotImplementedException();
        }

        public void ResolveReferences()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
