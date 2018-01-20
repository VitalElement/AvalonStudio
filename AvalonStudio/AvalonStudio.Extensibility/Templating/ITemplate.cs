namespace AvalonStudio.Extensibility.Templating
{
    public interface ITemplate
    {
        string Name { get; }
        string ShortName { get; }
        string DefaultName { get; }
        string Description { get; }

        TemplateKind Kind { get; }
    }
}
