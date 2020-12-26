using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using xApi.Data.Exceptions;

namespace xApi.Data
{
    /// <summary>
    /// ISO 8601 Duration
    /// </summary>
    [TypeConverter(typeof(DurationTypeConverter))]
    public struct Duration
    {
        //private static string _regexPattern = @"^P((\d+(?:\.\d+)?Y)?(\d+(?:\.\d+)?M)?(\d+(?:\.\d+)?D)?(T(?=\d)(\d+(?:\.\d+)?H)?(\d+(?:\.\d+)?M)?(\d+(?:\.\d+)?S)?)?)$|^P(\d+(?:\.\d+)?W)?$";
        private static readonly char[] _dateDesignators = new char[] { 'Y', 'M', 'W', 'D' };
        private static readonly char[] _timeDesignators = new char[] { 'T', 'H', 'M', 'S' };

        public long Ticks;

        public double? Years;
        public double? Months;
        public double? Weeks;
        public double? Days;

        public double? Hours;
        public double? Minutes;
        public double? Seconds;

        public Duration(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new DurationFormatException(nameof(s));
            }

            string strDuration = s.Trim();

            if (!strDuration.StartsWith("P"))
            {
                throw new DurationFormatException("Duration must start with the designator 'P' (for period).");
            }

            if (strDuration.Length == 1)
            {
                throw new DurationFormatException("'P' designator is not valid for duration alone.");
            }

            Years = null;
            Months = null;
            Weeks = null;
            Days = null;
            Hours = null;
            Minutes = null;
            Seconds = null;
            Ticks = 0;

            List<KeyValuePair<char, double>> elements = ParseElements(strDuration);

            AddElements(elements);
        }

        public Duration(long ticks)
        {
            Weeks = null;
            Ticks = ticks;
            var t = TimeSpan.FromTicks(ticks);
            var totalDays = t.TotalDays;
            Years = Math.Truncate(totalDays / 365);
            Months = Math.Truncate((totalDays % 365) / 30);
            Days = Math.Truncate(((totalDays % 365) % 7) % 30);
            Hours = t.Hours;
            Minutes = t.Minutes;
            Seconds = t.Seconds;
        }

        public static Duration Parse(string s)
        {
            return new Duration(s);
        }

        private List<KeyValuePair<char, double>> ParseElements(string s)
        {
            string strValue = "";
            string strDuration = s.Trim();
            char periodOrTime = 'P';
            var elements = new List<KeyValuePair<char, double>>();
            var destinct = new List<string>();
            for (int i = 0; i < strDuration.Length; i++)
            {
                var chr = strDuration[i];

                if (chr == 'P' || chr == 'T')
                {
                    periodOrTime = chr;
                    elements.Add(new KeyValuePair<char, double>(chr, 0.0));
                    continue;
                }

                // We expect digits first
                if (char.IsDigit(chr) || (chr == '.' || chr == ','))
                {
                    strValue += chr;
                    continue;
                }
                else if (char.IsLetter(chr))
                {
                    string combined = "";
                    combined += periodOrTime;
                    combined += chr;
                    if (destinct.Contains(combined))
                    {
                        throw new DurationFormatException($"Duplicate designator '{chr}' at index '{i}'.");
                    }
                    destinct.Add(combined);

                    // Not a valid date or time designator
                    bool validDateDesignator = _dateDesignators.Contains(chr);
                    bool validTimeDesignator = _timeDesignators.Contains(chr);

                    if (!(validDateDesignator || validTimeDesignator))
                    {
                        throw new DurationFormatException($"'{chr}' is not a valid period or time designator.");
                    }

                    if (!validDateDesignator && validDateDesignator && periodOrTime != 'T')
                    {
                        throw new DurationFormatException($"Time designator 'T' must appear before '{chr}'.");
                    }

                    // If we have not received any digits for this designator
                    if (string.IsNullOrEmpty(strValue))
                    {
                        throw new DurationFormatException($"'{chr}' designator must have a value.");
                    }

                    if (double.TryParse(strValue, out double value))
                    {
                        elements.Add(new KeyValuePair<char, double>(chr, value));
                        strValue = null; // Clear value
                    }
                    else
                    {
                        throw new DurationFormatException($"'{strValue}' is not valid for designator '{chr}'.");
                    }
                }
            }

            return elements;
        }

        private void AddElements(List<KeyValuePair<char, double>> elements)
        {
            if (elements == null)
            {
                throw new ArgumentNullException(nameof(elements));
            }

            char periodOrTime = 'P';
            int prevIndexOf = -1;
            for (int i = 0; i < elements.Count(); i++)
            {
                var element = elements[i];

                if (element.Key == 'P' || element.Key == 'T')
                {
                    // Is period or time designator
                    periodOrTime = element.Key;
                    prevIndexOf = -1; // Reset
                    continue;
                }

                if (element.Key == 'W')
                {
                    if (elements.Count() > 2)
                    {
                        throw new DurationFormatException($"'W' is the week designator, and cannot be paired with other designators.");
                    }
                }

                // Ensure the order of the designators
                if (element.Key != 'W')
                {
                    if (periodOrTime == 'P')
                    {
                        int indexOf = Array.IndexOf(_dateDesignators, element.Key);
                        if (prevIndexOf > indexOf)
                        {
                            // Previous designator
                            throw new DurationFormatException($"Date designators must be in the following order 'Y, M, D'.");
                        }
                        prevIndexOf = indexOf;
                    }
                    else if (periodOrTime == 'T')
                    {
                        int indexOf = Array.IndexOf(_timeDesignators, element.Key);
                        if (prevIndexOf > indexOf)
                        {
                            // Previous designator
                            throw new DurationFormatException($"Time designators must be in the following order 'H, M, S'.");
                        }
                        prevIndexOf = indexOf;
                    }
                }

                if (periodOrTime == 'P')
                {
                    AddPeriod(element.Key, element.Value);
                }
                else if (periodOrTime == 'T')
                {
                    AddTime(element.Key, element.Value);
                }
            }
        }

