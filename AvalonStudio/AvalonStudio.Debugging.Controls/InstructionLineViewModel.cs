using System;

namespace AvalonStudio.Debugging
{
    public class InstructionLineViewModel : LineViewModel
    {
        public InstructionLineViewModel(InstructionLine model) : base(model)
        {
        }

        public string Instruction
        {
            get { return Model.Instruction; }
        }

        public string Address
        {
            get
            {
                try
                {
                    return string.Format("0x{0:X}", Model.Address);
                }
                catch (Exception)
                {
                    return "";
                }
            }
        }

        public string Symbol
        {
            get { return string.Format("<{0} + {1}>", Model.FunctionName, Model.Offset); }
        }

        public new InstructionLine Model
        {
            get { return (InstructionLine)base.Model; }
        }
    }
}