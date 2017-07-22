using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Shell;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls.Standard.ErrorList
{
    public class FixIt : Diagnostic
    {
        private bool initialized = false;

        public FixIt()
        {
            Replacements = new List<Replacement>();
        }

        public void AddReplacement(Replacement replacement)
        {
            Replacements.Add(replacement);

            if (!initialized)
            {
                StartOffset = replacement.StartOffset;
            }
        }

        public List<Replacement> Replacements { get; set; }

        public async Task ApplyReplacementsAsync()
        {
            var shell = IoC.Get<IShell>();
            var errorList = IoC.Get<IErrorList>();

            errorList.RemoveFixIt(this);

            foreach (var replacement in Replacements)
            {
                var editor = await shell.OpenDocument(replacement.File, 1);
                editor.GotoOffset(replacement.StartOffset);

                editor.GetDocument().Replace(replacement.StartOffset, replacement.Length, replacement.ReplacementText);
            }
        }
    }
}
