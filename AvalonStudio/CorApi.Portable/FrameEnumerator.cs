using System;
using System.Collections;

namespace CorApi.Portable
{
    internal class FrameEnumerator : IEnumerable, IEnumerator
    {
        internal FrameEnumerator(FrameEnum frameEnumerator)
        {
            m_enum = frameEnumerator;
        }

        //
        // ICloneable interface
        //
        public Object Clone()
        {
            Enum clone = null;
            m_enum.Clone(out clone);
            return new FrameEnumerator((FrameEnum)clone);
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
            Frame[] a = new Frame[1];
            int c = 0;
            m_enum.Next(a.Length, a, out c);
            if (c == 1) // S_OK && we got 1 new element
                m_frame = new Frame(a[0].NativePointer);
            else
                m_frame = null;
            return m_frame != null;
        }

        public void Reset()
        {
            m_enum.Reset();
            m_frame = null;
        }

        public Object Current
        {
            get
            {
                return m_frame;
            }
        }

        private FrameEnum m_enum;
        private Frame m_frame;
    }
}
