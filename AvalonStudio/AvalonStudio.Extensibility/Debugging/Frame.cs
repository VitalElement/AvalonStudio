using System;
using System.Collections.Generic;

namespace AvalonStudio.Debugging
{
	public class Frame
	{
		public uint Level { get; set; }
		public ulong Address { get; set; }
		public string Function { get; set; }
		public List<Variable> Arguments { get; set; }
		public string File { get; set; }
		public string FullFileName { get; set; }
		public int Line { get; set; }
	}
}