namespace AvalonStudio.Models.Platform
{
    using System;

    public class Platform
    {
        public static PlatformID PlatformID
        {
            get
            {
                return Environment.OSVersion.Platform;
            }
        }

        public static string PlatformString
        {
            get
            {
                string result = string.Empty;

                switch (PlatformID)
                {
                    case PlatformID.Win32NT:
                        result = Constants.WindowsPlatformString;
                        break;

                    case PlatformID.Unix:
                        result = Constants.LinuxPlatformString;
                        break;

                    case PlatformID.MacOSX:
                        result = Constants.MacOSXPlatformString;
                        break;

                    default:
                        result = Constants.UnknownPlatformString;
                        break;
                }

                return result;
            }
        }
    }
}
