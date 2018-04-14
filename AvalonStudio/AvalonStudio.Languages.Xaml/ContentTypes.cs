namespace AvalonStudio.Languages.Xaml
{
    internal class ContentTypes
    {
        [ExportContentType("XML", ContentCapabilities.Xml)]
        [FileExtensions(".xml", ".csproj")]
        public object XmlContentType { get; }

        [ExportContentType("XAML", ContentCapabilities.Xml, ContentCapabilities.Xaml)]
        [FileExtensions(".xaml", ".paml")]
        public object XamlContentType { get; }
    }
}
