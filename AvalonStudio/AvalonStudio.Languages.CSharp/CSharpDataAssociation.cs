namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using AvaloniaEdit.Document;
    using AvaloniaEdit.Rendering;
    using AvalonStudio.Extensibility.Editor;
    using AvalonStudio.Languages;
    using CPlusPlus;
    using Projects.OmniSharp;
    using System;
    using System.Collections.Generic;

    internal class CSharpDataAssociation
    {
        public OmniSharpSolution Solution { get; set; }

        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}