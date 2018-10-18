using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.IO;
using Newtonsoft.Json;
using Avalonia.Media;
using System.Collections.Generic;
using Avalonia;
using System;

namespace AvalonStudio.Extensibility.Editor
{
    public class DefaultColorSchemes : IActivatableExtension
    {
        public void Activation()
        {
            ColorScheme.Register(ColorScheme.Default);
            ColorScheme.Register(ColorScheme.Light);
            ColorScheme.Register(ColorScheme.MonoDevelopLight);
            ColorScheme.Register(ColorScheme.SolarizedDark);
            ColorScheme.Register(ColorScheme.SolarizedLight);
        }

        public void BeforeActivation()
        {
        }
    }

    public class ColorScheme
    {
        private static List<ColorScheme> s_colorSchemes = new List<ColorScheme>();
        private static Dictionary<string, ColorScheme> s_colorSchemeIDs = new Dictionary<string, ColorScheme>();
        private static readonly ColorScheme DefaultColorScheme = ColorScheme.SolarizedLight;
        private static readonly Dictionary<string, Func<IBrush>> s_colorAccessors = new Dictionary<string, Func<IBrush>>();
        public static IEnumerable<ColorScheme> ColorSchemes => s_colorSchemes;

        static ColorScheme()
        {
            s_colorAccessors["background"] = () => CurrentColorScheme.Background;
            s_colorAccessors["background.accented"] = () => CurrentColorScheme.BackgroundAccent;
            s_colorAccessors["text"] = () => CurrentColorScheme.Text;
            s_colorAccessors["comment"] = () => CurrentColorScheme.Comment;
            s_colorAccessors["delegate.name"] = () => CurrentColorScheme.DelegateName;
            s_colorAccessors["keyword"] = () => CurrentColorScheme.Keyword;
            s_colorAccessors["literal"] = () => CurrentColorScheme.Literal;
            s_colorAccessors["identifier"] = () => CurrentColorScheme.Identifier;
            s_colorAccessors["callexpression"] = () => CurrentColorScheme.CallExpression;
            s_colorAccessors["numericliteral"] = () => CurrentColorScheme.NumericLiteral;
            s_colorAccessors["enumconst"] = () => CurrentColorScheme.EnumConstant;
            s_colorAccessors["enum"] = () => CurrentColorScheme.EnumType;
            s_colorAccessors["operator"] = () => CurrentColorScheme.Operator;
            s_colorAccessors["struct.name"] = () => CurrentColorScheme.StructName;
            s_colorAccessors["interface"] = () => CurrentColorScheme.InterfaceType;
            s_colorAccessors["punctuation"] = () => CurrentColorScheme.Punctuation;
            s_colorAccessors["type"] = () => CurrentColorScheme.Type;
            s_colorAccessors["xml.tag"] = () => CurrentColorScheme.XmlTag;
            s_colorAccessors["xml.property"] = () => CurrentColorScheme.XmlProperty;
            s_colorAccessors["xml.property.value"] = () => CurrentColorScheme.XmlPropertyValue;
            s_colorAccessors["xaml.markupextension"] = () => CurrentColorScheme.XamlMarkupExtension;
            s_colorAccessors["xaml.markupextension.property"] = () => CurrentColorScheme.XamlMarkupExtensionProperty;
            s_colorAccessors["xaml.markupextension.property.value"] = () => CurrentColorScheme.XamlMarkupExtensionPropertyValue;
        }

        public static void Register(ColorScheme colorScheme)
        {
            s_colorSchemes.Add(colorScheme);
            s_colorSchemeIDs.Add(colorScheme.Name, colorScheme);
        }

