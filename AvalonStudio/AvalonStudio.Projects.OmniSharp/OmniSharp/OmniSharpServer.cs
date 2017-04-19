namespace AvalonStudio.Languages.CSharp.OmniSharp
{
    using AvalonStudio.Platforms;
    using Extensibility.Languages.CompletionAssistance;
    using RestSharp;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    public class OmniSharpServer
    {
        private static readonly string BaseDir = Path.Combine(Platform.ReposDirectory, "AvalonStudio.Languages.CSharp", "OmniSharp");
        private static readonly string Binary = Path.Combine(BaseDir, $"OmniSharp{Platform.ExecutableExtension}");
        private Process process;
        private RestClient client;
        private int port;

        public OmniSharpServer(int port = 2000)
        {
            this.port = port;
        }

        public Task<OmniSharpHighlightData> Highlight(string file, string buffer)
        {
            return SendRequest(new HighlightOmniSharpRequest() { FileName = file, Buffer = buffer });
        }

        public Task<SignatureHelp> SignatureHelp(string file, string buffer, int line, int column)
        {
            return SendRequest(new SignatureHelpOmniSharpRequest() { FileName = file, Buffer = buffer, Line = line, Column = column });
        }

        public Task<List<CompletionData>> AutoComplete(string file, string buffer, int line, int column, bool wantDocumentationForEveryCompletionResult = true, bool wantImportableTypes = true, bool wantKind = true, bool wantMethodHeader = true, bool wantReturnType = true, bool wantSnippet = true)
        {
            return SendRequest(new AutoCompleteOmniSharpRequest() { FileName = file, Buffer = buffer, Line = line, Column = column, WantDocumentationForEveryCompletionResult = wantDocumentationForEveryCompletionResult, WantImportableTypes = wantImportableTypes, WantKind = wantKind, WantMethodHeader = wantMethodHeader, WantReturnType = wantReturnType, WantSnippet = wantSnippet });
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
            startInfo.Arguments = $"-p {port} -s {projectDir}";

            //// Hide console window
            //startInfo.UseShellExecute = false;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.RedirectStandardError = true;
            //startInfo.RedirectStandardInput = true;
            //startInfo.CreateNoWindow = true;

            TaskCompletionSource<Process> processStartedCompletionSource = new TaskCompletionSource<Process>();

            Task.Factory.StartNew(async () =>
            {
                process = Process.Start(startInfo);

                client = new RestClient($"http://localhost:{port}");

                while (true)
                {
                    if (await SendRequest(new CheckReadyStatusRequest()))
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                processStartedCompletionSource.SetResult(process);

                process.WaitForExit();
            });

            return processStartedCompletionSource.Task;
        }
    }
}