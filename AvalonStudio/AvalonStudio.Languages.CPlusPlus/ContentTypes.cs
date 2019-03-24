namespace AvalonStudio.Languages.CPlusPlus
{
    internal class ContentTypes
    {
        [ExportContentType("C", ContentCapabilities.C)]
        [FileExtensions(".h", ".c", ".cc")]
        public object CContentType { get; }

        [ExportContentType("C++", ContentCapabilities.CPP)]
        [FileExtensions(".h", ".hpp", ".cpp")]
        public object CppContentType { get; }
    }
}
