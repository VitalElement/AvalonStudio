using System;
using System.Runtime.InteropServices;

namespace AvalonStudio.Extensibility.Platforms.Terminals.Unix
{
    internal class Native
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
        public const ulong TIOCSWINSZ =	0x0000000080087467;

        public const int _SC_OPEN_MAX = 5;

        public const int EAGAIN = 11;  /* Try again */

        public const int EINTR = 4; /* Interrupted system call */

        public const int ENOENT = 2;

        public const int TIOCSCTTY = 0x540E;

        //int open(const char *pathname, int flags);
        [DllImport("libc")]
        internal static extern int open(string name, int flags);

        [DllImport("libSystem.dylib")]
        internal static extern int close(int fd);

        [DllImport("libSystem.dylib")]
        internal static extern int dup(int fd);

        



        //ssize_t read(int fd, void *buf, size_t count);
        [DllImport("libSystem.dylib")]
        internal static extern int read(int fd, IntPtr buffer, int length);

        //ssize_t write(int fd, const void *buf, size_t count); 
        [DllImport("libSystem.dylib")]
        internal static extern int write(int fd, IntPtr buffer, int length);

        //int grantpt(int fd);
        [DllImport("libSystem.dylib")]
        internal static extern int grantpt(int fd);

        //int unlockpt(int fd);
        [DllImport("libSystem.dylib")]
        internal static extern int unlockpt(int fd);

        //i later marshall the pointer to a string
        //char *ptsname(int fd);
        [DllImport("libSystem.dylib")]
        internal static extern IntPtr ptsname(int fd);

        [DllImport("libSystem.dylib")]
        internal static extern void free(IntPtr ptr);

        [DllImport("libSystem.dylib")]
        internal static extern int pipe(IntPtr[] fds);


        [DllImport("libSystem.dylib")]
        internal static extern void setsid();

        [DllImport("libSystem.dylib")]
        internal static extern int setpgid(int pid, int pgid);

        [DllImport("libSystem.dylib")]
        internal static extern int posix_spawn_file_actions_adddup2(IntPtr file_actions, int fildes, int newfildes);
        
        [DllImport("libSystem.dylib")]
        internal static extern int posix_spawn_file_actions_addclose(IntPtr file_actions, int fildes); 

        [DllImport("libSystem.dylib")]
        internal static extern int posix_spawn_file_actions_init(IntPtr file_actions);
        
        [DllImport("libSystem.dylib")]
        internal static extern int posix_spawnattr_init(IntPtr attributes);
        

        [DllImport("libSystem.dylib")]
        internal static extern int posix_spawnp(out IntPtr pid, string path, IntPtr fileActions, IntPtr attrib, string[] argv, string[] envp);

        [DllImport("libSystem.dylib")]
        internal static extern  int execve(string filename, string[] argv,string[] envp);



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

        [DllImport("libSystem.dylib")]
        public static extern long sysconf(int name);

        [DllImport("libSystem.dylib")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd);

        [DllImport("libSystem.dylib")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, long arg);

        [DllImport("libSystem.dylib")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, int arg);

        [DllImport("libSystem.dylib")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, IntPtr ptr);

        [DllImport("libSystem.dylib", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int ioctl(int handle, IntPtr request);


        [DllImport("libSystem.dylib", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int ioctl(int handle, uint request, IntPtr BufferSizeBytes);


        [DllImport("libSystem.dylib", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int ioctl(int handle, ulong request, ref winsize winsize);

        
        



        [StructLayout(LayoutKind.Sequential, Pack=1)]
        public struct winsize
        {
            public ushort ws_row;   /* rows, in characters */
            public ushort ws_col;   /* columns, in characters */
            public ushort ws_xpixel;    /* horizontal size, pixels */
            public ushort ws_ypixel;    /* vertical size, pixels */
        };


    }
}
