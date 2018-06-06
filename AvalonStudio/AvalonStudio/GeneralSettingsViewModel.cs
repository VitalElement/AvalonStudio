using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Settings;
using AvalonStudio.Extensibility.Theme;
using AvalonStudio.GlobalSettings;
using ReactiveUI;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace AvalonStudio
{
    [Export, Shared]
    class GeneralSettingsViewModel : SettingsViewModel
    {
        private int _selectedThemeIndex;

        [ImportingConstructor]
        public GeneralSettingsViewModel(ISettingsManager manager) : base("General")
        {
            manager.RegisterSettingsDialog("Environment", this);
            _selectedThemeIndex = -1;
        }

        public override void OnDialogLoaded()
        {
            base.OnDialogLoaded();

            var settings = Settings.GetSettings<GeneralSettings>();

            SelectedThemeIndex = Themes.IndexOf(settings.Theme);
        }

        private void Save()
        {
            var settings = Settings.GetSettings<GeneralSettings>();

            settings.Theme = Themes[SelectedThemeIndex];

            Settings.SetSettings(settings);
        }

        public List<string> Themes => ColorTheme.Themes.Select(t => t.Name).ToList();

        public int SelectedThemeIndex
        {
            get { return _selectedThemeIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedThemeIndex, value);

                if (_selectedThemeIndex >= 0 && Themes.Count > _selectedThemeIndex)
                {
                    var loadedTheme = ColorTheme.LoadTheme(Themes[_selectedThemeIndex]);

                    if (loadedTheme.Name != Themes[_selectedThemeIndex])
                    {
                        _selectedThemeIndex = Themes.IndexOf(loadedTheme.Name);
                    }

                    Save();
                }
            }
        }

    }
}
