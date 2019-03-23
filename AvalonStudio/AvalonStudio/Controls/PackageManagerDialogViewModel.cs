using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Packages;
using AvalonStudio.Packaging;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    public class PackageManagerDialogViewModel : ModalDialogViewModelBase, IConsole
    {
        //private ObservableCollection<PackageMetaData> availablePackages;

        private bool enableInterface = true;

        //private PackageMetaData selectedPackage;

        private string selectedVersion;

        private string status;

        private void InvalidateInstalledPackages()
        {
            InstalledPackages = new ObservableCollection<PackageIdentityViewModel>(PackageManager.ListInstalledPackages().Select(x => new PackageIdentityViewModel(x)));
            //InstalledPackages = new ObservableCollection<PackageIdentityViewModel>(AvalonStudio.Packages.PackageManager.ListInstalledPackages().Select(pr => new PackageIdentityViewModel(pr)));
        }

        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            //AvailablePackages = new ObservableCollection<PackageMetaData>();

            Dispatcher.UIThread.InvokeAsync(async () =>
            {
                InvalidateInstalledPackages();

                try
                {
                    await DownloadCatalog();
                }
                catch (Exception)
                {
                    // TODO indicate error accessing catalog.
                }
            });

            InstallCommand = ReactiveCommand.Create(() =>
            {
                //await AvalonStudio.Packages.PackageManager.InstallPackage(selectedPackage.Identity.Id, selectedPackage.Identity.Version.ToFullString());

                InvalidateInstalledPackages();
            });

            UninstallCommand = ReactiveCommand.Create(() =>
            {
                if (SelectedInstalledPackage != null)
                {
                   // await AvalonStudio.Packages.PackageManager.UninstallPackage(SelectedInstalledPackage.Model.Id, SelectedInstalledPackage.Model.Version.ToNormalizedString());

                    InvalidateInstalledPackages();
                }
            });

            OKCommand = ReactiveCommand.Create(() =>
            {
                IoC.Get<IStudio>().InvalidateCodeAnalysis();
                Close();
            },
            this.WhenAnyValue(x => x.EnableInterface));

            EnableInterface = true;
        }

        public string ButtonText
        {
            get
            {
               // if (selectedPackage != null)
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

        /*public PackageMetaData SelectedPackage
        {
            get
            {
                return selectedPackage;
            }
            set
            {
                if (value != null)
                {
                    Task.Run(async () => { Versions = (await value.GetVersionsAsync()); });
                }

                this.RaiseAndSetIfChanged(ref selectedPackage, value);
                this.RaisePropertyChanged(() => ButtonText);
            }
        }*/

        private IEnumerable<string> _versions;

        public IEnumerable<string> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public string SelectedVersion
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

        /*public ObservableCollection<PackageMetaData> AvailablePackages
        {
            get { return availablePackages; }
            set { this.RaiseAndSetIfChanged(ref availablePackages, value); }
        }*/

        private ObservableCollection<PackageIdentityViewModel> installedPackages;

        public ObservableCollection<PackageIdentityViewModel> InstalledPackages
        {
            get { return installedPackages; }
            set { this.RaiseAndSetIfChanged(ref installedPackages, value); }
        }

        private PackageIdentityViewModel selectedInstalledPackage;

        public PackageIdentityViewModel SelectedInstalledPackage
        {
            get { return selectedInstalledPackage; }
            set { this.RaiseAndSetIfChanged(ref selectedInstalledPackage, value); }
        }

        public ReactiveCommand<Unit, Unit> InstallCommand { get; }
        public ReactiveCommand<Unit, Unit> UninstallCommand { get; }
        public override ReactiveCommand<Unit, Unit> OKCommand { get; protected set; }

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

        private Task DownloadCatalog()
        {
            // var packages = await AvalonStudio.Packages.PackageManager.ListPackagesAsync(100);

            //foreach (var package in packages.Where(p => p.Title.EndsWith(Platform.AvalonRID) || p.Tags.Contains("gccdescription")))
            //{
            //  availablePackages.Add(package);
            //}

            return Task.CompletedTask;
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