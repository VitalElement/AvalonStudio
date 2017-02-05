namespace AvalonStudio.Extensibility.Templating
{
    using AvalonStudio.Platforms;
    using RazorLight;
    using System;

    public static class Template
    {
        public static IRazorLightEngine Engine => EngineFactory.CreatePhysical(Platform.TemplatesFolder);        
    }
}
