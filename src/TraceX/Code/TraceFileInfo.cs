using System;
using System.Collections.Generic;
using System.Globalization;

using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement.Code
{
    public class TraceFileInfo
    {
        private const uint THREAD_CONTEXT_INTERRUPT = 0xFFFFFFFF;
        private const uint THREAD_CONTEXT_INITIALIZE = 0xF0F0F0F0;
        private const uint THREAD_CONTEXT_IDLE = 0x00000000;

        private List<TmlObject> _objectsNew;

        public TmlInterruptIdle InterruptIdle { get; set; }

        public Dictionary<string, long> DictThreadExecution { get; }
        public Dictionary<uint, List<TmlExecutionStatus>> DictThreadExecutionStatus { get; }
        public TmlTrace TmlTraceInfo { get; set; }
        public List<TmlThread> Threads { get; set; }
        public List<TmlObject> Objects { get; set; }
        public List<TmlEvent> Events { get; set; }
        public int CurrentEventIndex { get; set; }

        public TraceFileInfo(TmlTrace tmlTraceInfo, List<TmlThread> threads, List<TmlObject> objects, List<TmlEvent> events, Dictionary<uint, List<TmlExecutionStatus>> dictThreadExecutionStatus)
        {
            double initializeUsageTotal = 0;
            TmlTraceInfo = tmlTraceInfo;
            InterruptIdle = new TmlInterruptIdle();

            Threads = new List<TmlThread>(threads.Count + 2);
            _objectsNew = new List<TmlObject>(objects.Count);

            TmlThread thd0 = new TmlThread();
            thd0.Address = THREAD_CONTEXT_INTERRUPT;
            thd0.Name = "Interrupt";
            thd0.DisplayIndex = 0;
            Threads.Insert(0, thd0);

            TmlThread thd1 = new TmlThread();
            thd1.Address = THREAD_CONTEXT_INITIALIZE;
            thd1.Name = "Initialize/Idle";
            thd1.DisplayIndex = 1;
            Threads.Insert(1, thd1);

            Threads.AddRange(threads);
            _objectsNew.AddRange(objects);

            Objects = objects;
            Events = events;
            DictThreadExecutionStatus = dictThreadExecutionStatus;

            // fill threadexecution dictionary
            DictThreadExecution = new Dictionary<string, long>();

            foreach (TmlThread tt in threads)
            {
                string threadName = tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")";
                //To prevent duplicate entries...

                if (!DictThreadExecution.ContainsKey(threadName))
                {
                    DictThreadExecution.Add(threadName, (long)tt.Usage);
                    initializeUsageTotal = initializeUsageTotal + tt.Usage;
                }
            }

            double interruptUsage = ELTMLManaged.TMLFunctions.InterruptIdle.InterruptUsage;
            double idleUsage = ELTMLManaged.TMLFunctions.InterruptIdle.IdleUsage;

            initializeUsageTotal = TmlTraceInfo.MaxRelativeTicks - (initializeUsageTotal + interruptUsage + idleUsage);
            ELTMLManaged.TMLFunctions.InterruptIdle.InitializeUsage = (long)initializeUsageTotal;

            DictThreadExecution.Add("Interrupt", (long)interruptUsage);
            DictThreadExecution.Add("Idle", (long)idleUsage);
            DictThreadExecution.Add("Initialize", (long)initializeUsageTotal);
        }

        public int FindThreadIndex(TmlEvent tmlEvent)
        {
            if (tmlEvent.Context == THREAD_CONTEXT_IDLE)
                return 1;

            for (int i = 0; i < Threads.Count; i++)
            {
                TmlThread tmlThrd = Threads[i];
                if (tmlThrd.Address == tmlEvent.Context)
                {
                    return i;
                }
            }
            return -1;
        }

        public string FindObject(uint tmlEventInfo)
        {
            for (int i = 0; i < _objectsNew.Count; i++)
            {
                TmlObject tmlObj = _objectsNew[i];

                if (tmlObj.Address == tmlEventInfo)
                {
                    return " (" + tmlObj.ObjectName + ")";
                }
                else if (0xFFFFFFFF == tmlEventInfo)
                {
                    return " (Interrupt)";
                }
                else if (0xF0F0F0F0 == tmlEventInfo)
                {
                    return " (Initialize)";
                }
                else if (0x00000000 == tmlEventInfo)
                {
                    return " (Idle)";
                }
                /* Insert if ffffffff or f0f0f0f0 or 0000000000.   */
            }
            return null;
        }

        // Find thread execution state
        public string FindStateField(uint tmlEventInfo)
        {
            switch (tmlEventInfo)
            {
                case 0x00:
                    return " (READY)";

                case 0x01:
                    return " (COMPLETE)";

                case 0x02:
                    return " (TERMINATE)";

                case 0x03:
                    return " (SUSPEND)";

                case 0x04:
                    return " (SLEEP)";

                case 0x05:
                    return " (QUEUE SUSPEND)";

                case 0x06:
                    return " (SEMAPHORE SUSPEND)";

                case 0x07:
                    return " (EVENT FLAG SUSPEND)";

                case 0x08:
                    return " (BLOCK MEMORY SUSPEND)";

                case 0x09:
                    return " (BYTE MEMORY SUSPEND)";

                case 0x0C:
                    return " (NETX SUSPEND)";

                case 0x0D:
                    return " (MUTEX SUSPEND)";

                default:
                    return null;
            }
        }

        public string FindSocketState(uint tmlEventInfo)
        {
            switch (tmlEventInfo)
            {
                case 0x01:
                    return " (CLOSED)";

                case 0x02:
                    return " (LISTEN)";

                case 0x03:
                    return " (SYN SENT)";

                case 0x04:
                    return " (SYN RECEIVED)";

                case 0x05:
                    return " (ESTABLISHED)";

                case 0x06:
                    return " (CLOSE WAIT)";

                case 0x07:
                    return " (FIN WAIT 1)";

                case 0x08:
                    return " (FIN WAIT 2)";

                case 0x09:
                    return " (CLOSING)";

                case 0x0A:
                    return " (TIMED WAIT)";

                case 0x0B:
                    return " (LAST ACK)";

                default:
                    return null;
            }
        }

        public string FindOpenType(uint tmlEventInfo)
        {
            switch (tmlEventInfo)
            {
                case 0x01:
                    return " (READ)";

                case 0x02:
                    return " (WRITE)";

                case 0x03:
                    return " (FAST READ)";

                default:
                    return null;
            }
        }

        public string FindStatus(uint tmlEventInfo)
        {
            if ((tmlEventInfo & Convert.ToInt32("0", 2)) == Convert.ToInt32("0", 2))
            {
                return " (INITIALIZED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("2", 2)) == Convert.ToInt32("2", 2))
            {
                return " (ADDRESS RESOLVED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("4", 2)) == Convert.ToInt32("4", 2))
            {
                return " (LINK ENABLED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("8", 2)) == Convert.ToInt32("8", 2))
            {
                return " (ARP ENABLED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("16", 2)) == Convert.ToInt32("16", 2))
            {
                return " (UDP ENABLED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("32", 2)) == Convert.ToInt32("32", 2))
            {
                return " (TCP ENABLED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("64", 2)) == Convert.ToInt32("64", 2))
            {
                return " (IGMP ENABLED)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("128", 2)) == Convert.ToInt32("128", 2))
            {
                return " (RARP ENABLED)";
            }
            else
            {
                return null;
            }
        }

        public string FindIPAddress(uint tmlEventInfo)
        {
            /* Declare local variables.  */
            ulong ipAddress = (ulong)tmlEventInfo;

            /* Shift bytes appropriately inorder to check endianness.  */
            ulong temp1 = (ipAddress >> 24);
            ulong temp2 = (ipAddress >> 16) & 0xFF;
            ulong temp3 = (ipAddress >> 8) & 0xFF;
            ulong temp4 = (ipAddress) & 0xFF;

            return " (" + temp1.ToString(CultureInfo.InvariantCulture) + "." + temp2.ToString(CultureInfo.InvariantCulture) + "." + temp3.ToString(CultureInfo.InvariantCulture) + "." + temp4.ToString(CultureInfo.InvariantCulture) + ")";
        }

        public string FindAttributes(uint tmlEventInfo)
        {
            if ((tmlEventInfo & Convert.ToInt32("0", 2)) == Convert.ToInt32("0", 2))
            {
                return " (READ ONLY)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("2", 2)) == Convert.ToInt32("2", 2))
            {
                return " (HIDDEN)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("4", 2)) == Convert.ToInt32("4", 2))
            {
                return " (SYSTEM)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("8", 2)) == Convert.ToInt32("8", 2))
            {
                return " (VOLUME)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("16", 2)) == Convert.ToInt32("16", 2))
            {
                return " (DIRECTORY)";
            }
            else if ((tmlEventInfo & Convert.ToInt32("32", 2)) == Convert.ToInt32("32", 2))
            {
                return " (ARCHIVE)";
            }
            else
            {
                return null;
            }
        }

        public string FindPacketType(uint tmlEventInfo)
        {
            if (tmlEventInfo == 36)
            {
                return " (IP Packet)";
            }
            else if (tmlEventInfo == 44)
            {
                return " (UDP Packet)";
            }
            else if (tmlEventInfo == 56)
            {
                return " (TCP Packet)";
            }
            else if (tmlEventInfo == 0)
            {
                return " (Receive Packet)";
            }
            else
            {
                return null;
            }
        }

        public string FindGetOption(uint tmlEventInfo)
        {
            if (tmlEventInfo == 0)
            {
                return " (TX_OR)";
            }
            else if (tmlEventInfo == 1)
            {
                return " (TX_OR_CLEAR)";
            }
            else if (tmlEventInfo == 2)
            {
                return " (TX_AND)";
            }
            else if (tmlEventInfo == 3)
            {
                return " (TX_AND_CLEAR)";
            }
            else
            {
                return null;
            }
        }

        public string FindPacketStatus(uint tmlEventInfo)
        {
            if (tmlEventInfo == 0xFFFFFFFF)
            {
                return " (FREE)";
            }
            else if (tmlEventInfo == 0xAAAAAAAA)
            {
                return " (ALLOCATED)";
            }
            else
            {
                return " (QUEUED)";
            }
        }

        public string FindWaitOption(uint tmlEventInfo)
        {
            if (tmlEventInfo == 0x00000000)
            {
                return " (TX_NO_WAIT)";
            }
            else if (tmlEventInfo == 0xFFFFFFFF)
            {
                return " (TX_WAIT_FOREVER)";
            }
            else
            {
                return null;
            }
        }

        public string FindInheritOption(uint tmlEventInfo)
        {
            if (tmlEventInfo == 0x01)
            {
                return " (TX_INHERIT)";
            }
            else if (tmlEventInfo == 0x00)
            {
                return " (TX_NO_INHERIT)";
            }
            else
            {
                return null;
            }
        }

        public string FindAutoActivateOption(uint tmlEventInfo)
        {
            if (tmlEventInfo == 0x01)
            {
                return " (TX_AUTO_ACTIVATE)";
            }
            else if (tmlEventInfo == 0x00)
            {
                return " (TX_NO_ACTIVATE)";
            }
            else
            {
                return null;
            }
        }

        public int FindNextContextIndex(TmlEvent tmlEvent)
        {
            if (tmlEvent.NextContext == 0)
                return 1;

            for (int i = 0; i < Threads.Count; i++)
            {
                TmlThread tmlThrd = Threads[i];
                if (tmlThrd.Address == tmlEvent.NextContext)
                {
                    return i;
                }
            }
            return -1;
        }

        public string FindThreadName(TmlEvent tmlEvent)
        {
            int index = FindThreadIndex(tmlEvent);
            if (index < 0)
                return string.Empty;
            return Threads[index].Name;
        }

        public string FindUXErrorType(uint tmlEventInfo)
        {
            switch (tmlEventInfo)
            {
                case 0xFF:
                    return " (UX_ERROR)";

                case 0x11:
                    return " (UX_TOO_MANY_DEVICES)";

                case 0x12:
                    return " (UX_MEMORY_INSUFFICIENT)";

                case 0x13:
                    return " (UX_NO_TD_AVAILABLE)";

                case 0x14:
                    return " (UX_NO_ED_AVAILABLE)";

                case 0x15:
                    return " (UX_SEMAPHORE_ERROR)";

                case 0x16:
                    return " (UX_THREAD_ERROR)";

                case 0x17:
                    return " (UX_MUTEX_ERROR)";

                case 0x18:
                    return " (UX_EVENT_ERROR)";

                case 0x19:
                    return " (UX_MEMORY_CORRUPTED)";

                case 0x1a:
                    return " (UX_MEMORY_ARRAY_FULL)";

                case 0x21:
                    return " (UX_TRANSFER_STALLED)";

                case 0x22:
                    return " (UX_TRANSFER_NO_ANSWER)";

                case 0x23:
                    return " (UX_TRANSFER_ERROR)";

                case 0x24:
                    return " (UX_TRANSFER_MISSED_FRAME)";

                case 0x25:
                    return " (UX_TRANSFER_NOT_READY)";

                case 0x26:
                    return " (UX_TRANSFER_BUS_RESET)";

                case 0x27:
                    return " (UX_TRANSFER_BUFFER_OVERFLOW)";

                case 0x28:
                    return " (UX_TRANSFER_APPLICATION_RESET)";

                case 0x31:
                    return " (UX_PORT_RESET_FAILED)";

                case 0x32:
                    return " (UX_CONTROLLER_INIT_FAILED)";

                case 0x41:
                    return " (UX_NO_BANDWIDTH_AVAILABLE)";

                case 0x42:
                    return " (UX_DESCRIPTOR_CORRUPTED)";

                case 0x43:
                    return " (UX_OVER_CURRENT_CONDITION)";

                case 0x44:
                    return " (UX_DEVICE_ENUMERATION_FAILURE)";

                case 0x50:
                    return " (UX_DEVICE_HANDLE_UNKNOWN)";

                case 0x51:
                    return " (UX_CONFIGURATION_HANDLE_UNKNOWN)";

                case 0x52:
                    return " (UX_INTERFACE_HANDLE_UNKNOWN)";

                case 0x53:
                    return " (UX_ENDPOINT_HANDLE_UNKNOWN)";

                case 0x54:
                    return " (UX_FUNCTION_NOT_SUPPORTED)";

                case 0x55:
                    return " (UX_CONTROLLER_UNKNOWN)";

                case 0x56:
                    return " (UX_PORT_INDEX_UNKNOWN)";

                case 0x57:
                    return " (UX_NO_CLASS_MATCH)";

                case 0x58:
                    return " (UX_HOST_CLASS_ALREADY_INSTALLED)";

                case 0x59:
                    return " (UX_HOST_CLASS_UNKNOWN)";

                case 0x5a:
                    return " (UX_CONNECTION_INCOMPATIBLE)";

                case 0x5b:
                    return " (UX_HOST_CLASS_INSTANCE_UNKNOWN)";

                case 0x5c:
                    return " (UX_TRANSFER_TIMEOUT)";

                case 0x5d:
                    return " (UX_BUFFER_OVERFLOW)";

                case 0x5e:
                    return " (UX_NO_ALTERNATE_SETTING)";

                case 0x5f:
                    return " (UX_NO_DEVICE_CONNECTED)";

                case 0x60:
                    return " (UX_HOST_CLASS_PROTOCOL_ERROR)";

                case 0x61:
                    return " (UX_HOST_CLASS_MEMORY_ERROR)";

                case 0x62:
                    return " (UX_HOST_CLASS_MEDIA_NOT_SUPPORTED)";

                case 0x70:
                    return " (UX_HOST_CLASS_HID_REPORT_OVERFLOW)";

                case 0x71:
                    return " (UX_HOST_CLASS_HID_USAGE_OVERFLOW)";

                case 0x72:
                    return " (UX_HOST_CLASS_HID_TAG_UNSUPPORTED)";

                case 0x73:
                    return " (UX_HOST_CLASS_HID_PUSH_OVERFLOW)";

                case 0x74:
                    return " (UX_HOST_CLASS_HID_POP_UNDERFLOW)";

                case 0x75:
                    return " (UX_HOST_CLASS_HID_COLLECTION_OVERFLOW)";

                case 0x76:
                    return " (UX_HOST_CLASS_HID_COLLECTION_UNDERFLOW)";

                case 0x77:
                    return " (UX_HOST_CLASS_HID_MIN_MAX_ERROR)";

                case 0x78:
                    return " (UX_HOST_CLASS_HID_DELIMITER_ERROR)";

                case 0x79:
                    return " (UX_HOST_CLASS_HID_REPORT_ERROR)";

                case 0x7a:
                    return " (UX_HOST_CLASS_HID_PERIODIC_REPORT_ERROR)";

                case 0x7b:
                    return " (UX_HOST_CLASS_HID_UNKNOWN)";

                case 0x80:
                    return " (UX_HOST_CLASS_AUDIO_WRONG_TYPE)";

                case 0x81:
                    return " (UX_HOST_CLASS_AUDIO_WRONG_INTERFACE)";

                default:
                    return null;
            }
        }

        public string GetThreadDetailString(TmlThread thread)
        {
            string s = string.Empty;
            double percent = 0.0;
            double percentUsed;

            if (thread.Address == 0xFFFFFFFF)
            {
                s += string.Format(CultureInfo.CurrentCulture, "Thread ID:  {0}\r\n", "Interrupt");
            }
            else if (thread.Address == 0xF0F0F0F0)
            {
                s += string.Format(CultureInfo.CurrentCulture, "Thread ID:  {0}\r\n", "Initialize/Idle");
            }
            else
            {
                s += string.Format(CultureInfo.CurrentCulture, "Thread ID:  0x{0:X8}\r\n", thread.Address);  //the thread ID is it's address.
            }
            s += string.Format(CultureInfo.CurrentCulture, "Thread Name:  {0}\r\n", thread.Name);
            s += string.Format(CultureInfo.CurrentCulture, "Stack Starting Address:  0x{0:X8}\r\n", thread.Parameter1);  //parameter1 has stack start.
            s += string.Format(CultureInfo.CurrentCulture, "Stack Ending Address:   0x{0:X8}\r\n", thread.Parameter2);   //parameter2 has stack end.
            s += string.Format(CultureInfo.CurrentCulture, "Stack Size:  {0}\r\n", thread.StackSize);

            if (thread.StackSize != 0)
            {
                percentUsed = ((double)(thread.StackSize - thread.Availability) / (double)thread.StackSize);
            }
            else
            {
                percentUsed = 0;
            }
            s += string.Format(CultureInfo.CurrentCulture, "Stack Used:  {0}\r\n", percentUsed.ToString("0.##%", CultureInfo.CurrentCulture));

            if (TmlTraceInfo.MaxRelativeTicks > 0)
            {
                percent = thread.Usage / TmlTraceInfo.MaxRelativeTicks;
            }

            if (thread.Address == 0xFFFFFFFF)
            {
                percent = (double)ELTMLManaged.TMLFunctions.InterruptIdle.InterruptUsage / TmlTraceInfo.MaxRelativeTicks;
            }
            else if (thread.Address == 0xF0F0F0F0)
            {
                percent = ((double)ELTMLManaged.TMLFunctions.InterruptIdle.InitializeUsage + (double)ELTMLManaged.TMLFunctions.InterruptIdle.IdleUsage) / TmlTraceInfo.MaxRelativeTicks;
                //percent = (double)ELTMLManaged.TMLFunctions.InterruptIdle.idleUsage / tmlTraceInfo.MaxRelativeTicks;
            }

            s += string.Format(CultureInfo.CurrentCulture, "Execution time:  {0}\r\n", percent.ToString("0.##%", CultureInfo.CurrentCulture));

            if ((thread.Address != 0xFFFFFFFF) && (thread.Address != 0xF0F0F0F0))
            {
                s += string.Format(CultureInfo.CurrentCulture, "Suspensions:   {0}\r\n", thread.Suspensions);
                s += string.Format(CultureInfo.CurrentCulture, "Resumptions:   {0}\r\n", thread.Resumptions);

                if ((thread.LowestPriority == 0xffffffff) && (thread.HighestPriority == 0xffffffff))
                {
                    s += string.Format(CultureInfo.CurrentCulture, "Highest Priority:   {0}\r\n", "unknown");
                    s += string.Format(CultureInfo.CurrentCulture, "Lowest Priority:   {0}\r\n", "unknown");
                }
                else
                {
                    s += string.Format(CultureInfo.CurrentCulture, "Highest Priority:   {0}\r\n", thread.HighestPriority);
                    s += string.Format(CultureInfo.CurrentCulture, "Lowest Priority:   {0}\r\n", thread.LowestPriority);
                }
            }
            return s;
        }

        public void SortThreads(int threadSortingType)
        {
            IComparer<TmlThread> threadComparer = null;
            switch (threadSortingType)
            {
                case ThreadSorting.ByCreationOrder:
                    threadComparer = new ThreadCreationOrderComparer();
                    break;

                case ThreadSorting.ByAlphabetic:
                    threadComparer = new ThreadNameComparer();
                    break;

                case ThreadSorting.ByExecutionTime:
                    threadComparer = new ThreadExecutionTimeComparer();
                    break;

                case ThreadSorting.ByLowestPriority:
                    threadComparer = new ThreadLowestPriorityComparer();
                    break;

                case ThreadSorting.ByHighestPriority:
                    threadComparer = new ThreadHighestPriorityComparer();
                    break;

                case ThreadSorting.ByMostEvents:
                    threadComparer = new ThreadHighestNumberOfEventsComparer();
                    break;

                case ThreadSorting.ByLeastEvents:
                    threadComparer = new ThreadLowestNumberOfEventsComparer();
                    break;

                default:
                    break;
            }

            Threads.Sort(threadComparer);
        }
    }
}
