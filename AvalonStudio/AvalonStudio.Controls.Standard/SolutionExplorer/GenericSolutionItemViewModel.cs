using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;

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
                if(Model.CanRename)
                {
                    Model.Name = value;
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
