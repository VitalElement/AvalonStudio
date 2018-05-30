using System.Threading.Tasks;
using AvalonStudio.Commands;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.CodeEditor.Commands
{
    internal class EditorCommands
    {
        [ExportCommandDefinition("Editor.GoToDefinition")]
        public CommandDefinition GoToDefintionCommand =>
            new CommandDefinition("Go to Definition", null, ReactiveCommand.CreateFromTask<IEditor>(GoToDefinition));

        private async Task GoToDefinition(IEditor editor)
        {
            var definition = await editor.LanguageService?.GotoDefinition(editor, 1);

            var shell = IoC.Get<IShell>();

            if (definition.MetaDataFile == null)
            {
                var document = shell.CurrentSolution.FindFile(definition.FileName);

                if (document != null)
                {
                    await shell.OpenDocumentAsync(document, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                }
            }
            else
            {
                await shell.OpenDocumentAsync(definition.MetaDataFile, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
            }
        }
    }
}
