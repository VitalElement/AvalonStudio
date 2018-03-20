using Microsoft.TemplateEngine.Abstractions;

namespace AvalonStudio.Extensibility.Templating
{
    public static class TemplateInfoExtensions
    {
        private const string LanguageTag = "language";
        private const string TypeTag = "type";

        public static string GetLanguage(this ITemplateInfo templateInfo) => GetTemplateTag(templateInfo, LanguageTag);

        public static TemplateKind GetTemplateKind(this ITemplateInfo templateInfo)
        {
            var type = GetTemplateTag(templateInfo, TypeTag);

            switch (type)
            {
                case "project":
                    return TemplateKind.Project;
                case "item":
                    return TemplateKind.Item;
                default:
                    return TemplateKind.Other;
            }
        }

        private static string GetTemplateTag(ITemplateInfo templateInfo, string tagName)
        {
            if (templateInfo.Tags.TryGetValue(tagName, out var cacheTag))
            {
                return cacheTag.DefaultValue;
            }

            return null;
        }
    }
}
