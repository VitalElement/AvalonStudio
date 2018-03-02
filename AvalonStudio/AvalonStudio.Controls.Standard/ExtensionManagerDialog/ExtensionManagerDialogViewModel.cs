using AvalonStudio.Extensibility;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace AvalonStudio.Controls.Standard.ExtensionManagerDialog
{
    public class ExtensionManagerDialogViewModel : DocumentTabViewModel
    {
        private IExtensionManager _extensionManager;

        private ObservableCollection<IExtensionManifest> _installedExtensions;
        private IExtensionManifest _selectedExtension;

        public ExtensionManagerDialogViewModel(IExtensionManager extensionManager)
        {
            Title = "Extensions";

            _extensionManager = extensionManager;
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
