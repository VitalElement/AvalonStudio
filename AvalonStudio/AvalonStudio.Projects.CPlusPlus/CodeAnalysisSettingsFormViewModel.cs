using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class CodeAnalysisSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        CodeAnalysisSettings _settings;
        CPlusPlusProject _project;

        public CodeAnalysisSettingsFormViewModel(CPlusPlusProject project) : base("Code Analysis", project)
        {
            _project = project;
            _settings = project.GetGenericSettings<CodeAnalysisSettings>();

            _enabled = _settings.Enabled;
        }

        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                this.RaiseAndSetIfChanged(ref _enabled, value);
                Save();
            }
        }


        public void Save()
        {
            _settings.Enabled = _enabled;

            _project.SetGenericSettings(_settings);
        }
    }
}