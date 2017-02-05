using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Avalonia.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public class SolutionViewModel : ViewModel<ISolution>
	{
		private ObservableCollection<ProjectViewModel> projects;
		private readonly IShell shell;

		public SolutionViewModel(ISolution model) : base(model)
		{
			shell = IoC.Get<IShell>();

			Projects = new ObservableCollection<ProjectViewModel>();
			IsExpanded = true;

			Projects.BindCollections(model.Projects, p => { return ProjectViewModel.Create(this, p); },
				(pvm, p) => pvm.Model == p);

			NewProjectCommand = ReactiveCommand.Create();
			NewProjectCommand.Subscribe(o =>
			{
				shell.ModalDialog = new NewProjectDialogViewModel(model);
				shell.ModalDialog.ShowDialog();
			});

			AddExistingProjectCommand = ReactiveCommand.Create();
			AddExistingProjectCommand.Subscribe(async o =>
			{
				var dlg = new OpenFileDialog();
				dlg.Title = "Open Project";

				var extensions = new List<string>();

				foreach (var projectType in shell.ProjectTypes)
				{
					extensions.AddRange(projectType.Extensions);
				}

				dlg.Filters.Add(new FileDialogFilter {Name = "AvalonStudio Project", Extensions = extensions});
				dlg.InitialFileName = string.Empty;
				dlg.InitialDirectory = Model.CurrentDirectory;
				dlg.AllowMultiple = false;

				var result = await dlg.ShowAsync();

				if (result != null && result.Length == 1)
				{
					var proj = Solution.LoadProjectFile(model, result[0]);

					if (proj != null)
					{
						model.AddProject(proj);
						model.Save();
					}
				}
			});

			OpenInExplorerCommand = ReactiveCommand.Create();
			OpenInExplorerCommand.Subscribe(o => { Process.Start(new ProcessStartInfo { UseShellExecute = true, FileName = model.CurrentDirectory }); });

			ConfigurationCommand = ReactiveCommand.Create();
			ConfigurationCommand.Subscribe(o =>
			{
				//Workspace.Instance.ModalDialog = new SolutionConfigurationDialogViewModel(Workspace.Instance.SolutionExplorer.Model);
				//Workspace.Instance.ModalDialog.ShowDialog();
			});

			BuildSolutionCommand = ReactiveCommand.Create();
			BuildSolutionCommand.Subscribe(o => { BuildSolution(); });

			CleanSolutionCommand = ReactiveCommand.Create();
			CleanSolutionCommand.Subscribe(o => { CleanSolution(); });

			RebuildSolutionCommand = ReactiveCommand.Create();
			RebuildSolutionCommand.Subscribe(o =>
			{
				CleanSolution();

				BuildSolution();
			});

			RunAllTestsCommand = ReactiveCommand.Create();
			RunAllTestsCommand.Subscribe(o => { RunTests(); });
		}

		public bool IsExpanded { get; set; }

		public ObservableCollection<ProjectViewModel> Projects
		{
			get { return projects; }
			set { this.RaiseAndSetIfChanged(ref projects, value); }
		}

		public ReactiveCommand<object> ConfigurationCommand { get; }
		public ReactiveCommand<object> CleanSolutionCommand { get; }
		public ReactiveCommand<object> BuildSolutionCommand { get; }
		public ReactiveCommand<object> RebuildSolutionCommand { get; }
		public ReactiveCommand<object> RunAllTestsCommand { get; }
		public ReactiveCommand<object> NewProjectCommand { get; }
		public ReactiveCommand<object> AddExistingProjectCommand { get; }
		public ReactiveCommand<object> OpenInExplorerCommand { get; }

		public string Title
		{
			get
			{
				if (Model != null)
				{
					return string.Format("Solution '{0}' ({1} {2})", Model.Name, Model.Projects.Count, StringProjects);
				}
				return string.Empty;
			}
		}

		private string StringProjects
		{
			get
			{
				if (Model.Projects.Count == 1)
				{
					return "project";
				}
				return "projects";
			}
		}


		private async void CleanSolution()
		{
			// await Model.Solution.DefaultProject.SelectedConfiguration.ToolChain.Clean(Workspace.Instance.StudioConsole, Model.Solution.DefaultProject, Workspace.Instance.ProcessCancellationToken);
		}

		private async void BuildSolution()
		{
			//Workspace.Instance.SaveAllCommand.Execute(null);
			//await Model.Solution.DefaultProject.SelectedConfiguration.ToolChain.Build(Workspace.Instance.StudioConsole, this.Model.Solution.DefaultProject, Workspace.Instance.ProcessCancellationToken);
		}

		private async void RunTests()
		{
			//var testProjects = Model.Solution.LoadedProjects.OfType<CatchTestProject>();
			//var tests = new List<Test>();

			//foreach(var testProject in testProjects)
			//{
			//    await testProject.Build(Workspace.Instance.StudioConsole, Workspace.Instance.ProcessCancellationToken);

			//    tests.AddRange(testProject.EnumerateTests(Workspace.Instance.StudioConsole));            
			//}

			//Workspace.Instance.TestRunner.AddTests(tests);
			//Workspace.Instance.TestRunner.RunTests();
		}

		//public override void Drop(ProjectItemViewModel item)
		//}

		//    return result;
		//    }
		//        result = true;
		//    {

		//    if (type == typeof(SolutionFolderViewModel))
		//    //}
		//    //    result = true;
		//    //{

		//    //if (type == typeof(TestProjectViewModel))
		//    }
		//        result = true;
		//    {

		//    if (type == typeof(StandardProjectViewModel))
		//    bool result = false;
		//{

		//public override bool CanAcceptDrop(Type type)
		//{
		//    var project = item as ProjectViewModel;

		//    if (project != null)
		//    {
		//        var parent = project.Model.Solution.GetParent(project.Model);

		//        parent.RemoveItem(project.Model);

		//        Model.AttachItem(project.Model);
		//    }
		//    else if (item is SolutionFolderViewModel)
		//    {
		//        var folder = item as SolutionFolderViewModel;

		//        var parent = folder.Model.Solution.GetParent(folder.Model);

		//        parent.DetachItem(folder.Model);

		//        Model.AttachItem(folder.Model);
		//    }
		//}
	}
}