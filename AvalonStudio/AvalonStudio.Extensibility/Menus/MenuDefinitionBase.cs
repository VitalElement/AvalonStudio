using System;
using Avalonia.Input;
using AvalonStudio.Extensibility.Commands;
using System.Composition;

namespace AvalonStudio.Extensibility.Menus
{
    [PartNotDiscoverable]
	public abstract class MenuDefinitionBase
	{
		public abstract int SortOrder { get; }
		public abstract string Text { get; }
		public abstract Uri IconSource { get; }
		public abstract KeyGesture KeyGesture { get; }
		public abstract CommandDefinitionBase CommandDefinition { get; }
	}
}