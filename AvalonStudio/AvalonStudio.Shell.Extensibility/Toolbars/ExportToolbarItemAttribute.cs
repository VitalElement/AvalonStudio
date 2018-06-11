using AvalonStudio.Menus;
using System;
using System.Composition;

namespace AvalonStudio.Toolbars
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ExportToolbarItemAttribute : ExportAttribute
    {
        public string ToolbarName { get; }
        public MenuPath Path { get; }

        public ExportToolbarItemAttribute(string toolbarName, params string[] path)
            : base(ExportContractNames.Toolbars, typeof(IMenuItem))
        {
            ToolbarName = toolbarName;
            Path = new MenuPath(path);
        }
    }
}
