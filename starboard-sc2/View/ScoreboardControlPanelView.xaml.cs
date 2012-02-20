// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControlPanelView.xaml.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   View for the scoreboard control panel, which has all the main controls for the scoreboard.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.View
{
    using System;
    using System.Collections.Specialized;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using XSplit.Wpf;

    /// <summary> Interaction logic for ScoreboardControlPanelView.xaml </summary>
    public partial class ScoreboardControlPanelView
    {
        #region Constructors and Destructors

        /// <summary>
        /// Reference to the timer for updating the XSplit output.
        /// </summary>
        private readonly TimedBroadcasterPlugin plugin;

        /// <summary> Initializes a new instance of the <see cref="ScoreboardControlPanelView"/> class. </summary>
        public ScoreboardControlPanelView()
        {
            this.InitializeComponent();

            this.plugin = TimedBroadcasterPlugin.CreateInstance(
                "BCB458E4-13D9-11E1-BF80-790C4824019B", this.contentView, 1380, 112, 50);

            if (this.plugin != null)
            {
                this.plugin.StartTimer();
                this.contentView.PreviewMouseDown += this.contentView_PreviewMouseDown;
                this.contentView.PreviewMouseMove += this.contentView_PreviewMouseMove;
            }
        }

        #endregion

        #region XBS (XSplit) Dragging Support

        private Point startPoint;

        private void contentView_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.startPoint = e.GetPosition(null);
        }

        private void contentView_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            var senderObj = sender as ScoreboardControl;

            if (senderObj == null)
            {
                // This shouldn't happen.
                return;
            }

            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = this.startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed && 
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "starboard.xbs");

               if (File.Exists(path) == false)
               {
                   return;
               }

                var strCol = new StringCollection { path };

                var o = new DataObject(DataFormats.FileDrop, strCol);
                o.SetFileDropList(strCol);
                DragDrop.DoDragDrop(senderObj, o, DragDropEffects.Copy);
            }
        }

        #endregion
    }
}