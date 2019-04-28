using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;

namespace NetManager.Client.Design
{
    /// <summary>
    /// Конвертер для свойства типа IPAddress
    /// </summary>
    class IPAddressConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || sourceType == typeof(long) || sourceType == typeof(IPAddress) ||
                base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(string) || destinationType == typeof(long) || destinationType == typeof(IPAddress) ||
                base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                return IPAddress.Parse(value.ToString().Trim());
            }
            else
            {
                if (value.GetType() == typeof(long))
                {
                    return new IPAddress((long)value);
                }
                else
                {
                    if (value.GetType() == typeof(byte[]))
                    {
                        return new IPAddress((byte[])value);
                    }
                    else
                    {
                        return base.ConvertFrom(context, culture, value);
                    }
                }
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value.ToString();
            }
            else
            {
                if (destinationType == typeof(int))
                {
                    return BitConverter.ToInt64((value as IPAddress).GetAddressBytes(), 0);
                }
                else
                {
                    if (destinationType == typeof(byte[]))
                    {
                        return (value as IPAddress).GetAddressBytes();
                    }
                    else
                    {
                        return base.ConvertTo(context, culture, value, destinationType);
                    }
                }
            }
        }
    }
}
