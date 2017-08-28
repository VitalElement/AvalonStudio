using Avalonia.Media;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SourceFileViewModel : ProjectItemViewModel<ISourceFile>
    {
        private DrawingGroup _icon;

        public SourceFileViewModel(ISourceFile model) : base(model)
        {
            OpenInExplorerCommand = ReactiveCommand.Create();
            OpenInExplorerCommand.Subscribe(o => { Platform.OpenFolderInExplorer(model.CurrentDirectory); });

            RemoveCommand = ReactiveCommand.Create();
            RemoveCommand.Subscribe(o => { model.Project.ExcludeFile(model); });

            _icon = model.Extension.Replace(".","").ToFileIcon();
        }

        public new ReactiveCommand<object> OpenInExplorerCommand { get; }
        public ReactiveCommand<object> RemoveCommand { get; }

        public override DrawingGroup Icon => _icon;
    }
}