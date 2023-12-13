// This is the main DLL file.

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Runtime::InteropServices;
using namespace AzureRTOS::Tml;

#include "stdafx.h"
#include "ELTMLManaged.h"

bool ELTMLManaged::TMLFunctions::ExtractTraceInfo(String^ file_name, TmlTrace^ tmlTrace, List<TmlThread^>^ threads, List<TmlObject^>^ objects, List<TmlEvent^>^ events)
{
	IntPtr filePtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(file_name);
	char* fileName = static_cast<char*>(filePtr.ToPointer());

	System::IO::FileInfo^ fileInfo = gcnew System::IO::FileInfo(file_name);
	int traceFileSize = fileInfo->Length;

	FILE* source_trace_file;
	source_trace_file = fopen(fileName, "rb");
	if (source_trace_file == NULL)
	{
		return false;
	}

	unsigned long	total_threads = 0;
	unsigned long	total_objects = 0;
	unsigned long	total_events = 0;
	_int64			max_relative_ticks = 0;
	char* error_string = NULL;

	int status = tml_initialize(source_trace_file, &total_threads, &total_objects, &total_events, &max_relative_ticks, &error_string);
	if (status != 0)
	{
		ErrorMessage = gcnew String(error_string);
		return false;
	}

	tmlTrace->TotalThreads = total_threads;
	tmlTrace->TotalObjects = total_objects;
	tmlTrace->TotalEvents = total_events;
	tmlTrace->MaxRelativeTicks = max_relative_ticks;

	// get header info
	unsigned long	trace_id;
	unsigned long	timer_valid_mask;
	unsigned long	trace_base_address;
	unsigned long	object_registry_start_address;
	unsigned short	reserved1;
	unsigned short	object_name_size;
	unsigned long	object_registry_end_address;
	unsigned long	trace_buffer_start_address;
	unsigned long	trace_buffer_end_address;
	unsigned long	trace_buffer_current_address;
	unsigned long	reserved2;
	unsigned long	reserved3;
	unsigned long	reserved4;

	status = tml_header_info_get(&trace_id, &timer_valid_mask, &trace_base_address, &object_registry_start_address,
		&reserved1, &object_name_size, &object_registry_end_address, &trace_buffer_start_address,
		&trace_buffer_end_address, &trace_buffer_current_address,
		&reserved2, &reserved3, &reserved4);

	HeaderInfo = gcnew TmlHeaderInfo();
	HeaderInfo->TraceFileSize = traceFileSize;
	HeaderInfo->TraceId = trace_id;
	HeaderInfo->TimerValidMask = timer_valid_mask;
	HeaderInfo->TraceBaseAddress = trace_base_address;
	HeaderInfo->ObjectRegistryStartAddress = object_registry_start_address;
	HeaderInfo->Reserved1 = reserved1;
	HeaderInfo->ObjectNameSize = object_name_size;
	HeaderInfo->ObjectRegistryEndAddress = object_registry_end_address;
	HeaderInfo->TraceBufferStartAddress = trace_buffer_start_address;
	HeaderInfo->TraceBufferEndAddress = trace_buffer_end_address;
	HeaderInfo->TraceBufferCurrentAddress = trace_buffer_current_address;
	HeaderInfo->Reserved2 = reserved2;
	HeaderInfo->Reserved3 = reserved3;
	HeaderInfo->Reserved4 = reserved4;

	// get thread  objects
	unsigned long	i;
	char* object_name;
	unsigned long	object_address;
	unsigned long	parameter_1;
	unsigned long	parameter_2;
	unsigned long	lowest_priority;
	unsigned long	highest_priority;

	for (i = 0; i < total_threads; i++)
	{
		tml_object_thread_get(i, &object_name, &object_address, &parameter_1, &parameter_2, &lowest_priority, &highest_priority);

		TmlThread^ thread = gcnew TmlThread();
		threads->Add(thread);

		thread->Index = i;
		thread->Parameter1 = parameter_1;
		thread->Parameter2 = parameter_1 + parameter_2;
		thread->Address = object_address;
		thread->Name = gcnew String(object_name);
		thread->LowestPriority = lowest_priority;
		thread->HighestPriority = highest_priority;
	}

	for (i = 0; i < total_objects; i++)
	{
		tml_object_get(i, &object_name, &object_address, &parameter_1, &parameter_2);
		TmlObject^ tmlObject = gcnew TmlObject();

		tmlObject->Address = object_address;
		tmlObject->ObjectName = gcnew String(object_name);
		tmlObject->Parameter1 = parameter_1;
		tmlObject->Parameter2 = parameter_2;
		objects->Add(tmlObject);
	}

	unsigned long	event_context;
	unsigned long	event_id;
	unsigned long	event_thread_priority;
	unsigned long	event_time_stamp;
	_int64			event_relative_ticks;
	unsigned long	event_info_1;
	unsigned long	event_info_2;
	unsigned long	event_info_3;
	unsigned long	event_info_4;
	unsigned long	next_context;
	unsigned long	thread_index;
	unsigned long	priority_inversion;
	unsigned long	bad_priority_inversion;

	for (i = 0; i < total_events; i++)
	{
		int status = tml_event_get(
			i,
			&event_context,
			&event_id,
			&event_thread_priority,
			&event_time_stamp,
			&event_info_1,
			&event_info_2,
			&event_info_3,
			&event_info_4,
			&event_relative_ticks,
			&next_context,
			&thread_index,
			&priority_inversion,
			&bad_priority_inversion
		);

		TmlEvent^ tmlEvent = gcnew TmlEvent();
		events->Add(tmlEvent);

		tmlEvent->Index = i;
		tmlEvent->Context = event_context;
		tmlEvent->Id = event_id;
		tmlEvent->ThreadPriority = event_thread_priority;
		tmlEvent->TimeStamp = event_time_stamp;
		tmlEvent->RelativeTicks = event_relative_ticks;
		tmlEvent->Info1 = event_info_1;
		tmlEvent->Info2 = event_info_2;
		tmlEvent->Info3 = event_info_3;
		tmlEvent->Info4 = event_info_4;
		tmlEvent->NextContext = next_context;
		tmlEvent->ThreadIndex = thread_index;
		tmlEvent->PriorityInversion = priority_inversion;
		tmlEvent->BadPriorityInversion = bad_priority_inversion;
	}

	unsigned long stackSize = 0;
	unsigned long minAvailable = 0;
	unsigned long eventid = 0;

	unsigned long	thread_susps;
	unsigned long	thread_resumps;
	_int64			thread_usage;

	for (i = 0; i < total_threads; i++)
	{
		TmlThread^ thread = threads[i];
		int retval = tml_thread_stack_usage_get(thread->Index, &stackSize, &minAvailable, &eventid);
		thread->StackSize = stackSize;
		thread->Availability = minAvailable;
		thread->EventId = eventid;

		retval = tml_thread_performance_statistics_get(thread->Index, &thread_susps, &thread_resumps);
		thread->Suspensions = thread_susps;
		thread->Resumptions = thread_resumps;

		retval = tml_thread_execution_profile_get(thread->Index, &thread_usage);
		thread->Usage = thread_usage;
	}

	unsigned long	context_switches = 0;
	unsigned long	thread_preemptions = 0;
	unsigned long	time_slices = 0;
	unsigned long	thread_suspensions = 0;
	unsigned long	thread_resumptions = 0;
	unsigned long	interrupts = 0;
	unsigned long	nested_interrupts = 0;
	unsigned long	deterministic_priority_inversions = 0;
	unsigned long	undeterministic_priority_inversions = 0;

	int retVal = tml_system_performance_statistics_get(
		&context_switches,
		&thread_preemptions,
		&time_slices,
		&thread_suspensions,
		&thread_resumptions,
		&interrupts,
		&nested_interrupts,
		&deterministic_priority_inversions,
		&undeterministic_priority_inversions
	);

	Performance = gcnew TmlPerformance();
	Performance->ContextSwitches = context_switches;
	Performance->Preemptions = thread_preemptions;
	Performance->TimeSlices = time_slices;
	Performance->Suspensions = thread_suspensions;
	Performance->Resumptions = thread_resumptions;
	Performance->Interrupts = interrupts;
	Performance->NestedInterrupts = nested_interrupts;
	Performance->DeterministicInversions = deterministic_priority_inversions;
	Performance->UndeterministicInversions = undeterministic_priority_inversions;

	_int64	interrupt_usage;
	_int64	idle_usage;
	retVal = tml_system_execution_profile_get(&interrupt_usage, &idle_usage);

	InterruptIdle = gcnew TmlInterruptIdle();
	InterruptIdle->InterruptUsage = interrupt_usage;
	InterruptIdle->IdleUsage = idle_usage;
	InterruptIdle->InitializeUsage = 0;

	unsigned long media_opens;
	unsigned long media_closes;
	unsigned long media_aborts;
	unsigned long media_flushes;
	unsigned long directory_reads;
	unsigned long directory_writes;
	unsigned long directory_cache_misses;
	unsigned long file_opens;
	unsigned long file_closes;
	unsigned long file_bytes_read;
	unsigned long file_bytes_written;
	unsigned long logical_sector_reads;
	unsigned long logical_sector_writes;
	unsigned long logical_sector_cache_misses;

	retVal = tml_system_filex_performance_statistics_get(
		&media_opens,
		&media_closes,
		&media_aborts,
		&media_flushes,
		&directory_reads,
		&directory_writes,
		&directory_cache_misses,
		&file_opens,
		&file_closes,
		&file_bytes_read,
		&file_bytes_written,
		&logical_sector_reads,
		&logical_sector_writes,
		&logical_sector_cache_misses);

	PerformanceFx = gcnew TmlPerformanceFx();
	PerformanceFx->MediaOpens = media_opens;
	PerformanceFx->MediaCloses = media_closes;
	PerformanceFx->MediaAborts = media_aborts;
	PerformanceFx->MediaFlushes = media_flushes;
	PerformanceFx->DirectoryReads = directory_reads;
	PerformanceFx->DirectoryWrites = directory_writes;
	PerformanceFx->DirectoryCacheMisses = directory_cache_misses;
	PerformanceFx->FileOpens = file_opens;
	PerformanceFx->FileCloses = file_closes;
	PerformanceFx->FileBytesRead = file_bytes_read;
	PerformanceFx->FileBytesWritten = file_bytes_written;
	PerformanceFx->LogicalSectorReads = logical_sector_reads;
	PerformanceFx->LogicalSectorWrites = logical_sector_writes;
	PerformanceFx->LogicalSectorCacheMisses = logical_sector_cache_misses;

	unsigned long	arp_requests_sent;
	unsigned long	arp_responses_sent;
	unsigned long	arp_requests_received;
	unsigned long	arp_responses_received;
	unsigned long	packet_allocations;
	unsigned long	packet_releases;
	unsigned long	empty_allocations;
	unsigned long	invalid_releases;
	unsigned long	ip_packets_sent;
	unsigned long	ip_bytes_sent;
	unsigned long	ip_packets_received;
	unsigned long	ip_bytes_received;
	unsigned long	pings_sent;
	unsigned long	ping_responses;
	unsigned long	tcp_client_connections;
	unsigned long	tcp_server_connections;
	unsigned long	tcp_packets_sent;
	unsigned long	tcp_bytes_sent;
	unsigned long	tcp_packets_received;
	unsigned long	tcp_bytes_received;
	unsigned long	udp_packets_sent;
	unsigned long	udp_bytes_sent;
	unsigned long	udp_packets_received;
	unsigned long	udp_bytes_received;

	retVal = tml_system_netx_performance_statistics_get(&arp_requests_sent, &arp_responses_sent,
		&arp_requests_received, &arp_responses_received,
		&packet_allocations, &packet_releases,
		&empty_allocations, &invalid_releases,
		&ip_packets_sent, &ip_bytes_sent,
		&ip_packets_received, &ip_bytes_received,
		&pings_sent, &ping_responses,
		&tcp_client_connections, &tcp_server_connections,
		&tcp_packets_sent, &tcp_bytes_sent,
		&tcp_packets_received, &tcp_bytes_received,
		&udp_packets_sent, &udp_bytes_sent,
		&udp_packets_received, &udp_bytes_received);

	PerformanceNx = gcnew TmlPerformanceNx();
	PerformanceNx->ArpRequestsSent = arp_requests_sent;
	PerformanceNx->ArpResponsesSent = arp_responses_sent;
	PerformanceNx->ArpRequestsReceived = arp_requests_received;
	PerformanceNx->ArpResponsesReceived = arp_responses_received;
	PerformanceNx->PacketAllocations = packet_allocations;
	PerformanceNx->PacketReleases = packet_releases;
	PerformanceNx->EmptyAllocations = empty_allocations;
	PerformanceNx->InvalidReleases = invalid_releases;
	PerformanceNx->IpPacketsSent = ip_packets_sent;
	PerformanceNx->IpBytesSent = ip_bytes_sent;
	PerformanceNx->IpPacketsReceived = ip_packets_received;
	PerformanceNx->IpBytesReceived = ip_bytes_received;
	PerformanceNx->PingsSent = pings_sent;
	PerformanceNx->PingResponses = ping_responses;
	PerformanceNx->TcpClientConnections = tcp_client_connections;
	PerformanceNx->TcpServerConnections = tcp_server_connections;
	PerformanceNx->TcpPacketsSent = tcp_packets_sent;
	PerformanceNx->TcpBytesSent = tcp_bytes_sent;
	PerformanceNx->TcpPacketsReceived = tcp_packets_received;
	PerformanceNx->TcpBytesReceived = tcp_bytes_received;
	PerformanceNx->UdpPacketsSent = udp_packets_sent;
	PerformanceNx->UdpBytesSent = udp_bytes_sent;
	PerformanceNx->UdpPacketsReceived = udp_packets_received;
	PerformanceNx->UdpBytesReceived = udp_bytes_received;

	fclose(source_trace_file);

	System::Runtime::InteropServices::Marshal::FreeHGlobal(filePtr);
	return true;
}

