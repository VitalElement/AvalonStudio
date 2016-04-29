namespace AvalonStudio.Toolchains.Make
{
    using AvalonStudio.Toolchains;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Projects;
    using Utils;
    using Perspex.Controls;

    public class MakeToolChain : IToolChain
    {
        public string Description
        {
            get
            {
                return "Builds GNU Makefile projects.";
            }
        }

        public IList<string> Includes
        {
            get
            {
                return new List<string>();
            }
        }

        public string Name
        {
            get
            {
                return "Make Toolchain";
            }
        }

        public Version Version
        {
            get
            {
                return new Version();
            }
        }

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        public Task<bool> Build(IConsole console, IProject project, string label = "")
        {
            // "make"
            return Task.Factory.StartNew(() =>
            {
                console.WriteLine("make");
                return true;
            });
        }

        public bool CanHandle(IProject project)
        {
            return true;
        }

        public Task Clean(IConsole console, IProject project)
        {
            // "make clean"
            return Task.Factory.StartNew(() =>
            {
                console.WriteLine("make clean");
            });
        }

        public IList<TabItem> GetConfigurationPages(IProject project)
        {
            return new List<TabItem>();
        }

        public UserControl GetSettingsControl(IProject project)
        {
            return null;
        }

        public void ProvisionSettings(IProject project)
        {
            
        }
    }
}
