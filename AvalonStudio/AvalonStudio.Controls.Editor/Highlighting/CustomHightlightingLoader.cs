using Avalonia.Markup.Xaml.Converters;
using Avalonia.Media;
using AvaloniaEdit.Highlighting;
using AvaloniaEdit.Highlighting.Xshd;
using AvaloniaEdit.Rendering;
using AvaloniaEdit.Utils;
using AvalonStudio.Extensibility.Editor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;

namespace AvalonStudio.Controls.Editor.Highlighting
{
    internal class ThemedHighlightingBrush : HighlightingBrush
    {
        private string _brushName;

        public ThemedHighlightingBrush(string name)
        {
            _brushName = name;
        }

        public override IBrush GetBrush(ITextRunConstructionContext context)
        {
            return ColorScheme.CurrentColorScheme[_brushName];
        }
    }

    public class ASXshdSyntaxDefinition : XshdSyntaxDefinition
    {
        public ASXshdSyntaxDefinition()
        {
            ContentTypes = new List<string>();
        }

        public IList<string> ContentTypes { get; }
    }

    /// <summary>
    /// Loads .xshd files, version 2.0.
    /// Version 2.0 files are recognized by the namespace.
    /// </summary>
    internal static class V2Loader
    {
        public const string Namespace = "http://icsharpcode.net/sharpdevelop/syntaxdefinition/2008";

        public static ASXshdSyntaxDefinition LoadDefinition(XmlReader reader, bool skipValidation)
        {
            var settings = new XmlReaderSettings
            {
                CloseInput = true,
                IgnoreComments = true,
                IgnoreWhitespace = true
            };

            reader = XmlReader.Create(reader, settings);

            reader.Read();

            return ParseDefinition(reader);
        }

        private static ASXshdSyntaxDefinition ParseDefinition(XmlReader reader)
        {
            Debug.Assert(reader.LocalName == "SyntaxDefinition");
            var def = new ASXshdSyntaxDefinition { Name = reader.GetAttribute("name") };
            var extensions = reader.GetAttribute("extensions");
            if (extensions != null)
                def.Extensions.AddRange(extensions.Split(';'));

            var contentTypes = reader.GetAttribute("contentTypes");
            if (contentTypes != null)
                def.ContentTypes.AddRange(contentTypes.Split(';'));

            ParseElements(def.Elements, reader);
            Debug.Assert(reader.NodeType == XmlNodeType.EndElement);
            Debug.Assert(reader.LocalName == "SyntaxDefinition");
            return def;
        }

        private static void ParseElements(ICollection<XshdElement> c, XmlReader reader)
        {
            if (reader.IsEmptyElement)
                return;
            while (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
            {
                Debug.Assert(reader.NodeType == XmlNodeType.Element);
                if (reader.NamespaceURI != Namespace)
                {
                    if (!reader.IsEmptyElement)
                        reader.Skip();
                    continue;
                }
                switch (reader.Name)
                {
                    case "RuleSet":
                        c.Add(ParseRuleSet(reader));
                        break;
                    case "Property":
                        c.Add(ParseProperty(reader));
                        break;                    
                    case "Keywords":
                        c.Add(ParseKeywords(reader));
                        break;
                    case "Span":
                        c.Add(ParseSpan(reader));
                        break;
                    case "Import":
                        c.Add(ParseImport(reader));
                        break;
                    case "Rule":
                        c.Add(ParseRule(reader));
                        break;
                    default:
                        throw new NotSupportedException("Unknown element " + reader.Name);
                }
            }
        }

        private static XshdElement ParseProperty(XmlReader reader)
        {
            var property = new XshdProperty();
            SetPosition(property, reader);
            property.Name = reader.GetAttribute("name");
            property.Value = reader.GetAttribute("value");
            return property;
        }

        private static XshdRuleSet ParseRuleSet(XmlReader reader)
        {
            var ruleSet = new XshdRuleSet();
            SetPosition(ruleSet, reader);
            ruleSet.Name = reader.GetAttribute("name");
            ruleSet.IgnoreCase = reader.GetBoolAttribute("ignoreCase");

            CheckElementName(reader, ruleSet.Name);
            ParseElements(ruleSet.Elements, reader);
            return ruleSet;
        }

        private static XshdRule ParseRule(XmlReader reader)
        {
            var rule = new XshdRule();
            SetPosition(rule, reader);
            rule.ColorReference = ParseColorReference(reader);

            var pattern = reader.GetAttribute("pattern");

            if (!reader.IsEmptyElement)
            {
                reader.Read();
                if (reader.NodeType == XmlNodeType.Text)
                {
                    rule.Regex = reader.ReadContentAsString();
                    rule.RegexType = XshdRegexType.IgnorePatternWhitespace;
                }
            }
            else if(pattern != null)
            {
                rule.Regex = pattern;
            }

            return rule;
        }

        private static XshdKeywords ParseKeywords(XmlReader reader)
        {
            var keywords = new XshdKeywords();
            SetPosition(keywords, reader);
            keywords.ColorReference = ParseColorReference(reader);
            reader.Read();
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                Debug.Assert(reader.NodeType == XmlNodeType.Element);
                keywords.Words.Add(reader.ReadElementContentAsString());
            }
            return keywords;
        }

