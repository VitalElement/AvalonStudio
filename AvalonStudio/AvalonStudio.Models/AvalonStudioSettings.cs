namespace AvalonStudio.Models
{
    using Tools.Compiler;
    using Tools.Debuggers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    public class AvalonStudioSettings
    {
        public AvalonStudioSettings ()
        {
            InstalledToolChains = new List<ToolChain> ();

            ToolchainSettings = new List<ToolChainSettings> ();

            InstalledDebugAdaptors = new List<GDBDebugAdaptor> ();
        }


		public void TestInterface (IConsole console)
		{
			NClang.ClangIndex index = NClang.ClangService.CreateIndex();

			var tu = index.ParseTranslationUnit("CardLaminator.cpp", new string[] { }, new NClang.ClangUnsavedFile[] { }, NClang.TranslationUnitFlags.None);

			var results = tu.CodeCompleteAt("CardLaminator.cpp", 49, 16, new NClang.ClangUnsavedFile[] { }, NClang.CodeCompleteFlags.None);

			console.WriteLine(results.ResultCount.ToString());

			foreach (var result in results.Results)
			{
				foreach (var chunk in result.CompletionString.Chunks)
				{
					console.Write(chunk.Text);
				}

				console.WriteLine();
			}
		}

        private static AvalonStudioSettings CreateNew ()
        {
            AvalonStudioSettings result = new AvalonStudioSettings ();

            result.ToolchainSettings.Add (new ToolChainSettings (typeof (Arm)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (ClangToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (GCCToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (CustomProcessToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (BitThunderToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (MinGWToolChain)));
            result.ToolchainSettings.Add (new ToolChainSettings (typeof (PicXC32ToolChain)));

            return result;
        }

        public static AvalonStudioSettings LoadSettings ()
        {
            AvalonStudioSettings result = null;

            if (File.Exists (AvalonStudioService.SettingsFile))
            {
                try
                {
                    var serializer = new XmlSerializer (typeof (AvalonStudioSettings));

                    var reader = new StreamReader (AvalonStudioService.SettingsFile);

                    result = (AvalonStudioSettings)serializer.Deserialize (reader);

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

        public static AvalonStudioSettings This = LoadSettings ();

        public void Save ()
        {
            try
            {
                var serializer = new XmlSerializer (typeof (AvalonStudioSettings));

                var textWriter = new StreamWriter (AvalonStudioService.SettingsFile);

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
        public List<GDBDebugAdaptor> InstalledDebugAdaptors { get; set; }
    }
}
