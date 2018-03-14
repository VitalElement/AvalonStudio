namespace AvalonStudio.Languages.Xaml
{
    internal class ContentTypes
    {
        [ExportContentType("XML", ContentCapabilities.Xml)]
        [FileExtensions(".xml")]
        public object XmlContentType { get; }

        [ExportContentType("XAML", ContentCapabilities.Xml, ContentCapabilities.Xaml)]
        [FileExtensions(".xaml")]
        public object XamlContentType { get; }
    }
}
