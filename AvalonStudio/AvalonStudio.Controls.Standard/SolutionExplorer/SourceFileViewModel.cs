using Avalonia.Media;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Reactive;

namespace AvalonStudio.Controls.Standard.SolutionExplorer
{
    public class SourceFileViewModel : ProjectItemViewModel<ISourceFile>
    {
        private DrawingGroup _icon;

        public SourceFileViewModel(ISourceFile model) : base(model)
        {
            OpenInExplorerCommand = ReactiveCommand.Create(() => Platform.OpenFolderInExplorer(model.CurrentDirectory));

            RemoveCommand = ReactiveCommand.Create(() => model.Project.ExcludeFile(model));

            DeleteCommand = ReactiveCommand.Create(() =>
            {
                model.Delete();
            });

            _icon = model.Extension.Replace(".", "").ToFileIcon();

            if (_icon == null)
            {
                _icon = "Txt".ToFileIcon();
            }
        }

        public new ReactiveCommand<Unit, Unit> OpenInExplorerCommand { get; }
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }
        
        public override DrawingGroup Icon => _icon;
    }
}