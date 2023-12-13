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
    /// Interaction logic for NetXStatistics.xaml
    /// </summary>
    public partial class NetXStatistics : Window
    {
        Code.TraceFileInfo tfi;
        Dictionary<string, uint> dictThread;
        private enum types : uint
        {
            STATISTICS_ARP = 0,
            STATISTICS_ARP_REQUESTS_SENT,
            STATISTICS_PACKET_POOL,
            STATISTICS_PING,
            STATISTICS_IP,
            STATISTICS_TCP,
            STATISTICS_UDP
        }

        private static readonly Dictionary<uint, string> typeStringDict = new Dictionary<uint, string>()
        {
            {(uint)types.STATISTICS_ARP, "ARP Statistics:" },
            {(uint)types.STATISTICS_PACKET_POOL, "Packet Pool Statistics:" },
            {(uint)types.STATISTICS_PING, "Ping Statistics:" },
            {(uint)types.STATISTICS_IP, "IP Statistics:" },
            {(uint)types.STATISTICS_TCP, "TCP Statistics:" },
            {(uint)types.STATISTICS_UDP, "UDP Statistics:" }
        };

        public NetXStatistics(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            this.tfi = tfi;
            InitializeComponent();

            dictThread = new Dictionary<string, uint>()
            {
                {typeStringDict[(uint)types.STATISTICS_ARP], 0},
                {"   ARP Requests Sent", ELTMLManaged.TMLFunctions.PerformanceNx.ArpRequestsSent},
                {"   ARP Responses Sent", ELTMLManaged.TMLFunctions.PerformanceNx.ArpResponsesSent},
                {"   ARP Requests Received", ELTMLManaged.TMLFunctions.PerformanceNx.ArpRequestsReceived},
                {"   ARP Responses Received", ELTMLManaged.TMLFunctions.PerformanceNx.ArpResponsesReceived},
                {typeStringDict[(uint)types.STATISTICS_PACKET_POOL], 0},
                {"   Packet Pool Allocations", ELTMLManaged.TMLFunctions.PerformanceNx.PacketAllocations},
                {"   Packet Pool Releases", ELTMLManaged.TMLFunctions.PerformanceNx.PacketReleases},
                {"   Empty Allocation Requests", ELTMLManaged.TMLFunctions.PerformanceNx.EmptyAllocations},
                {"   Packet Pool Invalid Releases", ELTMLManaged.TMLFunctions.PerformanceNx.InvalidReleases},
                {typeStringDict[(uint)types.STATISTICS_PING], 0},
                {"   Pings Sent", ELTMLManaged.TMLFunctions.PerformanceNx.PingsSent},
                {"   Ping Responses", ELTMLManaged.TMLFunctions.PerformanceNx.PingResponses},
                {typeStringDict[(uint)types.STATISTICS_IP], 0},
                {"   IP Packets Sent", ELTMLManaged.TMLFunctions.PerformanceNx.IpPacketsSent},
                {"   Total Bytes Sent", ELTMLManaged.TMLFunctions.PerformanceNx.IpBytesSent},
                {"   IP Packets Received", ELTMLManaged.TMLFunctions.PerformanceNx.IpPacketsReceived},
                {"   Total Bytes Received", ELTMLManaged.TMLFunctions.PerformanceNx.IpBytesReceived},
                {typeStringDict[(uint)types.STATISTICS_TCP], 0},
                {"   TCP Client Connections", ELTMLManaged.TMLFunctions.PerformanceNx.TcpClientConnections},
                {"   TCP Server Connections", ELTMLManaged.TMLFunctions.PerformanceNx.TcpServerConnections},
                {"   TCP Packets Sent", ELTMLManaged.TMLFunctions.PerformanceNx.TcpPacketsSent},
                {"   TCP Bytes Sent", ELTMLManaged.TMLFunctions.PerformanceNx.TcpBytesSent},
                {"   TCP Packets Received", ELTMLManaged.TMLFunctions.PerformanceNx.TcpPacketsReceived},
                {"   TCP Bytes Received", ELTMLManaged.TMLFunctions.PerformanceNx.TcpBytesReceived},
                {typeStringDict[(uint)types.STATISTICS_UDP], 0},
                {"   UDP Packets Sent", ELTMLManaged.TMLFunctions.PerformanceNx.UdpPacketsSent},
                {"   UDP Bytes Sent", ELTMLManaged.TMLFunctions.PerformanceNx.UdpBytesSent},
                {"   UDP Packets Received", ELTMLManaged.TMLFunctions.PerformanceNx.UdpPacketsReceived},
                { "   UDP Bytes Received", ELTMLManaged.TMLFunctions.PerformanceNx.UdpBytesReceived}
            };

            FillListView(null, ListSortDirection.Ascending);
        }

        public static Window Show(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            NetXStatistics nxDlg = new NetXStatistics(tfi, threads);
            //psDlg.ShowDialog();
            nxDlg.Show();
            return nxDlg;
        }

        private void FillListView(string sortBy, ListSortDirection direction)
        {
            listView1.Items.Clear();
            long sum = this.tfi.Events.Count;
            //var result = from pair in dictThread select pair;

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
                Code.PopServItem item = new AzureRTOS.TraceManagement.Code.PopServItem();

                item.Name = pair.Key;
                item.Count = pair.Value > 0 ? pair.Value : 0;
                item.stringCount = item.Count.ToString(CultureInfo.InvariantCulture);

                foreach (KeyValuePair<uint, string> entry in typeStringDict)
                {
                    if(0 == string.Compare(pair.Key, entry.Value, StringComparison.Ordinal))
                    {
                        if(entry.Key != (uint)types.STATISTICS_ARP)
                        {
                            // insert a blank row
                            listView1.Items.Add(new ListViewItem());
                        }

                        item.stringCount = string.Empty;
                        break;
                    }
                }

                Code.PopServItemTypeListViewItem lvi = new Code.PopServItemTypeListViewItem();
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
