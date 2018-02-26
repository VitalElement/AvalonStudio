using AvalonStudio.Platforms;
using System;
using System.Collections.Generic;
using System.IO;

namespace AvalonStudio.Extensibility
{
    public class ExtensionManager
    {
        public IEnumerable<IExtensionManifest> LoadExtensions()
        {
            foreach (var directory in Directory.GetDirectories(Platform.ExtensionsFolder))
            {
                var extensionManifest = Path.Combine(directory, "extension.json");

                if (File.Exists(extensionManifest))
                {
                    IExtensionManifest extension = null;

                    try
                    {
                        extension = ExtensionManifest.LoadFromManifest(extensionManifest);
                    }
                    catch (Exception e)
                    {
                        // todo: log exception
                    }

                    if (extension != null)
                    {
                        yield return extension;
                    }
                }
            }
        }
    }
}
