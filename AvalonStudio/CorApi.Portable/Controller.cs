// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using SharpGen.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Controller : ComObject
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
