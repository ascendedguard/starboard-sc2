using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Starboard
{
    using Starboard.Model;

    public class PlayerColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = (PlayerColor)value;

            var selectedColor = Colors.Black;

            switch(color)
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

            selectedColor.A = 202;
            return selectedColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
