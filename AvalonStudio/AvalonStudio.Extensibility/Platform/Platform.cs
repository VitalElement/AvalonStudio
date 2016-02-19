namespace AvalonStudio.Extensibility.Platform
{
    using AvalonStudio.Utils;
    using System;
    using System.IO;
    using Utils;

    public class Platform
    {
        public static void Initialise ()
        {
            if(!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            if(!Directory.Exists(AppDataDirectory))
            {
                Directory.CreateDirectory(AppDataDirectory);
            }

            if(!Directory.Exists(ReposDirectory))
            {
                Directory.CreateDirectory(ReposDirectory);
            }

            if(!Directory.Exists(RepoCatalogDirectory))
            {
                Directory.CreateDirectory(RepoCatalogDirectory);
            }
        }

        public static char DirectorySeperator
        {
            get
            {
				switch (PlatformIdentifier) 
				{
				case  PlatformID.Unix:
					{
						return '/';
					}

				case PlatformID.Win32NT:
					{
						return '\\';
					}

				default:
					throw new NotImplementedException("Not implemented for your platform.");
				}                
            }
        }

        public static PlatformID PlatformIdentifier
        {
            get
            {
                return Environment.OSVersion.Platform;
            }
        }

        public static string BaseDirectory
        {
            get
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Win32NT:
                        return "c:\\AvalonStudio";

				case PlatformID.Unix:
					var homeDir = Environment.GetEnvironmentVariable ("HOME");

					return Path.Combine (homeDir, "AvalonStudio");

                    default:
                        throw new NotImplementedException("Not implemented for your platform.");
                }
            }
        }

        public static string ProjectDirectory
        {
            get { return Path.Combine(BaseDirectory, "Projects"); }
        }

        public static string AppDataDirectory
        {
            get { return Path.Combine(BaseDirectory, "AppData"); }
        }

        public static string ReposDirectory
        {
            get
            {
                return Path.Combine(AppDataDirectory, "Repos");
            }
        }

        public static string RepoCatalogDirectory
        {
            get
            {
                return Path.Combine(AppDataDirectory, "RepoCatalog");
            }
        }

        public static string PackageSourcesFile
        {
            get
            {
                return Path.Combine(AppDataDirectory, "PackageSources.json");
            }
        }

        public static string PlatformString
        {
            get
            {
                string result = string.Empty;

                switch (PlatformIdentifier)
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
