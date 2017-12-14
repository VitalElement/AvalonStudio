using Microsoft.TemplateEngine.Edge.Template;

namespace AvalonStudio.Extensibility.Templating
{
    public class DotNetTemplateAdaptor : ITemplate
    {
        private ITemplateMatchInfo _template;
        private TemplateKind _kind;

        internal DotNetTemplateAdaptor(ITemplateMatchInfo template, TemplateKind kind)
        {
            _kind = kind;
            _template = template;
        }

        public string Name => _template.Info.Name;

        public string ShortName => _template.Info.ShortName;

        public string Description => _template.Info.Description;

        public string Author => _template.Info.Author;

        public string DefaultName
        {
            get
            {
                if(!string.IsNullOrEmpty(_template.Info.DefaultName))
                {
                    return _template.Info.DefaultName;
                }
                else
                {
                    return Name.Replace(" ", "").Replace(".", "");
                }
            }
        }


        public TemplateKind Kind => _kind;

        internal ITemplateMatchInfo DotnetTemplate => _template;
    }
}
