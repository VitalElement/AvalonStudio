using System.IO;
using System.Reflection;

namespace AvalonStudio.Controls.Editor.Highlighting.Resources
{
    internal static class Resources
    {
        private const string Prefix = "AvalonStudio.Controls.Editor.Highlighting.Resources.";

        public static Stream OpenStream(string name)
        {
            var s = typeof(Resources).GetTypeInfo().Assembly.GetManifestResourceStream(Prefix + name);
            if (s == null)
                throw new FileNotFoundException("The resource file '" + name + "' was not found.");
            return s;
        }

        internal static void RegisterBuiltInHighlightings(CustomHighlightingManager hlm)
        {
            hlm.RegisterHighlighting("XML-Mode.xshd");
        }
    }
}
