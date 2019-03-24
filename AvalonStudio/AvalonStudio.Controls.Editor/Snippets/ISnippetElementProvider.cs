using AvaloniaEdit.Snippets;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Editor.Snippets
{
    /// <summary>
	/// Used in "/SharpDevelop/ViewContent/AvalonEdit/SnippetElementProviders" to allow AddIns to provide custom snippet elements.
	/// </summary>
	public interface ISnippetElementProvider
    {
        SnippetElement GetElement(SnippetInfo snippetInfo);
    }

    public class SnippetInfo
    {
        public readonly string Tag;
        public readonly string SnippetText;
        public readonly int Position;

        public SnippetInfo(string tag, string snippetText, int position)
        {
            this.Tag = tag;
            this.SnippetText = snippetText;
            this.Position = position;
        }
    }
}
