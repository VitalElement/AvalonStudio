using AvalonStudio.Platforms;
using AvalonStudio.Utils;
using System.IO;
using Newtonsoft.Json;
using Avalonia.Media;

namespace AvalonStudio.Extensibility.Editor
{
    public class ColorScheme
    {
        public static readonly ColorScheme Default = new ColorScheme
        {
            Background = Brush.Parse("#1e1e1e"),
            BackgroundAccent = Brush.Parse("#1e1e1e"),
            Text = Brush.Parse("#DCDCDC"),
            Comment = Brush.Parse("#559A3F"),
            Keyword = Brush.Parse("#569CD6"),
            Literal = Brush.Parse("#D69D85"),
            Identifier = Brush.Parse("#C8C8C8"),
            CallExpression = Brush.Parse("#DCDCAA"),
            EnumConstant = Brush.Parse("#B5CEA8"),
            InterfaceType = Brush.Parse("#B5CEA8"),
            EnumType = Brush.Parse("#B5CEA8"),
            NumericLiteral = Brush.Parse("#B5CEA8"),
            Punctuation = Brush.Parse("#C8C8C8"),
            Type = Brush.Parse("#4EC9B0"),
        };

        public static readonly ColorScheme SolarizedDark = new ColorScheme {
            Background = Brush.Parse("#002b36"),
            BackgroundAccent = Brush.Parse("#073642"),
            Text = Brush.Parse("#839496"),
            Comment =Brush.Parse("#586e75"),
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
        };

        public static readonly ColorScheme SolarizedLight = new ColorScheme
        {
            Background = Brush.Parse("#fdf6e3"),
            BackgroundAccent = Brush.Parse("#eee8d5"),
            Text = Brush.Parse("#657b83"),
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
        };

        [JsonProperty(PropertyName = "editor.background")]
        public IBrush Background { get; set; }

        [JsonProperty(PropertyName = "editor.background.accented")]
        public IBrush BackgroundAccent { get; set; }

        [JsonProperty(PropertyName = "editor.text")]
        public IBrush Text { get; set; }

        [JsonProperty(PropertyName ="editor.comment")]
        public IBrush Comment { get; set; }

        [JsonProperty(PropertyName ="editor.keyword")]
        public IBrush Keyword { get; set; }

        [JsonProperty]
        public IBrush Literal { get; set; }

        [JsonProperty]
        public IBrush Identifier { get; set; }

        [JsonProperty]
        public IBrush CallExpression { get; set; }

        [JsonProperty]
        public IBrush NumericLiteral { get; set; }

        [JsonProperty]
        public IBrush EnumConstant { get; set; }

        [JsonProperty]
        public IBrush EnumType { get; set; }

        [JsonProperty]
        public IBrush InterfaceType { get; set; }

        [JsonProperty]
        public IBrush Punctuation { get; set; }

        [JsonProperty]
        public IBrush Type { get; set; }

        public void Save(string fileName)
        {
            SerializedObject.Serialize(Path.Combine(Platform.SettingsDirectory, fileName), this);
        }

        public static ColorScheme Load (string fileName)
        {
            return SerializedObject.Deserialize<ColorScheme>(fileName);
        }
    }
}