        private static XshdImport ParseImport(XmlReader reader)
        {
            var import = new XshdImport();
            SetPosition(import, reader);
            import.RuleSetReference = ParseRuleSetReference(reader);
            if (!reader.IsEmptyElement)
                reader.Skip();
            return import;
        }

        private static XshdSpan ParseSpan(XmlReader reader)
        {
            var span = new XshdSpan();
            SetPosition(span, reader);
            span.BeginRegex = reader.GetAttribute("begin");
            span.EndRegex = reader.GetAttribute("end");
            span.Multiline = reader.GetBoolAttribute("multiline") ?? false;
            span.SpanColorReference = ParseColorReference(reader);
            span.RuleSetReference = ParseRuleSetReference(reader);
            if (!reader.IsEmptyElement)
            {
                reader.Read();
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    Debug.Assert(reader.NodeType == XmlNodeType.Element);
                    switch (reader.Name)
                    {
                        case "Begin":
                            if (span.BeginRegex != null)
                                throw Error(reader, "Duplicate Begin regex");
                            span.BeginColorReference = ParseColorReference(reader);
                            span.BeginRegex = reader.ReadElementContentAsString();
                            span.BeginRegexType = XshdRegexType.IgnorePatternWhitespace;
                            break;
                        case "End":
                            if (span.EndRegex != null)
                                throw Error(reader, "Duplicate End regex");
                            span.EndColorReference = ParseColorReference(reader);
                            span.EndRegex = reader.ReadElementContentAsString();
                            span.EndRegexType = XshdRegexType.IgnorePatternWhitespace;
                            break;
                        case "RuleSet":
                            if (span.RuleSetReference.ReferencedElement != null)
                                throw Error(reader, "Cannot specify both inline RuleSet and RuleSet reference");
                            span.RuleSetReference = new XshdReference<XshdRuleSet>(ParseRuleSet(reader));
                            reader.Read();
                            break;
                        default:
                            throw new NotSupportedException("Unknown element " + reader.Name);
                    }
                }
            }
            return span;
        }

        private static Exception Error(XmlReader reader, string message)
        {
            return Error(reader as IXmlLineInfo, message);
        }

        private static Exception Error(IXmlLineInfo lineInfo, string message)
        {
            return new HighlightingDefinitionInvalidException(message);
        }

        /// <summary>
        /// Sets the element's position to the XmlReader's position.
        /// </summary>
        private static void SetPosition(XshdElement element, XmlReader reader)
        {
            if (reader is IXmlLineInfo lineInfo)
            {
                element.LineNumber = lineInfo.LineNumber;
                element.ColumnNumber = lineInfo.LinePosition;
            }
        }

        private static XshdReference<XshdRuleSet> ParseRuleSetReference(XmlReader reader)
        {
            var ruleSet = reader.GetAttribute("ruleSet");
            if (ruleSet != null)
            {
                // '/' is valid in highlighting definition names, so we need the last occurence
                var pos = ruleSet.LastIndexOf('/');
                if (pos >= 0)
                {
                    return new XshdReference<XshdRuleSet>(ruleSet.Substring(0, pos), ruleSet.Substring(pos + 1));
                }
                else
                {
                    return new XshdReference<XshdRuleSet>(null, ruleSet);
                }
            }
            else
            {
                return new XshdReference<XshdRuleSet>();
            }
        }

        private static void CheckElementName(XmlReader reader, string name)
        {
            if (name != null)
            {
                if (name.Length == 0)
                    throw Error(reader, "The empty string is not a valid name.");
                if (name.IndexOf('/') >= 0)
                    throw Error(reader, "Element names must not contain a slash.");
            }
        }

        #region ParseColor

        private static XshdColor ParseNamedColor(XmlReader reader)
        {
            var color = ParseColorAttributes(reader);
            // check removed: invisible named colors may be useful now that apps can read highlighting data
            //if (color.Foreground == null && color.FontWeight == null && color.FontStyle == null)
            //	throw Error(reader, "A named color must have at least one element.");
            color.Name = reader.GetAttribute("name");
            CheckElementName(reader, color.Name);
            color.ExampleText = reader.GetAttribute("exampleText");
            return color;
        }

