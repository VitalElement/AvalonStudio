namespace AvalonStudio.TestFrameworks
{
	using Projects;

	public class Test
	{
		public Test(IProject project)
		{
			_project = project;
		}

		private readonly IProject _project;

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
