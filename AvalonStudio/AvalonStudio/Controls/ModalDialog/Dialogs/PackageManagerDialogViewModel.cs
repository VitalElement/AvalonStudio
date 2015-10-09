namespace AvalonStudio.Controls.ViewModels
{
    using Models.PackageManager;
    using MVVM;
    using Perspex.Threading;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;

    public class PackageManagerDialogViewModel : ModalDialogViewModelBase
    {        
        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            this.AvailablePackages = new ObservableCollection<Package>();

            DownloadCatalog();

            InstallCommand = ReactiveCommand.Create();
            InstallCommand.Subscribe(async (o) =>
            {
                EnableInterface = false;

                try
                {
                    var fullPackage = await SelectedPackage.DownloadPackage(ProgressUpdate);

                    if (fullPackage.Install())
                    {
                        Status = "Package Installed Successfully.";
                    }
                    else
                    {
                        Status = "An error occurred trying to install package.";
                    }
                }
                catch (Exception e)
                {
                    Status = "An error occurred trying to install package. " + e.Message;
                }

                EnableInterface = true;

            });

            OKCommand = ReactiveCommand.Create(this.WhenAnyValue(x=>x.EnableInterface));

            OKCommand.Subscribe(_ =>
            {
                Workspace.This.InvalidateCodeAnalysis();
                this.Close();                
            });

            EnableInterface = true;
        }

        private async void DownloadCatalog()
        {
            var repo = await Repository.DownloadCatalog();

            foreach (var p in repo.Packages)
            {
                AvailablePackages.Add(p);
            }
        }

        public string ButtonText
        {
            get
            {
                if (selectedPackage != null)
                {
                    if (selectedPackage.IsInstalled)
                    {
                        return "Update";
                    }
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

        private Package selectedPackage;
        public Package SelectedPackage
        {
            get { return selectedPackage; }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedPackage, value);
                this.RaisePropertyChanged(() => ButtonText);
            }
        }


        private ObservableCollection<Package> availablePackage;
        public ObservableCollection<Package> AvailablePackages
        {
            get { return availablePackage; }
            set { this.RaiseAndSetIfChanged(ref availablePackage, value); }
        }

        public ReactiveCommand<object> InstallCommand { get; private set; }
        public override ReactiveCommand<object> OKCommand { get; protected set; }
    }
}
