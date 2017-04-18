using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Controls;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class UnCommentCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> _command;

        public UnCommentCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                (shell.SelectedDocument as EditorViewModel)?.UnComment();
            });
        }

        public override string Text => "Un-Comment";

        public override string ToolTip => "UnComments the selcted code.";

        public override Path IconPath
            =>
                new Path
                {
                    Fill = Brush.Parse("WhiteSmoke"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data = StreamGeometry.Parse("M3,3H21V5H3V3M9,7H21V9H9V7M3,11H21V13H3V11M9,15H21V17H9V15M3,19H21V21H3V19Z")
                };

        public override ICommand Command => _command;
    }
}