using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.ComponentModel;
using System.Data;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for PopularServices.xaml
    /// </summary>
    public partial class ExecutionProfile : Window
    {
        private static ExecutionProfile exeProfile = null;

        Code.TraceFileInfo tfi;
        ExecutionProfile(Code.TraceFileInfo tfi)
        {
            this.tfi = tfi;
            InitializeComponent();
            this.dictThread = tfi.DictThreadExecution;
            FillListView(null, ListSortDirection.Ascending);
        }

        private void FillListView(string sortBy, ListSortDirection direction)
        {
            listView1.Items.Clear();
            long sum = this.tfi.TmlTraceInfo.MaxRelativeTicks;
            var result = from pair in dictThread select pair;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Context")
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

            foreach (KeyValuePair<string,long> pair in result)
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

        Dictionary<string,long> dictThread;        
        public static Window Show(Code.TraceFileInfo tfi)
        {
            if (exeProfile == null)
            {
                exeProfile = new ExecutionProfile(tfi);
                exeProfile.Show();
            }
            exeProfile.Activate();
            return exeProfile;
        }

        protected override void OnClosed(EventArgs e)
        {
            exeProfile = null;
            base.OnClosed(e);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender,
                                                RoutedEventArgs e)
        {
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

        private void GridViewColumnHeaderGotFocus(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                headerClicked.Background = System.Windows.Media.Brushes.LightBlue;
                headerClicked.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void GridViewColumnHeaderLostFocus(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked != null)
            {
                headerClicked.Background = SystemColors.WindowBrush;
                headerClicked.Foreground = SystemColors.WindowTextBrush;
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

                    var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
                    if (peer != null)
                    {
                        StatusBlock.Text = header + " sorted " + direction.ToString();
                        peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                    }

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
