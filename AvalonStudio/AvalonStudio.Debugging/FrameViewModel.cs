using System.IO;
using AvalonStudio.MVVM;
using AvalonStudio.Utils;

namespace AvalonStudio.Debugging
{
	public class FrameViewModel : ViewModel<Frame>
	{
		private readonly IDebugManager _debugManager;

		public FrameViewModel(IDebugManager debugManager, Frame model) : base(model)
		{
			_debugManager = debugManager;
		}

		public string Function
		{
			get { return Model.Function; }
		}

		public string Address
		{
			get { return string.Format("0x{0:X}", Model.Address); }
		}

		public int Line
		{
			get { return Model.Line; }
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
				return string.Empty;
			}
		}
	}
}