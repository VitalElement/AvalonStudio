using Avalonia;
using AvaloniaEdit.Document;
using AvaloniaEdit.Editing;
using AvaloniaEdit.Rendering;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public interface IEditor
    {
        ISourceFile ProjectFile { get; }

        void InstallBackgroundRenderer(IBackgroundRenderer backgroundRenderer);

        void InstallVisualLineTransformer(IVisualLineTransformer transformer);

        void InstallMargin(AbstractMargin margin);
      
        void Focus();

        void TriggerCodeAnalysis();

        void Close();

        void Save();

        void Comment();

        void UnComment();

        void Undo();

        void Redo();

        void SetDebugHighlight(int line, int startColumn, int endColumn);

        void ClearDebugHighlight();

        int GetOffsetFromPoint(Point point);

        void GotoOffset(int offset);

        void GotoPosition(int line, int column);

        void FormatAll();

        Task<Symbol> GetSymbolAsync(int offset);

        string GetWordAtOffset(int offset);

        int CaretOffset { get; }

        TextDocument GetDocument();
    }
}