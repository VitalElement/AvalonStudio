using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;

namespace AvalonStudio
{
    class GeneralSettingsViewModel : SettingsViewModel, IExtension
    {
        public GeneralSettingsViewModel() : base("General")
        {
        }

        public void Activation()
        {
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("Environment", this);
        }

        public void BeforeActivation()
        {
        }
    }
}
