using System.ComponentModel.Composition;

namespace AvalonStudio.Extensibility.Menus
{
	public class MenuBarAttribute : ExportAttribute
	{
		public MenuBarAttribute() : base(typeof (MenuBarDefinition))
		{
		}
	}

	public class MenuAttribute : ExportAttribute
	{
		public MenuAttribute() : base(typeof (MenuDefinition))
		{
		}
	}

	public class MenuItemAttribute : ExportAttribute
	{
		public MenuItemAttribute()
			: base(typeof (MenuItemDefinition))
		{
		}
	}

	public class MenuGroupAttribute : ExportAttribute
	{
		public MenuGroupAttribute() : base(typeof (MenuItemGroupDefinition))
		{
		}
	}

	public class MenuDefinitionAttribute : ExportAttribute
	{
		public MenuDefinitionAttribute()
			: base(typeof (MenuDefinitionBase))
		{
		}
	}
}