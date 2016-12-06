namespace AvalonStudio.Shell.Commands
{
    using Avalonia.Input;
    using AvalonStudio.Controls;
    using AvalonStudio.Extensibility;
    using AvalonStudio.Extensibility.Commands;
    using ReactiveUI;
    using System;
    using System.ComponentModel.Composition;

    [CommandDefinition]
    public class QuickFindCommandDefinition : CommandDefinition
    {
        private readonly ReactiveCommand<object> _command;

        [Export]
        public static CommandKeyboardShortcut KeyGesture =
            new CommandKeyboardShortcut<QuickFindCommandDefinition>(new KeyGesture
            {
                Key = Key.F,
                Modifiers = InputModifiers.Control
            });

        public QuickFindCommandDefinition()
        {
            _command = ReactiveCommand.Create();

            _command.Subscribe(_ =>
            {
                var shell = IoC.Get<IShell>();

                var selectedDocument = shell?.SelectedDocument;

                if(selectedDocument is EditorViewModel)
                {
                    var document = selectedDocument as EditorViewModel;

                    document.FindInFile.IsVisible = true;
                }
            });
        }

        public override System.Windows.Input.ICommand Command => _command;

        public override string Text => "Quick Find";

        public override string ToolTip => "Finds text inside a document.";
    }
}