        private static XshdReference<XshdColor> ParseColorReference(XmlReader reader)
        {
            var color = reader.GetAttribute("color");
            if (color != null)
            {
                return new XshdReference<XshdColor>(ParseThemeColorReference(color));                
            }
            else
            {
                return new XshdReference<XshdColor>();
            }
        }

        private static XshdColor ParseThemeColorReference(string colorKey)
        {
            var color = new XshdColor
            {
                Foreground = ParseColorName(colorKey)
            };

            return color;
        }

        private static XshdColor ParseColorAttributes(XmlReader reader)
        {
            var color = new XshdColor();
            SetPosition(color, reader);
            
            color.Foreground = ParseColorName(reader.GetAttribute("name"));
            color.Background = ParseColor(reader.GetAttribute("background"));
            color.FontWeight = ParseFontWeight(reader.GetAttribute("fontWeight"));
            color.FontStyle = ParseFontStyle(reader.GetAttribute("fontStyle"));
            color.Underline = reader.GetBoolAttribute("underline");
            return color;
        }        

        private static HighlightingBrush ParseColor(string color)
        {
            if (string.IsNullOrEmpty(color))
                return null;
            return FixedColorHighlightingBrush(Color.Parse(color));
        }

        private static HighlightingBrush ParseColorName (string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return new ThemedHighlightingBrush(name);
        }

        private static HighlightingBrush FixedColorHighlightingBrush(Color? color)
        {
            if (color == null)
                return null;
            return new SimpleHighlightingBrush(color.Value);
        }

        private static FontWeight? ParseFontWeight(string fontWeight)
        {
            if (string.IsNullOrEmpty(fontWeight))
                return null;
            return (FontWeight)Enum.Parse(typeof(FontWeight), fontWeight, ignoreCase: true);
        }

        private static FontStyle? ParseFontStyle(string fontStyle)
        {
            if (string.IsNullOrEmpty(fontStyle))
                return null;
            return (FontStyle)Enum.Parse(typeof(FontStyle), fontStyle, ignoreCase: true);
        }
        #endregion
    }

    public class CustomHighlightingManager : IHighlightingDefinitionReferenceResolver
    {
        private readonly Dictionary<string, IHighlightingDefinition> _highlightingsByContentTypes = new Dictionary<string, IHighlightingDefinition>();

        public static CustomHighlightingManager Instance { get; } = new CustomHighlightingManager();

        public CustomHighlightingManager()
        {
            Resources.Resources.RegisterBuiltInHighlightings(this);
        }

        /// <summary>
        /// Gets a highlighting definition by name.
        /// Returns null if the definition is not found.
        /// </summary>
        public IHighlightingDefinition GetDefinition(string contentType)
        {
            lock (this)
            {
                return _highlightingsByContentTypes.TryGetValue(contentType, out var rh) ? rh as IHighlightingDefinition : null;
            }
        }

        internal void RegisterHighlighting(string resourceName)
        {
            try
            {
                var info = Load(resourceName);

                RegisterHighlighting(info.contentTypes, info.definition);
            }
            catch (HighlightingDefinitionInvalidException ex)
            {
                throw new InvalidOperationException("The built-in highlighting '" + resourceName + "' is invalid.", ex);
            }
        }

        public void RegisterHighlighting(IEnumerable<string> contentTypes, IHighlightingDefinition highlighting)
        {
            if (highlighting == null)
            {
                throw new ArgumentNullException(nameof(highlighting));
            }

            foreach(var contentType in contentTypes)
            {
                lock(this)
                {
                    _highlightingsByContentTypes[contentType] = highlighting;
                }
            }
        }

        public (IEnumerable<string> contentTypes, IHighlightingDefinition definition) Load(string resourceName)
        {
            ASXshdSyntaxDefinition xshd;
            using (var s = Resources.Resources.OpenStream(resourceName))
            using (var reader = XmlReader.Create(s))
            {
                // in release builds, skip validating the built-in highlightings
                xshd = LoadXshd(reader, true);
            }
            return (xshd.ContentTypes, HighlightingLoader.Load(xshd, this));
        }

        internal static ASXshdSyntaxDefinition LoadXshd(XmlReader reader, bool skipValidation)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));
            try
            {
                reader.MoveToContent();

                return V2Loader.LoadDefinition(reader, skipValidation);
            }
            catch (XmlException ex)
            {
                throw new Exception($"{ex} - {ex.LineNumber} - {ex.LinePosition}");
            }
        }
    }
}
