using AvalonStudio.GlobalSettings;
using AvalonStudio.Utils;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace AvalonStudio.Projects
{
    public static class IProjectExtensions
    {
        private static T ProvisionToolchainSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.ProvisionSettings<T>(() => project.ToolchainSettings, () => project.Save());
        }

        public static T GetToolchainSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.GetSettings<T>(() => project.ToolchainSettings, () => project.Save());
        }

        public static void SetToolchainSettings<T>(this IProject project, T value) where T : new()
        {
            SettingsSerializer.SetSettings<T>(() => project.ToolchainSettings, () => project.Save(), value);
        }

        private static T ProvisionDebuggerSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.ProvisionSettings<T>(() => project.DebugSettings, () => project.Save());
        }

        public static T GetDebuggerSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.GetSettings<T>(() => project.DebugSettings, () => project.Save());
        }

        public static void SetDebuggerSettings<T>(this IProject project, T value) where T : new()
        {
            SettingsSerializer.SetSettings<T>(() => project.DebugSettings, () => project.Save(), value);
        }

        private static T ProvisionGenericSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.ProvisionSettings<T>(() => project.Settings, () => project.Save());
        }

        public static T GetGenericSettings<T>(this IProject project) where T : new()
        {
            return SettingsSerializer.GetSettings<T>(() => project.Settings, () => project.Save());
        }

        public static void SetGenericSettings<T>(this IProject project, T value) where T : new()
        {
            SettingsSerializer.SetSettings<T>(() => project.Settings, () => project.Save(), value);
        }
    }
}