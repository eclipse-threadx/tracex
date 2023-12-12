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

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for HeaderInfo.xaml
    /// </summary>
    public partial class HeaderInfo : Window
    {
        private const uint TML_TRACE_VALID_BE  =  0x54585442;
        private const uint TML_TRACE_VALID_LE = 0x42545854;

        public HeaderInfo()
        {
            InitializeComponent();
            InitializeHeaderInfo();
        }

        private void InitializeHeaderInfo()
        {
            string fileSize = string.Format(CultureInfo.CurrentCulture, "The file length is: {0} bytes.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceFileSize);
            listBox1.Items.Add(fileSize);
            string traceIdType = ELTMLManaged.TMLFunctions.HeaderInfo.TraceId == TML_TRACE_VALID_LE ? "Little Endian" : "Big Endian";
            string traceId = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     {1}: Trace ID found.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceId, traceIdType);
            listBox1.Items.Add(traceId);            
            string timeMask = string.Format(CultureInfo.CurrentCulture, "{0:X8}:      Time source mask.", ELTMLManaged.TMLFunctions.HeaderInfo.TimerValidMask);
            listBox1.Items.Add(timeMask);
            string nameSize = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The number of bytes in object name.", ELTMLManaged.TMLFunctions.HeaderInfo.ObjectNameSize);
            listBox1.Items.Add(nameSize);
            string baseAddr = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The base address of all pointers.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceBaseAddress);
            listBox1.Items.Add(baseAddr);
            string startReg = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The pointer to the start of Trace Object Registry.", ELTMLManaged.TMLFunctions.HeaderInfo.ObjectRegistryStartAddress);
            listBox1.Items.Add(startReg);
            string endReg = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The pointer to the end of Trace Object Registry.", ELTMLManaged.TMLFunctions.HeaderInfo.ObjectRegistryEndAddress);
            listBox1.Items.Add(endReg);
            string startBuffer = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The pointer to the start of the Trace Buffer Area.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceBufferStartAddress);
            listBox1.Items.Add(startBuffer);
            string endBuffer = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The pointer to the end of Trace Buffer Area.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceBufferEndAddress);
            listBox1.Items.Add(endBuffer);
            string oldBuffer = string.Format(CultureInfo.CurrentCulture, "{0:X8}:     The pointer to the oldest entry in the Trace Buffer Area.", ELTMLManaged.TMLFunctions.HeaderInfo.TraceBufferCurrentAddress);
            listBox1.Items.Add(oldBuffer);
        }

        public static Window ShowMe()
        {
            HeaderInfo hiDlg = new HeaderInfo();            
            hiDlg.Show();
            return hiDlg;
        }

    }
}
