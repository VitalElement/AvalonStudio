namespace AvalonStudio.Toolchains.LocalGCC
{
    using AvalonStudio.Languages.CPlusPlus;
    using Extensibility;
    using Models.Tools.Debuggers.Local;
    using Projects;
    using Projects.CPlusPlus;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class STM32CPlusPlusProjectTemplate : BlankCPlusPlusLangaguageTemplate
    {
        public override string DefaultProjectName
        {
            get
            {
                return "ConsoleApplication";
            }
        }

        public override string Title
        {
            get
            {
                return "C++ Console Application";
            }
        }

        public override string Description
        {
            get
            {
                return "Creates a simple console application.";
            }
        }

        public override async Task<IProject> Generate(ISolution solution, string name)
        {
            var project = await base.Generate(solution, name);

            project.ToolChain = Shell.Instance.ToolChains.FirstOrDefault(tc => tc is LocalGCCToolchain);

            var settings = LocalGCCToolchain.ProvisionLocalGccSettings(project);

            project.Debugger = Shell.Instance.Debuggers.FirstOrDefault(db => db is LocalDebugAdaptor);

            var code = new StringBuilder();

            code.AppendLine("#include <stdio.h>");
            code.AppendLine();
            code.AppendLine("int main (void)");
            code.AppendLine("{");
            code.AppendLine("    printf(\"Hello World\");");
            code.AppendLine("    return 0;");
            code.AppendLine("}");
            code.AppendLine();

            project.AddFile(SourceFile.Create(project, project, project.CurrentDirectory, "main.cpp", code.ToString()));

            project.Save();

            return project;
        }
    }
}
