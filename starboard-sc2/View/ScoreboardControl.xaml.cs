// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControl.xaml.cs" company="Ascend">
//   Copyright 2011
// </copyright>
// <summary>
//   Interaction logic for ScoreboardControl.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System;
    using System.ComponentModel;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Media.Animation;
    using Timer = System.Timers.Timer;

    /// <summary> Interaction logic for ScoreboardControl.xaml </summary>
    public partial class ScoreboardControl
    {
        /// <summary> Index of the subbar binding currently being used. </summary>
        private int index;

        /// <summary> Reference to the timer object. We need to kill the timer when the control is disposed. </summary>
        private Timer currentTimer;

        /// <summary> Initializes a new instance of the <see cref="ScoreboardControl"/> class. </summary>
        public ScoreboardControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
            
            this.currentTimer = new Timer(20000) { AutoReset = true };
            this.currentTimer.AutoReset = false;
            this.currentTimer.Elapsed += this.TimerElapsed;
            this.currentTimer.Start();
        }

        /// <summary> Finalizes an instance of the <see cref="ScoreboardControl"/> class.  </summary>
        ~ScoreboardControl()
        {
            if (this.currentTimer != null)
            {
                this.currentTimer.Stop();
                this.currentTimer.Dispose();
            }
        }

        /// <summary> Gets or sets the viewmodel used for the control </summary>
        public ScoreboardControlViewModel ViewModel
        {
            get
            {
                return (ScoreboardControlViewModel)DataContext;
            }

            set
            {
                this.DataContext = value;
            }
        }

        /// <summary> Changes the matchup text to the bind specified. </summary>
        /// <param name="newBinding"> The path of the new binding. </param>
        private void ChangeMatchupText(string newBinding)
        {
            // Fade text away.
            this.Dispatcher.BeginInvoke((Action)(() => txtStatus.BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, 0.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))))));

            var timer = new Timer(800);
            timer.Elapsed += (sender, e) =>
                {
                    this.Dispatcher.BeginInvoke(
                        (Action)delegate
                            {
                                txtStatus.SetBinding(TextBlock.TextProperty, newBinding);
                                                                                  txtStatus.BeginAnimation(OpacityProperty, new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))));
                            });
                    timer.Dispose();
                };
            timer.Start();
        }

        /// <summary> Increments the subbar text when the given timer elapses, and disposes of the timer object. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            this.currentTimer.Dispose();         

            this.Dispatcher.BeginInvoke((Action)delegate
                                                    {
                                                        this.index += 1;
                                                        this.index %= 3;

                                                        string bindingField;
                                                        int seconds;

                                                        switch (this.index)
                                                        {
                                                            case 0:
                                                                bindingField = "SubbarLine1";
                                                                seconds = this.ViewModel.SubbarTime1;
                                                                break;
                                                            case 1:
                                                                bindingField = "SubbarLine2";
                                                                seconds = this.ViewModel.SubbarTime2;
                                                                break;
                                                            default:
                                                                bindingField = "SubbarLine3";
                                                                seconds = this.ViewModel.SubbarTime3;
                                                                break;
                                                        }

                                                        this.ChangeMatchupText(bindingField);

                                                        currentTimer = new Timer(seconds * 1000) { AutoReset = false };
                                                        currentTimer.Elapsed += this.TimerElapsed;
                                                        currentTimer.Start();
                                                    });
        }
    }
}
