namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System;
    using System.IO;
    using System.Reactive;

    public abstract class ProjectItemViewModel<T> : ProjectItemViewModel where T : IProjectItem
    {
        public ProjectItemViewModel(T model)
        {
            Model = model;

            RemoveItemCommand = ReactiveCommand.Create(() =>
            {
            });

            OpenInExplorerCommand = ReactiveCommand.Create(() =>
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
        }

        private new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        public override string Title
        {
            get { return Model.Name; }
            set
            {
                if (Model.CanRename && !string.IsNullOrEmpty(value))
                {
                    Model.Name = value;
                }

                this.RaisePropertyChanged();
            }
        }

        public int NumberOfSelections { get; set; }

        public string TitleWithoutExtension
        {
            get { return Path.GetFileNameWithoutExtension(Title); }
        }

        public ReactiveCommand<Unit, Unit> RemoveItemCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenInExplorerCommand { get; protected set; }
        public ReactiveCommand<Unit, Unit> DeleteCommand { get; protected set; }
    }
}
