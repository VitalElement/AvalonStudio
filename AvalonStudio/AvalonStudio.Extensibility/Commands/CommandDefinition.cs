using System;
using Avalonia.Controls.Shapes;

namespace AvalonStudio.Extensibility.Commands
{
	public abstract class CommandDefinition : CommandDefinitionBase
	{
		public override Uri IconSource
		{
			get { return null; }
		}

		public override Path IconPath
		{
			get { return null; }
		}

		public sealed override bool IsList
		{
			get { return false; }
		}
	}
}