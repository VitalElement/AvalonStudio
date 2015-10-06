namespace AvalonStudio.Controls.ViewModels
{
    using Models.PackageManager;
    using Perspex.MVVM;
    using Perspex.Threading;
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using System.Windows.Input;

    public class PackageManagerDialogViewModel : ModalDialogViewModelBase
    {

        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            this.AvailablePackages = new ObservableCollection<Package>();

            Task.Factory.StartNew(async () =>
            {
                var repo = await Repository.DownloadCatalog();

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    foreach (var p in repo.Packages)
                    {
                        AvailablePackages.Add(p);
                    }
                });
            });

            this.InstallCommand = new RoutingCommand(async (o) =>
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

            }, (o) => SelectedPackage != null && EnableInterface);

            OKCommand = new RoutingCommand((o) =>
            {
                Workspace.This.InvalidateCodeAnalysis();
                this.Close();
            }, (o) => EnableInterface);

            EnableInterface = true;
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
                enableInterface = value;

                if (value)
                {
                    OKButtonVisible = true;
                    CancelButtonVisible = true;
                }
                else
                {
                    OKButtonVisible = false;
                    CancelButtonVisible = false;
                }

                OnPropertyChanged();
            }
        }


        private bool ProgressUpdate(LibGit2Sharp.TransferProgress progress)
        {
            Status = string.Format("Bytes: {0}, Objects {1}", progress.ReceivedBytes, progress.ReceivedObjects);
            return true;
        }

        private string status;
        public string Status
        {
            get { return status; }
            set { status = value; OnPropertyChanged(); }
        }

        private Package selectedPackage;
        public Package SelectedPackage
        {
            get { return selectedPackage; }
            set { selectedPackage = value; OnPropertyChanged(); OnPropertyChanged(() => ButtonText); }
        }


        private ObservableCollection<Package> availablePackage;
        public ObservableCollection<Package> AvailablePackages
        {
            get { return availablePackage; }
            set { availablePackage = value; OnPropertyChanged(); }
        }

        public ICommand InstallCommand { get; private set; }
        public override ICommand OKCommand { get; protected set; }
    }
}
