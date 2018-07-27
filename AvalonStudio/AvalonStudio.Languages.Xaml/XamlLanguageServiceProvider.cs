namespace AvalonStudio.Languages.Xaml
{
    [ExportLanguageServiceProvider(ContentCapabilities.Xaml)]
    internal class XamlLanguageServiceProvider : ILanguageServiceProvider
    {
        public ILanguageService CreateLanguageService() => new XamlLanguageService();
    }
}
