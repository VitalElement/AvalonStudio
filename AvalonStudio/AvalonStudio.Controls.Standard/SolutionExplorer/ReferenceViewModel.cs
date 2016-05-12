﻿namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;

    public class ReferenceViewModel : ViewModel<IProject>
    {
        public ReferenceViewModel(IProject model) : base (model)
        {

        }

        public string Name
        {
            get
            {
                return Model.Name;
            }
        }
    }
}
