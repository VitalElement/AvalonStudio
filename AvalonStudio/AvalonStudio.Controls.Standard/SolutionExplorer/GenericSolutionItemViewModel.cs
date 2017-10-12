using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public abstract class SolutionItemViewModel<T> : SolutionItemViewModel where T : ISolutionItem
    {
        public SolutionItemViewModel(T model)
        {
            Model = model;
        }

        protected new T Model
        {
            get { return (T)base.Model; }
            set { base.Model = value; }
        }

        public string Title => Model.Name;
        
    }
}
