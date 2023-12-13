using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for PerformStats.xaml
    /// </summary>
    public partial class PerformStats : Window
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

        class PerfStatsTypeListViewItem
        {
            public PerfStats Content {get; set; }
            public override string ToString()
            {
                return ("Statistic: " + Content.Statistic + ". Occurances: " + Content.Occurrences);
            }
        }

        Code.TraceFileInfo tfi;
        PerformStats(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            this.tfi = tfi;
            InitializeComponent();

            uint deterministic_inversions = ELTMLManaged.TMLFunctions.Performance.DeterministicInversions;
            uint non_deterministic_inversions = ELTMLManaged.TMLFunctions.Performance.UndeterministicInversions;

            this.dictThread = new Dictionary<string, Dictionary<string, uint>>();
            Dictionary<string, uint> dictSum = new Dictionary<string, uint>();
            dictSum.Add("Context Switches", ELTMLManaged.TMLFunctions.Performance.ContextSwitches);
            dictSum.Add("Time Slices", ELTMLManaged.TMLFunctions.Performance.TimeSlices);
            dictSum.Add("Thread Preemptions", ELTMLManaged.TMLFunctions.Performance.Preemptions);
            dictSum.Add("Thread Suspensions", ELTMLManaged.TMLFunctions.Performance.Suspensions);
            dictSum.Add("Thread Resumptions", ELTMLManaged.TMLFunctions.Performance.Resumptions);
            dictSum.Add("Interrupts", ELTMLManaged.TMLFunctions.Performance.Interrupts);

            dictSum.Add("Priority Inversions", deterministic_inversions + non_deterministic_inversions);
            dictSum.Add("     Deterministic", deterministic_inversions);
            dictSum.Add("     Non-deterministic", non_deterministic_inversions);
            this.dictThread.Add("General Summary - Entire System", dictSum);

            Dictionary<string, uint> dictPerf;
            int NameAddition = 0;
            foreach (TmlThread tt in threads)
            {
                dictPerf = new Dictionary<string, uint>();
                string threadName = tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")";

                dictPerf.Add("Thread Suspensions", tt.Suspensions);
                dictPerf.Add("Thread Resumptions", tt.Resumptions);

                if (dictThread.ContainsKey(threadName))
                {
                    threadName = threadName + NameAddition.ToString(CultureInfo.InvariantCulture);
                    NameAddition++;
                }

                dictThread.Add(threadName, dictPerf);
            }

            comboBox1.Items.Clear();
            foreach (object key in dictThread.Keys)
            {
                comboBox1.Items.Add(Convert.ToString(key, CultureInfo.CurrentCulture));
            }
            comboBox1.SelectedIndex = 0;

            //InitPerfStats();
        }

        Dictionary<string, Dictionary<string, uint>> dictThread;

        public static Window Show(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            PerformStats psDlg = new PerformStats(tfi, threads);
            //psDlg.ShowDialog();
            psDlg.Show();
            return psDlg;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstvwPerfStats.Items.Clear();

            Dictionary<string, uint> dict;
            dict = this.dictThread[Convert.ToString(comboBox1.SelectedValue, CultureInfo.InvariantCulture)];

            long sum = this.tfi.Events.Count;
            foreach (string key in dict.Keys)
            {
                PerfStatsTypeListViewItem lvi = new PerfStatsTypeListViewItem();
                lvi.Content = new PerfStats(key, dict[key].ToString(CultureInfo.InvariantCulture));
                lstvwPerfStats.Items.Add(lvi);
            }
        }
    }
}
