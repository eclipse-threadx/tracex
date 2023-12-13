using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for CurrentEventInfo.xaml
    /// </summary>
    public partial class CurrentEventInfo : Window
    {
        private static CurrentEventInfo currentEventWindow = null;

        public CurrentEventInfo()
        {
            InitializeComponent();
        }

        public static Window Show(Code.TraceFileInfo tfi)
        {
            if (currentEventWindow == null)
            {
                currentEventWindow = new CurrentEventInfo();
                currentEventWindow.Show();
            }

            currentEventWindow.Update(tfi);
            currentEventWindow.Activate();
            return currentEventWindow;
        }

        public static bool hasCurrentEventWindow()
        {
            if (currentEventWindow == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            currentEventWindow = null;
            base.OnClosed(e);
        }

        public void Update(Code.TraceFileInfo tfi)
        {
            if (tfi != null)
            {
                TmlEvent tmlEvent = tfi.Events[tfi.CurrentEventIndex];
                int index = tfi.FindThreadIndex(tmlEvent);
                //Code.Event currentEvent = tfi.Events[tfi.CurrentEventIndex];
                String tName = tfi.Threads[index].Name;
                Code.Event currentEvent = Code.Event.CreateInstance(tmlEvent, tName, tfi);
                txtDetails.Text = currentEvent.GetEventDetailString(tfi);
            }
        }
    }
}
