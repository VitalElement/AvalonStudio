using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    static class SolutionExplorerViewModelExtensions
    {
        public static SolutionViewModel FindRoot(this SolutionItemViewModel item)
        {
            var solution = item.Parent;

            while (true)
            {
                if (solution == null)
                {
                    throw new InvalidOperationException("Unable to find solution");
                }

                if (solution is SolutionViewModel solutionViewModel)
                {
                    return solutionViewModel;
                }

                solution = solution.Parent;
            }
        }

        public static void VisitParents (this SolutionItemViewModel item, Action<ISolutionParentViewModel> visitor)
        {
            var parent = item.Parent;

            while (true)
            {
                visitor(parent);

                if(parent is SolutionViewModel)
                {
                    return;
                }

                parent = parent.Parent;
            }
        }
    }
}
