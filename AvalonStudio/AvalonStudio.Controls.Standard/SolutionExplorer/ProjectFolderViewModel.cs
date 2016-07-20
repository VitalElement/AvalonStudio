using System;
using System.Collections.ObjectModel;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	internal class ProjectFolderViewModel : ProjectItemViewModel<IProjectFolder>
	{
		public ProjectFolderViewModel(IProjectFolder model)
			: base(model)
		{
			Items = new ObservableCollection<ProjectItemViewModel>();
			Items.BindCollections(model.Items, p => { return Create(p); }, (pivm, p) => pivm.Model == p);

			NewItemCommand = ReactiveCommand.Create();
			NewItemCommand.Subscribe(_ =>
			{
				// ShellViewModel.Instance.ModalDialog = new NewItemDialogViewModel(model);
				//ShellViewModel.Instance.ModalDialog.ShowDialog();
			});

			RemoveCommand = ReactiveCommand.Create();
			RemoveCommand.Subscribe(_ => { model.Project.RemoveFolder(model); });
		}

		public ObservableCollection<ProjectItemViewModel> Items { get; }

		public ReactiveCommand<object> NewItemCommand { get; }
		public ReactiveCommand<object> RemoveCommand { get; }

		public static ProjectFolderViewModel Create(IProjectFolder model)
		{
			return new ProjectFolderViewModel(model);
		}
	}
}