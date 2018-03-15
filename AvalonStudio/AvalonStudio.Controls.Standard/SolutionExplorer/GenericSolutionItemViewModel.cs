using AvalonStudio.Projects;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class SolutionItemViewModel<T> : SolutionItemViewModel where T : ISolutionItem
    {
        public SolutionItemViewModel(ISolutionParentViewModel parent, T model) : base(parent)
        {
            Model = model;
        }

        public override bool CanRename => Model.CanRename;

        public override string Title
        {
            get => Model.Name;
            set
            {
                if (Model.CanRename && Title != value)
                {
                    if (!string.IsNullOrEmpty(value))
                    {
                        Model.Name = value;
                    }
                    else
                    {
                        throw new Exception("Name must have a length of at least 1.");
                    }
                }
            }
        }

        protected new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }
    }
}
