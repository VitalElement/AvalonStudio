using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Standard.FindInFiles
{
    class FindInFilesViewModel : ToolViewModel, IExtension
    {
        public FindInFilesViewModel()
        {
            Title = "Find Results";
        }

        public override Location DefaultLocation => Location.Bottom;

        public void Activation()
        {
        }

        public void BeforeActivation()
        {
        }
    }
}
