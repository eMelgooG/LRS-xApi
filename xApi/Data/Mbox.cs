using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using xApi.Data.Exceptions;

namespace xApi.Data
{
    [TypeConverter(typeof(MboxTypeConverter))]
    public class Mbox
    {
        const string emailPattern =
                  @"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})";
        private readonly string _mbox = null;
        public Mbox() { }
        public Mbox(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new MboxFormatException("value");
            }

            if (!value.StartsWith("mailto:"))
            {
                throw new MboxFormatException("Must start with 'mailto:'");
            }

            var email = value.Split(new char[] { ':' })[1];
            var match = Regex.Match(email, emailPattern);
            if (!match.Success)
            {
                throw new MboxFormatException($"'{email}' is not a valid e-mail.");
            }

            _mbox = email;
        }

        public override string ToString()
        {
            return _mbox;
        }
        public static bool TryParse(string value, out Mbox mbox)
        {
            mbox = null;
            try
            {
                mbox = new Mbox(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            var mbox = obj as Mbox;
            return mbox != null &&
                   _mbox == mbox._mbox;
        }

        public override int GetHashCode()
        {
            return -1076163258 + EqualityComparer<string>.Default.GetHashCode(_mbox);
        }

        public static bool operator ==(Mbox mbox1, Mbox mbox2)
        {
            return EqualityComparer<Mbox>.Default.Equals(mbox1, mbox2);
        }

        public static bool operator !=(Mbox mbox1, Mbox mbox2)
        {
            return !(mbox1 == mbox2);
        }
    }

    public class MboxTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                if (Mbox.TryParse(value as string, out Mbox mbox))
                {
                    return mbox;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return value.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }

}
