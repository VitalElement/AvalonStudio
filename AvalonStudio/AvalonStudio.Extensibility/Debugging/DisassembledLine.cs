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

		public static InstructionLine FromDataString(string data)
		{
			var result = new InstructionLine();

			var pairs = data.ToNameValuePairs();

			foreach (var pair in pairs)
			{
				switch (pair.Name)
				{
					case "address":
						result.Address = Convert.ToUInt64(pair.Value.Substring(2), 16);
						break;

					case "func-name":
						result.FunctionName = pair.Value;
						break;

					case "offset":
						result.Offset = Convert.ToInt32(pair.Value);
						break;

					case "opcodes":
						result.OpCodes = pair.Value;
						break;

					case "inst":
						result.Instruction = pair.Value.Replace("\\t", "\t");
						break;

					default:
						break; //throw new NotImplementedException ();
				}
			}

			return result;
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

		public static List<InstructionLine> FromDataString(string data)
		{
			var result = new List<InstructionLine>();

			var pairs = data.ToNameValuePairs();

			if (data[0] == '{')
			{
				result = new List<InstructionLine>();

				result.Add(InstructionLine.FromDataString(data.RemoveBraces()));
			}
			//else
			//{
			//    foreach (var pair in pairs)
			//    {
			//        switch (pair.Name)
			//        {
			//            case "src_and_asm_line":
			//                var mixedPairs = pair.Value.RemoveBraces ().ToNameValues ();
			//                var sourceLine = new SourceLine ();
			//                var instructionLines = new List<InstructionLine> ();
			//                foreach (var internalPair in mixedPairs)
			//                {
			//                    switch (internalPair.Name)
			//                    {
			//                        case "line":
			//                            sourceLine.Line = Convert.ToInt32 (internalPair.Value);
			//                            break;

			//                        case "file":
			//                            sourceLine.File = internalPair.Value;
			//                            break;

			//                        case "fullname":
			//                            sourceLine.FullFileName = internalPair.Value;
			//                            break;

			//                        case "line_asm_insn":
			//                            var instructions = internalPair.Value.ToArray ();

			//                            foreach (var instruction in instructions)
			//                            {
			//                                instructionLines.Add (InstructionLine.FromDataString (instruction.RemoveBraces ()));
			//                            }
			//                            break;

			//                        default:
			//                            throw new NotImplementedException ();
			//                    }
			//                }

			//                result.Add (sourceLine);
			//                result.AddRange (instructionLines);
			//                break;

			//            default:
			//                throw new NotImplementedException ();
			//        }
			//}
			//}

			return result;
		}
	}
}