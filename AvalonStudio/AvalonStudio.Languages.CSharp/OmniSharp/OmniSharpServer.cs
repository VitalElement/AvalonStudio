using AvalonStudio.Platforms;
using RestSharp;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    class OmniSharpServer
    {
        private static readonly string BaseDir = Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp", "OmniSharp");
        private static readonly string Binary = Path.Combine(BaseDir, $"OmniSharp{Platform.ExecutableExtension}");
        private Process process;
        private RestClient client;

        public OmniSharpServer()
        {

        }

        public Task<T> SendRequest<T>(OmniSharpRequest<T> request) where T : new()
        {
            var restRequest = new RestRequest(request.EndPoint, Method.POST);
            restRequest.AddJsonBody(request);                              

            TaskCompletionSource<T> responseReceived = new TaskCompletionSource<T>();
            restRequest.OnBeforeDeserialization = (resp) =>
            {
                resp.ContentType = "application/json";
            };

            client.ExecuteAsync<T>(restRequest, (response) =>
            {
                responseReceived.SetResult(response.Data);                                                
            });

            return responseReceived.Task;
        }

        public Task<Process> StartAsync(string projectDir)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.FileName = Binary;
            startInfo.Arguments = $"-s {projectDir}";

            //// Hide console window
            //startInfo.UseShellExecute = false;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
           // startInfo.CreateNoWindow = true;

            TaskCompletionSource<Process> processStartedCompletionSource = new TaskCompletionSource<Process>();

            var processes = Process.GetProcessesByName("OmniSharp");

            foreach (var process in processes)
            {
                process.Kill();
            }

            Task.Factory.StartNew(() =>
            {
                process = Process.Start(startInfo);

                client = new RestClient("http://localhost:2000");

                processStartedCompletionSource.SetResult(process);

                process.WaitForExit();
            });

            return processStartedCompletionSource.Task;
        }
    }
}
