using AvalonStudio.Menus;
using System;
using System.Composition;

namespace AvalonStudio.Toolbars
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportToolbarDefaultGroupAttribute : ExportAttribute
    {
        public string ToolbarName { get; }
        public MenuPath Path { get; }

        public ExportToolbarDefaultGroupAttribute(string toolbarName, params string[] path)
            : base(ExportContractNames.Toolbars, typeof(object))
        {
            ToolbarName = toolbarName;
            Path = new MenuPath(path);
        }
    }
}
