using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Projects.CPlusPlus
{
    public class ReferenceViewModel : ViewModel<IProject>
    {
        private bool isReferenced;

        private string name;
        private readonly IProject referencer;

        public ReferenceViewModel(IProject referencer, IProject referencee) : base(referencee)
        {
            this.referencer = referencer;
            name = referencee.Name;
            isReferenced = referencer.References.Contains(referencee);
        }

        public string Name
        {
            get { return name; }
            set { this.RaiseAndSetIfChanged(ref name, value); }
        }

        public bool IsReferenced
        {
            get
            {
                return isReferenced;
            }
            set
            {
                if (isReferenced != value)
                {
                    isReferenced = value;

                    if (value)
                    {
                        referencer.AddReference(Model);
                    }
                    else
                    {
                        referencer.RemoveReference(Model);
                    }

                    referencer.Save();
                }
            }
        }
    }
}