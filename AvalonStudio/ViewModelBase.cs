namespace Perspex.MVVM
{
    public class ViewModelBase<T> : ViewModelBase
    {
        public ViewModelBase(T model)
            : base(model)
        {
        }

        public T Model
        {
            get { return (T)model; }
            private set { model = value; this.Invalidate(); }
        }
    }

    public class ViewModelBase : ObservableObject
    {
        public ViewModelBase()
        {

        }

        public ViewModelBase(object model)
        {
            this.model = model;
        }

        public object BaseModel { get { return model; } }

        protected object model;
    }
}
