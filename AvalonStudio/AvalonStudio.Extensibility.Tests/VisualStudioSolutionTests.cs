using AvalonStudio.Extensibility.Projects;
using System.Text;
using AvalonStudio.Platforms;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    public class VisualStudioSolutionTests
    {
        [Fact]
        public void Startup_Project_Property_is_Remove_From_Sln_When_Startup_Project_is_remove_from_Solution()
        {
            var solution = VisualStudioSolution.Create(@"c:\TestProject\Test.sln", "Test", false);
            solution.SaveEnabled = false;

            var startupProject = new TestProject("c:\\TestProject\\Project1\\Project.csproj");

            solution.AddItem(startupProject);

            solution.StartupProject = startupProject;

            solution.RemoveItem(startupProject);

            Assert.Null(solution.StartupProject);
        }
    }
}
