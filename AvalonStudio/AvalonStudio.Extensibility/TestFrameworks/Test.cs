using AvalonStudio.Projects;

namespace AvalonStudio.TestFrameworks
{
	public class Test
	{
		private readonly IProject _project;

		public Test(IProject project)
		{
			_project = project;
		}

		public string Name { get; set; }
		public string File { get; set; }
		public int Line { get; set; }
		public string Assertion { get; set; }
		public bool Pass { get; set; }

		public void Run()
		{
			_project.TestFramework.RunTestAsync(this, _project).Wait();
		}
	}
}