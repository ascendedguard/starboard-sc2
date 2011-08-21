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
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView
    {
        /// <summary> Initializes a new instance of the <see cref="MainWindowView"/> class. </summary>
        public MainWindowView()
        {
            InitializeComponent();

            this.Closing += this.MainWindowClosing;
        }

        /// <summary> Shuts down the application when the main window closes. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = (MainWindowViewModel)this.DataContext;

            vm.CloseNetwork();
            vm.SaveSettings();

            Application.Current.Shutdown();
        }
    }
}
