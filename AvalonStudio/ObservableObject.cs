namespace Perspex.MVVM
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
        {
            if (!object.ReferenceEquals(field, value))
            {
                field = value;
                this.OnPropertyChanged(propertyName);
            }
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> changedProperty)
        {
            if (PropertyChanged != null)
            {
                string name = ((MemberExpression)changedProperty.Body).Member.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void Invalidate()
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(string.Empty));
            }
        }
    }
}
