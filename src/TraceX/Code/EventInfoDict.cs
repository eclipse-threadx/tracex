using System.Windows.Media;
using System.Collections.Generic;
using System.Globalization;
using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement.Code
{
    public static class EventInfoDict
    {
        public static Dictionary<uint, TmlEventInfo> eventInfoDict = new Dictionary<uint, TmlEventInfo>()
        {
            // I1 = thread ptr, I2 = previous_state, I3 = stack ptr, I4 = next thread
            [TmlDefines.TML_TRACE_THREAD_RESUME] = new TmlEventInfo()
            {
                IconCaption = "IR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal Thread Resume",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_PREVIOUS_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_POINTER,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEXT_THREAD_PTR
            },
            // I1 = thread ptr, I2 = new_state, I3 = stack ptr  I4 = next thread ptr
            [TmlDefines.TML_TRACE_THREAD_SUSPEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal Thread Suspend",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEXT_THREAD_PTR
            },
            // I1 = stack_ptr, I2 = ISR number (optional, user defined), I3 = system state, I4 = current thread
            [TmlDefines.TML_TRACE_ISR_ENTER] = new TmlEventInfo()
            {
                IconCaption = "IE",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Enter ISR",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ISR_NUMBER,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SYSTEM_STATE,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_PREEMPT_DISABLE
            },
            // I1 = stack_ptr, I2 = ISR number (optional, user defined), I3 = system state, I4 = preempt disable
            [TmlDefines.TML_TRACE_ISR_EXIT] = new TmlEventInfo()
            {
                IconCaption = "IX",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Exit ISR",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ISR_NUMBER,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SYSTEM_STATE,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_PREEMPT_DISABLE
            },
            // I1 = next thread ptr, I2 = system state, I3 = preempt disable, I4 = stack
            [TmlDefines.TML_TRACE_TIME_SLICE] = new TmlEventInfo()
            {
                IconCaption = "TS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal Time-Slice",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEXT_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SYSTEM_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_PREEMPT_DISABLE,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK
            },
            [TmlDefines.TML_TRACE_RUNNING] = new TmlEventInfo()
            {
                IconCaption = "R",
                ColorBackground = Color.FromArgb(255, 0, 220, 5),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Running",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = memory ptr, I3 = wait option, I4 = remaining blocks
            [TmlDefines.TML_TRACE_BLOCK_ALLOCATE] = new TmlEventInfo()
            {
                IconCaption = "BA",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_allocate",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MEMORY_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_REMAINING_BLOCKS
            },
            // I1 = pool ptr, I2 = pool_start, I3 = total blocks, I4 = block size
            [TmlDefines.TML_TRACE_BLOCK_POOL_CREATE] = new TmlEventInfo()
            {
                IconCaption = "PC",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_START,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TOTAL_BLOCKS,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_BLOCK_SIZE
            },
            // I1 = pool ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_BLOCK_POOL_DELETE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr
            [TmlDefines.TML_TRACE_BLOCK_POOL_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr
            [TmlDefines.TML_TRACE_BLOCK_POOL_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_BLOCK_POOL_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = suspended count, I3 = stack ptr
            [TmlDefines.TML_TRACE_BLOCK_POOL_PRIORITIZE] = new TmlEventInfo()
            {
                IconCaption = "PP",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_pool_prioritize",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = stack ptr
            [TmlDefines.TML_TRACE_BLOCK_RELEASE] = new TmlEventInfo()
            {
                IconCaption = "BR",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_block_release",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MEMORY_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR
            },
            // I1 = pool ptr, I2 = memory ptr, I3 = size requested, I4 = wait option
            [TmlDefines.TML_TRACE_BYTE_ALLOCATE] = new TmlEventInfo()
            {
                IconCaption = "BA",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_allocate",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MEMORY_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SIZE_REQUESTED,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION
            },
            // I1 = pool ptr, I2 = start ptr, I3 = pool size, I4 = stack ptr
            [TmlDefines.TML_TRACE_BYTE_POOL_CREATE] = new TmlEventInfo()
            {
                IconCaption = "PC",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_START_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_SIZE,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR
            },
            // I1 = pool ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_BYTE_POOL_DELETE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_START_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr
            [TmlDefines.TML_TRACE_BYTE_POOL_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr
            [TmlDefines.TML_TRACE_BYTE_POOL_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_BYTE_POOL_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = suspended count, I3 = stack ptr
            [TmlDefines.TML_TRACE_BYTE_POOL_PRIORITIZE] = new TmlEventInfo()
            {
                IconCaption = "PP",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_pool_prioritize",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = available bytes
            [TmlDefines.TML_TRACE_BYTE_RELEASE] = new TmlEventInfo()
            {
                IconCaption = "BR",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_byte_release",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MEMORY_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_AVAILABLE_BYTES
            },
            // I1 = group ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_EVENT_FLAGS_CREATE] = new TmlEventInfo()
            {
                IconCaption = "EC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = group ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_EVENT_FLAGS_DELETE] = new TmlEventInfo()
            {
                IconCaption = "ED",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = group ptr, I2 = requested flags, I3 = current flags, I4 = get option
            [TmlDefines.TML_TRACE_EVENT_FLAGS_GET] = new TmlEventInfo()
            {
                IconCaption = "EG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_REQUESTED_FLAGS,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_FLAGS,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GET_OPTION
            },
            // I1 = group ptr
            [TmlDefines.TML_TRACE_EVENT_FLAGS_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = group ptr
            [TmlDefines.TML_TRACE_EVENT_FLAGS_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_EVENT_FLAGS_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = group ptr, I2 = flags to set, I3 = set option, I4= suspended count
            [TmlDefines.TML_TRACE_EVENT_FLAGS_SET] = new TmlEventInfo()
            {
                IconCaption = "ES",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_set",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_FLAGS_TO_SET,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SET_OPTION,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT
            },
            // I1 = group ptr
            [TmlDefines.TML_TRACE_EVENT_FLAGS_SET_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "EN",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_event_flags_set_notify",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = new interrupt posture, I2 = stack ptr
            [TmlDefines.TML_TRACE_INTERRUPT_CONTROL] = new TmlEventInfo()
            {
                IconCaption = "IC",
                ColorBackground = Color.FromArgb(255, 145, 70, 30),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_interrupt_control",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_INTERRUPT_POSTURE,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr, I2 = inheritance, I3 = stack ptr
            [TmlDefines.TML_TRACE_MUTEX_CREATE] = new TmlEventInfo()
            {
                IconCaption = "MC",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_INHERITANCE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_MUTEX_DELETE] = new TmlEventInfo()
            {
                IconCaption = "MD",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr, I2 = wait option, I3 = owning thread, I4 = own count
            [TmlDefines.TML_TRACE_MUTEX_GET] = new TmlEventInfo()
            {
                IconCaption = "MG",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OWNING_THREAD,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OWN_COUNT
            },
            // I1 = mutex ptr
            [TmlDefines.TML_TRACE_MUTEX_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr
            [TmlDefines.TML_TRACE_MUTEX_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_MUTEX_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr, I2 = suspended count, I3 = stack ptr
            [TmlDefines.TML_TRACE_MUTEX_PRIORITIZE] = new TmlEventInfo()
            {
                IconCaption = "XP",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_prioritize",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = mutex ptr, I2 = owning thread, I3 = own count, I4 = stack ptr
            [TmlDefines.TML_TRACE_MUTEX_PUT] = new TmlEventInfo()
            {
                IconCaption = "MP",
                ColorBackground = Color.FromArgb(255, 115, 10, 80),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_mutex_put",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OWNING_THREAD,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OWN_COUNT,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR
            },
            // I1 = queue ptr, I2 = message size, I3 = queue start, I4 = queue size
            [TmlDefines.TML_TRACE_QUEUE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "QC",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_MESSAGE_SIZE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_START,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_SIZE
            },
            // I1 = queue ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_QUEUE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "QD",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = queue ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_QUEUE_FLUSH] = new TmlEventInfo()
            {
                IconCaption = "QF",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_flush",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued
            [TmlDefines.TML_TRACE_QUEUE_FRONT_SEND] = new TmlEventInfo()
            {
                IconCaption = "FS",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_front_send",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SOURCE_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ENQUEUED
            },
            // I1 = queue ptr
            [TmlDefines.TML_TRACE_QUEUE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = queue ptr
            [TmlDefines.TML_TRACE_QUEUE_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_QUEUE_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = queue ptr, I2 = suspended count, I3 = stack ptr
            [TmlDefines.TML_TRACE_QUEUE_PRIORITIZE] = new TmlEventInfo()
            {
                IconCaption = "QP",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_prioritize",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = queue ptr, I2 = destination ptr, I3 = wait option, I4 = enqueued
            [TmlDefines.TML_TRACE_QUEUE_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "QR",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_receive",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_DESTINATION_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ENQUEUED
            },
            // I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued
            [TmlDefines.TML_TRACE_QUEUE_SEND] = new TmlEventInfo()
            {
                IconCaption = "QS",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_send",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SOURCE_PTR,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ENQUEUED
            },
            // I1 = queue ptr
            [TmlDefines.TML_TRACE_QUEUE_SEND_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "SN",
                ColorBackground = Color.FromArgb(255, 0, 25, 150),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_queue_send_notify",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4 =ceiling
            [TmlDefines.TML_TRACE_SEMAPHORE_CEILING_PUT] = new TmlEventInfo()
            {
                IconCaption = "CP",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_ceiling_put",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CEILING
            },
            // I1 = semaphore ptr, I2 = initial count, I3 = stack ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_INITIAL_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr, I2 = wait option, I3 = current count, I4 = stack ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_COUNT,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR
            },
            // I1 = semaphore ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_SEMAPHORE_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr, I2 = suspended count, I2 = stack ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_PRIORITIZE] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_prioritize",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4=stack ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_PUT] = new TmlEventInfo()
            {
                IconCaption = "SP",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_put",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_COUNT,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SUSPENDED_COUNT,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR
            },
            // I1 = semaphore ptr
            [TmlDefines.TML_TRACE_SEMAPHORE_PUT_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "SN",
                ColorBackground = Color.FromArgb(255, 61, 197, 175),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_semaphore_put_notify",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = priority, I3 = stack ptr, I4 = stack_size
            [TmlDefines.TML_TRACE_THREAD_CREATE] = new TmlEventInfo()
            {
                IconCaption = "TC",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_PRIORITY,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_SIZE
            },
            // I1 = thread ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_DELETE] = new TmlEventInfo()
            {
                IconCaption = "TD",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_ENTRY_EXIT_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "TE",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_entry_exit_notify",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_THREAD_IDENTIFY] = new TmlEventInfo()
            {
                IconCaption = "TI",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_identify",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state
            [TmlDefines.TML_TRACE_THREAD_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state
            [TmlDefines.TML_TRACE_THREAD_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_THREAD_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = new threshold, I3 = old threshold, I4 =thread state
            [TmlDefines.TML_TRACE_THREAD_PREEMPTION_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "CT",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_preemption_change",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_THRESHOLD,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OLD_THRESHOLD,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE
            },
            // I1 = thread ptr, I2 = new priority, I3 = old priority, I4 = thread state
            [TmlDefines.TML_TRACE_THREAD_PRIORITY_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "CP",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_priority_change",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_PRIORITY,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OLD_PRIORITY,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE
            },
            // I1 = stack ptr, I2 = next thread ptr
            [TmlDefines.TML_TRACE_THREAD_RELINQUISH] = new TmlEventInfo()
            {
                IconCaption = "RQ",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_relinquish",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEXT_THREAD_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state
            [TmlDefines.TML_TRACE_THREAD_RESET] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_reset",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_RESUME_API] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_resume",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = sleep value, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_SLEEP] = new TmlEventInfo()
            {
                IconCaption = "TZ",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_sleep",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_SLEEP_VALUE,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_THREAD_STACK_ERROR_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "SE",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_stack_error_notify",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_SUSPEND_API] = new TmlEventInfo()
            {
                IconCaption = "TS",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_suspend",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_TERMINATE] = new TmlEventInfo()
            {
                IconCaption = "TT",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_terminate",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = new timeslice, I3 = old timeslice
            [TmlDefines.TML_TRACE_THREAD_TIME_SLICE_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "CS",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_time_slice_change",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_TIMESLICE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_OLD_TIMESLICE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = thread ptr, I2 = thread state, I3 = stack ptr
            [TmlDefines.TML_TRACE_THREAD_WAIT_ABORT] = new TmlEventInfo()
            {
                IconCaption = "WA",
                ColorBackground = Color.FromArgb(255, 255, 255, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "tx_thread_wait_abort",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = current time, I2 = stack ptr
            [TmlDefines.TML_TRACE_TIME_GET] = new TmlEventInfo()
            {
                IconCaption = "TG",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_time_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_TIME,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = new time
            [TmlDefines.TML_TRACE_TIME_SET] = new TmlEventInfo()
            {
                IconCaption = "TS",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_time_set",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_TIME,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr
            [TmlDefines.TML_TRACE_TIMER_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "TA",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_activate",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks
            [TmlDefines.TML_TRACE_TIMER_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "TC",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_change",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_INITIAL_TICKS,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_RESCHEDULE_TICKS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks, I4 = enable
            [TmlDefines.TML_TRACE_TIMER_CREATE] = new TmlEventInfo()
            {
                IconCaption = "CR",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_create",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_INITIAL_TICKS,
                Info3Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_RESCHEDULE_TICKS,
                Info4Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_ENABLE
            },
            // I1 = timer ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_TIMER_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "TD",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_deactivate",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr
            [TmlDefines.TML_TRACE_TIMER_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DE",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_delete",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr, I2 = stack ptr
            [TmlDefines.TML_TRACE_TIMER_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = timer ptr
            [TmlDefines.TML_TRACE_TIMER_PERFORMANCE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_performance_info_get",
                Info1Type = (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_TRACE_TIMER_PERFORMANCE_SYSTEM_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 64, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 0, 0, 0),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "tx_timer_performance_system_info_get",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = media ptr, I2 = sector, I3 = total misses, I4 = cache size
            [TmlDefines.TML_FX_TRACE_INTERNAL_LOG_SECTOR_CACHE_MISS] = new TmlEventInfo()
            {
                IconCaption = "LM",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal FileX Sector Cache Miss",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_TOTAL_MISSES,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_CACHE_SIZE
            },
            // I1 = media ptr, I2 = total misses
            [TmlDefines.TML_FX_TRACE_INTERNAL_DIR_CACHE_MISS] = new TmlEventInfo()
            {
                IconCaption = "DM",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal FileX Dir Cache Miss",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_TOTAL_MISSES,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = media ptr, I2 = dirty sectors
            [TmlDefines.TML_FX_TRACE_INTERNAL_MEDIA_FLUSH] = new TmlEventInfo()
            {
                IconCaption = "MF",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal FileX Media Flush",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRTY_SECTORS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_DIR_ENTRY_READ] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal FileX Dir Entry Read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_DIR_ENTRY_WRITE] = new TmlEventInfo()
            {
                IconCaption = "DW",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal FileX Dir Entry Write",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_READ] = new TmlEventInfo()
            {
                IconCaption = "IR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NUMBER_OF_SECTORS,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER
            },
            //  I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_WRITE] = new TmlEventInfo()
            {
                IconCaption = "IW",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Write",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NUMBER_OF_SECTORS,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_FLUSH] = new TmlEventInfo()
            {
                IconCaption = "IF",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Flush",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_ABORT] = new TmlEventInfo()
            {
                IconCaption = "IA",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Abort",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_INIT] = new TmlEventInfo()
            {
                IconCaption = "II",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Init",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = buffer
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_BOOT_READ] = new TmlEventInfo()
            {
                IconCaption = "BR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Boot Read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = sector, I3 = number of sectors
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_RELEASE_SECTORS] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Release Sectors",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NUMBER_OF_SECTORS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = buffer
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_BOOT_WRITE] = new TmlEventInfo()
            {
                IconCaption = "BW",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Boot Write",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_INTERNAL_IO_DRIVER_UNINIT] = new TmlEventInfo()
            {
                IconCaption = "DU",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "IO Internal FileX Driver Uninit",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name, I3 = attributes
            [TmlDefines.TML_FX_TRACE_DIRECTORY_ATTRIBUTES_READ] = new TmlEventInfo()
            {
                IconCaption = "AR",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_attributes_read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ATTRIBUTES,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name, I3 = attributes
            [TmlDefines.TML_FX_TRACE_DIRECTORY_ATTRIBUTES_SET] = new TmlEventInfo()
            {
                IconCaption = "AS",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_attributes_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ATTRIBUTES,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_CREATE] = new TmlEventInfo()
            {
                IconCaption = "DC",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_create",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = return path name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_DEFAULT_GET] = new TmlEventInfo()
            {
                IconCaption = "DG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_default_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_RETURN_PATH_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = new path name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_DEFAULT_SET] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_default_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_PATH_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DD",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_delete",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_FIRST_ENTRY_FIND] = new TmlEventInfo()
            {
                IconCaption = "DF",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_first_entry_find",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_FIRST_FULL_ENTRY_FIND] = new TmlEventInfo()
            {
                IconCaption = "FF",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_first_full_entry_find",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_INFORMATION_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_information_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_DIRECTORY_LOCAL_PATH_CLEAR] = new TmlEventInfo()
            {
                IconCaption = "LC",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_local_path_clear",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = return path name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_LOCAL_PATH_GET] = new TmlEventInfo()
            {
                IconCaption = "LG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_local_path_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_RETURN_PATH_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = local path ptr
            [TmlDefines.TML_FX_TRACE_DIRECTORY_LOCAL_PATH_RESTORE] = new TmlEventInfo()
            {
                IconCaption = "LR",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_local_path_restore",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LOCAL_PATH_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = local path ptr, I3 = new path name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_LOCAL_PATH_SET] = new TmlEventInfo()
            {
                IconCaption = "LS",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_local_path_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LOCAL_PATH_PTR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_PATH_NAME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = short file name, I3 = long file name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_LONG_NAME_GET] = new TmlEventInfo()
            {
                IconCaption = "NG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_long_name_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SHORT_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LONG_FILE_NAME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_NAME_TEST] = new TmlEventInfo()
            {
                IconCaption = "NT",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_name_test",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_NEXT_ENTRY_FIND] = new TmlEventInfo()
            {
                IconCaption = "NE",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_next_entry_find",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_NEXT_FULL_ENTRY_FIND] = new TmlEventInfo()
            {
                IconCaption = "NF",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_next_full_entry_find",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = old directory name, I3 = new directory name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_RENAME] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_rename",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_OLD_DIRECTORY_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_DIRECTORY_NAME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = long file name, I3 = short file name
            [TmlDefines.TML_FX_TRACE_DIRECTORY_SHORT_NAME_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_directory_short_name_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LONG_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SHORT_FILE_NAME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = file ptr, I2 = size I3 = previous size, I4 = new size
            [TmlDefines.TML_FX_TRACE_FILE_ALLOCATE] = new TmlEventInfo()
            {
                IconCaption = "FA",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_allocate",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SIZE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_PREVIOUS_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_SIZE
            },
            //  I1 = media ptr, I2 = file name, I3 = attributes
            [TmlDefines.TML_FX_TRACE_FILE_ATTRIBUTES_READ] = new TmlEventInfo()
            {
                IconCaption = "RF",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_attributes_read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ATTRIBUTES,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = file name, I3 = attributes
            [TmlDefines.TML_FX_TRACE_FILE_ATTRIBUTES_SET] = new TmlEventInfo()
            {
                IconCaption = "SF",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_attributes_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ATTRIBUTES,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = file ptr, I2 = size, I3 = actual_size_allocated
            [TmlDefines.TML_FX_TRACE_FILE_BEST_EFFORT_ALLOCATE] = new TmlEventInfo()
            {
                IconCaption = "BA",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_best_effort_allocate",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SIZE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ACTUAL_SIZE_ALLOCATED,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = file ptr, I2 = file size
            [TmlDefines.TML_FX_TRACE_FILE_CLOSE] = new TmlEventInfo()
            {
                IconCaption = "FC",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_close",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_SIZE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = file name
            [TmlDefines.TML_FX_TRACE_FILE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "CR",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_create",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = file name, I3 = year, I4 = month
            [TmlDefines.TML_FX_TRACE_FILE_DATE_TIME_SET] = new TmlEventInfo()
            {
                IconCaption = "TS",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_date_time_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_YEAR,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MONTH
            },
            //  I1 = media ptr, I2 = file name
            [TmlDefines.TML_FX_TRACE_FILE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "FD",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_delete",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = file ptr, I3 = file name, I4 = open type
            [TmlDefines.TML_FX_TRACE_FILE_OPEN] = new TmlEventInfo()
            {
                IconCaption = "FO",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_open",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_NAME,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_OPEN_TYPE
            },
            //  I1 = file ptr, I2 = buffer ptr, I3 = request size I4 = actual size
            [TmlDefines.TML_FX_TRACE_FILE_READ] = new TmlEventInfo()
            {
                IconCaption = "FR",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_REQUEST_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ACTUAL_SIZE
            },
            //  I1 = file ptr, I2 = byte offset, I3 = seek from, I4 = previous offset
            [TmlDefines.TML_FX_TRACE_FILE_RELATIVE_SEEK] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_relative_seek",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BYTE_OFFSET,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SEEK_FROM,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_PREVIOUS_OFFSET
            },
            //  I1 = media ptr, I2 = old file name, I3 = new file name
            [TmlDefines.TML_FX_TRACE_FILE_RENAME] = new TmlEventInfo()
            {
                IconCaption = "RN",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_rename",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_OLD_FILE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_FILE_NAME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = file ptr, I2 = byte offset, I3 = previous offset
            [TmlDefines.TML_FX_TRACE_FILE_SEEK] = new TmlEventInfo()
            {
                IconCaption = "SK",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_seek",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BYTE_OFFSET,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_PREVIOUS_OFFSET,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = file ptr, I2 = size, I3 = previous size, I4 = new size
            [TmlDefines.TML_FX_TRACE_FILE_TRUNCATE] = new TmlEventInfo()
            {
                IconCaption = "FT",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_truncate",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SIZE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_PREVIOUS_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_SIZE
            },
            //  I1 = file ptr, I2 = size, I3 = previous size, I4 = new size
            [TmlDefines.TML_FX_TRACE_FILE_TRUNCATE_RELEASE] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_truncate_release",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SIZE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_PREVIOUS_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_SIZE
            },
            //  I1 = file ptr, I2 = buffer ptr, I3 = size, I4 = bytes written
            [TmlDefines.TML_FX_TRACE_FILE_WRITE] = new TmlEventInfo()
            {
                IconCaption = "FW",
                ColorBackground = Color.FromArgb(255, 0, 185, 0),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_file_write",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BYTES_WRITTEN
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_MEDIA_ABORT] = new TmlEventInfo()
            {
                IconCaption = "MA",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_abort",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_MEDIA_CACHE_INVALIDATE] = new TmlEventInfo()
            {
                IconCaption = "CI",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_cache_invalidate",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = scratch memory, I3 = scratch memory size, I4 =errors
            [TmlDefines.TML_FX_TRACE_MEDIA_CHECK] = new TmlEventInfo()
            {
                IconCaption = "CK",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_check",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SCRATCH_MEMORY,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SCRATCH_MEMORY_SIZE,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ERRORS
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_MEDIA_CLOSE] = new TmlEventInfo()
            {
                IconCaption = "MC",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_close",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr
            [TmlDefines.TML_FX_TRACE_MEDIA_FLUSH] = new TmlEventInfo()
            {
                IconCaption = "FL",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_flush",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = root entries, I3 = sectors, I4 = sectors per cluster
            [TmlDefines.TML_FX_TRACE_MEDIA_FORMAT] = new TmlEventInfo()
            {
                IconCaption = "MF",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_format",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_ROOT_ENTRIES,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTORS,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECTORS_PER_CLUSTER
            },
            //  I1 = media ptr, I2 = media driver, I3 = memory ptr, I4 = memory size
            [TmlDefines.TML_FX_TRACE_MEDIA_OPEN] = new TmlEventInfo()
            {
                IconCaption = "MO",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_open",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_DRIVER,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEMORY_PTR,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEMORY_SIZE
            },
            //  I1 = media ptr, I2 = logical sector, I3 = buffer ptr, I4 = bytes read
            [TmlDefines.TML_FX_TRACE_MEDIA_READ] = new TmlEventInfo()
            {
                IconCaption = "MR",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_read",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LOGICAL_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BYTES_READ
            },
            //  I1 = media ptr, I2 = available bytes ptr, I3 = available clusters
            [TmlDefines.TML_FX_TRACE_MEDIA_SPACE_AVAILABLE] = new TmlEventInfo()
            {
                IconCaption = "SA",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_space_available",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_AVAILABLE_BYTES_PTR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_AVAILABLE_CLUSTERS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = volume name, I3 = volume source
            [TmlDefines.TML_FX_TRACE_MEDIA_VOLUME_GET] = new TmlEventInfo()
            {
                IconCaption = "VG",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_volume_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_VOLUME_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_VOLUME_SOURCE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = volume name
            [TmlDefines.TML_FX_TRACE_MEDIA_VOLUME_SET] = new TmlEventInfo()
            {
                IconCaption = "VS",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_volume_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_VOLUME_NAME,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = logical_sector, I3 = buffer_ptr, I4 = byte written
            [TmlDefines.TML_FX_TRACE_MEDIA_WRITE] = new TmlEventInfo()
            {
                IconCaption = "MW",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_media_write",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LOGICAL_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BYTE_WRITTEN
            },
            //  I1 = year, I2 = month, I3 = day
            [TmlDefines.TML_FX_TRACE_SYSTEM_DATE_GET] = new TmlEventInfo()
            {
                IconCaption = "DG",
                ColorBackground = Color.FromArgb(255, 200, 50, 60),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_system_date_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LOGICAL_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = year, I2 = month, I3 = day
            [TmlDefines.TML_FX_TRACE_SYSTEM_DATE_SET] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 200, 50, 60),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_system_date_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_YEAR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MONTH,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_DAY,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // None
            [TmlDefines.TML_FX_TRACE_SYSTEM_INITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 200, 50, 60),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_system_initialize",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = hour, I2 = minute, I3 = second
            [TmlDefines.TML_FX_TRACE_SYSTEM_TIME_GET] = new TmlEventInfo()
            {
                IconCaption = "TG",
                ColorBackground = Color.FromArgb(255, 200, 50, 60),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_system_time_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_HOUR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MINUTE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECOND,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = hour, I2 = minute, I3 = second
            [TmlDefines.TML_FX_TRACE_SYSTEM_TIME_SET] = new TmlEventInfo()
            {
                IconCaption = "TS",
                ColorBackground = Color.FromArgb(255, 200, 50, 60),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "fx_system_time_set",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_HOUR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MINUTE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SECOND,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short_name
            [TmlDefines.TML_FX_TRACE_UNICODE_DIRECTORY_CREATE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_directory_create",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_UNICODE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SHORT_NAME
            },
            //  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new_name
            [TmlDefines.TML_FX_TRACE_UNICODE_DIRECTORY_RENAME] = new TmlEventInfo()
            {
                IconCaption = "RD",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_directory_rename",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_UNICODE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_NAME
            },
            //  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short name
            [TmlDefines.TML_FX_TRACE_UNICODE_FILE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "CF",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_file_create",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_UNICODE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SHORT_NAME
            },
            //  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new name
            [TmlDefines.TML_FX_TRACE_UNICODE_FILE_RENAME] = new TmlEventInfo()
            {
                IconCaption = "RF",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_file_rename",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_UNICODE,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_NEW_NAME
            },
            //  I1 = unicode name, I2 = length
            [TmlDefines.TML_FX_TRACE_UNICODE_LENGTH_GET] = new TmlEventInfo()
            {
                IconCaption = "GL",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_length_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_UNICODE_NAME,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LENGTH,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //  I1 = media ptr, I2 = source short name, I3 = unicode name, I4 = length
            [TmlDefines.TML_FX_TRACE_UNICODE_NAME_GET] = new TmlEventInfo()
            {
                IconCaption = "GN",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_name_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_SHORT_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_UNICODE_NAME,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LENGTH
            },
            //  I1 = media ptr, I2 = source unicode name, I3 = length, I4 =  short name
            [TmlDefines.TML_FX_TRACE_UNICODE_SHORT_NAME_GET] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 100, 0, 235),
                ColorForeground = Color.FromArgb(255, 255, 255, 255),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "fx_unicode_short_name_get",
                Info1Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR,
                Info2Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SOURCE_UNICODE_NAME,
                Info3Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.FX_EVENT_INFO_SHORT_NAME
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_ARP_REQUEST_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "RP",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ARP Request Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = destination IP address, I3 = packet ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_ARP_REQUEST_SEND] = new TmlEventInfo()
            {
                IconCaption = "SP",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ARP Request Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_ARP_RESPONSE_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "AR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ARP Response Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = destination IP address, I3 = packet ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_ARP_RESPONSE_SEND] = new TmlEventInfo()
            {
                IconCaption = "AS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ARP Response Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0
            [TmlDefines.TML_NX_TRACE_INTERNAL_ICMP_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "CR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ICMP Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_WORD_0
            },
            // I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = header 0
            [TmlDefines.TML_NX_TRACE_INTERNAL_ICMP_SEND] = new TmlEventInfo()
            {
                IconCaption = "CS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX ICMP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_0
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0
            [TmlDefines.TML_NX_TRACE_INTERNAL_IGMP_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "GR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IGMP Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_WORD_0
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = Packet Length
            [TmlDefines.TML_NX_TRACE_INTERNAL_IP_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "IR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IP Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_LENGTH
            },
            // I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = length
            [TmlDefines.TML_NX_TRACE_INTERNAL_IP_SEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_LENGTH
            },
            // I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_DATA_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Data Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = Socket Ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_DATA_SEND] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Data Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = Socket Ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_FIN_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "FR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Fin Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_FIN_SEND] = new TmlEventInfo()
            {
                IconCaption = "FS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Fin Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_RESET_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "RR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Reset Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_RESET_SEND] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Reset Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_SYN_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "RE",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Syn Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_SYN_SEND] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Syn Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = header word 0
            [TmlDefines.TML_NX_TRACE_INTERNAL_UDP_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "UR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX UDP Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_WORD_0
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = header 0
            [TmlDefines.TML_NX_TRACE_INTERNAL_UDP_SEND] = new TmlEventInfo()
            {
                IconCaption = "US",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX UDP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_0
            },
            // I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1
            [TmlDefines.TML_NX_TRACE_INTERNAL_RARP_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX RARP Receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TARGET_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_WORD_1
            },
            // I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1
            [TmlDefines.TML_NX_TRACE_INTERNAL_RARP_SEND] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX RARP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TARGET_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_HEADER_WORD_1
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = number of retries
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_RETRY] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP Retry",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NUMBER_OF_RETRIES
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = previous state, I4 = new state
            [TmlDefines.TML_NX_TRACE_INTERNAL_TCP_STATE_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX TCP State Change",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PREVIOUS_STATE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NEW_STATE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_PACKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "PS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Packet Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_INITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "DI",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Initialize",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_LINK_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "LE",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Link Enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_LINK_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "LD",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Link Disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_PACKET_BROADCAST] = new TmlEventInfo()
            {
                IconCaption = "PB",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Packet Broadcast",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_ARP_SEND] = new TmlEventInfo()
            {
                IconCaption = "AS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver ARP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_ARP_RESPONSE_SEND] = new TmlEventInfo()
            {
                IconCaption = "SA",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver ARP Response Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_RARP_SEND] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver RARP Send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_MULTICAST_JOIN] = new TmlEventInfo()
            {
                IconCaption = "JM",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Multicast Join",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_MULTICAST_LEAVE] = new TmlEventInfo()
            {
                IconCaption = "LM",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Multicast Leave",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_STATUS] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get Status",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_SPEED] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get Speed",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_DUPLEX_TYPE] = new TmlEventInfo()
            {
                IconCaption = "TD",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX Driver Get Duplex Type",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_ERROR_COUNT] = new TmlEventInfo()
            {
                IconCaption = "CE",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get Error Count",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_RX_COUNT] = new TmlEventInfo()
            {
                IconCaption = "RC",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get RX Count",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_TX_COUNT] = new TmlEventInfo()
            {
                IconCaption = "CT",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get TX Count",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_ALLOC_ERRORS] = new TmlEventInfo()
            {
                IconCaption = "EA",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Get Alloc Errors",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_UNINITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "UD",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Uninitialize",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = packet size
            [TmlDefines.TML_NX_TRACE_INTERNAL_IO_DRIVER_DEFERRED_PROCESSING] = new TmlEventInfo()
            {
                IconCaption = "DP",
                ColorBackground = Color.FromArgb(255, 255, 100, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "Internal NetX IO Driver Deferred Processing",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = entries invalidated
            [TmlDefines.TML_NX_TRACE_ARP_DYNAMIC_ENTRIES_INVALIDATE] = new TmlEventInfo()
            {
                IconCaption = "EI",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_dynamic_entries_invalidate",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ENTRIES_INVALIDATED,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw
            [TmlDefines.TML_NX_TRACE_ARP_DYNAMIC_ENTRY_SET] = new TmlEventInfo()
            {
                IconCaption = "ES",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_dynamic_entry_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip ptr, I2 = arp cache memory, I3 = arp cache size
            [TmlDefines.TML_NX_TRACE_ARP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "AE",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ARP_CACHE_MEMORY,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ARP_CACHE_SIZE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_ARP_GRATUITOUS_SEND] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_gratuitous_send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip_address, I3 = physical msw, I4 = physical lsw
            [TmlDefines.TML_NX_TRACE_ARP_HARDWARE_ADDRESS_FIND] = new TmlEventInfo()
            {
                IconCaption = "HF",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_hardware_address_find",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip ptr, I2 = arps sent, I3 = arp responses, I3 = arps received
            [TmlDefines.TML_NX_TRACE_ARP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "AI",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ARPS_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ARP_RESPONSES,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ARPS_RECEIVED
            },
            // I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw
            [TmlDefines.TML_NX_TRACE_ARP_IP_ADDRESS_FIND] = new TmlEventInfo()
            {
                IconCaption = "AF",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_ip_address_find",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip ptr, I2 = entries deleted
            [TmlDefines.TML_NX_TRACE_ARP_STATIC_ENTRIES_DELETE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_static_entries_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ENTRIES_DELETED,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical_lsw
            [TmlDefines.TML_NX_TRACE_ARP_STATIC_ENTRY_CREATE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_static_entries_create",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip ptr, I2 = ip address, I3 = physical_msw, I4 = physical_lsw
            [TmlDefines.TML_NX_TRACE_ARP_STATIC_ENTRY_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 50, 150, 200),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_arp_static_entry_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_ICMP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "CE",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_icmp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = pings sent, I3 = ping responses, I4 = pings received
            [TmlDefines.TML_NX_TRACE_ICMP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "CI",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_icmp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PINGS_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PING_RESPONSES,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PINGS_RECEIVED
            },
            // I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size
            [TmlDefines.TML_NX_TRACE_ICMP_PING] = new TmlEventInfo()
            {
                IconCaption = "CP",
                ColorBackground = Color.FromArgb(255, 100, 255, 235),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_icmp_ping",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_SIZE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IGMP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "GE",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = reports sent, I3 = queries received, I4 = groups joined
            [TmlDefines.TML_NX_TRACE_IGMP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "GI",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_REPORTS_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_QUERIES_RECEIVED,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_GROUPS_JOINED
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IGMP_LOOPBACK_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "LD",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_loopback_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IGMP_LOOPBACK_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "LE",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_loopback_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = group address
            [TmlDefines.TML_NX_TRACE_IGMP_MULTICAST_JOIN] = new TmlEventInfo()
            {
                IconCaption = "MJ",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_multicast_join",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_GROUP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = group_address
            [TmlDefines.TML_NX_TRACE_IGMP_MULTICAST_LEAVE] = new TmlEventInfo()
            {
                IconCaption = "ML",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_igmp_multicast_leave",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_GROUP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address change notify, I3 = additional info
            [TmlDefines.TML_NX_TRACE_IP_ADDRESS_CHANGE_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "CN",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_address_change_notify",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS_CHANGED_NOTIFY,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ADDITIONAL_INFO,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = network_mask
            [TmlDefines.TML_NX_TRACE_IP_ADDRESS_GET] = new TmlEventInfo()
            {
                IconCaption = "AG",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_address_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_MASK,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = network_mask
            [TmlDefines.TML_NX_TRACE_IP_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "AS",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_address_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_MASK,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = network mask, I4 = default_pool
            [TmlDefines.TML_NX_TRACE_IP_CREATE] = new TmlEventInfo()
            {
                IconCaption = "IC",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_create",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_MASK,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DEFAULT_POOL
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_DELETE] = new TmlEventInfo()
            {
                IconCaption = "ID",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = command, I3 = return value
            [TmlDefines.TML_NX_TRACE_IP_DRIVER_DIRECT_COMMAND] = new TmlEventInfo()
            {
                IconCaption = "DC",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_driver_direct_command",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_COMMAND,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_RETURN_VALUE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_FORWARDING_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "FD",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_forwarding_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_FORWARDING_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "FE",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_forwarding_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_FRAGMENT_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "DF",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_fragment_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_FRAGMENT_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "EF",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_fragment_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = gateway address
            [TmlDefines.TML_NX_TRACE_IP_GATEWAY_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_gateway_address_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_GATEWAY_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = packets dropped
            [TmlDefines.TML_NX_TRACE_IP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "II",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_RECEIVED,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKETS_DROPPED
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_RAW_PACKET_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "RD",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_raw_packet_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_IP_RAW_PACKET_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "RE",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_raw_packet_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = wait option
            [TmlDefines.TML_NX_TRACE_IP_RAW_PACKET_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "RR",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_raw_packet_receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = packet ptr, I3 = destination ip, I4 = type of service
            [TmlDefines.TML_NX_TRACE_IP_RAW_PACKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_raw_packet_send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TYPE_OF_SERVICE
            },
            // I1 = ip ptr, I2 = needed status, I3 = actual status, I4 = wait option
            [TmlDefines.TML_NX_TRACE_IP_STATUS_CHECK] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_status_check",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NEEDED_STATUS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_ACTUAL_STATUS,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION
            },
            // I1 = pool ptr, I2 = packet ptr, I3 = packet type, I4 = available packets
            [TmlDefines.TML_NX_TRACE_PACKET_ALLOCATE] = new TmlEventInfo()
            {
                IconCaption = "PA",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_allocate",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_TYPE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_AVAILABLE_PACKETS
            },
            // I1 = packet ptr, I2 = new packet ptr, I3 = pool ptr, I4 = wait option
            [TmlDefines.TML_NX_TRACE_PACKET_COPY] = new TmlEventInfo()
            {
                IconCaption = "PC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_copy",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NEW_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION
            },
            // I1 = packet ptr, I2 = data start, I3 = data size, I4 = pool ptr
            [TmlDefines.TML_NX_TRACE_PACKET_DATA_APPEND] = new TmlEventInfo()
            {
                IconCaption = "DA",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_data_append",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_START,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_SIZE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR
            },
            // I1 = packet ptr, I2 = buffer start, I3 = bytes copied
            [TmlDefines.TML_NX_TRACE_PACKET_DATA_RETRIEVE] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_data_retrieve",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BUFFER_START,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_COPIED,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = packet ptr, I2 = length
            [TmlDefines.TML_NX_TRACE_PACKET_LENGTH_GET] = new TmlEventInfo()
            {
                IconCaption = "LG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_length_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_LENGTH,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = payload size, I3 = memory ptr, I4 = memory_size
            [TmlDefines.TML_NX_TRACE_PACKET_POOL_CREATE] = new TmlEventInfo()
            {
                IconCaption = "PC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_pool_create",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PAYLOAD_SIZE,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_MEMORY_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_MEMORY_SIZE
            },
            // I1 = pool ptr
            [TmlDefines.TML_NX_TRACE_PACKET_POOL_DELETE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_pool_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = pool ptr, I2 = total_packets, I3 = free packets, I4 = empty requests
            [TmlDefines.TML_NX_TRACE_PACKET_POOL_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_pool_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TOTAL_PACKETS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_FREE_PACKETS,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_EMPTY_REQUESTS
            },
            // I1 = packet ptr , I2 = packet status, I3 = available packets
            [TmlDefines.TML_NX_TRACE_PACKET_RELEASE] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_release",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_STATUS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_AVAILABLE_PACKETS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = packet ptr   , I2 = packet status, I3 = available packets
            [TmlDefines.TML_NX_TRACE_PACKET_TRANSMIT_RELEASE] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_transmit_release",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_STATUS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_AVAILABLE_PACKETS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_RARP_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "RD",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_rarp_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_RARP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "RE",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_rarp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = requests sent, I3 = responses received, I4 = invalids
            [TmlDefines.TML_NX_TRACE_RARP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "RI",
                ColorBackground = Color.FromArgb(255, 64, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_rarp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_REQUESTS_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_RESPONSES_RECEIVED,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INVALIDS
            },
            // None
            [TmlDefines.TML_NX_TRACE_SYSTEM_INITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 255, 0, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_system_initialize",
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option
            [TmlDefines.TML_NX_TRACE_TCP_CLIENT_SOCKET_BIND] = new TmlEventInfo()
            {
                IconCaption = "SB",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_client_socket_bind",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = server ip, I4 = server port
            [TmlDefines.TML_NX_TRACE_TCP_CLIENT_SOCKET_CONNECT] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_client_socket_connect",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SERVER_IP,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SERVER_PORT
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = port
            [TmlDefines.TML_NX_TRACE_TCP_CLIENT_SOCKET_PORT_GET] = new TmlEventInfo()
            {
                IconCaption = "PG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_client_socket_port_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr
            [TmlDefines.TML_NX_TRACE_TCP_CLIENT_SOCKET_UNBIND] = new TmlEventInfo()
            {
                IconCaption = "SU",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_client_socket_unbind",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_TCP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "TE",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = port, I3 = free port
            [TmlDefines.TML_NX_TRACE_TCP_FREE_PORT_FIND] = new TmlEventInfo()
            {
                IconCaption = "PF",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_free_port_find",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_FREE_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets
            [TmlDefines.TML_NX_TRACE_TCP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "TI",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_RECEIVED,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INVALID_PACKETS
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SERVER_SOCKET_ACCEPT] = new TmlEventInfo()
            {
                IconCaption = "SA",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_server_socket_accept",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = listen queue size
            [TmlDefines.TML_NX_TRACE_TCP_SERVER_SOCKET_LISTEN] = new TmlEventInfo()
            {
                IconCaption = "SL",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_server_socket_listen",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_LISTEN_QUEUE_SIZE
            },
            // I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SERVER_SOCKET_RELISTEN] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_server_socket_relisten",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SERVER_SOCKET_UNACCEPT] = new TmlEventInfo()
            {
                IconCaption = "SU",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_server_socket_unaccept",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = port
            [TmlDefines.TML_NX_TRACE_TCP_SERVER_SOCKET_UNLISTEN] = new TmlEventInfo()
            {
                IconCaption = "UL",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_server_socket_unlisten",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = window size
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_CREATE] = new TmlEventInfo()
            {
                IconCaption = "SN",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_create",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TYPE_OF_SERVICE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WINDOW_SIZE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_DELETE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_DISCONNECT] = new TmlEventInfo()
            {
                IconCaption = "DC",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_disconnect",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_SENT,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_RECEIVED
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_MSS_GET] = new TmlEventInfo()
            {
                IconCaption = "MG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_mss_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_MSS,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = peer_mss, I4 = socket state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_MSS_PEER_GET] = new TmlEventInfo()
            {
                IconCaption = "PG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_mss_peer_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PEER_MSS,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 socket state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_MSS_SET] = new TmlEventInfo()
            {
                IconCaption = "MS",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_mss_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_MSS,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE
            },
            // I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = rx sequence
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_RX_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = receive notify
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_RECEIVE_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "RN",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_receive_notify",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_RECEIVE_NOTIFY,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = tx sequence
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TX_SEQUENCE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = desired state, I4 = previous state
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_STATE_WAIT] = new TmlEventInfo()
            {
                IconCaption = "SW",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_state_wait",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DESIRED_STATE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PREVIOUS_STATE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = queue depth, I4 = timeout
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_TRANSMIT_CONFIGURE] = new TmlEventInfo()
            {
                IconCaption = "TC",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_transmit_configure",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_QUEUE_DEPTH,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TIMEOUT
            },
            // I1 = ip ptr
            [TmlDefines.TML_NX_TRACE_UDP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "UE",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = port, I3 = free port
            [TmlDefines.TML_NX_TRACE_UDP_FREE_PORT_FIND] = new TmlEventInfo()
            {
                IconCaption = "PF",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_free_port_find",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_FREE_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets
            [TmlDefines.TML_NX_TRACE_UDP_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "UI",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_SENT,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_RECEIVED,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INVALID_PACKETS
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_BIND] = new TmlEventInfo()
            {
                IconCaption = "SB",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_bind",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION
            },
            // I1 = ip ptr, I2 = socket ptr
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_CHECKSUM_DISABLE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_checksum_disable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_CHECKSUM_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "CE",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_checksum_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = queue maximum
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_CREATE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_create",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_TYPE_OF_SERVICE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_QUEUE_MAXIMUM
            },
            // I1 = ip ptr, I2 = socket ptr
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_DELETE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_SENT,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_RECEIVED
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = port
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_PORT_GET] = new TmlEventInfo()
            {
                IconCaption = "PG",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_port_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = packet size
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_receive",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = receive notify
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_RECEIVE_NOTIFY] = new TmlEventInfo()
            {
                IconCaption = "RN",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_receive_notify",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_RECEIVE_NOTIFY,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_send",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = port
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_UNBIND] = new TmlEventInfo()
            {
                IconCaption = "SU",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_unbind",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = packet ptr, I2 = ip address, I3 = port
            [TmlDefines.TML_NX_TRACE_UDP_SOURCE_EXTRACT] = new TmlEventInfo()
            {
                IconCaption = "SE",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_source_extract",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip address, I3 = interface index
            [TmlDefines.TML_NX_TRACE_IP_INTERFACE_ATTACH] = new TmlEventInfo()
            {
                IconCaption = "IA",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_interface_attach",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INTERFACE_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = bytes available
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_BYTES_AVAILABLE] = new TmlEventInfo()
            {
                IconCaption = "AB",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_bytes_available",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_AVAILABLE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = network_address, I3 = net_mask, I4 = next_hop
            [TmlDefines.TML_NX_TRACE_IP_STATIC_ROUTE_ADD] = new TmlEventInfo()
            {
                IconCaption = "AR",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_static_route_add",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NET_MASK,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NET_MASK
            },
            // I1 = ip_ptr, I2 = network_address, I3 = net_mask
            [TmlDefines.TML_NX_TRACE_IP_STATIC_ROUTE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_static_route_delete",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NET_MASK,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket ptr, I2 = network_address, I3 = port
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_PEER_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_peer_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket ptr
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_WINDOW_UPDATE_NOTIFY_SET] = new TmlEventInfo()
            {
                IconCaption = "WU",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_window_update_notify_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket_ptr, I2 = interface_index,
            [TmlDefines.TML_NX_TRACE_UDP_SOCKET_INTERFACE_SET] = new TmlEventInfo()
            {
                IconCaption = "SF",
                ColorBackground = Color.FromArgb(255, 90, 240, 120),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_udp_socket_interface_set",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INTERFACE_INDEX,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = ip_address, I3 = mtu_size, I4 = interface_index
            [TmlDefines.TML_NX_TRACE_IP_INTERFACE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "GI",
                ColorBackground = Color.FromArgb(255, 175, 95, 255),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ip_interface_info_get",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_MTU_SIZE,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_INTERFACE_INDEX
            },
            // I1 = packet_ptr, I2 = buffer_length, I3 = bytes_copied,
            [TmlDefines.TML_NX_TRACE_PACKET_DATA_EXTRACT_OFFSET] = new TmlEventInfo()
            {
                IconCaption = "EO",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_packet_data_extract_offset",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BUFFER_LENGTH,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_COPIED,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = socket ptr, I3 = bytes available
            [TmlDefines.TML_NX_TRACE_TCP_SOCKET_BYTES_AVAILABLE] = new TmlEventInfo()
            {
                IconCaption = "AB",
                ColorBackground = Color.FromArgb(255, 0, 0, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "nx_tcp_socket_bytes_available",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_BYTES_AVAILABLE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr
            [TmlDefines.TML_NXD_TRACE_ICMP_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "CE",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_icmp_enable",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size
            [TmlDefines.TML_NX_TRACE_ICMP_PING6] = new TmlEventInfo()
            {
                IconCaption = "C6",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_icmp_ping",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_PTR,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_SIZE
            },
            // I1 = packet ptr, I2 = IP Version (4 or 6), I3 = ip address, I4 = port
            [TmlDefines.TML_NXD_TRACE_UDP_SOURCE_EXTRACT] = new TmlEventInfo()
            {
                IconCaption = "UE",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_udp_source_extract",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_VERSION,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PORT
            },
            // I1 = ip_ptr
            [TmlDefines.TML_NX_TRACE_IPSEC_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "SE",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nx_ipsec_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = udp_socket_ptr, I2 = interface_id
            [TmlDefines.TML_NXD_TRACE_UDP_SOCKET_SET_INTERFACE] = new TmlEventInfo()
            {
                IconCaption = "UI",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_udp_socket_set_interface",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_UDP_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_INTERFACE_ID,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = tcp_socket_ptr, I2 = interface_id
            [TmlDefines.TML_NXD_TRACE_TCP_SOCKET_SET_INTERFACE] = new TmlEventInfo()
            {
                IconCaption = "TI",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_tcp_socket_set_interface",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_TCP_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_INTERFACE_ID,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address
            [TmlDefines.TML_NXD_TRACE_UDP_SOCKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "US",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_udp_socket_send",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_PTR,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_SIZE,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS
            },
            // I1 = dest_ip
            [TmlDefines.TML_NXD_TRACE_ND_CACHE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "ND",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_nd_cache_delete",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_DESTINATION_IP,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = IP address, I2 = physical msw, I3 = physical lsw
            [TmlDefines.TML_NXD_TRACE_ND_CACHE_ENTRY_SET] = new TmlEventInfo()
            {
                IconCaption = "NS",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_nd_cache_entry_set",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PHYSICAL_MSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PHYSICAL_LSW,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = IP address, I3 = physical msw, I4 = physical lsw
            [TmlDefines.TML_NXD_TRACE_ND_CACHE_IP_ADDRESS_FIND] = new TmlEventInfo()
            {
                IconCaption = "NF",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_nd_cache_ip_address_find",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_MSW,
                Info4Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_PHYSICAL_LSW
            },
            // I1 = ip_ptr
            [TmlDefines.TML_NXD_TRACE_ND_CACHE_INVALIDATE] = new TmlEventInfo()
            {
                IconCaption = "NI",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_nd_cache_invalidate",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length
            [TmlDefines.TML_NXD_TRACE_IPV6_GLOBAL_ADDRESS_GET] = new TmlEventInfo()
            {
                IconCaption = "6G",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_global_address_get",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PREFIX_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length
            [TmlDefines.TML_NXD_TRACE_IPV6_GLOBAL_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "6S",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_global_address_set",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PREFIX_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr
            [TmlDefines.TML_NXD_TRACE_IPV6_ENABLE] = new TmlEventInfo()
            {
                IconCaption = "6E",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_enable",
                Info1Type = (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = ip address lsw, I3 = protocol, I4 = packet_ptr
            [TmlDefines.TML_NXD_TRACE_IPV6_RAW_PACKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "S6",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_raw_packet_send",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PROTOCOL,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_PTR
            },
            // I1 = ip_ptr, I2 = ip address lsw, I3 = type of serveice, I4 = packet_ptr
            [TmlDefines.TML_NXD_TRACE_IP_RAW_PACKET_SEND] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ip_raw_packet_send",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_TYPE_OF_SERVICE,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_PTR
            },
            // I1 = ip_ptr, I2 = IP address lsw
            [TmlDefines.TML_NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_GET] = new TmlEventInfo()
            {
                IconCaption = "LG",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_linklocal_address_get",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length
            [TmlDefines.TML_NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "LS",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_linklocal_address_set",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PREFIX_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr
            [TmlDefines.TML_NXD_TRACE_IPV6_INITIATE_DAD_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "6D",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_initiate_dad_process",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = router addr lsw, I3 = router lifetime
            [TmlDefines.TML_NXD_TRACE_IPV6_DEFAULT_ROUTER_ADD] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_default_router_add",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_ROUTER_ADDR_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_ROUTER_LIFETIME,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = router addr lsw
            [TmlDefines.TML_NXD_TRACE_IPV6_DEFAULT_ROUTER_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DD",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_default_router_delete",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_ROUTER_ADDR_LSW,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index
            [TmlDefines.TML_NXD_TRACE_IPV6_INTERFACE_ADDRESS_GET] = new TmlEventInfo()
            {
                IconCaption = "AI",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_interface_address_get",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PREFIX_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_INTERFACE_INDEX
            },
            // I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index
            [TmlDefines.TML_NXD_TRACE_IPV6_INTERFACE_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "AS",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ipv6_interface_address_set",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_ADDRESS_LSW,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PREFIX_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_INTERFACE_INDEX
            },
            // I1 = socket_ptr, I2 = Peer IP address, I3 = peer_port
            [TmlDefines.TML_NXD_TRACE_TCP_SOCKET_PEER_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "SP",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_tcp_socket_peer_info_get",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PEER_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PEER_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = socket_ptr, I2 = Peer IP address, I3 = peer_port
            [TmlDefines.TML_NXD_TRACE_IP_MAX_PAYLOAD_SIZE_FIND] = new TmlEventInfo()
            {
                IconCaption = "MP",
                ColorBackground = Color.FromArgb(255, 210, 200, 160),
                ColorBackground2 = Color.FromArgb(255, 128, 128, 255),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "nxd_ip_max_payload_size_find",
                Info1Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_SOCKET_PTR,
                Info2Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PEER_IP_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.NXD_EVENT_INFO_PEER_PORT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class           , I2 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "IC",
                ColorBackground = Color.FromArgb(255, 255, 210, 120),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CLASS_INSTANCE_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class           , I2 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_DESTROY] = new TmlEventInfo()
            {
                IconCaption = "ID",
                ColorBackground = Color.FromArgb(255, 255, 210, 120),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CLASS_INSTANCE_DESTROY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CONFIGURATION_DELETE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 145, 230, 160),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CONFIGURATION_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CONFIGURATION_ENUMERATE] = new TmlEventInfo()
            {
                IconCaption = "CE",
                ColorBackground = Color.FromArgb(255, 145, 230, 160),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CONFIGURATION_ENUMERATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "CI",
                ColorBackground = Color.FromArgb(255, 145, 230, 160),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 145, 230, 160),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_CONFIGURATION_SET] = new TmlEventInfo()
            {
                IconCaption = "CS",
                ColorBackground = Color.FromArgb(255, 145, 230, 160),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_CONFIGURATION_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = device address
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_ADDRESS_SET] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_ADDRESS_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_ADDRESS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_GET] = new TmlEventInfo()
            {
                IconCaption = "DG",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_SELECT] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_SELECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_DESCRIPTOR_READ] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_DESCRIPTOR_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device index
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_GET] = new TmlEventInfo()
            {
                IconCaption = "GD",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_INDEX,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hcd             , I2 = parent          , I3 = port index        , I4 = device
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_REMOVE] = new TmlEventInfo()
            {
                IconCaption = "RD",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_REMOVE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARENT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_INDEX,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE
            },
            // I1 = device
            [TmlDefines.TML_UX_TRACE_HOST_STACK_DEVICE_RESOURCE_FREE] = new TmlEventInfo()
            {
                IconCaption = "RF",
                ColorBackground = Color.FromArgb(255, 145, 230, 225),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_DEVICE_RESOURCE_FREE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "EC",
                ColorBackground = Color.FromArgb(255, 140, 185, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "ED",
                ColorBackground = Color.FromArgb(255, 140, 185, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_STACK_ENDPOINT_RESET] = new TmlEventInfo()
            {
                IconCaption = "ER",
                ColorBackground = Color.FromArgb(255, 140, 185, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_ENDPOINT_RESET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_STACK_ENDPOINT_TRANSFER_ABORT] = new TmlEventInfo()
            {
                IconCaption = "EA",
                ColorBackground = Color.FromArgb(255, 140, 185, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_ENDPOINT_TRANSFER_ABORT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hcd name
            [TmlDefines.TML_UX_TRACE_HOST_STACK_HCD_REGISTER] = new TmlEventInfo()
            {
                IconCaption = "HR",
                ColorBackground = Color.FromArgb(255, 170, 155, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_HCD_REGISTER".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD_NAME,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "HI",
                ColorBackground = Color.FromArgb(255, 80, 117, 235),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INITIALIZE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface       , I2 = endpoint index
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INTERFACE_ENDPOINT_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 125, 192, 125),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INTERFACE_ENDPOINT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "II",
                ColorBackground = Color.FromArgb(255, 125, 192, 125),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "DI",
                ColorBackground = Color.FromArgb(255, 125, 192, 125),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INTERFACE_SET] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 125, 192, 125),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INTERFACE_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface
            [TmlDefines.TML_UX_TRACE_HOST_STACK_INTERFACE_SETTING_SELECT] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 125, 192, 125),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_INTERFACE_SETTING_SELECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = configuration
            [TmlDefines.TML_UX_TRACE_HOST_STACK_NEW_CONFIGURATION_CREATE] = new TmlEventInfo()
            {
                IconCaption = "NC",
                ColorBackground = Color.FromArgb(255, 158, 135, 180),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_NEW_CONFIGURATION_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hcd             , I2 = device owner    , I3 = port index        , I4 = device
            [TmlDefines.TML_UX_TRACE_HOST_STACK_NEW_DEVICE_CREATE] = new TmlEventInfo()
            {
                IconCaption = "ND",
                ColorBackground = Color.FromArgb(255, 158, 135, 180),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_NEW_DEVICE_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_OWNER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_INDEX,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE
            },
            // I1 = interface       , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_STACK_NEW_ENDPOINT_CREATE] = new TmlEventInfo()
            {
                IconCaption = "NE",
                ColorBackground = Color.FromArgb(255, 158, 135, 180),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_NEW_ENDPOINT_CREATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = port index
            [TmlDefines.TML_UX_TRACE_HOST_STACK_RH_CHANGE_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "RC",
                ColorBackground = Color.FromArgb(255, 196, 119, 193),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_RH_CHANGE_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_INDEX,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hcd             , I2 = port index
            [TmlDefines.TML_UX_TRACE_HOST_STACK_RH_DEVICE_EXTRACTION] = new TmlEventInfo()
            {
                IconCaption = "RE",
                ColorBackground = Color.FromArgb(255, 196, 119, 193),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_RH_DEVICE_EXTRACTION".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_INDEX,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hcd             , I2 = port index
            [TmlDefines.TML_UX_TRACE_HOST_STACK_RH_DEVICE_INSERTION] = new TmlEventInfo()
            {
                IconCaption = "RI",
                ColorBackground = Color.FromArgb(255, 196, 119, 193),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_RH_DEVICE_INSERTION".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_INDEX,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = endpoint        , I3 = transfer request
            [TmlDefines.TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 225, 220, 90),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_TRANSFER_REQUEST".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSFER_REQUEST,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device          , I2 = endpoint        , I3 = transfer request
            [TmlDefines.TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST_ABORT] = new TmlEventInfo()
            {
                IconCaption = "TA",
                ColorBackground = Color.FromArgb(255, 225, 220, 90),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_STACK_TRANSFER_REQUEST_ABORT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSFER_REQUEST,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_ASIX_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "XA",
                ColorBackground = Color.FromArgb(255, 128, 0, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_HOST_CLASS_ASIX_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_ASIX_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "XD",
                ColorBackground = Color.FromArgb(255, 128, 0, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_HOST_CLASS_ASIX_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_ASIX_INTERRUPT_NOTIFICATION] = new TmlEventInfo()
            {
                IconCaption = "IN",
                ColorBackground = Color.FromArgb(255, 128, 0, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_HOST_CLASS_ASIX_INTERRUPT_NOTIFICATION".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_ASIX_READ] = new TmlEventInfo()
            {
                IconCaption = "XR",
                ColorBackground = Color.FromArgb(255, 128, 0, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_HOST_CLASS_ASIX_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_ASIX_WRITE] = new TmlEventInfo()
            {
                IconCaption = "XW",
                ColorBackground = Color.FromArgb(255, 128, 0, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_HOST_CLASS_ASIX_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "AA",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_GET] = new TmlEventInfo()
            {
                IconCaption = "VG",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = audio control
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_SET] = new TmlEventInfo()
            {
                IconCaption = "VS",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_AUDIO_CONTROL,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "AD",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_READ] = new TmlEventInfo()
            {
                IconCaption = "AR",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = audio sampling
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_SET] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_AUDIO_SAMPLING,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_AUDIO_WRITE] = new TmlEventInfo()
            {
                IconCaption = "AW",
                ColorBackground = Color.FromArgb(255, 255, 0, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_AUDIO_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CA",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_CODING] = new TmlEventInfo()
            {
                IconCaption = "SL",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_CODING".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_LINE_CODING] = new TmlEventInfo()
            {
                IconCaption = "GL",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_LINE_CODING".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_STATE] = new TmlEventInfo()
            {
                IconCaption = "LS",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_STATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SEND_BREAK] = new TmlEventInfo()
            {
                IconCaption = "SB",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SEND_BREAK".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_IN_PIPE] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_IN_PIPE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = endpointr
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_OUT_PIPE] = new TmlEventInfo()
            {
                IconCaption = "PO",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_OUT_PIPE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_NOTIFICATION_CALLBACK] = new TmlEventInfo()
            {
                IconCaption = "NC",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_NOTIFICATION_CALLBACK".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = device status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_DEVICE_STATUS] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_DEVICE_STATUS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_STATUS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_READ] = new TmlEventInfo()
            {
                IconCaption = "MR",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_START] = new TmlEventInfo()
            {
                IconCaption = "MS",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_START".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_STOP] = new TmlEventInfo()
            {
                IconCaption = "MX",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_STOP".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_CDC_ACM_WRITE] = new TmlEventInfo()
            {
                IconCaption = "MW",
                ColorBackground = Color.FromArgb(255, 0, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_CDC_ACM_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HA",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = hid client name
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_CLIENT_REGISTER] = new TmlEventInfo()
            {
                IconCaption = "HC",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_CLIENT_REGISTER".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_NAME,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HD",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_IDLE_GET] = new TmlEventInfo()
            {
                IconCaption = "HI",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_IDLE_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_IDLE_SET] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_IDLE_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "KA",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_KEYBOARD_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "KD",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_KEYBOARD_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_MOUSE_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "MA",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_MOUSE_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_MOUSE_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "MD",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_MOUSE_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CA",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid client instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = client report
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_REPORT_GET] = new TmlEventInfo()
            {
                IconCaption = "RG",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_REPORT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLIENT_REPORT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = client report
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HID_REPORT_SET] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 128, 128, 128),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HID_REPORT_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLIENT_REPORT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HA",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_CHANGE_DETECT] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_CHANGE_DETECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = port            , I3 = port status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_CONNECTION_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "CP",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_CONNECTION_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_STATUS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = port            , I3 = port status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_ENABLE_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "EP",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_ENABLE_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_STATUS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = port            , I3 = port status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_OVER_CURRENT_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "PC",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_OVER_CURRENT_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_STATUS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = port            , I3 = port status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_RESET_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "RP",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_RESET_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_STATUS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = port            , I3 = port status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_SUSPEND_PROCESS] = new TmlEventInfo()
            {
                IconCaption = "SP",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_SUSPEND_PROCESS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PORT_STATUS,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_HUB_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HD",
                ColorBackground = Color.FromArgb(255, 128, 128, 64),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_HUB_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PA",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = pima device
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "PG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_DEVICE_INFO_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_DEVICE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_RESET] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_DEVICE_RESET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = event code      , I3 = transaction ID    , I4 = parameter1
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_NOTIFICATION] = new TmlEventInfo()
            {
                IconCaption = "PN",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_NOTIFICATION".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_EVENT_CODE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSACTION_ID,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER1
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_NUM_OBJECTS_GET] = new TmlEventInfo()
            {
                IconCaption = "NG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_NUM_OBJECTS_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_CLOSE] = new TmlEventInfo()
            {
                IconCaption = "OC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_CLOSE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_COPY] = new TmlEventInfo()
            {
                IconCaption = "CO",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_COPY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_DELETE] = new TmlEventInfo()
            {
                IconCaption = "OD",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle   , I3 = object
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_GET] = new TmlEventInfo()
            {
                IconCaption = "OG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle   , I3 = object
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_SEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_MOVE] = new TmlEventInfo()
            {
                IconCaption = "OM",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_MOVE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object          , I3 = object_buffer     , I4 = object length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_SEND] = new TmlEventInfo()
            {
                IconCaption = "OS",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_BUFFER,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_LENGTH
            },
            // I1 = class instance  , I2 = object handle   , I3 = object
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_TRANSFER_ABORT] = new TmlEventInfo()
            {
                IconCaption = "TA",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_OBJECT_TRANSFER_ABORT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = data length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_READ] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_REQUEST_CANCEL] = new TmlEventInfo()
            {
                IconCaption = "RC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_REQUEST_CANCEL".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = pima session
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_CLOSE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_SESSION_CLOSE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_SESSION,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = pima session
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_OPEN] = new TmlEventInfo()
            {
                IconCaption = "SO",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_SESSION_OPEN".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_SESSION,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = storage ID array, I3 = storage ID length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_IDS_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_STORAGE_IDS_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE_ID_ARRAY,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE_ID_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = storage ID      , I3 = storage
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_STORAGE_INFO_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE_ID,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_THUMB_GET] = new TmlEventInfo()
            {
                IconCaption = "TG",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_THUMB_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = data length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PIMA_WRITE] = new TmlEventInfo()
            {
                IconCaption = "PW",
                ColorBackground = Color.FromArgb(255, 255, 128, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PIMA_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PA",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_NAME_GET] = new TmlEventInfo()
            {
                IconCaption = "NG",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_NAME_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_READ] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_WRITE] = new TmlEventInfo()
            {
                IconCaption = "PW",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_SOFT_RESET] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_SOFT_RESET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = printer status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PRINTER_STATUS_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 186, 96, 69),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PRINTER_STATUS_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PRINTER_STATUS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PA",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_CODING] = new TmlEventInfo()
            {
                IconCaption = "SL",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_CODING".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_LINE_CODING] = new TmlEventInfo()
            {
                IconCaption = "GL",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_LINE_CODING".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_STATE] = new TmlEventInfo()
            {
                IconCaption = "LS",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_STATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_PURGE] = new TmlEventInfo()
            {
                IconCaption = "IP",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_PURGE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SEND_BREAK] = new TmlEventInfo()
            {
                IconCaption = "SB",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SEND_BREAK".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = endpoint
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_IN_PIPE] = new TmlEventInfo()
            {
                IconCaption = "PI",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_IN_PIPE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = endpointr
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_OUT_PIPE] = new TmlEventInfo()
            {
                IconCaption = "PO",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_OUT_PIPE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = parameter
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_REPORT_DEVICE_STATUS_CHANGE] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_REPORT_DEVICE_STATUS_CHANGE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = device status
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_DEVICE_STATUS] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_DEVICE_STATUS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_STATUS,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_READ] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_START] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_START".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_STOP] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_STOP".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_PROLIFIC_WRITE] = new TmlEventInfo()
            {
                IconCaption = "PW",
                ColorBackground = Color.FromArgb(255, 255, 0, 210),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_PROLIFIC_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "SA",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_CAPACITY_GET] = new TmlEventInfo()
            {
                IconCaption = "CG",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_CAPACITY_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_FORMAT_CAPACITY_GET] = new TmlEventInfo()
            {
                IconCaption = "FG",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_FORMAT_CAPACITY_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = sector
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_MOUNT] = new TmlEventInfo()
            {
                IconCaption = "MM",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_MOUNT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = media
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_OPEN] = new TmlEventInfo()
            {
                IconCaption = "MO",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_OPEN".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_MEDIA,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = sector start    , I3 = sector count      , I4 = data pointer
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_READ] = new TmlEventInfo()
            {
                IconCaption = "MR",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR_START,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR_COUNT,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER
            },
            // I1 = class instance  , I2 = sector start    , I3 = sector count      , I4 = data pointer
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_WRITE] = new TmlEventInfo()
            {
                IconCaption = "MW",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_MEDIA_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR_START,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR_COUNT,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_REQUEST_SENSE] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_REQUEST_SENSE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = start stop signal
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_START_STOP] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_START_STOP".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_START_STOP_SIGNAL,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_STORAGE_UNIT_READY_TEST] = new TmlEventInfo()
            {
                IconCaption = "UT",
                ColorBackground = Color.FromArgb(255, 45, 205, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_STORAGE_UNIT_READY_TEST".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_DPUMP_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "DA",
                ColorBackground = Color.FromArgb(255, 105, 145, 115),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_DPUMP_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_DPUMP_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "DD",
                ColorBackground = Color.FromArgb(255, 105, 145, 115),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_DPUMP_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_DPUMP_READ] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 105, 145, 115),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_DPUMP_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = data pointer    , I3 = requested length
            [TmlDefines.TML_UX_TRACE_HOST_CLASS_DPUMP_WRITE] = new TmlEventInfo()
            {
                IconCaption = "DW",
                ColorBackground = Color.FromArgb(255, 105, 145, 115),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_HOST_CLASS_DPUMP_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_GET] = new TmlEventInfo()
            {
                IconCaption = "SG",
                ColorBackground = Color.FromArgb(255, 105, 105, 150),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE_VALUE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface value , I2 = alternate setting value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_SET] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 105, 105, 150),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE_VALUE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ALTERNATE_SETTING_VALUE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class name      , I2 = interface number, I3 = parameter
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_CLASS_REGISTER] = new TmlEventInfo()
            {
                IconCaption = "CR",
                ColorBackground = Color.FromArgb(255, 89, 83, 172),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_STACK_CLASS_REGISTER".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_NAME,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE_NUMBER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = request type    , I2 = request value   , I3 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_CLEAR_FEATURE] = new TmlEventInfo()
            {
                IconCaption = "CF",
                ColorBackground = Color.FromArgb(255, 68, 181, 187),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_CLEAR_FEATURE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_TYPE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_VALUE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_GET] = new TmlEventInfo()
            {
                IconCaption = "CG",
                ColorBackground = Color.FromArgb(255, 40, 215, 84),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_CONFIGURATION_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION_VALUE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = configuration value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_SET] = new TmlEventInfo()
            {
                IconCaption = "CS",
                ColorBackground = Color.FromArgb(255, 40, 215, 84),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_CONFIGURATION_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION_VALUE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_CONNECT] = new TmlEventInfo()
            {
                IconCaption = "SC",
                ColorBackground = Color.FromArgb(255, 206, 215, 40),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_CONNECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = descriptor type , I2 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_DESCRIPTOR_SEND] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 151, 196, 60),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_DESCRIPTOR_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DESCRIPTOR_TYPE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = device
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_DISCONNECT] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 158, 218, 37),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_DISCONNECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = endpoint
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_ENDPOINT_STALL] = new TmlEventInfo()
            {
                IconCaption = "ES",
                ColorBackground = Color.FromArgb(255, 174, 244, 11),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_ENDPOINT_STALL".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = request type    , I2 = request value   , I3 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_GET_STATUS] = new TmlEventInfo()
            {
                IconCaption = "GS",
                ColorBackground = Color.FromArgb(255, 78, 252, 3),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_GET_STATUS".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_TYPE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_VALUE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_HOST_WAKEUP] = new TmlEventInfo()
            {
                IconCaption = "HW",
                ColorBackground = Color.FromArgb(255, 2, 253, 65),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_HOST_WAKEUP".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            //
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_INITIALIZE] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 3, 252, 184),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_INITIALIZE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_INTERFACE_DELETE] = new TmlEventInfo()
            {
                IconCaption = "ID",
                ColorBackground = Color.FromArgb(255, 1, 254, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_INTERFACE_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = interface value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_INTERFACE_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 1, 254, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_INTERFACE_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE_VALUE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = alternate setting value
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_INTERFACE_SET] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 1, 254, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_INTERFACE_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ALTERNATE_SETTING_VALUE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = request value   , I2 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_SET_FEATURE] = new TmlEventInfo()
            {
                IconCaption = "SF",
                ColorBackground = Color.FromArgb(255, 30, 225, 216),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_SET_FEATURE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_VALUE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = transfer request, I2 = completion code
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_TRANSFER_ABORT] = new TmlEventInfo()
            {
                IconCaption = "TA",
                ColorBackground = Color.FromArgb(255, 1, 209, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_TRANSFER_ABORT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSFER_REQUEST,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_COMPLETION_CODE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = endpoint        , I2 = completion code
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_TRANSFER_ALL_REQUEST_ABORT] = new TmlEventInfo()
            {
                IconCaption = "RA",
                ColorBackground = Color.FromArgb(255, 1, 209, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_TRANSFER_ALL_REQUEST_ABORT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_COMPLETION_CODE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = transfer request
            [TmlDefines.TML_UX_TRACE_DEVICE_STACK_TRANSFER_REQUEST] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 1, 209, 254),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_STACK_TRANSFER_REQUEST".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSFER_REQUEST,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_DPUMP_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "DA",
                ColorBackground = Color.FromArgb(255, 115, 3, 252),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_DPUMP_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_DPUMP_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "DD",
                ColorBackground = Color.FromArgb(255, 115, 3, 252),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_DPUMP_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = buffer          , I3 = requested_length
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_DPUMP_READ] = new TmlEventInfo()
            {
                IconCaption = "DR",
                ColorBackground = Color.FromArgb(255, 115, 3, 252),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_DPUMP_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_BUFFER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = buffer          , I3 = requested_length
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_DPUMP_WRITE] = new TmlEventInfo()
            {
                IconCaption = "DW",
                ColorBackground = Color.FromArgb(255, 115, 3, 252),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_DPUMP_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_BUFFER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_CDC_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CA",
                ColorBackground = Color.FromArgb(255, 133, 13, 242),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_CDC_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_CDC_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "CD",
                ColorBackground = Color.FromArgb(255, 133, 13, 242),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_CDC_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = buffer          , I3 = requested_length
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_CDC_READ] = new TmlEventInfo()
            {
                IconCaption = "CR",
                ColorBackground = Color.FromArgb(255, 133, 13, 242),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_CDC_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = buffer          , I3 = requested_length
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_CDC_WRITE] = new TmlEventInfo()
            {
                IconCaption = "CW",
                ColorBackground = Color.FromArgb(255, 133, 13, 242),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_CDC_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DATA_POINTER,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUESTED_LENGTH,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HA",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "HD",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid event
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_GET] = new TmlEventInfo()
            {
                IconCaption = "EG",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_EVENT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_EVENT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = hid event
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_SET] = new TmlEventInfo()
            {
                IconCaption = "ES",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_EVENT_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_EVENT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = descriptor type , I3 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_GET] = new TmlEventInfo()
            {
                IconCaption = "RG",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_REPORT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DESCRIPTOR_TYPE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = descriptor type , I3 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_SET] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_REPORT_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DESCRIPTOR_TYPE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = descriptor type , I3 = request index
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_HID_DESCRIPTOR_SEND] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 148, 48, 207),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 255, 255, 255),
                EventType = "UX_TRACE_DEVICE_CLASS_HID_DESCRIPTOR_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_DESCRIPTOR_TYPE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_REQUEST_INDEX,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PA",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "PD",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_DEVICE_INFO_SEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_DEVICE_INFO_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = pima event
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_GET] = new TmlEventInfo()
            {
                IconCaption = "EG",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_EVENT_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_EVENT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = pima event
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_SET] = new TmlEventInfo()
            {
                IconCaption = "ES",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_EVENT_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_EVENT,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_ADD] = new TmlEventInfo()
            {
                IconCaption = "OA",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_ADD".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_GET] = new TmlEventInfo()
            {
                IconCaption = "DG",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_SEND] = new TmlEventInfo()
            {
                IconCaption = "DS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DELETE] = new TmlEventInfo()
            {
                IconCaption = "OD",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DELETE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = storage id      , I3 = object format code, I4 = object association
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_HANDLES_SEND] = new TmlEventInfo()
            {
                IconCaption = "HS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_HANDLES_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE_ID,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJ_FORMAT_CODE,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJ_ASSOCIATION
            },
            // I1 = class instance  , I2 = object handle
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_GET] = new TmlEventInfo()
            {
                IconCaption = "IG",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_SEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = storage id      , I3 = object format code, I4 = object association
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECTS_NUMBER_SEND] = new TmlEventInfo()
            {
                IconCaption = "NS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_OBJECTS_NUMBER_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_STORAGE_ID,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJ_FORMAT_CODE,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJ_ASSOCIATION
            },
            // I1 = class instance  , I2 = object handle   , I3 = offset requested  , I4 = length requested
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_PARTIAL_OBJECT_DATA_GET] = new TmlEventInfo()
            {
                IconCaption = "DG",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_PARTIAL_OBJECT_DATA_GET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT_HANDLE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_OFFSET_REQUESTED,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LENGTH_REQUESTED
            },
            // I1 = class instance  , I2 = response code   , I3 = number parameter  , I4 = pima parameter 1
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_RESPONSE_SEND] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_RESPONSE_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_RESPONSE_CODE,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_NUMBER_PARAMETER,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_PARAMETER_1
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_ID_SEND] = new TmlEventInfo()
            {
                IconCaption = "IS",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_ID_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_INFO_SEND] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 128, 128, 255),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_INFO_SEND".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "RA",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "RD",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_RECEIVE] = new TmlEventInfo()
            {
                IconCaption = "PR",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_RECEIVE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_TRANSMIT] = new TmlEventInfo()
            {
                IconCaption = "PT",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_TRANSMIT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = rndis OID
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_QUERY] = new TmlEventInfo()
            {
                IconCaption = "MQ",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_MSG_QUERY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_RNDIS_OID,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_KEEP_ALIVE] = new TmlEventInfo()
            {
                IconCaption = "KA",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_MSG_KEEP_ALIVE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_RESET] = new TmlEventInfo()
            {
                IconCaption = "MR",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_MSG_RESET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = rndis OID
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_SET] = new TmlEventInfo()
            {
                IconCaption = "MS",
                ColorBackground = Color.FromArgb(255, 199, 233, 217),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_RNDIS_MSG_SET".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_RNDIS_OID,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_ACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "SA",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_ACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_DEACTIVATE] = new TmlEventInfo()
            {
                IconCaption = "SD",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_DEACTIVATE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_FORMAT] = new TmlEventInfo()
            {
                IconCaption = "SF",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_FORMAT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_INQUIRY] = new TmlEventInfo()
            {
                IconCaption = "SI",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_INQUIRY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SELECT] = new TmlEventInfo()
            {
                IconCaption = "MS",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SELECT".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SENSE] = new TmlEventInfo()
            {
                IconCaption = "SM",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SENSE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_PREVENT_ALLOW_MEDIA_REMOVAL] = new TmlEventInfo()
            {
                IconCaption = "MR",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_PREVENT_ALLOW_MEDIA_REMOVAL".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun             , I3 = sector              , I4 = number sectors
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ] = new TmlEventInfo()
            {
                IconCaption = "SR",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_READ".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_NUMBER_SECTORS
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_CAPACITY] = new TmlEventInfo()
            {
                IconCaption = "RC",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_READ_CAPACITY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_FORMAT_CAPACITY] = new TmlEventInfo()
            {
                IconCaption = "FC",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_READ_FORMAT_CAPACITY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_TOC] = new TmlEventInfo()
            {
                IconCaption = "RT",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_READ_TOC".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun             , I3 = sense key           , I4 = code
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_REQUEST_SENSE] = new TmlEventInfo()
            {
                IconCaption = "RS",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_REQUEST_SENSE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SENSE_KEY,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CODE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_TEST_READY] = new TmlEventInfo()
            {
                IconCaption = "TR",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_TEST_READY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_START_STOP] = new TmlEventInfo()
            {
                IconCaption = "SS",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_START_STOP".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_VERIFY] = new TmlEventInfo()
            {
                IconCaption = "SV",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_VERIFY".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            },
            // I1 = class instance  , I2 = lun             , I3 = sector              , I4 = number sectors
            [TmlDefines.TML_UX_TRACE_DEVICE_CLASS_STORAGE_WRITE] = new TmlEventInfo()
            {
                IconCaption = "SW",
                ColorBackground = Color.FromArgb(255, 187, 235, 244),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_DEVICE_CLASS_STORAGE_WRITE".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_LUN,
                Info3Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_SECTOR,
                Info4Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_NUMBER_SECTORS
            },
            [TmlDefines.TML_UX_TRACE_ERROR] = new TmlEventInfo()
            {
                IconCaption = "TE",
                ColorBackground = Color.FromArgb(255, 255, 0, 0),
                ColorBackground2 = Color.FromArgb(255, 210, 200, 160),
                ColorForeground = Color.FromArgb(255, 0, 0, 0),
                ColorForeground2 = Color.FromArgb(255, 0, 0, 0),
                EventType = "UX_TRACE_ERROR".ToLower(CultureInfo.InvariantCulture),
                Info1Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_USBX_ERROR,
                Info2Type = (uint)EventInfoTypes.types.UX_EVENT_INFO_ERROR_TYPE,
                Info3Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE,
                Info4Type = (uint)EventInfoTypes.types.EVENT_INFO_NONE
            }
        };
    }
}