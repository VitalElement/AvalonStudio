using System;
using System.Runtime.InteropServices;

namespace AvalonStudio.Extensibility.Platforms.Terminals.Unix
{
    internal class NativeDelegates
    {
        [Map]
        public enum FcntlCommand : int
        {
            // Form /usr/include/bits/fcntl.h
            F_DUPFD = 0, // Duplicate file descriptor.
            F_GETFD = 1, // Get file descriptor flags.
            F_SETFD = 2, // Set file descriptor flags.
            F_GETFL = 3, // Get file status flags.
            F_SETFL = 4, // Set file status flags.
            F_GETLK = 12, // Get record locking info. [64]
            F_SETLK = 13, // Set record locking info (non-blocking). [64]
            F_SETLKW = 14, // Set record locking info (blocking). [64]
            F_SETOWN = 8, // Set owner of socket (receiver of SIGIO).
            F_GETOWN = 9, // Get owner of socket (receiver of SIGIO).
            F_SETSIG = 10, // Set number of signal to be sent.
            F_GETSIG = 11, // Get number of signal to be sent.
            F_NOCACHE = 48, // OSX: turn data caching off/on for this fd.
            F_SETLEASE = 1024, // Set a lease.
            F_GETLEASE = 1025, // Enquire what lease is active.
            F_NOTIFY = 1026, // Required notifications on a directory
        }

        [DllImport("libdl.so.2", EntryPoint = "dlopen")]
        private static extern IntPtr dlopen_lin(string path, int flags);

        [DllImport("libdl.so.2", EntryPoint = "dlsym")]
        private static extern IntPtr dlsym_lin(IntPtr handle, string symbol);

        [DllImport("libSystem.dylib", EntryPoint = "dlopen")]
        private static extern IntPtr dlopen_mac(string path, int flags);

        [DllImport("libSystem.dylib", EntryPoint = "dlsym")]
        private static extern IntPtr dlsym_mac(IntPtr handle, string symbol);

