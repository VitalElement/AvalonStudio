namespace AvalonStudio.Debugging
{
    public class MemoryBytes
    {
        public ulong Address { get; set; }

        public ulong Offset { get; set; }

        public ulong End { get; set; }

        public byte[] Data { get; set; }

        public string Values { get; set; }
    }
}