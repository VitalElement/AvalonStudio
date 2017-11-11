using Avalonia.Input;
using System;

namespace AvalonStudio.Extensibility.Documents
{
    public interface ITextDocument
    {
        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately before the TextArea handles the TextInput event.
        /// </summary>
        event EventHandler<TextInputEventArgs> TextEntering;

        /// <summary>
        /// Occurs when the TextArea receives text input.
        /// but occurs immediately after the TextArea handles the TextInput event.
        /// </summary>
        event EventHandler<TextInputEventArgs> TextEntered;

        void Save();

        void Replace(int offset, int length, string text);

        string Text { get; }

        int Caret { get; set; }
    }
}
