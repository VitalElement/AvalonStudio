using System.Collections.Generic;
using System.Linq;
using Microsoft.TemplateEngine.Abstractions;

namespace AvalonStudio.Extensibility.Templating
{
    class DotnetTemplateParameterAdaptor : ITemplateParameter
    {
        private Microsoft.TemplateEngine.Abstractions.ITemplateParameter _inner;

        public DotnetTemplateParameterAdaptor(Microsoft.TemplateEngine.Abstractions.ITemplateParameter inner)
        {
            _inner = inner;
        }

        public string Documentation => _inner.Documentation;

        public string Name => _inner.Name;

        public TemplateParameterPriority Priority => (TemplateParameterPriority)_inner.Priority;

        public string Type => _inner.Type;

        public bool IsName => _inner.IsName;

        public string DefaultValue => _inner.DefaultValue;

        public string DataType => _inner.DataType;

        public IReadOnlyDictionary<string, string> Choices => _inner.Choices;
    }

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

        public IEnumerable<ITemplateParameter> Parameters => _inner.Parameters.Select(p => new DotnetTemplateParameterAdaptor(p));

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