        public static readonly ColorScheme Light = new ColorScheme
        {
            Name = "Light",
            Background = Brush.Parse("#FFFFFF"),
            BackgroundAccent = Brush.Parse("#EEEEF2"),
            BracketMatch = Brush.Parse("#E2E6D6"),
            Border = Brush.Parse("#FFCCCEDB"),
            Text = Brush.Parse("#000000"),
            ErrorDiagnostic = Brush.Parse("#FD2D2D"),
            WarningDiagnostic = Brush.Parse("#FFCF28"),
            InfoDiagnostic = Brush.Parse("#0019FF"),
            StyleDiagnostic = Brush.Parse("#D4D4D4"),
            Comment = Brush.Parse("#008000"),
            Keyword = Brush.Parse("#0000FF"),
            Literal = Brush.Parse("#A31515"),
            Identifier = Brush.Parse("#000000"),
            CallExpression = Brush.Parse("#000000"),
            EnumConstant = Brush.Parse("#2B91AF"),
            InterfaceType = Brush.Parse("#2B91AF"),
            EnumType = Brush.Parse("#2B91AF"),
            NumericLiteral = Brush.Parse("#000000"),
            Punctuation = Brush.Parse("#000000"),
            Type = Brush.Parse("#2B91AF"),
            StructName = Brush.Parse("#2B91AF"),
            Operator = Brush.Parse("#000000"),
            DelegateName = Brush.Parse("#2B91AF"),
            XmlTag = Brush.Parse("#A31515"),
            XmlProperty = Brush.Parse("#FF0000"),
            XmlPropertyValue = Brush.Parse("#0000FF"),
            XamlMarkupExtension = Brush.Parse("#A31515"),
            XamlMarkupExtensionProperty = Brush.Parse("#FF0000"),
            XamlMarkupExtensionPropertyValue = Brush.Parse("#0000FF")
        };

        public static readonly ColorScheme Default = new ColorScheme
        {
            Name = "Dark",
            Background = Brush.Parse("#1a1a1a"),
            BackgroundAccent = Brush.Parse("#1c1c1c"),
            BracketMatch = Brush.Parse("#123e70"),
            Border = Brush.Parse("#FF3E3E42"),
            Text = Brush.Parse("#C8C8C8"),
            ErrorDiagnostic = Brush.Parse("#FD2D2D"),
            WarningDiagnostic = Brush.Parse("#FFCF28"),
            InfoDiagnostic = Brush.Parse("#0019FF"),
            StyleDiagnostic = Brush.Parse("#D4D4D4"),
            Comment = Brush.Parse("#57A64A"),
            Keyword = Brush.Parse("#569CD6"),
            Literal = Brush.Parse("#D69D85"),
            Identifier = Brush.Parse("#C8C8C8"),
            CallExpression = Brush.Parse("#DCDCAA"),
            EnumConstant = Brush.Parse("#B5CEA8"),
            InterfaceType = Brush.Parse("#B5CEA8"),
            EnumType = Brush.Parse("#B5CEA8"),
            NumericLiteral = Brush.Parse("#B5CEA8"),
            Punctuation = Brush.Parse("#808080"),
            Type = Brush.Parse("#4EC9B0"),
            StructName = Brush.Parse("#4EC9B0"),
            Operator = Brush.Parse("#B4B4B4"),
            DelegateName = Brush.Parse("#4EC9B0"),
            XmlTag = Brush.Parse("#DCDCDC"),
            XmlProperty = Brush.Parse("#90C7EA"),
            XmlPropertyValue = Brush.Parse("#569CD6"),
            XamlMarkupExtension = Brush.Parse("#BBA08C"),
            XamlMarkupExtensionProperty = Brush.Parse("#D4B765"),
            XamlMarkupExtensionPropertyValue = Brush.Parse("#B1B1B1")
        };

        public static readonly ColorScheme MonoDevelopLight = new ColorScheme
        {
            Name = "MonoDevelopLight",
            Background = Brush.Parse("#FFFFFF"),
            BackgroundAccent = Brush.Parse("#EEEEF2"),
            BracketMatch = Brush.Parse("#E2E6D6"),
            Border = Brush.Parse("#FFCCCEDB"),
            Text = Brush.Parse("#000000"),
            ErrorDiagnostic = Brush.Parse("#C44D58"),
            WarningDiagnostic = Brush.Parse("#FFCF28"),
            InfoDiagnostic = Brush.Parse("#0019FF"),
            StyleDiagnostic = Brush.Parse("#D4D4D4"),
            Comment = Brush.Parse("#888a85"),
            Keyword = Brush.Parse("#009695"),
            Literal = Brush.Parse("#db7100"),
            Identifier = Brush.Parse("#000000"),
            CallExpression = Brush.Parse("#000000"),
            EnumConstant = Brush.Parse("#3465a4"),
            InterfaceType = Brush.Parse("#3465a4"),
            EnumType = Brush.Parse("#2B91AF"),
            NumericLiteral = Brush.Parse("#000000"),
            Punctuation = Brush.Parse("#000000"),
            Type = Brush.Parse("#3465a4"),
            StructName = Brush.Parse("#3465a4"),
            Operator = Brush.Parse("#000000"),
            DelegateName = Brush.Parse("#2B91AF"),
            XmlTag = Brush.Parse("#A31515"),
            XmlProperty = Brush.Parse("#FF0000"),
            XmlPropertyValue = Brush.Parse("#0000FF"),
            XamlMarkupExtension = Brush.Parse("#A31515"),
            XamlMarkupExtensionProperty = Brush.Parse("#FF0000"),
            XamlMarkupExtensionPropertyValue = Brush.Parse("#0000FF")
        };

