namespace AvalonStudio.Shell.Commands
{
    using Extensibility.Commands;
    using System;
    using System.ComponentModel.Composition;
    using Perspex.Input;
    using ReactiveUI;
    using Extensibility;
    using Perspex.Controls.Shapes;
    using Perspex.Media;

    [CommandDefinition]
    public class CleanCommandDefinition : CommandDefinition
    {
        public CleanCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                shell.Clean();
            });
        }

        public const string CommandName = "File.SaveFile";

        public override string Name
        {
            get { return CommandName; }
        }

        public override string Text
        {
            get { return "Clean"; }
        }

        public override string ToolTip
        {
            get { return "Cleans the Startup Project."; }
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("#FF0F8CD3"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M15,16H19V18H15V16M15,8H22V10H15V8M15,12H21V14H15V12M3,18A2,2 0 0,0 5,20H11A2,2 0 0,0 13,18V8H3V18M14,5H11L10,4H6L5,5H2V7H14V5Z") };
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