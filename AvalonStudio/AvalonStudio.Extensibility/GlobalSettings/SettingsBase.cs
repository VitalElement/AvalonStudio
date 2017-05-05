using System;

namespace AvalonStudio.GlobalSettings
{
    public class SettingsBase
    {
        public static T GetSettings<T>() where T : new()
        {
            try
            {
                return Settings.Instance.GetSettings<T>();
            }
            catch (Exception)
            {
                return ProvisionSettings<T>();
            }
        }

        public static void SetSettings<T> (T value) where T: new()
        {
            Settings.Instance.SetSettings<T>(value);
        }

        public static T ProvisionSettings<T>() where T : new()
        {
            return Settings.Instance.ProvisionSettings<T>();
        }

        public void Save()
        {
            Settings.Instance.Save();
        }
    }
}