using Microsoft.TemplateEngine.Abstractions;

namespace AvalonStudio.Extensibility.Templating
{
    public class DotNetTemplateAdaptor : ITemplate
    {
        public string Language { get; }

        public string Name { get; }
        public string ShortName { get; }
        public string Description { get; }
        public string Author { get; }

        public string DefaultName { get; }

        public TemplateKind Kind { get; }

        internal ITemplateInfo DotnetTemplate { get; }

        internal DotNetTemplateAdaptor(ITemplateInfo template)
        {
            Language = template.GetLanguage();

            Name = template.Name;
            ShortName = template.ShortName;
            Description = template.Description;
            Author = template.Author;

            if (!string.IsNullOrEmpty(template.DefaultName))
            {
                DefaultName = template.DefaultName;
            }
            else
            {
                DefaultName = Name.Replace(" ", "").Replace(".", "");
            }

            Kind = template.GetTemplateKind();

            DotnetTemplate = template;
        }
    }
}
