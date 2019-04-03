using AvalonStudio.Projects;
using System;
using System.Collections.Generic;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class ProjectConfigurationDialogViewModel : DocumentTabViewModel
    {
        private bool executableOptionsVisibility;
        private Action _onClose;

        public ProjectConfigurationDialogViewModel()
        {
        }

        public ProjectConfigurationDialogViewModel(IProject project, Action onClose)
        {
            Title = project.Name;

            ConfigPages = new List<object>();

            var configPages = project.ConfigurationPages;

            if (configPages != null)
            {
                ConfigPages.AddRange(configPages);
            }

            _onClose = onClose;
        }

        public override bool OnClose()
        {
            bool result = base.OnClose();

            _onClose?.Invoke();

            return result;
        }

        public object CompileContent { get; set; }

        public List<object> ConfigPages { get; set; }

        public bool ExecutableOptionsVisibility
        {
            get
            {
                return executableOptionsVisibility;
            }
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