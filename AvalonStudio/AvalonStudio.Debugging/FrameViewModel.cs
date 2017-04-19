using AvalonStudio.MVVM;

namespace AvalonStudio.Debugging
{
    public class FrameViewModel : ViewModel
    {
        private readonly IDebugManager2 _debugManager;

        public FrameViewModel(IDebugManager2 debugManager)
        {
            _debugManager = debugManager;
        }

        public string Function
        {
            get { return "Fn()"; }
        }

        public string Address
        {
            get { return string.Format("0x{0:X}", 0); }
        }

        public int Line
        {
            get { return 1; }
        }

        public string File
        {
            get
            {
                /*if (Model.FullFileName != null)
				{
					var filePath = Path.GetDirectoryName(Model.FullFileName);

					var file = Path.GetFileName(Model.FullFileName);

					var relativePath = filePath.MakeRelativePath(_debugManager.Project.CurrentDirectory);

					return Path.Combine(relativePath, file);
				}*/

                return string.Empty;
            }
        }
    }
}