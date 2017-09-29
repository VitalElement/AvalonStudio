using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    internal class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(SolutionViewModel solutionViewModel, IProject model) : base(solutionViewModel, model)
        {
            if (model.Solution.StartupProject == model)
            {
                IsExpanded = true;
            }
        }

        public ReactiveCommand SetDefaultProjectCommand { get; private set; }

        public ReactiveCommand Remove { get; private set; }
    }
}