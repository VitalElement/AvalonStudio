namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using System;
    using System.IO;
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System.Diagnostics;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public static ProjectItemViewModel Create(IProjectItem item)
        {
            ProjectItemViewModel result = null;

            if (item is IProjectFolder)
            {
                result = new ProjectFolderViewModel(item as IProjectFolder);
            }

            if (item is ISourceFile)
            {
                result = new SourceFileViewModel(item as ISourceFile);
            }

            if (item is IReferenceFolder)
            {
                result = new ReferenceFolderViewModel(item as IReferenceFolder);
            }

            return result;
        }
    }

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
                    Process.Start((model as IProjectFolder).Location);
                }
                else if (model is IProjectItem)
                {
                    Process.Start((model as IProjectItem).Parent.Location);
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
            get { return isEditingTitle; }
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