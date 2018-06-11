using ReactiveUI;

namespace AvalonStudio.MVVM
{
    public enum Location
    {
        None,
        Left,
        Right,
        Bottom,
        BottomRight,
        RightBottom,
        RightMiddle,
        RightTop,
        MiddleTop,
    }

    public abstract class ViewModel : ViewModel<object>
    {
        protected ViewModel() : base(null)
        {
        }
    }
}