namespace Starboard.Scoreboard
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Starboard.Model;

    public class RaceTypeConverter : IValueConverter
    {
        private static readonly ImageSource zergIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/ZergLogo.png"));
        private static readonly ImageSource terranIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/TerranEagleLogo.png"));
        private static readonly ImageSource protossIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/ProtossLogo.png"));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var race = (Race)value;

            switch (race)
            {
                case Race.Protoss:
                    return protossIcon;
                case Race.Terran:
                    return terranIcon;
                case Race.Zerg:
                    return zergIcon;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
