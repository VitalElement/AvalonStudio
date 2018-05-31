using Dock.Model.Controls;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public interface ISolutionExplorer : IToolTab
    {
        void NewSolution();

        void OpenSolution();
    }
}