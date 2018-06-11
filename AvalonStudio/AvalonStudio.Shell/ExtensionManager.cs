using AvalonStudio.Extensibility;
using AvalonStudio.Shell.Extensibility.Platforms;
using System;
using System.Collections.Generic;
using System.Composition;
using System.IO;

namespace AvalonStudio
{
    [Export(typeof(IExtensionManager))]
    [Shared]
    public class ExtensionManager : IExtensionManager
    {
        private const string ExtensionManifestFilename = "extension.json";

        private static Lazy<IEnumerable<IExtensionManifest>> _installedExtensions = new Lazy<IEnumerable<IExtensionManifest>>(LoadExtensions);

        public IEnumerable<IExtensionManifest> GetInstalledExtensions() => _installedExtensions.Value;

        private static List<IExtensionManifest> LoadExtensions()
        {
            var extensions = new List<IExtensionManifest>();

            foreach (var directory in Directory.GetDirectories(Platform.ExtensionsFolder))
            {
                var extensionManifest = Path.Combine(directory, ExtensionManifestFilename);

                if (File.Exists(extensionManifest))
                {
                    try
                    {
                        var extension = new ExtensionManifest(extensionManifest);
                        extensions.Add(extension);
                    }
                    catch (Exception e)
                    {
                        // todo: log exception
                    }
                }
            }

            return extensions;
        }
    }
}
