using AvaloniaEdit.Snippets;
using AvalonStudio.Controls.Editor.Snippets;
using System;
using Xunit;
using static AvalonStudio.Controls.Editor.Snippets.SnippetParser;

namespace AvalonStudio.Controls.Standard.Tests
{
    public class SnippetParserTests
    {
        [Fact]
        public void Snippet_Can_Parse_When_Language_Service_Is_Null()
        {
            var snippet = SnippetParser.Parse(null, 1, 1, 1, "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}");

            Assert.Equal(18, snippet.Elements.Count);

            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[0], "int");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[1], " ");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[2], "ClassName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[3], "::get_");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[4], "Property");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[5], "()\n{\n\treturn ");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[6], "_ToFieldName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[7], ";\n}\n\nvoid ");
            AssertBoundSnippetElement(snippet.Elements[8], snippet.Elements[2]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[9], "::set_");
            AssertBoundSnippetElement(snippet.Elements[10], snippet.Elements[4]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[11], "(");
            AssertBoundSnippetElement(snippet.Elements[12], snippet.Elements[0]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[13], " value)\n{\t");
            AssertBoundSnippetElement(snippet.Elements[14], snippet.Elements[6]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[15], " = value;");
            Assert.IsType<SnippetCaretElement>(snippet.Elements[16]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[17], "\n}");
        }

        [Fact]
        public void Snippet_FallBack_When_LanguageService_Doesnt_Contain_Variable()
        {
            var languageService = new TestLanguageService();

            languageService.SnippetCodeGenerators.Add("ToFieldName", (propertyName) =>
            {
                if (string.IsNullOrEmpty(propertyName))
                    return propertyName;
                string newName = Char.ToLower(propertyName[0]) + propertyName.Substring(1);
                if (newName == propertyName)
                    return "_" + newName;
                else
                    return newName;
            });

            var snippet = SnippetParser.Parse(languageService, 1, 1, 1, "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}");

            Assert.Equal(18, snippet.Elements.Count);

            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[0], "int");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[1], " ");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[2], "ClassName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[3], "::get_");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[4], "Property");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[5], "()\n{\n\treturn ");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[7], ";\n}\n\nvoid ");
            AssertBoundSnippetElement(snippet.Elements[8], snippet.Elements[2]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[9], "::set_");
            AssertBoundSnippetElement(snippet.Elements[10], snippet.Elements[4]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[11], "(");
            AssertBoundSnippetElement(snippet.Elements[12], snippet.Elements[0]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[13], " value)\n{\t");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[15], " = value;");
            Assert.IsType<SnippetCaretElement>(snippet.Elements[16]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[17], "\n}");
        }

        [Fact]
        public void Snippet_FallBack_When_LanguageService_Contains_Variable_But_Reports_Null()
        {
            var languageService = new TestLanguageService();

            languageService.SnippetCodeGenerators.Add("ToFieldName", (propertyName) =>
            {
                if (string.IsNullOrEmpty(propertyName))
                    return propertyName;
                string newName = Char.ToLower(propertyName[0]) + propertyName.Substring(1);
                if (newName == propertyName)
                    return "_" + newName;
                else
                    return newName;
            });

            languageService.SnippetDynamicVariables.Add("ClassName", (offset, line, column) => null);

            var snippet = SnippetParser.Parse(languageService, 1, 1, 1, "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}");

            Assert.Equal(18, snippet.Elements.Count);

            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[0], "int");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[1], " ");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[2], "ClassName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[3], "::get_");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[4], "Property");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[5], "()\n{\n\treturn ");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[7], ";\n}\n\nvoid ");
            AssertBoundSnippetElement(snippet.Elements[8], snippet.Elements[2]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[9], "::set_");
            AssertBoundSnippetElement(snippet.Elements[10], snippet.Elements[4]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[11], "(");
            AssertBoundSnippetElement(snippet.Elements[12], snippet.Elements[0]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[13], " value)\n{\t");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[15], " = value;");
            Assert.IsType<SnippetCaretElement>(snippet.Elements[16]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[17], "\n}");
        }

        [Fact]
        public void Snippet_Correctly_Parses_Variable_And_Function_From_Language_Service()
        {
            var languageService = new TestLanguageService();

            languageService.SnippetCodeGenerators.Add("ToFieldName", (propertyName) =>
            {
                if (string.IsNullOrEmpty(propertyName))
                    return propertyName;
                string newName = Char.ToLower(propertyName[0]) + propertyName.Substring(1);
                if (newName == propertyName)
                    return "_" + newName;
                else
                    return newName;
            });

            languageService.SnippetDynamicVariables.Add("ClassName", (offset, line, column) => $"Ls_ClassName{offset}_{line}_{column}");

            var snippet = SnippetParser.Parse(languageService, 8213, 101, 32, "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}");

            Assert.Equal(18, snippet.Elements.Count);

            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[0], "int");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[1], " ");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[2], "Ls_ClassName8213_101_32");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[3], "::get_");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[4], "Property");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[5], "()\n{\n\treturn ");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[6], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[7], ";\n}\n\nvoid ");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[8], "Ls_ClassName8213_101_32");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[9], "::set_");
            AssertBoundSnippetElement(snippet.Elements[10], snippet.Elements[4]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[11], "(");
            AssertBoundSnippetElement(snippet.Elements[12], snippet.Elements[0]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[13], " value)\n{\t");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "PropertyName", "propertyName");
            AssertFunctionBoundSnippetElement(snippet.Elements[14], snippet.Elements[4], "propertyName", "_propertyName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[15], " = value;");
            Assert.IsType<SnippetCaretElement>(snippet.Elements[16]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[17], "\n}");
        }

        [Fact]
        public void Snippet_Correctly_Falls_Back_When_Language_Service_Doesnt_Contain_Function()
        {
            var languageService = new TestLanguageService();
            
            languageService.SnippetDynamicVariables.Add("ClassName", (offset, line, column) => $"Ls_ClassName{offset}_{line}_{column}");

            var snippet = SnippetParser.Parse(languageService, 8213, 101, 32, "${type=int} ${ClassName}::get_${property=Property}()\n{\n\treturn ${ToFieldName(property)};\n}\n\nvoid ${ClassName}::set_${property}(${type} value)\n{\t${ToFieldName(property)} = value;${Caret}\n}");

            Assert.Equal(18, snippet.Elements.Count);

            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[0], "int");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[1], " ");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[2], "Ls_ClassName8213_101_32");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[3], "::get_");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[4], "Property");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[5], "()\n{\n\treturn ");
            AssertSnippetTextElement<SnippetReplaceableTextElement>(snippet.Elements[6], "_ToFieldName");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[7], ";\n}\n\nvoid ");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[8], "Ls_ClassName8213_101_32");
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[9], "::set_");
            AssertBoundSnippetElement(snippet.Elements[10], snippet.Elements[4]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[11], "(");
            AssertBoundSnippetElement(snippet.Elements[12], snippet.Elements[0]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[13], " value)\n{\t");
            AssertBoundSnippetElement(snippet.Elements[14], snippet.Elements[6]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[15], " = value;");
            Assert.IsType<SnippetCaretElement>(snippet.Elements[16]);
            AssertSnippetTextElement<SnippetTextElement>(snippet.Elements[17], "\n}");
        }
        
        public static void AssertSnippetTextElement<T>(SnippetElement snippet, string text)
        {
            Assert.IsType<T>(snippet);
            Assert.Equal(text, (snippet as SnippetTextElement).Text);
        }

        public static void AssertBoundSnippetElement(SnippetElement snippet, SnippetElement target)
        {
            Assert.IsType<SnippetBoundElement>(snippet);
            Assert.IsType<SnippetReplaceableTextElement>(target);
            Assert.Equal((snippet as SnippetBoundElement).TargetElement, target);
        }

        public static void AssertFunctionBoundSnippetElement(SnippetElement snippet, SnippetElement target, string input, string expected)
        {
            Assert.IsType<FunctionBoundElement>(snippet);
            Assert.IsType<SnippetReplaceableTextElement>(target);
            Assert.Equal((snippet as FunctionBoundElement).TargetElement, target);
            Assert.Equal(expected, (snippet as FunctionBoundElement).ConvertText(input));
        }
    }
}
