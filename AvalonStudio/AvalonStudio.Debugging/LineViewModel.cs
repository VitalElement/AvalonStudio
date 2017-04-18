using AvalonStudio.MVVM;

namespace AvalonStudio.Debugging
{
    public abstract class LineViewModel : ViewModel<DisassembledLine>
    {
        public LineViewModel(DisassembledLine model) : base(model)
        {
        }
    }
}