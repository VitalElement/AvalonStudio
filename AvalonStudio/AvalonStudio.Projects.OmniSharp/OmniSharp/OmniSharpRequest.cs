namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    public abstract class OmniSharpRequest<T>
    {
        public OmniSharpRequest()
        {
            Line = 1;
            Column = 1;
        }

        public abstract string EndPoint { get; }
        public int Line { get; set; }
        public int Column { get; set; }
        public string Buffer { get; set; }
        public string FileName { get; set; }
    }
}