using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Templating
{
    public interface ITemplate
    {
        string Language { get; }

        string Name { get; }
        string ShortName { get; }
        string DefaultName { get; }
        string Description { get; }

        TemplateKind Kind { get; }

        IEnumerable<ITemplateParameter> Parameters { get; }
    }

    public enum TemplateParameterPriority
    {
        Required,
        Suggested,
        Optional,
        Implicit
    }

    public interface ITemplateParameter
    {
        string Documentation { get; }

        string Name { get; }

        TemplateParameterPriority Priority { get; }

        string Type { get; }

        bool IsName { get; }

        string DefaultValue { get; }

        string DataType { get; }

        IReadOnlyDictionary<string, string> Choices { get; }
    }
}
