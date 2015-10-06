namespace Perspex.MVVM
{
    public class ReactiveObject<T> : ReactiveObject
    {
        public ReactiveObject(T model)
            : base(model)
        {
        }

        public T Model
        {
            get { return (T)model; }
            private set { model = value; this.Invalidate(); }
        }
    }

    public class ReactiveObject : ObservableObject
    {
        public ReactiveObject()
        {

        }

        public ReactiveObject(object model)
        {
            this.model = model;
        }

        public object BaseModel { get { return model; } }

        protected object model;
    }
}
