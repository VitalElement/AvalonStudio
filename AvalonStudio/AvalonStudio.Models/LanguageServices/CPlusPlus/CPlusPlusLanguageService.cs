namespace AvalonStudio.Models.LanguageServices.CPlusPlus
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public class CPlusPlusLanguageService : ILanguageService
    {
        public List<CodeCompletionData> CodeCompleteAt(uint line, uint column)
        {
            throw new NotImplementedException();
        }

        public void RunCodeAnalysis(Func<bool> interruptRequested)
        {
            for(int i = 0; i < 10; i++)
            {
                Thread.Sleep(50);

                if (interruptRequested())
                {
                    return;
                }
            }

            Console.WriteLine("Code analysis complete.");
        }
    }
}
