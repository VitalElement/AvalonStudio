using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Projects.DUB
{
    class BlankDUBProjectTemplate : IProjectTemplate
    {
        public string Title { get { return "Empty DUB Project"; } }
        public string DefaultProjectName { get { return "EmptyProject"; } }
        public string Description { get { return "Creates an empty DUB project."; } }

        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
            
        }

        public Task<IProject> Generate(ISolution solution, string name)
        {
            throw new NotImplementedException();
        }
    }
}
