using System;

namespace AvalonStudio.TextEditor.Document
{
	/// <summary>
	///     Allows registering a line tracker on a TextDocument using a weak reference from the document to the line tracker.
	/// </summary>
	public sealed class WeakLineTracker : ILineTracker
	{
		private readonly WeakReference targetObject;
		private TextDocument textDocument;

		private WeakLineTracker(TextDocument textDocument, ILineTracker targetTracker)
		{
			this.textDocument = textDocument;
			targetObject = new WeakReference(targetTracker);
		}

		void ILineTracker.BeforeRemoveLine(DocumentLine line)
		{
			var targetTracker = targetObject.Target as ILineTracker;
			if (targetTracker != null)
				targetTracker.BeforeRemoveLine(line);
			else
				Deregister();
		}

		void ILineTracker.SetLineLength(DocumentLine line, int newTotalLength)
		{
			var targetTracker = targetObject.Target as ILineTracker;
			if (targetTracker != null)
				targetTracker.SetLineLength(line, newTotalLength);
			else
				Deregister();
		}

		void ILineTracker.LineInserted(DocumentLine insertionPos, DocumentLine newLine)
		{
			var targetTracker = targetObject.Target as ILineTracker;
			if (targetTracker != null)
				targetTracker.LineInserted(insertionPos, newLine);
			else
				Deregister();
		}

		void ILineTracker.RebuildDocument()
		{
			var targetTracker = targetObject.Target as ILineTracker;
			if (targetTracker != null)
				targetTracker.RebuildDocument();
			else
				Deregister();
		}

		void ILineTracker.ChangeComplete(DocumentChangeEventArgs e)
		{
			var targetTracker = targetObject.Target as ILineTracker;
			if (targetTracker != null)
				targetTracker.ChangeComplete(e);
			else
				Deregister();
		}

		/// <summary>
		///     Registers the <paramref name="targetTracker" /> as line tracker for the <paramref name="textDocument" />.
		///     A weak reference to the target tracker will be used, and the WeakLineTracker will deregister itself
		///     when the target tracker is garbage collected.
		/// </summary>
		public static WeakLineTracker Register(TextDocument textDocument, ILineTracker targetTracker)
		{
			if (textDocument == null)
				throw new ArgumentNullException("textDocument");
			if (targetTracker == null)
				throw new ArgumentNullException("targetTracker");
			var wlt = new WeakLineTracker(textDocument, targetTracker);
			textDocument.LineTrackers.Add(wlt);
			return wlt;
		}

		/// <summary>
		///     Deregisters the weak line tracker.
		/// </summary>
		public void Deregister()
		{
			if (textDocument != null)
			{
				textDocument.LineTrackers.Remove(this);
				textDocument = null;
			}
		}
	}
}