// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections;

namespace CorApi.Portable
{
    /** Exposes an enumerator for Threads. */
    internal class ThreadEnumerator : IEnumerable, IEnumerator
    {
        private ThreadEnum m_enum;
        private Thread m_th;

        internal ThreadEnumerator(ThreadEnum threadEnumerator)
        {
            m_enum = threadEnumerator;
        }

        //
        // ICloneable interface
        //
        public Object Clone()
        {
            Enum clone = null;
            m_enum.Clone(out clone);
            return new ThreadEnumerator((ThreadEnum)clone);
        }

        //
        // IEnumerable interface
        //
        public IEnumerator GetEnumerator()
        {
            return this;
        }

        //
        // IEnumerator interface
        //
        public bool MoveNext()
        {
            var a = new Thread[1];
            var c = 0u;
            m_enum.Next((uint)a.Length, a, out c); // might need try catch here as new api swallows result!
            if (c == 1) // S_OK && we got 1 new element
                m_th = new Thread(a[0].NativePointer);
            else
                m_th = null;
            return m_th != null;
        }

        public void Reset()
        {
            m_enum.Reset();
            m_th = null;
        }

        public Object Current
        {
            get
            {
                return m_th;
            }
        }
    } /* class ThreadEnumerator */
}
