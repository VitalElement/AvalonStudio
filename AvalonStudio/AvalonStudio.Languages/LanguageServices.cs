namespace AvalonStudio.Languages
{
    using Projects;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;
    using System.ComponentModel.Composition.Hosting;
    using System.IO;
    using System.Linq;

    public class LanguageServices
    {
        private LanguageServices()
        {
            var location = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            var catalog = new DirectoryCatalog(location);
                          
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);
        }

        public static LanguageServices Instance = new LanguageServices();

        [ImportMany(typeof(ILanguageService))]
        List<ILanguageService> languageServices { get; set; }

        public ILanguageService GetLanguageService (ISourceFile file)
        {
            ILanguageService result = null;

            var services = languageServices.Where((ls) => ls.SupportsFile(file));

            switch (services.Count())
            {
                case 0:
                    throw new System.Exception("No language service has been found to support the file type.");

                case 1:
                    result = services.FirstOrDefault();
                    break;

                default:
                    throw new System.Exception("Multiple language services are available for the same file type.");
            }

            return result;
        }
    }
}
