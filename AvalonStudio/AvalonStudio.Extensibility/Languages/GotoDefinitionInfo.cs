using AvalonStudio.Projects;

namespace AvalonStudio.Languages
{
    public class GotoDefinitionInfo
    {
        public string FileName { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }
        public ISourceFile MetaDataFile { get; set; }
    }
}
