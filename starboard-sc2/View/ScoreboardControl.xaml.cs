// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScoreboardControl.xaml.cs" company="Starboard">
//   Copyright 2011
// </copyright>
// <summary>
//   Interaction logic for ScoreboardControl.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Scoreboard
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Media;
    using System.Windows.Media.Animation;

    using Starboard.Model;

    using Timer = System.Timers.Timer;

    /// <summary> Interaction logic for ScoreboardControl.xaml </summary>
    public partial class ScoreboardControl
    {
        private int previousSubbarIndex;
        private int previousAnnouncementIndex;

        /// <summary> Reference to the timer object. We need to kill the timer when the control is disposed. </summary>
        private Timer currentSubbarTimer;
        private Timer currentAnnouncementTimer;

        /// <summary> Initializes a new instance of the <see cref="ScoreboardControl"/> class. </summary>
        public ScoreboardControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                return;
            }
        }

        /// <summary> Finalizes an instance of the <see cref="ScoreboardControl"/> class.  </summary>
        ~ScoreboardControl()
        {
            if (this.currentSubbarTimer != null)
            {
                this.currentSubbarTimer.Stop();
                this.currentSubbarTimer.Dispose();
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
                // Unbind from old datacontext.
                var oldContext = this.DataContext as ScoreboardControlViewModel;
                if (oldContext != null)
                {
                    oldContext.Player1.ColorChanged -= this.Player1ColorChanged;
                    oldContext.Player2.ColorChanged -= this.Player2ColorChanged;                    
                }

                this.DataContext = value;

                value.Player1.ColorChanged += this.Player1ColorChanged;
                value.Player2.ColorChanged += this.Player2ColorChanged;

                this.previousSubbarIndex = 0;
                value.SubbarText.CollectionChanged += this.SubbarText_CollectionChanged;

                this.previousAnnouncementIndex = 0;
                value.AnnouncementText.CollectionChanged += this.AnnouncementText_CollectionChanged;
            }
        }

        private void AnnouncementText_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var s = (ObservableCollection<TimedText>)sender;

            if (s.Count == 0)
            {
                if (this.currentAnnouncementTimer != null)
                {
                    this.currentAnnouncementTimer.Stop();
                    this.currentAnnouncementTimer.Dispose();
                }

                this.ChangeAnnouncementText(null);
                this.ViewModel.IsAnnouncementShowing = false;
            }
            else if (s.Count == 1)
            {
                if (this.currentAnnouncementTimer != null)
                {
                    this.currentAnnouncementTimer.Stop();
                    this.currentAnnouncementTimer.Dispose();
                }

                this.ChangeAnnouncementText(new Binding("Text") { Source = s[0] });
            }
            else if (s.Count > 1)
            {
                if (this.currentAnnouncementTimer == null)
                {
                    var el = s[0];

                    this.currentAnnouncementTimer = new Timer(el.Time * 1000) { AutoReset = false };
                    this.currentAnnouncementTimer.Elapsed += this.AnnouncementTimerElapsed;
                    this.currentAnnouncementTimer.Start();
                }

                // Else the timer will take care of the updating.
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex == this.previousAnnouncementIndex)
                {
                    if (this.currentAnnouncementTimer != null)
                    {
                        this.currentAnnouncementTimer.Stop();
                        this.currentAnnouncementTimer.Dispose();                     
                    }

                    // Go back 1 so we get the next element.
                    this.previousAnnouncementIndex--;

                    this.AnnouncementTimerElapsed(sender, EventArgs.Empty);
                }
            }
        }

        private void SubbarText_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var s = (ObservableCollection<TimedText>)sender;

            if (s.Count == 0)
            {
                if (this.currentSubbarTimer != null)
                {
                    this.currentSubbarTimer.Stop();
                    this.currentSubbarTimer.Dispose();
                }

                this.ChangeMatchupText(null);
                this.ViewModel.IsSubbarShowing = false;
            }
            else if (s.Count == 1)
            {
                if (this.currentSubbarTimer != null)
                {
                    this.currentSubbarTimer.Stop();
                    this.currentSubbarTimer.Dispose();
                }

                this.ViewModel.IsSubbarShowing = true;
                this.ChangeMatchupText(new Binding("Text") { Source = s[0] });
            }
            else if (s.Count > 1)
            {
                if (this.currentSubbarTimer == null)
                {
                    var el = s[0];

                    this.currentSubbarTimer = new Timer(el.Time * 1000) { AutoReset = false };
                    this.currentSubbarTimer.Elapsed += this.SubbarTimerElapsed;
                    this.currentSubbarTimer.Start();
                }

                // Else the timer will take care of the updating.
            }

            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldStartingIndex == this.previousSubbarIndex)
                {
                    if (this.currentAnnouncementTimer != null)
                    {
                        this.currentSubbarTimer.Stop();
                        this.currentSubbarTimer.Dispose();                        
                    }

                    // Go back 1 so we get the next element.
                    this.previousSubbarIndex--;

                    this.SubbarTimerElapsed(sender, EventArgs.Empty);
                }
            }
        }

        private void Player2ColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var converter = new PlayerColorConverter();
            var color = (Color)converter.Convert(e.NewValue, typeof(Color), null, null);
            var anim = new ColorAnimation(color, new TimeSpan(0, 0, 0, 0, 500));
            player2Color.BeginAnimation(GradientStop.ColorProperty, anim);
        }

        private void Player1ColorChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var converter = new PlayerColorConverter();
            var color = (Color)converter.Convert(e.NewValue, typeof(Color), null, null);
            var anim = new ColorAnimation(color, new TimeSpan(0, 0, 0, 0, 500));
            player1Color.BeginAnimation(GradientStop.ColorProperty, anim);
        }

        private void ChangeAnnouncementText(BindingBase newBinding)
        {
            // Fade text away.
            this.Dispatcher.BeginInvoke((Action)(() => txtAnnouncement.BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, 0.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))))));

            var timer = new Timer(800);
            timer.Elapsed += (sender, e) =>
            {
                this.Dispatcher.BeginInvoke(
                    (Action)delegate
                    {
                        if (newBinding == null)
                        {
                            txtAnnouncement.ClearValue(TextBlock.TextProperty);
                        }
                        else
                        {
                            txtAnnouncement.SetBinding(TextBlock.TextProperty, newBinding);
                        }

                        txtAnnouncement.BeginAnimation(OpacityProperty, new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))));
                    });
                timer.Dispose();
            };
            timer.Start();
        }

        /// <summary> Changes the matchup text to the bind specified. </summary>
        /// <param name="newBinding"> The path of the new binding. </param>
        private void ChangeMatchupText(BindingBase newBinding)
        {
            // Fade text away.
            this.Dispatcher.BeginInvoke((Action)(() => txtStatus.BeginAnimation(OpacityProperty, new DoubleAnimation(1.0, 0.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))))));

            var timer = new Timer(800);
            timer.Elapsed += (sender, e) =>
                {
                    this.Dispatcher.BeginInvoke(
                        (Action)delegate
                            {
                                if (newBinding == null)
                                {
                                    txtStatus.ClearValue(TextBlock.TextProperty);
                                }
                                else
                                {
                                    txtStatus.SetBinding(TextBlock.TextProperty, newBinding);
                                }

                                txtStatus.BeginAnimation(OpacityProperty, new DoubleAnimation(0.0, 1.0, new Duration(new TimeSpan(0, 0, 0, 0, 300))));                                    
                            });
                    timer.Dispose();
                };
            timer.Start();
        }

        private void AnnouncementTimerElapsed(object sender, EventArgs e)
        {
            // Just incase we call this manually.
            if (this.currentAnnouncementTimer != null)
            {
                this.currentAnnouncementTimer.Dispose();
            }

            this.Dispatcher.BeginInvoke((Action)delegate
            {
                if (this.ViewModel.AnnouncementText.Count == 0)
                {
                    // Probably shouldn't get to this point if this is the case. Just incase...
                    return;
                }

                int currentIndex = (this.previousAnnouncementIndex + 1) % this.ViewModel.AnnouncementText.Count;

                var textField = this.ViewModel.AnnouncementText[currentIndex];

                this.previousAnnouncementIndex = currentIndex;

                this.ChangeAnnouncementText(new Binding("Text") { Source = textField });

                if (this.ViewModel.AnnouncementText.Count < 2)
                {
                    // No need to restart a timer or do anything.
                    return;
                }

                this.currentAnnouncementTimer = new Timer(textField.Time * 1000) { AutoReset = false };
                this.currentAnnouncementTimer.Elapsed += this.AnnouncementTimerElapsed;
                this.currentAnnouncementTimer.Start();
            });
        }

        /// <summary> Increments the subbar text when the given timer elapses, and disposes of the timer object. </summary>
        /// <param name="sender"> The sender. </param>
        /// <param name="e"> The event arguments. </param>
        private void SubbarTimerElapsed(object sender, EventArgs e)
        {
            // Just incase we call this manually.
            if (this.currentSubbarTimer != null)
            {
                this.currentSubbarTimer.Dispose();                         
            }

            this.Dispatcher.BeginInvoke((Action)delegate
                {
                    if (this.ViewModel.SubbarText.Count == 0)
                    {
                        // Probably shouldn't get to this point if this is the case. Just incase...
                        return;
                    }

                    int currentIndex = (this.previousSubbarIndex + 1) % this.ViewModel.SubbarText.Count;

                    var textField = this.ViewModel.SubbarText[currentIndex];

                    this.previousSubbarIndex = currentIndex;

                    this.ChangeMatchupText(new Binding("Text") { Source = textField });

                    if (this.ViewModel.SubbarText.Count < 2)
                    {
                        // No need to restart a timer or do anything.
                        return;
                    }

                    this.currentSubbarTimer = new Timer(textField.Time * 1000) { AutoReset = false };
                    this.currentSubbarTimer.Elapsed += this.SubbarTimerElapsed;
                    this.currentSubbarTimer.Start();
                });
        }
    }
}
