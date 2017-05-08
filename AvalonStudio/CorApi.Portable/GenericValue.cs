using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace CorApi.Portable
{
    public partial class GenericValue
    {
        public object GetValue()
        {
            return UnsafeGetValueAsType((CorElementType)this.Type);
        }

        public Object UnsafeGetValueAsType(CorElementType type)
        {
            switch (type)
            {
                case CorElementType.ElementTypeBoolean:
                    byte bValue = 4; // just initialize to avoid compiler warnings
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(byte));
                        this.GetValueInternal(new IntPtr(&bValue));
                    }
                    return (object)(bValue != 0);

                case CorElementType.ElementTypeChar:
                    char cValue = 'a'; // initialize to avoid compiler warnings
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(char));
                        this.GetValueInternal(new IntPtr(&cValue));
                    }
                    return (object)cValue;

                case CorElementType.ElementTypeI1:
                    SByte i1Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(SByte));
                        this.GetValueInternal(new IntPtr(&i1Value));
                    }
                    return (object)i1Value;

                case CorElementType.ElementTypeU1:
                    Byte u1Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Byte));
                        this.GetValueInternal(new IntPtr(&u1Value));
                    }
                    return (object)u1Value;

                case CorElementType.ElementTypeI2:
                    Int16 i2Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Int16));
                        this.GetValueInternal(new IntPtr(&i2Value));
                    }
                    return (object)i2Value;

                case CorElementType.ElementTypeU2:
                    UInt16 u2Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(UInt16));
                        this.GetValueInternal(new IntPtr(&u2Value));
                    }
                    return (object)u2Value;

                case CorElementType.ElementTypeI:
                    IntPtr ipValue = IntPtr.Zero;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(IntPtr));
                        this.GetValueInternal(new IntPtr(&ipValue));
                    }
                    return (object)ipValue;

                case CorElementType.ElementTypeU:
                    UIntPtr uipValue = UIntPtr.Zero;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(UIntPtr));
                        this.GetValueInternal(new IntPtr(&uipValue));
                    }
                    return (object)uipValue;

                case CorElementType.ElementTypeI4:
                    Int32 i4Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Int32));
                        this.GetValueInternal(new IntPtr(&i4Value));
                    }
                    return (object)i4Value;

                case CorElementType.ElementTypeU4:
                    UInt32 u4Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(UInt32));
                        this.GetValueInternal(new IntPtr(&u4Value));
                    }
                    return (object)u4Value;

                case CorElementType.ElementTypeI8:
                    Int64 i8Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Int64));
                        this.GetValueInternal(new IntPtr(&i8Value));
                    }
                    return (object)i8Value;

                case CorElementType.ElementTypeU8:
                    UInt64 u8Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(UInt64));
                        this.GetValueInternal(new IntPtr(&u8Value));
                    }
                    return (object)u8Value;

                case CorElementType.ElementTypeR4:
                    Single r4Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Single));
                        this.GetValueInternal(new IntPtr(&r4Value));
                    }
                    return (object)r4Value;

                case CorElementType.ElementTypeR8:
                    Double r8Value = 4;
                    unsafe
                    {
                        Debug.Assert(this.Size == sizeof(Double));
                        this.GetValueInternal(new IntPtr(&r8Value));
                    }
                    return (object)r8Value;


                case CorElementType.ElementTypeValuetype:
                    byte[] buffer = new byte[this.Size];
                    unsafe
                    {
                        fixed (byte* bufferPtr = &buffer[0])
                        {
                            Debug.Assert(this.Size == buffer.Length);
                            this.GetValueInternal(new IntPtr(bufferPtr));
                        }
                    }
                    return buffer;

                default:
                    Debug.Assert(false, "Generic value should not be of any other type");
                    throw new NotSupportedException();
            }
        }

        private void GetValueInternal(IntPtr valPtr)
        {
            GetValue(valPtr);
        }

        private void SetValueInternal(IntPtr valPtr)
        {
            SetValue(valPtr);
        }

        // Convert the supplied value to the type of this CorGenericValue using System.IConvertable.
        // Then store the value into this CorGenericValue.  Any compatible type can be supplied.
        // For example, if you supply a string and the underlying type is ELEMENT_TYPE_BOOLEAN,
        // Convert.ToBoolean will attempt to match the string against "true" and "false".
        public void SetValue(object value)
        {
            try
            {
                switch ((CorElementType)this.Type)
                {
                    case CorElementType.ElementTypeBoolean:
                        bool v = Convert.ToBoolean(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&v));
                        }
                        break;

                    case CorElementType.ElementTypeI1:
                        SByte sbv = Convert.ToSByte(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&sbv));
                        }
                        break;

                    case CorElementType.ElementTypeU1:
                        Byte bv = Convert.ToByte(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&bv));
                        }
                        break;

                    case CorElementType.ElementTypeChar:
                        Char cv = Convert.ToChar(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&cv));
                        }
                        break;

                    case CorElementType.ElementTypeI2:
                        Int16 i16v = Convert.ToInt16(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&i16v));
                        }
                        break;

                    case CorElementType.ElementTypeU2:
                        UInt16 u16v = Convert.ToUInt16(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&u16v));
                        }
                        break;

                    case CorElementType.ElementTypeI4:
                        Int32 i32v = Convert.ToInt32(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&i32v));
                        }
                        break;

                    case CorElementType.ElementTypeU4:
                        UInt32 u32v = Convert.ToUInt32(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&u32v));
                        }
                        break;

                    case CorElementType.ElementTypeI:
                        Int64 ip64v = Convert.ToInt64(value);
                        IntPtr ipv = new IntPtr(ip64v);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&ipv));
                        }
                        break;

                    case CorElementType.ElementTypeU:
                        UInt64 ipu64v = Convert.ToUInt64(value);
                        UIntPtr uipv = new UIntPtr(ipu64v);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&uipv));
                        }
                        break;

                    case CorElementType.ElementTypeI8:
                        Int64 i64v = Convert.ToInt64(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&i64v));
                        }
                        break;

                    case CorElementType.ElementTypeU8:
                        UInt64 u64v = Convert.ToUInt64(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&u64v));
                        }
                        break;

                    case CorElementType.ElementTypeR4:
                        Single sv = Convert.ToSingle(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&sv));
                        }
                        break;

                    case CorElementType.ElementTypeR8:
                        Double dv = Convert.ToDouble(value);
                        unsafe
                        {
                            SetValueInternal(new IntPtr(&dv));
                        }
                        break;

                    case CorElementType.ElementTypeValuetype:
                        byte[] bav = (byte[])value;
                        unsafe
                        {
                            fixed (byte* bufferPtr = &bav[0])
                            {
                                Debug.Assert(this.Size == bav.Length);
                                SetValue(new IntPtr(bufferPtr));
                            }
                        }
                        break;

                    default:
                        throw new InvalidOperationException("Type passed is not recognized.");
                }
            }
            catch (InvalidCastException e)
            {
                throw new InvalidOperationException("Wrong type used for SetValue command", e);
            }
        }
    }
}
