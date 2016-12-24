using System;
using System.Diagnostics;
using System.Windows;

namespace AvalonStudio.TextEditor.Utils
{
	/// <summary>
	///     WeakEventManager with AddListener/RemoveListener and CurrentManager implementation.
	///     Helps implementing the WeakEventManager pattern with less code.
	/// </summary>
	public abstract class WeakEventManagerBase<TManager, TEventSource> : WeakEventManager
		where TManager : WeakEventManagerBase<TManager, TEventSource>, new()
		where TEventSource : class
	{
		/// <summary>
		///     Creates a new WeakEventManagerBase instance.
		/// </summary>
		protected WeakEventManagerBase()
		{
			Debug.Assert(GetType() == typeof (TManager));
		}

		/// <summary>
		///     Gets the current manager.
		/// </summary>
		protected static TManager CurrentManager
		{
			get
			{
				var managerType = typeof (TManager);
				var manager = (TManager) GetCurrentManager(managerType);
				if (manager == null)
				{
					manager = new TManager();
					SetCurrentManager(managerType, manager);
				}
				return manager;
			}
		}

		/// <summary>
		///     Adds a weak event listener.
		/// </summary>
		public static void AddListener(TEventSource source, IWeakEventListener listener)
		{
			CurrentManager.ProtectedAddListener(source, listener);
		}

		/// <summary>
		///     Removes a weak event listener.
		/// </summary>
		public static void RemoveListener(TEventSource source, IWeakEventListener listener)
		{
			CurrentManager.ProtectedRemoveListener(source, listener);
		}

		/// <inheritdoc />
		protected sealed override void StartListening(object source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			StartListening((TEventSource) source);
		}

		/// <inheritdoc />
		protected sealed override void StopListening(object source)
		{
			if (source == null)
				throw new ArgumentNullException("source");
			StopListening((TEventSource) source);
		}

		/// <summary>
		///     Attaches the event handler.
		/// </summary>
		protected abstract void StartListening(TEventSource source);

		/// <summary>
		///     Detaches the event handler.
		/// </summary>
		protected abstract void StopListening(TEventSource source);
	}
}