namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System;
    using System.ComponentModel.Composition;
    using Avalonia.Input;
    using ReactiveUI;
    using Extensibility;
    using Avalonia.Controls.Shapes;
    using Avalonia.Media;

    [CommandDefinition]
    public class UndoCommandDefinition : CommandDefinition
    {
        public UndoCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                shell.SelectedDocument.Undo();
            });
        }

        public override string Text
        {
            get { return "Undo"; }
        }

        public override string ToolTip
        {
            get { return "Undo the last change."; }
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("#FF7AC1FF"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M12.5,8C9.85,8 7.45,9 5.6,10.6L2,7V16H11L7.38,12.38C8.77,11.22 10.54,10.5 12.5,10.5C16.04,10.5 19.05,12.81 20.1,16L22.47,15.22C21.08,11.03 17.15,8 12.5,8Z") };
            }
        }

        public override Uri IconSource
        {
            get { return new Uri(""); }
        }

        ReactiveCommand<object> command;
        public override System.Windows.Input.ICommand Command
        {
            get
            {
                return command;
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<SaveFileCommandDefinition>(new KeyGesture() { Key = Key.S, Modifiers = InputModifiers.Control });
    }
}