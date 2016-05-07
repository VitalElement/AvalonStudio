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
    public class UnCommentCommandDefinition : CommandDefinition
    {
        public UnCommentCommandDefinition()
        {
            command = ReactiveCommand.Create();

            command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                
            });
        }
        
        public override string Text
        {
            get { return "Un-Comment"; }
        }

        public override string ToolTip
        {
            get { return "UnComments the selcted code."; }
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("WhiteSmoke"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M3,3H21V5H3V3M9,7H21V9H9V7M3,11H21V13H3V11M9,15H21V17H9V15M3,19H21V21H3V19Z") };
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