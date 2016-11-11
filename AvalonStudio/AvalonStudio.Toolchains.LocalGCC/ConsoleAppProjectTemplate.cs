using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages.CPlusPlus;
using AvalonStudio.Models.Tools.Debuggers.Local;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;

namespace AvalonStudio.Toolchains.LocalGCC
{
	public class STM32CPlusPlusProjectTemplate : BlankCPlusPlusLanguageTemplate
	{
		public override string DefaultProjectName
		{
			get { return "ConsoleApplication"; }
		}

		public override string Title
		{
			get { return "C++ Console Application"; }
		}

		public override string Description
		{
			get { return "Creates a simple console application."; }
		}

		public override async Task<IProject> Generate(ISolution solution, string name)
		{
			var shell = IoC.Get<IShell>();
			var project = await base.Generate(solution, name);

			project.ToolChain = shell.ToolChains.FirstOrDefault(tc => tc is LocalGCCToolchain);

            project.ToolChain.ProvisionSettings(project);

			project.Debugger = shell.Debuggers.FirstOrDefault(db => db is LocalDebugAdaptor);

			var code = new StringBuilder();

			code.AppendLine("#include <stdio.h>");
			code.AppendLine();
			code.AppendLine("int main (void)");
			code.AppendLine("{");
			code.AppendLine("    printf(\"Hello World\");");
			code.AppendLine("    return 0;");
			code.AppendLine("}");
			code.AppendLine();

			await SourceFile.Create(project, "main.cpp", code.ToString());

			project.Save();

			return project;
		}
	}
}