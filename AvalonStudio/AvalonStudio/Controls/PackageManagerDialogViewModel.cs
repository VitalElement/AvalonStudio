using Avalonia.Threading;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.MVVM;
using AvalonStudio.Packages;
using AvalonStudio.Utils;
using NuGet.Common;
using NuGet.Protocol.Core.Types;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class PackageManagerDialogViewModel : ModalDialogViewModelBase, IConsole, ILogger
    {
        private ObservableCollection<IPackageSearchMetadata> availablePackages;

        private bool enableInterface = true;

        private IPackageSearchMetadata selectedPackage;

        private PackageManager _packageManager;

        private VersionInfoViewModel selectedVersion;

        private string status;

        private void InvalidateInstalledPackages()
        {
            InstalledPackages = new ObservableCollection<PackageIdentityViewModel>((_packageManager.ListInstalledPackages()).Select(pr => new PackageIdentityViewModel(pr)));
        }

        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            _packageManager = new PackageManager(this);

            AvailablePackages = new ObservableCollection<IPackageSearchMetadata>();

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                InvalidateInstalledPackages();

                await DownloadCatalog();
            });

            InstallCommand = ReactiveCommand.Create();
            InstallCommand.Subscribe(async _ =>
            {
                await _packageManager.InstallPackage(selectedPackage.Identity.Id, selectedPackage.Identity.Version.ToFullString());

                InvalidateInstalledPackages();
            });

            UninstallCommand = ReactiveCommand.Create();
            UninstallCommand.Subscribe(async _ =>
            {
                await _packageManager.UninstallPackage(selectedPackage.Identity.Id, selectedPackage.Identity.Version.ToNormalizedString());

                InvalidateInstalledPackages();
            });

            OKCommand = ReactiveCommand.Create(this.WhenAnyValue(x => x.EnableInterface));

            OKCommand.Subscribe(_ =>
            {
                ShellViewModel.Instance.InvalidateCodeAnalysis();
                Close();
            });

            EnableInterface = true;
        }

        public string ButtonText
        {
            get
            {
                if (selectedPackage != null)
                {
                    //if (selectedPackage.IsInstalled)
                    //{
                    //    return "Update";
                    //}
                }

                return "Install";
            }
        }

        public bool EnableInterface
        {
            get
            {
                return enableInterface;
            }
            set
            {
                OKButtonVisible = value;
                CancelButtonVisible = value;

                this.RaiseAndSetIfChanged(ref enableInterface, value);
            }
        }

        public string Status
        {
            get { return status; }
            set { this.RaiseAndSetIfChanged(ref status, value); }
        }

        public IPackageSearchMetadata SelectedPackage
        {
            get
            {
                return selectedPackage;
            }
            set
            {
                if (value != null)
                {
                    Task.Run(async () => { Versions = (await value.GetVersionsAsync()).Select(vi => new VersionInfoViewModel(vi)); });
                }

                this.RaiseAndSetIfChanged(ref selectedPackage, value);
                this.RaisePropertyChanged(() => ButtonText);
            }
        }

        private IEnumerable<VersionInfoViewModel> _versions;

        public IEnumerable<VersionInfoViewModel> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public VersionInfoViewModel SelectedVersion
        {
            get
            {
                return selectedVersion;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedVersion, value);
            }
        }

        public ObservableCollection<IPackageSearchMetadata> AvailablePackages
        {
            get { return availablePackages; }
            set { this.RaiseAndSetIfChanged(ref availablePackages, value); }
        }

        private ObservableCollection<PackageIdentityViewModel> installedPackages;

        public ObservableCollection<PackageIdentityViewModel> InstalledPackages
        {
            get { return installedPackages; }
            set { this.RaiseAndSetIfChanged(ref installedPackages, value); }
        }

        public ReactiveCommand<object> InstallCommand { get; }
        public ReactiveCommand<object> UninstallCommand { get; }
        public override ReactiveCommand<object> OKCommand { get; protected set; }

        public void WriteLine(string data)
        {
            throw new NotImplementedException();
        }

        public void WriteLine()
        {
            throw new NotImplementedException();
        }

        public void OverWrite(string data)
        {
            Dispatcher.UIThread.InvokeAsync(() => { Status = data; });
        }

        public void Write(string data)
        {
            throw new NotImplementedException();
        }

        public void Write(char data)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        private async Task DownloadCatalog()
        {
            var packages = await _packageManager.ListPackages(100);

            foreach (var package in packages)
            {
                availablePackages.Add(package);
            }
        }

        public void LogDebug(string data)
        {
            throw new NotImplementedException();
        }

        public void LogVerbose(string data)
        {
            throw new NotImplementedException();
        }

        public void LogInformation(string data)
        {
            Status = data;
        }

        public void LogMinimal(string data)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string data)
        {
            throw new NotImplementedException();
        }

        public void LogError(string data)
        {
            throw new NotImplementedException();
        }

        public void LogInformationSummary(string data)
        {
            throw new NotImplementedException();
        }

        public void LogErrorSummary(string data)
        {
            throw new NotImplementedException();
        }
    }
}