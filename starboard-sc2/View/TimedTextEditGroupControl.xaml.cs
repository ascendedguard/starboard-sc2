// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimedTextEditGroupControl.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   View for editting TimedText objects.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System;
    using System.Collections.ObjectModel;
    using System.Windows;

    using Starboard.Model;

    /// <summary> Interaction logic for TimedTextEditGroupControl.xaml </summary>
    public partial class TimedTextEditGroupControl
    {
        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref="TimedTextEditGroupControl"/> class. </summary>
        public TimedTextEditGroupControl()
        {
            this.InitializeComponent();

            this.DataContextChanged += this.TimedTextDataContextChanged;
        }

        #endregion

        #region Public Properties

        /// <summary> Gets the view model stored in the current DataContext. </summary>
        public ObservableCollection<TimedText> ViewModel
        {
            get
            {
                return this.DataContext as ObservableCollection<TimedText>;
            }
        }

        #endregion

        #region Methods

        /// <summary> The add text clicked. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void AddTextClicked(object sender, RoutedEventArgs e)
        {
            var text = new TimedText();
            var ctrl = new TimedTextControl { TimedText = text };

            ctrl.RowDeleted += this.RowDeleted;

            this.lstItems.Items.Add(ctrl);
            this.ViewModel.Add(text);
        }

        /// <summary> Handles when a TimedTextControl requests to be removed. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void RowDeleted(object sender, EventArgs e)
        {
            var ctrl = (TimedTextControl)sender;

            this.ViewModel.Remove(ctrl.TimedText);
            this.lstItems.Items.Remove(ctrl);

            ctrl.RowDeleted -= this.RowDeleted;
        }

        /// <summary> The timed text data context changed. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void TimedTextDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = this.ViewModel;

            if (vm == null)
            {
                return;
            }

            foreach (var v in vm)
            {
                var ctrl = new TimedTextControl { TimedText = v };

                ctrl.RowDeleted += this.RowDeleted;

                this.lstItems.Items.Add(ctrl);
            }
        }

        #endregion
    }
}