// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RaceSelectionControl.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Control for providing 1-click control over a player's race. Allows databinding to the selected
//   value through the SelectedRace property.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System.Windows;
    using System.Windows.Controls;

    using Starboard.Model;

    /// <summary>
    /// Control for providing 1-click control over a player's race. Allows databinding to the selected
    ///   value through the SelectedRace property.
    /// </summary>
    public partial class RaceSelectionControl
    {
        #region Constants and Fields

        /// <summary> DependencyProperty for the SelectedRace property. </summary>
        public static readonly DependencyProperty SelectedRaceProperty = DependencyProperty.Register(
            "SelectedRace", 
            typeof(Race), 
            typeof(RaceSelectionControl), 
            new FrameworkPropertyMetadata(Race.Unknown, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RaceChanged));

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "RaceSelectionControl" /> class. </summary>
        public RaceSelectionControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Properties

        /// <summary> Gets or sets the race that has been selected. </summary>
        public Race SelectedRace
        {
            get
            {
                return (Race)this.GetValue(SelectedRaceProperty);
            }

            set
            {
                this.SetValue(SelectedRaceProperty, value);
            }
        }

        #endregion

        #region Methods

        /// <summary> Checks the appropriate control button when the SelectedRace property changes externally. </summary>
        /// <param name="d"> The control affected by this change.  </param>
        /// <param name="e"> The event arguments, containing the new property value.  </param>
        private static void RaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var userControl = (RaceSelectionControl)d;
            var race = (Race)e.NewValue;

            foreach (RadioButton control in userControl.gridBase.Children)
            {
                if ((Race)control.Content == race)
                {
                    control.IsChecked = true;
                    break;
                }
            }
        }

        /// <summary> Sets the clicked race to the SelectedRace property, based on the button's content. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void RaceButtonClicked(object sender, RoutedEventArgs e)
        {
            var btn = (RadioButton)sender;

            this.SelectedRace = (Race)btn.Content;
        }

        #endregion
    }
}