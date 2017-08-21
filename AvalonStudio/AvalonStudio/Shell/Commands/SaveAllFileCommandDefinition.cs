using Avalonia.Controls.Shapes;
using Avalonia.Media;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace AvalonStudio.Shell.Commands
{
    public class SaveAllFileCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> _command;

        public SaveAllFileCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                shell?.SaveAll();
            });
        }

        public override string Text => "Save All";

        public override string ToolTip => "Saves all modified documents.";

       /* public override Path Icon
            =>
                new Path
                {
                    Fill = Brush.Parse("#FF7AC1FF"),
                    UseLayoutRounding = false,
                    Stretch = Stretch.Uniform,
                    Data =
                        StreamGeometry.Parse(
                            "M17,7V3H7V7H17M14,17A3,3 0 0,0 17,14A3,3 0 0,0 14,11A3,3 0 0,0 11,14A3,3 0 0,0 14,17M19,1L23,5V17A2,2 0 0,1 21,19H7C5.89,19 5,18.1 5,17V3A2,2 0 0,1 7,1H19M1,7H3V21H17V23H3A2,2 0 0,1 1,21V7Z")
                };*/

        public override ICommand Command => _command;
    }
}