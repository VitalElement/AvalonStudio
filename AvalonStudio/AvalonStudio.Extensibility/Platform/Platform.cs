namespace AvalonStudio.Platform
{
    using AvalonStudio.Utils;
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using Utils;

    public static class Platform
    {
        public static void Initialise()
        {
            if (!Directory.Exists(BaseDirectory))
            {
                Directory.CreateDirectory(BaseDirectory);
            }

            if (!Directory.Exists(AppDataDirectory))
            {
                Directory.CreateDirectory(AppDataDirectory);
            }

            if (!Directory.Exists(ReposDirectory))
            {
                Directory.CreateDirectory(ReposDirectory);
            }

            if (!Directory.Exists(RepoCatalogDirectory))
            {
                Directory.CreateDirectory(RepoCatalogDirectory);
            }
        }

        [Map]
        public enum Signum : int
        {
            SIGHUP = 1, // Hangup (POSIX).
            SIGINT = 2, // Interrupt (ANSI).
            SIGQUIT = 3, // Quit (POSIX).
            SIGILL = 4, // Illegal instruction (ANSI).
            SIGTRAP = 5, // Trace trap (POSIX).
            SIGABRT = 6, // Abort (ANSI).
            SIGIOT = 6, // IOT trap (4.2 BSD).
            SIGBUS = 7, // BUS error (4.2 BSD).
            SIGFPE = 8, // Floating-point exception (ANSI).
            SIGKILL = 9, // Kill, unblockable (POSIX).
            SIGUSR1 = 10, // User-defined signal 1 (POSIX).
            SIGSEGV = 11, // Segmentation violation (ANSI).
            SIGUSR2 = 12, // User-defined signal 2 (POSIX).
            SIGPIPE = 13, // Broken pipe (POSIX).
            SIGALRM = 14, // Alarm clock (POSIX).
            SIGTERM = 15, // Termination (ANSI).
            SIGSTKFLT = 16, // Stack fault.
            SIGCLD = SIGCHLD, // Same as SIGCHLD (System V).
            SIGCHLD = 17, // Child status has changed (POSIX).
            SIGCONT = 18, // Continue (POSIX).
            SIGSTOP = 19, // Stop, unblockable (POSIX).
            SIGTSTP = 20, // Keyboard stop (POSIX).
            SIGTTIN = 21, // Background read from tty (POSIX).
            SIGTTOU = 22, // Background write to tty (POSIX).
            SIGURG = 23, // Urgent condition on socket (4.2 BSD).
            SIGXCPU = 24, // CPU limit exceeded (4.2 BSD).
            SIGXFSZ = 25, // File size limit exceeded (4.2 BSD).
            SIGVTALRM = 26, // Virtual alarm clock (4.2 BSD).
            SIGPROF = 27, // Profiling alarm clock (4.2 BSD).
            SIGWINCH = 28, // Window size change (4.3 BSD, Sun).
            SIGPOLL = SIGIO, // Pollable event occurred (System V).
            SIGIO = 29, // I/O now possible (4.2 BSD).
            SIGPWR = 30, // Power failure restart (System V).
            SIGSYS = 31, // Bad system call.
            SIGUNUSED = 31
        }

        internal const string LIBC = "libc";
        private const string LIB = "MonoPosixHelper";

        [DllImport(LIB, EntryPoint = "Mono_Posix_FromSignum")]
        private static extern int FromSignum(Signum value, out Int32 rval);


        public static bool TryFromSignum(Signum value, out Int32 rval)
        {
            return FromSignum(value, out rval) == 0;
        }


        public static Int32 FromSignum(Signum value)
        {
            Int32 rval;
            if (FromSignum(value, out rval) == -1)
                throw new ArgumentException();
            return rval;
        }


        [DllImport(LIBC, SetLastError = true, EntryPoint = "kill")]
        private static extern int sys_kill(int pid, int sig);

        private static int kill(int pid, Signum sig)
        {
            int _sig = FromSignum(sig);
            return sys_kill(pid, _sig);
        }


        public static int SendSignal(int pid, Signum sig)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Unix:
                    return kill(pid, sig);

                case PlatformID.Win32NT:
                default:
                    throw new NotImplementedException();
            }
        }

        public static string ToAvalonPath(this string path)
        {
            return path.Replace('\\', '/');
        }

        public static string ToPlatformPath(this string path)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Win32NT:
                    return path.Replace('/', '\\');

                default:
                    return path.ToAvalonPath();
            }
        }

        public static string ExecutionPath
        {
            get
            {
                return Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            }
        }

        public static string PluginsDirectory
        {
            get
            {
                return Path.Combine(ExecutionPath, "Plugins");
            }
        }

        public static string ExecutableExtension
        {
            get
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Unix:
                        {
                            return string.Empty;
                        }

                    case PlatformID.Win32NT:
                        {
                            return ".exe";
                        }

                    default:
                        throw new NotImplementedException("Not implemented for your platform.");
                }
            }
        }

        public static char DirectorySeperator
        {
            get
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Unix:
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
                        var homeDir = Environment.GetEnvironmentVariable("HOME");

                        return Path.Combine(homeDir, "AvalonStudio");

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
