using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains.Standard;
using ReactiveUI;

namespace AvalonStudio.Toolchains.STM32
{
	public class LinkerSettingsFormViewModel : HeaderedViewModel<IProject>
	{
		private bool discardUnusedSections;

		private string inRam1Size;

		private string inRam1Start;

		private string inRam2Size;

		private string inRam2Start;

		private string inRom1Size;

		private string inRom1Start;

		private string inRom2Size;

		private string inRom2Start;

		private int librarySelectedIndex;


		private ObservableCollection<string> linkedLibraries;

		private string linkerArguments;

		private string miscOptions;


		private bool notUseStandardStartup;

		private IProject project;

		private string scatterFile;

		private string selectedLinkedLibrary;
		private readonly LinkSettings settings = new LinkSettings();

		private bool useMemoryLayout = true;

		public LinkerSettingsFormViewModel(IProject project) : base("Linker", project)
		{
			try
			{
				try
				{
					settings = STM32GCCToolchain.GetSettings(project).LinkSettings;
				}
				catch (Exception e)
				{
					Model.ToolchainSettings.STM32ToolchainSettings = new STM32ToolchainSettings();
				}
			}
			catch (Exception e)
			{
				Model.ToolchainSettings.STM32ToolchainSettings = new STM32ToolchainSettings();
				settings = Model.ToolchainSettings.STM32ToolchainSettings.LinkSettings;
			}

			if (settings == null)
			{
				settings = new LinkSettings();
			}


			useMemoryLayout = settings.UseMemoryLayout;
			discardUnusedSections = settings.DiscardUnusedSections;
			notUseStandardStartup = settings.NotUseStandardStartupFiles;
			linkedLibraries = new ObservableCollection<string>(settings.LinkedLibraries);
			inRom1Start = string.Format("0x{0:X8}", settings.InRom1Start);
			inRom1Size = string.Format("0x{0:X8}", settings.InRom1Size);
			inRom2Start = string.Format("0x{0:X8}", settings.InRom2Start);
			inRom2Size = string.Format("0x{0:X8}", settings.InRom2Size);
			inRam1Start = string.Format("0x{0:X8}", settings.InRam1Start);
			inRam1Size = string.Format("0x{0:X8}", settings.InRam1Size);
			inRam2Start = string.Format("0x{0:X8}", settings.InRam2Start);
			inRam2Size = string.Format("0x{0:X8}", settings.InRam2Size);
			scatterFile = settings.ScatterFile;
			miscOptions = settings.MiscLinkerArguments;
			librarySelectedIndex = (int) settings.Library;

			//AddLinkedLibraryCommand = new RoutingCommand(AddLinkedLibrary);
			//RemoveLinkedLibraryCommand = new RoutingCommand(RemoveLinkedLibrary);
			//BrowseScatterFileCommand = new RoutingCommand(BrowseScatterFile);
			//EditScatterFileCommand = new RoutingCommand(EditScatterFile);

			UpdateLinkerString();
		}

		public ICommand AddLinkedLibraryCommand { get; private set; }
		public ICommand RemoveLinkedLibraryCommand { get; private set; }
		public ICommand BrowseScatterFileCommand { get; private set; }
		public ICommand EditScatterFileCommand { get; private set; }

		public int MemoryAreasVisible
		{
			get
			{
				if (useMemoryLayout)
				{
					return 195;
				}
				return 0;
			}
		}

		public int ScatterFileVisible
		{
			get
			{
				if (useMemoryLayout)
				{
					return 0;
				}
				return 85;
			}
		}

		public bool UseMemoryLayout
		{
			get { return useMemoryLayout; }
			set
			{
				this.RaiseAndSetIfChanged(ref useMemoryLayout, value);
				this.RaisePropertyChanged(nameof(MemoryAreasVisible));
				this.RaisePropertyChanged(nameof(ScatterFileVisible));
				UpdateLinkerString();
			}
		}

		public bool DiscardUnusedSections
		{
			get { return discardUnusedSections; }
			set
			{
				this.RaiseAndSetIfChanged(ref discardUnusedSections, value);
				UpdateLinkerString();
			}
		}

		public bool NotUseStandardStartup
		{
			get { return notUseStandardStartup; }
			set
			{
				this.RaiseAndSetIfChanged(ref notUseStandardStartup, value);
				UpdateLinkerString();
			}
		}

		public int LibrarySelectedIndex
		{
			get { return librarySelectedIndex; }
			set
			{
				this.RaiseAndSetIfChanged(ref librarySelectedIndex, value);
				UpdateLinkerString();
			}
		}


