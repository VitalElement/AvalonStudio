using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AvalonStudio.TestFrameworks.Catch
{
    [ExportTestFramework]
    public class CatchTestFramework : ITestFramework
    {
        public async Task<IEnumerable<Test>> EnumerateTestsAsync(IProject project)
        {
            var result = new List<Test>();

            if (project.TestFramework != null && project.TestFramework is CatchTestFramework)
            {
                var startInfo = new ProcessStartInfo();
                startInfo.FileName = Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath();
                startInfo.Arguments = "--list-test-names-only";

                // Hide console window
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.CreateNoWindow = true;

                await Task.Factory.StartNew(() =>
                {
                    using (var process = Process.Start(startInfo))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                result.Add(new Test(project) { Name = e.Data });
                            }
                        };

                        process.BeginOutputReadLine();

                        process.WaitForExit();
                    }
                });
            }

            return result;
        }

        public async Task RunTestAsync(Test test, IProject project)
        {
            var startInfo = new ProcessStartInfo(Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath());

            startInfo.Arguments = "\"" + test.Name + "\"" + " --reporter xml";

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            test.Assertion = string.Empty;
            var lastMessage = string.Empty;

            await Task.Factory.StartNew(() =>
            {
                using (var process = Process.Start(startInfo))
                {
                    var output = process.StandardOutput.ReadToEnd();

                    if (output != string.Empty)
                    {
                        var document = XDocument.Parse(output);

                        var elements = document.Elements().First();

                        var testCase = elements.Element(XName.Get("Group")).Element(XName.Get("TestCase"));

                        if (testCase != null)
                        {
                            var expression = testCase.Element(XName.Get("Expression"));
                            var result = testCase.Element(XName.Get("OverallResult"));

                            if (expression != null)
                            {
                                test.File = expression.Attribute(XName.Get("filename")).Value;
                                test.Line = int.Parse(expression.Attribute(XName.Get("line")).Value);
                                test.Assertion = expression.Element(XName.Get("Original")).Value.Replace("\n", string.Empty).Trim();
                            }

                            if (result != null)
                            {
                                test.Pass = bool.Parse(result.Attribute(XName.Get("success")).Value);
                            }
                        }
                    }

                    process.WaitForExit();
                }
            });
        }
    }
}