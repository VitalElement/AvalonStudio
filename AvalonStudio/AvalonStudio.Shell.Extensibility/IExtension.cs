namespace AvalonStudio.Extensibility
{
    public interface IExtension
    {
    }

    public interface IActivatableExtension : IExtension
    {
        void BeforeActivation();

        void Activation();
    }
}