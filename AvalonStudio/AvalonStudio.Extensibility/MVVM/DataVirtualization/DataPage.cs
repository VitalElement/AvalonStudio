namespace AvalonStudio.MVVM.DataVirtualization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataPage<T> where T : class
    {
        public DataPage(int firstIndex, int pageLength)
        {
            this.Items = new List<DataWrapper<T>>(pageLength);
            for (int i = 0; i < pageLength; i++)
            {
                this.Items.Add(new DataWrapper<T>(firstIndex + i));
            }
            this.TouchTime = DateTime.Now;
        }

        public IList<DataWrapper<T>> Items { get; set; }

        public DateTime TouchTime { get; set; }

        public bool GarbageCollect { get; set; }

        public bool IsInUse
        {
            get { return this.Items.Any(wrapper => wrapper.IsInUse); }
        }

        public void Populate(IList<T> newItems)
        {
            int i;
            int index = 0;
            for (i = 0; i < newItems.Count && i < this.Items.Count; i++)
            {
                this.Items[i].Data = newItems[i];
                index = this.Items[i].Index;
            }

            while (i < newItems.Count)
            {
                index++;
                this.Items.Add(new DataWrapper<T>(index) { Data = newItems[i] });
                i++;
            }

            while (i < this.Items.Count)
            {
                this.Items.RemoveAt(this.Items.Count - 1);
            }
        }
    }
}
