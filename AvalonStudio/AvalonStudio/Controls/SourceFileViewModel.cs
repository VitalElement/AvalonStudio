namespace AvalonStudio.Controls.ViewModels
{
    using Projects;
    using System;

    public class SourceFileViewModel : ProjectItemViewModel<ISourceFile>
    {
        public SourceFileViewModel(ISourceFile model) : base(model)
        {
        }        
    }
}
