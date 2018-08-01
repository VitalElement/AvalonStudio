using AvalonStudio.Debugging;
using AvalonStudio.Projects;
using System.Threading.Tasks;

namespace AvalonStudio.Documents
{
    public static class DocumentExtensions
    {
        public static void SetDebugHighlight (this IDebugLineDocumentTabViewModel model, int line, int startColumn = -1, int endColumn= -1)
        {
            model.DebugHighlight = new DebugHighlightLocation
            {
                Line = line,
                StartColumn = startColumn,
                EndColumn = endColumn
            };
        }

        public static void ClearDebugHighlight (this IDebugLineDocumentTabViewModel model)
        {
            model.SetDebugHighlight(-1);
        }
    }

    public interface IDebugLineDocumentTabViewModel
    {
        DebugHighlightLocation DebugHighlight { get; set; }
    }

    public interface ITextDocumentTabViewModel : IFileTabViewModel
    {
        ITextDocument Document { get; }

        void GotoPosition(int line, int column);

        void GotoOffset(int offset);

        void Focus();

        void Save();
    }

    public interface IFileTabViewModel : IDocumentTabViewModel
    {
        ISourceFile SourceFile { get; }
    }
}