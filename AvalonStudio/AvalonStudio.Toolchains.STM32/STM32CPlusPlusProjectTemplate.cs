namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Languages.CPlusPlus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Projects;
    using Extensibility;
    using Projects.CPlusPlus;

    public class STM32CPlusPlusProjectTemplate : BlankCPlusPlusLangaguageTemplate
    {
        public override string DefaultProjectName
        {
            get
            {
                return "STM32Project";
            }
        }

        public override string Title
        {
            get
            {
                return "STM32 C++ Project";
            }
        }

        public override string Description
        {
            get
            {
                return "Basic template for STM32 based devices. Includes startup code and peripheral libraries.";
            }
        }

        public override async Task<IProject> Generate(ISolution solution, string name)
        {
            var project = await base.Generate(solution, name);

            project.ToolChain = Shell.Instance.ToolChains.FirstOrDefault(tc => tc is STM32GCCToolchain);

            var settings = STM32GCCToolchain.ProvisionSTM32Settings(project);

            project.AddFile(SourceFile.Create(project, project, project.CurrentDirectory, "main.cpp", "int main (void){}"));
            
            project.Save();            

            return project;
        }
    }
}
