namespace AvalonStudio.Debugging
{
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class MemoryBytesViewModel<T> : MemoryBytesViewModel
    {
        public MemoryBytesViewModel(MemoryBytes model, string formatString, Func<byte[], IList<T>> converter)
        {
            lastChangedValues = new List<MemoryValueViewModel<T>>();

            this.values = new ObservableCollection<MemoryValueViewModel<T>>();

            this.converter = converter;

            this.text = GetText(model.Data);

            this.actualAddress = model.Address;
            this.address = string.Format("0x{0:X8}", model.Address);
            ulong currentAddress = model.Address;

            ulong typeSize = (ulong)System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));

            var values = converter(model.Data);

            foreach (var value in values)
            {
                this.values.Add(new MemoryValueViewModel<T>(currentAddress, value, formatString));
                currentAddress += typeSize;
            }

            count = (uint)(values.Count * (int)typeSize);
        }

        private string GetText(byte[] data)
        {
            string result = string.Empty;
            // var text = Encoding.ASCII.GetString(data);

            foreach (byte character in data)
            {
                if (char.IsControl((char)character))
                {
                    result += ".";
                }
                else
                {
                    result += (char)character;
                }
            }

            return result;
        }

        public override Task InvalidateAsync(IDebugger2 debugger)
        {
            throw new NotImplementedException();
        }

        private uint count;
        private Func<byte[], IList<T>> converter;

        private ulong actualAddress;
        private string address;

        public string Address
        {
            get { return address; }
            set { this.RaiseAndSetIfChanged(ref address, value); }
        }

        private List<MemoryValueViewModel<T>> lastChangedValues;

        private ObservableCollection<MemoryValueViewModel<T>> values;

        public ObservableCollection<MemoryValueViewModel<T>> Values
        {
            get { return values; }
            set { this.RaiseAndSetIfChanged(ref values, value); }
        }

        private string text;

        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref text, value); }
        }
    }

    public abstract class MemoryBytesViewModel : ViewModel
    {
        public abstract Task InvalidateAsync(IDebugger2 debugger);
    }
}