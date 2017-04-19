namespace AvalonStudio.Extensibility.Templating
{
    using AvalonStudio.Platforms;
    using RazorLight;

    public static class Template
    {
        public static IRazorLightEngine Engine => EngineFactory.CreatePhysical(Platform.TemplatesFolder);
    }
}