using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AzureRTOS.TraceManagement
{

    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public string buildDate = GetLinkerTime().ToString("MM/dd/yyyy", CultureInfo.CurrentCulture);

        public bool ShowCloseButton { set { this.btnClose.Visibility = value == true? Visibility.Visible : Visibility.Hidden; } }

        System.Threading.Timer timer;
        bool closeOnClick = false;
        System.Threading.AutoResetEvent waitEvent = new System.Threading.AutoResetEvent(false);
        public Splash()
        {
            InitializeComponent();
            InitBuildVersion();
        }
        public Splash(System.Threading.AutoResetEvent waitEvent)
        {
            InitializeComponent();
            InitBuildVersion();
            timer = new System.Threading.Timer(new System.Threading.TimerCallback(Timer_Callback), waitEvent, 3000, System.Threading.Timeout.Infinite);            
        }


        void Timer_Callback(object param)
        {             
            timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            System.Threading.AutoResetEvent waitEvent = (System.Threading.AutoResetEvent)param;
            waitEvent.Set();            
        }

        public static DateTime GetLinkerTime()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {           
            if(closeOnClick)
                this.Close();
        }

        public static void ShowMe()
        {
            System.Threading.AutoResetEvent waitEvent = new System.Threading.AutoResetEvent(false);
            Splash splashWindow = new Splash(waitEvent);
            splashWindow.closeOnClick = false;
            splashWindow.Topmost = true;
            splashWindow.Show();
            waitEvent.WaitOne(System.Threading.Timeout.Infinite, true);
            splashWindow.Close();
        }

        private void InitBuildVersion()
        {
            BuildDateText.Text = "Build Date:  " + buildDate;
            VersionString.Text = TraceXView._tracexVersion;
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void btnGot_Focus(object sender, RoutedEventArgs e)
        {
            Button tb = e.Source as Button;
            tb.Background = Brushes.White;
            tb.Foreground = Brushes.Black;
        }

        private void btnLose_Focus(object sender, RoutedEventArgs e)
        {
            Button tb = e.Source as Button;
            tb.Background = Brushes.Black;
            tb.Foreground = Brushes.Gold;
        }

        private void btnMouse_Enter(object sender, MouseEventArgs e)
        {
            Button tb = e.Source as Button;
            if (!tb.IsFocused)
            {
                tb.Foreground = Brushes.Black;
            }
        }

        private void btnMouse_Leave(object sender, MouseEventArgs e)
        {
            Button tb = e.Source as Button;
            if (!tb.IsFocused)
            {
                tb.Foreground = Brushes.Gold;
            }
        }
    }
}
