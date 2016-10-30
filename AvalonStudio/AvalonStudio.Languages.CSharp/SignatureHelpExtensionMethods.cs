using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using System.Linq;
using System.Xml.Linq;

namespace AvalonStudio.Languages.CSharp
{
    internal static class SignatureHelpExtensionMethods
    {
        public static void ConvertXmlDocumentation(this Signature signature)
        {
            if (!string.IsNullOrEmpty(signature.Documentation))
            {
                var documentationXml = XDocument.Parse(signature.Documentation);
                var member = documentationXml.Element("member");
                var description = member.Element("summary");

                signature.Description = description.Value;

                var parametersXml = member.Elements("param");

                foreach (var param in parametersXml)
                {
                    var name = param.Attribute("name");

                    var parameter = signature.Parameters.FirstOrDefault(p => p.Name == name.Value);

                    parameter.Documentation = param.Value;
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