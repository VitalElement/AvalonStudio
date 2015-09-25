namespace AvalonStudio.Models.Solutions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    //using AvalonStudio.Models.Testing;
    //using AvalonStudio.Models.Tools.Debuggers;

    public class CatchTestProject : Project
    {
        private CatchTestProject () : base ()
        {
            userData = new ProjectItemUserData(Guid.NewGuid());
            userData.RunImmediately = true;
            userData.BreakOnMain = false;            
        }

        public CatchTestProject(Solution solution, Item container)
            : base(solution, container)
        {
            userData = new ProjectItemUserData(Guid.NewGuid());
            userData.RunImmediately = true;
            userData.BreakOnMain = false;
        }

        //public override GDBDebugAdaptor SelectedDebugAdaptor
        //{
        //    get
        //    {
        //        return new LocalDebugAdaptor();
        //    }

        //    set
        //    {
        //        base.SelectedDebugAdaptor = value;
        //    }
        //}

        private ProjectItemUserData userData;
        public override ProjectItemUserData UserData
        {
            get
            {
                return userData;
            }

            set
            {
                base.UserData = value;
            }
        }

        //public void RunTest (Test test)
        //{            
        //    if (File.Exists(Executable))
        //    {
        //        ProcessStartInfo startInfo = new ProcessStartInfo(Executable);

        //        startInfo.Arguments = "\"" + test.Name + "\"" + " --reporter xml";

        //        startInfo.UseShellExecute = false;
        //        startInfo.RedirectStandardOutput = true;
        //        startInfo.CreateNoWindow = true;

        //        test.Assertion = string.Empty;
        //        string lastMessage = string.Empty;

        //        using (var process = Process.Start(startInfo))
        //        {
        //            var output = process.StandardOutput.ReadToEnd();

        //            if (output != string.Empty)
        //            {
        //                var document = XDocument.Parse(output);

        //                var elements = document.Elements().First();

        //                var testCase = elements.Element(XName.Get("Group")).Element(XName.Get("TestCase"));

        //                if (testCase != null)
        //                {
        //                    var expression = testCase.Element(XName.Get("Expression"));
        //                    var result = testCase.Element(XName.Get("OverallResult"));

        //                    if (expression != null)
        //                    {
        //                        test.File = expression.Attribute(XName.Get("filename")).Value;
        //                        test.Line = int.Parse(expression.Attribute(XName.Get("line")).Value);
        //                        test.Assertion = expression.Element(XName.Get("Original")).Value.Replace("\n", string.Empty).Trim();
        //                    }

        //                    if (result != null)
        //                    {
        //                        test.Pass = bool.Parse(result.Attribute(XName.Get("success")).Value);
        //                    }
        //                }
        //            }

        //            process.WaitForExit();                   
        //        }
        //    }
        //}

        //public List<Test> EnumerateTests (IConsole console)
        //{
        //    var result = new List<Test> ();

        //    if (File.Exists(Executable))
        //    {
        //        ProcessStartInfo startInfo = new ProcessStartInfo(Executable);

        //        startInfo.Arguments = "--list-test-names-only";

        //        startInfo.UseShellExecute = false;
        //        startInfo.RedirectStandardOutput = true;
        //        startInfo.RedirectStandardError = true;
        //        startInfo.CreateNoWindow = true;
                

        //        using (var process = Process.Start(startInfo))
        //        {
        //            process.OutputDataReceived += (sender, e) =>
        //            {
        //                if (!string.IsNullOrEmpty(e.Data))
        //                {
        //                    var newTest = new Test(this);
        //                    newTest.Name = e.Data;
        //                    result.Add(newTest);
        //                }
        //            };

        //            process.ErrorDataReceived += (sender, e) =>
        //            {
        //                console.WriteLine(e.Data);
        //            };
                    
        //            process.BeginOutputReadLine();
        //            process.BeginErrorReadLine();              

        //            process.WaitForExit();
        //        }
        //    }

        //    return result;
        //}

        new public static CatchTestProject Create(Solution solution, SolutionFolder container, string name)
        {
            var result = new CatchTestProject(solution, container);

            string newFolder = Path.Combine(solution.CurrentDirectory, name);

            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            result.LocationRelativeToParent = Path.Combine(name, name + VEStudioService.ProjectExtension);

            result.Configurations.Add(new ProjectConfiguration() { Name = "Default" });

            result.SerializeToXml();

            container.AddProject(result, result.GetUnloadedProject());

            return result;
        }
    }
}
