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

using AzureRTOS.Tml;
using System.Globalization;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for FileXStatistics.xaml
    /// </summary>
    public partial class FileXStatistics : Window
    {
        class PerfStats
        {
            public string Statistic { get; set; }
            public string Occurrences { get; set; }

            public PerfStats(string statistic, string occurrences)
            {
                Statistic = statistic;
                Occurrences = occurrences;
            }
        }

        Code.TraceFileInfo tfi;
        Dictionary<string, uint> dictThread;

        FileXStatistics(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            this.tfi = tfi;
            InitializeComponent();

            //this.dictThread = new Dictionary<string, Dictionary<string, uint>>();
            Dictionary<string, uint> dictSum = new Dictionary<string, uint>();
            dictSum.Add("Media Statistics:", 0);
            dictSum.Add("   Media Opens", ELTMLManaged.TMLFunctions.PerformanceFx.MediaOpens);
            dictSum.Add("   MediaCloses", ELTMLManaged.TMLFunctions.PerformanceFx.MediaCloses);
            dictSum.Add("   Media Aborts", ELTMLManaged.TMLFunctions.PerformanceFx.MediaAborts);
            dictSum.Add("   Media Flushes", ELTMLManaged.TMLFunctions.PerformanceFx.MediaFlushes);

            dictSum.Add("Directory Statistics:", 0);
            dictSum.Add("   Directory Reads", ELTMLManaged.TMLFunctions.PerformanceFx.DirectoryReads);
            dictSum.Add("   Directory Writes", ELTMLManaged.TMLFunctions.PerformanceFx.DirectoryWrites);
            dictSum.Add("   Directory Cache Misses", ELTMLManaged.TMLFunctions.PerformanceFx.DirectoryCacheMisses);

            dictSum.Add("File Statistics:", 0);
            dictSum.Add("   File Opens", ELTMLManaged.TMLFunctions.PerformanceFx.FileOpens);
            dictSum.Add("   File Closes", ELTMLManaged.TMLFunctions.PerformanceFx.FileCloses);
            dictSum.Add("   File Bytes Read", ELTMLManaged.TMLFunctions.PerformanceFx.FileBytesRead);
            dictSum.Add("   File Bytes Written", ELTMLManaged.TMLFunctions.PerformanceFx.FileBytesWritten);

            dictSum.Add("Logical Sector Statistics", 0);
            dictSum.Add("   Logical Sector Reads", ELTMLManaged.TMLFunctions.PerformanceFx.LogicalSectorReads);
            dictSum.Add("   Logical Sector Writes", ELTMLManaged.TMLFunctions.PerformanceFx.LogicalSectorWrites);
            dictSum.Add("   Logical Sector Cache Misses", ELTMLManaged.TMLFunctions.PerformanceFx.LogicalSectorCacheMisses);

            dictThread = dictSum;
            FillListView(null, ListSortDirection.Ascending);
        }

        public static Window Show(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            FileXStatistics fxDlg = new FileXStatistics(tfi, threads);
            fxDlg.Show();
            return fxDlg;
        }

        private void FillListView(string sortBy, ListSortDirection direction)
        {
            listView1.Items.Clear();

            long sum = this.tfi.Events.Count;

            var result = from pair in dictThread select pair;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Statistic")
                {
                    if (direction == ListSortDirection.Ascending)
                    {
                        result = from pair in dictThread orderby pair.Key ascending select pair;
                    }
                    else
                    {
                        result = from pair in dictThread orderby pair.Key descending select pair;
                    }
                }
                else
                {
                    if (direction == ListSortDirection.Ascending)
                    {
                        result = from pair in dictThread orderby pair.Value ascending select pair;
                    }
                    else
                    {
                        result = from pair in dictThread orderby pair.Value descending select pair;
                    }
                }
            }

            foreach (KeyValuePair<string, uint> pair in result)
            {
                Code.PopServItemTypeListViewItem lvi = new Code.PopServItemTypeListViewItem();

                Code.PopServItem item = new AzureRTOS.TraceManagement.Code.PopServItem();
                item.Name = pair.Key;

                if (StringComparer.Ordinal.Equals(pair.Key, "Media Statistics:"))
                {
                    item.stringCount = string.Empty;
                }
                else
                {
                    if (StringComparer.Ordinal.Equals(pair.Key, "Directory Statistics:") ||
                        StringComparer.Ordinal.Equals(pair.Key, "File Statistics:") ||
                        StringComparer.Ordinal.Equals(pair.Key, "Logical Sector Statistics"))
                    {
                        // insert a blank row
                        ListViewItem lvi2 = new ListViewItem();
                        listView1.Items.Add(lvi2);

                        item.stringCount = string.Empty;
                    }
                    else
                    {
                        item.Count = pair.Value > 0 ? pair.Value : 0;
                        item.stringCount = item.Count.ToString(CultureInfo.InvariantCulture);
                    }
                }

                lvi.Content = item;
                listView1.Items.Add(lvi);
            }
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked =
                  e.OriginalSource as GridViewColumnHeader;
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
