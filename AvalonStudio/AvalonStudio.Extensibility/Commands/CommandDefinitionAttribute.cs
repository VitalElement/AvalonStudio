using System;
using System.ComponentModel.Composition;

namespace AvalonStudio.Extensibility.Commands
{
	[MetadataAttribute]
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandDefinitionAttribute : ExportAttribute
	{
		public CommandDefinitionAttribute()
			: base(typeof (CommandDefinitionBase))
		{
		}
	}
}