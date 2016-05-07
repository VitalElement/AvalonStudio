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
    class StepOutCommandDefinition : CommandDefinition
    {
        public StepOutCommandDefinition()
        {
            command = ReactiveCommand.Create();
            command.Subscribe(_ =>
            {
                var manager = IoC.Get<IDebugManager>();

                manager.StepOut();
            });
        }

        public override Path IconPath
        {
            get
            {
                return new Path() { Fill = Brush.Parse("#FF7AC1FF"), UseLayoutRounding = false, Stretch = Stretch.Uniform, Data = StreamGeometry.Parse("M12,22A2,2 0 0,1 10,20A2,2 0 0,1 12,18A2,2 0 0,1 14,20A2,2 0 0,1 12,22M13,16H11V6L6.5,10.5L5.08,9.08L12,2.16L18.92,9.08L17.5,10.5L13,6V16Z") };
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
                return "Step Over";
            }
        }

        public override string ToolTip
        {
            get
            {
                return "Steps over the current line.";
            }
        }

        [Export]
        public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<StepOverCommandDefinition>(new KeyGesture() { Key = Key.F11, Modifiers = InputModifiers.Shift });
    }
}
