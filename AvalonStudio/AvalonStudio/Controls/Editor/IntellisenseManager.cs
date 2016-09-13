using Avalonia.Input;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Controls
{
    class IntellisenseManager
    {
        private bool isProcessingKey;
        private ILanguageService languageService;
        private ISourceFile file;
        private IIntellisenseControl intellisenseControl;

        public IntellisenseManager(IIntellisenseControl intellisenseControl, ILanguageService languageService, ISourceFile file)
        {
            this.intellisenseControl = intellisenseControl;
            this.languageService = languageService;
            this.file = file;
        }

        public async void SetCursor(int index, int line, int column, List<UnsavedFile> unsavedFiles)
        {
            if (!isProcessingKey)
            {
                IoC.Get<IConsole>().WriteLine("SetCursor");
                await languageService.CodeCompleteAtAsync(file, line, column, unsavedFiles);
                IoC.Get<IConsole>().WriteLine("SetCursorCompleted");
            }
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            isProcessingKey = true;
            IoC.Get<IConsole>().WriteLine("OnKeyDown");
        }

        public void OnKeyUp(KeyEventArgs e)
        {
            isProcessingKey = false;
            IoC.Get<IConsole>().WriteLine("OnKeyUp");
        }
    }
}
