namespace AvalonStudio.Debugging
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using ReactiveUI;
    using MVVM;
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

        private string GetText (byte[] data)
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

        public override async Task InvalidateAsync (IDebugger debugger)
        {
            foreach(var value in lastChangedValues)
            {
                value.HasChanged = false;
            }

            lastChangedValues.Clear();

            List<MemoryBytes> newData = null;

            newData = await debugger.ReadMemoryBytesAsync(actualAddress, 0, count);

            if (newData != null)
            {
                var newValues = converter(newData.First().Data);

                for (int i = 0; i < values.Count; i++)
                {
                    if (!Values[i].Value.Equals(newValues[i]))
                    {
                        Values[i].Value = newValues[i];
                        Values[i].HasChanged = true;
                        lastChangedValues.Add(Values[i]);
                    }
                }

                Text = GetText(newData.First().Data);
            }
        }


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
        public abstract Task InvalidateAsync(IDebugger debugger);
    }
}
