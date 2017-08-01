using AsyncRpc;
using AsyncRpc.Transport.Tcp;
using AvalonStudio.CommandLineTools;
using AvalonStudio.Extensibility;
using AvalonStudio.MSBuildHost;
using AvalonStudio.Utils;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace AvalonStudio.Projects.OmniSharp.MSBuild
{
    public class MSBuildHost
    {
        private IMsBuildHostService msBuildHostService;
        private Process hostProcess;

        public async Task Connect()
        {
            hostProcess = PlatformSupport.LaunchShellCommand("dotnet", "\"C:\\Program Files\\dotnet\\sdk\\2.0.0-preview2-006497\\MSBuild.dll\" avalonstudio-intercept.csproj",
            (sender, e) =>
            {
                IoC.Get<IConsole>().WriteLine(e.Data);
            },
            (sender, e) =>
            {
                IoC.Get<IConsole>().WriteLine(e.Data);
            }, false, AvalonStudio.Platforms.Platform.ExecutionPath, false);

            msBuildHostService = new Engine().CreateProxy<IMsBuildHostService>(new TcpClientTransport(IPAddress.Loopback, 9000));

            var res = await msBuildHostService.GetVersion();
        }

        public async Task<(ProjectInfo info, List<string> projectReferences)> LoadProject(string solutionDirectory, string projectFile)
        {
            var loadData = await msBuildHostService.LoadProject(solutionDirectory, projectFile);

            var projectInfo = ProjectInfo.Create(
                ProjectId.CreateNewId(),
                VersionStamp.Create(),
                Path.GetFileNameWithoutExtension(projectFile), "",
                LanguageNames.CSharp,
                projectFile,
                metadataReferences: loadData.metaDataReferences.Select(ar => MetadataReference.CreateFromFile(ar.Assembly)));

            return (projectInfo, loadData.projectReferences);
        }
    }
}
