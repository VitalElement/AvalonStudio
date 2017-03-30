using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Debugging
{
	public class InstructionLine : DisassembledLine
	{
		public InstructionLine()
		{
			Visible = true;
		}

		public ulong Address { get; set; }

		public string FormattedAddress
		{
			get { return string.Format("0x{0:X8}", Address); }
		}

		public string FunctionName { get; set; }
		public int Offset { get; set; }
		public string OpCodes { get; set; }
		public string Instruction { get; set; }
		public bool Visible { get; set; }

		public string Symbol
		{
			get { return string.Format("<{0} + {1}>", FunctionName, Offset); }
		}
	}

	public abstract class DisassembledLine
	{
	}

	public class SourceLine : DisassembledLine
	{
		private string lineText;

		public string LineText
		{
			get
			{
				if (lineText == null)
				{
					lineText = System.IO.File.ReadLines(File).Skip(Line - 1).Take(1).First();
				}

				return lineText;
			}
		}


		public int Line { get; set; }
		public string File { get; set; }
		public string FullFileName { get; set; }
	}
}