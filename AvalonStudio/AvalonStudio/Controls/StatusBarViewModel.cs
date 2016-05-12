namespace AvalonStudio.Controls
{
    using ReactiveUI;
    using AvalonStudio.MVVM;

    public class StatusBarViewModel : ReactiveObject
    {
        private int instanceCount;

        public int InstanceCount
        {
            get { return instanceCount; }
            set { this.RaiseAndSetIfChanged(ref instanceCount, value); }
        }

        private bool debugMode;
        public bool DebugMode
        {
            get { return debugMode; }
            set { debugMode = value; this.RaisePropertyChanged(); }
        }

        private string platformString;
        public string PlatformString
        {
            get { return platformString; }
            set { platformString = value; this.RaisePropertyChanged(); }
        }

        private int lineNumber;
        public int LineNumber
        {
            get { return lineNumber; }
            set { lineNumber = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(() => LineText); }
        }

        private int column;
        public int Column
        {
            get { return column; }
            set { column = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(() => ColumnText); }
        }

        private int offset;
        public int Offset
        {
            get { return offset; }
            set { offset = value; this.RaisePropertyChanged(); this.RaisePropertyChanged(() => OffsetText); }
        }

        public string OffsetText
        {
            get
            {
                if (this.Offset >= 0)
                {
                    return string.Format("Loc {0}", Offset);
                }
                else
                {
                    return string.Empty;                    
                }
            }
        }

        private string language;
        public string Language
        {
            get { return language; }
            set { this.RaiseAndSetIfChanged(ref language, value); }
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
