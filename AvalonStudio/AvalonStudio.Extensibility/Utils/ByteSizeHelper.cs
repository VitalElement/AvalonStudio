namespace AvalonStudio.Extensibility.Utils
{
    public static class ByteSizeHelper
    {
        private static readonly string[] Prefixes =
        {
            "B",
            "KB",
            "MB",
            "GB",
            "TB"
        };

        public static string ToString(double bytes)
        {
            var index = 0;
            while (bytes >= 1000.0)
            {
                bytes /= 1000.0;
                ++index;
            }
            return $"{bytes:N} {Prefixes[index]}";
        }
    }
}