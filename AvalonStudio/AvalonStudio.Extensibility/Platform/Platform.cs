using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using AvalonStudio.Utils;
using System.Diagnostics;

namespace AvalonStudio.Platforms
{
    public enum PlatformID
    {
        Windows,
        MacOSX,
        Linux
    }

    public static class Platform
    {
        public delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

        public enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        [Map]
        public enum Signum
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

        public static string ExecutionPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string ExtensionsFolder => Path.Combine(ExecutionPath, "Extensions");

        public static string TemplatesFolder => Path.Combine(ExecutionPath, "Templates");

        public static string ExecutableExtension
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    case PlatformID.Linux:
                    case PlatformID.MacOSX:
                        {
                            return string.Empty;
                        }

                    case PlatformID.Windows:
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
                return Path.DirectorySeparatorChar;
            }
        }


        public static Architecture OSArchitecture => RuntimeInformation.OSArchitecture;
        public static string OSDescription => RuntimeInformation.OSDescription;

        public static PlatformID PlatformIdentifier
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return PlatformID.Windows;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return PlatformID.Linux;
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return PlatformID.MacOSX;
                }
                
                throw new Exception("Unknow platform");
            }
        }
        

        /// <summary>
        /// The base directory for AvalonStudio's data
        /// </summary>
		public static string BaseDirectory
        {
            get
            {
                string userDir = string.Empty;

                switch (PlatformIdentifier)
                {
                    case PlatformID.Windows:
                        userDir = Environment.GetEnvironmentVariable("UserProfile");                        
                        break;

                    default:
                        userDir = Environment.GetEnvironmentVariable("HOME");
                        break;
                }

                return Path.Combine(userDir, "AvalonStudio");
            }
        }

        public static string ProjectDirectory => Path.Combine(BaseDirectory, "Projects");

        public static string SettingsDirectory => Path.Combine(BaseDirectory, "Settings");

        public static string CacheDirectory => Path.Combine(BaseDirectory, "Cache");

        public static string AppDataDirectory => Path.Combine(BaseDirectory, "AppData");

        public static string ReposDirectory => Path.Combine(AppDataDirectory, "Repos");

        public static string RepoCatalogDirectory => Path.Combine(AppDataDirectory, "RepoCatalog");

        public static string PackageSourcesFile => Path.Combine(AppDataDirectory, "PackageSources.json");

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

            if (!Directory.Exists(CacheDirectory))
            {
                Directory.CreateDirectory(CacheDirectory);
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

        public static void OpenFolderInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Windows:
                        Process.Start(new ProcessStartInfo { FileName = "cmd.exe", Arguments = $"/c start {path}", CreateNoWindow = true });
                        break;

                    case PlatformID.Linux:
                        Process.Start(new ProcessStartInfo { FileName = "xdg-open", Arguments = path, CreateNoWindow = true });
                        break;

                    default:
                        break;
                }
            }
        }

        [DllImport(LIB, EntryPoint = "Mono_Posix_FromSignum")]
        private static extern int FromSignum(Signum value, out int rval);


        private static bool TryFromSignum(Signum value, out int rval)
        {
            return FromSignum(value, out rval) == 0;
        }


        private static int FromSignum(Signum value)
        {
            int rval;
            if (FromSignum(value, out rval) == -1)
                throw new ArgumentException();
            return rval;
        }


        [DllImport(LIBC, SetLastError = true, EntryPoint = "kill")]
        private static extern int sys_kill(int pid, int sig);

        private static int kill(int pid, Signum sig)
        {
            var _sig = FromSignum(sig);
            return sys_kill(pid, _sig);
        }

        public static int SendSignal(int pid, Signum sig)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Linux:
                case PlatformID.MacOSX:
                    return kill(pid, sig);

                case PlatformID.Windows:
                    switch (sig)
                    {
                        case Signum.SIGINT:
                            return SendCtrlC();

                        default:
                            throw new NotImplementedException();
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        public static bool AttachConsole(int pid)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Windows:
                    return Win32AttachConsole(pid);

                default:
                    return true;
            }
        }

        public static bool FreeConsole()
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Windows:
                    return Win32FreeConsole();

                default:
                    return true;
            }
        }

        public static bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Windows:
                    return Win32SetConsoleCtrlHandler(handlerRoutine, add);

                default:
                    return true;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "AttachConsole")]
        private static extern bool Win32AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, EntryPoint = "FreeConsole")]
        private static extern bool Win32FreeConsole();


        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);

        [DllImport("kernel32.dll", EntryPoint = "SetConsoleCtrlHandler")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool Win32SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add);

        private static int SendCtrlC()
        {
            return GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0) ? 1 : 0;
        }

        public static string ToAvalonPath(this string path)
        {
            return path.Replace('\\', '/');
        }

        public static string NormalizePath(this string path)
        {
            string result = path.ToPlatformPath();

            DirectoryInfo info = new DirectoryInfo(result);

            result = info.FullName;

            return result;
        }

        public static string ToPlatformPath(this string path)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Windows:
                    return path.Replace('/', '\\');

                default:
                    return path.ToAvalonPath();
            }
        }

        public static bool IsSamePathAs(this string path, string other)
        {
            return path.CompareFilePath(other) == 0;
        }

        public static int CompareFilePath(this string path, string other)
        {
            if (other != null && path != null)
            {
                path = path.NormalizePath().ToAvalonPath();
                other = other.NormalizePath().ToAvalonPath();

                if (other.EndsWith("/") && !path.EndsWith("/"))
                {
                    path += "/";
                }
                else if (path.EndsWith("/") && !other.EndsWith("/"))
                {
                    other +="/";
                }
            }

            switch (PlatformIdentifier)
            {
                case PlatformID.Windows:
                    // TODO consider using directory info?           
                    if (path == null && other == null)
                    {
                        return 0;
                    }
                    if (path == null)
                    {
                        return 1;
                    }
                    if (other == null)
                    {
                        return -1;
                    }
                    return path.ToLower().CompareTo(other.ToLower());

                default:
                    return path.CompareTo(other);
            }
        }
    }
}