void ELTMLManaged::TMLFunctions::Uninitialize()
{
	tml_uninitialize();
}

void ELTMLManaged::TMLFunctions::ThreadExecutionStatus(unsigned long thread_index, unsigned long starting_event, unsigned long ending_event, List<TmlExecutionStatus^>^ executionStatusList, unsigned long max_status_pairs)
{
	unsigned long* execution_status_list;

	if (max_status_pairs > SIZE_MAX / 2)
	{
		return;
	}

	execution_status_list = (unsigned long*)malloc((unsigned long)max_status_pairs * 2);

	unsigned long status_pairs_returned;
	int i = 0, k = 0;

	tml_thread_execution_status_get(thread_index, starting_event, ending_event, execution_status_list, max_status_pairs, &status_pairs_returned);

	if (status_pairs_returned > 0 && status_pairs_returned <= max_status_pairs)
	{
		k = 0;
		for (i = starting_event; i < ending_event; i++)
		{
			TmlExecutionStatus^ tmlExecutionStatus = gcnew TmlExecutionStatus();

			tmlExecutionStatus->EventNumber = execution_status_list[k]; //even
			tmlExecutionStatus->Status = execution_status_list[k + 1];	//odd

			executionStatusList->Add(tmlExecutionStatus);
			k += 2;
			if ((k / 2) == status_pairs_returned)
			{
				break;
			}
		}
	}

	free((unsigned long*)execution_status_list);
}

