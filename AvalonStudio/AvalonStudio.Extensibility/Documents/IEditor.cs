using AvalonStudio.Projects;

namespace AvalonStudio.Documents
{
    public interface IEditor
    {
        ISourceFile ProjectFile { get; }

        void Close();

        void Save();

        void Comment();

        void UnComment();

        void Undo();

        void Redo();

        void SetDebugHighlight(int line, int startColumn, int endColumn);

        void ClearDebugHighlight();

        void GotoOffset(int offset);
    }
}