// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimedTextControl.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Control to wrap the fields of a TimedText object. Expected to be used in a list, with logic for removing.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System;
    using System.Windows;

    using Starboard.Model;

    /// <summary>
    /// Control to wrap the fields of a TimedText object. Expected to be used in a list, with logic for removing.
    /// </summary>
    public partial class TimedTextControl
    {
        #region Constants and Fields

        /// <summary> TimedText object bound to this control </summary>
        private TimedText timedText;

        #endregion

        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "TimedTextControl" /> class. </summary>
        public TimedTextControl()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Public Events

        /// <summary> Triggered when the X button is clicked, indicating the row should be deleted from it's container </summary>
        public event EventHandler RowDeleted;

        #endregion

        #region Public Properties

        /// <summary> Gets or sets the TimedText object databound to this control </summary>
        public TimedText TimedText
        {
            get
            {
                return this.timedText;
            }

            set
            {
                this.timedText = value;
                this.gridBase.DataContext = value;
            }
        }

        #endregion

        #region Methods

        /// <summary> Handler for the X button, triggering the RowDeleted event. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void RemoveRowClicked(object sender, RoutedEventArgs e)
        {
            var handler = this.RowDeleted;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        #endregion
    }
}