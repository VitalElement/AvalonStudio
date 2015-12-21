namespace AvalonStudio.Controls.ViewModels
{
    using Projects;
    using ReactiveUI;
    using System;

    class StandardProjectViewModel : ProjectViewModel
    {
        public StandardProjectViewModel(IProject model) : base(model)
        {
           
        }

        public ReactiveCommand<object> SetDefaultProjectCommand { get; private set; }
        public ReactiveCommand<object> ConfigureCommand { get; private set; }
    }
}
