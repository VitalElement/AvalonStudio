namespace AvalonStudio.TestFrameworks.Catch
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Projects;
    using System.Diagnostics;
    using Utils;
    using System.IO;
    using Platform;
    using System.Threading;
    using System.Xml.Linq;
    public class CatchTestFramework : ITestFramework
    {
        public async Task<IEnumerable<Test>> EnumerateTestsAsync(IConsole console, IProject project)
        {
            List<Test> result = new List<Test>();

            if(project.TestFramework != null && project.TestFramework is CatchTestFramework)
            {
                if (await project.ToolChain?.Build(console, project))
                {
                    var startInfo = new ProcessStartInfo();
                    startInfo.FileName = Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath();
                    startInfo.Arguments = "--list-test-names-only";
                    
                    // Hide console window
                    startInfo.UseShellExecute = false;
                    startInfo.RedirectStandardOutput = true;
                    startInfo.RedirectStandardError = true;
                    startInfo.CreateNoWindow = true;

                    console.Write(string.Format("Enumerating {0} for tests...", project.Executable));

                    using (var process = Process.Start(startInfo))
                    {
                        process.OutputDataReceived += (sender, e) =>
                        {
                            if (!string.IsNullOrEmpty(e.Data))
                            {
                                result.Add(new Test(project) { Name = e.Data });
                            }
                        };

                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data != null)
                            {
                                console.WriteLine();
                                console.WriteLine(e.Data);
                            }
                        };

                        process.BeginOutputReadLine();

                        process.BeginErrorReadLine();

                        process.WaitForExit();                        

                        console.WriteLine("Done");
                    }
                }
                else
                {
                    console.WriteLine("Unable to run tests, build failed.");
                }
            }

            return result;
        }

        public async Task RunTestAsync(Test test, IProject project)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath());

            startInfo.Arguments = "\"" + test.Name + "\"" + " --reporter xml";

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            test.Assertion = string.Empty;
            string lastMessage = string.Empty;

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
