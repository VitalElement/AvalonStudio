using AvalonStudio.Platforms;
using System;

namespace AvalonStudio.Debugging
{
	public class LiveBreakPoint : BreakPoint
	{
		public enum BreakPointType
		{
			BreakPoint,
			WatchPoint
		}

		public int Number { get; set; }
		public BreakPointType Type { get; set; }
		//catchtype
		public bool Visible { get; set; }
		public bool Enabled { get; set; }
		public ulong Address { get; set; }
		public string Function { get; set; }
		public string FullFileName { get; set; }
		//public string At { get; set; } // if file unknown
		//pending
		//evaluated-by
		//thread
		//task
		//cond
		//ignore
		// enable count
		//thread groups ? list<int>ThreadGroups?
		public int HitCount { get; set; }
		public string OriginalLocation { get; set; }

		public static LiveBreakPoint FromArgumentList(NameValuePair[] argumentList)
		{
			var result = new LiveBreakPoint();

			foreach (var argument in argumentList)
			{
				switch (argument.Name)
				{
					case "number":
						result.Number = Convert.ToInt32(argument.Value);
						break;

					case "type":
						if (argument.Value == "breakpoint")
						{
							result.Type = BreakPointType.BreakPoint;
						}
						else
						{
							throw new Exception("Breakpoint type not implmented.");
						}
						break;

					case "disp":
						result.Visible = argument.Value == "keep";
						break;

					case "enabled":
						result.Enabled = argument.Value == "y";
						break;

					case "addr":
						if (argument.Value != "<MULTIPLE>")
						{
							result.Address = Convert.ToUInt64(argument.Value.Replace("0x", ""), 16);
						}
						break;

					case "func":
						result.Function = argument.Value;
						break;

					case "file":
						result.File = argument.Value.Replace("\\\\", "\\").NormalizePath();
						break;

					case "fullname":
						result.FullFileName = argument.Value.Replace("\\\\", "\\").NormalizePath();
						break;

					case "line":
						result.Line = Convert.ToUInt32(argument.Value);
						break;

					case "times":
						result.HitCount = Convert.ToInt32(argument.Value);
						break;

					case "original-location":
						result.OriginalLocation = argument.Value;
						break;

					default:
						Console.WriteLine("Unknown field in breakpoint data");
						break;
				}
			}

			return result;
		}
	}

	public class BreakPoint
	{
		public string File { get; set; }

		public uint Line { get; set; }
	}
}