using System.Linq;
using System.Threading.Tasks;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages.CPlusPlus;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;

namespace AvalonStudio.Toolchains.Clang
{
	public class ClangCPlusPlusProjectTemplate : BlankCPlusPlusLangaguageTemplate
	{
		public override string DefaultProjectName
		{
			get { return "STM32Project"; }
		}

		public override string Title
		{
			get { return "STM32 C++ Project"; }
		}

		public override string Description
		{
			get { return "Basic template for STM32 based devices. Includes startup code and peripheral libraries."; }
		}

		public override async Task<IProject> Generate(ISolution solution, string name)
		{
			var project = await base.Generate(solution, name);

			project.ToolChain = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc is ClangToolchain);

			var settings = ClangToolchain.ProvisionClangSettings(project);

			project.AddFile(SourceFile.Create(project, project, project.CurrentDirectory, "main.cpp", "int main (void){}"));

			project.Save();

			return project;
		}
	}
}