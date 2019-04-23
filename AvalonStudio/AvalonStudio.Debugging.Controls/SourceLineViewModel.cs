namespace AvalonStudio.Debugging
{
    public class SourceLineViewModel : LineViewModel
    {
        public SourceLineViewModel(SourceLine model) : base(model)
        {
        }

        public string LineText
        {
            get { return Model.LineText; }
        }

        public new SourceLine Model
        {
            get { return (SourceLine)base.Model; }
        }
    }
}