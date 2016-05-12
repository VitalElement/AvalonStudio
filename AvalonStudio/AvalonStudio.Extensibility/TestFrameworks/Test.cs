namespace AvalonStudio.TestFrameworks
{
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Test
    {
        public Test (IProject project)
        {
            this.project = project;
        }
                    
        private IProject project;

        public string Name { get; set; }
        public string File { get; set; }
        public int Line { get; set; }
        public string Assertion { get; set; }
        public bool Pass { get; set; }

        public void Run ()
        {
            this.project.TestFramework.RunTestAsync(this, project).Wait();
        }
    }
}
