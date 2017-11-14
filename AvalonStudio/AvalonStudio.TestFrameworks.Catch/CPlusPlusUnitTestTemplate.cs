using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Git;
using AvalonStudio.Extensibility.Templating;
using AvalonStudio.Projects;
using AvalonStudio.Projects.CPlusPlus;
using AvalonStudio.Shell;
using AvalonStudio.TestFrameworks.Catch;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Toolchains.LocalGCC;
using AvalonStudio.Utils;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus
{
    public class CPlusPlusUnitTestTemplate : BlankCPlusPlusLanguageTemplate, IConsole
    {
        public override string DefaultProjectName
        {
            get { return "UnitTestProject"; }
        }

        public override string Description
        {
            get { return "Creates a Unit Test Project using Catch Test Framework."; }
        }

        public override string Title
        {
            get { return "C/C++ Unit Test Project"; }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void OverWrite(string data)
        {
            Console.WriteLine(data);
        }

        public void Write(char data)
        {
            throw new NotImplementedException();
        }

        public void Write(string data)
        {
            throw new NotImplementedException();
        }

        public void WriteLine()
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string data)
        {
            throw new NotImplementedException();
        }

        public override async Task<IProject> Generate(ISolutionFolder solutionFolder, string name)
        {
            var shell = IoC.Get<IShell>();
            var project = await base.Generate(solutionFolder, name);
            var catchProjectDir = Path.Combine(solutionFolder.Solution.CurrentDirectory, "AvalonStudio.Testing.Catch");
            var catchProjectFile = Path.Combine(catchProjectDir, "CatchTestFramework.acproj");
            if (!Directory.Exists(catchProjectDir))
            {
                await
                    Git.ClonePublicHttpSubmodule(this, "https://github.com/VitalElement/AvalonStudio.Testing.Catch.git",
                        catchProjectDir);
            }

            // Add project
            var catchProject = await Project.LoadProjectFileAsync(solutionFolder.Solution, catchProjectFile);
            catchProject.Hidden = true;

            solutionFolder.Solution.AddItem(catchProject);

            // Reference catch.
            project.AddReference(catchProject);

            await SourceFile.Create(project, "UnitTest1.cpp", await Template.Engine.CompileRenderAsync("CatchUnitTest.template", new { }));

            project.ToolChain = shell.ToolChains.FirstOrDefault(tc => tc is LocalGCCToolchain);
            project.Debugger2 = shell.Debugger2s.FirstOrDefault(db => db.GetType().FullName == "AvalonStudio.Debuggers.GDB.Local.LocalGdbDebugger");
            project.TestFramework = shell.TestFrameworks.FirstOrDefault(d => d is CatchTestFramework);

            var settings = project.GetToolchainSettings<GccToolchainSettings>();
            settings.CompileSettings.Exceptions = true;
            settings.CompileSettings.Rtti = true;
            settings.CompileSettings.Optimization = OptimizationLevel.Debug;
            settings.CompileSettings.CustomFlags += " -Wno-unknown-pragmas ";

            project.Save();
            solutionFolder.Solution.Save();

            return project;
        }
    }
}