        public static readonly ColorScheme SolarizedDark = new ColorScheme
        {
            Name = "Solarized Dark",
            Background = Brush.Parse("#002b36"),
            BackgroundAccent = Brush.Parse("#073642"),
            BracketMatch = Brush.Parse("#123e70"),
            Border = Brush.Parse("#093844"),
            Text = Brush.Parse("#839496"),
            ErrorDiagnostic = Brush.Parse("#FD2D2D"),
            WarningDiagnostic = Brush.Parse("#FFCF28"),
            InfoDiagnostic = Brush.Parse("#0019FF"),
            StyleDiagnostic = Brush.Parse("#D4D4D4"),
            Comment = Brush.Parse("#586e75"),
            Keyword = Brush.Parse("#859900"),
            Literal = Brush.Parse("#2aa198"),
            Identifier = Brush.Parse("#839496"),
            CallExpression = Brush.Parse("#268bd2"),
            EnumConstant = Brush.Parse("#b58900"),
            InterfaceType = Brush.Parse("#b58900"),
            EnumType = Brush.Parse("#b58900"),
            NumericLiteral = Brush.Parse("#2aa198"),
            Punctuation = Brush.Parse("#839496"),
            Type = Brush.Parse("#b58900"),
            StructName = Brush.Parse("Red"),
            Operator = Brush.Parse("Red")
        };

        public static readonly ColorScheme SolarizedLight = new ColorScheme
        {
            Name = "Solarized Light",
            Background = Brush.Parse("#fdf6e3"),
            BackgroundAccent = Brush.Parse("#eee8d5"),
            BracketMatch = Brush.Parse("#E2E6D6"),
            Border = Brush.Parse("#F0F0d7"),
            Text = Brush.Parse("#657b83"),
            ErrorDiagnostic = Brush.Parse("#FD2D2D"),
            WarningDiagnostic = Brush.Parse("#FFCF28"),
            InfoDiagnostic = Brush.Parse("#0019FF"),
            StyleDiagnostic = Brush.Parse("#D4D4D4"),
            Comment = Brush.Parse("#93a1a1"),
            Keyword = Brush.Parse("#859900"),
            Literal = Brush.Parse("#2aa198"),
            Identifier = Brush.Parse("#839496"),
            CallExpression = Brush.Parse("#268bd2"),
            EnumConstant = Brush.Parse("#b58900"),
            InterfaceType = Brush.Parse("#b58900"),
            EnumType = Brush.Parse("#b58900"),
            NumericLiteral = Brush.Parse("#2aa198"),
            Punctuation = Brush.Parse("#839496"),
            Type = Brush.Parse("#b58900"),
            StructName = Brush.Parse("Red"),
            Operator = Brush.Parse("Red"),
            XmlTag = Brush.Parse("DarkMagenta"),
            XmlProperty = Brush.Parse("Red"),
            XmlPropertyValue = Brush.Parse("Blue")
        };

        public static ColorScheme LoadColorScheme(string name)
        {
            if (!string.IsNullOrEmpty(name) && s_colorSchemeIDs.ContainsKey(name))
            {
                LoadColorScheme(s_colorSchemeIDs[name]);

                return s_colorSchemeIDs[name];
            }
            else
            {
                LoadColorScheme(Default);

                return Default;
            }
        }

        public static void LoadColorScheme(ColorScheme colorScheme)
        {
            if (colorScheme != CurrentColorScheme)
            {
                if (Application.Current != null)
                {
                    Application.Current.Resources["EditorColorScheme"] = colorScheme;
                    Application.Current.Resources["EditorBackgroundBrush"] = colorScheme.Background;
                    Application.Current.Resources["EditorBackgroundAccentBrush"] = colorScheme.BackgroundAccent;
                    Application.Current.Resources["EditorForegroundBrush"] = colorScheme.Text;
                    Application.Current.Resources["EditorBorderBrush"] = colorScheme.Border;
                }

                CurrentColorScheme = colorScheme;
            }
        }

