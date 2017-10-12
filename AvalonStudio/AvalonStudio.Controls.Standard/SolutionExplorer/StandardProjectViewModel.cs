using Avalonia.Media;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    internal class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(IProject model) : base(model)
        {
            if (model.Solution.StartupProject == model)
            {
                IsExpanded = true;
            }
        }

        public ReactiveCommand SetDefaultProjectCommand { get; private set; }

        public ReactiveCommand Remove { get; private set; }

        public override DrawingGroup Icon => Model.GetIcon();
    }
}