namespace AvalonStudio.Languages.CSharp
{
    [ExportLanguageServiceProvider(ContentCapabilities.CSharp)]
    internal class CSharpLanguageServiceProvider : ILanguageServiceProvider
    {
        public ILanguageService CreateLanguageService() => new CSharpLanguageService();
    }
}
