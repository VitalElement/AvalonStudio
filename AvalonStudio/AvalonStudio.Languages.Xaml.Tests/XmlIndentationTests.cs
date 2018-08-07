using AvalonStudio.Extensibility.Tests;
using System;
using Xunit;
using AvalonStudio.Languages.Xaml;

namespace AvalonStudio.Languages.Xaml.Tests
{
    public class XmlIndentationTests
    {
        [Fact]
        public void Xml_Indentation_Inserts_Indentation_When_NewLine_Inserted_Between_Tags()
        {
            var testEditor = TestEditorManager.Create("<Tag>|</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("\n");

            testEditor.AssertEditorState("<Tag>\n  |\n</Tag>");
        }

        [Fact]
        public void Xml_Indentation_Doesnt_Indentation_When_NewLine_Inserted_Between_Tags_But_After_Short_Closed_Tag()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n  <Tag/>\n  |/Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("<");

            testEditor.AssertEditorState("<Tag>\n  <Tag/>\n<|/Tag>");
        }

        [Fact]
        public void Xml_Indentation_Inserts_Indentation_When_NewLine_Inserted_Between_Nested_Tags()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n  <Tag>|</Tag>\n</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("\n");

            testEditor.AssertEditorState("<Tag>\n  <Tag>\n    |\n  </Tag>\n</Tag>");
        }

        [Fact]
        public void Xml_Indentation_Doesnt_Indent_When_New_Line_outside_tag()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n  \n</Tag>|", new XmlIndentationTextInputHelper());

            testEditor.Input("\n");

            testEditor.AssertEditorState("<Tag>\n  \n</Tag>\n|");
        }

        [Fact]
        public void Xml_Indentation_Doesnt_Indent_When_New_Line_outside__nested_tag()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n  <Tag>\n  </Tag>|\n</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("\n");

            testEditor.AssertEditorState("<Tag>\n  <Tag>\n  </Tag>\n  |\n</Tag>");
        }

        [Fact]
        public void Xml_Indentation_Doesnt_Indent_When_New_Line_following_short_closed_tag()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n  <Tag />|\n</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("\n");

            testEditor.AssertEditorState("<Tag>\n  <Tag />\n  |\n</Tag>");
        }

        [Fact]
        public void Xml_Reindentation_Occurs_On_Element_Close()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n    <Tag /|\n</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input(">");

            testEditor.AssertEditorState("<Tag>\n  <Tag />|\n</Tag>");
        }

        [Fact]
        public void Xml_Reindentation_Occurs_On_Element_ShortClose()
        {
            var testEditor = TestEditorManager.Create("<Tag>\n    <Tag |\n</Tag>", new XmlIndentationTextInputHelper());

            testEditor.Input("/");

            testEditor.AssertEditorState("<Tag>\n  <Tag /|\n</Tag>");
        }
    }
}
