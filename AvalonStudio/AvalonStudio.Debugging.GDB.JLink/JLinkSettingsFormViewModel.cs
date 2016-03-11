namespace AvalonStudio.Debugging.GDB.JLink
{
    using AvalonStudio.MVVM;
    using AvalonStudio.Projects;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;

    public class JLinkSettingsFormViewModel : ViewModel<IProject>
    {
        private JLinkSettings settings;

        public JLinkSettingsFormViewModel(IProject model) : base(model)
        {
            settings = JLinkDebugAdaptor.GetSettings(model);

            //InterfaceOptions = new List<string>(Enum.GetNames(typeof(JlinkInterfaceType)));
            interfaceType = settings.Interface;
        }

        private void Save()
        {
            settings.Interface = (JlinkInterfaceType)interfaceSelectedIndex;            

            JLinkDebugAdaptor.SetSettings(Model, settings);
            Model.Save();
        }

        public string[] InterfaceOptions
        {
            get
            {
                return Enum.GetNames(typeof(JlinkInterfaceType));
            }
        }

        private int interfaceSelectedIndex;
        public int InterfaceSelectedIndex
        {
            get { return interfaceSelectedIndex; }
            set
            {
                interfaceSelectedIndex = value;
                Save();                                
            }
        }

        private JlinkInterfaceType interfaceType;
        public JlinkInterfaceType InterfaceType
        {
            get { return interfaceType; }
            set
            {
                interfaceType = value;
                //this.RaiseAndSetIfChanged(ref interfaceType, value);

                if (value != interfaceType)
                {
                    Save();
                }
            }
        }     
    }
}
