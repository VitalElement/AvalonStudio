using AvalonStudio.Extensibility.MVVM;

namespace AvalonStudio.Extensibility.ToolBars
{
    public interface IToolBars
    {
        IObservableCollection<IToolBar> Items { get; }
        bool Visible { get; set; }
    }
}