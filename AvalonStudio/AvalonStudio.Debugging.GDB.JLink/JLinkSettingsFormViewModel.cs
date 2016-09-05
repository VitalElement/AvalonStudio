using System;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using System.Collections.Generic;
using ReactiveUI;
using System.IO;
using AvalonStudio.Platforms;
using System.Linq;
using Avalonia.Threading;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public class JLinkSettingsFormViewModel : ViewModel<IProject>
    {
        private int interfaceSelectedIndex;

        private JlinkInterfaceType interfaceType;
        private readonly JLinkSettings settings;

        public JLinkSettingsFormViewModel(IProject model) : base(model)
        {
            settings = JLinkDebugAdaptor.GetSettings(model);

            //InterfaceOptions = new List<string>(Enum.GetNames(typeof(JlinkInterfaceType)));
            interfaceSelectedIndex = (int)settings.Interface;
            interfaceType = settings.Interface;

            string devPath = Path.Combine(JLinkDebugAdaptor.BaseDirectory, "devices.csv");

            deviceList = new ObservableCollection<JLinkTargetDeviceViewModel>();

            if (System.IO.File.Exists(devPath))
            {
                LoadDeviceList(devPath);
            }
        }

        private bool hasLoaded = false;

        private async void LoadDeviceList(string deviceFile)
        {
            var list = new ObservableCollection<JLinkTargetDeviceViewModel>();

            using (TextReader tr = System.IO.File.OpenText(deviceFile))
            {
                tr.ReadLine();

                string line = null;
                while ((line = await tr.ReadLineAsync()) != null)
                {
                    line = line.Replace("\"", string.Empty);
                    line = line.Replace("{", string.Empty);
                    line = line.Replace("}", string.Empty);
                    var splits = line.Split(',');
                    var newdev = new JLinkTargetDeviceViewModel();
                    newdev.Manufacturer = splits[0];
                    newdev.Device = splits[1].Trim();
                    newdev.Core = splits[2].Trim();
                    newdev.FlashStart = Convert.ToUInt32(splits[3].Trim(), 16);
                    newdev.FlashLength = Convert.ToUInt32(splits[4].Trim(), 16);
                    newdev.RamStart = Convert.ToUInt32(splits[5].Trim(), 16);
                    newdev.RamLength = Convert.ToUInt32(splits[6].Trim(), 16);

                    list.Add(newdev);
                }
            }

            unfilteredList = list;

            await FilterListAsync();

            var selectedDevice = list.FirstOrDefault((d) => d.Device == settings.DeviceKey);

            await Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                SelectedDevice = selectedDevice;
            });

            hasLoaded = true;
        }

        public string[] InterfaceOptions
        {
            get { return Enum.GetNames(typeof(JlinkInterfaceType)); }
        }

        public int InterfaceSelectedIndex
        {
            get { return interfaceSelectedIndex; }
            set
            {
                interfaceSelectedIndex = value;
                InterfaceType = (JlinkInterfaceType)interfaceSelectedIndex;
            }
        }

        public JlinkInterfaceType InterfaceType
        {
            get { return interfaceType; }
            set
            {
                interfaceType = value;
                Save();
            }
        }

        private void Save()
        {
            if (hasLoaded)
            {
                settings.Interface = (JlinkInterfaceType)interfaceSelectedIndex;
                settings.DeviceKey = selectedDevice?.Device;
                settings.TargetDevice = selectedDevice?.Device.Split(' ')[0].Trim();

                JLinkDebugAdaptor.SetSettings(Model, settings);
                Model.Save();
            }
        }

        private ObservableCollection<JLinkTargetDeviceViewModel> unfilteredList;
        private ObservableCollection<JLinkTargetDeviceViewModel> deviceList;
        public ObservableCollection<JLinkTargetDeviceViewModel> DeviceList
        {
            get { return deviceList; }
            set { this.RaiseAndSetIfChanged(ref deviceList, value); }
        }

        private JLinkTargetDeviceViewModel selectedDevice;
        public JLinkTargetDeviceViewModel SelectedDevice
        {
            get { return selectedDevice; }
            set { this.RaiseAndSetIfChanged(ref selectedDevice, value); Save(); }
        }

        private string filter = string.Empty;

        public string Filter
        {
            get { return filter; }
            set
            {
                this.RaiseAndSetIfChanged(ref filter, value);
                FilterListAsync();
            }
        }

        private async Task FilterListAsync()
        {
            var currentSelected = selectedDevice;

            await Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                SelectedDevice = null;
            });

            if (!string.IsNullOrEmpty(filter))
            {
                ObservableCollection<JLinkTargetDeviceViewModel> newList = null;

                await Task.Factory.StartNew(() =>
                {
                    newList = new ObservableCollection<JLinkTargetDeviceViewModel>(unfilteredList.Where((d) => d.Manufacturer.ToLower().Contains(filter.ToLower()) || d.Core.ToLower().Contains(filter.ToLower()) || d.Device.ToLower().Contains(filter.ToLower())));
                });

                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    DeviceList = newList;
                });
            }
            else
            {
                await Dispatcher.UIThread.InvokeTaskAsync(() =>
                {
                    DeviceList = unfilteredList;
                });
            }

            await Dispatcher.UIThread.InvokeTaskAsync(() =>
            {
                SelectedDevice = currentSelected;
            });
        }
    }
}