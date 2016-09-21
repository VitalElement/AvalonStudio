namespace AvalonStudio.Extensibility.Reactive
{
    using Avalonia.Threading;
    using ReactiveUI;
    using System;
    using System.Linq.Expressions;
    using System.Reactive;
    using System.Reactive.Concurrency;

    public class PropertyHelper<TRet>
    {
        public PropertyHelper(IReactiveObject source, IObservable<TRet> observable, string propertyName)
        {
            observable.Subscribe(
                v =>
                {
                    source.RaisePropertyChanging(propertyName);
                    Value = v;
                    source.RaisePropertyChanged(propertyName);
                });
        }

        public TRet Value { get; set; }
    }

    public static class AvaloniaObservableExtensions
    {
        public static PropertyHelper<TRet> ToProperty<TObj, TRet>(
            this IObservable<TRet> This,
            TObj source,
            Expression<Func<TObj, TRet>> property,
            TRet initialValue = default(TRet),
            bool deferSubscription = false,
            IScheduler scheduler = null)
            where TObj : IReactiveObject
        {
            return source.ObservableToProperty(This, property, initialValue, deferSubscription, scheduler);
        }


        public static PropertyHelper<TRet> ObservableToProperty<TObj, TRet>(
                this TObj This,
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

            return new PropertyHelper<TRet>(This, observable, name);
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
