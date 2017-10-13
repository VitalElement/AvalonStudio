using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Linq;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SolutionViewModel : SolutionParentViewModel<ISolution>
    {
        private readonly IShell shell;

        public SolutionViewModel(ISolution model) : base(null, model)
        {
            Parent = this;

            shell = IoC.Get<IShell>();

            OpenInExplorerCommand = ReactiveCommand.Create(() => { Platform.OpenFolderInExplorer(model.CurrentDirectory); });

            BuildSolutionCommand = ReactiveCommand.Create(() => BuildSolution());

            CleanSolutionCommand = ReactiveCommand.Create(() => CleanSolution());

            RebuildSolutionCommand = ReactiveCommand.Create(() =>
            {
                CleanSolution();

                BuildSolution();
            });

            RunAllTestsCommand = ReactiveCommand.Create(() => RunTests());

            IsExpanded = true;
        }

        public ReactiveCommand CleanSolutionCommand { get; }
        public ReactiveCommand BuildSolutionCommand { get; }
        public ReactiveCommand RebuildSolutionCommand { get; }
        public ReactiveCommand RunAllTestsCommand { get; }
        public ReactiveCommand OpenInExplorerCommand { get; }

        private void CleanSolution()
        {
        }

        private void BuildSolution()
        {
        }

        private void RunTests()
        {
        }

        public override string Title
        {
            get
            {
                if (Model != null)
                {
                    return string.Format("Solution '{0}' ({1} {2})", Model.Name, Model.Solution.Projects.Count(), StringProjects);
                }

                return string.Empty;
            }
        }

        private string StringProjects
        {
            get
            {
                if (Model.Solution.Projects.Count() == 1)
                {
                    return "project";
                }

                return "projects";
            }
        }

        public override DrawingGroup Icon => null;
    }
}