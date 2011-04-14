// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BooleanVisibilityConverter.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Converts a boolean into it's logical Visibility enumeration.
//   The parameter controls whether the returned value should be reversed.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Converters
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    /// <summary> 
    /// Converts a boolean into it's logical Visibility enumeration. 
    /// The parameter controls whether the returned value should be reversed.
    /// </summary>
    public class BooleanVisibilityConverter : IValueConverter
    {
        /// <summary> Converts a boolean to its logical Visibility. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. If true is passed in, the value will be reversed. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> Visible for true, Collapsed for false. This is reversed is the parameter used is true. </returns>
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

        /// <summary> Converts the Visibility back to a boolean. Not Implemented. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> Throws a NotImplementedException. </returns>
        /// <exception cref="NotImplementedException"> Always Thrown. This function is never used. </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
