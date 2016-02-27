namespace AvalonStudio.Controls.ViewModels
{
    using Projects;
    using ReactiveUI;
    using System;
    using System.Diagnostics;
    using System.IO;

    public class SourceFileViewModel : ProjectItemViewModel<ISourceFile>
    {
        public SourceFileViewModel(ISourceFile model) : base(model)
        {
            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe((o) =>
            {
                Process.Start(Path.GetDirectoryName(model.Location));
            });

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe((o) =>
            {
                model.Project.RemoveFile(model);
            });          
        }

        new public ReactiveCommand<object> OpenInExplorerCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }
    }
}
