using System;
using System.Collections.Generic;
using System.Linq;

namespace AvalonStudio.MVVM.DataVirtualization
{
    public class DataPage<T> where T : class
    {
        public DataPage(int firstIndex, int pageLength)
        {
            Items = new List<DataWrapper<T>>(pageLength);
            for (var i = 0; i < pageLength; i++)
            {
                Items.Add(new DataWrapper<T>(firstIndex + i));
            }
            TouchTime = DateTime.Now;
        }

        public IList<DataWrapper<T>> Items { get; set; }

        public DateTime TouchTime { get; set; }

        public bool GarbageCollect { get; set; }

        public bool IsInUse
        {
            get { return Items.Any(wrapper => wrapper.IsInUse); }
        }

        public void Populate(IList<T> newItems)
        {
            int i;
            var index = 0;
            for (i = 0; i < newItems.Count && i < Items.Count; i++)
            {
                Items[i].Data = newItems[i];
                index = Items[i].Index;
            }

            while (i < newItems.Count)
            {
                index++;
                Items.Add(new DataWrapper<T>(index) { Data = newItems[i] });
                i++;
            }

            while (i < Items.Count)
            {
                Items.RemoveAt(Items.Count - 1);
            }
        }
    }
}