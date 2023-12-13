// ELTMLManaged.h

#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace AzureRTOS::Tml;

#include "stdio.h"
#include "tml_library.h"

namespace ELTMLManaged
{
	public ref class TMLFunctions
	{
	public:
		static TmlHeaderInfo^ HeaderInfo;
		static TmlPerformance^ Performance;
		static TmlPerformanceNx^ PerformanceNx;
		static TmlPerformanceFx^ PerformanceFx;
		static TmlThreadPerformance^ ThreadPerformance;
		static TmlInterruptIdle^ InterruptIdle;
		static String^ ErrorMessage;

		static bool ExtractTraceInfo(String^ file_name, TmlTrace^ tmlTrace, List<TmlThread^>^ threads, List<TmlObject^>^ objects, List<TmlEvent^>^ events);

		static bool RawTraceFileDump(String^ file_name, String^ tracex_version, String^ input_file_name, String^ dump_file_name);

		static void Uninitialize();

		static void ThreadExecutionStatus(unsigned long thread_index, unsigned long starting_event, unsigned long ending_event, List<TmlExecutionStatus^>^ executionStatusList, unsigned long max_status_pairs);
	};
}
