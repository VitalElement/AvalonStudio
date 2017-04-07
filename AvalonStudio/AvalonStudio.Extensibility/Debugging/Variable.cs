using System;

namespace AvalonStudio.Debugging
{
	public class Variable
	{
		public string Name { get; set; }
		public bool IsArgument { get; set; }
		public string Type { get; set; }
		public string Value { get; set; }
	}
}