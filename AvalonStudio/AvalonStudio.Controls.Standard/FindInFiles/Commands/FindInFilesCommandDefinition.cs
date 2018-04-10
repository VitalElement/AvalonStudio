using Avalonia.Input;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Commands;
using ReactiveUI;
using System.Windows.Input;

namespace AvalonStudio.Controls.Standard.FindInFiles.Commands
{
    class FindInFilesCommandDefinition : CommandDefinition
    {
        public FindInFilesCommandDefinition()
        {
            Command = ReactiveCommand.Create(() =>
            {
                var vm = IoC.Get<FindInFilesViewModel>();

                vm.IsVisible = true;
                vm.IsSelected = true;                
            });
        }

        public override string Text => "Find in Files";

        public override string ToolTip => Text;

        public override ICommand Command { get; }

        public override KeyGesture Gesture => KeyGesture.Parse("CTRL + SHIFT + F");
    }
}
