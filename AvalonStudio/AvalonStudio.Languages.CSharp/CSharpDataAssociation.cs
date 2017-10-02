namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using Projects.OmniSharp;
    using System;

    internal class CSharpDataAssociation
    {
        public OmniSharpSolution Solution { get; set; }

        public EventHandler<TextInputEventArgs> BeforeTextInputHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}