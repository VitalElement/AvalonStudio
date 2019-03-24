using AvalonStudio.Projects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices;
using AvalonStudio.CommandLineTools;

namespace AvalonStudio.Platforms
{
    public enum PlatformID
    {
        Win32S = 0,
        Win32Windows = 1,
        Win32NT = 2,
        WinCE = 3,
        Unix = 4,
        Xbox = 5,
        MacOSX = 6
    }

    public static class Platform
    {
        public delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

        [DllImport("libc")]
        private static extern void chmod(string file, int mode);

        public static void Chmod(string file, int mode)
        {
            if (PlatformIdentifier != PlatformID.Win32NT)
            {
                chmod(file, mode);
            }
        }

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

        public static string AvalonRID
        {
            get
            {
                string result = "UnknownRid";

                switch (Platform.PlatformIdentifier)
                {
                    case PlatformID.Win32NT:
                        result = "win-";
                        break;

                    case PlatformID.MacOSX:
                        result = "osx-";
                        break;

                    case PlatformID.Unix:
                        result = "linux-";
                        break;
                }

                switch (Platform.OSArchitecture)
                {
                    case Architecture.X64:
                        result += "x64";
                        break;

                    case Architecture.X86:
                        result += "x86";
                        break;

                    case Architecture.Arm:
                        result += "ARM";
                        break;

                    case Architecture.Arm64:
                        result += "ARM64";
                        break;
                }

                return result;
            }
        }

        private static string numberPattern = " ({0})";

        public static string NextAvailableDirectoryName(string path)
        {
            // Short-cut if already available
            if (!Directory.Exists(path))
                return path;

            // Otherwise just append the pattern to the path and return next filename
            return GetNextDirectoryName(path + numberPattern);
        }

        public static string NextAvailableFileName(string path)
        {
            // Short-cut if already available
            if (!File.Exists(path))
                return path;

            // If path has extension then insert the number pattern just before the extension and return next filename
            if (Path.HasExtension(path))
                return GetNextFileName(path.Insert(path.LastIndexOf(Path.GetExtension(path)), numberPattern));

            // Otherwise just append the pattern to the path and return next filename
            return GetNextFileName(path + numberPattern);
        }

