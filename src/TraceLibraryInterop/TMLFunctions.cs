using System;
using System.Runtime.InteropServices;

namespace AzureRTOS.Tml
{
    public class TmlFunctions
    {
        // To reduce security risk, marshal parameter 'fileName' as Unicode,
        // by setting DllImport.CharSet to CharSet.Unicode, or by explicitly marshaling the parameter as
        // UnmanagedType.LPWStr.If you need to marshal this string as ANSI or system-dependent,
        // set BestFitMapping = false; for added security, also set ThrowOnUnmappableChar= true.
        [DllImport("CRTDll.dll", EntryPoint = "fopen", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern IntPtr FileOpen(string fileName, string mode);

        [DllImport("CRTDll.dll", EntryPoint = "fclose", CallingConvention = CallingConvention.Cdecl)]
        internal static extern void FileClose(IntPtr handle);

        [DllImport("ELTMLibrary.dll", EntryPoint = "extract_trace_info", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int ExtractTraceInfo(
            // [MarshalAs(UnmanagedType.LPStr)] string fileName,
            // [MarshalAs(UnmanagedType.AnsiBStr)] string fileName,
            string fileName,
            // [MarshalAs(UnmanagedType.LPStruct)] ref TmlTrace trace
            ref TmlTrace trace);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_raw_trace_file_dump", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern bool RawTraceFileDump(
            // [MarshalAs(UnmanagedType.LPStr)] string fileName,
            // [MarshalAs(UnmanagedType.AnsiBStr)] string fileName,
            string fileName);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_thread_execution_status_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int ThreadExecutionStatus(
            //[MarshalAs(UnmanagedType.SysInt)] IntPtr sourceTraceFile,
            ulong threadIndex);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_initialize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int Initialize(
            // [MarshalAs(UnmanagedType.SysInt)] IntPtr sourceTraceFile,
            IntPtr sourceTraceFile,
            out uint totalThreads,
            out uint totalObjects,
            out uint totalEvents,
            out long maxRelativeTicks);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_uninitialize", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern void Uninitialize();

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_header_info_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int GetHeaderInfo(
            out uint header_trace_id,
            out uint header_timer_valid_mask,
            out uint header_trace_base_address,
            out uint header_object_registry_start_address,
            out ushort header_reserved1,
            out ushort header_object_name_size,
            out uint header_object_registry_end_address,
            out uint header_trace_buffer_start_address,
            out uint header_trace_buffer_end_address,
            out uint header_trace_buffer_current_address,
            out uint header_reserved2,
            out uint header_reserved3,
            out uint header_reserved4);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_object_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int GetObject(
            uint object_index,
            out string object_name,
            out uint object_address,
            out uint parameter_1,
            out uint parameter_2);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_object_thread_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        internal static extern int GetObjectThread(
            uint thread_index,
            out string object_name,
            out uint object_address,
            out uint parameter_1,
            out uint parameter_2);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_object_by_address_find", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int FindObjectByAddress(
            uint object_address,
            out uint object_index);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_event_get", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int GetEvent(
            uint event_index,
            out uint event_context,
            out uint event_id,
            out uint event_thread_priority,
            out uint event_time_stamp,
            out uint event_info_1,
            out uint event_info_2,
            out uint event_info_3,
            out uint event_info_4,
            out long event_relative_ticks);

        [DllImport("ELTMLibrary.dll", EntryPoint = "tml_event_by_relative_ticks_find", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        internal static extern int FindEventByRelativeTicks(
            long relative_ticks_start,
            long relative_ticks_end,
            out int event_index);
    }
}
