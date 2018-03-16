using System.Collections.Generic;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;

namespace AvalonStudio.Extensibility.Templating
{
    class DotNetTemplateAdaptor : ITemplate
    {
        private ITemplateInfo _inner;

        public string Language { get; }

        public string Name => _inner.Name;
        public string ShortName => _inner.ShortName;
        public string Description => _inner.Description;
        public string Author => _inner.Author;

        public string DefaultName { get; }

        public TemplateKind Kind { get; }

        internal ITemplateInfo DotnetTemplate => _inner;

        public IEnumerable<ITemplateParameter> Parameters => _inner.Parameters
            .Where(p=>p.Name != "language" && p.Name != "type" && p.Name != "namespace")
            .Select(p => new DotnetTemplateParameterAdaptor(p));

        internal DotNetTemplateAdaptor(ITemplateInfo template)
        {
            _inner = template;

            Language = template.GetLanguage();

            if (!string.IsNullOrEmpty(template.DefaultName))
            {
                DefaultName = template.DefaultName;
            }
            else
            {
                DefaultName = Name.Replace(" ", "").Replace(".", "");
            }

            Kind = template.GetTemplateKind();
        }
    }
}
