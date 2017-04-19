using ReactiveUI;

namespace AvalonStudio.MVVM
{
    public interface IActivatable
    {
        void Activate();
    }

    public enum Location
    {
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