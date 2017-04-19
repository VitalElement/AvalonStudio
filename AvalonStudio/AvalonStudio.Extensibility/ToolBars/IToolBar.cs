using AvalonStudio.Extensibility.MVVM;
using AvalonStudio.Extensibility.ToolBars.Models;

namespace AvalonStudio.Extensibility.ToolBars
{
    public interface IToolBar : IObservableCollection<ToolBarItemBase>
    {
    }
}