namespace AvalonStudio.Extensibility.Reactive
{
    using Avalonia.Threading;
    using ReactiveUI;
    using System;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Concurrency;

    public static class AvaloniaObservableExtensions
    {
        public static PropertyHelper<TRet> ToProperty<TObj, TRet>(
            this IObservable<TRet> instance,
            TObj source,
            Expression<Func<TObj, TRet>> property,
            TRet initialValue = default(TRet),
            bool deferSubscription = false,
            IScheduler scheduler = null)
            where TObj : IReactiveObject
        {
            return source.ObservableToProperty(instance, property, initialValue, deferSubscription, scheduler);
        }

        public static PropertyHelper<TRet> ObservableToProperty<TObj, TRet>(
                this TObj instance,
                IObservable<TRet> observable,
                Expression<Func<TObj, TRet>> property,
                TRet initialValue = default(TRet),
                bool deferSubscription = false,
                IScheduler scheduler = null)
            where TObj : IReactiveObject
        {
            Expression expression = Reflection.Rewrite(property.Body);

            if (expression.GetParent().NodeType != ExpressionType.Parameter)
            {
                throw new ArgumentException("Property expression must be of the form 'x => x.SomeProperty'");
            }

            var name = expression.GetMemberInfo().Name;
            if (expression is IndexExpression)
                name += "[]";

            return new PropertyHelper<TRet>(instance, observable, name);
        }

        public static IObservable<TSource> ObserveOnUi<TSource>(this IObservable<TSource> source)
        {
            return new AnonymousObservable<TSource>(observer =>
            {
                return source.Subscribe(
                        x => Dispatcher.UIThread.InvokeAsync(() => observer.OnNext(x)),
                        exception => Dispatcher.UIThread.InvokeAsync(() => observer.OnError(exception)),
                        () => Dispatcher.UIThread.InvokeAsync(() => observer.OnCompleted()));
            });
        }
    }
}