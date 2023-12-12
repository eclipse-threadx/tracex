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
using System.ComponentModel;
using AzureRTOS.Tml;
using System.Globalization;
using System.Windows.Automation;
using System.Windows.Automation.Peers;


namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for StackUsage.xaml
    /// </summary>
    public partial class StackUsage : Window
    {
        public StackUsage()
        {
            InitializeComponent();
        }

        List<TmlThread> threads;
        public StackUsage(List<TmlThread> threads)
        {
            InitializeComponent();
            this.threads = threads;

            /* Place percentage value in thread.percentage for simple reordering (CompareTo).  */
            foreach (TmlThread thread in threads)
            {
                if (thread.StackSize == 0)
                {
                    thread.Percentage = 0;
                }
                else
                {
                    thread.Percentage = (double)(thread.StackSize - thread.Availability) / (double)thread.StackSize;
                }
            }

            FillListView(null, ListSortDirection.Ascending);
        }

        public static Window ShowMe(List<TmlThread> threads)
        {
            StackUsage usage = new StackUsage(threads);
            usage.Show();
            return usage;
        }

        /* Below this line was added.  10.23.08.  */
        private bool FillListView(string sortBy, ListSortDirection direction)
        {
            lstvwStackUsage.Items.Clear();
            bool sorted = false;

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy == "Thread Name")
                {
                    sorted = true;
                    if (direction == ListSortDirection.Ascending)
                    {
                        threads.Sort(delegate (TmlThread name1, TmlThread name2) { return string.Compare(name1.Name, name2.Name, StringComparison.Ordinal); });
                    }
                    else
                    {
                        threads.Sort(delegate (TmlThread name1, TmlThread name2) { return string.Compare(name2.Name, name1.Name, StringComparison.Ordinal); });
                    }
                }
                else if (sortBy == "Availability")
                {
                    sorted = true;
                    if (direction == ListSortDirection.Ascending)
                    {
                        threads.Sort(delegate (TmlThread minAvail1, TmlThread minAvail2) { return minAvail1.Availability.CompareTo(minAvail2.Availability); });
                    }
                    else
                    {
                        threads.Sort(delegate (TmlThread minAvail1, TmlThread minAvail2) { return minAvail2.Availability.CompareTo(minAvail1.Availability); });
                    }
                }
                else if (sortBy == "Usage Graph")
                {
                    sorted = true;
                    if (direction == ListSortDirection.Ascending)
                    {
                        threads.Sort(delegate (TmlThread usage1, TmlThread usage2) { return usage1.Percentage.CompareTo(usage2.Percentage); });
                    }
                    else
                    {
                        threads.Sort(delegate (TmlThread usage1, TmlThread usage2) { return usage2.Percentage.CompareTo(usage1.Percentage); });
                    }
                }
            }

            foreach (TmlThread thread in threads)
            {
                Code.ThreadStackUsageItem item = new AzureRTOS.TraceManagement.Code.ThreadStackUsageItem();
                item.Name = thread.Name + " (0x" + thread.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")";
                item.StackSize = thread.StackSize;
                item.Availability = thread.Availability;
                item.Percent = thread.Percentage;

                item.eventID = thread.EventId;
                Code.ThreadStackUsageItemTypeListViewItem lvi = new Code.ThreadStackUsageItemTypeListViewItem();
                lvi.Content = item;
                lstvwStackUsage.Items.Add(lvi);
            }
            return sorted;
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;

        void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
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
            bool sorted = false;

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
                    sorted = Sort(header, direction);

                    var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
                    if (peer != null)
                    {
                        if (sorted)
                        {
                            StatusBlock.Text = header + " sorted " + direction.ToString();
                        }
                        else
                        {
                            StatusBlock.Text = "sort by " + header + " not supported.";
                        }
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

        private bool Sort(string sortBy, ListSortDirection direction)
        {
            return FillListView(sortBy, direction);
        }
    }
}
