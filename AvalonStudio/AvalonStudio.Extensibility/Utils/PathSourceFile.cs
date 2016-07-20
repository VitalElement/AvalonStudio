using System.IO;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;

namespace AvalonStudio.Extensibility.Utils
{
	public class PathSourceFile : ISourceFile
	{
		private PathSourceFile()
		{
		}

		public string Flags { get; set; }

		public string File { get; set; }

		public int CompareTo(ISourceFile other)
		{
			return File.CompareFilePath(other.File);
		}

		public string Location
		{
			get { return Path.Combine(Project.CurrentDirectory, File); }
		}

		public IProject Project { get; set; }

		public Language Language
		{
			get
			{
				var result = Language.C;

				switch (Path.GetExtension(File))
				{
					case ".c":
						result = Language.C;
						break;

					case ".cpp":
						result = Language.Cpp;
						break;
				}

				return result;
			}
		}

		public string Name
		{
			get { return Path.GetFileName(Location); }
		}

		public IProjectFolder Parent { get; set; }

		public string CurrentDirectory
		{
			get { return Path.GetDirectoryName(Location); }
		}

		public void SetProject(IProject project)
		{
			Project = project;
		}

		public static PathSourceFile FromPath(IProject project, IProjectFolder parent, string filePath)
		{
			return new PathSourceFile {Project = project, Parent = parent, File = filePath.ToPlatformPath()};
		}
	}
}