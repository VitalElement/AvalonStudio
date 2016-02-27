namespace AvalonStudio.Utils
{
    using Perspex.Utilities;
    using System;

    public class WeakSubscriber<T> : IWeakSubscriber<T>
    {
        private readonly Action<T> _onEvent;

        public WeakSubscriber(Action<T> onEvent)
        {
            _onEvent = onEvent;
        }

        public void OnEvent(object sender, T ev)
        {
            _onEvent?.Invoke(ev);
        }

        public static WeakSubscriber<T> Subscribe(object target, string eventName, Action<T> onEvent)
        {
            var result = new WeakSubscriber<T>(onEvent);

            WeakSubscriptionManager.Subscribe(target, eventName, result);

            return result;
        }
    }
}
