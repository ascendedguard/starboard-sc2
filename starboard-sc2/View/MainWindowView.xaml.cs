// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowView.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Interaction logic for MainWindow.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System.ComponentModel;
    using System.Windows;

    using Starboard.ViewModel;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref = "MainWindowView" /> class. </summary>
        public MainWindowView()
        {
            this.InitializeComponent();

            this.Closing += this.MainWindowClosing;
        }

        #endregion

        #region Methods

        /// <summary> Shuts down the application when the main window closes. </summary>
        /// <param name="sender"> The sender.  </param>
        /// <param name="e"> The event arguments.  </param>
        private void MainWindowClosing(object sender, CancelEventArgs e)
        {
            var vm = (MainWindowViewModel)this.DataContext;

            vm.CloseNetwork();
            vm.SaveSettings();

            Application.Current.Shutdown();
        }

        #endregion

    }
}