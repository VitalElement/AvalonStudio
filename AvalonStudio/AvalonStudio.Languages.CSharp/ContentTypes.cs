namespace AvalonStudio.Languages.CSharp
{
    internal class ContentTypes
    {
        [ExportContentType("C#", ContentCapabilities.CSharp, ContentCapabilities.Roslyn)]
        [FileExtensions(".cs", ".csx")]
        public object CSharpContentType { get; }

        [ExportContentType("Visual Basic", ContentCapabilities.VisualBasic, ContentCapabilities.Roslyn)]
        [FileExtensions(".vb")]
        public object VisualBasicContentType { get; }
    }
}
