using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.OmniSharp.MSBuild
{
    public static class Helpers
    {
        public static TcpClient WaitForOneTcpConnection(this TcpListener l, TimeSpan? timeout = null)
        {
            timeout = timeout ?? new TimeSpan(0, 0, 60);
            var timedOut = new CancellationTokenSource();
            var tcs = new TaskCompletionSource<TcpClient>();
            l.AcceptTcpClientAsync().ContinueWith(t =>
            {
                if (t.IsCompleted)
                {
                    lock (tcs)
                    {
                        if (timedOut.IsCancellationRequested)
                            t.Result.Close();
                        else
                            tcs.SetResult(t.Result);
                    }
                }
            });
            if (!tcs.Task.Wait(timeout.Value))
            {
                lock (tcs)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        timedOut.Cancel();
                        throw new TimeoutException();
                    }
                }
            }
            return tcs.Task.Result;
        }

        public static async Task<string> ReceiveStringMessage(this WebSocket ws) => Encoding.UTF8.GetString(await ReceiveMessage(ws));

        public static async Task<byte[]> ReceiveMessage(this WebSocket ws)
        {
            var ms = new MemoryStream();
            var buffer = new byte[1024];
            while (ws.State == WebSocketState.Open)
            {
                var res = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (ws.State != WebSocketState.Open)
                    break;
                ms.Write(buffer, 0, res.Count);
                if (res.EndOfMessage)
                    return ms.ToArray();
            }
            throw new EndOfStreamException();
        }

        public static Task SendJson(this WebSocket ws, object data)
        {
            var bdata = Encoding.UTF8.GetBytes(JObject.FromObject(data).ToString());
            return ws.SendAsync(new ArraySegment<byte>(bdata), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }

    public class OneShotTcpServer : IDisposable
    {
        private TcpListener _l;

        public OneShotTcpServer(int port = 0)
        {
            _l = new TcpListener(IPAddress.Loopback, port);
            _l.Start();
            Port = ((IPEndPoint)_l.LocalEndpoint).Port;
        }

        public int Port { get; }

        public void Dispose()
        {
            try
            {
                _l.Stop();
            }
            catch
            {
                //Ignore
            }
        }

        public TcpClient WaitForOneConnection(TimeSpan? timeout = null) => _l.WaitForOneTcpConnection(timeout);

    }

}
