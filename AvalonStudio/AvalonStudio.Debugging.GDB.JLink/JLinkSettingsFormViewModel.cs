using Avalonia.Threading;
using AvalonStudio.MVVM;
using AvalonStudio.Packaging;
using AvalonStudio.Projects;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public class JLinkSettingsFormViewModel : ViewModel<IProject>
    {
        private List<string> _versions;
        private int interfaceSelectedIndex;
        private int speedSelectedIndex;
        private string filter = string.Empty;
        private string _selectedVersion;
        private bool _download;
        private bool _postDownloadReset;
        private bool _useRemote;
        private bool _reset;
        private bool _run;

        private string speed;
        private JlinkInterfaceType interfaceType;
        private readonly JLinkSettings settings;

        public JLinkSettingsFormViewModel(IProject model) : base(model)
        {
            settings = model.GetDebuggerSettings<JLinkSettings>();

            interfaceSelectedIndex = (int)settings.Interface;
            interfaceType = settings.Interface;
            _download = settings.Download;
            _reset = settings.Reset;
            _useRemote = settings.UseRemote;
            _ipAddress = settings.RemoteIPAddress;
            _postDownloadReset = settings.PostDownloadReset;
            _run = settings.Run;
            _selectedVersion = settings.Version;

            speedSelectedIndex = SpeedOptions.IndexOf(settings.SpeedkHz.ToString());

            speed = settings.SpeedkHz.ToString();

            string devPath = Path.Combine(JLinkDebugger.GetBaseDirectory(settings.Version), "devices.csv");

            deviceList = new ObservableCollection<JLinkTargetDeviceViewModel>();

            LoadDeviceList(devPath);
        }

        private bool hasLoaded = false;

        private async void LoadDeviceList(string deviceFile)
        {
            var list = new ObservableCollection<JLinkTargetDeviceViewModel>();

            var versions = await PackageManager.ListToolchainPackages("JLink");

            if(versions != null)
            {
                Versions = new List<string>(versions.Select(v => v.Version.ToString()));
            }

            if(File.Exists(deviceFile))
            {
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

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    SelectedDevice = selectedDevice;
                });
            }

            hasLoaded = true;
        }

        public string[] InterfaceOptions
        {
            get { return Enum.GetNames(typeof(JlinkInterfaceType)); }
        }

        public List<string> SpeedOptions
        {
            get
            {
                return new List<string>
                {
                    "5",
                    "10",
                    "20",
                    "30",
                    "50",
                    "100",
                    "200",
                    "300",
                    "400",
                    "500",
                    "600",
                    "750",
                    "900",
                    "1000",
                    "1334",
                    "1600",
                    "2000",
                    "2667",
                    "3200",
                    "4000",
                    "4800",
                    "5334",
                    "6000",
                    "8000",
                    "9600",
                    "12000"
                };
            }
        }

        public int SpeedSelectedIndex
        {
            get
            {
                return speedSelectedIndex;
            }
            set
            {
                speedSelectedIndex = value;

                if (value >= 0)
                {
                    Speed = SpeedOptions[value];
                }
            }
        }

        public int InterfaceSelectedIndex
        {
            get
            {
                return interfaceSelectedIndex;
            }
            set
            {
                interfaceSelectedIndex = value;
                InterfaceType = (JlinkInterfaceType)interfaceSelectedIndex;
            }
        }

        public string Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                Save();
            }
        }

        public JlinkInterfaceType InterfaceType
        {
            get
            {
                return interfaceType;
            }
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

                if(selectedDevice != null)
                {
                    settings.DeviceKey = selectedDevice?.Device;
                    settings.TargetDevice = selectedDevice?.Device.Split(' ')[0].Trim();
                }

                settings.PostDownloadReset = _postDownloadReset;
                settings.Download = _download;
                settings.Reset = _reset;
                settings.UseRemote = _useRemote;
                settings.RemoteIPAddress = _ipAddress;
                settings.Run = _run;
                settings.Version = _selectedVersion;

                if (!string.IsNullOrEmpty(speed))
                {
                    settings.SpeedkHz = Convert.ToInt32(speed);
                }
                else
                {
                    settings.SpeedkHz = 12000;
                }

                Model.SetDebuggerSettings(settings);
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

        public List<string> Versions
        {
            get { return _versions; }
            set { this.RaiseAndSetIfChanged(ref _versions, value); }
        }

        public string SelectedVersion
        {
            get => _selectedVersion;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedVersion, value);

                Save();
            }
        }

        private JLinkTargetDeviceViewModel selectedDevice;

        public JLinkTargetDeviceViewModel SelectedDevice
        {
            get
            {
                return selectedDevice;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref selectedDevice, value);
                Save();
            }
        }
        
        public string Filter
        {
            get
            {
                return filter;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref filter, value);
                Task.Run(FilterListAsync);
            }
        }

        public bool Download
        {
            get
            {
                return _download;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _download, value);
                Save();
            }
        }

        public bool PostDownloadReset
        {
            get { return _postDownloadReset; }
            set
            {
                this.RaiseAndSetIfChanged(ref _postDownloadReset, value);
                Save();
            }
        }

        public bool Run
        {
            get { return _run; }
            set 
            { 
                this.RaiseAndSetIfChanged(ref _run, value);
                Save();
            }
        }

        public bool UseRemote
        {
            get { return _useRemote; }
            set
            {
                this.RaiseAndSetIfChanged(ref _useRemote, value);
                Save();
            }
        }

        private string _ipAddress;

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                this.RaiseAndSetIfChanged(ref _ipAddress, value);
                Save();
            }
        }

        public bool Reset
        {
            get
            {
                return _reset;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _reset, value);
                Save();
            }
        }

        private async Task FilterListAsync()
        {
            var currentSelected = selectedDevice;

            await Dispatcher.UIThread.InvokeAsync(() =>
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

                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    DeviceList = newList;
                });
            }
            else
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    DeviceList = unfilteredList;
                });
            }

            await Dispatcher.UIThread.InvokeAsync(() =>
            {
                SelectedDevice = currentSelected;
            });
        }
    }
}