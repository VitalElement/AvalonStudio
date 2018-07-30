using AvalonStudio.Projects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.TestFrameworks
{
    public interface ITestFramework
    {
        Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project);

        Task RunTestAsync(Test test, IProject project);
    }
}