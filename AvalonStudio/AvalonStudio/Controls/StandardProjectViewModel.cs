namespace AvalonStudio.Controls.ViewModels
{
    using Projects;
    using ReactiveUI;
    using System;

    class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(IProject model) : base(model)
        {
            if(model.Solution.StartupProject == model)
            {
                IsExpanded = true;
            }
        }

        public ReactiveCommand<object> SetDefaultProjectCommand { get; private set; }

        public ReactiveCommand<object> Remove { get; private set; }
        
    }
}
