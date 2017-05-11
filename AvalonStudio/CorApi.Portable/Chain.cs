// Copyright (c) 2017 Vital Element Avalon Studio - Dan Walmsley dan at walms dot co dot uk
// 
// This code is licensed for use only with AvalonStudio. It is not permitted for use in any 
// project unless explicitly authorized.
//

using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public partial class Chain
    {
        public IEnumerable Frames
        {
            get
            {
                FrameEnum eframes = null;
                EnumerateFrames(out eframes);

                return new FrameEnumerator(eframes);
            }
        }
    }
}
