using AvalonStudio.Languages;

namespace AvalonStudio.LanguageSupport.TypeScript.LanguageService
{
    [ExportLanguageServiceProvider(ContentCapabilities.TypeScript)]
    internal class TypeScriptLanguageServiceProvider : ILanguageServiceProvider
    {
        public ILanguageService CreateLanguageService() => new TypeScriptLanguageService();
    }
}
