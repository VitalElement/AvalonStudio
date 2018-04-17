using System.Collections.Generic;

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
}
