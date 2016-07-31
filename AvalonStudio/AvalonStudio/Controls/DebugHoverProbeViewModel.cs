namespace AvalonStudio.Controls
{
    using AvalonStudio.Debugging;

    public class DebugHoverProbeViewModel : WatchListViewModel
    {
        private DebugHoverProbeViewModel()
        {

        }

        public DebugHoverProbeViewModel(IDebugManager debugManager) : base(debugManager)
        {

        }
    }
}
