namespace AvalonStudio.Controls
{
    using Models.LanguageServices.CPlusPlus;
    using Models.LanguageServices;
    using Models.Solutions;
    using Perspex.Threading;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Document
    {
        private ProjectFile file;

        public Document (ProjectFile file)
        {
            this.file = file;

            if (File.Exists(file.Location))
            {
                using (var fs = File.OpenText(file.Location))
                {
                    Text = fs.ReadToEnd();
                }
            }

            LanguageService = new CPlusPlusLanguageService(file.Project.Solution.NClangIndex, file);
        }

        public ILanguageService LanguageService { get; set; }
        public string Text { get; set; }
    }
}
