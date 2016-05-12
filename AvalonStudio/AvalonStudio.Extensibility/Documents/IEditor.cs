using AvalonStudio.Projects;

namespace AvalonStudio.Documents
{
    public interface IEditor
    {
        void Comment();
        void UnComment();
        void Undo();
        void Redo();
        void ClearDebugHighlight();
        ISourceFile ProjectFile { get; }
        void GotoOffset(int offset);
    }
}
