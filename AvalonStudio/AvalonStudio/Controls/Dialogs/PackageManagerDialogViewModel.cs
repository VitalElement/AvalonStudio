namespace AvalonStudio.Controls.ViewModels
{
    using System;
    using MVVM;
    using Perspex.Threading;
    using ReactiveUI;
    using Repositories;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Utils;

    public class PackageManagerDialogViewModel : ModalDialogViewModelBase, IConsole
    {
        public PackageManagerDialogViewModel()
            : base("Packages")
        {
            this.AvailablePackages = new ObservableCollection<PackageReference>();

            DownloadCatalog();

            InstallCommand = ReactiveCommand.Create();
            InstallCommand.Subscribe(async (o) =>
            {
                EnableInterface = false;

                try
                {
                    await SelectedPackageIndex.Synchronize(SelectedTag, this);

                    //if (fullPackage.Install())
                    //{
                    //    Status = "Package Installed Successfully.";
                    //}
                    //else
                    //{
                    //    Status = "An error occurred trying to install package.";
                    //}
                }
                catch (Exception e)
                {
                    Status = "An error occurred trying to install package. " + e.Message;
                }

                EnableInterface = true;

            });

            OKCommand = ReactiveCommand.Create(this.WhenAnyValue(x => x.EnableInterface));

            OKCommand.Subscribe(_ =>
            {
                WorkspaceViewModel.Instance.InvalidateCodeAnalysis();
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
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Status = data;
            });
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

        private string status;
        public string Status
        {
            get { return status; }
            set { this.RaiseAndSetIfChanged(ref status, value); }
        }


        private  async void GetPackageInfo (PackageReference reference)
        {
            try
            {
                SelectedPackageIndex = await reference.DownloadInfoAsync();
            }
            catch (Exception e)
            {
                
            }
        }

        private PackageReference selectedPackage;
        public PackageReference SelectedPackage
        {
            get { return selectedPackage; }
            set
            {
                GetPackageInfo(value);

                this.RaiseAndSetIfChanged(ref selectedPackage, value);
                this.RaisePropertyChanged(() => ButtonText);
            }
        }

        private PackageIndex selectedPackageIndex;
        public PackageIndex SelectedPackageIndex
        {
            get { return selectedPackageIndex; }
            set { this.RaiseAndSetIfChanged(ref selectedPackageIndex, value); SelectedTag = value.Tags.FirstOrDefault(); }
        }

        private string selectedTag;
        public string SelectedTag
        {
            get { return selectedTag; }
            set { this.RaiseAndSetIfChanged(ref selectedTag, value); }
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
