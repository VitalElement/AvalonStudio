﻿namespace AvalonStudio.Controls.Standard.ViewModels
{
    using Extensibility.Dialogs;
    using Perspex.Controls;
    using Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;

    public class ProjectConfigurationDialogViewModel : ModalDialogViewModelBase
    {
        public ProjectConfigurationDialogViewModel() : base("Project Properties", true, false)
        {

        }

        public ProjectConfigurationDialogViewModel(IProject project, Action onClose)
            : base("Project Configuration", true, false)
        {
            this.configPages = new List<TabItem>();
            configPages.AddRange(project.ConfigurationPages);

            OKCommand = ReactiveCommand.Create();

            OKCommand.Subscribe((o) =>
            {
                onClose();
                this.Close();
            });
        }

        private void OnPropertyChanged()
        {

        }

        private object compileContent;
        public object CompileContent
        {
            get { return compileContent; }
            set { compileContent = value; }
        }

        private List<TabItem> configPages;
        public List<TabItem> ConfigPages
        {
            get { return configPages; }
            set { configPages = value; }
        }

        private void SetExecutionOptionsVisibility(bool visible)
        {
            if (visible)
            {
                //ExecutableOptionsVisibility = Visibility.Visible;
            }
            else
            {
                //ExecutableOptionsVisibility = Visibility.Collapsed;
            }
        }

        private bool executableOptionsVisibility;
        public bool ExecutableOptionsVisibility
        {
            get { return executableOptionsVisibility; }
            set { executableOptionsVisibility = value; OnPropertyChanged(); }
        }
    }
}