        public static T GetProc<T>()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var dl = dlopen_mac("libSystem.dylib", 2);
                var a = dlsym_mac(dl, typeof(T).Name);
                return Marshal.GetDelegateForFunctionPointer<T>(a);
            }
            else
            {
                var dl = dlopen_lin("libc.6.so", 2);
                var a = dlsym_lin(dl, typeof(T).Name);
                return Marshal.GetDelegateForFunctionPointer<T>(a);
            }
        }

        public static T GetProc<T>(string function)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var dl = dlopen_mac("libSystem.dylib", 2);
                var a = dlsym_mac(dl, function);
                return Marshal.GetDelegateForFunctionPointer<T>(a);
            }
            else
            {
                var dl = dlopen_lin("libc.6.so", 2);
                var a = dlsym_lin(dl, function);
                return Marshal.GetDelegateForFunctionPointer<T>(a);
            }
        }

        public delegate void dup2(int oldfd, int newfd);
        public delegate int fork();
        public delegate void setsid();
        public delegate int ioctl(int fd, UInt64 ctl, int arg);
        public delegate int ioctl_wsize(int fd, UInt64 ctl, [MarshalAs(UnmanagedType.LPStruct)]ref Native.winsize arg);
        public delegate void close(int fd);
        public delegate int open([MarshalAs(UnmanagedType.LPStr)] string file, int flags);
        public delegate IntPtr ptsname(int fd);
        public delegate int grantpt(int fd);
        public delegate int unlockpt(int fd);
        public unsafe delegate void execve([MarshalAs(UnmanagedType.LPStr)]string path, [MarshalAs(UnmanagedType.LPArray)]string[] argv, [MarshalAs(UnmanagedType.LPArray)]string[] envp);
        public delegate int read(int fd, IntPtr buffer, int length);
        public delegate int write(int fd, IntPtr buffer, int length);
        public delegate void free(IntPtr ptr);
        public delegate int pipe(IntPtr[] fds);
        public delegate int setpgid(int pid, int pgid);
        public delegate int posix_spawn_file_actions_adddup2(IntPtr file_actions, int fildes, int newfildes);
        public delegate int posix_spawn_file_actions_addclose(IntPtr file_actions, int fildes);
        public delegate int posix_spawn_file_actions_init(IntPtr file_actions);
        public delegate int posix_spawnattr_init(IntPtr attributes);
        public delegate int posix_spawnp(out IntPtr pid, string path, IntPtr fileActions, IntPtr attrib, string[] argv, string[] envp);
        public delegate int dup(int fd);
        public delegate void _exit(int code);
        public delegate int getdtablesize();
    }

    internal static class Native
    {
        public const int O_RDONLY = 0x0000;
        public const int O_WRONLY = 0x0001;
        public const int O_RDWR = 0x0002;
        public const int O_ACCMODE = 0x0003;

        public const int O_CREAT = 0x0100; /* second byte, away from DOS bits */
        public const int O_EXCL = 0x0200;
        public const int O_NOCTTY = 0x0400;
        public const int O_TRUNC = 0x0800;
        public const int O_APPEND = 0x1000;
        public const int O_NONBLOCK = 0x2000;

        public const int TIOCGWINSZ = 0x5413;
        public const ulong TIOCSWINSZ = 0x0000000080087467;

        public const int _SC_OPEN_MAX = 5;

        public const int EAGAIN = 11;  /* Try again */

        public const int EINTR = 4; /* Interrupted system call */

        public const int ENOENT = 2;

        public const int TIOCSCTTY = 0x540E;

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct winsize
        {
            public ushort ws_row;   /* rows, in characters */
            public ushort ws_col;   /* columns, in characters */
            public ushort ws_xpixel;    /* horizontal size, pixels */
            public ushort ws_ypixel;    /* vertical size, pixels */
        };

        public static NativeDelegates.open open = NativeDelegates.GetProc<NativeDelegates.open>();
        public static NativeDelegates.write write = NativeDelegates.GetProc<NativeDelegates.write>();
        public static NativeDelegates.grantpt grantpt = NativeDelegates.GetProc<NativeDelegates.grantpt>();
        public static NativeDelegates.unlockpt unlockpt = NativeDelegates.GetProc<NativeDelegates.unlockpt>();
        public static NativeDelegates.ptsname ptsname = NativeDelegates.GetProc<NativeDelegates.ptsname>();
        public static NativeDelegates.posix_spawn_file_actions_init posix_spawn_file_actions_init = NativeDelegates.GetProc<NativeDelegates.posix_spawn_file_actions_init>();
        public static NativeDelegates.posix_spawn_file_actions_adddup2 posix_spawn_file_actions_adddup2 = NativeDelegates.GetProc<NativeDelegates.posix_spawn_file_actions_adddup2>();
        public static NativeDelegates.posix_spawn_file_actions_addclose posix_spawn_file_actions_addclose = NativeDelegates.GetProc<NativeDelegates.posix_spawn_file_actions_addclose>();
        public static NativeDelegates.posix_spawnattr_init posix_spawnattr_init = NativeDelegates.GetProc<NativeDelegates.posix_spawnattr_init>();
        public static NativeDelegates.posix_spawnp posix_spawnp = NativeDelegates.GetProc<NativeDelegates.posix_spawnp>();
        public static NativeDelegates.dup dup = NativeDelegates.GetProc<NativeDelegates.dup>();
        public static NativeDelegates.setsid setsid = NativeDelegates.GetProc<NativeDelegates.setsid>();
        public static NativeDelegates.ioctl ioctl = NativeDelegates.GetProc<NativeDelegates.ioctl>();
        public static NativeDelegates.ioctl_wsize ioctl_wsize = NativeDelegates.GetProc<NativeDelegates.ioctl_wsize>();
        public static NativeDelegates.execve execve = NativeDelegates.GetProc<NativeDelegates.execve>();




    }
}
