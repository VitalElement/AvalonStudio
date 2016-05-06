using Caliburn.Micro;

namespace AvalonStudio.ToolBars
{
    public interface IToolBars
    {
        IObservableCollection<IToolBar> Items {get;}
        bool Visible { get; set; }
    }
}