namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using System;

    public interface ISolutionParentViewModel
    {
        bool IsExpanded { get; set; }

        ISolutionParentViewModel Parent { get; }

        void VisitChildren(Action<SolutionItemViewModel> visitor);
    }
}