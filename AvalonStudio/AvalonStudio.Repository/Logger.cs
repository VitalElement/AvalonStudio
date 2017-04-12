using System;
using System.Collections.Generic;
using System.Text;
using NuGet.Common;
using NuGet.Configuration;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Protocol.Core.v2;

namespace AvalonStudio.Repositories
{
    public static class Extensions
    {
        public static void Dump(this string s) => Console.WriteLine(s);
    }

    public class ConsoleNuGetLogger : ILogger
    {
        public void LogDebug(string data) => $"DEBUG: {data}".Dump();
        public void LogVerbose(string data) => $"VERBOSE: {data}".Dump();
        public void LogInformation(string data) => $"INFORMATION: {data}".Dump();
        public void LogMinimal(string data) => $"MINIMAL: {data}".Dump();
        public void LogWarning(string data) => $"WARNING: {data}".Dump();
        public void LogError(string data) => $"ERROR: {data}".Dump();
        public void LogSummary(string data) => $"SUMMARY: {data}".Dump();

        public void LogInformationSummary(string data) => $"Infosummary: {data}".Dump();

        public void LogErrorSummary(string data) => $"LogErrorSummary: {data }".Dump();
    }
}
