namespace AvalonStudio.Controls.ViewModels
{
    using MVVM;
    using Perspex.Threading;
    using ReactiveUI;
    using Repositories;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    public class PackageManagerDialogViewModel : ModalDialogViewModelBase
    {
        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            this.AvailablePackages = new ObservableCollection<PackageReference>();

            DownloadCatalog();

            //InstallCommand = ReactiveCommand.Create();
            //InstallCommand.Subscribe(async (o) =>
            //{
            //    EnableInterface = false;

            //    try
            //    {
            //        var fullPackage = await SelectedPackage.DownloadPackage(ProgressUpdate);

            //        if (fullPackage.Install())
            //        {
            //            Status = "Package Installed Successfully.";
            //        }
            //        else
            //        {
            //            Status = "An error occurred trying to install package.";
            //        }
            //    }
            //    catch (Exception e)
            //    {
            //        Status = "An error occurred trying to install package. " + e.Message;
            //    }

            //    EnableInterface = true;

            //});

            OKCommand = ReactiveCommand.Create(this.WhenAnyValue(x => x.EnableInterface));

            OKCommand.Subscribe(_ =>
            {
                Workspace.Instance.InvalidateCodeAnalysis();
                this.Close();
            });

            EnableInterface = true;
        }

        private void AddPackages(Repository repo)
        {
            foreach (var package in repo.Packages)
            {
                AvailablePackages.Add(package);
            }
        }

        private async void DownloadCatalog()
        {
            foreach (var packageSource in PackageSources.Instance.Sources)
            {
                Repository repo = null;

                await Task.Factory.StartNew(() => repo = packageSource.DownloadCatalog());

                if(repo != null)
                {
                    AddPackages(repo);
                }
            }
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

        private bool enableInterface = true;
        public bool EnableInterface
        {
            get { return enableInterface; }
            set
            {
                OKButtonVisible = value;
                CancelButtonVisible = value;

                this.RaiseAndSetIfChanged(ref enableInterface, value);
            }
        }


        private bool ProgressUpdate(LibGit2Sharp.TransferProgress progress)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Status = string.Format("Bytes: {0}, Objects {1}", progress.ReceivedBytes, progress.ReceivedObjects);
            });

            return true;
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { this.RaiseAndSetIfChanged(ref status, value); }
        }

        private PackageReference selectedPackage;
        public PackageReference SelectedPackage
        {
            get { return selectedPackage; }
            set
            {
                value.DownloadInfo();

                this.RaiseAndSetIfChanged(ref selectedPackage, value);
                this.RaisePropertyChanged(() => ButtonText);
            }
        }


        private ObservableCollection<PackageReference> availablePackage;
        public ObservableCollection<PackageReference> AvailablePackages
        {
            get { return availablePackage; }
            set { this.RaiseAndSetIfChanged(ref availablePackage, value); }
        }

        public ReactiveCommand<object> InstallCommand { get; private set; }
        public override ReactiveCommand<object> OKCommand { get; protected set; }
    }
}
