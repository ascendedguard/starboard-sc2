// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanVisibilityConverter.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Converts a boolean into it's logical Visibility enumeration.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary> Converts a boolean into it's logical Visibility enumeration. </summary>
    public class BooleanVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var val = (bool)value;

            if (parameter != null)
            {
                var reverse = bool.Parse((string)parameter);

                if (reverse)
                {
                    return val ? Visibility.Collapsed : Visibility.Visible;
                }
            }

            return val ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
