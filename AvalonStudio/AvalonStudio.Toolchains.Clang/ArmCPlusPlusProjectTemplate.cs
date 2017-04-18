using AvalonStudio.Projects.CPlusPlus;

namespace AvalonStudio.Toolchains.Clang
{
    public class ArmCPlusPlusProjectTemplate : BlankCPlusPlusLanguageTemplate
    {
        public override string DefaultProjectName
        {
            get { return "ArmProject"; }
        }

        public override string Title
        {
            get { return "ARM C++ Project"; }
        }

        public override string Description
        {
            get { return "Basic template for any ARM based device."; }
        }
    }
}