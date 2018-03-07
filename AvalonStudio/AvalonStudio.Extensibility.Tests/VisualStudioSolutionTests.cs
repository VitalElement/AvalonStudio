using AvalonStudio.Projects;
using System.Linq;
using System;
using Xunit;

namespace AvalonStudio.Extensibility.Tests
{
    public class VisualStudioSolutionTests
    {
        private const string TestProjectTypeGuidString = "42c950ab-82c7-447b-9b4a-31e8c85f8083";
        private static readonly Guid TestProjectTypeGuid = new Guid(TestProjectTypeGuidString);

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

        [Fact]
        public void Removing_Solution_Folder_Removes_Children()
        {
            var solution = VisualStudioSolution.Create(@"c:\TestProject\Test.sln", "Test", false);
            solution.SaveEnabled = false;

            var startupProject = new TestProject("c:\\TestProject\\Project1\\Project.csproj");

            solution.AddItem(startupProject);

            solution.StartupProject = startupProject;

            var folder = solution.AddItem(SolutionFolder.Create("SubFolder"));

            solution.AddItem(new TestProject("c:\\TestProject\\Project1\\Project1.csproj"), TestProjectTypeGuid, folder);
            solution.AddItem(new TestProject("c:\\TestProject\\Project1\\Project2.csproj"), TestProjectTypeGuid, folder);

            Assert.Equal(3, solution.Projects.Count());
            Assert.Equal(4, solution.Model.Projects.Count);

            solution.RemoveItem(folder);
            Assert.Equal(1, solution.Projects.Count());
            Assert.Equal(1, solution.Model.Projects.Count);

            Assert.Equal(startupProject, solution.Projects.FirstOrDefault());
        }
    }
}
