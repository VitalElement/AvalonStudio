using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Models.Solutions;
//using AvalonStudio.Models.Tools.Debuggers;

namespace AvalonStudio.Models.Tools.Compiler
{
    [XmlInclude (typeof (ClangToolChain))]
    [XmlInclude (typeof (GCCToolChain))]
    [XmlInclude (typeof (Arm))]
    [XmlInclude (typeof (CustomProcessToolChain))]
    [XmlInclude (typeof (BitThunderToolChain))]
    [XmlInclude (typeof (MinGWToolChain))]
    [XmlInclude (typeof (PicXC32ToolChain))]
    public abstract class ToolChain
    {
        public static string AppendPath (string path, string addition)
        {
            if (path [path.Length - 1] != ';')
            {
                path += ";";
            }

            return path + addition;
        }

        [XmlIgnore]
        public abstract string GDBExecutable { get; }

        public abstract Task<bool> Build (IConsole console, Project project, CancellationTokenSource cancellationSource);

        public abstract Task Clean (IConsole console, Project project, CancellationTokenSource cancellationSource);

        public string Name
        {
            get
            {
                return this.GetType ().Name;
            }
        }

        public ToolChainSettings Settings
        {
            get
            {
                ToolChainSettings result = VEStudioSettings.This.ToolchainSettings.FirstOrDefault ((tcs) => tcs.ToolChainRealType == this.GetType ());

                return result;
            }
        }
    }
   
    public class ToolChainSettings
    {
        public ToolChainSettings (Type toolChainType)
        {
            this.ToolChainType = toolChainType.Name;
            this.ToolChainLocation = string.Empty;
            
            this.IncludePaths = new List<string>();            
        }

        private ToolChainSettings ()
        {

        }

        [XmlIgnore]
        public Type ToolChainRealType
        {
            get
            {
                switch (ToolChainType)
                {
                    case "Arm":
                        return typeof (Arm);

                    case "ClangToolChain":
                        return typeof(ClangToolChain);

                    case "GCCToolChain":
                        return typeof(GCCToolChain);

                    case "CustomProcessToolChain":
                        return typeof(CustomProcessToolChain);

                    case "BitThunderToolChain":
                        return typeof(BitThunderToolChain);

                    case "MinGWToolChain":
                        return typeof(MinGWToolChain);

                    case "PicXC32ToolChain":
                        return typeof(PicXC32ToolChain);

                    default:
                        throw new Exception ("Invalid Tool chain");
                }
            }
        }


        public string ToolChainType { get; set; }
        public string ToolChainLocation { get; set; }     

        public List<string> IncludePaths { get; set; }      
    }

    public class ProcessResult
    {
        public int ExitCode { get; set; }
    }

    public class CompileResult : ProcessResult
    {
        public CompileResult ()
        {            
            ObjectLocations = new List<string> ();
            LibraryLocations = new List<string> ();
            ExecutableLocations = new List<string> ();
        }        

        public List<string> ObjectLocations { get; set; }
        public List<string> LibraryLocations { get; set; }
        public List<string> ExecutableLocations { get; set; }
        public int NumberOfObjectsCompiled { get; set; }

		public int Count
		{
			get
			{
				return ObjectLocations.Count + LibraryLocations.Count + ExecutableLocations.Count;
			}
		}
    }

    public class LinkResult : ProcessResult
    {
        public string Executable { get; set; }
    }
}
