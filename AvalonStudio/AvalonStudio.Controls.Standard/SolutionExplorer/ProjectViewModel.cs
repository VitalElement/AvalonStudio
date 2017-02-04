using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public abstract class ProjectViewModel : ViewModel<IProject>
	{
		private readonly IShell shell;
		private SolutionViewModel solutionViewModel;
        private ProjectConfigurationDialogViewModel configuration;

		private bool visibility;

		public ProjectViewModel(SolutionViewModel solutionViewModel, IProject model)
			: base(model)
		{
			shell = IoC.Get<IShell>();

			Items = new ObservableCollection<ProjectItemViewModel>();

			Items.BindCollections(model.Items, p => { return ProjectItemViewModel.Create(p); }, (pivm, p) => pivm.Model == p);

			ConfigureCommand = ReactiveCommand.Create();

			ConfigureCommand.Subscribe(o =>
			{
                if (configuration == null)
                {
                    configuration = new ProjectConfigurationDialogViewModel(model, () =>
                    {
                        configuration = null;
                    });

                    shell.AddDocument(configuration);
                }
                else
                {
                    shell.SelectedDocument = configuration;
                }
				//shell.ModalDialog.ShowDialog();
			});

			DebugCommand = ReactiveCommand.Create();
			DebugCommand.Subscribe(_ =>
			{
				//shell.Debug(model);                
			});

			BuildCommand = ReactiveCommand.Create();
			BuildCommand.Subscribe(o => { shell.Build(model); });

			CleanCommand = ReactiveCommand.Create();

			CleanCommand.Subscribe(o => { shell.Clean(model); });

			ManageReferencesCommand = ReactiveCommand.Create();

			ManageReferencesCommand.Subscribe(o => { });

			SetProjectCommand = ReactiveCommand.Create();

			SetProjectCommand.Subscribe(o =>
			{
				model.Solution.StartupProject = model;
				model.Solution.Save();

				shell.InvalidateCodeAnalysis();

				foreach (var project in solutionViewModel.Projects)
				{
					project.Invalidate();
				}
			});

			OpenInExplorerCommand = ReactiveCommand.Create();
			OpenInExplorerCommand.Subscribe(o => { Process.Start(new ProcessStartInfo { FileName = Model.CurrentDirectory, UseShellExecute = true }); });

			NewItemCommand = ReactiveCommand.Create();
			NewItemCommand.Subscribe(_ =>
			{
				shell.ModalDialog = new NewItemDialogViewModel(model);
				shell.ModalDialog.ShowDialog();
			});

			RemoveCommand = ReactiveCommand.Create();
			RemoveCommand.Subscribe(async o =>
			{
                await shell.CloseDocumentsForProjectAsync(Model);
				Model.Solution.RemoveProject(Model);
				Model.Solution.Save();
			});
		}

		public bool IsExpanded { get; set; }

		public string Title
		{
			get { return Model.Name; }
		}

		public bool IsVisible
		{
			get { return !Model.Hidden; }
		}

		public ObservableCollection<ProjectItemViewModel> Items { get; }

		public ReactiveCommand<object> BuildCommand { get; protected set; }
		public ReactiveCommand<object> CleanCommand { get; protected set; }
		public ReactiveCommand<object> DebugCommand { get; protected set; }
		public ReactiveCommand<object> ManageReferencesCommand { get; protected set; }
		public ReactiveCommand<object> RemoveCommand { get; protected set; }
		public ReactiveCommand<object> ConfigureCommand { get; }
		public ReactiveCommand<object> SetProjectCommand { get; }
		public ReactiveCommand<object> OpenInExplorerCommand { get; }
		public ReactiveCommand<object> NewItemCommand { get; }

		public bool Visibility
		{
			get { return visibility; }
			set
			{
				visibility = value;
				this.RaisePropertyChanged();
			}
		}

		public FontWeight FontWeight
		{
			get
			{
				if (Model == Model.Solution.StartupProject)
				{
					return FontWeight.Bold;
				}

                return FontWeight.Light;
			}
		}

		public ObservableCollection<string> IncludePaths { get; private set; }

		public static ProjectViewModel Create(SolutionViewModel solutionViewModel, IProject model)
		{
			ProjectViewModel result = new StandardProjectViewModel(solutionViewModel, model);

			return result;
		}
	}
}