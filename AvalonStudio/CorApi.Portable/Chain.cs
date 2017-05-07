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
