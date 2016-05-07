﻿namespace AvalonStudio.Debugging.Commands
{
    using AvalonStudio.Extensibility.Commands;
    using Debugging;
    using Extensibility;
    using Perspex.Controls;
    using Perspex.Controls.Shapes;
    using Perspex.Input;
    using Perspex.Media;
    using Projects;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.Windows.Input;
    using Utils;

    [CommandDefinition]
    class PauseDebuggingCommandDefinition : CommandDefinition
    {
        public PauseDebuggingCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();

                manager.Pause();
            });
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("#FF7AC1FF"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M13,16.14H15V8.14H13M12,20.14C7.59,20.14 4,16.55 4,12.14C4,7.73 7.59,4.14 12,4.14C16.41,4.14 20,7.73 20,12.14C20,16.55 16.41,20.14 12,20.14M12,2.14A10,10 0 0,0 2,12.14A10,10 0 0,0 12,22.14A10,10 0 0,0 22,12.14C22,6.61 17.5,2.14 12,2.14M9,16.14H11V8.14H9V16.14Z") };
            }
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
                return "Pause";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Pauses the current debug session.";
            }
        }
    }
}
