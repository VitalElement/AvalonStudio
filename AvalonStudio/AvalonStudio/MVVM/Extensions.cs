using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AvalonStudio.MVVM
{
    public static class Extensions
    {

        //public static System.Drawing.Point ToSystemDrawing (this Point p)
        //{
        //    return new System.Drawing.Point ((int)p.X, (int)p.Y);
        //}

        //public static Point ToWpf (this System.Drawing.Point p)
        //{
        //    return new Point (p.X, p.Y);
        //}

        //public static Size ToWpf (this System.Drawing.Size s)
        //{
        //    return new Size (s.Width, s.Height);
        //}

        //public static Rect ToWpf (this System.Drawing.Rectangle rect)
        //{
        //    return new Rect (rect.Location.ToWpf (), rect.Size.ToWpf ());
        //}
        //#region DPI independence
        //public static Rect TransformToDevice (this Rect rect, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformToDevice;
        //    return Rect.Transform (rect, matrix);
        //}

        //public static Rect TransformFromDevice (this Rect rect, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformFromDevice;
        //    return Rect.Transform (rect, matrix);
        //}

        //public static Size TransformToDevice (this Size size, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformToDevice;
        //    return new Size (size.Width * matrix.M11, size.Height * matrix.M22);
        //}

        //public static Size TransformFromDevice (this Size size, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformFromDevice;
        //    return new Size (size.Width * matrix.M11, size.Height * matrix.M22);
        //}

        //public static Point TransformToDevice (this Point point, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformToDevice;
        //    return new Point (point.X * matrix.M11, point.Y * matrix.M22);
        //}

        //public static Point TransformFromDevice (this Point point, Visual visual)
        //{
        //    Matrix matrix = PresentationSource.FromVisual (visual).CompositionTarget.TransformFromDevice;
        //    return new Point (point.X * matrix.M11, point.Y * matrix.M22);
        //}

        public static T BinarySearch<T, TKey>(this IList<T> list, Func<T, TKey> keySelector, TKey key)
   where TKey : IComparable<TKey>
        {
            int min = 0;
            int max = list.Count;
            while (min < max)
            {
                int mid = min + ((max - min) / 2);
                T midItem = list[mid];
                TKey midKey = keySelector(midItem);
                int comp = midKey.CompareTo(key);
                if (comp < 0)
                {
                    min = mid + 1;
                }
                else if (comp > 0)
                {
                    max = mid - 1;
                }
                else
                {
                    return midItem;
                }
            }

            if (min == max && min < list.Count &&
                keySelector(list[min]).CompareTo(key) == 0)
            {
                return list[min];
            }

            return default(T);
        }

        public static double GetDistance (this Point start, Point point)
        {
            double a2 = Math.Pow (Math.Abs (start.X - point.X), 2);
            double b2 = Math.Pow(Math.Abs (start.Y - point.Y), 2);

            return Math.Sqrt (a2 + b2);
        }
        //#endregion

    }
}
