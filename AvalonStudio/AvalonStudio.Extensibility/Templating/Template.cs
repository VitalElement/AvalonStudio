namespace AvalonStudio.Extensibility.Templating
{
    using AvalonStudio.Platforms;
    using RazorLight;

    public static class Template
    {
        private static EngineFactory s_factory = new EngineFactory();
        private static IRazorLightEngine s_engine;

        public static IRazorLightEngine Engine
        {
            get
            {
                if (s_engine == null)
                {
                    s_engine = s_factory.ForFileSystem(Platform.TemplatesFolder);
                }

                return s_engine;
            }
        }
    }
}