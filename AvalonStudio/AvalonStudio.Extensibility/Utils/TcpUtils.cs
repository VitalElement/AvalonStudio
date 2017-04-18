namespace AvalonStudio.Extensibility.Utils
{
    using System.Net;
    using System.Net.Sockets;

    public class TcpUtils
    {
        public static int FreeTcpPort()
        {
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}