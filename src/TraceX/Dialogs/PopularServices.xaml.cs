using System;
using System.Collections.Generic;
using System.Collections;
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
using System.ComponentModel;
using System.Data;
using System.Globalization;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for PopularServices.xaml
    /// </summary>
    public partial class PopularServices : Window
    {
        Code.TraceFileInfo tfi;
        public PopularServices psDlgGlobal;

        public PopularServices(Code.TraceFileInfo tfi)
        {
            this.tfi = tfi;
            InitializeComponent();
            this.dictThread = new  Dictionary<string,Dictionary<string,long>>();
            this.dictSum = new  Dictionary<string,long>();
            this.dictThread.Add("General Summary - Entire System", dictSum);
            foreach (Tml.TmlEvent te in tfi.Events)
            {
                int index = tfi.FindThreadIndex(te);

                string threadName = String.Empty;
                if (-1 != index)
                {
                    //threadName = tfi.Threads[index].ObjectName;
                    threadName = tfi.Threads[index].Name + " (0x" + tfi.Threads[index].Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) +")" ;
                }
                

                Code.Event evt = Code.Event.CreateInstance(te, threadName,tfi);

                Dictionary<string, long> dictService;
                if (dictThread.ContainsKey(evt.ThreadName))
                {
                    dictService = (Dictionary<string, long>)dictThread[evt.ThreadName];
                }
                else
                {
                    dictService = new  Dictionary<string,long>();
                    dictThread.Add(evt.ThreadName, dictService);                    
                }

                if (dictService.ContainsKey(evt.EventTypeName))
                {
                    long cc = (long)dictService[evt.EventTypeName];
                    dictService[evt.EventTypeName] = ++cc;
                }
                else
                {
                    dictService.Add(evt.EventTypeName, (long)1);
                }

                if (dictSum.ContainsKey(evt.EventTypeName))
                {
                    long cc = (long)dictSum[evt.EventTypeName];
                    dictSum[evt.EventTypeName] = ++cc;
                }
                else
                {
                    dictSum.Add(evt.EventTypeName, (long)1);
                }

            }

            comboBox1.Items.Clear();            
            foreach (object key in dictThread.Keys)
            {
                comboBox1.Items.Add(Convert.ToString(key, CultureInfo.CurrentCulture));
            }
            comboBox1.SelectedIndex = 0; 
        }

        Dictionary<string,Dictionary<string,long> > dictThread;
        Dictionary<string, long> dictSum;

        
        public static Window Show(Code.TraceFileInfo tfi)
        {
            
            PopularServices psDlg = new PopularServices(tfi);
            psDlg.Show();                        
            return psDlg;
        }
       
        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


            FillListView(null, ListSortDirection.Ascending);
        }

        /* Below this line was added.  10.23.08.  */
        private void FillListView(string sortBy, ListSortDirection direction)
        {
            listView1.Items.Clear();
            long sum = this.tfi.Events.Count;
            //var result = from pair in dictThread select pair;
            
            Dictionary<string, long> dict;
            dict = dictThread[Convert.ToString(comboBox1.SelectedValue, CultureInfo.InvariantCulture)];
            var result = from pair in dict select pair;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Service")
                {
                    if (direction == ListSortDirection.Ascending)
                    {
                        result = from pair in dict orderby pair.Key ascending select pair;
                    }
                    else
                    {
                        result = from pair in dict orderby pair.Key descending select pair;
                    }
                }
                
                else
                {
                    if (direction == ListSortDirection.Ascending)
                    {
                        result = from pair in dict orderby pair.Value ascending select pair;
                    }
                    else
                    {
                        result = from pair in dict orderby pair.Value descending select pair;
                    }
                }
            }

            foreach (KeyValuePair<string, long> pair in result)
            {
                Code.PopServItem item = new AzureRTOS.TraceManagement.Code.PopServItem();
                item.Name = pair.Key;
                item.Count = pair.Value > 0 ? pair.Value : 0;
                item.Percent = ((double)item.Count) / sum;
                Code.PopServItemTypeListViewItem lvi = new Code.PopServItemTypeListViewItem();
                lvi.Content = item;
                listView1.Items.Add(lvi);
            }

        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender,RoutedEventArgs e)
        {
            // Re-sort the ListView as required.
            OnSort(e.OriginalSource);
        }

        private void GridViewColumnHeaderKeyDownHandler(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Space) || (e.Key == Key.Enter))
            {
                // Re-sort the ListView as required.
                OnSort(e.OriginalSource);
            }
        }

        private void OnSort(object originalSource)
        {
            GridViewColumnHeader headerClicked = originalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    string header = headerClicked.Column.Header as string;
                    Sort(header, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }


                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            FillListView(sortBy, direction);
        }

    }
}
