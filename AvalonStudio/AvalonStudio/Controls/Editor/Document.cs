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
    using TextEditor;
    using TextEditor.Document;
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
                    TextDocument = new TextDocument(fs.ReadToEnd());
                }
            }

            LanguageService = new CPlusPlusLanguageService(file.Project.Solution.NClangIndex, file);
        }

        public event EventHandler<EventArgs> TextChanged;

        public event EventHandler<EventArgs> CodeAnalysisDataChanged;

        private CodeAnalysisResults codeAnalysisResults;
        public CodeAnalysisResults CodeAnalysisResults
        {
            get { return codeAnalysisResults; }
            set
            {
                codeAnalysisResults = value;

                if (CodeAnalysisDataChanged != null)
                {
                    CodeAnalysisDataChanged(this, new EventArgs());
                }
            }
        }

        public ILanguageService LanguageService { get; set; }
        public TextDocument TextDocument { get; set; }
    }
}
