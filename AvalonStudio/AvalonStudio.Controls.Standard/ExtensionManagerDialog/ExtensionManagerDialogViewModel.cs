using AvalonStudio.Extensibility;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.ExtensionManagerDialog
{
    public class ExtensionManagerDialogViewModel : DocumentTabViewModel
    {
        private ExtensionManager _extensionManager;

        private ObservableCollection<IExtensionManifest> _installedExtensions;
        private IExtensionManifest _selectedExtension;

        public ExtensionManagerDialogViewModel()
        {
            Title = "Extensions";

            _extensionManager = IoC.Get<ExtensionManager>();
            _installedExtensions = new ObservableCollection<IExtensionManifest>(_extensionManager.GetInstalledExtensions());
        }

        public ObservableCollection<IExtensionManifest> InstalledExtensions => _installedExtensions;

        public IExtensionManifest SelectedExtension
        {
            get => _selectedExtension;
            set => this.RaiseAndSetIfChanged(ref _selectedExtension, value);
        }
    }
}
