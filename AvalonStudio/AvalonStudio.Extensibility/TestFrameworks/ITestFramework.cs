using System.Collections.Generic;
using System.Threading.Tasks;
using AvalonStudio.Projects;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio.TestFrameworks
{
	public interface ITestFramework : IExtension
	{
		Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project);

		Task RunTestAsync(Test test, IProject project);
	}
}