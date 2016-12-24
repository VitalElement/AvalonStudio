using System;
using Avalonia;
using System.Reflection;
using System.ComponentModel;

namespace AvalonStudio.MVVM
{
	public static class Extensions
	{
        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }
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


        public static double GetDistance(this Point start, Point point)
		{
			var a2 = Math.Pow(Math.Abs(start.X - point.X), 2);
			var b2 = Math.Pow(Math.Abs(start.Y - point.Y), 2);

			return Math.Sqrt(a2 + b2);
		}

		//#endregion
	}
}