        private void AddPeriod(char designator, double value)
        {
            switch (designator)
            {
                case 'Y':
                    AddYears(value);
                    break;
                case 'M':
                    AddMonths(value);
                    break;
                case 'W':
                    AddWeeks(value);
                    break;
                case 'D':
                    AddDays(value);
                    break;

                default:
                    throw new DurationFormatException($"'{designator}' is not a valid period designator.");
            }
        }

        public void AddYears(double value)
        {
            Years = value;
            Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 365.242199) * value);
        }

        public void AddMonths(double value)
        {
            Months = value;
            Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 30.4368499) * value);
        }

        public void AddWeeks(double value)
        {
            Weeks = value;
            Ticks += (long)Math.Floor((TimeSpan.TicksPerDay * 7) * value);
        }

        public void AddDays(double value)
        {
            Days = value;
            Ticks += (long)Math.Floor(TimeSpan.TicksPerDay * value);
        }

        private void AddTime(char designator, double value)
        {
            switch (designator)
            {
                case 'H':
                    AddHours(value);
                    break;
                case 'M':
                    AddMinutes(value);
                    break;
                case 'S':
                    AddSeconds(value);
                    break;
                default:
                    throw new DurationFormatException($"'{designator}' is not a valid time designator.");
            }
        }

        public void AddHours(double value)
        {
            Hours = value;
            Ticks += (long)Math.Floor(TimeSpan.TicksPerHour * value);
        }

        public void AddMinutes(double value)
        {
            Minutes = value;
            Ticks += (long)Math.Floor(TimeSpan.TicksPerMinute * value);
        }

        public void AddSeconds(double value)
        {
            Seconds = value;
            Ticks += (long)Math.Floor(TimeSpan.TicksPerSecond * value);
        }

        public static Duration FromTicks(long ticks)
        {
            return new Duration(ticks);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("P");
            if (Years.HasValue)
            {
                sb.AppendFormat("{0}Y", Years.Value);
            }
            if (Months.HasValue)
            {
                sb.AppendFormat("{0}M", Months.Value);
            }
            if (Weeks.HasValue)
            {
                sb.AppendFormat("{0}W", Weeks.Value);
            }
            if (Days.HasValue)
            {
                sb.AppendFormat("{0}D", Days.Value);
            }

            if (Hours.HasValue || Minutes.HasValue || Seconds.HasValue)
            {
                sb.Append("T");
                if (Hours.HasValue)
                {
                    sb.AppendFormat("{0}H", Hours.Value);
                }
                if (Minutes.HasValue)
                {
                    sb.AppendFormat("{0}M", Minutes.Value);
                }
                if (Seconds.HasValue)
                {
                    sb.AppendFormat("{0}S", Seconds.Value);
                }
            }

            return sb.ToString();
        }

        public static bool TryParse(string durationString, out Duration duration)
        {
            duration = new Duration();
            try
            {
                duration = new Duration(durationString);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Duration))
            {
                return false;
            }

            var duration = (Duration)obj;
            return Ticks == duration.Ticks &&
                   EqualityComparer<double?>.Default.Equals(Years, duration.Years) &&
                   EqualityComparer<double?>.Default.Equals(Months, duration.Months) &&
                   EqualityComparer<double?>.Default.Equals(Weeks, duration.Weeks) &&
                   EqualityComparer<double?>.Default.Equals(Days, duration.Days) &&
                   EqualityComparer<double?>.Default.Equals(Hours, duration.Hours) &&
                   EqualityComparer<double?>.Default.Equals(Minutes, duration.Minutes) &&
                   EqualityComparer<double?>.Default.Equals(Seconds, duration.Seconds);
        }

        public override int GetHashCode()
        {
            var hashCode = 1706700940;
            hashCode = hashCode * -1521134295 + Ticks.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Years);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Months);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Weeks);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Days);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Hours);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Minutes);
            hashCode = hashCode * -1521134295 + EqualityComparer<double?>.Default.GetHashCode(Seconds);
            return hashCode;
        }

        //public static explicit operator Duration(string s)
        //{
        //    return new Duration(s);
        //}

        //public static explicit operator string(Duration d)
        //{
        //    return d.ToString();
        //}

        //public static explicit operator Duration(long lng)
        //{
        //    return new Duration(lng);
        //}

        //public static explicit operator long(Duration d)
        //{
        //    return d.Ticks;
        //}

        public static bool operator ==(Duration left, Duration right)
        {
            return left.ToString() == right.ToString();
        }

        public static bool operator !=(Duration left, Duration right)
        {
            return !(left == right);
        }

        public static bool operator <(Duration left, Duration right)
        {
            return left.Ticks < right.Ticks;
        }

        public static bool operator >(Duration left, Duration right)
        {
            return left.Ticks > right.Ticks;
        }
    }

    public class DurationTypeConverter : TypeConverter
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
                var duration = new Duration();
                if (Duration.TryParse(value as string, out duration))
                {
                    return duration;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(Duration))
            {
                return true;
            }

            return base.CanConvertTo(context, destinationType);
        }
    }
}