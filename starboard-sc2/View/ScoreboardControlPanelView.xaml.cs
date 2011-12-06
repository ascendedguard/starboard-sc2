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
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;

    using Starboard.ViewModel;

    using Timer = System.Timers.Timer;

    /// <summary> Interaction logic for ScoreboardControlPanelView.xaml </summary>
    public partial class ScoreboardControlPanelView
    {
        #region Constructors and Destructors

        /// <summary> Initializes a new instance of the <see cref="ScoreboardControlPanelView"/> class. </summary>
        public ScoreboardControlPanelView()
        {
            this.InitializeComponent();

            this.uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();

            if (MainWindowViewModel.IsXSplitInstalled)
            {
                // 20FPS update to XSplit. This should only be created if the COM object exists.
                this.timer = new Timer { Interval = 50, AutoReset = true };
                this.timer.Elapsed += this.XSplitTimerElapsed;
                this.timer.Start();

                this.contentView.PreviewMouseDown += this.contentView_PreviewMouseDown;
                this.contentView.PreviewMouseMove += this.contentView_PreviewMouseMove;
            }
        }

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

        #endregion

        private Timer timer;

        private TaskScheduler uiScheduler;

        private void XSplitTimerElapsed(object sender, ElapsedEventArgs e)
        {
            const int width = 1380;
            const int height = 112;

            if ((MainWindowViewModel.extsrc.ConnectionStatus & 3) == 3)
            {
                Task.Factory.StartNew(
                    () =>
                        {
                            var bmp = new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Default);

                            var elementBrush = new VisualBrush(this.contentView);
                            var visual = new DrawingVisual();
                            var dc = visual.RenderOpen();

                            dc.DrawRectangle(elementBrush, null, new Rect(0, 0, width, height));
                            dc.Close();

                            bmp.Render(visual);

                            var encoder = new BmpBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bmp));

                            using (var stream = new MemoryStream())
                            {
                                encoder.Save(stream);

                                stream.Position = 0;

                                byte[] bytes = stream.ToArray();

                                // hardcoded to show that we will allocate 640x480x4 bytes for 32-bit BGRA data this is also the size of picturebox1
                                int length = width * height * 4;

                                // allocate memory for bitmap transfer to COM
                                IntPtr dataptr = Marshal.AllocCoTaskMem(length);
                                Marshal.Copy(bytes, bytes.Length - length, dataptr, length);
                                MainWindowViewModel.extsrc.SendFrame(width, height, dataptr.ToInt32());
                                    
                                // send to broadcaster
                                Marshal.FreeCoTaskMem(dataptr);
                            }                            
                        },
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    this.uiScheduler);
            }
        }
    }
}