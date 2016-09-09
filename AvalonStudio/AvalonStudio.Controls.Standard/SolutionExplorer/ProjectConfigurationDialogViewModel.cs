using System;
using System.Collections.Generic;
using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Projects;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
	public class ProjectConfigurationDialogViewModel : DocumentTabViewModel
	{
		private bool executableOptionsVisibility;

		public ProjectConfigurationDialogViewModel()
		{
		}

		public ProjectConfigurationDialogViewModel(IProject project, Action onClose)
		{
            Title = project.Name;

			ConfigPages = new List<object>();
			ConfigPages.AddRange(project.ConfigurationPages);

            CloseCommand.Subscribe(_ =>
            {
                onClose();
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