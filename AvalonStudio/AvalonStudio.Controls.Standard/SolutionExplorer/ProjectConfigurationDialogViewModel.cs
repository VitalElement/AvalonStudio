using System;
using System.Collections.Generic;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public class ProjectConfigurationDialogViewModel : ModalDialogViewModelBase
	{
		private bool executableOptionsVisibility;

		public ProjectConfigurationDialogViewModel() : base("Project Properties", true, false)
		{
		}

		public ProjectConfigurationDialogViewModel(IProject project, Action onClose)
			: base("Project Configuration", true, false)
		{
			ConfigPages = new List<object>();
			ConfigPages.AddRange(project.ConfigurationPages);

			OKCommand = ReactiveCommand.Create();

			OKCommand.Subscribe(o =>
			{
				onClose();
				Close();
			});
		}

		public object CompileContent { get; set; }

		public List<object> ConfigPages { get; set; }

		public bool ExecutableOptionsVisibility
		{
			get { return executableOptionsVisibility; }
			set
			{
				executableOptionsVisibility = value;
				OnPropertyChanged();
			}
		}

		private void OnPropertyChanged()
		{
		}

		private void SetExecutionOptionsVisibility(bool visible)
		{
			if (visible)
			{
				//ExecutableOptionsVisibility = Visibility.Visible;
			}
		}
	}
}