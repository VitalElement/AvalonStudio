// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections;

namespace CorApi.Portable
{
    internal class ChainEnumerator : IEnumerable, IEnumerator
    {
        private ChainEnum m_enum;

        private Chain m_chain;

        internal ChainEnumerator(ChainEnum chainEnumerator)
        {
            m_enum = chainEnumerator;
        }

        //
        // ICloneable interface
        //
        public Object Clone()
        {
            Enum clone = null;
            m_enum.Clone(out clone);
            return new ChainEnumerator((ChainEnum)clone);
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
            Chain[] a = new Chain[1];
            var c = 0u;
            m_enum.Next((uint)a.Length, a, out c);
            if (c == 1) // S_OK && we got 1 new element
                m_chain = new Chain(a[0].NativePointer);
            else
                m_chain = null;
            return m_chain != null;
        }

        public void Reset()
        {
            m_enum.Reset();
            m_chain = null;
        }

        public Object Current
        {
            get
            {
                return m_chain;
            }
        }
    }
    }
