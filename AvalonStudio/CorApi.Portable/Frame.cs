using System;
using System.Collections.Generic;
using System.Text;

namespace CorApi.Portable
{
    public enum CorFrameType
    {
        ILFrame, NativeFrame, InternalFrame,
    }

    public partial class Frame
    {
        private ILFrame m_ilFrame = null;
        private bool m_ilFrameCached = false;

        private InternalFrame m_iFrame = null;
        private bool m_iFrameCached = false;

        public int IP
        {
            get
            {
                return QueryInterface<NativeFrame>().IP;
            }
        }

        private ILFrame GetILFrame()
        {
            if (!m_ilFrameCached)
            {
                m_ilFrameCached = true;
                m_ilFrame = QueryInterface<ILFrame>();

            }
            return m_ilFrame;
        }

        private InternalFrame GetInternalFrame()
        {
            if (!m_iFrameCached)
            {
                m_iFrameCached = true;

                m_iFrame = QueryInterface<InternalFrame>();
            }
            return m_iFrame;
        }

        public CorFrameType FrameType
        {
            get
            {
                var ilframe = GetILFrame();
                if (ilframe != null)
                    return CorFrameType.ILFrame;

                var iframe = GetInternalFrame();
                if (iframe != null)
                    return CorFrameType.InternalFrame;

                return CorFrameType.NativeFrame;
            }
        }
    }
}
