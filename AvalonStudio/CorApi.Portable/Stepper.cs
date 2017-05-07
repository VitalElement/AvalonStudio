using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Stepper
    {
        public void SetJmcStatus(bool status)
        {
            QueryInterface<Stepper2>().SetJMC(status);
        }

        public void StepRange(RawBool bStepIn, CorDebugStepRange[] ranges)
        {
            StepRange(bStepIn, ranges, ranges.Length);
        }
    }
}
