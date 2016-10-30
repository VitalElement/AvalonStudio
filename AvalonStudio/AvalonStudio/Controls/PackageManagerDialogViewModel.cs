using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.MVVM;
using AvalonStudio.Repositories;
using AvalonStudio.Utils;
using ReactiveUI;

namespace AvalonStudio.Controls
{
	public class PackageManagerDialogViewModel : ModalDialogViewModelBase, IConsole
	{
		private ObservableCollection<PackageReference> availablePackage;

		private bool enableInterface = true;

		private PackageReference selectedPackage;

		private PackageIndex selectedPackageIndex;

		private string selectedTag;

		private string status;

		public PackageManagerDialogViewModel()
			: base("Packages")
		{
			AvailablePackages = new ObservableCollection<PackageReference>();

			DownloadCatalog();

			InstallCommand = ReactiveCommand.Create();
			InstallCommand.Subscribe(async o =>
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

		public PackageReference SelectedPackage
		{
			get { return selectedPackage; }
			set
			{
                if (value != null)
                {
                    GetPackageInfo(value);
                }

				this.RaiseAndSetIfChanged(ref selectedPackage, value);
				this.RaisePropertyChanged(() => ButtonText);
			}
		}

		public PackageIndex SelectedPackageIndex
		{
			get { return selectedPackageIndex; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedPackageIndex, value);
				SelectedTag = value.Tags.FirstOrDefault();
			}
		}

		public string SelectedTag
		{
			get { return selectedTag; }
			set { this.RaiseAndSetIfChanged(ref selectedTag, value); }
		}

		public ObservableCollection<PackageReference> AvailablePackages
		{
			get { return availablePackage; }
			set { this.RaiseAndSetIfChanged(ref availablePackage, value); }
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

				if (repo != null)
				{
					AddPackages(repo);
				}
			}
		}


		private async void GetPackageInfo(PackageReference reference)
		{
			try
			{
				SelectedPackageIndex = await reference.DownloadInfoAsync();
			}
			catch (Exception)
			{
			}
		}
	}
}