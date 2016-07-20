using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public class SolutionExplorerViewModel : ToolViewModel, IExtension
	{
		public const string ToolId = "CIDSEVM00";

		private ISolution model;

		private ProjectItemViewModel selectedItem;


		private IProject selectedProject;
		private IShell shell;

		private ObservableCollection<SolutionViewModel> solution;

		public SolutionExplorerViewModel()
		{
			Title = "Solution Explorer";
			solution = new ObservableCollection<SolutionViewModel>();
		}

		public ISolution Model
		{
			get { return model; }
			set
			{
				Projects = new ObservableCollection<ProjectItemViewModel>();
				model = value;

				if (Model != null)
				{
					if (Model.Projects.Count > 0)
					{
						SelectedProject = Model.StartupProject;
					}

					var sol = new ObservableCollection<SolutionViewModel>();
					sol.Add(new SolutionViewModel(model));

					Solution = sol;
				}
				else
				{
					Solution = null;
				}

				this.RaisePropertyChanged();
				this.RaisePropertyChanged(nameof(Projects));
			}
		}

		public ObservableCollection<SolutionViewModel> Solution
		{
			get { return solution; }
			set { this.RaiseAndSetIfChanged(ref solution, value); }
		}

		public IProject SelectedProject
		{
			get { return selectedProject; }

			set
			{
				selectedProject = value;
				this.RaisePropertyChanged();
			}
		}

		public ProjectItemViewModel SelectedItem
		{
			get { return selectedItem; }
			set
			{
				this.RaiseAndSetIfChanged(ref selectedItem, value);

				if (value is SourceFileViewModel)
				{
					// might need wait here?
					Task.Factory.StartNew(
						async () => { await shell.OpenDocument((ISourceFile) (value as SourceFileViewModel).Model, 1); });
				}
			}
		}

		public ObservableCollection<ProjectItemViewModel> Projects { get; set; }

		public override Location DefaultLocation
		{
			get { return Location.Right; }
		}

		public void BeforeActivation()
		{
		}

		public void Activation()
		{
			shell = IoC.Get<IShell>();

			shell.SolutionChanged += (sender, e) => { Model = shell.CurrentSolution; };
		}
	}
}