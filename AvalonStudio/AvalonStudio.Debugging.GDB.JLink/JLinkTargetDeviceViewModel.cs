using AvalonStudio.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace AvalonStudio.Debugging.GDB.JLink
{
    public class JLinkTargetDeviceViewModel : ViewModel
    {
        private string manufacturer;

        public string Manufacturer
        {
            get { return manufacturer; }
            set { this.RaiseAndSetIfChanged(ref manufacturer, value); }
        }

        private string device;

        public string Device
        {
            get { return device; }
            set { this.RaiseAndSetIfChanged(ref device, value); }
        }

        private string core;

        public string Core
        {
            get { return core; }
            set { this.RaiseAndSetIfChanged(ref core, value); }
        }

        private UInt32 flashStart;

        public UInt32 FlashStart
        {
            get { return flashStart; }
            set { this.RaiseAndSetIfChanged(ref flashStart, value); }
        }

        private UInt32 flashLength;

        public UInt32 FlashLength
        {
            get { return flashLength; }
            set { this.RaiseAndSetIfChanged(ref flashLength, value); }
        }

        private UInt32 ramStart;

        public UInt32 RamStart
        {
            get { return ramStart; }
            set { this.RaiseAndSetIfChanged(ref ramStart, value); }
        }

        private UInt32 ramLength;

        public UInt32 RamLength
        {
            get { return ramLength; }
            set { this.RaiseAndSetIfChanged(ref ramLength, value); }
        }

    }
}
