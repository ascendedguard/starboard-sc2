// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Settings.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Contains saved settings reloaded at application startup.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard
{
    using System.Globalization;
    using System.Windows;

    using Microsoft.Win32;

    /// <summary> Contains saved settings reloaded at application startup. </summary>
    public class Settings
    {
        /// <summary> Registry path storing all settings. </summary>
        private const string RegistryPath = @"HKEY_CURRENT_USER\SOFTWARE\Ascend\Starboard\";

        /// <summary> Prevents a default instance of the <see cref="Settings"/> class from being created. </summary>
        private Settings()
        {
        }

        /// <summary> Gets or sets the desired width of the scoreboard. </summary>
        public int Size { get; set; }

        /// <summary> Gets or sets the Top-Left position of the scoreboard. </summary>
        public Point Position { get; set; }


        /// <summary> Gets or sets a value indicating whether the scoreboard allows transparency. </summary>
        public bool AllowTransparency { get; set; }

        /// <summary>
        /// Gets or sets the window transparency.
        /// </summary>
        public double WindowTransparency { get; set; }

        /// <summary> Loads the settings from the Registry. </summary>
        /// <returns> Returns the Settings object with all stored settings. </returns>
        public static Settings Load()
        {
            var settings = new Settings();

            var defaultSize = (int)(SystemParameters.PrimaryScreenWidth * .36);
            var sizeValue = Registry.GetValue(RegistryPath, "Size", defaultSize) as string;

            settings.Size = sizeValue != null ? int.Parse(sizeValue) : defaultSize;

            const string DefaultPos = "0x0";

            var pos = Registry.GetValue(RegistryPath, "Position", DefaultPos) as string;
            
            if (pos != null)
            {
                var splitPos = pos.Split('x');
                try
                {
                    settings.Position = new Point(int.Parse(splitPos[0]), int.Parse(splitPos[1]));
                }
                catch
                {
                    // Incase the field is corrupt and the split didn't work:
                    settings.Position = new Point(0, 0);
                }
            }
            else
            {
                settings.Position = new Point(0, 0);
            }

            var allowTrans = Registry.GetValue(RegistryPath, "AllowTransparency", 0);

            if (allowTrans != null)
            {
                var value = (int)allowTrans;
                settings.AllowTransparency = value != 0;
            }
            else
            {
                settings.AllowTransparency = false;
            }

            var winTrans = Registry.GetValue(RegistryPath, "WindowTransparency", 1.ToString(CultureInfo.InvariantCulture)) as string;

            settings.WindowTransparency = winTrans != null ? double.Parse(winTrans) : 1;

            return settings;
        }

        /// <summary> Saves the settings to the Registry </summary>
        public void Save()
        {
            Registry.SetValue(RegistryPath, "Size", this.Size, RegistryValueKind.String);

            var pos = this.Position.X + "x" + this.Position.Y;

            Registry.SetValue(RegistryPath, "Position", pos, RegistryValueKind.String);
            Registry.SetValue(RegistryPath, "AllowTransparency", this.AllowTransparency ? 1 : 0, RegistryValueKind.DWord);
            Registry.SetValue(RegistryPath, "WindowTransparency", this.WindowTransparency.ToString(), RegistryValueKind.String);
        }
    }
}
