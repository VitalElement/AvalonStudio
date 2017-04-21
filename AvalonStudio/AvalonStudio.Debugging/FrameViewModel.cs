using AvalonStudio.MVVM;
using Mono.Debugging.Client;
using System.IO;

namespace AvalonStudio.Debugging
{
    public class FrameViewModel : ViewModel<StackFrame>
    {
        private readonly IDebugManager2 _debugManager;

        public FrameViewModel(IDebugManager2 debugManager, StackFrame frame) : base(frame)
        {
            _debugManager = debugManager;
        }

        public string Function
        {
            get
            {
                if (!string.IsNullOrEmpty(Model.FullModuleName))
                {
                    return Path.GetFileName(Model.FullModuleName) + "!" + Model.FullStackframeText;
                }
                else
                {
                    return Model.FullStackframeText;
                }
            }
        }

        public string Address
        {
            get { return string.Format("0x{0:X}", Model.Address); }
        }

        public int Line
        {
            get { return Model.SourceLocation.Line; }
        }

        public string File
        {
            get
            {
                return Model.SourceLocation.FileName;
            }
        }
    }
}