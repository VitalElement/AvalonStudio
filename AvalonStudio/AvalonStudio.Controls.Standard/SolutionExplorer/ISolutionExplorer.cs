using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public interface ISolutionExplorer : IToolViewModel
    {
        void NewSolution();

        void OpenSolution();
    }
}