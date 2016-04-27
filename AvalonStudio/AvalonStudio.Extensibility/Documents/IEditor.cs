using AvalonStudio.Projects;

namespace AvalonStudio.Documents
{
    public interface IEditor
    {
        void ClearDebugHighlight();
        ISourceFile ProjectFile { get; }
        void GotoOffset(int offset);
    }
}
