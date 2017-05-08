using SharpDX.Mathematics.Interop;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Controller
    {
        public IEnumerable Threads
        {
            get
            {
                ThreadEnum ethreads = null;
                EnumerateThreads(out ethreads);

                return new ThreadEnumerator(ethreads);
            }
        }

        public virtual void Continue(bool outOfBand)
        {
            ContinueImpl(outOfBand);
        }
    }
}
