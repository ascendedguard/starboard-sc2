// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerColorConverter.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Defines the PlayerColorConverter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    using Starboard.Model;

    /// <summary> Converts a PlayerColor enumeration into an actual Color for used in overlays. </summary>
    public class PlayerColorConverter : IValueConverter
    {
        /// <summary> Converts a PlayerColor enumeration into an actual color. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. If Brush, a SolidColorBrush is returned. </param>
        /// <param name="parameter"> The parameter, not used. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> The player color as a System.Windows.Media.Color. If a Brush is requested, a SolidColorBrush is returned. </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PlayerColor)value;

            var selectedColor = Colors.Transparent;

            switch (color)
            {
                case PlayerColor.Red:
                    selectedColor = Color.FromRgb(207, 0, 0);
                    break;
                case PlayerColor.Blue:
                    selectedColor = Color.FromRgb(0, 0, 140);
                    break;
                case PlayerColor.Green:
                    selectedColor = Color.FromRgb(0, 140, 0);
                    break;
                case PlayerColor.Orange:
                    selectedColor = Color.FromRgb(192, 132, 0);
                    break;
                case PlayerColor.Purple:
                    selectedColor = Color.FromRgb(132, 0, 192);
                    break;
                case PlayerColor.Teal:
                    selectedColor = Color.FromRgb(0, 202, 209);
                    break;
                case PlayerColor.Yellow:
                    selectedColor = Color.FromRgb(202, 195, 0);
                    break;
                case PlayerColor.LightPink:
                    selectedColor = Color.FromRgb(244, 122, 234);
                    break;
            }

            if (targetType == typeof(Brush))
            {
                return new SolidColorBrush(selectedColor);
            }

            if (selectedColor != Colors.Transparent)
            {
                selectedColor.A = 202;                
            }

            return selectedColor;
        }

        /// <summary> Not implemented. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> Throws a NotImplementedException. </returns>
        /// <exception cref="NotImplementedException"> Always thrown, as the function is never used. </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
