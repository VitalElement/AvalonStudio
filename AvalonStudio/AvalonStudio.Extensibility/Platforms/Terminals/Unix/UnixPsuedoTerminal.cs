using AvalonStudio.Platforms.Terminals;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
        public const int TIOCSWINSZ = 0x5414;

        public const int _SC_OPEN_MAX = 5;

        public const int EAGAIN = 11;  /* Try again */

        public const int EINTR = 4; /* Interrupted system call */

        public const int ENOENT = 2;

        //int open(const char *pathname, int flags);
        [DllImport("libc.so.6")]
        internal static extern IntPtr open(string name, int flags);

        [DllImport("libc.so.6")]
        internal static extern int close(IntPtr fd);



        //ssize_t read(int fd, void *buf, size_t count);
        [DllImport("libc.so.6")]
        internal static extern int read(IntPtr fd, IntPtr buffer, int length);

        //ssize_t write(int fd, const void *buf, size_t count); 
        [DllImport("libc.so.6")]
        internal static extern int write(IntPtr fd, IntPtr buffer, int length);

        //int grantpt(int fd);
        [DllImport("libc.so.6")]
        internal static extern int grantpt(IntPtr fd);

        //int unlockpt(int fd);
        [DllImport("libc.so.6")]
        internal static extern int unlockpt(IntPtr fd);

        //i later marshall the pointer to a string
        //char *ptsname(int fd);
        [DllImport("libc.so.6")]
        internal static extern IntPtr ptsname(IntPtr fd);

        [DllImport("libc.so.6")]
        internal static extern void free(IntPtr ptr);

        [DllImport("libc.so.6")]
        internal static extern int pipe(IntPtr[] fds);


        [DllImport("libc.so.6")]
        internal static extern void setsid();

        [DllImport("libc.so.6")]
        internal static extern int setpgid(int pid, int pgid);



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

        [DllImport("libc.so.6")]
        public static extern long sysconf(int name);

        [DllImport("libc.so.6")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd);

        [DllImport("libc.so.6")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, long arg);

        [DllImport("libc.so.6")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, int arg);

        [DllImport("libc.so.6")]
        public static extern int fcntl(IntPtr fd, FcntlCommand cmd, IntPtr ptr);

        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int ioctl(IntPtr handle, IntPtr request);


        [DllImport("libc", EntryPoint = "ioctl", SetLastError = true)]
        internal static extern int ioctl(IntPtr handle, int request, IntPtr BufferSizeBytes);






        public struct winsize
        {
            public ushort ws_row;   /* rows, in characters */
            public ushort ws_col;   /* columns, in characters */
            public ushort ws_xpixel;    /* horizontal size, pixels */
            public ushort ws_ypixel;    /* vertical size, pixels */
        };


    }

    public class UnixPsuedoTerminal : IPsuedoTerminal
    {
        public static void Test()
        {
            var terminal = _vte_pty_open_unix98(out int child, new string[0], "/bin/bash", new string[0], "~/", 80, 20);



            var m = Native.open("/dev/ptmx", Native.O_RDWR | Native.O_NOCTTY);

            var result = Native.grantpt(m);

            result = Native.unlockpt(m);

            var namePtr = Native.ptsname(m);

            var name = Marshal.PtrToStringAnsi(namePtr);

        }

        static IntPtr _vte_pty_getpt()
        {
            IntPtr fd;
            int flags;
            /* Try to allocate a pty by accessing the pty master multiplex. */
            fd = Native.open("/dev/ptmx", Native.O_RDWR | Native.O_NOCTTY);
            if (((int)fd == -1) && (Marshal.GetLastWin32Error() == Native.ENOENT))
            {
                fd = Native.open("/dev/ptc", Native.O_RDWR | Native.O_NOCTTY); /* AIX */
            }
            /* Set it to blocking. */
            flags = Native.fcntl(fd, Native.FcntlCommand.F_GETFL);
            flags &= ~(Native.O_NONBLOCK);
            Native.fcntl(fd, Native.FcntlCommand.F_SETFL, flags);
            return fd;
        }

        static IntPtr _vte_pty_open_unix98(out int child, string[] envVars,
        string command, string[] argv, string directory, int columns, int rows)
        {
            child = -1;
            IntPtr fd = new IntPtr(-1);
            IntPtr buf;

            

            /* Attempt to open the master. */
            fd = _vte_pty_getpt();

            if ((int)fd != -1)
            {
                /* Read the slave number and unlock it. */
                if (((buf = Native.ptsname(fd)) == IntPtr.Zero) ||
                    (Native.grantpt(fd) != 0) ||
                    (Native.unlockpt(fd) != 0))
                {

                    Native.close(fd);
                    fd = new IntPtr(-1);
                }
                else
                {
                    var ptsname = Marshal.PtrToStringAnsi(buf);
                    /* Start up a child process with the given command. */
                    if (_vte_pty_fork_on_pty_name(ptsname, fd, envVars, command,
                                      argv, directory,
                                      columns, rows,
                                      out child) != 0)
                    {
                        Native.close(fd);
                        fd = new IntPtr(-1);
                    }
                    Native.free(buf);
                }
            }

            return fd;
        }

        /* Open the named PTY slave, fork off a child (storing its PID in child),
 * and exec the named command in its own session as a process group leader */
        static int _vte_pty_fork_on_pty_name(string path, IntPtr parent_fd, string[] env_add, string command, string[] argv,
                      string directory, int columns, int rows, out int child)
        {
            child = -1;
            IntPtr fd;
            int i;
            IntPtr c = IntPtr.Zero;
            IntPtr[] ready_a = new IntPtr[2];
            IntPtr[] ready_b = new IntPtr[2];
            int pid;

            /* Open pipes for synchronizing between parent and child. */
            if (_vte_pty_pipe_open_bi(ref ready_a[0], ref ready_a[1],
                          ref ready_b[0], ref ready_b[1]) == -1)
            {
                // Error setting up pipes.  Bail. 
                child = -1;
                return -1;
            }

            /* Start up a child. */
            var process = new Process();
            process.StartInfo = new ProcessStartInfo();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.FileName = "/bin/bash";
            process.Start();

            
            //pid = Native.fork();
            pid = process.Id;
            switch (pid)
            {
                case -1:
                    /* Error fork()ing.  Bail. */
                    child = -1;
                    return -1;
                    break;
                case 0:
                    /* Child. Close the parent's ends of the pipes. */
                    Native.close(ready_a[0]);
                    Native.close(ready_b[1]);

                    /* Start a new session and become process-group leader. */
                    Native.setsid();
                    Native.setpgid(0, 0);

                    /* Close most descriptors. */
                    /* for (i = 0; i < Native.sysconf(Native._SC_OPEN_MAX); i++)
                    {
                        if ((i != (int)ready_b[0]) && (i != (int)ready_a[1]))
                        {
                            Native.close(new IntPtr(i));
                        }
                    }*/

                    /* Open the slave PTY, acquiring it as the controlling terminal
                     * for this process and its children. */
                    fd = Native.open(path, Native.O_RDWR);
                    if ((int)fd == -1)
                    {
                        return -1;
                    }
                    /* # ifdef TIOCSCTTY
                                        // TIOCSCTTY is defined?  Let's try that, too. 
                                        Native.ioctl(fd, TIOCSCTTY, fd);
                    #endif*/
                    /* Store 0 as the "child"'s ID to indicate to the caller that
                     * it is now the child. */
                    child = 0;
                    throw new NotImplementedException();
                    //return _vte_pty_run_on_pty(fd, ready_b[0], ready_a[1], env_add, command, argv, directory);
                    break;
                default:
                    /* Parent.  Close the child's ends of the pipes, do the ready
                     * handshake, and return the child's PID. */
                    Native.close(ready_b[0]);
                    Native.close(ready_a[1]);

                    /* Wait for the child to be ready, set the window size, then
                     * signal that we're ready.  We need to synchronize here to
                     * avoid possible races when the child has to do more setup
                     * of the terminal than just opening it. */

                    n_read(ready_a[0], ref c, 1);

                    _pty_sharp_set_size(parent_fd, columns, rows);

                    n_write(ready_b[1], c, 1);
                    Native.close(ready_a[0]);
                    Native.close(ready_b[1]);

                    child = pid;
                    return 0;
                    break;
            }

            throw new Exception();
            //Native.g_assert_not_reached();
            return -1;
        }

        static int _vte_pty_pipe_open(ref IntPtr a, ref IntPtr b)
        {

            IntPtr[] p = new IntPtr[2];
            int ret = -1;

            ret = Native.pipe(p);

            if (ret == 0)
            {
                a = p[0];
                b = p[1];
            }
            return ret;
        }

        static int _vte_pty_pipe_open_bi(ref IntPtr a, ref IntPtr b, ref IntPtr c, ref IntPtr d)
        {
            int ret;
            ret = _vte_pty_pipe_open(ref a, ref b);
            if (ret != 0)
            {
                return ret;
            }
            ret = _vte_pty_pipe_open(ref c, ref d);
            if (ret != 0)
            {
                Native.close(a);
                Native.close(b);
            }
            return ret;
        }

        /* Like read, but hide EINTR and EAGAIN. */
        static int n_read(IntPtr fd, ref IntPtr buffer, int count)
        {
            int n = 0;
            int buf = (int)buffer;
            int i;
            while (n < count)
            {
                i = Native.read(fd, new IntPtr(buf + n), count - n);
                switch (i)
                {
                    case 0:
                        return n;
                        break;
                    case -1:
                        switch (Marshal.GetLastWin32Error())
                        {
                            case Native.EINTR:
                            case Native.EAGAIN:

                                break;
                            default:
                                return -1;
                        }
                        break;
                    default:
                        n += i;
                        break;
                }
            }
            return n;
        }



        /* Like write, but hide EINTR and EAGAIN. */
        static int n_write(IntPtr fd, IntPtr buf, int count)
        {
            int n = 0;
            int i;
            while (n < count)
            {
                i = Native.write(fd, new IntPtr((int)buf + n), count - n);
                switch (i)
                {
                    case 0:
                        return n;
                        break;
                    case -1:
                        switch (Marshal.GetLastWin32Error())
                        {
                            case Native.EINTR:
                            case Native.EAGAIN:
                                break;
                            default:
                                return -1;
                        }
                        break;
                    default:
                        n += i;
                        break;
                }
            }
            return n;
        }

        /**
 * _pty_sharp_set_size:
 * @master: the file descriptor of the pty master
 * @columns: the desired number of columns
 * @rows: the desired number of rows
 *
 * Attempts to resize the pseudo terminal's window size.  If successful, the
 * OS kernel will send #SIGWINCH to the child process group.
 *
 * Returns: 0 on success, -1 on failure.
 */
        static int _pty_sharp_set_size(IntPtr master, int columns, int rows)
        {

            Native.winsize size = new Native.winsize();
            int ret;
            size.ws_row = (ushort)(rows > 0 ? rows : 24);
            size.ws_col = (ushort)(columns > 0 ? columns : 80);

            IntPtr unmanagedAddr = Marshal.AllocHGlobal(Marshal.SizeOf(size));
            Marshal.StructureToPtr(size, unmanagedAddr, true);
            ret = Native.ioctl(master, Native.TIOCSWINSZ, unmanagedAddr);

            Marshal.PtrToStructure(unmanagedAddr, size);

            Marshal.FreeHGlobal(unmanagedAddr);
            unmanagedAddr = IntPtr.Zero;

            return ret;
        }

        /**
         * vte_pty_get_size:
         * @master: the file descriptor of the pty master
         * @columns: a place to store the number of columns
         * @rows: a place to store the number of rows
         *
         * Attempts to read the pseudo terminal's window size.
         *
         * Returns: 0 on success, -1 on failure.
         */
        static int _pty_sharp_get_size(IntPtr master, out int columns, out int rows)
        {
            columns = 0;
            rows = 0;
            Native.winsize size = new Native.winsize();
            int ret;

            IntPtr unmanagedAddr = Marshal.AllocHGlobal(Marshal.SizeOf(size));
            Marshal.StructureToPtr(size, unmanagedAddr, true);

            ret = Native.ioctl(master, Native.TIOCGWINSZ, unmanagedAddr);


            Marshal.PtrToStructure(unmanagedAddr, size);

            Marshal.FreeHGlobal(unmanagedAddr);
            unmanagedAddr = IntPtr.Zero;

            if (ret == 0)
            {
                columns = size.ws_col;
                rows = size.ws_row;
            }

            return ret;
        }

        public Process Process => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void SetSize(int columns, int rows)
        {
            throw new NotImplementedException();
        }

        public Task WriteAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
