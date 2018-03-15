using AvalonStudio.Extensibility.Shell;
using AvalonStudio.MVVM;
using ReactiveUI;
using System.Composition;

namespace AvalonStudio.Controls
{
    [Export]
    [Export(typeof(IStatusBar))]
    [Shared]
    public class StatusBarViewModel : ReactiveObject, IStatusBar
    {
        public StatusBarViewModel()
        {
            LineNumber = 1;
            Column = 1;
        }

        private int column;

        private bool debugMode;

        private string language;

        private int lineNumber;

        private int offset;

        private string _text;

        public bool DebugMode
        {
            get
            {
                return debugMode;
            }
            set
            {
                debugMode = value;
                this.RaisePropertyChanged();
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _text, value);
            }
        }

        public int LineNumber
        {
            get
            {
                return lineNumber;
            }
            set
            {
                lineNumber = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(() => LineText);
            }
        }

        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                column = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(() => ColumnText);
            }
        }

        public int Offset
        {
            get
            {
                return offset;
            }
            set
            {
                offset = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(() => OffsetText);
            }
        }

        public string OffsetText
        {
            get
            {
                if (Offset >= 0)
                {
                    return string.Format("Loc {0}", Offset);
                }
                return string.Empty;
            }
        }

        public string Language
        {
            get { return language; }
            set { this.RaiseAndSetIfChanged(ref language, value); }
        }

        public string LineText
        {
            get
            {
                if (LineNumber == 0)
                {
                    return string.Empty;
                }

                return string.Format("Ln {0}", LineNumber);
            }
        }

        public string ColumnText
        {
            get
            {
                if (Column == 0)
                {
                    return string.Empty;
                }

                return string.Format("Col {0}", Column);
            }
        }

        public bool SetText(string text)
        {
            Text = text;

            return true;
        }

        public void ClearText ()
        {
            Text = "Ready";
        }
    }
}