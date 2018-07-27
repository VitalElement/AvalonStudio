namespace AvalonStudio.Languages.CPlusPlus
{
    [ExportLanguageServiceProvider(ContentCapabilities.C, ContentCapabilities.CPP)]
    internal class CPlusPlusLanguageServiceProvider : ILanguageServiceProvider
    {
        public ILanguageService CreateLanguageService() => new CPlusPlusLanguageService();
    }
}
