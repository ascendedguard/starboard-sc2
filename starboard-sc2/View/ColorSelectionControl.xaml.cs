// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColorSelectionControl.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Control for providing 1-click control over a player's color. Provides databinding through the SelectedColor property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Starboard.Model;

    /// <summary> Control for providing 1-click control over a player's color. Provides databinding through the SelectedColor property. </summary>
    public partial class ColorSelectionControl
    {
        #region Constants and Fields

        /// <summary> DependencyProperty for the SelectedColor property. </summary>
        public static readonly DependencyProperty SelectedColorProperty = DependencyProperty.Register(
            "SelectedColor", 
            typeof(PlayerColor), 
            typeof(ColorSelectionControl), 
            new FrameworkPropertyMetadata(PlayerColor.Unknown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ColorChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "ColorSelectionControl" /> class. </summary>
        public ColorSelectionControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary> Gets or sets the selected player color. </summary>
        public PlayerColor SelectedColor
        {
            get
            {
                return (PlayerColor)this.GetValue(SelectedColorProperty);
            }

            set
            {
                this.SetValue(SelectedColorProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary> Checks the appropriate button when the SelectedColor property is changed externally. </summary>
        /// <param name="d"> The object to apply the DependencyProperty change to.  </param>
        /// <param name="e"> The event arguments, containing the new property value.  </param>
        private static void ColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (ColorSelectionControl)d;
            var color = (PlayerColor)e.NewValue;

            foreach (RadioButton control in userControl.gridBase.Children)
            {
                if ((PlayerColor)control.Content == color)
                {
                    control.IsChecked = true;
                    break;
                }
            }
        }

        /// <summary> Sets the SelectedColor property when a button has been pressed. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void ColorClicked(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedColor = (PlayerColor)btn.Content;
        }

        #endregion
    }
}