using Avalonia.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Editor
{
    static class KeyExtensions
    {
        public static bool IsModifierKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.LeftShift:
                case Key.LeftAlt:
                case Key.LeftCtrl:
                case Key.RightAlt:
                case Key.RightCtrl:
                case Key.RightShift:
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsNavigationKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                case Key.Escape:
                    result = true;
                    break;

                default:
                    // Do nothing
                    break;
            }

            return result;
        }

        public static bool IsTriggerKey(this Key key)
        {
            bool result = false;

            if (!key.IsNavigationKey())
            {
                if (key >= Key.A && key <= Key.Z)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool IsLanguageSpecificTriggerKey(this Key key)
        {
            bool result = false;

            switch (key)
            {
                case Key.OemPeriod:
                    result = true;
                    break;
            }

            return result;
        }

        public static bool IsSearchKey(this Key key)
        {
            bool result = false;

            if (!key.IsNavigationKey() && !key.IsTriggerKey())
            {
                switch (key)
                {
                    case Key.OemPeriod:
                    case Key.Space:
                    case Key.OemOpenBrackets:
                    case Key.OemCloseBrackets:
                        result = true;
                        break;
                }
            }

            return result;
        }
    }
}
