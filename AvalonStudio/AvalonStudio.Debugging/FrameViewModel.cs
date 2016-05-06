namespace AvalonStudio.Controls
{
    using AvalonStudio.Debugging;
    using AvalonStudio.MVVM;
    using Shell;
    using System.IO;
    using Utils;

    public class FrameViewModel : ViewModel<Frame>
    {
        private IDebugManager _debugManager;

        public FrameViewModel(Frame model) : base(model)
        {
        }

        public string Function
        {
            get
            {
                return Model.Function;
            }
        }

        public string Address
        {
            get
            {
                return string.Format("0x{0:X}", Model.Address);
            }
        }

        public int Line
        {
            get
            {
                return Model.Line;
            }
        }

        public string File
        {
            get
            {
                if (Model.File != null)
                {
                    var filePath = Path.GetDirectoryName(Model.File);

                    var file = Path.GetFileName(Model.File);

                    var relativePath = filePath.MakeRelativePath(_debugManager.Project.CurrentDirectory);

                    return Path.Combine(relativePath, file);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}
