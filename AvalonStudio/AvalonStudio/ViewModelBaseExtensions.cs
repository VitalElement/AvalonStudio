using ReactiveUI;
using System;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace AvalonStudio.MVVM
{
    public static class ReactiveObjectExtensions
    {
        public static void OnPropertyChanged(this ReactiveObject reactiveObject, [CallerMemberName] string propertyName = null)
        {
            reactiveObject.RaisePropertyChanged(propertyName);
        }

        public static void RaisePropertyChanged<T>(this ReactiveObject reactiveObject, Expression<Func<T>> changedProperty)
        {
            var name = ((MemberExpression)changedProperty.Body).Member.Name;
            reactiveObject.OnPropertyChanged(name);
        }
    }
}