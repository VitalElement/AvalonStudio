// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public class ProcessEnumerator :
        IEnumerable, IEnumerator
    {
        private ProcessEnum m_enum;
        private Process m_proc;

        public ProcessEnumerator(ProcessEnum processEnumerator)
        {
            m_enum = processEnumerator;
        }

        //
        // ICloneable interface
        //
        public Object Clone()
        {
            Enum clone = null;
            m_enum.Clone(out clone);
            return new ProcessEnumerator((ProcessEnum)clone);
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
            var a = new Process[1];
            var c = 0u;
            m_enum.Next((uint)a.Length, a, out c);
            if (c == 1) // S_OK && we got 1 new element
                m_proc = Process.GetCorProcess(a[0].NativePointer);
            else
                m_proc = null;
            return m_proc != null;
        }

        public void Reset()
        {
            m_enum.Reset();
            m_proc = null;
        }

        public Object Current
        {
            get
            {
                return m_proc;
            }
        }
    } /* class ProcessEnumerator */
}
