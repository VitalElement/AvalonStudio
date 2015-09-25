using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Models.Solutions;
using AvalonStudio.Models.Tools.Compiler;
//using AvalonStudio.Models.Tools.Debuggers;

namespace AvalonStudio.Models
{
    public class VEStudioSettings
    {
        public VEStudioSettings ()
        {
            InstalledToolChains = new List<ToolChain> ();

            ToolchainSettings = new List<ToolChainSettings> ();

           // InstalledDebugAdaptors = new List<GDBDebugAdaptor> ();
        }

        private static VEStudioSettings CreateNew ()
        {
            VEStudioSettings result = new VEStudioSettings ();

            result.ToolchainSettings.Add (new ToolChainSettings (typeof (Arm)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (ClangToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (GCCToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (CustomProcessToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (BitThunderToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (MinGWToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (PicXC32ToolChain)));

            return result;
        }

        public static VEStudioSettings LoadSettings ()
        {
            VEStudioSettings result = null;

            if (File.Exists (VEStudioService.SettingsFile))
            {
                try
                {
                    var serializer = new XmlSerializer (typeof (VEStudioSettings));

                    var reader = new StreamReader (VEStudioService.SettingsFile);

                    result = (VEStudioSettings)serializer.Deserialize (reader);

                    reader.Close ();
                }
                catch (Exception e)
                {
                    Console.WriteLine (e);
                    result = CreateNew ();
                }
            }
            else
            {
                result = CreateNew ();
            }

            return result;
        }

        public static VEStudioSettings This = LoadSettings ();

        public void Save ()
        {
            try
            {
                var serializer = new XmlSerializer (typeof (VEStudioSettings));

                var textWriter = new StreamWriter (VEStudioService.SettingsFile);

                serializer.Serialize (textWriter, this);

                textWriter.Close ();
            }
            catch (Exception e)
            {
                Console.WriteLine (e);
            }
        }

        public bool ShowAllWarnings { get; set; }
        public List<ToolChainSettings> ToolchainSettings { get; set; }
        public List<ToolChain> InstalledToolChains { get; set; }
        //public List<GDBDebugAdaptor> InstalledDebugAdaptors { get; set; }
    }
}
