using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace AvalonStudio.TextEditor.Utils
{
    /// <summary>
    ///     Invokes an action when it is disposed.
    /// </summary>
    /// <remarks>
    ///     This class ensures the callback is invoked at most once,
    ///     even when Dispose is called on multiple threads.
    /// </remarks>
    internal sealed class CallbackOnDispose : IDisposable
    {
        private Action action;

        public CallbackOnDispose(Action action)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            this.action = action;
        }

        public void Dispose()
        {
            var a = Interlocked.Exchange(ref action, null);
            if (a != null)
            {
                a();
            }
        }
    }

    /// <summary>
    ///     This class is used to prevent stack overflows by representing a 'busy' flag
    ///     that prevents reentrance when another call is running.
    ///     However, using a simple 'bool busy' is not thread-safe, so we use a
    ///     thread-static BusyManager.
    /// </summary>
    internal static class BusyManager
    {
        [ThreadStatic] private static List<object> _activeObjects;

        public static BusyLock Enter(object obj)
        {
            var activeObjects = _activeObjects;
            if (activeObjects == null)
                activeObjects = _activeObjects = new List<object>();
            for (var i = 0; i < activeObjects.Count; i++)
            {
                if (activeObjects[i] == obj)
                    return BusyLock.Failed;
            }
            activeObjects.Add(obj);
            return new BusyLock(activeObjects);
        }

        [SuppressMessage("Microsoft.Performance", "CA1815:OverrideEqualsAndOperatorEqualsOnValueTypes")]
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible",
            Justification = "Should always be used with 'var'")]
        public struct BusyLock : IDisposable
        {
            public static readonly BusyLock Failed = new BusyLock(null);

            private readonly List<object> objectList;

            internal BusyLock(List<object> objectList)
            {
                this.objectList = objectList;
            }

            public bool Success
            {
                get { return objectList != null; }
            }

            public void Dispose()
            {
                if (objectList != null)
                {
                    objectList.RemoveAt(objectList.Count - 1);
                }
            }
        }
    }
}