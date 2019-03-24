using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Editor;
using AvalonStudio.Extensibility.Settings;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.GlobalSettings;
using AvalonStudio.Shell;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.Controls.Editor
{
    public class EditorSettingsViewModel : SettingsViewModel, IActivatableExtension
    {
        private bool _removeTrailingWhiteSpaceOnSave;
        private bool _autoFormat;
        private int _selectedColorSchemeIndex;
        private bool _loading;

        public EditorSettingsViewModel() : base("General")
        {
        }

        public override void OnDialogLoaded()
        {
            _loading = true;

            base.OnDialogLoaded();

            var settings = Settings.GetSettings<EditorSettings>();

            SelectedColorSchemeIndex = ColorSchemes.IndexOf(settings.ColorScheme);

            RemoveTrailingWhiteSpaceOnSave = settings.RemoveTrailingWhitespaceOnSave;
            AutoFormat = settings.AutoFormat;

            _loading = false;
        }

        private void Save()
        {
            if (!_loading)
            {
                var settings = Settings.GetSettings<EditorSettings>();

                settings.RemoveTrailingWhitespaceOnSave = RemoveTrailingWhiteSpaceOnSave;
                settings.AutoFormat = AutoFormat;
                settings.ColorScheme = ColorSchemes[SelectedColorSchemeIndex];

                Settings.SetSettings(settings);
            }
        }

        public List<string> ColorSchemes => ColorScheme.ColorSchemes.Select(t => t.Name).ToList();

        public bool AutoFormat
        {
            get { return _autoFormat; }
            set
            {
                this.RaiseAndSetIfChanged(ref _autoFormat, value);

                Save();
            }
        }

        public bool RemoveTrailingWhiteSpaceOnSave
        {
            get { return _removeTrailingWhiteSpaceOnSave; }
            set
            {
                this.RaiseAndSetIfChanged(ref _removeTrailingWhiteSpaceOnSave, value);

                Save();
            }
        }

        public int SelectedColorSchemeIndex
        {
            get { return _selectedColorSchemeIndex; }
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedColorSchemeIndex, value);

                if (_selectedColorSchemeIndex >= 0 && ColorSchemes.Count > _selectedColorSchemeIndex)
                {
                    var loadedScheme = ColorScheme.LoadColorScheme(ColorSchemes[_selectedColorSchemeIndex]);

                    if (loadedScheme.Name != ColorSchemes[_selectedColorSchemeIndex])
                    {
                        _selectedColorSchemeIndex = ColorSchemes.IndexOf(loadedScheme.Name);
                    }

                    Save();

                    IoC.Get<IStudio>().CurrentColorScheme = loadedScheme;
                }
            }
        }

        public void Activation()
        {
            IoC.Get<ISettingsManager>()?.RegisterSettingsDialog("Editor", this);
        }

        public void BeforeActivation()
        {

        }
    }
}
