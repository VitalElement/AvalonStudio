namespace AvalonStudio.Extensibility.Menus
{
    using System.Windows.Input;

    public interface IMenuItem
    {
        string Title { get; }
        ICommand Command { get; } 
    }
}
