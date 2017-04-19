using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SourceFileViewModel : ProjectItemViewModel<ISourceFile>
    {
        public SourceFileViewModel(ISourceFile model) : base(model)
        {
            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe(o => { Platform.OpenFolderInExplorer(model.CurrentDirectory); });

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe(o => { model.Project.ExcludeFile(model); });
        }

        public new ReactiveCommand<object> OpenInExplorerCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }
    }
}