bool ELTMLManaged::TMLFunctions::RawTraceFileDump(
	String^ file_name,
	String^ tracex_version,
	String^ input_file_name,
	String^ dump_file_name
)
{
	IntPtr filePtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(file_name);
	char* fileName = static_cast<char*>(filePtr.ToPointer());

	System::IO::FileInfo^ fileInfo = gcnew System::IO::FileInfo(file_name);

	FILE* source_trace_file = fopen(fileName, "w");

	IntPtr tracexVersionPtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(tracex_version);
	char* tracexVersion = static_cast<char*>(tracexVersionPtr.ToPointer());

	IntPtr inputFileNamePtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(input_file_name);
	char* inputFileName = static_cast<char*>(inputFileNamePtr.ToPointer());

	IntPtr dumpFileNamePtr = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(dump_file_name);
	char* dumpFileName = static_cast<char*>(dumpFileNamePtr.ToPointer());

	tml_raw_trace_file_dump(source_trace_file, tracexVersion, inputFileName, dumpFileName);

	if (source_trace_file == NULL)
	{
		return false;
	}

	fclose(source_trace_file);

	System::Runtime::InteropServices::Marshal::FreeHGlobal(filePtr);
	System::Runtime::InteropServices::Marshal::FreeHGlobal(tracexVersionPtr);
	System::Runtime::InteropServices::Marshal::FreeHGlobal(inputFileNamePtr);
	System::Runtime::InteropServices::Marshal::FreeHGlobal(dumpFileNamePtr);

	return true;
}
