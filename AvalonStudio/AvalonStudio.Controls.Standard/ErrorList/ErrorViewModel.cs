using System.IO;
using Avalonia.Media;
using AvalonStudio.Languages;
using AvalonStudio.MVVM;

namespace AvalonStudio.Controls.Standard.ErrorList
{
	public class ErrorViewModel : ViewModel<Diagnostic>
	{
		public ErrorViewModel(Diagnostic model) : base(model)
		{
			offset = model.Offset;
		}

		public string File
		{
			get { return Path.GetFileName(Model.File); }
		}

		public string Spelling
		{
			get { return Model.Spelling; }
		}

		public string Project
		{
			get { return Model.Project.Name; }
		}

		public int Line
		{
			get { return Model.Line; }
		}

		private int offset { get; }

		public DiagnosticLevel Level
		{
			get { return Model.Level; }
		}

		public IBrush LevelBrush
		{
			get
			{
				switch (Level)
				{
					case DiagnosticLevel.Error:
					case DiagnosticLevel.Fatal:
						return Brush.Parse("#E51400");

					case DiagnosticLevel.Warning:
						return Brush.Parse("#FFCC00");

					default:
						return Brush.Parse("#1BA1E2");
				}
			}
		}

		public bool IsEqual(ErrorViewModel other)
		{
			var result = false;

			if (File == other.File)
			{
				if (offset == other.offset)
				{
					if (Level == other.Level)
					{
						//if (this.rangeCount == other.rangeCount)
						{
							if (Spelling == other.Spelling)
							{
								result = true;
							}
						}
					}
				}
			}

			return result;
		}
	}
}