using AvalonStudio.Commands;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using ReactiveUI;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Editor.Commands
{
    internal class EditorCommands
    {
        [ExportCommandDefinition("Editor.GoToDefinition")]
        public CommandDefinition GoToDefintionCommand =>
            new CommandDefinition("Go to Definition", null, ReactiveCommand.CreateFromTask<ICodeEditor>(GoToDefinition));

        private async Task GoToDefinition(ICodeEditor editor)
        {
            var definition = await editor.LanguageService?.GotoDefinition(1);

            var studio = IoC.Get<IStudio>();

            if (definition.MetaDataFile == null)
            {
                var document = studio.CurrentSolution.FindFile(definition.FileName);

                if (document != null)
                {
                    await studio.OpenDocumentAsync(document, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
                }
            }
            else
            {
                await studio.OpenDocumentAsync(definition.MetaDataFile, definition.Line, definition.Column, definition.Column, selectLine: true, focus: true);
            }
        }
    }
}
