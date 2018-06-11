using AvalonStudio.Menus;
using System;
using System.Composition;

namespace AvalonStudio.MainMenu
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
    public class ExportMainMenuItemAttribute : ExportAttribute
    {
        public MenuPath Path { get; }
        
        public ExportMainMenuItemAttribute(params string[] path)
            : base(ExportContractNames.MainMenu, typeof(IMenuItem))
        {
            Path = new MenuPath(path);
        }
    }
}
