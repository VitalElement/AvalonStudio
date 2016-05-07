namespace AvalonStudio.Debugging.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Debugging;
    using Extensibility;
    using Perspex.Controls;
    using Perspex.Input;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Utils;
    using Perspex.Controls.Shapes;
    using Perspex.Media;
    [CommandDefinition]
    class StartDebuggingCommandDefinition : CommandDefinition
    {
        public StartDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();
                
                if(manager.CurrentDebugger == null)
                {
                    var shell = IoC.Get<IShell>();
                    
                    var project = shell.GetDefaultProject();

                    if (project != null)
                    {
                        manager.StartDebug(project);
                    }
                }   
                else
                {
                    manager.Continue();
                }             
            });
        }

        private ReactiveCommand<object> command;

        public override ICommand Command
        {
            get
            {
                return command;
            }
        }

        public override string Text
        {
            get
            {
                return "Start Debugging";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Starts a debug session.";
            }
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("#FF8DD28A"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M8,5.14V19.14L19,12.14L8,5.14Z") };
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StartDebuggingCommandDefinition>(new KeyGesture() { Key = Key.F5 });
    }
}
