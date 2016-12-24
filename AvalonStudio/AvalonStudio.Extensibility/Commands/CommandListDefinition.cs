using System;

namespace AvalonStudio.Extensibility.Commands
{
	public abstract class CommandListDefinition : CommandDefinitionBase
	{
		public sealed override string Text
		{
			get { return "[NotUsed]"; }
		}

		public sealed override string ToolTip
		{
			get { return "[NotUsed]"; }
		}

		public sealed override Uri IconSource
		{
			get { return null; }
		}

		public sealed override bool IsList
		{
			get { return true; }
		}
	}
}