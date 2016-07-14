using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ClassTemplateSettingsViewModel : ViewModel
    {
        public ClassTemplateSettingsViewModel()
        {
            GenerateClass = true;
            GenerateHeader = true;
        }

        private bool _generateHeader;

        public bool GenerateHeader
        {
            get { return _generateHeader; }
            set { this.RaiseAndSetIfChanged(ref _generateHeader, value); }
        }

        private bool _generateClass;

        public bool GenerateClass
        {
            get { return _generateClass; }
            set { this.RaiseAndSetIfChanged(ref _generateClass, value); }
        }

        private string _className;

        public string ClassName
        {
            get { return _className; }
            set { this.RaiseAndSetIfChanged(ref _className, value); }
        }
    }
}