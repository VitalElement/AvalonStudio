using ReactiveUI;
using System;
using System.Diagnostics;
using System.Windows;

namespace AvalonStudio.TextEditor.Utils
{
	/// <summary>
	///     WeakEventManager with AddListener/RemoveListener and CurrentManager implementation.
	///     Helps implementing the WeakEventManager pattern with less code.
	/// </summary>
	public abstract class WeakEventManagerBase<TManager, TEventSource, TEventHandler, TEventArgs> : WeakEventManager<TEventSource, TEventHandler, TEventArgs>
		where TManager : WeakEventManagerBase<TManager, TEventSource, TEventHandler, TEventArgs>, new()
		where TEventSource : class
	{
		/// <summary>
		///     Creates a new WeakEventManagerBase instance.
		/// </summary>
		protected WeakEventManagerBase()
		{
		}

        private static TManager s_currentManager;

		/// <summary>
		///     Gets the current manager.
		/// </summary>
		protected static TManager CurrentManager
		{
			get
			{
				var managerType = typeof (TManager);
				
				if (s_currentManager == null)
				{
                    s_currentManager = new TManager();
				}

				return s_currentManager;
			}
		}

		/// <summary>
		///     Adds a weak event listener.
		/// </summary>
		public static void AddListener(TEventSource source)
		{
			CurrentManager.StartListening(source);
		}

		/// <summary>
		///     Removes a weak event listener.
		/// </summary>
		public static void RemoveListener(TEventSource source)
		{
			CurrentManager.StopListening(source);
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