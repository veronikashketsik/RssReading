using System;
using System.ComponentModel;
using System.Globalization;

namespace RssReading.Infrastructure
{
    public class SortingConverter : TypeConverter
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
                Sorting sort;
                if (Sorting.TryParse((string)value, out sort))
                {
                    return sort;
                }
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}