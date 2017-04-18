namespace AvalonStudio.Debugging
{
    using AvalonStudio.MVVM.DataVirtualization;
    using MVVM;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Threading.Tasks;

    public class MemoryViewDataProvider : ViewModel, IItemsProvider<MemoryBytesViewModel>
    {
        public enum IntegerSize
        {
            [Description("No Data")]
            NoData = 0,

            [Description("1-byte Integer")]
            U8 = 1,

            [Description("2-byte Integer")]
            U16 = 2,

            [Description("4-byte Integer")]
            U32 = 4,

            [Description("8-byte Integer")]
            U64 = 8
        }

        private IntegerSize integerDisplaySize = IntegerSize.U8;

        public IntegerSize IntegerDisplaySize
        {
            get { return integerDisplaySize; }
            set { this.RaiseAndSetIfChanged(ref integerDisplaySize, value); Count = Count; }
        }

        public MemoryViewDataProvider(int columns)
        {
            this.columns = columns;
            Enable();
        }

        public void SetDebugger(IDebugger2 debugger)
        {
            this.debugger = debugger;
        }

        public void SetRowWidth(int width)
        {
            columns = width;
        }

        private IDebugger2 debugger;
        private int columns;

        private int count;

        public int Count
        {
            get { return count; }
            set { this.RaiseAndSetIfChanged(ref count, value); }
        }

        public void Enable()
        {
            Count = (int)(UInt32.MaxValue / columns) + 1;
        }

        public void Clear()
        {
            Count = 0;
        }

        private MemoryBytesViewModel GenerateViewModels(MemoryBytes data)
        {
            switch (IntegerDisplaySize)
            {
                case IntegerSize.NoData:
                    return new MemoryBytesViewModel<byte>(data, string.Empty, (source) => { return source; });

                case IntegerSize.U8:
                    return new MemoryBytesViewModel<byte>(data, "{0:X2}", (source) => { return source; });

                case IntegerSize.U16:
                    return new MemoryBytesViewModel<UInt16>(data, "{0:X4}", (source) =>
                    {
                        var values = new List<UInt16>();

                        for (int i = 0; i < source.Length; i += 2)
                        {
                            values.Add(BitConverter.ToUInt16(source, i));
                        }

                        return values;
                    });

                case IntegerSize.U32:
                    return new MemoryBytesViewModel<UInt32>(data, "{0:X8}", (source) =>
                    {
                        var values = new List<UInt32>();

                        for (int i = 0; i < source.Length; i += 4)
                        {
                            values.Add(BitConverter.ToUInt32(source, i));
                        }

                        return values;
                    });

                case IntegerSize.U64:
                    return new MemoryBytesViewModel<UInt64>(data, "{0:X16}", (source) =>
                    {
                        var values = new List<UInt64>();

                        for (int i = 0; i < source.Length; i += 8)
                        {
                            values.Add(BitConverter.ToUInt64(source, i));
                        }

                        return values;
                    });

                default:
                    throw new NotImplementedException();
            }
        }

        private async Task<List<MemoryBytes>> ReadMemoryBytesAsync(ulong address, ulong offset, uint count)
        {
            /*var result = await debugger.ReadMemoryBytesAsync(address, offset, count);

            if(result == null)
            {
                result = new List<MemoryBytes>();

                result.Add(new MemoryBytes() { Address = address, Data = new byte[count] });
            }

            return result;*/
            throw new NotImplementedException();
        }

        private async Task<List<MemoryBytesViewModel>> FetchRange(int startIndex, int pageCount)
        {
            List<MemoryBytesViewModel> result = null;

            result = new List<MemoryBytesViewModel>();

            for (int i = 0; i < pageCount; i++)
            {
                ulong address = ((ulong)startIndex * (ulong)columns) + (ulong)i * (ulong)columns;

                var data = await ReadMemoryBytesAsync(address, 0, (uint)columns);

                result.Add(GenerateViewModels(data.First()));
            }

            return result;
        }

        public IList<MemoryBytesViewModel> FetchRange(int startIndex, int pageCount, out int overallCount)
        {
            overallCount = Count;

            List<MemoryBytesViewModel> result = null;

            if (debugger != null)
            {
                var task = FetchRange(startIndex, pageCount);

                task.Wait();

                result = task.Result;
            }

            return result;
        }
    }
}