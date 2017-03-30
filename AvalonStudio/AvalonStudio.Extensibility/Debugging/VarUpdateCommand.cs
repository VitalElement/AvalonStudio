using System;

namespace AvalonStudio.Debugging
{
	public class VariableObjectChange
	{
		public string Expression { get; set; }
		public string Value { get; set; }
		public bool InScope { get; set; }
		public bool TypeChanged { get; set; }
		public int HasMore { get; set; }
	}
}