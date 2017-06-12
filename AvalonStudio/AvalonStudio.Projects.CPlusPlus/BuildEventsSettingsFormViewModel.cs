using AvalonStudio.Debugging;
using AvalonStudio.Extensibility;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class BuildEventsSettingsFormViewModel : HeaderedViewModel<CPlusPlusProject>
    {
        BuildEventsSettings _settings;
        CPlusPlusProject _project;

        public BuildEventsSettingsFormViewModel(CPlusPlusProject project) : base("Build Events", project)
        {
            _project = project;
            _settings = project.GetGenericSettings<BuildEventsSettings>();

            _preBuildCommands = _settings.PreBuildCommands;
            _postBuildCommands = _settings.PostBuildCommands;
        }

        private string _preBuildCommands;

        public string PreBuildCommands
        {
            get
            {
                return _preBuildCommands;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _preBuildCommands, value);
                Save();
            }
        }

        private string _postBuildCommands;

        public string PostBuildCommands
        {
            get
            {
                return _postBuildCommands;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _postBuildCommands, value);
                Save();
            }
        }

        public void Save()
        {
            _settings.PreBuildCommands = PreBuildCommands;
            _settings.PostBuildCommands = PostBuildCommands;

            _project.SetGenericSettings(_settings);
        }
    }
}