
namespace AvalonStudio.Controls.ViewModels
{
    using AvalonStudio.MVVM;
    using Perspex.Controls;
    using Projects;
    using Projects.Standard;
    using ReactiveUI;
    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Windows.Input;

    public abstract class ProjectItemViewModel : ViewModel
    {
        public static ProjectItemViewModel Create(IProjectItem item)
        {
            ProjectItemViewModel result = null;

            if (item is IProjectFolder)
            {
                result = new ProjectFolderViewModel(item as IProjectFolder) as ProjectItemViewModel<IProjectFolder>;
            }

            if(item is ISourceFile)
            {
                result = new SourceFileViewModel(item as ISourceFile);
            }

            if(item is ReferenceFolder)
            {
                result = new ReferenceFolderViewModel(item as ReferenceFolder);
            }

            return result;
        }        
    }

    public abstract class ProjectItemViewModel<T> : ProjectItemViewModel  where T : IProjectItem
    {
        public ProjectItemViewModel(T model)
        {
            Model = model;

            ToggleEditingModeCommand = ReactiveCommand.Create();

            ToggleEditingModeCommand.Subscribe(args =>
            {
                if (((object)WorkspaceViewModel.Instance.SolutionExplorer.SelectedItem) == (object)this && NumberOfSelections > 1)
                {
                    IsEditingTitle = (bool)args;
                }
            });

            RemoveItemCommand = ReactiveCommand.Create();
            RemoveItemCommand.Subscribe((o) =>
            {
                //if (model is EditorViewModel)
                //{
                //    //(this.model as EditorViewModel).CloseCommand.Execute (null);
                //}

                //if (model is ProjectItem)
                //{
                //    (model as ProjectItem).Container.RemoveItem(model as ProjectItem);
                //}
            });

            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe((o) =>
            {
                //if (model is ProjectItem)
                //{
                //    Process.Start((model as ProjectItem).CurrentDirectory);
                //}
            });

            textBoxVisibility = false;
            labelVisibility = true;
        }

        new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        public string Title
        {
            get { return this.Model.Name; }
           // set { this.Model.Name = value; this.RaisePropertyChanged(); IsEditingTitle = false; }
        }

        public void Invalidate()
        {
            this.RaisePropertyChanged();
        }

        public int NumberOfSelections { get; set; }

        public string TitleWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(Title);
            }
        }

        private bool isEditingTitle;
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

        public ReactiveCommand<object> RemoveItemCommand { get; private set; }
        public ReactiveCommand<object> ToggleEditingModeCommand { get; private set; }
        public ReactiveCommand<object> OpenInExplorerCommand { get; protected set; }

        private bool textBoxVisibility;
        public bool TextBoxVisibility
        {
            get { return textBoxVisibility; }
            set { this.RaiseAndSetIfChanged(ref textBoxVisibility, value); }
        }

        private bool labelVisibility;
        public bool LabelVisibility
        {
            get { return labelVisibility; }
            set { this.RaiseAndSetIfChanged(ref labelVisibility, value); }
        }


    }    
}
