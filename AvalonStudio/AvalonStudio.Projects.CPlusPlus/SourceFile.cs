using System;
using System.IO;
using AvalonStudio.Platforms;
using Newtonsoft.Json;

namespace AvalonStudio.Projects.CPlusPlus
{
	public class SourceFile : ISourceFile
	{
		private SourceFile()
		{
		}

		public string Flags { get; set; }

		public string File { get; set; }

		public int CompareTo(ISourceFile other)
		{
			return File.CompareFilePath(other.File);
		}

		[JsonIgnore]
		public string Location
		{
			get { return Path.Combine(Project.CurrentDirectory, File); }
		}

		[JsonIgnore]
		public IProject Project { get; set; }

		[JsonIgnore]
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

		public static SourceFile FromPath(IProject project, IProjectFolder parent, string filePath)
		{
			return new SourceFile {Project = project, Parent = parent, File = filePath.ToPlatformPath()};
		}

		public static SourceFile Create(IProject project, IProjectFolder parent, string location, string name,
			string text = "")
		{
			var filePath = Path.Combine(location, name);
			var file = System.IO.File.CreateText(filePath);
			file.Write(text);
			file.Close();

			return new SourceFile {File = filePath.ToPlatformPath(), Project = project};
		}

        public int CompareTo(IProjectItem other)
        {
            return this.CompareProjectItems(other);
        }

        public int CompareTo(string other)
        {
            return Location.CompareFilePath(other);
        }
    }
}