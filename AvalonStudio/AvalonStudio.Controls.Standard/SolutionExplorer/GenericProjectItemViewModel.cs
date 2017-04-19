namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System;
    using System.IO;

    public abstract class ProjectItemViewModel<T> : ProjectItemViewModel where T : IProjectItem
    {
        private bool isEditingTitle;

        private bool labelVisibility;

        private bool textBoxVisibility;

        public ProjectItemViewModel(T model)
        {
            Model = model;

            RemoveItemCommand = ReactiveCommand.Create();
            RemoveItemCommand.Subscribe(o =>
            {
            });

            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe(o =>
            {
                if (model is IProjectFolder)
                {
                    Platform.OpenFolderInExplorer((model as IProjectFolder).Location);
                }
                else if (model is IProjectItem)
                {
                    Platform.OpenFolderInExplorer((model as IProjectItem).Parent.Location);
                }
            });

            textBoxVisibility = false;
            labelVisibility = true;
        }

        private new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        public string Title
        {
            get { return Model.Name; }
            // set { this.Model.Name = value; this.RaisePropertyChanged(); IsEditingTitle = false; }
        }

        public int NumberOfSelections { get; set; }

        public string TitleWithoutExtension
        {
            get { return Path.GetFileNameWithoutExtension(Title); }
        }

        public bool IsEditingTitle
        {
            get
            {
                return isEditingTitle;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref isEditingTitle, value);
                LabelVisibility = !value;
                TextBoxVisibility = value;
            }
        }

        public ReactiveCommand<object> RemoveItemCommand { get; }
        public ReactiveCommand<object> ToggleEditingModeCommand { get; }
        public ReactiveCommand<object> OpenInExplorerCommand { get; protected set; }

        public bool TextBoxVisibility
        {
            get { return textBoxVisibility; }
            set { this.RaiseAndSetIfChanged(ref textBoxVisibility, value); }
        }

        public bool LabelVisibility
        {
            get { return labelVisibility; }
            set { this.RaiseAndSetIfChanged(ref labelVisibility, value); }
        }
    }
}
