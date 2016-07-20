namespace AvalonStudio.TextEditor.Document
{
	/// <summary>
	///     This Interface describes a the basic Undo/Redo operation
	///     all Undo Operations must implement this interface.
	/// </summary>
	public interface IUndoableOperation
	{
		/// <summary>
		///     Undo the last operation
		/// </summary>
		void Undo();

		/// <summary>
		///     Redo the last operation
		/// </summary>
		void Redo();
	}

	internal interface IUndoableOperationWithContext : IUndoableOperation
	{
		void Undo(UndoStack stack);
		void Redo(UndoStack stack);
	}
}