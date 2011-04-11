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
        // White logos
        private static readonly ImageSource zergIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/ZergLogo.png"));
        private static readonly ImageSource terranIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/TerranEagleLogo.png"));
        private static readonly ImageSource protossIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/ProtossLogo.png"));
        private static readonly ImageSource randomIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/RandomLogo.png"));

        // Black logos
        private static readonly ImageSource zergBlackIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/BlackLogo/BlackZergLogo.png"));
        private static readonly ImageSource terranBlackIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/BlackLogo/BlackTerranEagleLogo.png"));
        private static readonly ImageSource protossBlackIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/BlackLogo/BlackProtossLogo.png"));
        private static readonly ImageSource randomBlackIcon = new BitmapImage(new Uri("pack://application:,,,/starboard-sc2;component/Images/BlackLogo/BlackRandomLogo.png"));


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var race = (Race)value;

            if (parameter != null)
            {
                if (parameter.ToString().Equals("black"))
                {
                    switch (race)
                    {
                        case Race.Protoss:
                            return protossBlackIcon;
                        case Race.Terran:
                            return terranBlackIcon;
                        case Race.Zerg:
                            return zergBlackIcon;
                        case Race.Random:
                            return randomBlackIcon;
                    }
                }
            }

            switch (race)
            {
                case Race.Protoss:
                    return protossIcon;
                case Race.Terran:
                    return terranIcon;
                case Race.Zerg:
                    return zergIcon;
                case Race.Random:
                    return randomIcon;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
