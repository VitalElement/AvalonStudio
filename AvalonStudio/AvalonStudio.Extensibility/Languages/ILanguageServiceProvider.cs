namespace AvalonStudio.Languages
{
    public interface ILanguageServiceProvider
    {
        ILanguageService CreateLanguageService();
    }
}
