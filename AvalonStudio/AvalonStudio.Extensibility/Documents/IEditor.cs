using AvalonStudio.Projects;

namespace AvalonStudio.Documents
{
    public interface IEditor
    {
        ISourceFile ProjectFile { get; }

        void Comment();

        void UnComment();

        void Undo();

        void Redo();

        void ClearDebugHighlight();

        void GotoOffset(int offset);
    }
}