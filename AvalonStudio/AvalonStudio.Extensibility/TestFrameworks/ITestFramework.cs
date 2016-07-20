namespace AvalonStudio.TestFrameworks
{
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Threading.Tasks;
	using Projects;

	[InheritedExport(typeof(ITestFramework))]
	public interface ITestFramework
	{
		Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project);

		Task RunTestAsync(Test test, IProject project);
	}
}
