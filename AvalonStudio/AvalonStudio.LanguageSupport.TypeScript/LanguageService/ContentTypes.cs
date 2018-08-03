using AvalonStudio.Languages;

namespace AvalonStudio.LanguageSupport.TypeScript.LanguageService
{
    internal class ContentTypes
    {
        [ExportContentType("TypeScript", ContentCapabilities.TypeScript)]
        [FileExtensions(".ts", ".tsx")]
        public object CSharpContentType { get; }
    }
}
