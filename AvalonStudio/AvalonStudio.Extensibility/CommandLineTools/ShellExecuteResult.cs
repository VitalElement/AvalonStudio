namespace AvalonStudio.CommandLineTools
{
    public struct ShellExecuteResult
    {
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public string ErrorOutput { get; set; }
    }
}