using Avalonia.Media;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class SolutionItemViewModel : ViewModel
    {
        public SolutionItemViewModel (ISolutionParentViewModel parent)
        {
            Parent = parent;
        }

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
