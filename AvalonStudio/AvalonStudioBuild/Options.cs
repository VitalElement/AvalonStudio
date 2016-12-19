using AvalonStudio.Projects.Standard;
using CommandLine;
using System.Collections.Generic;

namespace AvalonStudio
{
	internal class Options
	{
		[Option('p', "Project", Required = true, HelpText = "Name of project to build.")]
		public string Project { get; set; }

		[Option('j', "jobs", Required = false, HelpText = "Number of jobs for compiling.")]
		public int Jobs { get; set; }
	}

	internal abstract class ProjectOption
	{
		[Value(0, MetaName = "Solution", HelpText = "Solution file (asln)", Required = true)]
		public string Solution { get; set; }

		[Value(1, MetaName = "Project", HelpText = "Name of project to run command on")]
		public string Project { get; set; }
	}

	[Verb("test")]
	internal class TestOptions : ProjectOption
	{
	}

	[Verb("build", HelpText = "Builds the project in the current directory.")]
	internal class BuildOptions : ProjectOption
	{
		[Option('l', "label", Required = false, Default = "",
			HelpText = "Provides a label to append to the output file name of the build process. Usually a build number.")]
		public string Label { get; set; }

        [Option('D', "define", Required = false, Separator = ':')]
        public IEnumerable<string> Defines { get; set; }

        [Option('j', "jobs", Required = false, Default = 4, HelpText = "Number of jobs for compiling.")]
		public int Jobs { get; set; }
	}

	[Verb("install-package", HelpText = "Installs a package manually.")]
	internal class PackageOptions
	{
		[Value(0, MetaName = "PackageName", HelpText = "Package Name i.e. AvalonStudio.Toolchains.STM32")]
		public string Package { get; set; }

		[Value(1, MetaName = "Tag", HelpText = "")]
		public string Tag { get; set; }
	}

	[Verb("clean", HelpText = "Cleans the specified project.")]
	internal class CleanOptions : ProjectOption
	{
	}

	[Verb("add", HelpText = "Adds source files to the specified project file.")]
	internal class AddOptions : ProjectOption
	{
		[Value(0, Required = true, HelpText = "File name to add to directory.")]
		public string File { get; set; }
	}

	[Verb("remove", HelpText = "Removes source files to the specified project file.")]
	internal class RemoveOptions : ProjectOption
	{
		[Value(0, Required = true, HelpText = "File name to add to directory.")]
		public string File { get; set; }
	}

	[Verb("addref", HelpText = "Adds a reference to the current project")]
	internal class AddReferenceOptions : ProjectOption
	{
		[Value(1, Required = true, HelpText = "Name of the reference.", MetaName = "Reference Name")]
		public string Name { get; set; }

		[Option('u', "giturl", HelpText = "Url to GitRepository containing a reference.")]
		public string GitUrl { get; set; }

		[Option('r', "revision", HelpText = "Revision to keep the reference at, this can be HEAD, any tag or SHA")]
		public string Revision { get; set; }
	}

	[Verb("create", HelpText = "Creates new projects.")]
	internal class CreateOptions
	{
		[Value(0, Required = true, MetaName = "Project Name", HelpText = "Name of project to create")]
		public string Project { get; set; }

		[Option('t', "Type", Required = true, HelpText = "Options are Executable, StaticLibrary or SuperProject")]
		public ProjectType Type { get; set; }
	}
}