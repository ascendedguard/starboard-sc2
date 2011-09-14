// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsPanelView.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   View for the settings panel, which has special settings for the application.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System.Reflection;

    /// <summary> Interaction logic for SettingsPanelView.xaml </summary>
    public partial class SettingsPanelView
    {
        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref="SettingsPanelView"/> class. </summary>
        public SettingsPanelView()
        {
            this.InitializeComponent();

            this.txtBuild.Text = string.Format("Build: {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        #endregion
    }
}