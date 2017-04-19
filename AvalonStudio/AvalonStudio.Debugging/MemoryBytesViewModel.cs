namespace AvalonStudio.Debugging
{
    using MVVM;
    using System.Threading.Tasks;

    public abstract class MemoryBytesViewModel : ViewModel
    {
        public abstract Task InvalidateAsync(IDebugger2 debugger);
    }
}