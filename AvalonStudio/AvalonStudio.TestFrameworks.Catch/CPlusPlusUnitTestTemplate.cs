﻿namespace AvalonStudio.Languages.CPlusPlus
{
    using AvalonStudio.Projects;
    using Extensibility;
    using Extensibility.Git;
    using Models.Tools.Debuggers.Local;
    using Projects.CPlusPlus;
    using Shell;
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using TestFrameworks.Catch;
    using Toolchains.LocalGCC;
    using Utils;

    public class CPlusPlusUnitTestTemplate : BlankCPlusPlusLangaguageTemplate, IConsole
    {
        public override string DefaultProjectName
        {
            get
            {
                return "UnitTestProject";
            }
        }

        public override string Description
        {
            get
            {
                return "Creates a Unit Test Project using Catch Test Framework.";
            }
        }

        public override string Title
        {
            get
            {
                return "C/C++ Unit Test Project";
            }
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public override async Task<IProject> Generate(ISolution solution, string name)
        {
            var shell = IoC.Get<IShell>();
            var project = await base.Generate(solution, name);
            string catchProjectDir = Path.Combine(solution.CurrentDirectory, "AvalonStudio.Testing.Catch");
            string catchProjectFile = Path.Combine(catchProjectDir, "CatchTestFramework.acproj");
            if (!Directory.Exists(catchProjectDir))
            {
                await Git.ClonePublicHttpSubmodule(this, "https://github.com/VitalElement/AvalonStudio.Testing.Catch.git", catchProjectDir);
            }

            // Add project
            var catchProject = Solution.LoadProjectFile(solution, catchProjectFile);
            catchProject.Hidden = true;

            solution.AddProject(catchProject);

            // Reference catch.
            project.AddReference(catchProject);
            
            project.AddFile(SourceFile.Create(project, project, project.CurrentDirectory, "UnitTest1.cpp", new UnitTestTemplate().TransformText()));
            project.ToolChain = shell.ToolChains.FirstOrDefault(tc => tc is LocalGCCToolchain);
            project.Debugger = shell.Debuggers.FirstOrDefault(d => d is LocalDebugAdaptor);
            project.TestFramework = shell.TestFrameworks.FirstOrDefault(d => d is CatchTestFramework);
            var settings = LocalGCCToolchain.ProvisionLocalGccSettings(project);

            settings.CompileSettings.Exceptions = true;
            settings.CompileSettings.Rtti = true;
            settings.CompileSettings.Optimization = OptimizationLevel.Debug;
            settings.CompileSettings.CustomFlags += " -Wno-unknown-pragmas ";

            project.Save();
            solution.Save();

            return project;
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
    }
}
