using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace AvalonStudio.Languages.CSharp
{
    internal static class SignatureHelpExtensionMethods
    {
        public static void ConvertXmlDocumentation(this Signature signature)
        {
            if (!string.IsNullOrEmpty(signature.Documentation))
            {
                var fragments = signature.Documentation.Trim();
                var myRootedXml = "<root>" + fragments + "</root>";
                var documentationXml = XDocument.Parse(myRootedXml);

                var root = documentationXml.Element("root");
                var description = root.Element("summary");

                signature.Description = description.Value;

                var parametersXml = root.Elements("param");

                foreach (var param in parametersXml)
                {
                    var name = param.Attribute("name");

                    var parameter = signature.Parameters.FirstOrDefault(p => p.Name == name.Value);

                    parameter.Documentation = param.Value;
                }

                var exceptions = root.Elements("exception");

                foreach(var exception in exceptions)
                {
                    signature.Exceptions.Add(new SignatureException { Documentation = exception.Value, Type = exception.Attribute("cref").Value });
                }
            }
        }

        private static readonly string[] CharpInBuiltTypes = { "bool", "byte", "sbyte", "char", "decimal", "double", "float", "int", "uint", "long", "ulong", "object", "short", "ushort", "string" };

        private static bool IsCSharpInBuiltType(this string typeName)
        {
            return typeName.Contains(typeName);
        }

        private static void DetectReturnType(this Signature signature)
        {
            var parts = signature.Label.Split(' ');

            if (parts.First().IsCSharpInBuiltType())
            {
                signature.BuiltInReturnType = parts.First();
            }
            else
            {
                signature.ReturnType = parts.First();
            }
        }

        private static void DetectType(this Parameter parameter)
        {
            var parts = parameter.Label.Split(' ');

            if (parts.First().IsCSharpInBuiltType())
            {
                parameter.BuiltInType = parts.First();
            }
            else
            {
                parameter.Type = parts.First();
            }
        }

        private static void DetectParameterTypes(this Signature signature)
        {
            foreach (var parameter in signature.Parameters)
            {
                parameter.DetectType();
            }
        }

        public static void NormalizeSignatureData(this SignatureHelp signatureHelp)
        {
            foreach (var signature in signatureHelp.Signatures)
            {
                signature.ConvertXmlDocumentation();

                signature.DetectReturnType();
                signature.DetectParameterTypes();
            }
        }
    }
}