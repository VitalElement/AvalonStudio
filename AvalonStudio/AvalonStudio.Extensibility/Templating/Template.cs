namespace AvalonStudio.Extensibility.Templating
{
    using AvalonStudio.Platforms;
    using RazorLight;

    public static class Template
    {
        public static string Parse(string templateKey, object model)
        {
            var engine = EngineFactory.CreatePhysical(Platform.TemplatesFolder);

            return engine.Parse(templateKey, model);
        }
    }
}
