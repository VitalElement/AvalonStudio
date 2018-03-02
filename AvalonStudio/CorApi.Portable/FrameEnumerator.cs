// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

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
            var c = 0u;
            m_enum.Next((uint)a.Length, a, out c);
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
