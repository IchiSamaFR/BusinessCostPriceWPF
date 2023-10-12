﻿using System.Globalization;
using System.Windows.Data;

namespace CostOfRevenue.Helpers
{
    public class BooleanInvertConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value as bool? == false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
