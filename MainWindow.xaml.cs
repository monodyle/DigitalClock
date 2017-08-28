using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DigitalClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // change size function
        public void ChangeSize(int a, int b)
        {
            clockTimes.FontSize = a;
            clockDates.FontSize = b;

            if ( a == 94 )
            {
                clockTimes.Margin = new Thickness(0, 30, 0, 70);
                clockDates.Margin = new Thickness(0, 135, 0, 60);
            }
            else if ( a == 72 )
            {
                clockTimes.Margin = new Thickness(0, 50, 0, 70);
                clockDates.Margin = new Thickness(0, 120, 0, 60);
            }
            else if ( a == 120 )
            {
                clockTimes.Margin = new Thickness(0, 20, 0, 70);
                clockDates.Margin = new Thickness(0, 120, 0, 20);
            }
        }
        // scale clock function
        public void ClockScale(string size)
        {
        }
        public MainWindow()
        {
            InitializeComponent();
            // configure clock
            _clock = new Thread(() => {
                while (true)
                {
                    // update date and time by realtime
                    clockTimes.Dispatcher.Invoke(new Action(() => clockTimes.Content = DateTime.Now.ToString("HH:mm")));
                    clockDates.Dispatcher.Invoke(new Action(() => clockDates.Content = DateTime.Now.ToString("dddd, dd MMMM yyyy")));
                    Thread.Sleep(5000); // sleep
                }
            });
            // start digital clock
            _clock.Start();

            // set window startup location
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            // do not make this program crash
            try
            {
                string[] datafile = File.ReadAllLines("config.setting"); // read setting file
                string[] datalist = datafile[0].Split(';'); // get position

                this.Top = int.Parse(datalist[0]);
                this.Left = int.Parse(datalist[1]);

                string color = datafile[1]; // get color
                if ( color == "#FF000000") {
                    clockTimes.Foreground = Brushes.Black;
                    clockDates.Foreground = Brushes.Black;
                }
                else
                {
                    clockTimes.Foreground = Brushes.White;
                    clockDates.Foreground = Brushes.White;
                }

                string size = datafile[2];
                int scale = 2;
                if (size == "94") { scale = 2; } else if (size == "72") { scale = 1; } else if (size == "120") { scale = 3; } else { scale = 1; }

                if (scale == 2)
                {
                    ChangeSize(94, 18);
                }
                else if (scale == 1)
                {
                    ChangeSize(72, 14);
                }
                else if (scale == 3)
                {
                    ChangeSize(120, 24);
                }
            }
            catch
            {
                this.Top = 0; this.Left = 0; // set defaut position
            }

        }

        // create new thread variable
        Thread _clock;

        // drag the digital clock
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        // close digital clock
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string position = this.Top.ToString() + ";" + this.Left.ToString();
            File.WriteAllLines("config.setting", new string[] { position, clockTimes.Foreground.ToString(), clockTimes.FontSize.ToString() });
            _clock.Abort();
        }

        // close contextmenu
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // change color contextmenu
        private void ChangeColor_Click(object sender, RoutedEventArgs e)
        {
            string color = clockTimes.Foreground.ToString();
            if (color == "#FF000000")
            {
                clockTimes.Foreground = Brushes.White;
                clockDates.Foreground = Brushes.White;
                color = "#00000000";
            }
            else
            {
                clockTimes.Foreground = Brushes.Black;
                clockDates.Foreground = Brushes.Black;
                color = "#FF000000";
            }
        }

        // change size contextmenu
        private void ChangeSize_Click(object sender, RoutedEventArgs e)
        {
            int scale = 2;
            string size = clockTimes.FontSize.ToString();
            if (size == "94") { scale = 2; } else if (size == "72") { scale = 1; } else if (size == "120") { scale = 3; } else { scale = 1; }

            if (scale == 2)
            {
                ChangeSize(120, 24);
            }
            else if (scale == 1)
            {
                ChangeSize(94, 18);
            }
            else if (scale == 3)
            {
                ChangeSize(72, 14);
            }
        }
    }
}