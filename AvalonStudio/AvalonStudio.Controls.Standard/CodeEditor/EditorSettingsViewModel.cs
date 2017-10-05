using AvalonStudio.Controls.Standard.SettingsDialog;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.GlobalSettings;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls.Standard.CodeEditor
{
    public class EditorSettingsViewModel : SettingsViewModel, IExtension
    {
        private bool _removeTrailingWhiteSpaceOnSave;
        private bool _autoFormat;

        public EditorSettingsViewModel() : base("General")
        {
        }

        public override void OnDialogLoaded()
        {
            base.OnDialogLoaded();

            var settings = Settings.GetSettings<EditorSettings>();

            RemoveTrailingWhiteSpaceOnSave = settings.RemoveTrailingWhitespaceOnSave;
            AutoFormat = settings.AutoFormat;
        }

        private void Save()
        {
            var settings = Settings.GetSettings<EditorSettings>();

            settings.RemoveTrailingWhitespaceOnSave = RemoveTrailingWhiteSpaceOnSave;
            settings.AutoFormat = AutoFormat;

            Settings.SetSettings(settings);
        }

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

        public void Activation()
        {
            IoC.Get<ISettingsManager>().RegisterSettingsDialog("Editor", this);
        }

        public void BeforeActivation()
        {

        }
    }
}