        public static ColorScheme CurrentColorScheme { get; private set; }

        public IBrush this[string key]
        {
            get
            {
                key = key.ToLower();

                if (s_colorAccessors.ContainsKey(key))
                {
                    return s_colorAccessors[key]();
                }

                return Brushes.Red;
            }
        }

        public string Name { get; private set; }

        public string Description { get; set; }

        [JsonProperty(PropertyName = "editor.background")]
        public IBrush Background { get; set; }

        [JsonProperty(PropertyName = "editor.background.accented")]
        public IBrush BackgroundAccent { get; set; }

        [JsonProperty(PropertyName = "editor.bracket.match")]
        public IBrush BracketMatch { get; set; }

        [JsonProperty(PropertyName = "editor.border")]
        public IBrush Border { get; set; }

        [JsonProperty(PropertyName = "editor.text")]
        public IBrush Text { get; set; }

        [JsonProperty(PropertyName = "editor.diagnostics.error")]
        public IBrush ErrorDiagnostic { get; set; }

        [JsonProperty(PropertyName = "editor.diagnostics.warning")]
        public IBrush WarningDiagnostic { get; set; }

        [JsonProperty(PropertyName = "editor.diagnostics.info")]
        public IBrush InfoDiagnostic { get; set; }

        [JsonProperty(PropertyName = "editor.diagnostics.style")]
        public IBrush StyleDiagnostic { get; set; }

        [JsonProperty(PropertyName = "editor.comment")]
        public IBrush Comment { get; set; }

        [JsonProperty(PropertyName = "editor.delegate.name")]
        public IBrush DelegateName { get; set; }

        [JsonProperty(PropertyName = "editor.keyword")]
        public IBrush Keyword { get; set; }

        [JsonProperty(PropertyName = "editor.literal")]
        public IBrush Literal { get; set; }

        [JsonProperty(PropertyName = "editor.identifier")]
        public IBrush Identifier { get; set; }

        [JsonProperty(PropertyName = "editor.callexpression")]
        public IBrush CallExpression { get; set; }

        [JsonProperty(PropertyName = "editor.numericliteral")]
        public IBrush NumericLiteral { get; set; }

        [JsonProperty(PropertyName = "editor.enumconst")]
        public IBrush EnumConstant { get; set; }

        [JsonProperty(PropertyName = "editor.enum")]
        public IBrush EnumType { get; set; }

        [JsonProperty(PropertyName = "editor.operator")]
        public IBrush Operator { get; set; }

        [JsonProperty(PropertyName = "editor.struct.name")]
        public IBrush StructName { get; set; }

        [JsonProperty(PropertyName = "editor.interface")]
        public IBrush InterfaceType { get; set; }

        [JsonProperty(PropertyName = "editor.punctuation")]
        public IBrush Punctuation { get; set; }

        [JsonProperty(PropertyName = "editor.type")]
        public IBrush Type { get; set; }

        [JsonProperty(PropertyName = "editor.xml.tag")]
        public IBrush XmlTag { get; set; }

        [JsonProperty(PropertyName = "editor.xml.property")]
        public IBrush XmlProperty { get; set; }

        [JsonProperty(PropertyName = "editor.xml.property.value")]
        public IBrush XmlPropertyValue { get; set; }

        [JsonProperty(PropertyName = "editor.xaml.markupext")]
        public IBrush XamlMarkupExtension { get; set; }

        [JsonProperty(PropertyName = "editor.xaml.markupext.property")]
        public IBrush XamlMarkupExtensionProperty { get; set; }

        [JsonProperty(PropertyName = "editor.xaml.markupext.property.value")]
        public IBrush XamlMarkupExtensionPropertyValue { get; set; }

        public void Save(string fileName)
        {
            SerializedObject.Serialize(Path.Combine(Platform.SettingsDirectory, fileName), this);
        }

        public static ColorScheme Load(string fileName)
        {
            return SerializedObject.Deserialize<ColorScheme>(fileName);
        }
    }
}
