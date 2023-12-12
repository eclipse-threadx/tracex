using System.Runtime.InteropServices;
using System.Windows.Media;

namespace AzureRTOS.Tml
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlTrace
    {
        public int TotalThreads { get; set; }
        public int TotalObjects { get; set; }
        public int TotalEvents { get; set; }
        public long MaxRelativeTicks { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlInterruptIdle
    {
        public long InterruptUsage { get; set; }
        public long IdleUsage { get; set; }
        public long InitializeUsage { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlThread
    {
        public uint Index { get; set; }
        public uint DisplayIndex { get; set; }
        public uint Parameter1 { get; set; }
        public uint Parameter2 { get; set; }
        public uint Address { get; set; }
        public uint StackSize { get; set; }
        public uint Availability { get; set; }
        public uint Suspensions { get; set; }
        public uint Resumptions { get; set; }
        public uint LowestPriority { get; set; }
        public uint HighestPriority { get; set; }
        public double Usage { get; set; }
        public double Percentage { get; set; }
        public uint EventId { get; set; }
        public string Name { get; set; }
        public uint NumberOfEvents { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlPerformance
    {
        public uint ContextSwitches { get; set; }
        public uint Preemptions { get; set; }
        public uint TimeSlices { get; set; }
        public uint Suspensions { get; set; }
        public uint Resumptions { get; set; }
        public uint Interrupts { get; set; }
        public uint NestedInterrupts { get; set; }
        public uint DeterministicInversions { get; set; }
        public uint UndeterministicInversions { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlExecutionStatus
    {
        public uint EventNumber { get; set; }
        public uint Status { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class CustomEvent
    {
        public uint EventNumber { get; set; }
        public string EventName { get; set; }
        public string TwoLetterAbbreviation { get; set; }
        public int[] IconColor1 { get; } = new int[3];
        public int[] IconColor2 { get; set; } = new int[3];
        public string Info1 { get; set; }
        public string Info2 { get; set; }
        public string Info3 { get; set; }
        public string Info4 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlPerformanceNx
    {
        public uint ArpRequestsSent { get; set; }
        public uint ArpResponsesSent { get; set; }
        public uint ArpRequestsReceived { get; set; }
        public uint ArpResponsesReceived { get; set; }
        public uint PacketAllocations { get; set; }
        public uint PacketReleases { get; set; }
        public uint EmptyAllocations { get; set; }
        public uint InvalidReleases { get; set; }
        public uint IpPacketsSent { get; set; }
        public uint IpBytesSent { get; set; }
        public uint IpPacketsReceived { get; set; }
        public uint IpBytesReceived { get; set; }
        public uint PingsSent { get; set; }
        public uint PingResponses { get; set; }
        public uint TcpClientConnections { get; set; }
        public uint TcpServerConnections { get; set; }
        public uint TcpPacketsSent { get; set; }
        public uint TcpBytesSent { get; set; }
        public uint TcpPacketsReceived { get; set; }
        public uint TcpBytesReceived { get; set; }
        public uint UdpPacketsSent { get; set; }
        public uint UdpBytesSent { get; set; }
        public uint UdpPacketsReceived { get; set; }
        public uint UdpBytesReceived { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlPerformanceFx
    {
        public uint MediaOpens { get; set; }
        public uint MediaCloses { get; set; }
        public uint MediaAborts { get; set; }
        public uint MediaFlushes { get; set; }
        public uint DirectoryReads { get; set; }
        public uint DirectoryWrites { get; set; }
        public uint DirectoryCacheMisses { get; set; }
        public uint FileOpens { get; set; }
        public uint FileCloses { get; set; }
        public uint FileBytesRead { get; set; }
        public uint FileBytesWritten { get; set; }
        public uint LogicalSectorReads { get; set; }
        public uint LogicalSectorWrites { get; set; }
        public uint LogicalSectorCacheMisses { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlObject
    {
        public uint CreatedOrder { get; set; }
        public uint Type { get; set; }
        public uint Parameter1 { get; set; }
        public uint Parameter2 { get; set; }
        public uint Address { get; set; }
        public string ObjectName { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlHeaderInfo
    {
        public uint TraceFileSize { get; set; }
        public uint TraceId { get; set; }
        public uint TimerValidMask { get; set; }
        public uint TraceBaseAddress { get; set; }
        public uint ObjectRegistryStartAddress { get; set; }
        public ushort Reserved1 { get; set; }
        public ushort ObjectNameSize { get; set; }
        public uint ObjectRegistryEndAddress { get; set; }
        public uint TraceBufferStartAddress { get; set; }
        public uint TraceBufferEndAddress { get; set; }
        public uint TraceBufferCurrentAddress { get; set; }
        public uint Reserved2 { get; set; }
        public uint Reserved3 { get; set; }
        public uint Reserved4 { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlEvent
    {
        public uint Index { get; set; }
        public uint Context { get; set; }
        public uint ThreadPriority { get; set; }
        public uint Id { get; set; }
        public uint TimeStamp { get; set; }
        public uint Info1 { get; set; }
        public uint Info2 { get; set; }
        public uint Info3 { get; set; }
        public uint Info4 { get; set; }
        public uint NextContext { get; set; }
        public uint ThreadIndex { get; set; }
        public uint PriorityInversion { get; set; }
        public uint BadPriorityInversion { get; set; }
        public long RelativeTicks { get; set; }
        public double IconWidth { get; set; }
    } 
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlEventInfo
    {
        public string EventType { get; set; }
        public Color ColorBackground { get; set; }
        public Color ColorBackground2 { get; set; }
        public Color ColorForeground { get; set; }
        public Color ColorForeground2 { get; set; }
        public string IconCaption { get; set; }
        public uint Info1Type { get; set; }
        public uint Info2Type { get; set; }
        public uint Info3Type { get; set; }
        public uint Info4Type { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlThreadPerformance
    {
        public ulong Index { get; set; }
        public ulong Suspensions { get; set; }
        public ulong Resumptions { get; set; }
        public ulong HighestPriority { get; set; }
        public ulong LowestPriority { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlInversionDetection
    {
        public uint BlockedThreadAddress { get; set; }
        public uint BlockedThreadPriority { get; set; }
        public uint OwningThreadAddress { get; set; }
        public uint OwningThreadPriority { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class TmlEventPopularity
    {
        public uint EventId { get; set; }
        public uint EventCount { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class SystemTime
    {
        public ushort Year { get; set; }
        public ushort Month { get; set; }
        public ushort DayOfWeek { get; set; }
        public ushort Day { get; set; }
        public ushort Hour { get; set; }
        public ushort Minute { get; set; }
        public ushort Second { get; set; }
        public ushort Milliseconds { get; set; }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public class FileIO
    {
        public string Ptr { get; set; }
        public int Count { get; set; }
        public string Base { get; set; }
        public int Flag { get; set; }
        public int File { get; set; }
        public int CharBuf { get; set; }
        public int BufSize { get; set; }
        public string TempFileName { get; set; }
    }
}
