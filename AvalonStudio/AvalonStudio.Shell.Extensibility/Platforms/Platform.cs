using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace AvalonStudio.Shell.Extensibility.Platforms
{
    public class Platform
    {
        public static string AppName { get; set; }

        public static string SettingsDirectory => Path.Combine(BaseDirectory, "Settings");

        public static void Initialise()
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            if (!Directory.Exists(SettingsDirectory))
            {
                Directory.CreateDirectory(SettingsDirectory);
            }

            if (!Directory.Exists(ExtensionsFolder))
            {
                Directory.CreateDirectory(ExtensionsFolder);
            }
        }

        public static PlatformID PlatformIdentifier
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return PlatformID.Win32NT;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return PlatformID.Unix;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return PlatformID.MacOSX;
                }

                throw new Exception("Unknow platform");
            }
        }

        public static string ExecutionPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string ExtensionsFolder => Path.Combine(ExecutionPath, "Extensions");

        public static string BaseDirectory
        {
            get
            {
                string userDir = string.Empty;

                switch (PlatformIdentifier)
                {
                    case PlatformID.Win32NT:
                        userDir = Environment.GetEnvironmentVariable("UserProfile");
                        break;

                    default:
                        userDir = Environment.GetEnvironmentVariable("HOME");
                        break;
                }

                return Path.Combine(userDir, AppName);
            }
        }
    }
}
