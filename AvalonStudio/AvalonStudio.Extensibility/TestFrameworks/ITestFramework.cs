namespace AvalonStudio.TestFrameworks
{
    using Projects;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Threading.Tasks;
    using Utils;

    [InheritedExport(typeof(ITestFramework))]
    public interface ITestFramework
    {
        Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project);

        Task RunTestAsync(Test test, IProject project);
    }
}
