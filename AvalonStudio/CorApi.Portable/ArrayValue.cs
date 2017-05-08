using SharpDX.Mathematics.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CorApi.Portable
{
    public partial class ArrayValue
    {
        public int[] GetDimensions()
        {
            Debug.Assert(Rank != 0);
            var dims = new int[Rank];
            GetDimensions(dims.Length, dims);
            
            return dims;
        }

        public Value GetElement(int[] indices)
        {
            Debug.Assert(indices != null);
            Value ppValue = GetElement(indices.Length, indices);
            return ppValue;
        }

        public int[] GetBaseIndicies()
        {
            Debug.Assert(Rank != 0);
            var baseIndicies = new int[Rank];
            GetBaseIndicies(baseIndicies.Length, baseIndicies);
            
            return baseIndicies;
        }

        public bool HasBaseIndiciesP
        {
            get
            {
                return HasBaseIndicies();
            }
        }
    }
}
