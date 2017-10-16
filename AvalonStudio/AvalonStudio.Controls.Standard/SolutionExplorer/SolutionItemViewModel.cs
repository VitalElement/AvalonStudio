using Avalonia.Media;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class SolutionItemViewModel : ViewModel
    {
        private bool _inEditMode;

        public SolutionItemViewModel (ISolutionParentViewModel parent)
        {
            Parent = parent;
        }

        public virtual bool InEditMode
        {
            get { return _inEditMode; }
            set
            {
                this.RaiseAndSetIfChanged(ref _inEditMode, value);
            }
        }

        public virtual bool CanRename { get; }

        public virtual string Title { get; set; }

        public abstract DrawingGroup Icon { get; }

        public ISolutionParentViewModel Parent { get; protected set; }

        public static SolutionItemViewModel Create(ISolutionParentViewModel parent, ISolutionItem item)
        {
            SolutionItemViewModel result = null;

            if (item is ISolutionFolder folder)
            {
                result = new SolutionFolderViewModel(parent, folder);
            }
            else if(item is IProject project)
            {
                result = new StandardProjectViewModel(parent, project);
            }
            else
            {
                throw new Exception("Unrecognised model type");
            }

            return result;
        }
    }
}
