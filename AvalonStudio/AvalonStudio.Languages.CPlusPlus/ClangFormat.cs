namespace AvalonStudio.Languages.CPlusPlus
{
    using Platforms;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Linq;

    public class ClangFormat
    {
        public string Text { get; private set; }
        public UInt32 Cursor { get; private set; }

        public static XDocument FormatXml(string text, UInt32 offset, UInt32 length, UInt32 cursor, ClangFormatSettings settings)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            string resultText = string.Empty;

            startInfo.FileName = Path.Combine(Platform.PluginsDirectory, "clang-format" + Platform.ExecutableExtension);
            startInfo.Arguments = string.Format("-offset={0} -length={1} -cursor={2} -style=\"{3}\" -output-replacements-xml", offset, length, cursor, settings.ToString());

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true; //we can get the erros text now.
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            using (var process = Process.Start(startInfo))
            {
                using (var streamWriter = process.StandardInput)
                {
                    streamWriter.Write(text);
                    streamWriter.Close();

                    using (var streamReader = process.StandardOutput)
                    {
                        resultText = streamReader.ReadToEnd();
                    }
                }
            }

            return XDocument.Parse(resultText);
        }
    }
}
