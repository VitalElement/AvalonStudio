using AvalonStudio.Extensibility;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Toolchains.Clang
{
    public class ClangCPlusPlusProjectTemplate : BlankCPlusPlusLanguageTemplate
    {
        public override string DefaultProjectName
        {
            get { return "ClangProject"; }
        }

        public override string Title
        {
            get { return "Clang C++ Project"; }
        }

        public override string Description
        {
            get { return "Basic template for projects using Clang Toolchain."; }
        }

        public override async Task<IProject> Generate(ISolutionFolder solutionFolder, string name)
        {
            var project = await base.Generate(solutionFolder, name);

            project.ToolChain = IoC.Get<IShell>().ToolChains.FirstOrDefault(tc => tc is ClangToolchain);

            await SourceFile.Create(project, "main.cpp", "int main (void){}");

            project.Save();

            return project;
        }
    }
}