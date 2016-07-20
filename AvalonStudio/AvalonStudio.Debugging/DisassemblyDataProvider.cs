using System;
using System.Collections.Generic;
using System.Linq;
using AvalonStudio.MVVM;
using AvalonStudio.MVVM.DataVirtualization;
using ReactiveUI;

namespace AvalonStudio.Debugging
{
	public class DissasemblyDataProvider : ViewModel, IItemsProvider<InstructionLine>
	{
		private int count;

		private IDebugger debugger;

		public DissasemblyDataProvider()
		{
			count = int.MaxValue;
		}

		public ulong BaseAddress { get; set; }

		public int Count
		{
			get { return count; }
			set { this.RaiseAndSetIfChanged(ref count, value); }
		}


		/// <summary>
		///     Fetches a range of items.
		/// </summary>
		/// <param name="startIndex">The start index.</param>
		/// <param name="count">The number of items to fetch.</param>
		/// <returns></returns>
		public IList<InstructionLine> FetchRange(int startIndex, int pageCount, out int overallCount)
		{
			overallCount = Count;

			List<InstructionLine> result = null;

			if (debugger != null)
			{
				result = FetchRange(startIndex, pageCount);

				if (result == null)
				{
					result = new List<InstructionLine>();

					for (uint i = 0; i < pageCount; i++)
					{
						var address = (uint) startIndex + i;

						result.Add(new InstructionLine {Address = address, Instruction = "Unable to read memory."});
					}
				}
			}
			else
			{
				result = new List<InstructionLine>();

				for (var i = 0; i < pageCount; i++)
				{
					result.Add(new InstructionLine {Address = (uint) (startIndex + i), Instruction = "Debugger Disconncted."});
				}
			}

			return result;
		}

		public void SetDebugger(IDebugger debugger)
		{
			this.debugger = debugger;
		}


		private List<InstructionLine> FetchRange(int startIndex, int pageCount)
		{
			List<InstructionLine> result = null;

			Console.WriteLine("Make asynchrpnouse");

			var task = debugger.DisassembleAsync(BaseAddress + (ulong) startIndex, (uint) pageCount);
			task.Wait();
			var instructions = task.Result;

			if (instructions != null)
			{
				result = new List<InstructionLine>();

				for (uint i = 0; i < pageCount; i++)
				{
					var address = (uint) startIndex + i;

					var instruction = instructions.Where(inst => inst.Address == address);

					if (instruction.Count() == 0)
					{
						result.Add(new InstructionLine {Visible = false});
					}
					else
					{
						result.Add(instruction.First());
					}
				}
			}

			return result;
		}
	}
}