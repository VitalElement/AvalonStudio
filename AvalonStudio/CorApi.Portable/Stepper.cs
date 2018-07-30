// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using SharpGen.Runtime;
using SharpGen.Runtime.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Stepper : ComObject
    {
        public void SetJmcStatus(bool status)
        {
            QueryInterface<Stepper2>().SetJMC(status);
        }

        public void StepRange(RawBool bStepIn, CorDebugStepRange[] ranges)
        {
            StepRange(bStepIn, ranges, (uint)ranges.Length);
        }
    }
}
