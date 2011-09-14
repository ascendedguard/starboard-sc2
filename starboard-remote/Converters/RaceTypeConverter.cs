// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaceTypeConverter.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Converts a Race to an appropriate image. If "black" is passed in as a parameter, an appropriate black icon is returned.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Starboard.Model;

    /// <summary> Converts a Race to an appropriate image. If "black" is passed in as a parameter, an appropriate black icon is returned. </summary>
    public class RaceTypeConverter : IValueConverter
    {
        /// <summary> Assembly-embedded location of the white zerg icon. </summary>
        private static readonly ImageSource ZergIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/ZergLogo.png"));

        /// <summary> Assembly-embedded location of the white terran icon. </summary>
        private static readonly ImageSource TerranIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/TerranEagleLogo.png"));

        /// <summary> Assembly-embedded location of the white protoss icon. </summary>
        private static readonly ImageSource ProtossIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/ProtossLogo.png"));

        /// <summary> Assembly-embedded location of the white random icon. </summary>
        private static readonly ImageSource RandomIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/RandomLogo.png"));

        /// <summary> Assembly-embedded location of the black zerg icon. </summary>
        private static readonly ImageSource ZergBlackIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/BlackLogo/BlackZergLogo.png"));

        /// <summary> Assembly-embedded location of the black terran icon. </summary>
        private static readonly ImageSource TerranBlackIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/BlackLogo/BlackTerranEagleLogo.png"));

        /// <summary> Assembly-embedded location of the black protoss icon. </summary>
        private static readonly ImageSource ProtossBlackIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/BlackLogo/BlackProtossLogo.png"));

        /// <summary> Assembly-embedded location of the black random icon. </summary>
        private static readonly ImageSource RandomBlackIcon = new BitmapImage(new Uri("pack://application:,,,/StarboardRemoteControl;component/Images/BlackLogo/BlackRandomLogo.png"));

        /// <summary> Returns an ImageSource for the given Race enumeration. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. If "black", a black ImageSource is returned. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> Returns a white ImageSource of the given race. If "black" is used as a parameter, a black ImageSource is returned. </returns>
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
                            return ProtossBlackIcon;
                        case Race.Terran:
                            return TerranBlackIcon;
                        case Race.Zerg:
                            return ZergBlackIcon;
                        case Race.Random:
                            return RandomBlackIcon;
                    }
                }
            }

            switch (race)
            {
                case Race.Protoss:
                    return ProtossIcon;
                case Race.Terran:
                    return TerranIcon;
                case Race.Zerg:
                    return ZergIcon;
                case Race.Random:
                    return RandomIcon;
            }

            return null;
        }

        /// <summary> Not Implemented. </summary>
        /// <param name="value"> The value. </param>
        /// <param name="targetType"> The target type. </param>
        /// <param name="parameter"> The parameter. </param>
        /// <param name="culture"> The culture. </param>
        /// <returns> Throws a NotImplementedException </returns>
        /// <exception cref="NotImplementedException"> Always thrown. This function is not used. </exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
