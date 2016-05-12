namespace AvalonStudio.Extensibility.MainMenu.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using Caliburn.Micro;
    using Gemini.Framework.Themes;
    using Gemini.Modules.Settings;

    [Export(typeof (ISettingsEditor))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MainMenuSettingsViewModel : PropertyChangedBase, ISettingsEditor
    {
        private readonly IThemeManager _themeManager;

        private ITheme _selectedTheme;
        private bool _autoHideMainMenu;

        [ImportingConstructor]
        public MainMenuSettingsViewModel(IThemeManager themeManager)
        {
            _themeManager = themeManager;
            SelectedTheme = themeManager.CurrentTheme;
            AutoHideMainMenu = Properties.Settings.Default.AutoHideMainMenu;
        }

        public IEnumerable<ITheme> Themes
        {
            get { return _themeManager.Themes; }
        }

        public ITheme SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (value.Equals(_selectedTheme)) return;
                _selectedTheme = value;
                NotifyOfPropertyChange(() => SelectedTheme);
            }
        }

        public bool AutoHideMainMenu
        {
            get { return _autoHideMainMenu; }
            set
            {
                if (value.Equals(_autoHideMainMenu)) return;
                _autoHideMainMenu = value;
                NotifyOfPropertyChange(() => AutoHideMainMenu);
            }
        }

        public string SettingsPageName
        {
            get { return Properties.Resources.SettingsPageGeneral; }
        }

        public string SettingsPagePath
        {
            get { return "Environment"; }
        }

        public void ApplyChanges()
        {
            Properties.Settings.Default.ThemeName = SelectedTheme.Name;
            Properties.Settings.Default.AutoHideMainMenu = AutoHideMainMenu;
            Properties.Settings.Default.Save();
        }
    }
}