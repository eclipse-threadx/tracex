using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AzureRTOS.Tml;
using AzureRTOS.TraceManagement.Code;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for PopularServices.xaml
    /// </summary>
    public partial class Legend : Window
    {
        private Dictionary<string, FrameworkElement> _dictLegend = new Dictionary<string, FrameworkElement>();

        Legend(Code.TraceFileInfo tfi, string legendType)
        {
            InitializeComponent();

            uint[] array= { };

            if (legendType == "tx")
            {
                Title = "ThreadX Legend";
                array = EventArrays.TmlTxEvents;
            }
            else if(legendType == "fx")
            {
                Title = "FileX Legend";
                array = EventArrays.TmlFxEvents;
            }
            else if(legendType == "nx")
            {
                Title = "NetX Legend";
                array = EventArrays.TmlNxEvents;
            }
            else if(legendType == "ux")
            {
                Title = "USBX Legend";
                array = EventArrays.TmlUxEvents;
            }

            for (int index = 0; index < array.Length; index++)
            {
                var te = new TmlEvent
                {
                    Id = Convert.ToUInt32(array[index], CultureInfo.InvariantCulture),
                };

                var e = Code.Event.CreateInstance(te, string.Empty, tfi);
                _dictLegend.Add(e.EventTypeName, e.CreateIcon());
            }

            var result = from pair in _dictLegend select pair;
            foreach (KeyValuePair<string, FrameworkElement> pair in result)
            {
                var canvas = (Canvas)pair.Value;
                UIElementCollection children = canvas.Children;
                int childcount = children.Count;

                for (int index = 0; index > childcount; index++)
                {
                    Canvas.SetTop(children[index], Canvas.GetTop(children[index]) + 12);
                }
                var lvi = new KeyValuePairTypeListViewItem
                {
                    Content = pair
                };

                listView1.Items.Add(lvi);
            }
        }
        class KeyValuePairTypeListViewItem
        {
            public KeyValuePair<string, FrameworkElement> Content { get; set; }
            public override string ToString()
            {
                return Content.Key;
            }
        }

        public static Window Show(Code.TraceFileInfo tfi, string legendType)
        {
            var lDlg = new Legend(tfi, legendType);
            lDlg.Show();
            return lDlg;
        }

        class LegendItem
        {
            public LegendItem(string eventName, FrameworkElement icon)
            {
                EventName = eventName;
                Icon = icon;
            }

            public string EventName { get; set; }
            public FrameworkElement Icon { get; set; }
        }
    }
}
