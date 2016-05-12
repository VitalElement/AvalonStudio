namespace AvalonStudio.TextEditor.Utils
{
    /// <summary>
	/// Provides immutable empty list instances.
	/// </summary>
	static class Empty<T>
    {
        public static readonly T[] Array = new T[0];
        //public static readonly ReadOnlyCollection<T> ReadOnlyCollection = new ReadOnlyCollection<T>(Array);
    }
}
