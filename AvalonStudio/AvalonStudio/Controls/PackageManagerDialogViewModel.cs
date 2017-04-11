using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.MVVM;
using AvalonStudio.Utils;
using ReactiveUI;
using NuGet.Protocol.Core.Types;
using AvalonStudio.Packages;
using System.Collections.Generic;

namespace AvalonStudio.Controls
{
    public class VersionInfoViewModel : ViewModel<VersionInfo>
    {
        public VersionInfoViewModel(VersionInfo model) : base(model)
        {

        }

        public string Title => Model.Version.ToNormalizedString();
    }

    public class PackageReferenceViewModel : ViewModel<NuGet.Packaging.PackageReference>
    {
        public PackageReferenceViewModel(NuGet.Packaging.PackageReference model) : base(model)
        {

        }

        public string Title => Model.PackageIdentity.Id;
    }


	public class PackageManagerDialogViewModel : ModalDialogViewModelBase, IConsole
	{
		private ObservableCollection<IPackageSearchMetadata> availablePackages;

		private bool enableInterface = true;

		private IPackageSearchMetadata selectedPackage;

        private PackageManager _packageManager;

		//private PackageIndex selectedPackageIndex;

		private VersionInfoViewModel selectedVersion;

		private string status;

		public PackageManagerDialogViewModel()
			: base("Packages")
		{
            _packageManager = new PackageManager();            

			AvailablePackages = new ObservableCollection<IPackageSearchMetadata>();

			Dispatcher.UIThread.InvokeAsync (async () => {
                InstalledPackages = new  ObservableCollection<PackageReferenceViewModel>((await _packageManager.ListInstalledPackages()).Select(pr=>new PackageReferenceViewModel(pr)));
                
				await DownloadCatalog ();
			});

			InstallCommand = ReactiveCommand.Create();
            InstallCommand.Subscribe(async _ => 
            {
                await _packageManager.InstallPackage(selectedPackage.Identity.Id, selectedPackage.Identity.Version.ToFullString());
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
			get { return enableInterface; }
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
			get { return selectedPackage; }
			set
			{
                if (value != null)
                {
                    Task.Run(async () => { Versions = (await value.GetVersionsAsync()).Select(vi=>new VersionInfoViewModel(vi)); });                   
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


        //public PackageIndex SelectedPackageIndex
        //{
        //	get { return selectedPackageIndex; }
        //	set
        //	{
        //		this.RaiseAndSetIfChanged(ref selectedPackageIndex, value);
        //		SelectedTag = value.Tags.FirstOrDefault();
        //	}
        //}

        public VersionInfoViewModel SelectedVersion
		{
			get { return selectedVersion; }
			set
            {
                this.RaiseAndSetIfChanged(ref selectedVersion, value);            }
		}

		public ObservableCollection<IPackageSearchMetadata> AvailablePackages
		{
			get { return availablePackages; }
			set { this.RaiseAndSetIfChanged(ref availablePackages, value); }
		}

        private ObservableCollection<PackageReferenceViewModel> installedPackages;

        public ObservableCollection<PackageReferenceViewModel> InstalledPackages
        {
            get { return installedPackages; }
            set { this.RaiseAndSetIfChanged(ref installedPackages, value); }
        }


        public ReactiveCommand<object> InstallCommand { get; }
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
	}
}