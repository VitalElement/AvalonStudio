using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Thread
    {
        public int Id
        {
            get
            {
                GetID(out int result);

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
