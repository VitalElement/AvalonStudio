using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Controls
{
    public class StatusBarViewModel : ReactiveObject
    {
        private int column;

        private bool debugMode;

        private string language;

        private int lineNumber;

        private int offset;

        private string platformString;

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

        public string PlatformString
        {
            get
            {
                return platformString;
            }
            set
            {
                platformString = value;
                this.RaisePropertyChanged();
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

        private string _testText = "TestText";

        public string TestText
        {
            get { return _testText; }
            set { this.RaiseAndSetIfChanged(ref _testText, value); }
        }

    }
}