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
}