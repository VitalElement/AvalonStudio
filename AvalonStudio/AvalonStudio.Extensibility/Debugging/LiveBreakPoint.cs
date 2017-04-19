namespace AvalonStudio.Debugging
{
    public class LiveBreakPoint : BreakPoint
    {
        public enum BreakPointType
        {
            BreakPoint,
            WatchPoint
        }

        public int Number { get; set; }
        public BreakPointType Type { get; set; }

        //catchtype
        public bool Visible { get; set; }

        public bool Enabled { get; set; }
        public ulong Address { get; set; }
        public string Function { get; set; }
        public string FullFileName { get; set; }

        //public string At { get; set; } // if file unknown
        //pending
        //evaluated-by
        //thread
        //task
        //cond
        //ignore
        // enable count
        //thread groups ? list<int>ThreadGroups?
        public int HitCount { get; set; }

        public string OriginalLocation { get; set; }
    }
}