		public string[] LibraryOptions
		{
			get { return Enum.GetNames(typeof (LibraryType)); }
		}

		public string SelectedLinkedLibrary
		{
			get { return selectedLinkedLibrary; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedLinkedLibrary, value);
				UpdateLinkerString();
			}
		}

		public ObservableCollection<string> LinkedLibraries
		{
			get { return linkedLibraries; }
			set
			{
				this.RaiseAndSetIfChanged(ref linkedLibraries, value);
				UpdateLinkerString();
			}
		}

		public string InRom1Start
		{
			get { return inRom1Start; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRom1Start, value);
				UpdateLinkerString();
			}
		}

		public string InRom1Size
		{
			get { return inRom1Size; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRom1Size, value);
				UpdateLinkerString();
			}
		}

		public string InRom2Start
		{
			get { return inRom2Start; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRom2Start, value);
				UpdateLinkerString();
			}
		}

		public string InRom2Size
		{
			get { return inRom2Size; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRom2Size, value);
				UpdateLinkerString();
			}
		}

		public string InRam1Start
		{
			get { return inRam1Start; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRam1Start, value);
				UpdateLinkerString();
			}
		}

		public string InRam1Size
		{
			get { return inRam1Size; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRam1Size, value);
				UpdateLinkerString();
			}
		}

		public string InRam2Start
		{
			get { return inRam2Start; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRam2Start, value);
				UpdateLinkerString();
			}
		}

		public string InRam2Size
		{
			get { return inRam2Size; }
			set
			{
				this.RaiseAndSetIfChanged(ref inRam2Size, value);
				UpdateLinkerString();
			}
		}

		public string ScatterFile
		{
			get { return scatterFile; }
			set
			{
				this.RaiseAndSetIfChanged(ref scatterFile, value);
				UpdateLinkerString();
			}
		}

		public string MiscOptions
		{
			get { return miscOptions; }
			set
			{
				this.RaiseAndSetIfChanged(ref miscOptions, value);
				UpdateLinkerString();
			}
		}

		public string LinkerArguments
		{
			get { return linkerArguments; }
			set { this.RaiseAndSetIfChanged(ref linkerArguments, value); }
		}

		public void UpdateLinkerString()
		{
			Save();

			if (Model.ToolChain != null && Model.ToolChain is StandardToolChain)
			{
				LinkerArguments = (Model.ToolChain as StandardToolChain).GetLinkerArguments(Model as IStandardProject);
			}
		}

		private void EditScatterFile(object obj)
		{
			//throw new NotImplementedException();
		}

		private void BrowseScatterFile(object obj)
		{
			// throw new NotImplementedException();
		}

		public void Save()
		{
			settings.UseMemoryLayout = useMemoryLayout;
			settings.DiscardUnusedSections = discardUnusedSections;
			settings.NotUseStandardStartupFiles = notUseStandardStartup;
			settings.LinkedLibraries = linkedLibraries.ToList();
			settings.InRom1Start = Convert.ToUInt32(inRom1Start, 16);
			settings.InRom1Size = Convert.ToUInt32(inRom1Size, 16);
			settings.InRom2Start = Convert.ToUInt32(inRom2Start, 16);
			settings.InRom2Size = Convert.ToUInt32(inRom2Size, 16);
			settings.InRam1Start = Convert.ToUInt32(inRam1Start, 16);
			settings.InRam1Size = Convert.ToUInt32(inRam1Size, 16);
			settings.InRam2Start = Convert.ToUInt32(inRam2Start, 16);
			settings.InRam2Size = Convert.ToUInt32(inRam2Size, 16);
			settings.ScatterFile = scatterFile;
			settings.MiscLinkerArguments = miscOptions;
			settings.Library = (LibraryType) librarySelectedIndex;

			Model.ToolchainSettings.STM32ToolchainSettings.LinkSettings = settings;
			Model.Save();
		}

		private void AddLinkedLibrary(object param)
		{
			//var ofd = new OpenFileDialog();

			//ofd.InitialDirectory = project.CurrentDirectory;

			//if (ofd.ShowDialog() == DialogResult.OK)
			//{
			//    string newInclude = project.CurrentDirectory.MakeRelativePath(ofd.FileName);

			//    if (newInclude == string.Empty)
			//    {
			//        newInclude = "\\";
			//    }

			//    LinkedLibraries.Add(newInclude);

			//    UpdateLinkerString();
			//}
		}

		private void RemoveLinkedLibrary(object param)
		{
			LinkedLibraries.Remove(SelectedLinkedLibrary);
			UpdateLinkerString();
		}
	}
}