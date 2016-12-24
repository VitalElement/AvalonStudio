using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using AvalonStudio.Projects;

namespace AvalonStudio.TestFrameworks
{
	[InheritedExport(typeof (ITestFramework))]
	public interface ITestFramework
	{
		Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project);

		Task RunTestAsync(Test test, IProject project);
	}
}