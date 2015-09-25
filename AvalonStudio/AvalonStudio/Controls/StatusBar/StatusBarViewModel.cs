namespace AvalonStudio.Controls
{
    using Perspex.MVVM;

    public class StatusBarViewModel : ViewModelBase
    {
        private bool debugMode;
        public bool DebugMode
        {
            get { return debugMode; }
            set { debugMode = value; OnPropertyChanged(); }
        }

        private int lineNumber;
        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; OnPropertyChanged(); OnPropertyChanged(() => LineText); }
        }

        private int column;
        public int Column
        {
            get { return column; }
            set { column = value; OnPropertyChanged(); OnPropertyChanged(() => ColumnText); }
        }

        private int offset;
        public int Offset
        {
            get { return offset; }
            set { offset = value; OnPropertyChanged(); OnPropertyChanged(() => OffsetText); }
        }

        public string OffsetText
        {
            get
            {
                if (this.Offset == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return string.Format("Loc {0}", Offset);
                }
            }
        }


        public string LineText
        {
            get
            {
                if (this.LineNumber == 0)
                {
                    return string.Empty;
                }

                return string.Format("Ln {0}", this.LineNumber);
            }
        }

        public string ColumnText
        {
            get
            {
                if (this.Column == 0)
                {
                    return string.Empty;
                }

                return string.Format("Col {0}", this.Column);
            }
        }
    }
}
