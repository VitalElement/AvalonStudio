// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Thread
    {
        public uint Id
        {
            get
            {
                GetID(out uint result);

                return result;
            }
        }

        public Stepper CreateStepper()
        {
            CreateStepper(out Stepper result);

            return result;
        }

        public IEnumerable Chains
        {
            get
            {
                ChainEnum echains = null;
                EnumerateChains(out echains);

                return new ChainEnumerator(echains);
            }
        }

        /** Get the runtime thread object. */
        public Value ThreadVariable
        {
            get
            {
                Value v = null;
                GetObjectW(out v);
                return v;
            }
        }

        public Process Process
        {
            get
            {
                Process p = null;
                GetProcess(out p);
                return Process.GetCorProcess(p.NativePointer);
            }
        }

    }
}
