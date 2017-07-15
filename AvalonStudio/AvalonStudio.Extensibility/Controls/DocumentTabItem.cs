using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace AvalonStudio.Controls
{
    public class DocumentTabItem : ContentControl
    {
        static DocumentTabItem()
        {
            PseudoClass(IsFocusedProperty, o => o, ":focused");
            PseudoClass(DockPanel.DockProperty, o => o == Dock.Right, ":dockright");
            PseudoClass(DockPanel.DockProperty, o => o == Dock.Left, ":dockleft");
            PseudoClass(DockPanel.DockProperty, o => o == Dock.Top, ":docktop");
            PseudoClass(DockPanel.DockProperty, o => o == Dock.Bottom, ":dockbottom");
        }

        public static readonly AvaloniaProperty<string> TitleProprty =
            AvaloniaProperty.Register<ToolControl, string>(nameof(Title));

        public string Title
        {
            get { return GetValue(TitleProprty); }
            set { SetValue(TitleProprty, value); }
        }

        public static readonly AvaloniaProperty<IBrush> HeaderBackgroundProperty =
            AvaloniaProperty.Register<ToolControl, IBrush>(nameof(HeaderBackground), defaultValue: Brushes.WhiteSmoke);

        public IBrush HeaderBackground
        {
            get { return GetValue(HeaderBackgroundProperty); }
            set { SetValue(HeaderBackgroundProperty, value); }
        }
    }
}