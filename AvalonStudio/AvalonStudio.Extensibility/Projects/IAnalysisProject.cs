using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Projects
{
    public interface IAnalysisProject
    {
        void Analyze(IConsole console);
    }
}