        private static string GetNextDirectoryName(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!Directory.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (Directory.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (Directory.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        private static string GetNextFileName(string pattern)
        {
            string tmp = string.Format(pattern, 1);
            if (tmp == pattern)
                throw new ArgumentException("The pattern must include an index place-holder", "pattern");

            if (!File.Exists(tmp))
                return tmp; // short-circuit if no matches

            int min = 1, max = 2; // min is inclusive, max is exclusive/untested

            while (File.Exists(string.Format(pattern, max)))
            {
                min = max;
                max *= 2;
            }

            while (max != min + 1)
            {
                int pivot = (max + min) / 2;
                if (File.Exists(string.Format(pattern, pivot)))
                    min = pivot;
                else
                    max = pivot;
            }

            return string.Format(pattern, max);
        }

        public static IDictionary EnvironmentVariables => Environment.GetEnvironmentVariables();

        private const string UserDataDir = ".as";

        public static string ExecutionPath => Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string ExtensionsFolder => Path.Combine(ExecutionPath, "Extensions");

        public static string NativeFolder
        {
            get
            {
                string osdir = string.Empty;

                switch (PlatformIdentifier)
                {
                    case PlatformID.Win32NT:
                        osdir = "win7-x64";
                        break;

                    case PlatformID.Unix:
                        osdir = "unix";
                        break;

                    case PlatformID.MacOSX:
                        osdir = "mac";
                        break;
                }

                return Path.Combine(ExecutionPath, "native", osdir);
            }
        }

        public static string DLLExtension
        {
            get
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Unix:
                        return ".so";

                    case PlatformID.MacOSX:
                        return ".dylib";

                    case PlatformID.Win32NT:
                        return ".dll";

                    default:
                        throw new NotImplementedException("Not implemented for your platform.");
                }
            }
        }

        public static string PathSeperator
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
                        {
                            return ":";
                        }

                    case PlatformID.Win32NT:
                        {
                            return ";";
                        }

                    default:
                        throw new NotImplementedException("Not implemented for your platform.");
                }
            }
        }

        public static string ExecutableExtension
        {
            get
            {
                switch (Platform.PlatformIdentifier)
                {
                    case PlatformID.Unix:
                    case PlatformID.MacOSX:
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
                return Path.DirectorySeparatorChar;
            }
        }

        public static Architecture OSArchitecture => RuntimeInformation.OSArchitecture;
        public static string OSDescription => RuntimeInformation.OSDescription;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]

        internal static extern byte CreateSymbolicLinkW(string lpSymlinkFileName, string lpTargetFileName, uint dwFlags);

        internal const uint SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 2;

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]

        internal static extern byte CreateHardLinkW(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);

        public static bool CreateSymbolicLinkWin32(string linkName, string target, bool isFile)
        {
            return CreateSymbolicLinkW(linkName, target, SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE) != 0;
        }

        public static bool CreateHardLinkWin32(string linkName, string target, bool isFile)
        {
            return CreateHardLinkW(linkName, target, IntPtr.Zero) != 0;
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
                    case PlatformID.Win32NT:
                        userDir = Environment.GetEnvironmentVariable("UserProfile");
                        break;

                    default:
                        userDir = Environment.GetEnvironmentVariable("HOME");
                        break;
                }

                return Path.Combine(userDir, "AvalonStudio");
            }
        }

        public static void EnsureSolutionUserDataDirectory(ISolution solution)
        {
            if (!Directory.Exists(GetUserDataDirectory(solution)))
            {
                Directory.CreateDirectory(GetUserDataDirectory(solution));
            }
        }

        public static string GetUserDataDirectory(ISolution solution)
        {
            return Path.Combine(solution.CurrentDirectory, UserDataDir);
        }

        public static string GetSolutionSnippetDirectory(ISolution solution)
        {
            return Path.Combine(solution.CurrentDirectory, "Snippets");
        }

        public static string GetProjectSnippetDirectory(IProject project)
        {
            return Path.Combine(project.CurrentDirectory, "Snippets");
        }

        public static string ProjectDirectory => Path.Combine(BaseDirectory, "Projects");

        public static string PackageDirectory => Path.Combine(BaseDirectory, "Packages");

        public static string SettingsDirectory => Path.Combine(BaseDirectory, "Settings");

        public static string TemplatesFolder => Path.Combine(ExecutionPath, "Templates");

        public static string InBuiltSnippetsFolder => Path.Combine(ExecutionPath, "Snippets");

        public static string SnippetsFolder => Path.Combine(BaseDirectory, "Snippets");

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

            if (!Directory.Exists(PackageDirectory))
            {
                Directory.CreateDirectory(PackageDirectory);
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

            if (!Directory.Exists(SnippetsFolder))
            {
                Directory.CreateDirectory(SnippetsFolder);
            }

            if (!Directory.Exists(InBuiltSnippetsFolder))
            {
                Directory.CreateDirectory(InBuiltSnippetsFolder);
            }

            if (!Directory.Exists(ExtensionsFolder))
            {
                Directory.CreateDirectory(ExtensionsFolder);
            }

            if (Platform.PlatformIdentifier == PlatformID.MacOSX)
            {
                var paths = PlatformSupport.GetSystemPaths();

                Environment.SetEnvironmentVariable("PATH", string.Join(":", paths));
            }
        }

        public static void OpenFolderInExplorer(string path)
        {
            if (Directory.Exists(path))
            {
                switch (PlatformIdentifier)
                {
                    case PlatformID.Win32NT:
                        Process.Start(new ProcessStartInfo { FileName = "explorer.exe", Arguments = $"\"{path}\"" });
                        break;

                    case PlatformID.Unix:
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
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    return kill(pid, sig);

                case PlatformID.Win32NT:
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
                case PlatformID.Win32NT:
                    return Win32AttachConsole(pid);

                default:
                    return true;
            }
        }

        public static bool FreeConsole()
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Win32NT:
                    return Win32FreeConsole();

                default:
                    return true;
            }
        }

        public static bool SetConsoleCtrlHandler(ConsoleCtrlDelegate handlerRoutine, bool add)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Win32NT:
                    return Win32SetConsoleCtrlHandler(handlerRoutine, add);

                default:
                    return true;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "AttachConsole")]
        private static extern bool Win32AttachConsole(int processId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, EntryPoint = "FreeConsole")]
        private static extern bool Win32FreeConsole();

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes ctrlEvent, uint processGroupId);

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
            string result = path?.Replace("\\\\", "\\").ToPlatformPath();

            if (!string.IsNullOrEmpty(result))
            {
                DirectoryInfo info = new DirectoryInfo(result);

                result = info.FullName;
            }

            return result;
        }

        public static string ToPlatformPath(this string path)
        {
            switch (PlatformIdentifier)
            {
                case PlatformID.Win32NT:
                    return path.Replace('/', '\\').Trim();

                default:
                    return path.ToAvalonPath().Trim();
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
                    other += "/";
                }
            }

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

            switch (PlatformIdentifier)
            {
                case PlatformID.Win32NT:
                    // TODO consider using directory info?
                    return path.ToLower().CompareTo(other.ToLower());

                default:

                    return path.CompareTo(other);
            }
        }
    }
}