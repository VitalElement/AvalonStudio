namespace AvalonStudio.Languages.Xaml
{
    [ExportLanguageServiceProvider(ContentCapabilities.Xml)]
    internal class XmlLanguageServiceProvider : ILanguageServiceProvider
    {
        public ILanguageService CreateLanguageService() => new XmlLanguageService();
    }
}
