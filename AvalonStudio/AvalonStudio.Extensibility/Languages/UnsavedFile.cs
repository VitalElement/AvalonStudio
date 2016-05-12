namespace AvalonStudio.Languages
{
    public class UnsavedFile
    {
        public UnsavedFile (string filename, string contents)
        {
            this.FileName = filename;
            this.Contents = contents;
        }

        public readonly string FileName;
        public string Contents;
    }
}
