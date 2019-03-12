using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio.Controls.Editor.Snippets
{ 
    public static class Extensions
    {
        public static int FindIndex<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }
    }
}
