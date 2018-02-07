namespace AvalonStudio.Languages.CSharp
{
    using Avalonia.Input;
    using AvalonStudio.Projects;
    using System;

    internal class CSharpDataAssociation
    {
        public ISolution Solution { get; set; }

        public EventHandler<TextInputEventArgs> BeforeTextInputHandler { get; set; }
        public EventHandler<TextInputEventArgs> TextInputHandler { get; set; }
    }
}