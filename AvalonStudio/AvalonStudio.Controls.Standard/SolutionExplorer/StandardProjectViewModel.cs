using Avalonia.Media;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    internal class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(ISolutionParentViewModel parent, IProject model) : base(parent, model)
        {
            if (model.Solution.StartupProject == model)
            {
                this.VisitParents(parentVm =>
                {
                    parentVm.IsExpanded = true;
                });

                IsExpanded = true;
            }
        }

        public ReactiveCommand SetDefaultProjectCommand { get; private set; }

        public ReactiveCommand Remove { get; private set; }

        public override DrawingGroup Icon => Model.GetIcon();
    }
}