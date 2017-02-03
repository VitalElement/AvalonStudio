using System.Collections.Generic;
using System.IO;
using AvalonStudio.Projects.Standard;

namespace AvalonStudio.Toolchains
{
	public static class ProjectExtensions
	{
		public static string GetOutputDirectory(this IStandardProject project, IStandardProject superProject)
		{
			var outputDirectory = string.Empty;

			if (string.IsNullOrEmpty(superProject.BuildDirectory))
			{
				outputDirectory = Path.Combine(superProject.CurrentDirectory, "build");
			}

			if (!string.IsNullOrEmpty(superProject.BuildDirectory))
			{
				outputDirectory = Path.Combine(superProject.CurrentDirectory, superProject.BuildDirectory);
			}

			if (project != superProject)
			{
				outputDirectory = Path.Combine(outputDirectory, project.Name);
			}

			return outputDirectory;
		}

		public static string GetObjectDirectory(this IStandardProject project, IStandardProject superProject)
		{
			return Path.Combine(project.GetOutputDirectory(superProject), "obj");
		}

		public static string GetBinDirectory(this IStandardProject project, IStandardProject superProject)
		{
			return Path.Combine(project.GetOutputDirectory(superProject), "bin");
		}

		public static List<string> GetDependencies(string dependencyFile)
		{
			var result = new List<string>();

            using (var sr = File.OpenText(dependencyFile))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (!line.EndsWith(":") && !line.EndsWith(": \\"))
                        {
                            result.Add(line.Replace(" \\", string.Empty).Trim());
                        }
                    }
                }
            }

			return result;
		}
	}
}