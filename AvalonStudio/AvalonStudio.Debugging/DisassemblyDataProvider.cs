namespace AvalonStudio.Debugging
{
    using Debugging;
    using MVVM;
    using MVVM.DataVirtualization;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;

    public class DissasemblyDataProvider : ViewModel, IItemsProvider<InstructionLine>
    {
        public DissasemblyDataProvider()
        {
            this.count = Int32.MaxValue;
        }

        public void SetDebugger(IDebugger debugger)
        {
            this.debugger = debugger;
        }

        private IDebugger debugger;

        private ulong baseAddress;
        public ulong BaseAddress
        {
            get { return baseAddress; }
            set { baseAddress = value; }
        }

        private int count;
        public int Count
        {
            get { return count; }
            set { this.RaiseAndSetIfChanged(ref count, value); }
        }


        private List<InstructionLine> FetchRange(int startIndex, int pageCount)
        {
            List<InstructionLine> result = null;

            Console.WriteLine("Make asynchrpnouse");
            
            var task = debugger.DisassembleAsync(baseAddress + (ulong)startIndex, (uint)pageCount);
            task.Wait();
            var instructions = task.Result;

            if (instructions != null)
            {
                result = new List<InstructionLine>();

                for (uint i = 0; i < pageCount; i++)
                {
                    uint address = (uint)startIndex + i;

                    var instruction = instructions.Where((inst) => inst.Address == address);

                    if (instruction.Count() == 0)
                    {
                        result.Add(new InstructionLine() { Visible = false });
                    }
                    else
                    {
                        result.Add(instruction.First());
                    }
                }
            }

            return result;
        }


        /// <summary>
        /// Fetches a range of items.
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
                        uint address = (uint)startIndex + i;

                        result.Add(new InstructionLine() { Address = address, Instruction = "Unable to read memory." });
                    }
                }
            }
            else
            {
                result = new List<InstructionLine>();

                for (int i = 0; i < pageCount; i++)
                {
                    result.Add(new InstructionLine() { Address = (uint)(startIndex + i), Instruction = "Debugger Disconncted." });
                }
            }

            return result;
        }
    }
}
