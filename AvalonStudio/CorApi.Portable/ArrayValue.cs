// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CorApi.Portable
{
    public partial class ArrayValue
    {
        public uint[] GetDimensions()
        {
            Debug.Assert(Rank != 0);
            var dims = new uint[Rank];
            GetDimensions((uint)dims.Length, dims);
            
            return dims;
        }

        public Value GetElement(uint[] indices)
        {
            Debug.Assert(indices != null);
            var ppValue = GetElement((uint)indices.Length, indices);
            return ppValue;
        }

        public uint[] GetBaseIndicies()
        {
            Debug.Assert(Rank != 0);
            var baseIndicies = new uint[Rank];
            GetBaseIndicies((uint)baseIndicies.Length, baseIndicies);
            
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
