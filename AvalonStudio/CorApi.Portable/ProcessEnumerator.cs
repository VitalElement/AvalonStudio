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
            int c = 0;
            m_enum.Next(a.Length, a, out c);
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
