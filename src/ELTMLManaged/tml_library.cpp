/***************************************************************************
 * Copyright (c) 2024 Microsoft Corporation 
 * 
 * This program and the accompanying materials are made available under the
 * terms of the MIT License which is available at
 * https://opensource.org/licenses/MIT.
 * 
 * SPDX-License-Identifier: MIT
 **************************************************************************/


/**************************************************************************/
/**************************************************************************/
/**                                                                       */ 
/** ThreadX Component                                                     */ 
/**                                                                       */
/**   Trace Management Library (TML)                                      */
/**                                                                       */
/**************************************************************************/
/**************************************************************************/

#include "stdafx.h"
#include "ELTMLManaged.h"
#include "tml_library.h"


/* Define the main data structures of the Trace Management Library (TML).  */

/* Define the control header information.  */

unsigned long   tml_header_trace_id;
unsigned long   tml_header_timer_valid_mask;
unsigned long   tml_header_trace_base_address;
unsigned long	tml_header_object_registry_start_address;
unsigned short	tml_header_reserved1;
unsigned short  tml_header_object_name_size;
unsigned long   tml_header_object_registry_end_address;
unsigned long   tml_header_trace_buffer_start_address;
unsigned long   tml_header_trace_buffer_end_address;
unsigned long   tml_header_trace_buffer_current_address;
unsigned long   tml_header_reserved2;
unsigned long   tml_header_reserved3;
unsigned long   tml_header_reserved4;


TML_OBJECT		*tml_object_array;
unsigned long	*tml_object_thread_list;
unsigned long   tml_total_threads;
unsigned long	tml_total_objects;
unsigned long	tml_max_objects;
unsigned long   tml_total_priority_inversions;
unsigned long	tml_total_bad_priority_inversions;
	

/* This is the array of events for storing in memory.  */

TML_EVENT		*tml_event_array;
unsigned long	tml_total_events;
_int64			tml_relative_ticks;


/* Define the thread status list, that keeps track of the status changes a thread goes through during the trace file.  */

TML_THREAD_STATUS_SUMMARY   *tml_thread_status_list;


/* Define error messages for parsing the trace file.  */

char			tml_header_id_read_error[] =        "Unable to Read Trace File ID";
char			tml_header_id_error[] =             "Invalid Trace File ID";
char			tml_header_read_error[] =           "Unable to Read Trace File Header";
char			tml_object_allocation_error[] =     "Unable to Allocate Memory For Objects";
char			tml_thread_allocation_error[] =     "Unable to Allocate Memory For Threads";
char			tml_object_name_read_error[] =      "Unable to Read Object Name";
char			tml_event_allocation_error[] =      "Unable to Allocate Memory For Events";
char			tml_event_read_error[] =            "Unable to Read Event - Possible Trace File Truncation?  ";
char			tml_memory_calculation_error[] =    "Memory allocate size calculation overflow/underflow";


char			*tml_event_type[] = {"INVALID  ",
                                     "TX INTERNAL RESUME        ",       /* TML_TRACE_THREAD_RESUME                              1   I1 = thread ptr, I2 = previous_state, I3 = stack ptr, I4 = next thread   */ 
                                     "TX INTERNAL SUSPEND       ",       /* TML_TRACE_THREAD_SUSPEND                             2   I1 = thread ptr, I2 = new_state, I3 = stack ptr  I4 = next thread        */ 
                                     "ISR ENTER                 ",       /* TML_TRACE_ISR_ENTER                                  3   I1 = stack_ptr, I2 = ISR number, I3 = system state, I4 = current thread  */ 
									 "ISR EXIT                  ",		/* TML_TRACE_ISR_EXIT                                   4   I1 = stack_ptr, I2 = ISR number (optional, user defined)                 */ 
                                     "TX TIME SLICE             ",       /* TML_TRACE_TIME_SLICE                                 5   I1 = next thread, I2 = system state, I3 = preempt disable, I4 = stack    */
                                     "TX RUNNING                ",       /* TML_TRACE_RUNNING                                    6   None                                                                     */ 
									 "INVALID  ",
									 "INVALID  ",
									 "INVALID  ",
                                     "TX BLOCK ALLOCATE         ",       /* TML_TRACE_BLOCK_ALLOCATE                             10  I1 = pool ptr, I2 = memory ptr, I3 = wait option, I4 = remaining blocks  */ 
                                     "TX BLOCK POOL CREATE      ",       /* TML_TRACE_BLOCK_POOL_CREATE                          11  I1 = pool ptr, I2 = pool_start, I3 = total blocks, I4 = block size       */ 
                                     "TX BLOCK POOL DELETE      ",       /* TML_TRACE_BLOCK_POOL_DELETE                          12  I1 = pool ptr, I2 = stack ptr                                            */ 
                                     "TX BLOCK INFO GET         ",       /* TML_TRACE_BLOCK_POOL_INFO_GET                        13  I1 = pool ptr                                                            */
                                     "TX BLOCK PERF GET         ",       /* TML_TRACE_BLOCK_POOL_PERFORMANCE_INFO_GET            14  I1 = pool ptr                                                            */ 
                                     "TX BLOCK SYS PERF GET     ",       /* TML_TRACE_BLOCK_POOL_PERFORMANCE_SYSTEM_INFO_GET     15  None                                                                     */ 
                                     "TX BLOCK PRIORITIZE       ",       /* TML_TRACE_BLOCK_POOL_PRIORITIZE                      16  I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
                                     "TX BLOCK RELEASE          ",       /* TML_TRACE_BLOCK_RELEASE                              17  I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = stack ptr           */ 
									 "INVALID  ",
									 "INVALID  ",
                                     "TX BYTE ALLOCATE          ",       /* TML_TRACE_BYTE_ALLOCATE                              20  I1 = pool ptr, I2 = memory ptr, I3 = size requested, I4 = wait option    */ 
                                     "TX BYTE POOL CREATE       ",       /* TML_TRACE_BYTE_POOL_CREATE                           21  I1 = pool ptr, I2 = start ptr, I3 = pool size, I4 = stack ptr            */ 
                                     "TX BYTE POOL DELETE       ",       /* TML_TRACE_BYTE_POOL_DELETE                           22  I1 = pool ptr, I2 = stack ptr                                            */ 
                                     "TX BYTE INFO GET          ",       /* TML_TRACE_BYTE_POOL_INFO_GET                         23  I1 = pool ptr                                                            */
                                     "TX BYTE PERF GET          ",       /* TML_TRACE_BYTE_POOL_PERFORMANCE_INFO_GET             24  I1 = pool ptr                                                            */
                                     "TX BYTE SYS PERF GET      ",       /* TML_TRACE_BYTE_POOL_PERFORMANCE_SYSTEM_INFO_GET      25  None                                                                     */
                                     "TX BYTE PRIORITIZE        ",       /* TML_TRACE_BYTE_POOL_PRIORITIZE                       26  I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
                                     "TX BYTE RELEASE           ",       /* TML_TRACE_BYTE_RELEASE                               27  I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = available bytes     */ 
									 "INVALID  ", 
									 "INVALID  ",
                                     "TX EVENT FLAG CREATE      ",       /* TML_TRACE_EVENT_FLAGS_CREATE                         30  I1 = group ptr, I2 = stack ptr                                           */ 
                                     "TX EVENT FLAG DELETE      ",       /* TML_TRACE_EVENT_FLAGS_DELETE                         31  I1 = group ptr, I2 = stack ptr                                           */ 
                                     "TX EVENT FLAG GET         ",       /* TML_TRACE_EVENT_FLAGS_GET                            32  I1 = group ptr, I2 = requested flags, I3 = current flags, I4 = get option*/ 
                                     "TX EVENT FLAG INFO GET    ",       /* TML_TRACE_EVENT_FLAGS_INFO_GET                       33  I1 = group ptr                                                           */
                                     "TX EVENT FLAG PERF GET    ",       /* TML_TRACE_EVENT_FLAGS_PERFORMANCE_INFO_GET           34  I1 = group ptr                                                           */
                                     "TX EVENT FLAG SYSP GET    ",       /* TML_TRACE_EVENT_FLAGS_PERFORMANCE_SYSTEM_INFO_GET    35  None                                                                     */
                                     "TX EVENT FLAG SET         ",       /* TML_TRACE_EVENT_FLAGS_SET                            36  I1 = group ptr, I2 = flags to set, I3 = set option, I4= suspended count  */
                                     "TX EVENT FLAG NOTIFY      ",       /* TML_TRACE_EVENT_FLAGS_SET_NOTIFY                     37  I1 = group ptr                                                           */
									 "INVALID  ", 
									 "INVALID  ", 
                                     "INT CNTRL                 ",       /* TML_TRACE_INTERRUPT_CONTROL                          40  I1 = new interrupt posture, I2 = stack ptr                               */
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
                                     "TX MUTEX CREATE           ",       /* TML_TRACE_MUTEX_CREATE                               50   I1 = mutex ptr, I2 = inheritance, I3 = stack ptr                         */
                                     "TX MUTEX DELETE           ",       /* TML_TRACE_MUTEX_DELETE                               51   I1 = mutex ptr, I2 = stack ptr                                           */
                                     "TX MUTEX GET              ",       /* TML_TRACE_MUTEX_GET                                  52   I1 = mutex ptr, I2 = wait option, I3 = owning thread, I4 = own count     */
                                     "TX MUTEX INFO GET         ",       /* TML_TRACE_MUTEX_INFO_GET                             53   I1 = mutex ptr                                                           */
                                     "TX MUTEX PERF GET         ",       /* TML_TRACE_MUTEX_PERFORMANCE_INFO_GET                 54   I1 = mutex ptr                                                           */
                                     "TX MUTEX SYS PERF GET     ",       /* TML_TRACE_MUTEX_PERFORMANCE_SYSTEM_INFO_GET          55   None                                                                     */
                                     "TX MUTEX PRIORITIZE       ",       /* TML_TRACE_MUTEX_PRIORITIZE                           56   I1 = mutex ptr, I2 = suspended count, I3 = stack ptr                     */
                                     "TX MUTEX PUT              ",       /* TML_TRACE_MUTEX_PUT                                  57   I1 = mutex ptr, I2 = owning thread, I3 = own count, I4 = stack ptr       */
									 "INVALID  ", 
									 "INVALID  ", 
                                     "TX QUEUE CREATE           ",       /* TML_TRACE_QUEUE_CREATE                               60   I1 = queue ptr, I2 = message size, I3 = queue start, I4 = queue size     */
                                     "TX QUEUE DELETE           ",       /* TML_TRACE_QUEUE_DELETE                               61   I1 = queue ptr, I2 = stack ptr                                           */
                                     "TX QUEUE FLUSH            ",       /* TML_TRACE_QUEUE_FLUSH                                62   I1 = queue ptr, I2 = stack ptr                                           */
                                     "TX QUEUE FRONT SEND       ",       /* TML_TRACE_QUEUE_FRONT_SEND                           63   I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued         */
                                     "TX QUEUE INFO GET         ",       /* TML_TRACE_QUEUE_INFO_GET                             64   I1 = queue ptr                                                           */
                                     "TX QUEUE PERF GET         ",       /* TML_TRACE_QUEUE_PERFORMANCE_INFO_GET                 65   I1 = queue ptr                                                           */
                                     "TX QUEUE SYS PERF GET     ",       /* TML_TRACE_QUEUE_PERFORMANCE_SYSTEM_INFO_GET          66   None                                                                     */
                                     "TX QUEUE PRIORITIZE       ",       /* TML_TRACE_QUEUE_PRIORITIZE                           67   I1 = queue ptr, I2 = suspended count, I3 = stack ptr                     */
                                     "TX QUEUE RECEIVE          ",       /* TML_TRACE_QUEUE_RECEIVE                              68   I1 = queue ptr, I2 = destination ptr, I3 = wait option, I4 = enqueued    */
                                     "TX QUEUE SEND             ",       /* TML_TRACE_QUEUE_SEND                                 69   I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued         */
                                     "TX QUEUE NOTIFY           ",       /* TML_TRACE_QUEUE_SEND_NOTIFY                          70   I1 = queue ptr                                                           */
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
                                     "TX SEMAPHORE CEIL PUT     ",       /* TML_TRACE_SEMAPHORE_CEILING_PUT                      80   I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4 =ceiling */
                                     "TX SEMAPHORE CREATE       ",       /* TML_TRACE_SEMAPHORE_CREATE                           81   I1 = semaphore ptr, I2 = initial count, I3 = stack ptr                   */
                                     "TX SEMAPHORE DELETE       ",       /* TML_TRACE_SEMAPHORE_DELETE                           82   I1 = semaphore ptr, I2 = stack ptr                                       */
                                     "TX SEMAPHORE GET          ",       /* TML_TRACE_SEMAPHORE_GET                              83   I1 = semaphore ptr, I2 = wait option, I3 = current count, I4 = stack ptr */
                                     "TX SEMAPHORE INFO GET     ",       /* TML_TRACE_SEMAPHORE_INFO_GET                         84   I1 = semaphore ptr                                                       */
                                     "TX SEMAPHORE PERF GET     ",       /* TML_TRACE_SEMAPHORE_PERFORMANCE_INFO_GET             85   I1 = semaphore ptr                                                       */
                                     "TX SEMAPHORE SYS PERF     ",       /* TML_TRACE_SEMAPHORE_PERFORMANCE_SYSTEM_INFO_GET      86   None                                                                     */
                                     "TX SEMAPHORE PRIORITIZE   ",       /* TML_TRACE_SEMAPHORE_PRIORITIZE                       87   I1 = semaphore ptr, I2 = suspended count, I2 = stack ptr                 */
                                     "TX SEMAPHORE PUT          ",       /* TML_TRACE_SEMAPHORE_PUT                              88   I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4=stack ptr*/
                                     "TX SEMAPHORE NOTIFY       ",       /* TML_TRACE_SEMAPHORE_PUT_NOTIFY                       89   I1 = semaphore ptr                                                       */
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
									 "INVALID  ", 
                                     "TX THREAD CREATE          ",       /* TML_TRACE_THREAD_CREATE                              100  I1 = thread ptr, I2 = priority, I3 = stack ptr, I4 = stack_size          */
                                     "TX THREAD DELETE          ",       /* TML_TRACE_THREAD_DELETE                              101  I1 = thread ptr, I2 = stack ptr                                          */
                                     "TX THREAD NOTIFY          ",       /* TML_TRACE_THREAD_ENTRY_EXIT_NOTIFY                   102  I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
                                     "TX THREAD IDENTIFY        ",       /* TML_TRACE_THREAD_IDENTIFY                            103  None                                                                     */
                                     "TX THREAD INFO GET        ",       /* TML_TRACE_THREAD_INFO_GET                            104  I1 = thread ptr, I2 = thread state                                       */
                                     "TX THREAD PERF GET        ",       /* TML_TRACE_THREAD_PERFORMANCE_INFO_GET                105  I1 = thread ptr, I2 = thread state                                       */
                                     "TX THREAD SYS PERF GET    ",       /* TML_TRACE_THREAD_PERFORMANCE_SYSTEM_INFO_GET         106  None                                                                     */
                                     "TX THREAD PREEMPT CHNG    ",       /* TML_TRACE_THREAD_PREEMPTION_CHANGE                   107  I1 = thread ptr, I2 = new threshold, I3 = old threshold, I4 =thread state*/
                                     "TX THREAD PRIORITY CHNG   ",       /* TML_TRACE_THREAD_PRIORITY_CHANGE                     108  I1 = thread ptr, I2 = new priority, I3 = old priority, I4 = thread state */
                                     "TX THREAD RELINQUISH      ",       /* TML_TRACE_THREAD_RELINQUISH                          109  I1 = stack ptr, I2 = next thread                                         */
                                     "TX THREAD RESET           ",       /* TML_TRACE_THREAD_RESET                               110  I1 = thread ptr, I2 = thread state                                       */
                                     "TX THREAD RESUME          ",       /* TML_TRACE_THREAD_RESUME_API                          111  I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
                                     "TX THREAD SLEEP           ",       /* TML_TRACE_THREAD_SLEEP                               112  I1 = sleep value, I2 = thread state, I3 = stack ptr                      */
                                     "TX THREAD STACK NOTIFY    ",       /* TML_TRACE_THREAD_STACK_ERROR_NOTIFY                  113  None                                                                     */
                                     "TX THREAD SUSPEND         ",       /* TML_TRACE_THREAD_SUSPEND_API                         114  I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
                                     "TX THREAD TERMINATE       ",       /* TML_TRACE_THREAD_TERMINATE                           115  I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
                                     "TX THREAD TIMESLICE CHN   ",       /* TML_TRACE_THREAD_TIME_SLICE_CHANGE                   116  I1 = thread ptr, I2 = new timeslice, I3 = old timeslice                  */
                                     "TX THREAD WAIT ABORT      ",       /* TML_TRACE_THREAD_WAIT_ABORT                          117  I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
									 "INVALID  ", 
									 "INVALID  ", 
                                     "TX TIME GET               ",       /* TML_TRACE_TIME_GET                                   120  I1 = current time, I2 = stack ptr                                        */
                                     "TX TIME SET               ",       /* TML_TRACE_TIME_SET                                   121  I1 = new time                                                            */
                                     "TX TIMER ACTIVATE         ",       /* TML_TRACE_TIMER_ACTIVATE                             122  I1 = timer ptr                                                           */
                                     "TX TIMER CHANGE           ",       /* TML_TRACE_TIMER_CHANGE                               123  I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks                 */ 
                                     "TX TIMER CREATE           ",       /* TML_TRACE_TIMER_CREATE                               124  I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks, I4 = enable    */
                                     "TX TIMER DEACTIVATE       ",       /* TML_TRACE_TIMER_DEACTIVATE                           125  I1 = timer ptr, I2 = stack ptr                                           */
                                     "TX TIMER DELETE           ",       /* TML_TRACE_TIMER_DELETE                               126  I1 = timer ptr                                                           */
                                     "TX TIMER INFO GET         ",       /* TML_TRACE_TIMER_INFO_GET                             127  I1 = timer ptr, I2 = stack ptr                                           */
                                     "TX TIMER PERF GET         ",       /* TML_TRACE_TIMER_PERFORMANCE_INFO_GET                 128  I1 = timer ptr                                                           */
                                     "TX TIMER SYS PERF GET     ",       /* TML_TRACE_TIMER_PERFORMANCE_SYSTEM_INFO_GET          129  None                                                                     */
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "FX INTERNAL SEC MISS      ",       /* TML_FX_TRACE_INTERNAL_LOG_SECTOR_CACHE_MISS          201  I1 = media ptr, I2 = sector, I3 = total misses, I4 = cache size          */
                                     "FX INTERNAL DIR MISS      ",       /* TML_FX_TRACE_INTERNAL_DIR_CACHE_MISS                 202  I1 = media ptr, I2 = total misses                                        */
                                     "FX INTERNAL MEDIA FLUSH   ",       /* TML_FX_TRACE_INTERNAL_MEDIA_FLUSH                    203  I1 = media ptr, I2 = dirty sectors                                       */
                                     "FX INTERNAL DIR READ      ",       /* TML_FX_TRACE_INTERNAL_DIR_ENTRY_READ                 204  I1 = media ptr                                                           */
                                     "FX INTERNAL DIR WRITE     ",       /* TML_FX_TRACE_INTERNAL_DIR_ENTRY_WRITE                205  I1 = media ptr                                                           */
                                     "FX INTERNAL IO READ       ",       /* TML_FX_TRACE_IO_DRIVER_READ                          206  I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer         */
                                     "FX INTERNAL IO WRITE      ",       /* TML_FX_TRACE_IO_DRIVER_WRITE                         207  I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer         */
                                     "FX INTERNAL IO FLUSH      ",       /* TML_FX_TRACE_IO_DRIVER_FLUSH                         208  I1 = media ptr                                                           */
                                     "FX INTERNAL IO ABORT      ",       /* TML_FX_TRACE_IO_DRIVER_ABORT                         209  I1 = media ptr                                                           */
                                     "FX INTERNAL IO INIT       ",       /* TML_FX_TRACE_IO_DRIVER_INIT                          210  I1 = media ptr                                                           */
                                     "FX INTERNAL IO BOOT RD    ",       /* TML_FX_TRACE_IO_DRIVER_BOOT_READ                     211  I1 = media ptr, I2 = buffer                                              */
                                     "FX INTERNAL IO RELEASE    ",       /* TML_FX_TRACE_IO_DRIVER_RELEASE_SECTORS               212  I1 = media ptr, I2 = sector, I3 = number of sectors                      */
                                     "FX INTERNAL IO BOOT WRT   ",       /* TML_FX_TRACE_IO_DRIVER_BOOT_WRITE                    213  I1 = media ptr, I2 = buffer                                              */
                                     "FX INTERNAL IO UN-INIT    ",       /* TML_FX_TRACE_IO_DRIVER_UNINIT                        214  I1 = media ptr                                                           */
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "FX DIR ATTRIBUTES READ    ",       /* TML_FX_TRACE_DIRECTORY_ATTRIBUTES_READ               220  I1 = media ptr, I2 = directory name, I3 = attributes                     */ 
                                     "FX DIR ATTRIBUTES SET     ",       /* TML_FX_TRACE_DIRECTORY_ATTRIBUTES_SET                221  I1 = media ptr, I2 = directory name, I3 = attributes                     */ 
                                     "FX DIR CREATE             ",       /* TML_FX_TRACE_DIRECTORY_CREATE                        222  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR DEFAULT GET        ",       /* TML_FX_TRACE_DIRECTORY_DEFAULT_GET                   223  I1 = media ptr, I2 = return path name                                    */ 
                                     "FX DIR DEFAULT SET        ",       /* TML_FX_TRACE_DIRECTORY_DEFAULT_SET                   224  I1 = media ptr, I2 = new path name                                       */ 
                                     "FX DIR DELETE             ",       /* TML_FX_TRACE_DIRECTORY_DELETE                        225  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR FIRST ENTRY FIND   ",       /* TML_FX_TRACE_DIRECTORY_FIRST_ENTRY_FIND              226  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR FIRST FULL ENTRY   ",       /* TML_FX_TRACE_DIRECTORY_FIRST_FULL_ENTRY_FIND         227  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR INFO GET           ",       /* TML_FX_TRACE_DIRECTORY_INFORMATION_GET               228  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR LOCAL PATH CLEAR   ",       /* TML_FX_TRACE_DIRECTORY_LOCAL_PATH_CLEAR              229  I1 = media ptr                                                           */ 
                                     "FX DIR LOCAL PATH GET     ",       /* TML_FX_TRACE_DIRECTORY_LOCAL_PATH_GET                230  I1 = media ptr, I2 = return path name                                    */ 
                                     "FX DIR LOCAL PATH RESTO   ",       /* TML_FX_TRACE_DIRECTORY_LOCAL_PATH_RESTORE            231  I1 = media ptr, I2 = local path ptr                                      */ 
                                     "FX DIR LOCAL PATH SET     ",       /* TML_FX_TRACE_DIRECTORY_LOCAL_PATH_SET                232  I1 = media ptr, I2 = local path ptr, I3 = new path name                  */ 
                                     "FX DIR LONG NAME GET      ",       /* TML_FX_TRACE_DIRECTORY_LONG_NAME_GET                 233  I1 = media ptr, I2 = short file name, I3 = long file name                */ 
                                     "FX DIR NAME TEST          ",       /* TML_FX_TRACE_DIRECTORY_NAME_TEST                     234  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR NEXT ENTRY FIND    ",       /* TML_FX_TRACE_DIRECTORY_NEXT_ENTRY_FIND               235  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR NEXT FULL ENTRY    ",       /* TML_FX_TRACE_DIRECTORY_NEXT_FULL_ENTRY_FIND          236  I1 = media ptr, I2 = directory name                                      */ 
                                     "FX DIR RENAME             ",       /* TML_FX_TRACE_DIRECTORY_RENAME                        237  I1 = media ptr, I2 = old directory name, I3 = new directory name         */
                                     "FX DIR SHORT NAME GET     ",       /* TML_FX_TRACE_DIRECTORY_SHORT_NAME_GET                238  I1 = media ptr, I2 = long file name, I3 = short file name                */ 
                                     "FX FILE ALLOCATE          ",       /* TML_FX_TRACE_FILE_ALLOCATE                           239  I1 = file ptr, I2 = size I3 = previous size, I4 = new size               */ 
                                     "FX FILE ATTRIBUTE READ    ",       /* TML_FX_TRACE_FILE_ATTRIBUTES_READ                    240  I1 = media ptr, I2 = file name, I3 = attributes                          */ 
                                     "FX FILE ATTRIBUTE SET     ",       /* TML_FX_TRACE_FILE_ATTRIBUTES_SET                     241  I1 = media ptr, I2 = file name, I3 = attributes                          */ 
                                     "FX FILE BEST ALLOCATE     ",       /* TML_FX_TRACE_FILE_BEST_EFFORT_ALLOCATE               242  I1 = file ptr, I2 = size, I3 = actual_size_allocated                     */ 
                                     "FX FILE CLOSE             ",       /* TML_FX_TRACE_FILE_CLOSE                              243  I1 = file ptr, I3 = file size                                            */ 
                                     "FX FILE CREATE            ",       /* TML_FX_TRACE_FILE_CREATE                             244  I1 = media ptr, I2 = file name                                           */ 
                                     "FX FILE DATE TIME SET     ",       /* TML_FX_TRACE_FILE_DATE_TIME_SET                      245  I1 = media ptr, I2 = file name, I3 = year, I4 = month                    */ 
                                     "FX FILE DELETE            ",       /* TML_FX_TRACE_FILE_DELETE                             246  I1 = media ptr, I2 = file name                                           */ 
                                     "FX FILE OPEN              ",       /* TML_FX_TRACE_FILE_OPEN                               247  I1 = media ptr, I2 = file ptr, I3 = file name, I4 = open type            */ 
                                     "FX FILE READ              ",       /* TML_FX_TRACE_FILE_READ                               248  I1 = file ptr, I2 = buffer ptr, I3 = request size I4 = actual size       */ 
                                     "FX FILE RELATIVE SEEK     ",       /* TML_FX_TRACE_FILE_RELATIVE_SEEK                      249  I1 = file ptr, I2 = byte offset, I3 = seek from, I4 = previous offset    */ 
                                     "FX FILE RENAME            ",       /* TML_FX_TRACE_FILE_RENAME                             250  I1 = media ptr, I2 = old file name, I3 = new file name                   */ 
                                     "FX FILE SEEK              ",       /* TML_FX_TRACE_FILE_SEEK                               251  I1 = file ptr, I2 = byte offset, I3 = previous offset                    */ 
                                     "FX FILE TRUNCATE          ",       /* TML_FX_TRACE_FILE_TRUNCATE                           252  I1 = file ptr, I2 = size, I3 = previous size, I4 = new size              */ 
                                     "FX FILE TRUNCATE RELEAS   ",       /* TML_FX_TRACE_FILE_TRUNCATE_RELEASE                   253  I1 = file ptr, I2 = size, I3 = previous size, I4 = new size              */ 
                                     "FX FILE WRITE             ",       /* TML_FX_TRACE_FILE_WRITE                              254  I1 = file ptr, I2 = buffer ptr, I3 = size, I4 = bytes written            */ 
                                     "FX MEDIA ABORT            ",       /* TML_FX_TRACE_MEDIA_ABORT                             255  I1 = media ptr                                                           */ 
                                     "FX MEDIA CACHE INVALID    ",       /* TML_FX_TRACE_MEDIA_CACHE_INVALIDATE                  256  I1 = media ptr                                                           */ 
                                     "FX MEDIA CHECK            ",       /* TML_FX_TRACE_MEDIA_CHECK                             257  I1 = media ptr, I2 = scratch memory, I3 = scratch memory size, I4 =errors*/
                                     "FX MEDIA CLOSE            ",       /* TML_FX_TRACE_MEDIA_CLOSE                             258  I1 = media ptr                                                           */ 
                                     "FX MEDIA FLUSH            ",       /* TML_FX_TRACE_MEDIA_FLUSH                             259  I1 = media ptr                                                           */ 
                                     "FX MEDIA FORMAT           ",       /* TML_FX_TRACE_MEDIA_FORMAT                            260  I1 = media ptr, I2 = root entries, I3 = sectors, I4 = sectors per cluster*/
                                     "FX MEDIA OPEN             ",       /* TML_FX_TRACE_MEDIA_OPEN                              261  I1 = media ptr, I2 = media driver, I3 = memory ptr, I4 = memory size     */ 
                                     "FX MEDIA READ             ",       /* TML_FX_TRACE_MEDIA_READ                              262  I1 = media ptr, I2 = logical sector, I3 = buffer ptr, I4 = bytes read    */ 
                                     "FX MEDIA SPACE AVAILABL   ",       /* TML_FX_TRACE_MEDIA_SPACE_AVAILABLE                   263  I1 = media ptr, I2 = available bytes ptr, I3 = available clusters        */ 
                                     "FX MEDIA VOLUME GET       ",       /* TML_FX_TRACE_MEDIA_VOLUME_GET                        264  I1 = media ptr, I2 = volume name, I3 = volume source                     */ 
                                     "FX MEDIA VOLUME SET       ",       /* TML_FX_TRACE_MEDIA_VOLUME_SET                        265  I1 = media ptr, I2 = volume name                                         */
                                     "FX MEDIA WRITE            ",       /* TML_FX_TRACE_MEDIA_WRITE                             266  I1 = media ptr, I2 = logical_sector, I3 = buffer_ptr, I4 = byte written  */ 
                                     "FX SYSTEM DATE GET        ",       /* TML_FX_TRACE_SYSTEM_DATE_GET                         267  I1 = year, I2 = month, I3 = day                                          */ 
                                     "FX SYSTEM DATE SET        ",       /* TML_FX_TRACE_SYSTEM_DATE_SET                         268  I1 = year, I2 = month, I3 = day                                          */ 
                                     "FX SYSTEM INITIALIZE      ",       /* TML_FX_TRACE_SYSTEM_INITIALIZE                       269  None                                                                     */ 
                                     "FX SYSTEM TIME GET        ",       /* TML_FX_TRACE_SYSTEM_TIME_GET                         270  I1 = hour, I2 = minute, I3 = second                                      */ 
                                     "FX SYSTEM TIME SET        ",       /* TML_FX_TRACE_SYSTEM_TIME_SET                         271  I1 = hour, I2 = minute, I3 = second                                      */ 
                                     "FX UNICODE DIR CREATE     ",       /* TML_FX_TRACE_UNICODE_DIRECTORY_CREATE                272  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short_name */
                                     "FX UNICODE DIR RENAME     ",       /* TML_FX_TRACE_UNICODE_DIRECTORY_RENAME                273  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new_name   */
                                     "FX UNICODE FILE CREATE    ",       /* TML_FX_TRACE_UNICODE_FILE_CREATE                     274  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short name */ 
                                     "FX UNICODE FILE RENAME    ",       /* TML_FX_TRACE_UNICODE_FILE_RENAME                     275  I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new name   */ 
                                     "FX UNICODE LENGTH GET     ",       /* TML_FX_TRACE_UNICODE_LENGTH_GET                      276  I1 = unicode name, I2 = length                                           */ 
                                     "FX UNICODE NAME GET       ",       /* TML_FX_TRACE_UNICODE_NAME_GET                        277  I1 = media ptr, I2 = source short name, I3 = unicode name, I4 = length   */ 
                                     "FX UNICODE SHORT NAME     ",       /* TML_FX_TRACE_UNICODE_SHORT_NAME_GET                  278  I1 = media ptr, I2 = source unicode name, I3 = length, I4 =  short name  */ 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "INVALID  ", 
                                     "NX INTERNAL ARP REQ RX    ",       /* TML_NX_TRACE_INTERNAL_ARP_REQUEST_RECEIVE            300  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = active        */ 
                                     "NX INTERNAL ARP REQ TX    ",       /* TML_NX_TRACE_INTERNAL_ARP_REQUEST_SEND               301  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = active   */ 
                                     "NX INTERNAL ARP RES RX    ",       /* TML_NX_TRACE_INTERNAL_ARP_RESPONSE_RECEIVE           302  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = active   */
                                     "NX INTERNAL ARP RES TX    ",       /* TML_NX_TRACE_INTERNAL_ARP_RESPONSE_SEND              303  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = active   */
                                     "NX INTERNAL ICMP RX       ",       /* TML_NX_TRACE_INTERNAL_ICMP_RECEIVE                   304  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0 */
                                     "NX INTERNAL ICMP TX       ",       /* TML_NX_TRACE_INTERNAL_ICMP_SEND                      305  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = header 0 */
                                     "NX INTERNAL IGMP RX       ",       /* TML_NX_TRACE_INTERNAL_IGMP_RECEIVE                   306  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0 */
                                     "INVALID                   ", 
                                     "NX INTERNAL IP RX         ",       /* TML_NX_TRACE_INTERNAL_IP_RECEIVE                     308  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = packet length */
                                     "NX INTERNAL IP TX         ",       /* TML_NX_TRACE_INTERNAL_IP_SEND                        309  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = length   */
                                     "NX INTERNAL TCP DATA RX   ",       /* TML_NX_TRACE_INTERNAL_TCP_DATA_RECEIVE               310  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = sequence      */
                                     "NX INTERNAL TCP DATA TX   ",       /* TML_NX_TRACE_INTERNAL_TCP_DATA_SEND                  311  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP FIN RX    ",       /* TML_NX_TRACE_INTERNAL_TCP_FIN_RECEIVE                312  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP FIN TX    ",       /* TML_NX_TRACE_INTERNAL_TCP_FIN_SEND                   313  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP RST RX    ",       /* TML_NX_TRACE_INTERNAL_TCP_RESET_RECEIVE              314  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP RST TX    ",       /* TML_NX_TRACE_INTERNAL_TCP_RESET_SEND                 315  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP SYN RX    ",       /* TML_NX_TRACE_INTERNAL_TCP_SYN_RECEIVE                316  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP SYN TX    ",       /* TML_NX_TRACE_INTERNAL_TCP_SYN_SEND                   317  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = sequence */
                                     "NX INTERNAL TCP UDP RX    ",       /* TML_NX_TRACE_INTERNAL_UDP_RECEIVE                    318  I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0 */
                                     "NX INTERNAL TCP UDP TX    ",       /* TML_NX_TRACE_INTERNAL_UDP_SEND                       319  I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = header 0 */
                                     "NX INTERNAL RARP RX       ",       /* TML_NX_TRACE_INTERNAL_RARP_RECEIVE                   320  I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1 */
                                     "NX INTERNAL RARP TX       ",       /* TML_NX_TRACE_INTERNAL_RARP_SEND                      321  I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1 */
                                     "NX INTERNAL TCP RETRY     ",       /* TML_NX_TRACE_INTERNAL_TCP_RETRY                      322  I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = number of retries    */
                                     "NX INTERNAL TCP STATE C   ",       /* TML_NX_TRACE_INTERNAL_TCP_STATE_CHANGE               323  I1 = ip ptr, I2 = socket ptr, I3 = previous state, I4 = new state        */
                                     "NX INTERNAL IO DRIVER     ",       /* TML_NX_TRACE_IO_DRIVER_REQUEST                       324  I1 = ip ptr, I2 = command, I3 = packet ptr                               */
                                     "INVALID  ",  /* 325  */
                                     "INVALID  ", /* 326  */
                                     "INVALID  ", /* 327  */
                                     "INVALID  ", /* 328  */
                                     "INVALID  ", /* 329  */
                                     "INVALID  ", /* 330  */
                                     "INVALID  ", /* 331  */
                                     "INVALID  ", /* 332  */
                                     "INVALID  ", /* 333  */
                                     "INVALID  ", /* 334  */
                                     "INVALID  ", /* 335  */
                                     "INVALID  ", /* 336  */
                                     "INVALID  ", /* 337  */
                                     "INVALID  ", /* 338  */
                                     "INVALID  ", /* 339  */
                                     "INVALID  ", /* 340  */
                                     "INVALID  ", /* 341  */
                                     "INVALID  ", /* 342  */
                                     "INVALID  ", /* 343  */
                                     "INVALID  ", /* 344  */
                                     "INVALID  ", /* 345  */
                                     "INVALID  ", /* 346  */
                                     "INVALID  ", /* 347  */
                                     "INVALID  ", /* 348  */
                                     "INVALID  ", /* 349  */
                                     "NX ARP DYN ENTRY INVALI   ",       /* TML_NX_TRACE_ARP_DYNAMIC_ENTRIES_INVALIDATE          350  I1 = ip ptr, I2 = entries invalidated                                    */ 
                                     "NX ARP DYN ENTRY SET      ",       /* TML_NX_TRACE_ARP_DYNAMIC_ENTRY_SET                   351  I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw       */ 
                                     "NX ARP ENABLE             ",       /* TML_NX_TRACE_ARP_ENABLE                              352  I1 = ip ptr, I2 = arp cache memory, I3 = arp cache size                  */ 
                                     "NX ARP GRATUITOUS TX      ",       /* TML_NX_TRACE_ARP_GRATUITOUS_SEND                     353  I1 = ip ptr                                                              */ 
                                     "NX ARP HW ADDRESS FIND    ",       /* TML_NX_TRACE_ARP_HARDWARE_ADDRESS_FIND               354  I1 = ip ptr, I2 = ip_address, I3 = physical msw, I4 = physical lsw       */ 
                                     "NX ARP INFO GET           ",       /* TML_NX_TRACE_ARP_INFO_GET                            355  I1 = ip ptr, I2 = arps sent, I3 = arp responses, I3 = arps received      */ 
                                     "NX ARP IP ADDRESS FIND    ",       /* TML_NX_TRACE_ARP_IP_ADDRESS_FIND                     356  I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw       */ 
                                     "NX ARP STAT ENTRIES DEL   ",       /* TML_NX_TRACE_ARP_STATIC_ENTRIES_DELETE               357  I1 = ip ptr, I2 = entries deleted                                        */ 
                                     "NX ARP STAT ENTRY CREAT   ",       /* TML_NX_TRACE_ARP_STATIC_ENTRY_CREATE                 358  I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical_lsw       */ 
                                     "NX ARP STAT ENTRY DELET   ",       /* TML_NX_TRACE_ARP_STATIC_ENTRY_DELETE                 359  I1 = ip ptr, I2 = ip address, I3 = physical_msw, I4 = physical_lsw       */  
                                     "NX ICMP ENABLE            ",       /* TML_NX_TRACE_ICMP_ENABLE                             360  I1 = ip ptr                                                              */ 
                                     "NX ICMP INFO GET          ",       /* TML_NX_TRACE_ICMP_INFO_GET                           361  I1 = ip ptr, I2 = pings sent, I3 = ping responses, I4 = pings received   */ 
                                     "NX ICMP PING              ",       /* TML_NX_TRACE_ICMP_PING                               362  I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size              */ 
                                     "NX IGMP ENABLE            ",       /* TML_NX_TRACE_IGMP_ENABLE                             363  I1 = ip ptr                                                              */ 
                                     "NX IGMP INFO GET          ",       /* TML_NX_TRACE_IGMP_INFO_GET                           364  I1 = ip ptr, I2 = reports sent, I3 = queries received, I4 = groups joined*/
                                     "NX IGMP LOOPBCK DISABLE   ",       /* TML_NX_TRACE_IGMP_LOOPBACK_DISABLE                   365  I1 = ip ptr                                                              */ 
                                     "NX IGMP LOOPBACK ENABLE   ",       /* TML_NX_TRACE_IGMP_LOOPBACK_ENABLE                    366  I1 = ip ptr                                                              */ 
                                     "NX IGMP MULTICAST JOIN    ",       /* TML_NX_TRACE_IGMP_MULTICAST_JOIN                     367  I1 = ip ptr, I2 = group address                                          */ 
                                     "NX IGMP MULTICAST LEAVE   ",       /* TML_NX_TRACE_IGMP_MULTICAST_LEAVE                    368  I1 = ip ptr, I2 = group_address                                          */ 
                                     "NX IP ADDR CHANGE NOTIF   ",       /* TML_NX_TRACE_IP_ADDRESS_CHANGE_NOTIFY                369  I1 = ip ptr, I2 = ip address change notify, I3 = additional info         */ 
                                     "NX IP ADDRESS GET         ",       /* TML_NX_TRACE_IP_ADDRESS_GET                          370  I1 = ip ptr, I2 = ip address, I3 = network_mask                          */ 
                                     "NX IP ADDRESS SET         ",       /* TML_NX_TRACE_IP_ADDRESS_SET                          371  I1 = ip ptr, I2 = ip address, I3 = network_mask                          */ 
                                     "NX IP CREATE              ",       /* TML_NX_TRACE_IP_CREATE                               372  I1 = ip ptr, I2 = ip address, I3 = network mask, I4 = default_pool       */  
                                     "NX IP DELETE              ",       /* TML_NX_TRACE_IP_DELETE                               373  I1 = ip ptr                                                              */ 
                                     "NX IP DRIVER DIRECT CMD   ",       /* TML_NX_TRACE_IP_DRIVER_DIRECT_COMMAND                374  I1 = ip ptr, I2 = command, I3 = return value                             */ 
                                     "NX IP FORWARD DISABLE     ",       /* TML_NX_TRACE_IP_FORWARDING_DISABLE                   375  I1 = ip ptr                                                              */ 
                                     "NX IP FORWARD ENABLE      ",       /* TML_NX_TRACE_IP_FORWARDING_ENABLE                    376  I1 = ip ptr                                                              */ 
                                     "NX IP FRAGMENT DISABLE    ",       /* TML_NX_TRACE_IP_FRAGMENT_DISABLE                     377  I1 = ip ptr                                                              */ 
                                     "NX IP FRAGMENT ENABLE     ",       /* TML_NX_TRACE_IP_FRAGMENT_ENABLE                      378  I1 = ip ptr                                                              */ 
                                     "NX IP GATEWAY ADDR SET    ",       /* TML_NX_TRACE_IP_GATEWAY_ADDRESS_SET                  379  I1 = ip ptr, I2 = gateway address                                        */ 
                                     "NX IP INFO GET            ",       /* TML_NX_TRACE_IP_INFO_GET                             380  I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = packets dropped  */ 
                                     "NX IP RAW PACKET DISABL   ",       /* TML_NX_TRACE_IP_RAW_PACKET_DISABLE                   381  I1 = ip ptr                                                              */ 
                                     "NX IP RAW PACKET ENABLE   ",       /* TML_NX_TRACE_IP_RAW_PACKET_ENABLE                    382  I1 = ip ptr                                                              */ 
                                     "NX IP RAW PACKET RX       ",       /* TML_NX_TRACE_IP_RAW_PACKET_RECEIVE                   383  I1 = ip ptr, I2 = packet ptr, I3 = wait option                           */ 
                                     "NX IP RAW PACKET TX       ",       /* TML_NX_TRACE_IP_RAW_PACKET_SEND                      384  I1 = ip ptr, I2 = packet ptr, I3 = destination ip, I4 = type of service  */ 
                                     "NX IP STATUS CHECK        ",       /* TML_NX_TRACE_IP_STATUS_CHECK                         385  I1 = ip ptr, I2 = needed status, I3 = actual status, I4 = wait option    */ 
                                     "NX PACKET ALLOCATE        ",       /* TML_NX_TRACE_PACKET_ALLOCATE                         386  I1 = pool ptr, I2 = packet ptr, I3 = packet type, I4 = available packets */ 
                                     "NX PACKET COPY            ",       /* TML_NX_TRACE_PACKET_COPY                             387  I1 = packet ptr, I2 = new packet ptr, I3 = pool ptr, I4 = wait option    */ 
                                     "NX PACKET DATA APPEND     ",       /* TML_NX_TRACE_PACKET_DATA_APPEND                      388  I1 = packet ptr, I2 = data start, I3 = data size, I4 = pool ptr          */ 
                                     "NX PACKET DATA RETRIEVE   ",       /* TML_NX_TRACE_PACKET_DATA_RETRIEVE                    389  I1 = packet ptr, I2 = buffer start, I3 = bytes copied                    */ 
                                     "NX PACKET LENGTH GET      ",       /* TML_NX_TRACE_PACKET_LENGTH_GET                       390  I1 = packet ptr, I2 = length                                             */ 
                                     "NX PACKET POOL CREATE     ",       /* TML_NX_TRACE_PACKET_POOL_CREATE                      391  I1 = pool ptr, I2 = payload size, I3 = memory ptr, I4 = memory_size      */ 
                                     "NX PACKET POOL DELETE     ",       /* TML_NX_TRACE_PACKET_POOL_DELETE                      392  I1 = pool ptr                                                            */ 
                                     "NX PACKET INFO GET        ",       /* TML_NX_TRACE_PACKET_POOL_INFO_GET                    393  I1 = pool ptr, I2 = total_packets, I3 = free packets, I4 = empty requests*/ 
                                     "NX PACKET RELEASE         ",       /* TML_NX_TRACE_PACKET_RELEASE                          394  I1 = packet ptr, I2 = packet status, I3 = available packets              */ 
                                     "NX PACKET TRANSMIT REL    ",       /* TML_NX_TRACE_PACKET_TRANSMIT_RELEASE                 395  I1 = packet ptr, I2 = packet status, I3 = available packets              */ 
                                     "NX RARP DISABLE           ",       /* TML_NX_TRACE_RARP_DISABLE                            396  I1 = ip ptr                                                              */ 
                                     "NX RAPR ENABLE            ",       /* TML_NX_TRACE_RARP_ENABLE                             397  I1 = ip ptr                                                              */ 
                                     "NX RARP INFO GET          ",       /* TML_NX_TRACE_RARP_INFO_GET                           398  I1 = ip ptr, I2 = requests sent, I3 = responses received, I4 = invalids  */ 
                                     "NX SYSTEM INITIALIZE      ",       /* TML_NX_TRACE_SYSTEM_INITIALIZE                       399  none                                                                     */ 
                                     "NX TCP CLIENT SOCK BIND   ",       /* TML_NX_TRACE_TCP_CLIENT_SOCKET_BIND                  400  I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option                */ 
                                     "NX TCP CLIENT SOCK CONN   ",       /* TML_NX_TRACE_TCP_CLIENT_SOCKET_CONNECT               401  I1 = ip ptr, I2 = socket ptr, I3 = server ip, I4 = server port           */ 
                                     "NX TCP CLIENT PORT GET    ",       /* TML_NX_TRACE_TCP_CLIENT_SOCKET_PORT_GET              402  I1 = ip ptr, I2 = socket ptr, I3 = port                                  */ 
                                     "NX TCP CLIENT SOCK UBIN   ",       /* TML_NX_TRACE_TCP_CLIENT_SOCKET_UNBIND                403  I1 = ip ptr, I2 = socket ptr                                             */ 
                                     "NX TCP ENABLE             ",       /* TML_NX_TRACE_TCP_ENABLE                              404  I1 = ip ptr                                                              */ 
                                     "NX TCP FREE PORT FIND     ",       /* TML_NX_TRACE_TCP_FREE_PORT_FIND                      405  I1 = ip ptr, I2 = port, I3 = free port                                   */ 
                                     "NX TCP INFO GET           ",       /* TML_NX_TRACE_TCP_INFO_GET                            406  I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets  */ 
                                     "NX TCP SERV SOCK ACCEPT   ",       /* TML_NX_TRACE_TCP_SERVER_SOCKET_ACCEPT                407  I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state        */ 
                                     "NX TCP SERV SOCK LISTEN   ",       /* TML_NX_TRACE_TCP_SERVER_SOCKET_LISTEN                408  I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = listen queue size          */ 
                                     "NX TCP SERV SOCK RELIST   ",       /* TML_NX_TRACE_TCP_SERVER_SOCKET_RELISTEN              409  I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = socket state               */ 
                                     "NX TCP SERV SOCK UNACPT   ",       /* TML_NX_TRACE_TCP_SERVER_SOCKET_UNACCEPT              410  I1 = ip ptr, I2 = socket ptr, I3 = socket state                          */ 
                                     "NX TCP SERV SOCK UNLIST   ",       /* TML_NX_TRACE_TCP_SERVER_SOCKET_UNLISTEN              411  I1 = ip ptr, I2 = port                                                   */ 
                                     "NX TCP SOCK CREATE        ",       /* TML_NX_TRACE_TCP_SOCKET_CREATE                       412  I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = window size     */ 
                                     "NX TCP SOCK DELETE        ",       /* TML_NX_TRACE_TCP_SOCKET_DELETE                       413  I1 = ip ptr, I2 = socket ptr, I3 = socket state                          */ 
                                     "NX TCP SOCK DISCONNECT    ",       /* TML_NX_TRACE_TCP_SOCKET_DISCONNECT                   414  I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state        */ 
                                     "NX TCP SOCK INFO GET      ",       /* TML_NX_TRACE_TCP_SOCKET_INFO_GET                     415  I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received       */  
                                     "NX TCP SOCK MSS GET       ",       /* TML_NX_TRACE_TCP_SOCKET_MSS_GET                      416  I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 = socket state                */ 
                                     "NX TCP MSS PEER GET       ",       /* TML_NX_TRACE_TCP_SOCKET_MSS_PEER_GET                 417  I1 = ip ptr, I2 = socket ptr, I3 = peer_mss, I4 = socket state           */ 
                                     "NX TCP SOCK MSS SET       ",       /* TML_NX_TRACE_TCP_SOCKET_MSS_SET                      418  I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 socket state                  */ 
                                     "NX TCP SOCK RX            ",       /* TML_NX_TRACE_TCP_SOCKET_RECEIVE                      419  I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = rx sequence          */ 
                                     "NX TCP SOCK RX NOTIFY     ",       /* TML_NX_TRACE_TCP_SOCKET_RECEIVE_NOTIFY               420  I1 = ip ptr, I2 = socket ptr, I3 = receive notify                        */ 
                                     "NX TCP SOCK TX            ",       /* TML_NX_TRACE_TCP_SOCKET_SEND                         421  I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = tx sequence          */ 
                                     "NX TCP SOCK STATE WAIT    ",       /* TML_NX_TRACE_TCP_SOCKET_STATE_WAIT                   422  I1 = ip ptr, I2 = socket ptr, I3 = desired state, I4 = previous state    */ 
                                     "NX TCP SOCK TX CONFIGUR   ",       /* TML_NX_TRACE_TCP_SOCKET_TRANSMIT_CONFIGURE           423  I1 = ip ptr, I2 = socket ptr, I3 = queue depth, I4 = timeout             */  
                                     "NX UDP ENABLE             ",       /* TML_NX_TRACE_UDP_ENABLE                              424  I1 = ip ptr                                                              */ 
                                     "NX UDP FREE PORT FIND     ",       /* TML_NX_TRACE_UDP_FREE_PORT_FIND                      425  I1 = ip ptr, I2 = port, I3 = free port                                   */ 
                                     "NX UDP INFO GET           ",       /* TML_NX_TRACE_UDP_INFO_GET                            426  I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets  */ 
                                     "NX UDP SOCK BIND          ",       /* TML_NX_TRACE_UDP_SOCKET_BIND                         427  I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option                */ 
                                     "NX UDP SOCK CHECKSM DIS   ",       /* TML_NX_TRACE_UDP_SOCKET_CHECKSUM_DISABLE             428  I1 = ip ptr, I2 = socket ptr                                             */ 
                                     "NX UPD SOCK CHECKSM ENA   ",       /* TML_NX_TRACE_UDP_SOCKET_CHECKSUM_ENABLE              429  I1 = ip ptr, I2 = socket ptr                                             */ 
                                     "NX UDP SOCK CREATE        ",       /* TML_NX_TRACE_UDP_SOCKET_CREATE                       430  I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = queue maximum   */ 
                                     "NX UDP SOCK DELETE        ",       /* TML_NX_TRACE_UDP_SOCKET_DELETE                       431  I1 = ip ptr, I2 = socket ptr                                             */ 
                                     "NX UDP SOCK INFO GET      ",       /* TML_NX_TRACE_UDP_SOCKET_INFO_GET                     432  I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received       */ 
                                     "NX UDP SOCK PORT GET      ",       /* TML_NX_TRACE_UDP_SOCKET_PORT_GET                     433  I1 = ip ptr, I2 = socket ptr, I3 = port                                  */ 
                                     "NX UDP SOCK RX            ",       /* TML_NX_TRACE_UDP_SOCKET_RECEIVE                      434  I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = packet size          */ 
                                     "NX UDP SOCK RX NOTIFY     ",       /* TML_NX_TRACE_UDP_SOCKET_RECEIVE_NOTIFY               435  I1 = ip ptr, I2 = socket ptr, I3 = receive notify                        */ 
                                     "NX UDP SOCK TX            ",       /* TML_NX_TRACE_UDP_SOCKET_SEND                         436  I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address      */ 
                                     "NX UDP SOCK UNBIND        ",       /* TML_NX_TRACE_UDP_SOCKET_UNBIND                       437  I1 = ip ptr, I2 = socket ptr                                             */ 
                                     "NX UDP SOCK SRC EXTRACT   ",       /* TML_NX_TRACE_UDP_SOURCE_EXTRACT                      438  I1 = packet ptr, I2 = ip address, I3 = port                              */
									 "NX IP INTERFACE ATTACH    ",       /* NX_TRACE_UDP_SOCKET_BYTES_AVAILABLE					 439  I1 = ip ptr, I2 = ip address, I3 = interface index                       */ 
									 "NX UDP SOCKET BYTES AVAIL ",       /* NX_TRACE_ICMP_PING6                                  440  I1 = ip ptr, I2 = socket ptr, I3 = bytes available                       */
									 "NX IP STATIC ROUTE ADD    ",		 /* NX_TRACE_IP_STATIC_ROUTE_ADD                         441  I1 = ip_ptr, I2 = network_address, I3 = net_mask, I4 = next_hop          */
									 "NX IP STATIC ROUTE DELETE ",       /* NX_TRACE_IP_STATIC_ROUTE_DELETE                      442  I1 = ip_ptr, I2 = network_address, I3 = net_mask                         */
 									 "NX TCP SOCKET PEER INF GET",       /* NX_TRACE_TCP_SOCKET_PEER_INFO_GET                    443  I1 = socket ptr, I2 = network_address, I3 = port                         */
									 "NX TCP SOC WIN UPD NOTIF S",       /* NX_TRACE_TCP_SOCKET_WINDOW_UPDATE_NOTIFY_SET         444  I1 = socket ptr,                                                         */
									 "NX UDP SOC INTERFACE SET  ",       /* NX_TRACE_UDP_SOCKET_INTERFACE_SET					 445  I1 = socket_ptr, I2 = interface_index,                                   */
									 "NX IP INTERFACE INFO GET  ",       /* NX_TRACE_IP_INTERFACE_INFO_GET						 446  I1 = ip_ptr, I2 = ip_address, I3 = mtu_size, I4 = interface_index        */
									 "NX PACKET DATA EXTRACT OFF",       /* NX_TRACE_PACKET_DATA_EXTRACT_OFFSET					 447  I1 = packet_ptr, I2 = buffer_length, I3 = bytes_copied,                  */
									 "NX TCP SOC BYTES AVAILABLE",       /* NX_TRACE_TCP_SOCKET_BYTES_AVAILABLE					 448  I1 = ip ptr, I2 = socket ptr, I3 = bytes available                       */								     
									 "INVALID  ", //449
                                     "INVALID  ", //450
                                     "INVALID  ", //451
                                     "INVALID  ", //452
                                     "INVALID  ", //453
                                     "INVALID  ", //454
									 "INVALID  ", //455
                                     "INVALID  ", //456
                                     "INVALID  ", //457
                                     "INVALID  ", //458
                                     "INVALID  ", //459
                                     "INVALID  ", //460
									 "INVALID  ", //461
                                     "INVALID  ", //462                                     
									 "INVALID  ", //463
                                     "INVALID  ", //464
                                     "INVALID  ", //465
                                     "INVALID  ", //466
                                     "INVALID  ", //467
                                     "INVALID  ", //468
								     "NX IPSEC ENABLE           ",       /* NX_TRACE_IPSEC_ENABLE	                             469  I1 = ip_ptr                                                              */
									 "NXD ICMP ENABLE           ",       /* NXD_TRACE_ICMP_ENABLE								 470  I1 = ip ptr                                                              */ 
									 "NX ICMP PING6             ",       /* NX_TRACE_ICMP_PING6				                     471  I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size              */ 
									 "NXD UDP SOURCE EXTRACT    ",       /* NXD_TRACE_UDP_SOURCE_EXTRACT                         472  I1 = packet ptr, I2 = IP Version (4 or 6), I3 = ip address, I4 = port    */ 									 
									 "NXD UDP SOCKET SET INTERFA",       /* NXD_TRACE_UDP_SOCKET_SET_INTERFACE                   473  I1 = udp_socket_ptr, I2 = interface_id                                   */
									 "NXD TCP SOCKET SET INTERFA",       /* NXD_TRACE_TCP_SOCKET_SET_INTERFACE                   474  I1 = tcp_socket_ptr, I2 = interface_id                                   */
									 "NXD UDP SOCKET SEND       ",       /* NXD_TRACE_UDP_SOCKET_SEND                            475  I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address      */ 
									 "NXD ND CACHE DELETE       ",       /* NXD_TRACE_ND_CACHE_DELETE                            476  I1 = dest_ip                                                             */ 
                                     "NXD ND CACHE ENTRY SET    ",       /* NXD_TRACE_ND_CACHE_ENTRY_SET                         477  I1 = IP address, I2 = physical msw, I3 = physical lsw                    */
									 "NX ND CACHE IP ADDRESS FIN",       /* NX_TRACE_ND_CACHE_IP_ADDRESS_FIND                    478  I1 = ip_ptr, I2 = IP address, I3 = physical msw, I4 = physical lsw       */
									 "NXD ND CACHE INVALIDATE   ",       /* NXD_TRACE_ND_CACHE_INVALIDATE                        479  I1 = ip_ptr                                                              */
									 "NXD IPV6 GLOBAL ADDR GET  ",       /* NXD_TRACE_IPV6_GLOBAL_ADDRESS_GET                    480  I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
									 "NXD IPV6 GLOBAL ADDR SET  ",	     /* NXD_TRACE_IPV6_GLOBAL_ADDRESS_SET                    481  I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
									 "NX IPSTATIC ROUTE ADD     ",       /* NX_TRACE_IPSTATIC_ROUTE_ADD                          482  I1 = ip_ptr, I2 = network address, I3 = net_mask, I4 = next hop address  */
									 "NX IP STATIC ROUTING ENABL",       /* NX_TRACE_IP_STATIC_ROUTING_ENABLE                    483  I1 = ip_ptr                                                              */
									 "NX IP STATIC ROUTING DISAB",       /* NX_TRACE_IP_STATIC_ROUTING_DISABLE                   484  I1 = ip_ptr                                                              */
									 "NX IPV6 ENABLE            ",       /* NX_TRACE_IPV6_ENABLE                                 485  I1 = ip_ptr                                                              */
									 "NXD IPV6 RAW PACKET SEND  ",       /* NXD_TRACE_IPV6_RAW_PACKET_SEND                       486  I1 = ip_ptr, I2 = ip address lsw, I3 = protocol, I4 = packet_ptr         */
                                     "NXD IP RAW PACKET SEND    ",       /* NXD_TRACE_IP_RAW_PACKET_SEND                         487  I1 = ip_ptr, I2 = ip address lsw, I3 = type of serveice, I4 = packet_ptr */
                                     "NXD IPV6 LINKLOCAL ADDR GT",       /* NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_GET                 488  I1 = ip_ptr, I2 = IP address lsw                                         */
                                     "NXD IPV6 LINKLOCAL ADDR ST",       /* NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_SET                 489  I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
                                     "NXD IPV6 INITIATE DAD PROC",       /* NXD_TRACE_IPV6_INITIATE_DAD_PROCESS                  490  I1 = ip_ptr                                                              */
									 "NSD IPV6 DEFALT ROUTER ADD",       /* NXD_TRACE_IPV6_DEFAULT_ROUTER_ADD                    491  I1 = ip_ptr, I2 = router addr lsw, I3 = router lifetime                  */
									 "NXD IPV6 DEFALT ROUTER DEL",       /* NXD_TRACE_IPV6_DEFAULT_ROUTER_DELETE                 492  I1 = ip_ptr, I2 = router addr lsw,                                       */                                     
									 "NXD IPV6 INTRFACE ADDR GET",       /* NXD_TRACE_IPV6_INTERFACE_ADDRESS_GET                 493  I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index */
                                     "NXD IPV6 INTRFACE ADDR SET",       /* NXD_TRACE_IPV6_INTERFACE_ADDRESS_SET                 494  I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index */
									 "NSD TCP SOC PEER INFO GET ",       /* NXD_TRACE_TCP_SOCKET_PEER_INFO_GET                   495  I1 = socket_ptr, I2 = Peer IP address, I3 = peer_port                    */
									 "NXD IP MAX PAYLD SIZE FIND",       /* NXD_TRACE_IP_MAX_PAYLOAD_SIZE_FIND                   496  I1 = ip_ptr                                                              */									 									 									                                      
                                     "INVALID  ", //497
									 "INVALID  ", //498
                                     "INVALID  ", //499
                                     "INVALID  ", //500
                                     "INVALID  ", //501
                                     "INVALID  ", //502
                                     "INVALID  ", //503
                                     "INVALID  ", //504
                                     "INVALID  ", //505
                                     "INVALID  ", //506
                                     "INVALID  ", //507
                                     "INVALID  ", //508
                                     "INVALID  ", //509
                                     "INVALID  ", //510
                                     "INVALID  ", //511
                                     "INVALID  ", //512
                                     "INVALID  ", //513
                                     "INVALID  ", //514
                                     "INVALID  ", //515
                                     "INVALID  ", //516
                                     "INVALID  ", //517
                                     "INVALID  ", //518
                                     "INVALID  ", //519
                                     "INVALID  ", //520
                                     "INVALID  ", //521
                                     "INVALID  ", //522
									 "INVALID  ", //523
                                     "INVALID  ", //524
                                     "INVALID  ", //525
                                     "INVALID  ", //526
                                     "INVALID  ", //527
                                     "INVALID  ", //528
                                     "INVALID  ", //529
                                     "INVALID  ", //530
                                     "INVALID  ", //531
                                     "INVALID  ", //532
                                     "INVALID  ", //533
                                     "INVALID  ", //534
                                     "INVALID  ", //535
                                     "INVALID  ", //536
                                     "INVALID  ", //537
                                     "INVALID  ", //538
                                     "INVALID  ", //539
                                     "INVALID  ", //540
                                     "INVALID  ", //541
                                     "INVALID  ", //542
                                     "INVALID  ", //543
                                     "INVALID  ", //544
                                     "INVALID  ", //545
                                     "INVALID  ", //546
                                     "INVALID  ", //547
									 "INVALID  ", //548
                                     "INVALID  ", //549
                                     "INVALID  ", //550
                                     "INVALID  ", //551
                                     "INVALID  ", //552
                                     "INVALID  ", //553
                                     "INVALID  ", //554
                                     "INVALID  ", //555
                                     "INVALID  ", //556
                                     "INVALID  ", //557
                                     "INVALID  ", //558
                                     "INVALID  ", //559
                                     "INVALID  ", //560
                                     "INVALID  ", //561
                                     "INVALID  ", //562
                                     "INVALID  ", //563
                                     "INVALID  ", //564
                                     "INVALID  ", //565
                                     "INVALID  ", //566
                                     "INVALID  ", //567
                                     "INVALID  ", //568
                                     "INVALID  ", //569
                                     "INVALID  ", //570
                                     "INVALID  ", //571
                                     "INVALID  ", //572
									 "INVALID  ", //573
                                     "INVALID  ", //574
                                     "INVALID  ", //575
                                     "INVALID  ", //576
                                     "INVALID  ", //577
                                     "INVALID  ", //578
                                     "INVALID  ", //579
                                     "INVALID  ", //580
                                     "INVALID  ", //581
                                     "INVALID  ", //582
                                     "INVALID  ", //583
                                     "INVALID  ", //584
                                     "INVALID  ", //585
                                     "INVALID  ", //586
                                     "INVALID  ", //587
                                     "INVALID  ", //588
                                     "INVALID  ", //589
                                     "INVALID  ", //590
                                     "INVALID  ", //591
                                     "INVALID  ", //592
                                     "INVALID  ", //593
                                     "INVALID  ", //594
                                     "INVALID  ", //595
                                     "INVALID  ", //596
                                     "INVALID  ", //597
									 "INVALID  ", //598
                                     "INVALID  ", //599
                                     "INVALID  ", //600
                                     									 

									/* Define the UsbX host stack events first.    */
									"UX HST STK CLS INST CREATE", /* TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_CREATE  (600 + 1) I1 = class,    I2 = class instance  */    
									"UX HST STK CLS INST DESTRO", /* TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_DESTROY  (600 + 2) I1 = class   , I2 = class instance  */    
									"UX HST STK CONFIG DELETE  ", /* TML_UX_TRACE_HOST_STACK_CONFIGURATION_DELETE  (600 + 3)I1 = configuration  */    
									"UX HST STK CONFIG ENUM    ", /* TML_UX_TRACE_HOST_STACK_CONFIGURATION_ENUMERATE  (600 + 4)I1 = device  */    
									"UX HST STK CNFG INST CREAT", /* TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_CREATE  (600 + 5)I1 = configuration  */    
									"UX HST STK CONFIG INST DEL", /* TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_DELETE  (600 + 6)I1 = configuration  */    
									"UX HST STK CONFIG SET     ", /* TML_UX_TRACE_HOST_STACK_CONFIGURATION_SET  (600 + 7)I1 = configuration  */    
									"UX HST STK DEVICE ADDR SET", /* TML_UX_TRACE_HOST_STACK_DEVICE_ADDRESS_SET  (600 + 8)I1 = device          , I2 = device address  */    
									"UX HST STK DEVICE CNFG GET", /* TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_GET  (600 + 9)I1 = device          , I2 = configuration  */    
									"UX HST STK DEVICE CNFG SEL", /* TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_SELECT  (600 + 10)I1 = device          , I2 = configuration  */    
									"UX HST STK DEVICE DSCRP RD", /* TML_UX_TRACE_HOST_STACK_DEVICE_DESCRIPTOR_READ  (600 + 11)I1 = device  */    
									"UX HST STK DEVICE GET     ", /* TML_UX_TRACE_HOST_STACK_DEVICE_GET  (600 + 12)I1 = device index  */    
									"UX HST STK DEVICE REMOVE  ", /* TML_UX_TRACE_HOST_STACK_DEVICE_REMOVE  (600 + 13)I1 = hcd             , I2 = parent          , I3 = port index        , I4 = device  */    
									"UX HST STK DEVICE RSRC FRE", /* TML_UX_TRACE_HOST_STACK_DEVICE_RESOURCE_FREE  (600 + 14)I1 = device  */    
									"UX HST STK ENDPT INST CREA", /* TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_CREATE  (600 + 15)I1 = device          , I2 = endpoint  */    
									"UX HST STK ENDPT INST DEL ", /* TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_DELETE  (600 + 16)I1 = device          , I2 = endpoint  */    
									"UX HST STK ENDPT RESET    ", /* TML_UX_TRACE_HOST_STACK_ENDPOINT_RESET  (600 + 17)I1 = device          , I2 = endpoint  */    
									"UX HST STK ENDPT TRAN ABRT", /* TML_UX_TRACE_HOST_STACK_ENDPOINT_TRANSFER_ABORT  (600 + 18)I1 = endpoint  */    
									"UX HST STK HCD REGISTER   ", /* TML_UX_TRACE_HOST_STACK_HCD_REGISTER600 + 19)I1 = hcd name  */    
									"UX HST_STK INIT           ", /* TML_UX_TRACE_HOST_STACK_INITIALIZE  (600 + 20)  */       
									"UX HST STK INTRFC ENDPT GE", /* TML_UX_TRACE_HOST_STACK_INTERFACE_ENDPOINT_GET  (600 + 21)I1 = interface       , I2 = endpoint index  */    
									"UX HST STK INTRFCE INST CR", /* TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_CREATE  (600 + 22)I1 = interface  */    
									"UX HST STK INTRFC INST DEL", /* TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_DELETE  (600 + 23)I1 = interface  */    
									"UX HST STK INTRFCE SET    ", /* TML_UX_TRACE_HOST_STACK_INTERFACE_SET  (600 + 24)I1 = interface  */    
									"UX HST STK INTRFC SET SEL ", /* TML_UX_TRACE_HOST_STACK_INTERFACE_SETTING_SELECT  (600 + 25)I1 = interface   */    
									"UX HST STK NEW CNFG CREATE", /* TML_UX_TRACE_HOST_STACK_NEW_CONFIGURATION_CREATE  (600 + 26)I1 = device          , I2 = configuration  */    
									"UX HST STK NEW DEV CREATE ", /* TML_UX_TRACE_HOST_STACK_NEW_DEVICE_CREATE  (600 + 27)				 I1 = hcd             , I2 = device owner    , I3 = port index        , I4 = device  */    
									"UX HST STK NEW ENDPT CREAT", /* TML_UX_TRACE_HOST_STACK_NEW_ENDPOINT_CREATE  (600 + 28)				 I1 = interface       , I2 = endpoint  */    
									"UX HST STK RH CHNG PROCESS", /* TML_UX_TRACE_HOST_STACK_RH_CHANGE_PROCESS  (600 + 29)				 I1 = port index  */       
									"UX HST STK RH DEV EXTRACT ", /* TML_UX_TRACE_HOST_STACK_RH_DEVICE_EXTRACTION  (600 + 30)				 I1 = hcd             , I2 = port index  */    
									"UX HST STK RH DEV INSERT  "  /* TML_UX_TRACE_HOST_STACK_RH_DEVICE_INSERTION  (600 + 31)				 I1 = hcd             , I2 = port index  */    
									"UX HST STK TRANSFER REQST ", /* TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST  (600 + 32)				 I1 = device          , I2 = endpoint        , I3 = transfer request  */    
									"UX HST STK TRNSFR REQ ABOR", /* TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST_ABORT  (600 + 33)				 I1 = device          , I2 = endpoint        , I3 = transfer request  */    
																																	    																             
																																																		   
                                     "INVALID  ", //634
                                     "INVALID  ", //635
                                     "INVALID  ", //636
                                     "INVALID  ", //637
                                     "INVALID  ", //638
                                     "INVALID  ", //639
                                     "INVALID  ", //640
                                     "INVALID  ", //641
                                     "INVALID  ", //642
                                     "INVALID  ", //643
                                     "INVALID  ", //644
                                     "INVALID  ", //645
                                     "INVALID  ", //646
                                     "INVALID  ", //647
									 "INVALID  ", //648
                                     "INVALID  ", //649
                                     "INVALID  ", //650
                                     		 
																																																					 
									/* Define the UsbX host class events first.    */																						    																             
									"UX HST CLASS ASIX ACTIVATE", /* TML_UX_TRACE_HOST_CLASS_ASIX_ACTIVATE  (650 + 1)				 I1 = class instance  */       
									"UX HST CLASS ASIX DEACTIVA", /* TML_UX_TRACE_HOST_CLASS_ASIX_DEACTIVATE  (650 + 2)				 I1 = class instance  */       
									"UX HST CLS ASIX INTRUPT NO", /* TML_UX_TRACE_HOST_CLASS_ASIX_INTERRUPT_NOTIFICATION  (650 + 3)				 I1 = class instance  */       
									"UX HST CLASS ASIX READ    ", /* TML_UX_TRACE_HOST_CLASS_ASIX_READ  (650 + 4)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */       
									"UX HST CLASS ASIX WRITE   ", /* TML_UX_TRACE_HOST_CLASS_ASIX_WRITE  (650 + 5)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */       

									 "INVALID  ", //656
									 "INVALID  ", //657
									 "INVALID  ", //658
									 "INVALID  ", //659
                                 
							 
									"UX HST CLASS AUDIO ACTIVAT", /* TML_UX_TRACE_HOST_CLASS_AUDIO_ACTIVATE  (650 + 10)				 I1 = class instance  */       
									"UX HST CLS AUD CTRL VAL GE", /* TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_GET  (650 + 11)				 I1 = class instance  */       					    														        
									"UX HST CLS AUD CTRL VAL SE", /* TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_SET  (650 + 12)				 I1 = class instance  , I2 = audio control  */
									"UX HST CLS AUDIO DEACTIVAT", /* TML_UX_TRACE_HOST_CLASS_AUDIO_DEACTIVATE  (650 + 13)				 I1 = class instance  */       
									"UX HST CLASS AUDIO READ   ", /* TML_UX_TRACE_HOST_CLASS_AUDIO_READ  (650 + 14)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */       
									"UX HST CLS AUD STRM SA GET", /* TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_GET  (650 + 15)				 I1 = class instance  */       					    														        
									"UX HST CLS AUD STRM SA SET", /* TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_SET  (650 + 16)				 I1 = class instance  , I2 = audio sampling  */					    														        					    														        
									"UX HST CLASS AUDIO WRITE  ", /* TML_UX_TRACE_HOST_CLASS_AUDIO_WRITE  (650 + 17)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */       


									 "INVALID  ", //668
									 "INVALID  ", //669
                                 
																 								    											     			                   
									"UX HST CLS CDC ACM ACTIVAT", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_ACTIVATE  (650 + 20)				 I1 = class instance  */       
									"UX HST CLS CDC ACM DEACTIV", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_DEACTIVATE  (650 + 21)				 I1 = class instance  */       
									"UX HST CDC ACM IOCTL SL CD", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_CODING  (650 + 22)				 I1 = class instance  , I2 = parameter  */					    														        
									"UX HST CDC ACM IOCTL GL CD" /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_LINE_CODING  (650 + 23)				 I1 = class instance  , I2 = parameter  */					    														        
									"UX HST CDC ACM IOCTL SL ST", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_STATE  (650 + 24)				 I1 = class instance  , I2 = parameter  */					    														        
									"UX HST CDCACM IOCTL SND BR", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SEND_BREAK  (650 + 25)				 I1 = class instance  , I2 = parameter  */					    														        
									"UX HST CDCACM IOCTL ABR IP", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_IN_PIPE  (650 + 26)				 I1 = class instance  , I2 = endpoint  */					    														        
									"UX HST CDCACM IOCTL ABR OP", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_OUT_PIPE  (650 + 27)				 I1 = class instance  , I2 = endpointr  */					    														        
									"UX HST ACM IOCTL NTFN CLBK", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_NOTIFICATION_CALLBACK  (650 + 28)				 I1 = class instance  , I2 = parameter  */					    														        
									"UX HST A IOCTL GET DEV STA", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_DEVICE_STATUS  (650 + 29)				 I1 = class instance  , I2 = device status  */					    														        
									"UX HST CLS CDC ACM READ   ", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_READ  (650 + 30)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */
									"UX HST CLS CDCACM RE START", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_START  (650 + 31)				 I1 = class instance  */					    														        
									"UX HST CLS CDCACM RE STOP ", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_STOP  (650 + 32)				 I1 = class instance  */					    														        
									"UX HST CLS CDC ACM WRITE  ", /* TML_UX_TRACE_HOST_CLASS_CDC_ACM_WRITE  (650 + 33)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */

									 "INVALID  ", //684
									 "INVALID  ", //685
									 "INVALID  ", //686
									 "INVALID  ", //687
									 "INVALID  ", //688
									 "INVALID  ", //689
                                 
							 
									"UX HST CLASS HID ACTIVATE ", /* TML_UX_TRACE_HOST_CLASS_HID_ACTIVATE  (650 + 40)				 I1 = class instance  */       
									"UX HST CLASS HID CLNT REG ", /* TML_UX_TRACE_HOST_CLASS_HID_CLIENT_REGISTER  (650 + 41)				 I1 = hid client name  */					    														        
									"UX HST CLASS HID DEACTIVAT", /* TML_UX_TRACE_HOST_CLASS_HID_DEACTIVATE  (650 + 42)				 I1 = class instance  */       
									"UX HST CLASS HID IDLE GET ", /* TML_UX_TRACE_HOST_CLASS_HID_IDLE_GET  (650 + 43)				 I1 = class instance  */					    														        
									"UX HST CLASS HID IDLE SET ", /* TML_UX_TRACE_HOST_CLASS_HID_IDLE_SET  (650 + 44)				 I1 = class instance  */					    														        
									"UX HST CLS HID KYBRD ACTIV", /* TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_ACTIVATE  (650 + 45)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID KYBRD DEACT", /* TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_DEACTIVATE  (650 + 46)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID MOUS ACTIVA", /* TML_UX_TRACE_HOST_CLASS_HID_MOUSE_ACTIVATE  (650 + 47)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID MOUS DEACTI", /* TML_UX_TRACE_HOST_CLASS_HID_MOUSE_DEACTIVATE  (650 + 48)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID RMTE CTRL A", /* TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_ACTIVATE  (650 + 49)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID RMTE CTRL D", /* TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_DEACTIVATE  (650 + 50)				 I1 = class instance  , I2 = hid client instance  */					    														        
									"UX HST CLS HID REPORT GET ", /* TML_UX_TRACE_HOST_CLASS_HID_REPORT_GET  (650 + 51)				 I1 = class instance  , I2 = client report  */					    														        
									"UX HST CLS HID REPORT SET ", /* TML_UX_TRACE_HOST_CLASS_HID_REPORT_SET  (650 + 52)				 I1 = class instance  , I2 = client report  */					    														        
									 "INVALID  ", //703
									 "INVALID  ", //704
									 "INVALID  ", //705
									 "INVALID  ", //706
									 "INVALID  ", //707
									 "INVALID  ", //708
									 "INVALID  ", //709
                                 
																 							    											     			                   
									"UX HOST CLASS HUB ACTIVATE", /* TML_UX_TRACE_HOST_CLASS_HUB_ACTIVATE  (650 + 60)				 I1 = class instance  */     
							 
									"INVALID  ", //711 
							 
									"UX HST CLASS HUB CHNG DET ", /* TML_UX_TRACE_HOST_CLASS_HUB_CHANGE_DETECT650 + 62)				 I1 = class instance  */       					    														        
									"UX HST HUB PRT CHNG C PROC", /* TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_CONNECTION_PROCESS  (650 + 63)				 I1 = class instance  , I2 = port , I3 = port status  */
									"UX HST HUB PRT CHNG E PROC", /* TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_ENABLE_PROCESS  (650 + 64)				 I1 = class instance  , I2 = port, I3 = port status  */       					    														        
									"UX HST HUB PRT CHNG O C PR", /* TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_OVER_CURRENT_PROCESS  (650 + 65)				 I1 = class instance  , I2 = port , I3 = port status  */
									"UX HST HUB PRT CHNG R PROC", /* TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_RESET_PROCESS  (650 + 66)				 I1 = class instance  , I2 = port, I3 = port status  */
									"UX HST HUB PRT CHNG S PROC", /* TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_SUSPEND_PROCESS  (650 + 67)				 I1 = class instance  , I2 = port            , I3 = port status  */
									"UX HST CLASS HUB DEACTIVAT", /* TML_UX_TRACE_HOST_CLASS_HUB_DEACTIVATE  (650 + 68)				 I1 = class instance  */       
																 					
									"INVALID  ", //719 

							 
									"UX HST CLASS PIMA ACTIVATE", /* TML_UX_TRACE_HOST_CLASS_PIMA_ACTIVATE  (650 + 70)				 I1 = class instance  */       
									"UX HST CLASS PIMA DEACTIVA", /* TML_UX_TRACE_HOST_CLASS_PIMA_DEACTIVATE  (650 + 71)				 I1 = class instance  */					    														        
									"UX HST CLS PIMA DEV INF GE", /* TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_INFO_GET  (650 + 72)				 I1 = class instance  , I2 = pima device  */       
									"UX HST CLASS PIMA DEV REST", /* TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_RESET  (650 + 73)				 I1 = class instance  */					    														        
									"UX HST CLASS PIMA NOTIFICA", /* TML_UX_TRACE_HOST_CLASS_PIMA_NOTIFICATION  (650 + 74)				 I1 = class instance  , I2 = event code      , I3 = transaction ID    , I4 = parameter1  */					    														        
									"UX HST CLS PIMA NUM OBJ GE", /* TML_UX_TRACE_HOST_CLASS_PIMA_NUM_OBJECTS_GET  (650 + 75)				 I1 = class instance  */       					    														        
									"UX HST CLASS PIMA OBJ CLS ", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_CLOSE  (650 + 76)				 I1 = class instance  , I2 = object  */       					    														        
									"UX HST CLASS PIMA OBJ COPY", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_COPY  (650 + 77)				 I1 = class instance  , I2 = object handle  */       					    														        
									"UX HST CLASS PIMA OBJ DEL ", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_DELETE  (650 + 78)				 I1 = class instance  , I2 = object handle  */       					    														        					    														        
									"UX HST CLASS PIMA OBJ GET ", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_GET  (650 + 79)				 I1 = class instance  , I2 = object handle   , I3 = object  */       					    														        					    														        					    														        
									"UX HST CLS PIMA OBJ INF GE", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_GET  (650 + 80)				 I1 = class instance  , I2 = object handle   , I3 = object  */       					    														        					    														        					    														        					    														        
									"UX HST CLS PIMA OBJ IN SND", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_SEND  (650 + 81)				 I1 = class instance  , I2 = object  */					    														        
									"UX HST CLASS PIMA OBJ MOVE", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_MOVE  (650 + 82)				 I1 = class instance  , I2 = object handle  */					    														        
									"UX HST CLASS PIMA OBJ SEND", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_SEND  (650 + 83)				 I1 = class instance  , I2 = object, I3 = object_buffer, I4 = object length  */					    														        
									"UX HST PIMA OBJ TRAN ABORT", /* TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_TRANSFER_ABORT  (650 + 84)				 I1 = class instance  , I2 = object handle   , I3 = object  */					    														        
									"UX HST CLASS PIMA READ    ", /* TML_UX_TRACE_HOST_CLASS_PIMA_READ  (650 + 85)				 I1 = class instance  , I2 = data pointer    , I3 = data length  */					    														        
									"UX HST CLS PIMA RQST CANCE", /* TML_UX_TRACE_HOST_CLASS_PIMA_REQUEST_CANCEL  (650 + 86)				 I1 = class instance  */
									"UX HST CLASS PIMA SES CLOS", /* TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_CLOSE  (650 + 87)				 I1 = class instance  , I2 = pima session  */					    														        					    														        
									"UX HST CLASS PIMA SES OPEN", /* TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_OPEN  (650 + 88)				 I1 = class instance  , I2 = pima session  */					    														        					    														        					    														        
									"UX HST PIMA STOR IDS GET  ", /* TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_IDS_GET  (650 + 89)				 I1 = class instance  , I2 = storage ID array, I3 = storage ID length  */					    														        
									"UX HST CLS PIMA STOR IN GE", /* TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_INFO_GET  (650 + 90)				 I1 = class instance  , I2 = storage ID      , I3 = storage  */					    														        
									"UX HST CLASS PIMA THUMB GE", /* TML_UX_TRACE_HOST_CLASS_PIMA_THUMB_GET  (650 + 91)				 I1 = class instance  , I2 = object handle  */					    														        
									"UX HST CLASS PIMA WRITE   ", /* TML_UX_TRACE_HOST_CLASS_PIMA_WRITE  (650 + 92)				 I1 = class instance  , I2 = data pointer    , I3 = data length  */					    														        
																 				
								 "INVALID  ", //742
								 "INVALID  ", //743
								 "INVALID  ", //744
								 "INVALID  ", //745
								 "INVALID  ", //746
								 "INVALID  ", //747
								 "INVALID  ", //748
								 "INVALID  ", //749
                                 
																				
								"UX HST CLASS PRNTR ACTIVAT", /* TML_UX_TRACE_HOST_CLASS_PRINTER_ACTIVATE  (650 + 100)				 I1 = class instance  */       
								"UX HST CLASS PRNTR DEACTIV", /* TML_UX_TRACE_HOST_CLASS_PRINTER_DEACTIVATE  (650 + 101)				 I1 = class instance  */       
								"UX HST CLASS PRNTR NAME GE", /* TML_UX_TRACE_HOST_CLASS_PRINTER_NAME_GET  (650 + 102)				 I1 = class instance  */					    														               
								"UX HST CLASS PRNTR READ   ", /* TML_UX_TRACE_HOST_CLASS_PRINTER_READ  (650 + 103)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */					    														               
								"UX HST CLASS PRNTR WRITE  ", /* TML_UX_TRACE_HOST_CLASS_PRINTER_WRITE  (650 + 104)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */					    														               
								"UX HST CLASS PRNTR SFT RES", /* TML_UX_TRACE_HOST_CLASS_PRINTER_SOFT_RESET  (650 + 105)				 I1 = class instance  */					    														               
								"UX HST CLASS PRNTR STAT GE", /* TML_UX_TRACE_HOST_CLASS_PRINTER_STATUS_GET  (650 + 106)				 I1 = class instance  , I2 = printer status  */
							 
								 "INVALID  ", //756
								 "INVALID  ", //757
								 "INVALID  ", //758
								 "INVALID  ", //759									 
																 										    														                      
								"UX HST CLS PRLFC ACTIVATE ", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_ACTIVATE  (650 + 110)				 I1 = class instance  */       
								"UX HST CLS PRLFC DEACTIVAT", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_DEACTIVATE  (650 + 111)				 I1 = class instance  */       
								"UX HST PRLFC IOCTL S LN CO", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_CODING  (650 + 112)              I1 = class instance  , I2 = parameter  */   											               
								"UX HST PRLFC IOCTL G LN CO", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_LINE_CODING  (650 + 113)				 I1 = class instance  , I2 = parameter  */					    														               
								"UX HST PRLFC IOCTL S LN ST", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_STATE650 + 114)				 I1 = class instance  , I2 = parameter  */					    														               
								"UX HST CLS PRLFC IOCTL PRG", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_PURGE  (650 + 115)				 I1 = class instance  , I2 = parameter  */					    														               
								"UX HST PRLFC IOCTL SE BREK", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SEND_BREAK  (650 + 116)				 I1 = class instance  */					    														               
								"UX HST PRLF IOCTL ABRT I P", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_IN_PIPE  (650 + 117)				 I1 = class instance  , I2 = endpoint  */					    														               
								"UX HST PRLF IOCTL ABRT O P", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_OUT_PIPE  (650 + 118)				 I1 = class instance  , I2 = endpointr  */					    														               
								"UX HST PRF IOCTL RE DE S C", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_REPORT_DEVICE_STATUS_CHANGE	  (650 + 119)				 I1 = class instance  , I2 = parameter  */					    														               
								"UX HST PRLFC IOCTL G DE ST", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_DEVICE_STATUS650 + 120)				 I1 = class instance  , I2 = device status  */					    														               
								"UX HST CLS PRLFC READ     ", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_READ  (650 + 121)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */					    														               
								"UX HST CLS PRLF RECEP STRT", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_START  (650 + 122)				 I1 = class instance  */					    														               
								"UX HST CLS PRLFC RECEP STP", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_STOP  (650 + 123)				 I1 = class instance  */					    														               
								"UX HST CLS PRLFC WRITE    ", /* TML_UX_TRACE_HOST_CLASS_PROLIFIC_WRITE  (650 + 124)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */

								 "INVALID  ", //774
								 "INVALID  ", //775
								 "INVALID  ", //776
								 "INVALID  ", //777						    														               
								 "INVALID  ", //778
								 "INVALID  ", //779
		                     
							 
								"UX HST CLS STRG ACTIVATE ", /* TML_UX_TRACE_HOST_CLASS_STORAGE_ACTIVATE  (650 + 130)				 I1 = class instance  */       
								"UX HST CLS STRG DEACTIVAT", /* TML_UX_TRACE_HOST_CLASS_STORAGE_DEACTIVATE  (650 + 131)				 I1 = class instance  */       
								"UX HST CLS STRG MEDIA C G", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_CAPACITY_GET  (650 + 132)				 I1 = class instance  */					    														               
								"UX HST CLS STRG MDA F C G", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_FORMAT_CAPACITY_GET  (650 + 133)				 I1 = class instance  */					    														               
								"UX HST CLS STRG MEDIA MNT", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_MOUNT  (650 + 134)				 I1 = class instance  , I2 = sector  */					    														               
								"UX HST CLS STRG MEDIA OPN", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_OPEN  (650 + 135)				 I1 = class instance  , I2 = media  */					    														               
								"UX HST CLS STRG MEDIA RD ", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_READ  (650 + 136)				 I1 = class instance, I2 = sector start, I3 = sector count, I4 = data pointer  */					    														               
								"UX HST CLS STRG MDA WRITE", /* TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_WRITE  (650 + 137)				 I1 = class instance  , I2 = sector start, I3 = sector count, I4 = data pointer  */						    														               
								"UX HST CLS STRG RQST SNSE", /* TML_UX_TRACE_HOST_CLASS_STORAGE_REQUEST_SENSE  (650 + 138)				 I1 = class instance  */					    														               
								"UX HST CLS STRG STRT STOP", /* TML_UX_TRACE_HOST_CLASS_STORAGE_START_STOP  (650 + 139)				 I1 = class instance  , I2 = start stop signal  */					    														               
								"UX HST CLS STRG UNT RDY T", /* TML_UX_TRACE_HOST_CLASS_STORAGE_UNIT_READY_TEST  (650 + 140)				 I1 = class instance  */					    														               

								 "INVALID  ", //791
                                 "INVALID  ", //792
                                 "INVALID  ", //793
                                 "INVALID  ", //794
                                 "INVALID  ", //795
                                 "INVALID  ", //796
                                 "INVALID  ", //797
                                 "INVALID  ", //798
                                 "INVALID  ", //799
                                 
							 
																 										    														                      
								"UX HST CLS DPUMP ACTIVATE", /* TML_UX_TRACE_HOST_CLASS_DPUMP_ACTIVATE  (650 + 150)				 I1 = class instance  */       
								"UX HST CLS DPUMP DEACTIVA", /* TML_UX_TRACE_HOST_CLASS_DPUMP_DEACTIVATE  (650 + 151)				 I1 = class instance  */       
								"UX HST CLS DPUMP READ    ", /* TML_UX_TRACE_HOST_CLASS_DPUMP_READ  (650 + 152)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */					    														               
								"UX HST CLS DPUMP WRITE   ", /* TML_UX_TRACE_HOST_CLASS_DPUMP_WRITE  (650 + 153)				 I1 = class instance  , I2 = data pointer    , I3 = requested length  */					    														               
																																		    														   
                                 "INVALID  ", //804
                                 "INVALID  ", //805
                                 "INVALID  ", //806
                                 "INVALID  ", //807
                                 "INVALID  ", //808
                                 "INVALID  ", //809
                                 "INVALID  ", //810
                                 "INVALID  ", //811
                                 "INVALID  ", //812
                                 "INVALID  ", //813
                                 "INVALID  ", //814
                                 "INVALID  ", //815
                                 "INVALID  ", //816
                                 "INVALID  ", //817
                                 "INVALID  ", //818
                                 "INVALID  ", //819
                                 "INVALID  ", //820
                                 "INVALID  ", //821
                                 "INVALID  ", //822
								 "INVALID  ", //823
                                 "INVALID  ", //824
                                 "INVALID  ", //825
                                 "INVALID  ", //826
                                 "INVALID  ", //827
                                 "INVALID  ", //828
                                 "INVALID  ", //829
                                 "INVALID  ", //830
                                 "INVALID  ", //831
                                 "INVALID  ", //832
                                 "INVALID  ", //833
                                 "INVALID  ", //834
                                 "INVALID  ", //835
                                 "INVALID  ", //836
                                 "INVALID  ", //837
                                 "INVALID  ", //838
                                 "INVALID  ", //839
                                 "INVALID  ", //840
                                 "INVALID  ", //841
                                 "INVALID  ", //842
                                 "INVALID  ", //843
                                 "INVALID  ", //844
                                 "INVALID  ", //845
                                 "INVALID  ", //846
                                 "INVALID  ", //847
								 "INVALID  ", //848
                                 "INVALID  ", //849
                                 "INVALID  ", //850
                                 

								/* Define the UsbX device stack events first.    */																								    														                      
								"UX DEV STK ALT SETTIN GET", /* TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_GET  (850 + 1)				 I1 = interface value  */					    														               
								"UX DEV STK ALT SETTIN SET", /* TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_SET  (850 + 2)				 I1 = interface value , I2 = alternate setting value  */					    														               
								"UX DEV STK CLASS REGISTER", /* TML_UX_TRACE_DEVICE_STACK_CLASS_REGISTER  (850 + 3)				 I1 = class name      , I2 = interface number, I3 = parameter  */
								"UX DEV STK CLEAR_FEATURE ", /* TML_UX_TRACE_DEVICE_STACK_CLEAR_FEATURE  (850 + 4)				 I1 = request type    , I2 = request value   , I3 = request index  */
								"UX DEV STK CONFIG GET    ", /* TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_GET  (850 + 5)				 I1 = configuration value  */
								"UX DEV STK CONFIG SET    ", /* TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_SET  (850 + 6)				 I1 = configuration value  */
								"UX DEV STK CONNECT       ", /* TML_UX_TRACE_DEVICE_STACK_CONNECT  (850 + 7)  */					    														               
								"UX DEV STK DESCRIPTOR SND", /* TML_UX_TRACE_DEVICE_STACK_DESCRIPTOR_SEND  (850 + 8)				 I1 = descriptor type , I2 = request index  */					    														               
								"UX DEV STK DISCONNECT    ", /* TML_UX_TRACE_DEVICE_STACK_DISCONNECT  (850 + 9)				 I1 = device  */
								"UX DEV STK ENDPOINT STALL", /* TML_UX_TRACE_DEVICE_STACK_ENDPOINT_STALL  (850 + 10)				 I1 = endpoint  */
								"UX DEV STK GET STATUS    ", /* TML_UX_TRACE_DEVICE_STACK_GET_STATUS  (850 + 11)				 I1 = request type    , I2 = request value   , I3 = request index  */					    														               
								"UX DEV STK HOST WAKEUP   ", /* TML_UX_TRACE_DEVICE_STACK_HOST_WAKEUP  (850 + 12)  */					    														               
								"UX DEV STK INITIALIZE    ", /* TML_UX_TRACE_DEVICE_STACK_INITIALIZE  (850 + 13)  */					    														               
								"UX DEV STK INTERFACE DEL ", /* TML_UX_TRACE_DEVICE_STACK_INTERFACE_DELETE  (850 + 14)				 I1 = interface  */					    														               
								"UX DEV STK INTERFACE GET ", /* TML_UX_TRACE_DEVICE_STACK_INTERFACE_GET  (850 + 15)				 I1 = interface value  */
								"UX DEV STK INTERFACE SET ", /* TML_UX_TRACE_DEVICE_STACK_INTERFACE_SET  (850 + 16)				 I1 = alternate setting value  */
								"UX DEV STK SET FEATURE   ", /* TML_UX_TRACE_DEVICE_STACK_SET_FEATURE  (850 + 17)				 I1 = request value   , I2 = request index  */
								"UX DEV STK TRANSFER ABRT ", /* TML_UX_TRACE_DEVICE_STACK_TRANSFER_ABORT  (850 + 18)				 I1 = transfer request, I2 = completion code  */
								"UX DEV STK TRAN A RQ ABO ", /* TML_UX_TRACE_DEVICE_STACK_TRANSFER_ALL_REQUEST_ABORT  (850 + 19)				 I1 = endpoint        , I2 = completion code  */
								"UX DEV STK TRAN REQUEST  ", /* TML_UX_TRACE_DEVICE_STACK_TRANSFER_REQUEST  (850 + 20)				 I1 = transfer request  */
																																		    														                      
																																																						  
																																																										 "INVALID  ", //871
                                 "INVALID  ", //872
								"INVALID  ", //873
                                 "INVALID  ", //874
                                 "INVALID  ", //875
                                 "INVALID  ", //876
                                 "INVALID  ", //877
                                 "INVALID  ", //878
                                 "INVALID  ", //879
                                 "INVALID  ", //880
                                 "INVALID  ", //881
                                 "INVALID  ", //882
                                 "INVALID  ", //883
                                 "INVALID  ", //884
                                 "INVALID  ", //885
                                 "INVALID  ", //886
                                 "INVALID  ", //887
                                 "INVALID  ", //888
                                 "INVALID  ", //889
                                 "INVALID  ", //890
                                 "INVALID  ", //891
                                 "INVALID  ", //892
                                 "INVALID  ", //893
                                 "INVALID  ", //894
                                 "INVALID  ", //895
                                 "INVALID  ", //896
                                 "INVALID  ", //897
								 "INVALID  ", //898
                                 "INVALID  ", //899
                                 "INVALID  ", //900					  
																																																						  
								/* Define the UsbX device stack events first.    */																								    														                      
								"UX DEV CLS DPUMP ACTIVATE", /* TML_UX_TRACE_DEVICE_CLASS_DPUMP_ACTIVATE  (900 + 1)				 I1 = class instance  */       
								"UX DEV CLS DPUMP DEACTIVA", /* TML_UX_TRACE_DEVICE_CLASS_DPUMP_DEACTIVATE  (900 + 2)				 I1 = class instance  */       
								"UX DEV CLS DPUMP READ    ", /* TML_UX_TRACE_DEVICE_CLASS_DPUMP_READ  (900 + 3)				 I1 = class instance  , I2 = buffer          , I3 = requested_length  */
								"UX DEV CLS DPUMP WRITE   ", /* TML_UX_TRACE_DEVICE_CLASS_DPUMP_WRITE  (900 + 4)				 I1 = class instance  , I2 = buffer          , I3 = requested_length  */

								  "INVALID  ", //905
								  "INVALID  ", //906
								  "INVALID  ", //907
								  "INVALID  ", //908
								  "INVALID  ", //909
		 					  

											 									    														                      
								"UX DEV CLS CDC ACTIVATE  ", /* TML_UX_TRACE_DEVICE_CLASS_CDC_ACTIVATE  (900 + 10)				 I1 = class instance  */       
								"UX DEV CLS CDC DEACTIVATE", /* TML_UX_TRACE_DEVICE_CLASS_CDC_DEACTIVATE  (900 + 11)				 I1 = class instance  */       
								"UX DEV CLS CDC READ      ", /* TML_UX_TRACE_DEVICE_CLASS_CDC_READ  (900 + 12)				 I1 = class instance  , I2 = buffer          , I3 = requested_length  */
								"UX DEV CLS CDC WRITE     ", /* TML_UX_TRACE_DEVICE_CLASS_CDC_WRITE  (900 + 13)				 I1 = class instance  , I2 = buffer          , I3 = requested_length  */
											 				
								  "INVALID  ", //914
								  "INVALID  ", //915
								  "INVALID  ", //916
								  "INVALID  ", //917
								  "INVALID  ", //918
								  "INVALID  ", //919
		 	
															
								"UX DEV CLS HID ACTIVATE  ", /* TML_UX_TRACE_DEVICE_CLASS_HID_ACTIVATE  (900 + 20)				 I1 = class instance  */       
								"UX DEV CLS HID DEACTIVATE", /* TML_UX_TRACE_DEVICE_CLASS_HID_DEACTIVATE  (900 + 21)				 I1 = class instance  */       
								"UX DEV CLS HID EVENT GET ", /* TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_GET  (900 + 22)				 I1 = class instance  , I2 = hid event  */
								"UX DEV CLS HID EVENT SET ", /* TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_SET  (900 + 23)				 I1 = class instance  , I2 = hid event  */
								"UX DEV CLS HID REPORT GET", /* TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_GET  (900 + 24)				 I1 = class instance  , I2 = descriptor type , I3 = request index  */
								"UX DEV CLS HID REPORT SET", /* TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_SET  (900 + 25)				 I1 = class instance  , I2 = descriptor type , I3 = request index  */
								"UX DEV CLS HID DSCRIP SND", /* TML_UX_TRACE_DEVICE_CLASS_HID_DESCRIPTOR_SEND  (900 + 26)				 I1 = class instance  , I2 = descriptor type , I3 = request index  */
											 	 										
									
								"INVALID  ", //927
								"INVALID  ", //928
								"INVALID  ", //929
							 
						 
								"UX DEV CLS PIMA ACTIVATE ", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_ACTIVATE  (900 + 30)				 I1 = class instance  */       
								"UX DEV CLS PIMA DEACTIVAT", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_DEACTIVATE  (900 + 31)				 I1 = class instance  */       
								"UX DEV CLS PIMA DEV INF S", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_DEVICE_INFO_SEND  (900 + 32)				 I1 = class instance  */       
								"UX DEV CLS PIMA EVENT GET", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_GET  (900 + 33)				 I1 = class instance  , I2 = pima event  */       
								"UX DEV CLS PIMA EVENT SET", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_SET  (900 + 34)				 I1 = class instance  , I2 = pima event  */       
								"UX DEV CLS PIMA OBJ ADD  ", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_ADD  (900 + 35)				 I1 = class instance  , I2 = object handle  */       
								"UX DEV PIMA OBJ DATA_GET ", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_GET  (900 + 36)				 I1 = class instance  , I2 = object handle  */
								"UX DEV PIMA OBJ DATA SND ", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_SEND  (900 + 37)				 I1 = class instance  , I2 = object handle  */
								"UX DEV CLS PIMA OBJ_DELET", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DELETE  (900 + 38)				 I1 = class instance  , I2 = object handle  */
								"UX DEV PIMA OBJ HAND SND ", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_HANDLES_SEND  (900 + 39) I1 = class instance, I2 = storage id, I3 = object format code, I4 = object association  */
								"UX DEV CLS PIMA OBJ INF G", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_GET  (900 + 40)				 I1 = class instance  , I2 = object handle  */
								"UX DEV CLS PIMA OBJ INF S", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_SEND  (900 + 41)				 I1 = class instance  */
								"UX DEV PIMA OBJS NUM SEND", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECTS_NUMBER_SEND  (900 + 42)I1 = class instance  , I2 = storage id, I3 = object format code, I4 = object association  */
								"UX DEV PIMA PRT OBJ DAT G", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_PARTIAL_OBJECT_DATA_GET  (900 + 43) I1 = class instance, I2 = object handle, I3 = offset requested, I4 = length requested  */
								"UX DEV CLS PIMA RSPNS SND", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_RESPONSE_SEND  (900 + 44) I1 = class instance, I2 = response code, I3 = number parameter, I4 = pima parameter 1  */
								"UX DEV CLS PIMA STOR ID S", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_ID_SEND  (900 + 45)				 I1 = class instance  */
								"UX DEV CLS PIMA STOR IN S", /* TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_INFO_SEND  (900 + 46)				 I1 = class instance  */
										 							

								"INVALID  ", //947
								"INVALID  ", //948
								"INVALID  ", //949

		 
								"UX DEV CLS RNDIS ACTIVATE", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_ACTIVATE  (900 + 50)				 I1 = class instance  */       		
								"UX DEV CLS RNDIS DEACTIVA", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_DEACTIVATE  (900 + 51)				 I1 = class instance  */       
								"UX DEV CLS RNDIS PKT RECE", 	/* TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_RECEIVE  (900 + 52)				 I1 = class instance  */
								"UX DEV CLS RNDIS PKT TRNS", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_TRANSMIT  (900 + 53)				 I1 = class instance  */
								"UX DEV CLS RNDIS MSG QUER", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_QUERY  (900 + 54)				 I1 = class instance  , I2 = rndis OID  */
								"UX DEV CLS RNDIS MSG KP A", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_KEEP_ALIVE  (900 + 55)				 I1 = class instance  */
								"UX DEV CLS RNDIS MSG RSET", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_RESET  (900 + 56)				 I1 = class instance  */
								"UX DEV CLS RNDIS MSG SET ", /* TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_SET  (900 + 57)				 I1 = class instance  , I2 = rndis OID  */

                                 "INVALID  ", //958
                                 "INVALID  ", //959
                                 "INVALID  ", //960
                                 "INVALID  ", //961
                                 "INVALID  ", //962
                                 "INVALID  ", //963
                                 "INVALID  ", //964
                                 "INVALID  ", //965
                                 "INVALID  ", //966
                                 "INVALID  ", //967
                                 "INVALID  ", //968
                                 "INVALID  ", //969

																 										    														               
								"UX DEV CLS STOR ACTIVATE ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_ACTIVATE  (900 + 70)				 I1 = class instance  */       
								"UX DEV CLS STOR DEACTIVAT", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_DEACTIVATE  (900 + 71)				 I1 = class instance  */       
								"UX DEV CLS STOR FORMAT   ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_FORMAT  (900 + 72)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR INQUIRY  ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_INQUIRY  (900 + 73)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR MODE SELE", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SELECT  (900 + 74)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR MODE SENS", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SENSE  (900 + 75)				 I1 = class instance  , I2 = lun  */       
								"UX DEV STO PRE ALOW MED R", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_PREVENT_ALLOW_MEDIA_REMOVAL  (900 + 76)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR READ     ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ  (900 + 77)				 I1 = class instance  , I2 = lun, I3 = sector, I4 = number sectors       */       
								"UX DEV CLS STOR READ CAPA", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_CAPACITY  (900 + 78)            	 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR RD FRMT C", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_FORMAT_CAPACITY  (900 + 79)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR READ TOC ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_TOC  (900 + 80)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR RQST SENS", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_REQUEST_SENSE  (900 + 81)				 I1 = class instance  , I2 = lun, I3 = sense key, I4 = code  */       
								"UX DEV CLS STOR TEST RDY ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_TEST_READY  (900 + 82)            	 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR START STP", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_START_STOP  (900 + 83)            	 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR VERIFY   ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_VERIFY  (900 + 84)				 I1 = class instance  , I2 = lun  */       
								"UX DEV CLS STOR WRITE    ", /* TML_UX_TRACE_DEVICE_CLASS_STORAGE_WRITE (900 + 85)				 I1 = class instance  , I2 = lun, I3 = sector, I4 = number sectors  */       


								 "INVALID  ", //986
                                 "INVALID  ", //987
                                 "INVALID  ", //988
                                 "INVALID  ", //989
                                 "INVALID  ", //990
                                 "INVALID  ", //991
                                 "INVALID  ", //992
                                 "INVALID  ", //993
                                 "INVALID  ", //994
                                 "INVALID  ", //995
                                 "INVALID  ", //996
                                 "INVALID  ", //997
                                 "INVALID  ", //998

							 
								/* Define the UsbX Error Event.    */
								"UX TRACE ERROR           "/* TML_TML_UX_TRACE_ERROR		  											999  */



									};


/* Define the endian flag used by the helper routine(s).  */

unsigned long    tml_little_endian;

/* TML internal routines.  */
int     tml_calculate_priority_inversions(unsigned long total_threads, unsigned long total_objects, unsigned long total_events,
									unsigned long *priority_inversions, unsigned long *bad_priority_inversions);

int		tml_read_32(FILE *source_file, unsigned long *value);
int		tml_read_16(FILE *source_file, unsigned short *value);
int		tml_read_8(FILE *source_file, unsigned char *value);

/* Define the actual TML library functions.  */

int tml_read_32(FILE *source_file, unsigned long *value)
{

int	byte1;
int	byte2;
int	byte3;
int	byte4;
unsigned long	temp;


	/* Get the first byte.  */
	byte1 =  fgetc(source_file);

	/* Check for error.  */
	if (byte1 == EOF)
		return(1);

	/* Get the second byte.  */
	byte2 =  fgetc(source_file);

	/* Check for error.  */
	if (byte2 == EOF)
		return(1);

	/* Get the third byte.  */
	byte3 =  fgetc(source_file);

	/* Check for error.  */
	if (byte3 == EOF)
		return(1);

	/* Get the fourth byte.  */
	byte4 =  fgetc(source_file);

	/* Check for error.  */
	if (byte4 == EOF)
		return(1);

	/* Now use the endian flag to determine how to assemble the bytes!  */
	if (tml_little_endian)
	{
		temp =  (((unsigned long) byte4) << 24) |
				(((unsigned long) byte3) << 16) |
				(((unsigned long) byte2) << 8)  |
				 ((unsigned long) byte1);
	}
	else
	{
		temp =  (((unsigned long) byte1) << 24) |
				(((unsigned long) byte2) << 16) |
				(((unsigned long) byte3) << 8)  |
				 ((unsigned long) byte4);
	}

	/* Place the value in the return value.  */
	*value =  temp;

	return(0);
}


int tml_read_16(FILE *source_file, unsigned short *value)
{

int	byte1;
int	byte2;
unsigned short	temp;


	/* Get the first byte.  */
	byte1 =  fgetc(source_file);

	/* Check for error.  */
	if (byte1 == EOF)
		return(1);

	/* Get the second byte.  */
	byte2 =  fgetc(source_file);

	/* Check for error.  */
	if (byte2 == EOF)
		return(1);

	/* Now use the endian flag to determine how to assemble the bytes!  */
	if (tml_little_endian)
	{
		temp =  (((unsigned short) byte2) << 8) |
				 ((unsigned short) byte1);
	}
	else
	{
		temp =  (((unsigned short) byte1) << 8) |
				 ((unsigned short) byte2);
	}

	/* Place the value in the return value.  */
	*value =  temp;

	return(0);
}


int tml_read_8(FILE *source_file, unsigned char *value)
{

int	byte1;


	/* Get the first byte.  */
	byte1 =  fgetc(source_file);

	/* Check for error.  */
	if (byte1 == EOF)
		return(1);

	/* Place the value in the return value.  */
	*value =  (unsigned char) byte1;

	return(0);
}


/* Define the TML initialization routine that processes the trace file and gets everything
   into nice and manageable data structures with access functions for the application using
   it.  */
int  tml_initialize(FILE *source_trace_file, unsigned long *total_threads, unsigned long *total_objects, 
								unsigned long *total_events, _int64 *max_relative_ticks, char  **error_string)
{

int				status;
unsigned char	byte;
unsigned char   available, type, reserved1, reserved2;
unsigned long   i, j, k;
#if 0
unsigned long   l, m;
#endif
unsigned long	actual_objects;
unsigned long	objects;
unsigned long	events;
unsigned long	actual_events;
unsigned long	threads;
unsigned long   address;
unsigned long   parameter1;
unsigned long   parameter2;
char			name[TML_OBJECT_NAME_SIZE];
TML_EVENT		*working_event_array;
TML_EVENT		*first_portion_array;
TML_EVENT		*new_event_array;
unsigned long	first_portion_size;
unsigned long   previous_time_stamp;
unsigned long   current_time_stamp;
unsigned long   tick_decreases;
unsigned long	tick_increases;
unsigned long	thread_index;
TML_OBJECT		*new_object_array;
unsigned long	*new_thread_list;
_int64			relative_ticks;
_int64			delta_ticks;
unsigned long	current_thread;
unsigned long   interrupt_nesting;
char			*temp_string;
unsigned long	new_events;
unsigned long   priority;
unsigned long   preemption_threshold;
FILE            *converted_file;


	/* NULL terminate error string.  */
	if (error_string)	
		*error_string =  0;
	else
		error_string = &temp_string;

	/* Default to big endian.  */
	tml_little_endian =  0;

    /* Set the converted file pointer to NULL.  */
    converted_file =  NULL;
	
    do
    {
	    /* First read in the trace id.  */
        status =  tml_read_32(source_trace_file, &tml_header_trace_id);

        /* Check for an error condition.  */
        if (status)
        {

            /* Read error, return 1.  */
            *error_string =  tml_header_id_read_error;
            return(1);
        }

        /* Determine if the trace buffer ID is valid and whether or not little endian is present.  */
        if (tml_header_trace_id == TML_TRACE_VALID_LE)
        {
        
		/* Yes, little endian is present - switch to little endian. */
		tml_little_endian =  1;
            break;
        } 
        else if (tml_header_trace_id == TML_TRACE_VALID_BE)
        {

            /* Bad trace buffer, return an error.  */
            tml_little_endian =  0;
            break;
	} 
        else
        {
    
            /* Attempt to convert the file from HEX or S-Records.  */
            status =  tml_convert_file(source_trace_file, &converted_file);
        
            /* Determine if the conversion was successful.  */
            if (status)
	{

		/* Bad trace buffer, return an error.  */
		*error_string =  tml_header_id_error;
		return(2);
	}

            /* Otherwise, replace the source file pointer with the converted file.  */
            source_trace_file =   converted_file;
            
            /* Rewind the converted file.  */
            fseek(source_trace_file, 0, SEEK_SET);
        }
    } while(1);

	/* Now read the rest of the header information in.  */
	status =   tml_read_32(source_trace_file, &tml_header_timer_valid_mask);
	status +=  tml_read_32(source_trace_file, &tml_header_trace_base_address);
	status +=  tml_read_32(source_trace_file, &tml_header_object_registry_start_address);
	status +=  tml_read_16(source_trace_file, &tml_header_reserved1);
	status +=  tml_read_16(source_trace_file, &tml_header_object_name_size);
	status +=  tml_read_32(source_trace_file, &tml_header_object_registry_end_address);
	status +=  tml_read_32(source_trace_file, &tml_header_trace_buffer_start_address);
	status +=  tml_read_32(source_trace_file, &tml_header_trace_buffer_end_address);
	status +=  tml_read_32(source_trace_file, &tml_header_trace_buffer_current_address);
	status +=  tml_read_32(source_trace_file, &tml_header_reserved2);
	status +=  tml_read_32(source_trace_file, &tml_header_reserved3);
	status +=  tml_read_32(source_trace_file, &tml_header_reserved4);

    if (tml_header_object_name_size >= TML_OBJECT_NAME_SIZE)
    {
        /* Determine if the converted file needs to be closed.  */
        if (converted_file)
            fclose(converted_file);

		*error_string =  tml_header_read_error;
		return(3);
    }

	/* Check for an error condition.  */
	if (status)
	{

        /* Determine if the converted file needs to be closed.  */
        if (converted_file)
            fclose(converted_file);

		/* Read header error, return 3.  */
		*error_string =  tml_header_read_error;
		return(3);
	}

	/* At this point we need to process the object registry.  */

	/* Calculate how many objects there are in the registry so we can look 
	   through all of them.  */
	if ((tml_header_object_registry_end_address <= tml_header_object_registry_start_address) ||
		(tml_header_object_name_size > ULONG_MAX - 16))
	{

		/* Free resources already allocated.  */
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
	objects =  (tml_header_object_registry_end_address - tml_header_object_registry_start_address)/(16UL+tml_header_object_name_size);

	/* Remember the maximum objects.  */
	tml_max_objects =  objects;

	/* Allocate the size of the internal object array.  */
	if(objects > SIZE_MAX / sizeof(TML_OBJECT))
	{

		/* Free resources already allocated.  */
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
	tml_object_array =  (TML_OBJECT *) malloc(objects*sizeof(TML_OBJECT));
	
	/* Check for an error condition.  */
	if (!tml_object_array)
	{

        /* Determine if the converted file needs to be closed.  */
        if (converted_file)
            fclose(converted_file);

		/* System error.  */
		*error_string =  tml_object_allocation_error;
		return(4);
	}

	/* Allocate size for thread only index array.  */
	if (objects > SIZE_MAX / sizeof(unsigned long))
	{

		/* Free resources already allocated.  */
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
	tml_object_thread_list =  (unsigned long *) malloc(objects*sizeof(unsigned long));

	/* Check for an error condition.  */
	if (!tml_object_thread_list)
	{

        /* Determine if the converted file needs to be closed.  */
        if (converted_file)
            fclose(converted_file);

		/* System error.  */
		*error_string =  tml_thread_allocation_error;
		return(5);
	}

	/* Initialize the thread count to zero.  */
	threads =  0;

	/* Initialize the actual objects count to zero.  */
	actual_objects =  0;

	/* Loop to read in all the objects.  */
	for (i = 0; i < objects; i++)
	{
		/* Read in the next object.  */
		status =  tml_read_8(source_trace_file, &available);
		status += tml_read_8(source_trace_file, &type);
		status += tml_read_8(source_trace_file, &reserved1);
		status += tml_read_8(source_trace_file, &reserved2);
		status += tml_read_32(source_trace_file, &address);
		status += tml_read_32(source_trace_file, &parameter1);
		status += tml_read_32(source_trace_file, &parameter2);

		/* Read in the object name string.  */
		for (j = 0; j < (unsigned long) tml_header_object_name_size; j++)
		{

			/* Read a byte in.  */
			status += tml_read_8(source_trace_file, &byte);

			/* Determine if we have storage room for it.  */
			if (j < TML_OBJECT_NAME_SIZE-1)
			{

				/* Yes there is room, place it in the buffer.  */
				name[j] =  byte;
			}
		}

		/* Check for a read error.  */
		if (status)
		{

            /* Determine if the converted file needs to be closed.  */
            if (converted_file)
                fclose(converted_file);

            /* Register error, return 2.  */
			*error_string =  tml_object_name_read_error;
			return(6);
		}

		/* Null terminate the string.  */
		name[j] =  0;

        /* Determine if the entry is valid. The registry will always start with every entry marked as invalid. Hence,
		   a non-zero type indicates there is a valid registry entry.  */
        if (type)
        {

            /* Yes, this entry is in-use so store it in the registry.  */
			tml_object_array[actual_objects].tml_object_type =                          (unsigned long) type;
            tml_object_array[actual_objects].tml_object_address =                       address;
            tml_object_array[actual_objects].tml_object_parameter_1 =                   parameter1;
            tml_object_array[actual_objects].tml_object_parameter_2 =                   parameter2;
            tml_object_array[actual_objects].tml_object_reserved1 =                     reserved1;
            tml_object_array[actual_objects].tml_object_reserved2 =                     reserved2;
            tml_object_array[actual_objects].tml_object_lowest_priority =               0xFFFFFFFF;
            tml_object_array[actual_objects].tml_object_highest_priority =              0xFFFFFFFF;
            tml_object_array[actual_objects].tml_object_lowest_preemption_threshold =   0xFFFFFFFF;
            tml_object_array[actual_objects].tml_object_highest_preemption_threshold =  0xFFFFFFFF;
            tml_object_array[actual_objects].tml_object_total_events =                  0;

            /* Loop to copy the string.  */
            j = 0;
            do
            {
                /* Copy character.  */
                tml_object_array[actual_objects].tml_object_name[j] =  name[j];

                /* Check for null termination.  */
                if (name[j] == 0)
                    break;

                /* Move to next character.  */
                j++;

            } while (j < TML_OBJECT_NAME_SIZE);

            /* Determine if a non-printable character starts the string.  */
            if ((tml_object_array[actual_objects].tml_object_name[0] < 0x20) ||
                (tml_object_array[actual_objects].tml_object_name[0] > 0x7E))
            {

                tml_object_array[actual_objects].tml_object_name[0] =  'N';
                tml_object_array[actual_objects].tml_object_name[1] =  'o';
                tml_object_array[actual_objects].tml_object_name[2] =  'n';
                tml_object_array[actual_objects].tml_object_name[3] =  '-';
                tml_object_array[actual_objects].tml_object_name[4] =  'A';
                tml_object_array[actual_objects].tml_object_name[5] =  'S';
                tml_object_array[actual_objects].tml_object_name[6] =  'C';
                tml_object_array[actual_objects].tml_object_name[7] =  'I';
                tml_object_array[actual_objects].tml_object_name[8] =  'I';
                tml_object_array[actual_objects].tml_object_name[9] =  ' ';
                tml_object_array[actual_objects].tml_object_name[10] = 'N';
                tml_object_array[actual_objects].tml_object_name[11] = 'a';
                tml_object_array[actual_objects].tml_object_name[12] = 'm';
                tml_object_array[actual_objects].tml_object_name[13] = 'e';
                tml_object_array[actual_objects].tml_object_name[14] = 0;
            }

            /* Determine if this is a thread.  */
            if (type == TML_TRACE_OBJECT_TYPE_THREAD)
            {

                /* Store index in the thread only list.  */
                tml_object_thread_list[threads] =  actual_objects;

                /* Yes, increment the thread count.  */
                threads++;
                
                /* Determine if there is an initial priority in the thread object registry.  Newer versions of 
                   ThreadX have this.  */
                if (reserved1 & 0x80)
                {
                
                    /* Yes, pickup the initial priority.  */
                    priority =  (unsigned long) (reserved1 & 0x3);
                    priority =  (priority << 8) | ((unsigned long) reserved2);
                    
                    /* Override the lowest/highest priority.  */
                    tml_object_array[actual_objects].tml_object_lowest_priority =   priority;
                    tml_object_array[actual_objects].tml_object_highest_priority =  priority;
                }
            }

            /* Move to next actual object.  */
            actual_objects++;
        }
    }

	/* Save the actual objects.  */
	tml_total_objects =  actual_objects;

	/* Save the total threads.  */
	tml_total_threads =  threads;

	/* At this point we are ready to read in the event log.  Note that a NULL
	   thread pointer indicates a partial buffer.  */

	/* Calculate the total number of events.  */
	if((tml_header_trace_buffer_end_address <= tml_header_trace_buffer_start_address) ||
		((tml_header_trace_buffer_end_address - tml_header_trace_buffer_start_address) > ULONG_MAX / 32))
	{

		/* Free resources already allocated.  */
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
	events =   (tml_header_trace_buffer_end_address - tml_header_trace_buffer_start_address)/32;

	/* Allocate memory for the event log.  */
	if(events > SIZE_MAX / sizeof(TML_EVENT))
	{

		/* Free resources already allocated.  */
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
	working_event_array =  (TML_EVENT *) malloc(events * sizeof(TML_EVENT));

	/* Check for an error condition.  */
	if (!working_event_array)
	{

        /* Determine if the converted file needs to be closed.  */
        if (converted_file)
            fclose(converted_file);

		/* System error.  */
		*error_string =  tml_event_allocation_error;
		return(7);
	}

	/* Setup various loop control variables.  */
	address =				tml_header_trace_buffer_start_address;
	actual_events =			0;
	j =						0;
	first_portion_array =   0;
	first_portion_size =	0;
	previous_time_stamp =   0;
	tick_decreases =		0;
	tick_increases =        0;

	/* Loop through the events and store them into the internal array.  */
	for (i = 0; i < events; i++)
	{

		/* Read in the event from the trace file.  */
		status =  tml_read_32(source_trace_file, &working_event_array[j].tml_event_context);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_thread_priority);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_id);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_time_stamp);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_info_1);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_info_2);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_info_3);
		status += tml_read_32(source_trace_file, &working_event_array[j].tml_event_info_4);

		/* Check for a read error.  */
		if (status)
		{

            /* Determine if the converted file needs to be closed.  */
            if (converted_file)
                fclose(converted_file);

			/* Event read error, return 8.  */
			*error_string =  tml_event_read_error;
			return(8);
		}

		/* Determine if this is a partial trace buffer as indicated by a NULL thread pointer. */
		if (working_event_array[j].tml_event_context == 0)
			break;

		/* Otherwise, we have a valid trace buffer.  */

		/* Determine if the address is the oldest in the array.  */
		if (address == tml_header_trace_buffer_current_address)
		{

			/* Yes, this entry is the oldest entry, so we need to allocate a new event buffer
			   and start from this entry. After the loop, the previous buffer will be merged
			   to the end.  */

			/* Save information on the first portion of the trace buffer.  */
			first_portion_array =  working_event_array;
			first_portion_size =   actual_events;

			/* Allocate memory for the event log.
			 * events was tested for overflow above.
			 */
			working_event_array =  (TML_EVENT *) malloc(events * sizeof(TML_EVENT));

			/* Check for an error condition.  */
			if (!working_event_array)
			{

                /* Determine if the converted file needs to be closed.  */
                if (converted_file)
                    fclose(converted_file);

                /* System error.  */
                *error_string =  tml_event_allocation_error;
                return(9);
            }

            /* Copy the last event into the new array.  */
            working_event_array[0] =  first_portion_array[j];

            /* Set j back to 0!  */
            j =  0;
        }

        /* Save off the raw event ID and priority fields, since they have additional purposes.  */
        working_event_array[j].tml_event_raw_id =           working_event_array[j].tml_event_id;
        working_event_array[j].tml_event_raw_priority =     working_event_array[j].tml_event_thread_priority;

        /* Adjust some of the fields of the current event.  */
        working_event_array[j].tml_event_time_stamp =       working_event_array[j].tml_event_time_stamp & tml_header_timer_valid_mask;
        working_event_array[j].tml_event_relative_ticks =   0;

        /* Keep the event ID to the lower 64 bits.  */
        working_event_array[j].tml_event_id =  (working_event_array[j].tml_event_id & 0x0000FFFF);

        /* Default the preemption-threshold to invalid.  */
        working_event_array[j].tml_event_thread_preemption_threshold =  0xFFFFFFFF;

        /* Determine if the priority needs to be adjusted.  */
        if ((working_event_array[j].tml_event_context != 0) && 
            (working_event_array[j].tml_event_context != 0xF0F0F0F0) && 
            (working_event_array[j].tml_event_context != 0xFFFFFFFF))
        {
        
            /* A thread is present.  */ 
            
            /* Does this trace buffer have preemption-threshold?  */
            if (working_event_array[j].tml_event_thread_priority & 0x80000000)
            {
            
                /* Yes, preemption-threshold is present.  Setup the preemption-threshold.  */
                working_event_array[j].tml_event_thread_preemption_threshold =  (working_event_array[j].tml_event_thread_priority >> 16) & 0x000003FF;
            }

            /* In any case, isolate the thread priority to the right values.  */
            working_event_array[j].tml_event_thread_priority =  working_event_array[j].tml_event_thread_priority & 0x000003FF;
        }

		/* Look at the time stamp.  */
		current_time_stamp =  working_event_array[j].tml_event_time_stamp;

        /* Only look at this after the first event.  */
        if (j > 1)
        {

            /* Check to see if there is any change.  */
            if (current_time_stamp != previous_time_stamp)
            {
            
		if (current_time_stamp > previous_time_stamp)
			tick_increases++;
		else
			tick_decreases++;
            }
        }

		/* Save previous time stamp.  */
		previous_time_stamp =  current_time_stamp;

		/* Increment the actual event counter.  */
		actual_events++;

		/* Move pointers and indexes forward.  */
		address =  address + 32;

		j++;
	}

	/* Determine if we have a partial file, this is the most common case.  */
	if (first_portion_array)
	{

		/* Simply copy the elements of the first portion to the end of the current
		   working array... then everything will be in order from oldest to newest.  */
		for (i = 0; i < first_portion_size; i++)
		{
			/* Copy event.  */
			working_event_array[j++] =  first_portion_array[i];
		}

		/* Release the memory for the first portion.  */
		free(first_portion_array);
	}


	/* Make another pass through the array, building the relative time.  */
	relative_ticks =  0;
	for (i = 1; i < actual_events; i++)
	{
		unsigned long  current_stamp;
		unsigned long  previous_stamp;

		/* Pickup the current and previous time stamps.  */
		current_stamp =  working_event_array[i].tml_event_time_stamp;
		previous_stamp = working_event_array[i-1].tml_event_time_stamp;

		/* Determine which way the time stamps are going.  */
		if (tick_increases > tick_decreases)
		{
			/* Increasing time stamps.  */

			/* Determine if we have the normal condition.  */
			if (current_stamp >= previous_stamp)
			{


				/* Update the relative time accordingly.  */
				relative_ticks =  relative_ticks + (_int64) (current_stamp - previous_stamp);
			}
			else
			{

				/* Time has wrapped.  */
				relative_ticks =  relative_ticks + ((_int64) (tml_header_timer_valid_mask - previous_stamp)) + ((_int64) (current_stamp)) + ((_int64) 1);
			}
		}
		else
		{
			/* Decreasing time stamps.  */

			/* Determine if we have the normal condition.  */
			if (current_stamp <= previous_stamp)
			{

				/* Update the relative time accordingly.  */
				relative_ticks =  relative_ticks + (_int64) (previous_stamp - current_stamp);
			}
			else
			{

				/* Time has wrapped.  */
				relative_ticks =  relative_ticks + ((_int64) (tml_header_timer_valid_mask - current_stamp)) + ((_int64) (previous_stamp)) + ((_int64) 1);
			}
		}

		/* Store the relative time in the event.  */
		working_event_array[i].tml_event_relative_ticks =  relative_ticks;
	}


	/* Make yet another pass through the array, building the next context.  Values for tml_event_next_context are as follows:
					
					  tml_event_next_context ==  0				-> Next context is idle
					  tml_event_next_context ==  0xFFFFFFFF		-> Next context is interrupt
					  tml_event_next_context ==  0xF0F0F0F0		-> Next context is initialization
					  tml_event_next_context ==  other values	-> Next context is a thread 
	*/
	current_thread =     0;
	interrupt_nesting =  0;
	for (i = 0; i < actual_events; i++)
	{

		/* Determine if we are in a thread context.  */
		if ((working_event_array[i].tml_event_context != 0xFFFFFFFF) && (working_event_array[i].tml_event_context != 0xF0F0F0F0))
		{
			/* Yup, remember the current thread.  */
			current_thread =  working_event_array[i].tml_event_context;

			/* Clear the nested interrupt counter.  */
			interrupt_nesting =  0;
		}

		/* Determine if the current event is Initialization or Interrupt.  */
		if ((working_event_array[i].tml_event_context == 0xFFFFFFFF) || (working_event_array[i].tml_event_context == 0xF0F0F0F0))
		{
            /* Check for an interrupt event.  */
            if (working_event_array[i].tml_event_context == 0xFFFFFFFF)
            {


                /* Now check and see if the priority field is non-null. In newer versions of ThreadX, this field contains
                   the current thread pointer in interrupt context.  */
                if (working_event_array[i].tml_event_thread_priority)
                {

                    /* Yup, newer version of ThreadX is present. Look to see if we need to update the previous events next context.  */
                    if ((i > 1) && (working_event_array[i-1].tml_event_context != 0xFFFFFFFF) && (working_event_array[i].tml_event_thread_priority) && (working_event_array[i].tml_event_thread_priority != 0xFFFFFFFF))
                    {

                        /* Yup, we need to update the previous next context and other interrupt related variables.  */
                        working_event_array[i-1].tml_event_next_context =  working_event_array[i].tml_event_thread_priority;
                        current_thread =  working_event_array[i].tml_event_thread_priority;
                    }
                }
            }

            /* Default the next context to the current context.  */
            working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_context;

            /* Look for ISR enter.  */
			if (working_event_array[i].tml_event_id == TML_TRACE_ISR_ENTER)
			{

				/* Increment the nested counter.  */
				interrupt_nesting++;
            }

            /* Check for thread suspend event from within an ISR.  */
            else if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_SUSPEND)
            {
            
                /* Use the Info4 field to set the current thread.  */
                current_thread =  working_event_array[i].tml_event_info_4;
            }

            /* Check for time slice event.  */
            else if (working_event_array[i].tml_event_id == TML_TRACE_TIME_SLICE)
            {

                /* Use the Info1 field to set the current thread.  */
                current_thread =  working_event_array[i].tml_event_info_1;
            }
                    
            /* Check for resume event from within an ISR.  */
            else if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_RESUME)
            {


                /* Determine if there is a next thread pointer. If so, use it!  */
                if (working_event_array[i].tml_event_info_4)
                {

                    /* This is the next thread to execute.  */
                    current_thread =  working_event_array[i].tml_event_info_4;
			}
			else 
			{
                    /* We have to search ahead and see if we can tell if a thread preemption occurred as a result 
                       of the thread resume.  */

                    /* Pickup the thread that was resumed.  */
                    address =  working_event_array[i].tml_event_info_1;

                    /* If the new address appears as a context before the current thread, we can assume that preemption
                       has occurred.  */
                    j =  i+1;
                    while (j < actual_events)
                    {

                        /* Determine if the event is in a thread context.  */
                        if ((working_event_array[j].tml_event_context != 0xFFFFFFFF) && (working_event_array[j].tml_event_context != 0xF0F0F0F0))
                        {

                            /* Yes, we are in a thread context, check to see if the thread matches the thread resumed.  */
                            if (current_thread == working_event_array[j].tml_event_context)
                                break;

                            
                            if (address == working_event_array[j].tml_event_context)
                            {

                                /* Yes, setup this thread as the current thread.  */
                                current_thread =  address;
                                break;
                            }
                        }

                        /* Look at the next event.  */
                        j++;
                    }
                }
            }

            
				/* Check for an ISR exit event.  */
            else if (working_event_array[i].tml_event_id == TML_TRACE_ISR_EXIT)
			{

                /* Determine if there is a nested count, need this for partial buffers.  */
                if (interrupt_nesting)
                {

                    /* Yes, decrement. */
                    interrupt_nesting--;
                }
            }

            /* In any case, check for an interrupt count...   */
            if (interrupt_nesting == 0)
            {

                /* We need to now look ahead to see if the next event is an interrupt or a thread event.  */

                /* Yes, look at the next event if there is one.  */
                if ((i+1) < actual_events)
                {

                    /* Is the next event an interrupt event and not an internal resume?  */
                    if ((working_event_array[i+1].tml_event_context == 0xFFFFFFFF) &&
                        (working_event_array[i+1].tml_event_id != TML_TRACE_THREAD_RESUME))
                    {


						/* Special case. If the next event is an interrupt event AND there is a thread context saved (it is in the priority field for interrupts) then use it for the current event's next context. */
						if ((working_event_array[i+1].tml_event_context == 0xFFFFFFFF) && (working_event_array[i+1].tml_event_thread_priority))
						{

							/* Use the context of the interrupt for better accuracy (which is stored in the thread priority field for interrupt events).  */
							working_event_array[i].tml_event_next_context =  working_event_array[i+1].tml_event_thread_priority;
						}
						else
						{

                            /* Yup, use current thread instead.  */
                            working_event_array[i].tml_event_next_context =  current_thread;
                        }
                    }
                    else
                    {

                        /* The next context is always the next event context.  */
                        working_event_array[i].tml_event_next_context =  working_event_array[i+1].tml_event_context;
                    }
                }
                else
                {

                    /* Use the current thread if we are at the end of the trace.  */
                    working_event_array[i].tml_event_next_context =  current_thread;
                }
            }
        }

		/* At this point the current event is always an event in a thread context!  */

		/* Check for thread suspend event.  */
		else if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_SUSPEND)
		{
			
			/* Use the Info4 field to set the next context field.  */
			working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_info_4;

			/* Determine if the very next event has the same context and is a resume. This indicates a trick inside of 
               the ThreadX priority inheritance and we should override the next context in this case.  */
            if (((i+1) < actual_events) && (working_event_array[i].tml_event_context == working_event_array[i].tml_event_info_1) &&
                (working_event_array[i+1].tml_event_context == working_event_array[i].tml_event_info_1))
            {

                /* Yup, override the next context for this event.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_context;
            }

            current_thread =  working_event_array[i].tml_event_next_context;
        }

        /* Check for resume event.  */
        else if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_RESUME)
        {

            /* Determine if there is a next thread pointer. If so, use it!  */
            if (working_event_array[i].tml_event_info_4)
            {

                /* This is the next thread to execute.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_info_4;
                current_thread =  working_event_array[i].tml_event_next_context;
            }
            else
            {
                /* We have to search ahead and see if we can tell if a thread preemption occurred as a result 
                   of the thread resume.  */

                /* First, default the next context to the current context.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_context;
                

                /* Pickup the thread that was resumed.  */
                address =  working_event_array[i].tml_event_info_1;

                /* If the address is the context of the next thread execution, then we can assume that this
                   thread resume resulted in a preemption context switch.  */
                j =  i+1;
                while (j < actual_events)
                {

                    /* Determine if the event is in a thread context.  */
                    if ((working_event_array[j].tml_event_context != 0xFFFFFFFF) && (working_event_array[j].tml_event_context != 0xF0F0F0F0))
                    {
                        /* Yes, we are in a thread context, check to see if the thread matches the thread resumed.  */
                        if (address == working_event_array[j].tml_event_context)
                        {

                            /* Yes, setup this as the next context in the current event.  */
                            working_event_array[i].tml_event_next_context =  address;
                            current_thread =  working_event_array[i].tml_event_next_context;
                        }

                        /* In any case, get out of this loop.  */
                        break;
                    }

                    /* Look at the next event.  */
                    j++;
                }
            }
        }

        /* Check for relinquish event.  */
        else if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_RELINQUISH)
        {

            /* Determine if there is a next thread pointer. If so, use it!  */
            if (working_event_array[i].tml_event_info_2)
            {

                /* This is the next thread to execute.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_info_2;
                current_thread =  working_event_array[i].tml_event_next_context;
            }
            else
            {

                /* We have to search ahead and see if we can tell if a thread change occurred as a result 
                   of the thread relinquish.  */

                /* First, default the next context to the current context.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_context;
                

                /* First find the next thread execution.  */
                address =  working_event_array[i].tml_event_context;

                /* If the address is the context of the next thread execution, then we can assume that this
                   thread resume resulted in a preemption context switch.  */
                j =  i+1;

                while (j < actual_events)
                {

                    /* Determine if the event is in a thread context.  */
                    if ((working_event_array[j].tml_event_context != 0xFFFFFFFF) && (working_event_array[j].tml_event_context != 0xF0F0F0F0))
                    {

                        /* Save the address of the next thread execution.  */
                        address =  working_event_array[j].tml_event_context;
                        
                        /* In any case, get out of this loop.  */
                        break;
                    }

                    /* Look at the next event.  */
                    j++;
                }

            
                /* Now determine if the address is different.  */
                k =  i+1;
                if (address != working_event_array[i].tml_event_context)
                {

                    /* Yup, something has changed.  Determine if there was a thread resume of the different thread between the 
                       relinquish and the next thread execution.  */
                    while (k < j)
                    {
    
                        /* Is this event a thread resume?  */
                        if ((working_event_array[k].tml_event_id == TML_TRACE_THREAD_RESUME) &&
                            (working_event_array[k].tml_event_info_1 == address))
                        {

                            /* Yup, the thread change is a result of the thread resumption.  */
                            break;
                        }

                        /* Is this event a thread suspend?  */
                        if ((working_event_array[k].tml_event_id == TML_TRACE_THREAD_SUSPEND) &&
                            (working_event_array[k].tml_event_info_1 == address))
                        {

                            /* Yup, the thread change is a result of the thread suspension.  */
                            break;
                        }
                        
                        /* Move to next event.  */
                        k =  k + 1;
                    }
                }

                /* Now determine if we need to update the next context for this event.  */
                if ((address != working_event_array[i].tml_event_context) && (j == k))
                {

                    /* Update the next context. */
                    working_event_array[i].tml_event_next_context =  address;
                }
                current_thread =  working_event_array[i].tml_event_next_context;
            }
        }

        /* All other events.  */
        else
        {

            /* Determine if the next event is a thread execution.  */
            if ((i+1 < actual_events) && (working_event_array[i+1].tml_event_context != 0xFFFFFFFF) && (working_event_array[i+1].tml_event_context != 0xF0F0F0F0))
            {

                /* Use the next thread as the context.   */
                working_event_array[i].tml_event_next_context =  working_event_array[i+1].tml_event_context;
            }
            else
            {

                /* Else, use the current event as the next context.  */
                working_event_array[i].tml_event_next_context =  working_event_array[i].tml_event_context;
            }
            current_thread =  working_event_array[i].tml_event_next_context;
        }
    }


    /* Make another pass through the events to find the context id. If not found, create a context ID with an unknown name so the GUI doesn't get
       confused.  */
    for (i = 0; i < actual_events; i++)
    {
        /* Clear the priority inversion event index in each event - indicating there is no priority inversion.  */
        working_event_array[i].tml_event_priority_inversion =      0;
        working_event_array[i].tml_event_bad_priority_inversion =  0;
        working_event_array[i].tml_event_thread_index =            0;

        /* Determine if the current event is a thread.  */
        if ((working_event_array[i].tml_event_context != 0xFFFFFFFF) && (working_event_array[i].tml_event_context != 0xF0F0F0F0))
        {

            /* Yes, current event is a thread. Let's try to find the context for the thread.  */
            status =  tml_object_by_address_find(working_event_array[i].tml_event_context, &thread_index);

            /* Determine if it was successful.  */
            if ((status == 0) && (tml_object_array[thread_index].tml_object_type == TML_TRACE_OBJECT_TYPE_THREAD))
            {

                /* Find the thread index for this event.  */
                for (j = 0; j < tml_total_threads; j++)
                {
                    if (tml_object_thread_list[j] == thread_index)
                    {
                        /* Use the index into the thread list.  */
                        break;
                    }
                }

				/* Store the thread index in the event.  */
                working_event_array[i].tml_event_thread_index =  j;
                
                /* Determine if the priorities have been setup.  */
                if (tml_object_array[thread_index].tml_object_lowest_priority == 0xFFFFFFFF)
                {
                
                    /* First event for this thread, simply mark the highest and lowest priority to the same value.  */
                    tml_object_array[thread_index].tml_object_highest_priority =    working_event_array[i].tml_event_thread_priority;
                    tml_object_array[thread_index].tml_object_lowest_priority =     working_event_array[i].tml_event_thread_priority;
                }
                else
                {
                
                    /* Not the first event...  */
                    
                    /* Do we have a new lowest priority?  */
                    if (working_event_array[i].tml_event_thread_priority > tml_object_array[thread_index].tml_object_lowest_priority)
                    {
                    
                        /* Yes, setup the new lowest priority.  */
                        tml_object_array[thread_index].tml_object_lowest_priority =  working_event_array[i].tml_event_thread_priority;
                    }
                    
                    /* Do we have a new highest priority?  */
                    if (working_event_array[i].tml_event_thread_priority < tml_object_array[thread_index].tml_object_highest_priority)
                    {
                    
                        /* Yes, setup the new highest priority.  */
                        tml_object_array[thread_index].tml_object_highest_priority =  working_event_array[i].tml_event_thread_priority;
                    }
                }

                /* Determine if this event has a valid preemption-threshold.  */
                if (working_event_array[i].tml_event_thread_preemption_threshold != 0xFFFFFFFF)
                {

                    /* Determine if the preemption-threshold has been setup.  */
                    if (tml_object_array[thread_index].tml_object_lowest_preemption_threshold == 0xFFFFFFFF)
                    {
                
                        /* First event for this thread, simply mark the highest and lowest preemption-threshold to the same value.  */
                        tml_object_array[thread_index].tml_object_highest_preemption_threshold =    working_event_array[i].tml_event_thread_preemption_threshold;
                        tml_object_array[thread_index].tml_object_lowest_preemption_threshold =     working_event_array[i].tml_event_thread_preemption_threshold;
					}
                }
                else
                {
                
                    /* Not the first event...  */
                    
                    /* Do we have a new lowest preemption-threshold?  */
                    if (working_event_array[i].tml_event_thread_preemption_threshold > tml_object_array[thread_index].tml_object_lowest_preemption_threshold)
                    {
                    
                        /* Yes, setup the new lowest preemption-threshold.  */
                        tml_object_array[thread_index].tml_object_lowest_preemption_threshold  =  working_event_array[i].tml_event_thread_preemption_threshold;
                    }
                    
                    /* Do we have a new highest preemption-threshold?  */
                    if (working_event_array[i].tml_event_thread_preemption_threshold < tml_object_array[thread_index].tml_object_highest_preemption_threshold)
                    {
                    
                        /* Yes, setup the new highest preemption-threshold.  */
                        tml_object_array[thread_index].tml_object_highest_preemption_threshold =  working_event_array[i].tml_event_thread_preemption_threshold;
                    }
                }
            }
            else
            {

                /* Create a thread entry, since one wasn't found in the registry.  */

                /* Setup maximums.  */
                threads =  tml_total_threads;
                objects =  actual_objects;

                /* First, determine if we need to allocate more memory for the object and thread list arrays.  */
                if (actual_objects >= tml_max_objects)
                {

                    /* We need to allocate more space.  */

                    /* Setup the new maximum.  */
                    tml_max_objects++;
				
				/* Allocate space in the thread list and the object list for the newly found thread.  */
                    if((tml_max_objects > ULONG_MAX - 1) ||
                        (tml_max_objects + 1 > SIZE_MAX / sizeof(TML_OBJECT)))
                    {

                        /* Free allocated resources.  */
                        free(working_event_array);
                        if (converted_file)
                            fclose(converted_file);

            			/* Allocate memory calculation error.  */
                        *error_string = tml_memory_calculation_error;
                        return(__LINE__);
                    }
                    new_object_array =  (TML_OBJECT *) malloc((tml_max_objects+1)*sizeof(TML_OBJECT));
    
                /* Check for an error condition.  */
                if (!new_object_array)
                {

                    /* Determine if the converted file needs to be closed.  */
                    if (converted_file)
                        fclose(converted_file);

                    /* System error.  */
                    *error_string =  tml_object_allocation_error;
                    return(10);
                }

                    /* Allocate size for thread only index array.
                     * There is no overflow here since above tests for "(tml_max_objects+1)*sizeof(TML_OBJECT)" passed.
                     */
                new_thread_list =  (unsigned long *) malloc((tml_max_objects+1)*sizeof(unsigned long));

				/* Check for an error condition.  */
				if (!new_thread_list)
				{

                    /* Determine if the converted file needs to be closed.  */
                    if (converted_file)
                        fclose(converted_file);

					/* System error.  */
					*error_string =  tml_thread_allocation_error;
					return(11);
				}

				/* At this point we need to copy the object and thread lists into the new list.  */
                    for (j = 0; j < actual_objects; j++)
				{

					/* Copy the object into the new, larger object array.  */
					new_object_array[j] =  tml_object_array[j];					
				}

				for (j = 0; j < threads; j++)
				{

					/* Copy the thread into the new, larger thread list.  */
					new_thread_list[j] =  tml_object_thread_list[j];
				}

                    /* At this point, free the old object array and thread list.  */
				free(tml_object_array);
				free(tml_object_thread_list);

                    /* Setup new object and thread lists.  */
                    tml_object_array =  new_object_array;
                    tml_object_thread_list = new_thread_list;
                }
				
                /* Increment the number of threads and objects.  */
                threads++;
                objects++;
                actual_objects++;

				/* Remember the maximum objects.  */
                tml_total_objects =  objects;

				/* Remember the maximum threads.  */
                tml_total_threads =  threads;

				/* Setup the new thread index in the thread list.  */
                tml_object_thread_list[threads-1] =  objects-1;

				/* Setup the new thread object.  */
                tml_object_array[objects-1].tml_object_address =        working_event_array[i].tml_event_context;
                tml_object_array[objects-1].tml_object_name[0] =        'U';
                tml_object_array[objects-1].tml_object_name[1] =        'n';
                tml_object_array[objects-1].tml_object_name[2] =        '-';
                tml_object_array[objects-1].tml_object_name[3] =        'n';
                tml_object_array[objects-1].tml_object_name[4] =        'a';
                tml_object_array[objects-1].tml_object_name[5] =        'm';
                tml_object_array[objects-1].tml_object_name[6] =        'e';
                tml_object_array[objects-1].tml_object_name[7] =        'd';
                tml_object_array[objects-1].tml_object_name[8] =        ' ';
                tml_object_array[objects-1].tml_object_name[9] =        'T';
                tml_object_array[objects-1].tml_object_name[10] =       'h';
                tml_object_array[objects-1].tml_object_name[11] =       'r';
                tml_object_array[objects-1].tml_object_name[12] =       'e';
                tml_object_array[objects-1].tml_object_name[13] =       'a';
                tml_object_array[objects-1].tml_object_name[14] =       'd';
                tml_object_array[objects-1].tml_object_name[15] =       0;
                tml_object_array[objects-1].tml_object_parameter_1 =    0;
                tml_object_array[objects-1].tml_object_parameter_2 =    0;
                tml_object_array[objects-1].tml_object_type =           TML_TRACE_OBJECT_TYPE_THREAD;

                /* First event for this thread, simply mark the highest and lowest priority to the same value.  */
                tml_object_array[objects-1].tml_object_highest_priority =    working_event_array[i].tml_event_thread_priority;
                tml_object_array[objects-1].tml_object_lowest_priority =     working_event_array[i].tml_event_thread_priority;
				
				/* And finally, setup the thread index for the event. */
				working_event_array[i].tml_event_thread_index =  threads-1;
			}
		}
	}

    /* Make another pass through the events to look for cases where we need to insert a fake event in order to 
	   properly show event execution.  */
	new_events =  0;

    for (i = 0; i < actual_events; i++)
    {

        /* Determine if a fake event needs to be created.  */
        if (((i + 1) < actual_events) && (working_event_array[i].tml_event_context != working_event_array[i].tml_event_next_context) &&
										 (working_event_array[i].tml_event_next_context != working_event_array[i+1].tml_event_context))
		{
			
			/* Increment the new events counter.  */
			new_events++;
		}

        /* Yes, current event is a thread. Let's try to find the context for the thread.  */
        status =  tml_object_by_address_find(working_event_array[i].tml_event_context, &thread_index);

        /* Determine if it was successful.  */
        if ((status == 0) && (tml_object_array[thread_index].tml_object_type == TML_TRACE_OBJECT_TYPE_THREAD))
        {

            /* Increment the number of events for this object (thread).  */
            tml_object_array[thread_index].tml_object_total_events++;
        }
	}

	/* Now determine if we have to allocate new events.  */
	if (new_events)
	{

		/* Allocate memory for a new array of events.  */
		if (((unsigned long long)actual_events + new_events > (unsigned long long)ULONG_MAX) ||
			(actual_events + new_events > SIZE_MAX / sizeof(TML_EVENT)))
		{

			/* Free allocated resources.  */
			free(working_event_array);
			if (converted_file)
				fclose(converted_file);

			/* Allocate memory calculation error.  */
			*error_string = tml_memory_calculation_error;
			return(__LINE__);
		}
		new_event_array =  (TML_EVENT *) malloc((actual_events+new_events) * sizeof(TML_EVENT));

        /* Check for an error condition.  */
        if (!new_event_array)
        {

            /* Determine if the converted file needs to be closed.  */
            if (converted_file)
                fclose(converted_file);

            /* System error.  */
            *error_string =  tml_event_allocation_error;
            return(91);
        }

        /* Setup index "j" as an index into the new array.  */
        j =  0;

        /* Make a final pass (hopefully!) through the events to look for cases where we need to insert a fake event in order to 
           properly show event execution.  */
        for (i = 0; i < actual_events; i++)
        {

            /* Copy the current event into the new array.  */
            new_event_array[j] =  working_event_array[i];
            
            /* Determine if a fake event needs to be created.  */
            if (((i + 1) < actual_events) && (working_event_array[i].tml_event_context != working_event_array[i].tml_event_next_context) &&
                                         (working_event_array[i].tml_event_next_context != working_event_array[i+1].tml_event_context))
            {
            
                /* We need to insert a fake "Running" event for Sequential mode to display properly.  */

                /* Move to next event in the new array.  */
                j++;

                /* Now create a new event.  */
                new_event_array[j].tml_event_id =                       TML_TRACE_RUNNING;
                new_event_array[j].tml_event_context =                  working_event_array[i].tml_event_next_context;
                new_event_array[j].tml_event_next_context =             working_event_array[i].tml_event_next_context;
                new_event_array[j].tml_event_relative_ticks =           working_event_array[i].tml_event_relative_ticks;
                new_event_array[j].tml_event_thread_index =             0;
                new_event_array[j].tml_event_thread_priority =          0;
                new_event_array[j].tml_event_thread_preemption_threshold =  0;
                new_event_array[j].tml_event_time_stamp =               working_event_array[i].tml_event_time_stamp;
                new_event_array[j].tml_event_info_1 =                   0;
                new_event_array[j].tml_event_info_2 =                   0;
                new_event_array[j].tml_event_info_3 =                   0;
                new_event_array[j].tml_event_info_4 =                   0;
                new_event_array[j].tml_event_priority_inversion =       0;
                new_event_array[j].tml_event_bad_priority_inversion =   0;

                /* Calculate the difference in relative time.   */
                delta_ticks =  working_event_array[i+1].tml_event_relative_ticks -  working_event_array[i].tml_event_relative_ticks;

                /* See if we can update the relative time so that the fake event does not overlap.  */
                if (((unsigned long) delta_ticks) > 12)
                {
                
                    /* Update the relative time.  */
                    new_event_array[j].tml_event_relative_ticks =       working_event_array[i].tml_event_relative_ticks + 12;

                    /* Update the fake time stamp.  */
                    if (tick_increases > tick_decreases)
                    {

                        new_event_array[j].tml_event_time_stamp =       working_event_array[i].tml_event_time_stamp + 12;
                    }
                    else
                    {

                        new_event_array[j].tml_event_time_stamp =       working_event_array[i].tml_event_time_stamp - 12;
                    }
                }

				/* Determine if the current event is a thread.  */
				if ((new_event_array[j].tml_event_context != 0xFFFFFFFF) && (new_event_array[j].tml_event_context != 0xF0F0F0F0) &&
					(new_event_array[j].tml_event_context != 0))
				{

					/* Yes, current event is a thread. Let's try to find the context for the thread.  */
					status =  tml_object_by_address_find(new_event_array[j].tml_event_context, &thread_index);

					/* Determine if it was successful.  */
					if ((status == 0) && (tml_object_array[thread_index].tml_object_type == TML_TRACE_OBJECT_TYPE_THREAD))
					{

						/* Find the thread index for this event.  */
						for (k = 0; k < tml_total_threads; k++)
						{

							if (tml_object_thread_list[k] == thread_index)
							{

								/* Use the index into the thread list.  */
								break;
							}
						}

						/* Store the thread index in the event.  */
                        new_event_array[j].tml_event_thread_index =  k;

                        /* Initialize priority.  */
                        priority =  0xFFFFFFFF;

                        /* Initialize preemption-threshold.  */
                        preemption_threshold =  0xFFFFFFFF;

					/* Search backwards for the priority of the thread.  */
					if (i > 0)
					{

						/* Start looking backward in the event log.  */
						k = i - 1;
						do
						{

							/* Have we found the event with this same event context and is it NOT Running?  */
							if ((working_event_array[k].tml_event_context == new_event_array[j].tml_event_context) &&
								(working_event_array[k].tml_event_id != TML_TRACE_RUNNING))
							{

                                    /* Pickup the priority.  */
                                    priority =  working_event_array[k].tml_event_thread_priority;

                                    /* Pickup the preemption-threshold.  */
                                    preemption_threshold =  working_event_array[k].tml_event_thread_preemption_threshold;

                                    /* Pickup the priority and get out of the loop.  */
                                    new_event_array[j].tml_event_thread_priority =  priority;
                                    new_event_array[j].tml_event_thread_preemption_threshold =  preemption_threshold;
                                    break;
                                }

                                /* Look for a thread create event for this thread and scoop up the priority.  */
                                if ((working_event_array[k].tml_event_info_1 == new_event_array[j].tml_event_context) &&
                                    (working_event_array[k].tml_event_id == TML_TRACE_THREAD_CREATE))
                                {
                                
                                    /* Pickup the priority - info 2 field has priority.  */
                                    priority =  working_event_array[k].tml_event_info_2;

								/* Pickup the priority and get out of the loop.  */
                                    new_event_array[j].tml_event_thread_priority =  priority;
								break;
							}

						} while (k--);
					}

                        /* Determine if the priority was found.  */
                        if (priority == 0xFFFFFFFF)
                        {
                        
                            /* No, we should look ahead in the buffer at this point.  */
                            k =  i;
                            do
                            {
                            
                                /* Have we found the event with this same event context and is it NOT Running?  */
                                if ((working_event_array[k].tml_event_context == new_event_array[j].tml_event_context) &&
                                    (working_event_array[k].tml_event_id != TML_TRACE_RUNNING))
                                {

                                    /* Pickup the priority.  */
                                    priority =  working_event_array[k].tml_event_thread_priority;

                                    /* Pickup the preemption-threshold.  */
                                    preemption_threshold =  working_event_array[k].tml_event_thread_preemption_threshold;

                                    /* Pickup the priority and get out of the loop.  */
                                    new_event_array[j].tml_event_thread_priority =  priority;
                                    new_event_array[j].tml_event_thread_preemption_threshold =  preemption_threshold;
                                    break;
                                }

                                /* Look for a thread create event for this thread and scoop up the priority.  */
                                if ((working_event_array[k].tml_event_info_1 == new_event_array[j].tml_event_context) &&
                                    (working_event_array[k].tml_event_id == TML_TRACE_THREAD_CREATE))
                                {
                                
                                    /* Pickup the priority - info 2 field has priority.  */
                                    priority =  working_event_array[k].tml_event_info_2;

                                    /* Pickup the priority and get out of the loop.  */
                                    new_event_array[j].tml_event_thread_priority =  priority;
                                    break;
                                }
                            
                                /* Move to next entry.  */
                                k++;
                            
                            } while (k < actual_events);
                        }

                        /* Did we find a priority???  */
                        if (priority != 0xFFFFFFFF)
                        {

                            /* Determine if the priorities for this thread have been setup.  */
                            if (tml_object_array[thread_index].tml_object_lowest_priority == 0xFFFFFFFF)
                            {
                
                                /* First event for this thread, simply mark the highest and lowest priority to the same value.  */
                                tml_object_array[thread_index].tml_object_highest_priority =    priority;
                                tml_object_array[thread_index].tml_object_lowest_priority =     priority;
                            }
                            else
                            {
                
                                /* Not the first event...  */
                    
                                /* Do we have a new lowest priority?  */
                                if (priority > tml_object_array[thread_index].tml_object_lowest_priority)
                                {
                    
                                    /* Yes, setup the new lowest priority.  */
                                    tml_object_array[thread_index].tml_object_lowest_priority =  priority;
                                }
                    
                                /* Do we have a new highest priority?  */
                                if (priority < tml_object_array[thread_index].tml_object_highest_priority)
                                {
                    
                                    /* Yes, setup the new highest priority.  */
                                    tml_object_array[thread_index].tml_object_highest_priority =  priority;
                                }
                            }
                        }

                        /* Did we find a preemption-threshold???  */
                        if (preemption_threshold != 0xFFFFFFFF)
                        {

                            /* Determine if the preemption-threshold for this thread have been setup.  */
                            if (tml_object_array[thread_index].tml_object_lowest_preemption_threshold == 0xFFFFFFFF)
                            {
                
                                /* First event for this thread, simply mark the highest and lowest preemption-threshold to the same value.  */
                                tml_object_array[thread_index].tml_object_highest_preemption_threshold =    preemption_threshold;
                                tml_object_array[thread_index].tml_object_lowest_preemption_threshold =     preemption_threshold;
                            }
                            else
                            {
                
                                /* Not the first event...  */
                    
                                /* Do we have a new lowest preemption-threshold?  */
                                if (preemption_threshold > tml_object_array[thread_index].tml_object_lowest_preemption_threshold)
                                {
                    
                                    /* Yes, setup the new lowest preemption-threshold.  */
                                    tml_object_array[thread_index].tml_object_lowest_preemption_threshold =  preemption_threshold;
                                }
                    
                                /* Do we have a new highest preemption-threshold?  */
                                if (preemption_threshold < tml_object_array[thread_index].tml_object_highest_preemption_threshold)
                                {
                    
                                    /* Yes, setup the new highest preemption-threshold.  */
                                    tml_object_array[thread_index].tml_object_highest_preemption_threshold =  preemption_threshold;
                                }
                            }
                        }
                    }
                }
            }

            /* Now move the index into the new array.  */
            j++;
        }
        
        /* At this point, we need increment the total events.  */
        actual_events =  actual_events + new_events;

        /* Release the old event array.  */
        free(working_event_array);

        /* Now switch the pointer to the new event array.  */
        working_event_array =  new_event_array;
    }
    
    /* Allocate space for the thread status list.  */
    if (tml_total_threads > SIZE_MAX / sizeof(TML_THREAD_STATUS_SUMMARY))
	{

		/* Free allocated resources.  */
		free(working_event_array);
		if (converted_file)
			fclose(converted_file);

		/* Allocate memory calculation error.  */
		*error_string = tml_memory_calculation_error;
		return(__LINE__);
	}
    tml_thread_status_list =  (TML_THREAD_STATUS_SUMMARY *) malloc(sizeof(TML_THREAD_STATUS_SUMMARY)*tml_total_threads);
    
    /* Determine if the array was allocated.  */
    if (tml_thread_status_list)
    {
    
    unsigned long   *current_status_list;
    unsigned long   *new_status_list;
    unsigned long   thread_index;
    unsigned long   status_index;
   

        /* Now, loop through the thread status list and initialize it.  */
    for (i = 0; i < tml_total_threads; i++)
    {

            /* Initialize this entry.   */
            tml_thread_status_list[i].tml_thread_status_summary_status_changes =  1;
            tml_thread_status_list[i].tml_thread_status_summary_list =            (unsigned long *)malloc(sizeof(unsigned long)*4);
            tml_thread_status_list[i].tml_thread_status_summary_address =         tml_object_array[tml_object_thread_list[i]].tml_object_address;  
            
            /* Determine if there is a good status list.  */
            current_status_list =  tml_thread_status_list[i].tml_thread_status_summary_list;
            if (current_status_list)
        {
                /* Indicate that the status list is unknown.  */
                current_status_list[0] =  0;
                current_status_list[1] =  TML_THREAD_STATUS_UNKNOWN;
                current_status_list[2] =  0xFFFFFFFF;
            }
        }

        /* Now make another pass through the event list to keep track of the suspension/ready status of each thread.  */
        for (i = 0; i < actual_events; i++)
        {

            /* Determine if there is an internal thread suspend or resume event.  */
            if ((working_event_array[i].tml_event_id != TML_TRACE_THREAD_RESUME) && (working_event_array[i].tml_event_id != TML_TRACE_THREAD_SUSPEND))
            {
            
                   /* Determine if the thread index in the event structure is valid.  */
               thread_index =  working_event_array[i].tml_event_thread_index;

               //if (thread_index < tml_total_threads)
			   if ((working_event_array[i].tml_event_context > 0) && (working_event_array[i].tml_event_context < 0xF0F0F0F0) && (thread_index < tml_total_threads))
               {

                   /* Check for the thread being in an unknown state.  */

                   /* Pickup the current status list.  */
                  current_status_list =  tml_thread_status_list[thread_index].tml_thread_status_summary_list;

                   /* Is there a current list?  */
                   if (current_status_list)
                   {

                        /* Yes, now let's look at the first status.  If it is unknown, we need to update it to ready since the thread is obviously running.  */
                        if (current_status_list[1] == TML_THREAD_STATUS_UNKNOWN)
                        {

                            /* Update the status to ready, since an event is present for this thread.  */
                            current_status_list[1] =  TML_THREAD_STATUS_READY;
                        }
                   }

               }

               /* Nothing to look at here... just move to the next event.  */
               continue;
            }
            
            /* Now let's calculate the thread index.  */
            thread_index =  0;
            while (thread_index < tml_total_threads)
            {
                    
                /* Is this the thread?  */
                if (working_event_array[i].tml_event_info_1 == tml_thread_status_list[thread_index].tml_thread_status_summary_address)
                {
                        
                    /* Yes, found the thread, get out of the loop!  */
                    break;
                }
                       
                /* Otherwise, move to the next thread.  */
                thread_index++;
            }

            /* Check the thread index...   */
            if (thread_index >= tml_total_threads)
            {
            
                /* Simply continue to next event... couldn't find the thread index.  */
                continue;
            }

            /* At this point, we need to add an event.  */
            
            /* Pickup the current status list.  */
            current_status_list =  tml_thread_status_list[thread_index].tml_thread_status_summary_list;

            /* Is there a current list?  */
            if (current_status_list == NULL)
                continue;
            
            /* Pickup the current status index.  */
            status_index =  tml_thread_status_list[thread_index].tml_thread_status_summary_status_changes;

            /* Allocate a new list.  */
            if(status_index > ULONG_MAX / 2 ||
				(status_index * 2 > ULONG_MAX - 4) ||
				((status_index * 2 + 4) > SIZE_MAX / sizeof(unsigned long)))
			{

				/* Free allocated resources.  */
				free(working_event_array);
				if (converted_file)
					fclose(converted_file);

				/* Allocate memory calculation error.  */
				*error_string = tml_memory_calculation_error;
				return(__LINE__);
			}
            new_status_list =  (unsigned long *)malloc(sizeof(unsigned long)*((status_index*2)+4));

            /* Was a new list allocated?  */
            if (new_status_list == NULL)
                continue;

            /* Copy the old list into the new list.  */
            for (j = 0; j < (status_index*2); j++)
            {
            
                /* Copy one word of status.  */
                new_status_list[j] =  current_status_list[j];
            }           

            /* Release the original list.  */
            free(current_status_list);

            /* At this point, we need to setup the last entry and add the new entry.  */
            j =  (status_index-1)*2;

            /* Is the current status unknown?  */
            if (new_status_list[j+1] == TML_THREAD_STATUS_UNKNOWN)
            {
            
                /* Yes, fixup the status based on the event.   */
                if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_RESUME)
                    new_status_list[j+1] =  working_event_array[i].tml_event_info_2;
                else
                    new_status_list[j+1] =  TML_THREAD_STATUS_READY;
            }

            /* At this point, we need to add the new event.  */
            new_status_list[j+2] =  i;
            if (working_event_array[i].tml_event_id == TML_TRACE_THREAD_RESUME)
                new_status_list[j+3] =  TML_THREAD_STATUS_READY;
            else
                new_status_list[j+3] =  working_event_array[i].tml_event_info_2;
            new_status_list[j+4] =      0xFFFFFFFF;

            /* Replace the current list pointer and increment the status count.  */
            tml_thread_status_list[thread_index].tml_thread_status_summary_list =  new_status_list;
            tml_thread_status_list[thread_index].tml_thread_status_summary_status_changes++;
        } 
    }
	
	/* Setup globals for subsequent use.  */
	tml_event_array =		working_event_array;
	tml_total_events =		actual_events;
	tml_relative_ticks =	relative_ticks;

	/* Setup return values.  */
	*total_threads =	   tml_total_threads;
	*total_objects =       tml_total_objects;
	*total_events =		   actual_events;

	/* Safety check...  make sure that relative ticks has 1 in it!  */
	if (relative_ticks == 0)
		relative_ticks =  1;
	*max_relative_ticks =  relative_ticks;

    /* Calculate priority inversions.  */
	tml_calculate_priority_inversions(tml_total_threads, tml_total_objects, tml_total_events,
									&tml_total_priority_inversions, &tml_total_bad_priority_inversions);


    /* Determine if the converted file needs to be closed.  */
    if (converted_file)
        fclose(converted_file);

	/* At this point, we are finished processing the trace buffer and are ready for
	   requests from the application for access to it.  */

	return(0);
}


int  tml_header_info_get(unsigned long  *header_trace_id, 
							unsigned long  *header_timer_valid_mask,
							unsigned long  *header_trace_base_address,
							unsigned long  *header_object_registry_start_address,
							unsigned short *header_reserved1,
							unsigned short *header_object_name_size,
							unsigned long  *header_object_registry_end_address,
							unsigned long  *header_trace_buffer_start_address,
							unsigned long  *header_trace_buffer_end_address,
							unsigned long  *header_trace_buffer_current_address,
							unsigned long  *header_reserved2,
							unsigned long  *header_reserved3,
							unsigned long  *header_reserved4)
{

	if (header_trace_id)
		*header_trace_id =  tml_header_trace_id;

	if (header_timer_valid_mask)
		*header_timer_valid_mask =  tml_header_timer_valid_mask;

	if (header_trace_base_address)
		*header_trace_base_address =  tml_header_trace_base_address;

	if (header_object_registry_start_address)
		*header_object_registry_start_address =  tml_header_object_registry_start_address;

	if (header_reserved1)
		*header_reserved1 =  tml_header_reserved1;

	if (header_object_name_size)
		*header_object_name_size =  tml_header_object_name_size;

	if (header_object_registry_end_address)
		*header_object_registry_end_address =  tml_header_object_registry_end_address;

	if (header_trace_buffer_start_address)
		*header_trace_buffer_start_address =  tml_header_trace_buffer_start_address;

	if (header_trace_buffer_end_address)
		*header_trace_buffer_end_address =  tml_header_trace_buffer_end_address;

	if (header_trace_buffer_current_address)
		*header_trace_buffer_current_address =  tml_header_trace_buffer_current_address;

	if (header_reserved2)
		*header_reserved2 =  tml_header_reserved2;

	if (header_reserved3)
		*header_reserved3 =  tml_header_reserved3;

	if (header_reserved4)
		*header_reserved4 =  tml_header_reserved4;

	return(0);
}


int  tml_object_thread_get(unsigned long thread_index, char **object_name, unsigned long *object_address,
                                        unsigned long *parameter_1, unsigned long *parameter_2, 
                                        unsigned long *lowest_priority, unsigned long *highest_priority)
{

unsigned long   object_index;

	/* Determine if the thread index is valid.  */
	if (thread_index >= tml_total_threads)
	{

		/* Exceeded total threads... error!  */
		return(1);
	}

	/* Pickup the object index.  */
	object_index =  tml_object_thread_list[thread_index];

	if (object_name)
		*object_name =  tml_object_array[object_index].tml_object_name;

	if (object_address)
		*object_address = tml_object_array[object_index].tml_object_address;
	
	if (parameter_1)
		*parameter_1 =  tml_object_array[object_index].tml_object_parameter_1;

	if (parameter_2)
		*parameter_2 =  tml_object_array[object_index].tml_object_parameter_2;
	
    if (lowest_priority)
        *lowest_priority =  tml_object_array[object_index].tml_object_lowest_priority;
        
    if (highest_priority)
        *highest_priority =  tml_object_array[object_index].tml_object_highest_priority;
    
    return(0);
}

int  tml_object_thread_preemption_threshold_get(unsigned long thread_index, unsigned long *lowest_preemption_threshold, unsigned long *highest_preemption_threshold)
{

unsigned long   object_index;


    /* Determine if the thread index is valid.  */
    if (thread_index >= tml_total_threads)
    {

        /* Exceeded total threads... error!  */
        return(1);
    }

    /* Pickup the object index.  */
    object_index =  tml_object_thread_list[thread_index];

    if (lowest_preemption_threshold)
        *lowest_preemption_threshold =  tml_object_array[object_index].tml_object_lowest_preemption_threshold;
        
    if (highest_preemption_threshold)
        *highest_preemption_threshold =  tml_object_array[object_index].tml_object_highest_preemption_threshold;
    
    return(0);
}


int  tml_object_get(unsigned long object_index, char **object_name, unsigned long *object_address,
										unsigned long *parameter_1, unsigned long *parameter_2)
{

	/* Determine if the object index is valid.  */
	if (object_index >= tml_total_objects)
	{

		/* Exceeded total objects... error!  */
		return(1);
	}

	if (object_name)
		*object_name =  tml_object_array[object_index].tml_object_name;

	if (object_address)
		*object_address = tml_object_array[object_index].tml_object_address;
	
	if (parameter_1)
		*parameter_1 =  tml_object_array[object_index].tml_object_parameter_1;

	if (parameter_2)
		*parameter_2 =  tml_object_array[object_index].tml_object_parameter_2;
	
	return(0);
}


int  tml_object_by_address_find(unsigned long object_address, unsigned long *object_index)
{


unsigned long	i;

	
    /* Set object index to 0.  */
    *object_index =  0;
    
	/* Find object.  */
	for (i = 0; i < tml_total_objects; i++)
	{

		/* See if the address matches the object index.  */
		if (tml_object_array[i].tml_object_address == object_address)
		{

			/* Return the object index and success.  */
			*object_index =  i;
			return(0);
		}
	}

	/* If we get here the address was not found.  */
	return(1);
}



int  tml_event_get(unsigned long event_index, unsigned long *event_context, unsigned long *event_id,
								unsigned long *event_thread_priority, unsigned long *event_time_stamp,
								unsigned long *event_info_1, unsigned long *event_info_2, 
								unsigned long *event_info_3, unsigned long *event_info_4,
								_int64 *event_relative_ticks, 
								unsigned long *next_context, unsigned long *thread_index, 
								unsigned long *priority_inversion_event, unsigned long *bad_priority_inversion_event)
{

	/* Determine if the event index is valid.  */
	if (event_index >= tml_total_events)
	{

		/* Exceeded total events... error!  */
		return(1);
	}

	if (event_context)
		*event_context =  tml_event_array[event_index].tml_event_context;

	if (event_id)
		*event_id =  tml_event_array[event_index].tml_event_id;

	if (event_thread_priority)
    {

        if (tml_event_array[event_index].tml_event_context == 0xFFFFFFFF)
            *event_thread_priority =  tml_event_array[event_index].tml_event_thread_priority;
        else
		*event_thread_priority =  tml_event_array[event_index].tml_event_thread_priority  & 0x7FF;
    }

	if (event_time_stamp)
		*event_time_stamp =  tml_event_array[event_index].tml_event_time_stamp;

	if (event_info_1)
		*event_info_1 =  tml_event_array[event_index].tml_event_info_1;

	if (event_info_2)
		*event_info_2 =  tml_event_array[event_index].tml_event_info_2;

	if (event_info_3)
		*event_info_3 =  tml_event_array[event_index].tml_event_info_3;

	if (event_info_4)
		*event_info_4 =  tml_event_array[event_index].tml_event_info_4;

	if (event_relative_ticks)
		*event_relative_ticks =  tml_event_array[event_index].tml_event_relative_ticks;

	if (next_context)
		*next_context =  tml_event_array[event_index].tml_event_next_context;

	if (thread_index)
		*thread_index =  tml_event_array[event_index].tml_event_thread_index;

	if (priority_inversion_event)
		*priority_inversion_event =  tml_event_array[event_index].tml_event_priority_inversion;

	if (bad_priority_inversion_event)
		*bad_priority_inversion_event =  tml_event_array[event_index].tml_event_bad_priority_inversion;

	return(0);
}


int  tml_event_preemption_threshold_get(unsigned long event_index, unsigned long *preemption_threshold)
{

    /* Determine if the event index is valid.  */
    if (event_index >= tml_total_events)
    {

        /* Exceeded total events... error!  */
        return(1);
    }

    if (preemption_threshold)
        *preemption_threshold =  tml_event_array[event_index].tml_event_thread_preemption_threshold;

    return(0);
}

int  tml_event_by_relative_ticks_find(_int64 relative_ticks_start, _int64 relative_ticks_end, 
																		unsigned long *event_index)
{

unsigned long   i;
unsigned long   start, previous_start;
unsigned long   end, previous_end;


    /* Set event index to zero.  */
    *event_index =  0;

    /* Determine if the range is invalid. */
    if ((relative_ticks_start > relative_ticks_end) ||
        (relative_ticks_end > tml_relative_ticks))
    {

        /* Out of range, return an error.  */
        return(1);
    }

    /* Use binary search to find the event.  */
    start =  0;
    end   =  tml_total_events-1;
    i =      end/2;
    while (1)
    {

        /* Determine if this event is within the range.  */
        if ((tml_event_array[i].tml_event_relative_ticks >= relative_ticks_start) &&
            (tml_event_array[i].tml_event_relative_ticks <= relative_ticks_start))
        {

            /* Find the first event that fits this range.  */
            while (i)
            {

                /* Is the previous event within the range?  */
                if ((tml_event_array[i-1].tml_event_relative_ticks >= relative_ticks_start) &&
                    (tml_event_array[i-1].tml_event_relative_ticks <= relative_ticks_start))
                {
                
                    /* Yes, move back one event.  */
                    i--;
                }
                else
                {

                    break;
                }
                
            }

            /* We are all done, return the index and success!  */
            *event_index =  i;
            return(0);
        }
        else
        {


            /* Determine if the search has been exhausted.  */
            if (start == end)
            {

                /* Return error.  */
                *event_index =  tml_total_events;
                return(1);
            }

            /* Save previous start and end.  */
            previous_start =  start;
            previous_end =    end;

            /* Not found. We need to figure out which way to search.  */
            if (tml_event_array[i].tml_event_relative_ticks < relative_ticks_start)
            {
                /* Upper half of search. Move start index.  */
                start =  i;
            }
            else
            {
                /* Lower half of search. Move end index.  */
                end =   i;
            }

            /* Determine if the start and the end are the same.. this should never be.  */
            if ((previous_start == start) && (previous_end == end))
            {

                /* Need to manually adjust the start.  */
                if (i == start)
                    start++;
                else
                    end--;
            }

            /* Compute new value of i.  */
            i =  start + ((end-start)/2);
        }
    }
}



int   tml_calculate_priority_inversions(unsigned long total_threads, unsigned long total_objects, unsigned long total_events,
                                    unsigned long *priority_inversions, unsigned long *bad_priority_inversions)
{

unsigned long           i, j, k;
unsigned long			thread_index;
unsigned long			inversion_start_index;
unsigned long			mutex_index;
unsigned long			*thread_array;
TML_INVERSION_DETECTION	*inversion_array;
unsigned long			event_context;
unsigned long			event_id;
unsigned long			event_thread_priority;
unsigned long			event_time_stamp;
unsigned long			event_info_1;
unsigned long			event_info_2;
unsigned long			event_info_3;
unsigned long			event_info_4;
_int64					event_relative_ticks;
unsigned long           search_context;
unsigned long           search_id;
unsigned long           search_thread_priority;
unsigned long           search_time_stamp;
unsigned long           search_info_1;
unsigned long           search_info_2;
unsigned long           search_info_3;
unsigned long           search_info_4;
_int64                  search_relative_ticks;
unsigned long			inversions = 0;
unsigned long			bad_inversions = 0;


	/* Allocate memory for all the arrays.  */
	if(total_objects > SIZE_MAX / sizeof(unsigned long))
		return(-1);
	if(total_objects > SIZE_MAX / sizeof(TML_INVERSION_DETECTION))
		return(-1);
	thread_array =  (unsigned long *) malloc(sizeof(unsigned long)*total_objects);
	inversion_array =  (TML_INVERSION_DETECTION *) malloc(sizeof(TML_INVERSION_DETECTION)*total_objects);

	/* Determine if there are any errors.  */
	if ((thread_array == 0) || (inversion_array == 0))
		return(1);

	/* Build the thread array... set to an invalid priority for now.  */
	for (i = 0; i < total_objects; i++)
	{

		thread_array[i] =  0xFFFF;
	}

	/* Loop through the events and get the first priority of the threads in the event trace.  */
	for (i = 0; i < total_events; i++)
	{

		/* Get the current event.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);


		/* Save off the priority.  */
		if (event_context >= 0xF0F0F0F0)
			continue;

		/* Pickup the thread index.  */
		tml_object_by_address_find(event_context, &thread_index);

		/* Has the thread priority been setup yet?  */
		if (thread_array[thread_index] == 0xFFFF)
		{

			/* No, set it up now!  */
			thread_array[thread_index] =  event_thread_priority;
		}
	}

	/* Clear the inversion detection array.  */
	for (i = 0; i < total_objects; i++)
	{

		inversion_array[i].tml_blocked_thread_address =  0;
	}

	/* Now loop through all the events in the trace... looking for priority inversions and then 
	   undeterministic priority inversions.  */
	for (i = 0; i < total_events; i++)
	{

		/* Get the current event.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);

		/* Save off the priority.  */
		if (event_context >= 0xF0F0F0F0)
			continue;

		/* Pickup the thread index.  */
		tml_object_by_address_find(event_context, &thread_index);

		/* Setup the priority in the thread array.  */
		thread_array[thread_index] =  event_thread_priority;

		/* Pickup the object index for this mutex.   */
		tml_object_by_address_find(event_info_1, &mutex_index);

		/* Determine if the current event is a mutex get, there is suspension, and 
		   the start of a priority inversion window is present.  */
		if ((event_id == TML_TRACE_MUTEX_GET) && (event_info_3) &&
			(event_info_3 != event_context))
		{


			/* Pickup the other thread index.  */
			tml_object_by_address_find(event_info_3, &j);

			/* Check for start of a priority inversion window.  */
			if ((thread_array[j] != 0xFFFF) &&
				(event_thread_priority < thread_array[j]) &&
				(inversion_array[mutex_index].tml_blocked_thread_address == 0))
			{

				/* Start of a priority inversion window.  */
				inversions++;

				/* Save off the information.  */
				inversion_array[mutex_index].tml_blocked_thread_address =   event_context;
				inversion_array[mutex_index].tml_blocked_thread_priority =  event_thread_priority;
				inversion_array[mutex_index].tml_owning_thread_address =    event_info_3;
				inversion_array[mutex_index].tml_owning_thread_priority =   thread_array[j];

				/* Save the event index.  */
				inversion_start_index =  i;
				continue;
			}
		}

		/* Determine if the end of a priority inversion window is present.  */
		if ((event_id == TML_TRACE_MUTEX_PUT) &&
			(inversion_array[mutex_index].tml_blocked_thread_address) &&
			(event_info_3 == 1))
		{

            /* Loop to find the first event with the blocked thread running.  */
            k =  i;
            do
            { 

                /* Get next event.  */
                tml_event_get(k, &search_context, &search_id,
                                 &search_thread_priority, &search_time_stamp,
                                 &search_info_1, &search_info_2, 
                                 &search_info_3, &search_info_4,
                                 &search_relative_ticks, 0, 0, 0, 0);
                
                /* Determine if this context is the one we are looking for.  */
                if (search_context == inversion_array[mutex_index].tml_blocked_thread_address)
                {
                
                    /* Yes, get out of the search loop.  */
                    break;
                }
                
                /* Look at next entry.  */
                k++;
            } while (k < total_events);

            /* Determine if k is valid.  */
            if (k >= total_events)
                k =  i;
            
			/* Clear the mutex inversion detection entry pointer.  */
			inversion_array[mutex_index].tml_blocked_thread_address =  0;

			/* Set the inversion window for this event.  */
            tml_event_array[inversion_start_index].tml_event_priority_inversion =  k;
			
			continue;
		}

		/* At this point, examine all of the entries in the inversion list to 
		   see if an undeterministic priority inversion is present.  */
		for (j = 0; j < total_objects; j++)
		{

			/* Determine if there is a priority inversion for this mutex.  */
			if (inversion_array[j].tml_blocked_thread_address)
			{

				/* Is it a different thread than the owning thread.  */
				if ((event_context != inversion_array[j].tml_owning_thread_address) &&
					(event_thread_priority > inversion_array[j].tml_blocked_thread_priority))
				{

                    /* Loop to find the first event with the blocked thread running.  */
                    k =  i;
                    do
                    { 

                        /* Get next event.  */
                        tml_event_get(k, &search_context, &search_id,
                                         &search_thread_priority, &search_time_stamp,
                                         &search_info_1, &search_info_2, 
                                         &search_info_3, &search_info_4,
                                         &search_relative_ticks, 0, 0, 0, 0);
                
                        /* Determine if this context is the one we are looking for.  */
                        if (search_context == inversion_array[j].tml_blocked_thread_address)
                        {
                
                            /* Yes, get out of the search loop.  */
                            break;
                        }
                
                        /* Look at next entry.  */
                        k++;
                    } while (k < total_events);

                    /* Determine if k is valid.  */
                    if (k >= total_events)
                        k =  i;

					/* Undeterministic priority inversion is found.  Increment the counter and clear
                       the window, since we only want to find the first occurrence.  */
					bad_inversions++;
					inversion_array[j].tml_blocked_thread_address =  0;

					/* Set the bad inversion window for this event.  */
                    tml_event_array[inversion_start_index].tml_event_bad_priority_inversion  =  k;
				}
			}
		}
	}

	free(thread_array);
	free(inversion_array);

	/* Return the info and success.  */
	*priority_inversions =  inversions;
	*bad_priority_inversions =  bad_inversions;
	return(0);
}


int  tml_system_performance_statistics_get(unsigned long *context_switches, unsigned long *thread_preemptions, unsigned long *time_slices, 
											  unsigned long *thread_suspensions, unsigned long *thread_resumptions, 
											  unsigned long *interrupts, unsigned long *nested_interrupts,
											  unsigned long *deterministic_priority_inversions, unsigned long *undeterministic_priority_inversions)
{

unsigned long	switches;
unsigned long	event_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
_int64			event_relative_ticks;
unsigned long	last_event_context;
unsigned long	last_event_id;
unsigned long	last_event_thread_priority;
unsigned long	last_event_time_stamp;
unsigned long	last_event_info_1;
unsigned long	last_event_info_2;
unsigned long	last_event_info_3;
unsigned long	last_event_info_4;
_int64			last_event_relative_ticks;
unsigned long	current_thread;
unsigned long	preemptions;
unsigned long	suspensions;
unsigned long	resumptions;
unsigned long	interrupt_count;
unsigned long	nested_interrupt_count;
unsigned long	interrupt_in_progress;
unsigned long	slices;
unsigned long   i;
unsigned long	total_events;
unsigned long	last_event;
unsigned long   next_context;

	
	/* Setup the total events.  */
	total_events =  tml_total_events;

	
	/* Calculate the number of context switches.   */
	switches =  0;
	current_thread =    0;

	for (i = 0; i < total_events; i++)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
                                &event_relative_ticks, &next_context, 0, 0, 0);

        /* Determine if we need to set the current thread with the event context... this is a special case at the beginning of the
           trace buffer.  */
        if ((current_thread == 0) && (event_context != 0xFFFFFFFF) && (event_context != 0xF0F0F0F0))
        {
        
            /* Setup current thread as the current event context so we won't miss the first context switch.  */
            current_thread =  event_context;
        }
       
        /* Check for a context switch.  */
        if ((current_thread) && (next_context != 0xFFFFFFFF) && (next_context != 0xF0F0F0F0) && (current_thread != next_context))
			{

				/* Increment the context switches count.  */
				switches++;
			}

        /* Setup current thread.  */
        if ((next_context) && (next_context != 0xFFFFFFFF) && (next_context != 0xF0F0F0F0))
            current_thread =  next_context;
	}

	/* Determine if it needs to be returned.  */
	if (context_switches)
		*context_switches =  switches;

    /* Now look for thread preemptions from thread resume events.  */
    i =  0;
    current_thread =  0;
	preemptions =		0;
    while (i < total_events)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
                                &event_relative_ticks, &next_context, 0, 0, 0);

		/* Determine if there is a context switch.  */
        if (event_id == TML_TRACE_THREAD_RESUME)
		{


            /* Find next context...  */
            last_event =  i+1;
	    next_context =  0;
            while ((last_event < total_events) && ((next_context == 0) || (next_context == 0xFFFFFFFF) || (next_context == 0xF0F0F0F0)))
			{

				/* Get last event that setup the current thread.  */
				tml_event_get(last_event, &last_event_context, &last_event_id,
								&last_event_thread_priority, &last_event_time_stamp,
								&last_event_info_1, &last_event_info_2, 
								&last_event_info_3, &last_event_info_4,
                                &last_event_relative_ticks, &next_context, 0, 0, 0);
                last_event++;
            }

            /* Determine if next context is different than the current context.  */
            if (next_context != current_thread)
				{

                preemptions++;
            }
        }
        else if (event_id == TML_TRACE_TIME_SLICE)
					{

            /* Determine if a time-slice is present.  */
            if (next_context != current_thread)
							{
								preemptions++;
				}
			}

        /* Setup current thread.  */
        if ((next_context) && (next_context != 0xFFFFFFFF) && (next_context != 0xF0F0F0F0))
            current_thread =  next_context;

        i++;
	}

	/* Determine if we need to return the preemptions.  */
	if (thread_preemptions)
		*thread_preemptions =  preemptions;

	/* Calculate the number of time slices.  */
	slices =			0;
	current_thread =    0;
	last_event =		0;

	for (i = 0; i < total_events; i++)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);

        if (event_id == TML_TRACE_TIME_SLICE)
		{

            /* Determine if a time-slice is present.  */
            if (next_context != current_thread)
				{
							slices++;
					}
				}

        /* Setup current thread.  */
        if ((next_context) && (next_context != 0xFFFFFFFF) && (next_context != 0xF0F0F0F0))
            current_thread =  next_context;
	}

	/* Determine if we need time slices.  */
	if (time_slices)
		*time_slices =  slices;


	/* Calculate the total number of thread suspensions and resumptions.  */
	suspensions =  0;
	resumptions =  0;

	for (i = 0; i < total_events; i++)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);

		/* Determine if the event is a suspension. */
		if (event_id == TML_TRACE_THREAD_SUSPEND) 
		{

			/* Increment thread suspensions.  */
			suspensions++;
		}
		else if (event_id == TML_TRACE_THREAD_RESUME)
		{

			/* Increment thread resumptions.  */
			resumptions++;
		}
	}

	/* Determine if we need to return the suspensions or resumptions.  */
	if (thread_suspensions)
		*thread_suspensions =  suspensions;
	if (thread_resumptions)
		*thread_resumptions =  resumptions;

	/* Calculate the total number of interrupts.  */
	interrupt_count =		  0;
	nested_interrupt_count =  0;
	interrupt_in_progress =   0;

	for (i = 0; i < total_events; i++)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);

		/* Determine if the event is ISR enter. */
		if (event_id == TML_TRACE_ISR_ENTER) 
		{

			/* Increment the interrupt count.  */
			interrupt_count++;

			/* Determine if there is a nested condition.  */
			if (interrupt_in_progress)
				nested_interrupt_count++;

			interrupt_in_progress++;
		}
        else if (event_id == TML_TRACE_ISR_EXIT)
		{
            /* Decrement the interrupt in progress count. */
            if (interrupt_in_progress)
                interrupt_in_progress--;

        }
        else if ((event_context == 0xFFFFFFFF) && (interrupt_in_progress == 0))
			{

				/* Yes, this is the start of an interrupt.  */
				interrupt_count++;
		}

        /* Determine if the context is no long an interrupt.  */
        else if (event_context != 0xFFFFFFFF)
		{

			/* Clear the in-progress flag.  */
			interrupt_in_progress =  0;
		}
	}

	/* Determine if this information is necessary.  */
	if (interrupts)
		*interrupts =  interrupt_count;
	if (nested_interrupts)
		*nested_interrupts =  nested_interrupt_count;

	/* Determine if the priority inversion information is required.  */
	if (deterministic_priority_inversions)
		*deterministic_priority_inversions =  tml_total_priority_inversions;
	if (undeterministic_priority_inversions)
		*undeterministic_priority_inversions = tml_total_bad_priority_inversions;

	return(0);
}


int  tml_system_filex_performance_statistics_get(unsigned long *media_opens, unsigned long *media_closes, unsigned long *media_aborts, unsigned long *media_flushes,
                                              unsigned long *directory_reads, unsigned long *directory_writes, unsigned long *directory_cache_misses,
                                              unsigned long *file_opens, unsigned long *file_closes, 
                                              unsigned long *file_bytes_read, unsigned long *file_bytes_written,
                                              unsigned long *logical_sector_reads, unsigned long *logical_sector_writes, unsigned long *logical_sector_cache_misses)
{

unsigned long   i;
unsigned long   total_events;
unsigned long   event_context;
unsigned long   event_id;
unsigned long   event_thread_priority;
unsigned long   event_time_stamp;
unsigned long   event_info_1;
unsigned long   event_info_2;
unsigned long   event_info_3;
unsigned long   event_info_4;
_int64          event_relative_ticks;
unsigned long   next_context;
unsigned long   local_media_opens;
unsigned long   local_media_closes;
unsigned long   local_media_aborts;
unsigned long   local_media_flushes;
unsigned long   local_directory_reads;
unsigned long   local_directory_writes;
unsigned long   local_directory_cache_misses;
unsigned long   local_file_opens;
unsigned long   local_file_closes;
unsigned long   local_file_bytes_read;
unsigned long   local_file_bytes_written;
unsigned long   local_logical_sector_reads;
unsigned long   local_logical_sector_writes;
unsigned long   local_logical_sector_cache_misses;


    /* Initialize all the local FileX statistics.  */
    local_media_opens =  0;
    local_media_closes =  0;
    local_media_aborts =  0;
    local_media_flushes =  0;
    local_directory_reads =  0;
    local_directory_writes =  0;
    local_directory_cache_misses =  0;
    local_file_opens =  0;
    local_file_closes =  0;
    local_file_bytes_read =  0;
    local_file_bytes_written =  0;
    local_logical_sector_reads =  0;
    local_logical_sector_writes =  0;
    local_logical_sector_cache_misses =  0;
    
    /* Setup the total events.  */
    total_events =  tml_total_events;

    /* Loop through all events.  */
    for (i = 0; i < total_events; i++)
    {

        /* Pickup event in the trace.  */
        tml_event_get(i, &event_context, &event_id,
                                &event_thread_priority, &event_time_stamp,
                                &event_info_1, &event_info_2, 
                                &event_info_3, &event_info_4,
                                &event_relative_ticks, &next_context, 0, 0, 0);

        /* Determine if we have a media open event.  */
        if (event_id == TML_FX_TRACE_MEDIA_OPEN)
        {
        
            /* Yes, increment the counter.  */
            local_media_opens++;
        }
        
        /* Determine if we have a media close event.  */
        else if (event_id == TML_FX_TRACE_MEDIA_CLOSE)
        {
        
            /* Yes, increment the counter.  */
            local_media_closes++;
        }

        /* Determine if we have a media abort event.  */
        else if (event_id == TML_FX_TRACE_MEDIA_ABORT)
        {
        
            /* Yes, increment the counter.  */
            local_media_aborts++;
        }

        /* Determine if we have a media flush event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_MEDIA_FLUSH)
        {
        
            /* Yes, increment the counter.  */
            local_media_flushes++;
        }

        /* Determine if we have a directory read event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_DIR_ENTRY_READ)
        {
        
            /* Yes, increment the counter.  */
            local_directory_reads++;
        }

        /* Determine if we have a directory write event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_DIR_ENTRY_WRITE)
        {
        
            /* Yes, increment the counter.  */
            local_directory_writes++;
        }

        /* Determine if we have a directory cache miss event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_DIR_CACHE_MISS)
        {
        
            /* Yes, increment the counter.  */
            local_directory_cache_misses++;
        }

        /* Determine if we have a file open event.  */
        else if (event_id == TML_FX_TRACE_FILE_OPEN)
        {
        
            /* Yes, increment the counter.  */
            local_file_opens++;
        }

        /* Determine if we have a file close event.  */
        else if (event_id == TML_FX_TRACE_FILE_CLOSE)
        {
        
            /* Yes, increment the counter.  */
            local_file_closes++;
        }

        /* Determine if we have a file read event.  */
        else if (event_id == TML_FX_TRACE_FILE_READ)
        {
        
            /* Yes, accumulate the number of bytes read.  */
            local_file_bytes_read =  local_file_bytes_read + event_info_4;
        }

        /* Determine if we have a file write event.  */
        else if (event_id == TML_FX_TRACE_FILE_WRITE)
        {
        
            /* Yes, accumulate the number of bytes read.  */
            local_file_bytes_written =  local_file_bytes_written + event_info_4;
        }

        /* Determine if we have a logical sector read event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_IO_DRIVER_READ)
        {
        
            /* Yes, accumulate the number of sectors read.  */
            local_logical_sector_reads =  local_logical_sector_reads + event_info_3;
        }

        /* Determine if we have a logical sector write event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_IO_DRIVER_WRITE)
        {
        
            /* Yes, accumulate the number of sectors written.  */
            local_logical_sector_writes =  local_logical_sector_writes + event_info_3;
        }

        /* Determine if we have a logical sector cache miss event.  */
        else if (event_id == TML_FX_TRACE_INTERNAL_LOG_SECTOR_CACHE_MISS)
        {
        
            /* Yes, increment the counter.  */
            local_logical_sector_cache_misses++;
        }
    }

    /* Initialize all the local FileX statistics.  */
    *media_opens =                  local_media_opens;
    *media_closes =                 local_media_closes;
    *media_aborts =                 local_media_aborts;
    *media_flushes =                local_media_flushes;
    *directory_reads =              local_directory_reads;
    *directory_writes =             local_directory_writes;
    *directory_cache_misses =       local_directory_cache_misses;
    *file_opens =                   local_file_opens;
    *file_closes =                  local_file_closes;
    *file_bytes_read =              local_file_bytes_read;
    *file_bytes_written =           local_file_bytes_written;
    *logical_sector_reads =         local_logical_sector_reads;
    *logical_sector_writes =        local_logical_sector_writes;
    *logical_sector_cache_misses =  local_logical_sector_cache_misses;
    
    return(0);
}


int  tml_system_netx_performance_statistics_get(unsigned long *arp_requests_sent, unsigned long *arp_responses_sent,
                                              unsigned long *arp_requests_received, unsigned long *arp_responses_received,
                                              unsigned long *packet_allocations, unsigned long *packet_releases,
                                              unsigned long *empty_allocations, unsigned long *invalid_releases,
                                              unsigned long *ip_packets_sent, unsigned long *ip_bytes_sent, 
                                              unsigned long *ip_packets_received, unsigned long *ip_bytes_received, 
                                              unsigned long *pings_sent, unsigned long *ping_responses,
                                              unsigned long *tcp_client_connections, unsigned long *tcp_server_connections,
                                              unsigned long *tcp_packets_sent, unsigned long *tcp_bytes_sent, 
                                              unsigned long *tcp_packets_received, unsigned long *tcp_bytes_received, 
                                              unsigned long *udp_packets_sent, unsigned long *udp_bytes_sent,
                                              unsigned long *udp_packets_received, unsigned long *udp_bytes_received)
{

unsigned long   i;
unsigned long   total_events;
unsigned long   event_context;
unsigned long   event_id;
unsigned long   event_thread_priority;
unsigned long   event_time_stamp;
unsigned long   event_info_1;
unsigned long   event_info_2;
unsigned long   event_info_3;
unsigned long   event_info_4;
_int64          event_relative_ticks;
unsigned long   next_context;
unsigned long   local_arp_requests_sent;
unsigned long   local_arp_responses_sent;
unsigned long   local_arp_requests_received;
unsigned long   local_arp_responses_received;
unsigned long   local_packet_allocations;
unsigned long   local_packet_releases;
unsigned long   local_empty_allocations;
unsigned long   local_invalid_releases;
unsigned long   local_ip_packets_sent;
unsigned long   local_ip_bytes_sent; 
unsigned long   local_ip_packets_received;
unsigned long   local_ip_bytes_received;
unsigned long   local_pings_sent;
unsigned long   local_ping_responses;
unsigned long   local_tcp_client_connections;
unsigned long   local_tcp_server_connections;
unsigned long   local_tcp_packets_sent;
unsigned long   local_tcp_bytes_sent;
unsigned long   local_tcp_packets_received;
unsigned long   local_tcp_bytes_received;
unsigned long   local_udp_packets_sent;
unsigned long   local_udp_bytes_sent;
unsigned long   local_udp_packets_received;
unsigned long   local_udp_bytes_received;


    /* Initialize all the counters to zero.  */
    local_arp_requests_sent =  0;
    local_arp_responses_sent =  0;
    local_arp_requests_received =  0;
    local_arp_responses_received =  0;
    local_packet_allocations =  0;
    local_packet_releases =  0;
    local_empty_allocations =  0;
    local_invalid_releases =  0;
    local_ip_packets_sent =  0;
    local_ip_bytes_sent =  0; 
    local_ip_packets_received =  0;
    local_ip_bytes_received =  0;
    local_pings_sent =  0;
    local_ping_responses =  0;
    local_tcp_client_connections =  0;
    local_tcp_server_connections =  0;
    local_tcp_packets_sent =  0;
    local_tcp_bytes_sent =  0;
    local_tcp_packets_received =  0;
    local_tcp_bytes_received =  0;
    local_udp_packets_sent =  0;
    local_udp_bytes_sent =  0;
    local_udp_packets_received =  0;
    local_udp_bytes_received =  0;

    /* Setup the total events.  */
    total_events =  tml_total_events;

    /* Loop through all events.  */
    for (i = 0; i < total_events; i++)
    {

        /* Pickup event in the trace.  */
        tml_event_get(i, &event_context, &event_id,
                                &event_thread_priority, &event_time_stamp,
                                &event_info_1, &event_info_2, 
                                &event_info_3, &event_info_4,
                                &event_relative_ticks, &next_context, 0, 0, 0);

        /* Determine if we have an arp request send event.  */
        if (event_id == TML_NX_TRACE_INTERNAL_ARP_REQUEST_SEND)
        {
        
            /* Yes, increment the counter.  */
            local_arp_requests_sent++;
        }

        /* Determine if we have an arp request send event.  */
        else if (event_id == TML_NX_TRACE_INTERNAL_ARP_RESPONSE_SEND)
        {
        
            /* Yes, increment the counter.  */
            local_arp_responses_sent++;
        }

        /* Determine if we have an arp requests received event.  */
        else if (event_id == TML_NX_TRACE_INTERNAL_ARP_REQUEST_RECEIVE)
        {
        
            /* Yes, increment the counter.  */
            local_arp_requests_received++;
        }

        /* Determine if we have an arp response received event.  */
        else if (event_id == TML_NX_TRACE_INTERNAL_ARP_RESPONSE_RECEIVE)
        {
        
            /* Yes, increment the counter.  */
            local_arp_responses_received++;
        }

        /* Determine if we have a packet allocate event.  */
        else if (event_id == TML_NX_TRACE_PACKET_ALLOCATE)
        {
        
            /* Yes, increment the counter.  */
            local_packet_allocations++;

            /* Check for empty requests.  */
            if (event_info_4 == 0)
            {
            
                /* Increment the empty requests counter.  */
                local_empty_allocations++;
            }
        }

        /* Determine if we have a packet release event.  */
        else if (event_id == TML_NX_TRACE_PACKET_RELEASE)
        {
        
            /* Yes, increment the counter.  */
            local_packet_releases++;

            /* Check for empty requests.  */
            if (event_info_2 != 0xAAAAAAAA)
            {
            
                /* Increment the invalid releases counter.  */
                local_invalid_releases++;
            }
        }

        /* Determine if we have an IP packet send event.  */
        else if (event_id == TML_NX_TRACE_INTERNAL_IP_SEND)
        {
        
            /* Yes, increment the counter.  */
            local_ip_packets_sent++;
            
            /* Accumulate the number of bytes sent.  */
            local_ip_bytes_sent =  local_ip_bytes_sent + event_info_4;
        }

        /* Determine if we have an IP packet receive event.  */
        else if (event_id == TML_NX_TRACE_INTERNAL_IP_RECEIVE)
        {
        
            /* Yes, increment the counter.  */
            local_ip_packets_received++;
            
            /* Accumulate the number of bytes received.  */
            local_ip_bytes_received =  local_ip_bytes_received + event_info_4;
        }

        /* Determine if we have a ping send event.  */
        else if (event_id == TML_NX_TRACE_ICMP_PING)
        {
        
            /* Yes, increment the counter.  */
            local_pings_sent++;
        }

        /* Determine if we have a ping response event.  */
        else if ((event_id == TML_NX_TRACE_INTERNAL_ICMP_RECEIVE) && ((event_info_4 & 0xFFFF0000) == 0))
        {
        
            /* Yes, increment the counter.  */
            local_ping_responses++;
        }

        /* Determine if we have a client connection event.  */
        else if (event_id == TML_NX_TRACE_TCP_CLIENT_SOCKET_CONNECT)
        {
        
            /* Yes, increment the counter.  */
            local_tcp_client_connections++;
        }

        /* Determine if we have a server connection event.  */
        else if (event_id == TML_NX_TRACE_TCP_SERVER_SOCKET_ACCEPT)
        {
        
            /* Yes, increment the counter.  */
            local_tcp_server_connections++;
        }

        /* Determine if we have a TCP receive event.  */
        else if (event_id == TML_NX_TRACE_TCP_SOCKET_RECEIVE)
        {
        
            /* Yes, increment the counter.  */
            local_tcp_packets_received++;
            
            /* Accumulate the number of bytes received.  */
            local_tcp_bytes_received =  local_tcp_bytes_received + event_info_3;
        }

        /* Determine if we have a TCP send event.  */
        else if (event_id == TML_NX_TRACE_TCP_SOCKET_SEND)
        {
        
            /* Yes, increment the counter.  */
            local_tcp_packets_sent++;
            
            /* Accumulate the number of bytes sent.  */
            local_tcp_bytes_sent =  local_tcp_bytes_sent + event_info_3;
        }

        /* Determine if we have a UDP receive event.  */
        else if (event_id == TML_NX_TRACE_UDP_SOCKET_RECEIVE)
        {
        
            /* Yes, increment the counter.  */
            local_udp_packets_received++;
            
            /* Accumulate the number of bytes received.  */
            local_udp_bytes_received =  local_udp_bytes_received + event_info_4;
        }

        /* Determine if we have a UDP send event.  */
        else if (event_id == TML_NX_TRACE_UDP_SOCKET_SEND)
        {
        
            /* Yes, increment the counter.  */
            local_udp_packets_sent++;
            
            /* Accumulate the number of bytes sent.  */
            local_udp_bytes_sent =  local_udp_bytes_sent + event_info_3;
        }
    }

    /* Return all the information.  */
    *arp_requests_sent =  local_arp_requests_sent;
    *arp_responses_sent =  local_arp_responses_sent;
    *arp_requests_received =  local_arp_requests_received;
    *arp_responses_received =  local_arp_responses_received;
    *packet_allocations =  local_packet_allocations;
    *packet_releases =  local_packet_releases;
    *empty_allocations =  local_empty_allocations;
    *invalid_releases = local_invalid_releases;
    *ip_packets_sent =  local_ip_packets_sent;
    *ip_bytes_sent =  local_ip_bytes_sent; 
    *ip_packets_received =  local_ip_packets_received;
    *ip_bytes_received =  local_ip_bytes_received;
    *pings_sent =  local_pings_sent;
    *ping_responses =  local_ping_responses;
    *tcp_client_connections =  local_tcp_client_connections;
    *tcp_server_connections =  local_tcp_server_connections;
    *tcp_packets_sent =  local_tcp_packets_sent;
    *tcp_bytes_sent =  local_tcp_bytes_sent;
    *tcp_packets_received =  local_tcp_packets_received;
    *tcp_bytes_received =  local_tcp_bytes_received;
    *udp_packets_sent =  local_udp_packets_sent;
    *udp_bytes_sent =  local_udp_bytes_sent;
    *udp_packets_received =  local_udp_packets_received;
    *udp_bytes_received =  local_udp_bytes_received;

    return(0);
}


int  tml_thread_performance_statistics_get(unsigned long thread_index, unsigned long *thread_suspensions, unsigned long *thread_resumptions)
{

unsigned long	event_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
_int64			event_relative_ticks;
unsigned long	suspensions;
unsigned long	resumptions;
unsigned long	i;
unsigned long	total_events;
unsigned char   *thread_name;
unsigned long   thread_address;
unsigned long   parameter_1;
unsigned long   parameter_2;
unsigned long   lowest_priority;
unsigned long   highest_priority;


	/* Determine if the thread index is valid.  */
	if (thread_index >= tml_total_threads)
	{

		/* Exceeded total threads... error!  */
		return(1);
	}

	/* Setup the total events.  */
	total_events =  tml_total_events;

    /* Pickup the thread context.  */
    tml_object_thread_get(thread_index, (char**)&thread_name, &thread_address,
                          &parameter_1, &parameter_2, &lowest_priority, &highest_priority);

	/* Calculate the total number of thread suspensions and resumptions.  */
	suspensions =  0;
	resumptions =  0;

	for (i = 0; i < total_events; i++)
	{

		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
								&event_thread_priority, &event_time_stamp,
								&event_info_1, &event_info_2, 
								&event_info_3, &event_info_4,
								&event_relative_ticks, 0, 0, 0, 0);

        /* Is this the thread in question?  */
        if (event_context != thread_address)
        {
        
            /* No, continue at the top of the loop!  */
            continue;
        }

			/* Determine if the event is a suspension. */
			if (event_id == TML_TRACE_THREAD_SUSPEND) 
			{

				/* Increment thread suspensions.  */
				suspensions++;
			}
			else if (event_id == TML_TRACE_THREAD_RESUME)
			{

				/* Increment thread resumptions.  */
				resumptions++;
			}
		}

	/* Determine if we need to return the suspensions or resumptions.  */
	if (thread_suspensions)
		*thread_suspensions =  suspensions;
	if (thread_resumptions)
		*thread_resumptions =  resumptions;

	return(0);
}


int  tml_system_execution_profile_get(_int64 *interrupt, _int64 *idle)
{

unsigned long	j;
unsigned long	total_events;
unsigned long	event_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
unsigned long	event_next_context;
_int64			event_relative_ticks;
unsigned long	prev_event_context;
unsigned long	prev_event_id;
unsigned long	prev_event_thread_priority;
unsigned long	prev_event_time_stamp;
unsigned long	prev_event_info_1;
unsigned long	prev_event_info_2;
unsigned long	prev_event_info_3;
unsigned long	prev_event_info_4;
unsigned long	prev_event_next_context;
_int64			prev_event_relative_ticks;
_int64			interrupt_ticks;
_int64			idle_ticks;



	/* Setup the total events.  */
	total_events =  tml_total_events;

	/* Set interrupt and idle time to 0.  */
	interrupt_ticks =  0;
	idle_ticks =       0;

	/* Loop through all events to calculate the idle and interrupt total time.  */
	for (j = 1; j < total_events; j++)
	{


		/* Pickup event in the trace.  */
		tml_event_get(j, &event_context, &event_id,
						 &event_thread_priority, &event_time_stamp,
						 &event_info_1, &event_info_2, 
						 &event_info_3, &event_info_4,
						 &event_relative_ticks, &event_next_context, 0, 0, 0);


		/* Pickup previous event in the trace.  */
		tml_event_get(j-1, &prev_event_context, &prev_event_id,
									 &prev_event_thread_priority, &prev_event_time_stamp,
									 &prev_event_info_1, &prev_event_info_2, 
									 &prev_event_info_3, &prev_event_info_4,
									 &prev_event_relative_ticks, &prev_event_next_context, 0, 0, 0);

		/* Check for interrupt event.  */
		if (prev_event_next_context == 0xFFFFFFFF)
		{

			/* Previous event was also an interrupt accumulate time in interrupt. */
			interrupt_ticks =  interrupt_ticks + (event_relative_ticks - prev_event_relative_ticks);
		}

		/* Determine if the system was idle.  */
		else if (prev_event_next_context == 0)
		{

			/* Idle system.  */
			idle_ticks =  idle_ticks + (event_relative_ticks - prev_event_relative_ticks);
		}
	}

	/* Determine if these values are required.  */
	if (interrupt)
		*interrupt =  interrupt_ticks;
	if (idle)
		*idle =  idle_ticks;

	return(0);
}


int  tml_thread_execution_profile_get(unsigned long thread_index, _int64 *thread_time)
{
unsigned long	j;
unsigned long	total_events;
unsigned long	event_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
unsigned long	event_next_context;
_int64			event_relative_ticks;
unsigned long	prev_event_context;
unsigned long	prev_event_id;
unsigned long	prev_event_thread_priority;
unsigned long	prev_event_time_stamp;
unsigned long	prev_event_info_1;
unsigned long	prev_event_info_2;
unsigned long	prev_event_info_3;
unsigned long	prev_event_info_4;
unsigned long	prev_event_next_context;
_int64			prev_event_relative_ticks;
_int64			relative_ticks;
char			*object_name;
unsigned long	object_address;
unsigned long	parameter_1;
unsigned long	parameter_2;


	/* Determine if the thread index is valid.  */
	if (thread_index >= tml_total_threads)
	{

		/* Exceeded total threads... error!  */
		return(1);
	}

	/* Setup the total events.  */
	total_events =  tml_total_events;

	/* Set relative time to 0.  */
	relative_ticks =  0;

	/* Pickup thread info.  */
	tml_object_thread_get(thread_index, &object_name, &object_address,
                                         &parameter_1, &parameter_2, 0, 0);


	/* Loop through all events to calculate the idle and interrupt total time.  */
	for (j = 1; j < total_events; j++)
	{


		/* Pickup event in the trace.  */
		tml_event_get(j, &event_context, &event_id,
						 &event_thread_priority, &event_time_stamp,
						 &event_info_1, &event_info_2, 
						 &event_info_3, &event_info_4,
						 &event_relative_ticks, &event_next_context, 0, 0, 0);


		/* Pickup previous event in the trace.  */
		tml_event_get(j-1, &prev_event_context, &prev_event_id,
									 &prev_event_thread_priority, &prev_event_time_stamp,
									 &prev_event_info_1, &prev_event_info_2, 
									 &prev_event_info_3, &prev_event_info_4,
									 &prev_event_relative_ticks, &prev_event_next_context, 0, 0, 0);

		/* Check for interrupt event.  */
		if (prev_event_next_context == object_address)
		{

			/* Previous event was also an interrupt accumulate time in interrupt. */
			relative_ticks =  relative_ticks + (event_relative_ticks - prev_event_relative_ticks);
		}
	}

	/* Determine if the value is required.  */
	if (thread_time)
		*thread_time =  relative_ticks;

	return(0);
}


int  tml_thread_stack_usage_get(unsigned long thread_index, unsigned long *stack_size, unsigned long *minimum_available, unsigned long *event)
{

unsigned long   j;
unsigned long	total_events;
char			*object_name;
unsigned long	object_address;
unsigned long	parameter_1;
unsigned long	parameter_2;
unsigned long	event_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
_int64			event_relative_ticks;
unsigned long	minimum_size;
unsigned long   stack_pointer;
unsigned long	saved_event;


	/* Determine if the thread index is valid.  */
	if (thread_index >= tml_total_threads)
	{

		/* Exceeded total threads... error!  */
		return(1);
	}

	/* Setup the total events.  */
	total_events =  tml_total_events;

	/* Pickup thread info.  */
	tml_object_thread_get(thread_index, &object_name, &object_address,
                                         &parameter_1, &parameter_2, 0, 0);

	/* Thread's minimum size.  */
	minimum_size =  parameter_2;

	/* Clear saved event.  */
	saved_event =  0;

	/* Loop through all events to calculate this thread's minimum stack.  */
	for (j = 0; j < total_events; j++)
	{


		/* Pickup event in the trace.  */
		tml_event_get(j, &event_context, &event_id,
									 &event_thread_priority, &event_time_stamp,
									 &event_info_1, &event_info_2, 
									 &event_info_3, &event_info_4,
									 &event_relative_ticks, 0, 0, 0, 0);

		/* Determine if this event is for the thread in question.  */
		if (event_context != object_address)
			continue;

		/* Look for events and the corresponding stack pointer.  */

        /* Default to the bottom of the stack.  */
		stack_pointer =  parameter_1 + (parameter_2-1);

        switch(event_id)
        {
        case TML_TRACE_ISR_ENTER:                   /* I1 = stack_ptr, I2 = ISR number (optional, user defined)                 */
        case TML_TRACE_ISR_EXIT:                    /* I1 = stack_ptr, I2 = ISR number (optional, user defined)                 */
        case TML_TRACE_THREAD_RELINQUISH:           /* I1 = stack ptr                                                           */
            stack_pointer = event_info_1;					
            break;

        case TML_TRACE_BLOCK_POOL_DELETE:           /* I1 = pool ptr, I2 = stack ptr                                            */
        case TML_TRACE_BYTE_POOL_DELETE:            /* I1 = pool ptr, I2 = stack ptr                                            */
        case TML_TRACE_EVENT_FLAGS_CREATE:          /* I1 = pool ptr, I2 = stack ptr                                            */
        case TML_TRACE_EVENT_FLAGS_DELETE:          /* I1 = pool ptr, I2 = stack ptr                                            */
        case TML_TRACE_INTERRUPT_CONTROL:           /* I1 = new interrupt posture, I2 = stack ptr                               */
        case TML_TRACE_MUTEX_DELETE:                /* I1 = mutex ptr, I2 = stack ptr                                           */
        case TML_TRACE_QUEUE_DELETE:                /* I1 = queue ptr, I2 = stack ptr                                           */
        case TML_TRACE_QUEUE_FLUSH:                 /* I1 = queue ptr, I2 = stack ptr                                           */
        case TML_TRACE_SEMAPHORE_DELETE:            /* I1 = semaphore ptr, I2 = stack ptr                                       */
        case TML_TRACE_THREAD_DELETE:               /* I1 = thread ptr, I2 = stack ptr                                          */
        case TML_TRACE_TIME_GET:                    /* I1 = current time, I2 = stack ptr                                        */
        case TML_TRACE_TIMER_DEACTIVATE:            /* I1 = timer ptr, I2 = stack ptr                                           */
        case TML_TRACE_TIMER_INFO_GET:              /* I1 = timer ptr, I2 = stack ptr                                           */
            stack_pointer = event_info_2;	
            break;
				
        case TML_TRACE_THREAD_RESUME:               /* I1 = thread ptr, I2 = previous_state, I3 = stack ptr                     */
        case TML_TRACE_THREAD_SUSPEND:              /* I1 = thread ptr, I2 = new_state, I3 = stack ptr  I4 = next thread ptr    */
        case TML_TRACE_BLOCK_POOL_PRIORITIZE:       /* I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
        case TML_TRACE_BYTE_POOL_PRIORITIZE:        /* I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
        case TML_TRACE_MUTEX_CREATE:                /* I1 = mutex ptr, I2 = inheritance, I3 = stack ptr                         */
        case TML_TRACE_MUTEX_PRIORITIZE:            /* I1 = mutex ptr, I2 = suspended count, I3 = stack ptr                     */
        case TML_TRACE_QUEUE_PRIORITIZE:            /* I1 = queue ptr, I2 = suspended count, I3 = stack ptr                     */
        case TML_TRACE_SEMAPHORE_CREATE:            /* I1 = semaphore ptr, I2 = initial count, I3 = stack ptr                   */
        case TML_TRACE_SEMAPHORE_PRIORITIZE:        /* I1 = semaphore ptr, I2 = suspended count, I3 = stack ptr                 */
        case TML_TRACE_THREAD_CREATE:               /* I1 = thread ptr, I2 = priority, I3 = stack ptr, I4 = stack_size          */
        case TML_TRACE_THREAD_ENTRY_EXIT_NOTIFY:    /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
        case TML_TRACE_THREAD_RESUME_API:           /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
        case TML_TRACE_THREAD_SLEEP:                /* I1 = sleep value, I2 = thread state, I3 = stack ptr                      */
        case TML_TRACE_THREAD_SUSPEND_API:          /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
        case TML_TRACE_THREAD_TERMINATE:            /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
        case TML_TRACE_THREAD_WAIT_ABORT:           /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
            stack_pointer = event_info_3;   
            break;

        case TML_TRACE_BLOCK_RELEASE:				/* I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = stack ptr           */
        case TML_TRACE_BYTE_POOL_CREATE:
        case TML_TRACE_MUTEX_PUT:                   /* I1 = mutex ptr, I2 = owning thread, I3 = own count, I4 = stack ptr       */
        case TML_TRACE_SEMAPHORE_GET:               /* I1 = semaphore ptr, I2 = wait option, I3 = current count, I4 = stack ptr */
        case TML_TRACE_SEMAPHORE_PUT:               /* I1 = semaphore ptr, I2 = current count, I3=suspended count, I4=stack ptr */
            stack_pointer = event_info_4;           /* I1 = pool ptr, I2 = start ptr, I3 = pool size, I4 = stack ptr            */
            break;

        default:
            /* Skip this event since there is no stack info for it  */
            continue;
        }

		/* Look for new minimum.  */
		if (stack_pointer < parameter_1)
		{
			saved_event =   j;
			minimum_size =  0;
		}
		else if ((stack_pointer - parameter_1) < minimum_size)
		{
			saved_event =   j;
			minimum_size =  stack_pointer - parameter_1;
		}
	}

	/* Determine what is required.  */
	if (stack_size)
		*stack_size =  parameter_2;
	if (minimum_available)
		*minimum_available =  minimum_size;
	if (event)
		*event =  saved_event;

	return(0);
}


int  tml_system_event_popularity_get(TML_EVENT_POPULARITY **popularity_list, unsigned long *list_size)
{

unsigned long			i, j;
unsigned long			total_events;
unsigned long			event_context;
unsigned long			event_id;
unsigned long			event_thread_priority;
unsigned long			event_time_stamp;
unsigned long			event_info_1;
unsigned long			event_info_2;
unsigned long			event_info_3;
unsigned long			event_info_4;
unsigned long			event_next_context;
_int64					event_relative_ticks;
TML_EVENT_POPULARITY	*current_popularity_list;
TML_EVENT_POPULARITY    *new_popularity_list;
TML_EVENT_POPULARITY	temp;
unsigned long			event_types;


	/* Setup the total events.  */
	total_events =  tml_total_events;

	/* Initialize the variables.  */
	current_popularity_list =	0;
	new_popularity_list =		0;
	event_types =				0;

	/* Loop through all events to calculate the idle and interrupt total time.  */
	for (i = 0; i < total_events; i++)
	{


		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
						 &event_thread_priority, &event_time_stamp,
						 &event_info_1, &event_info_2, 
						 &event_info_3, &event_info_4,
						 &event_relative_ticks, &event_next_context, 0, 0, 0);


		/* Don't count running event.  */
		if (event_id == TML_TRACE_RUNNING)
			continue;

		/* Determine if the entry was found.  */
		j =  0;
		while (j < event_types)
		{

			/* Do we have the same event?  */
			if (current_popularity_list[j].tml_event_popularity_event_id == event_id)
			{

				/* Yes, just increment the count of this event.  */
                current_popularity_list[j].tml_event_popularity_event_count++;

				/* Break out of the loop!  */
				break;
			}

			/* Otherwise, look at the next entry.  */
			j++;
		}

		/* Determine if we need to add a new entry in the list.  */
		if (j >= event_types)
		{

			/* Yup, we need to allocate space for the new event type that was found.  */

			/* Allocate size for new list. */
			if(event_types > ULONG_MAX - 1)
				return(-1);
			if(event_types + 1 > SIZE_MAX / sizeof(TML_EVENT_POPULARITY))
				return(-1);
			new_popularity_list =  (TML_EVENT_POPULARITY *) malloc((event_types+1)*sizeof(TML_EVENT_POPULARITY));

			/* Was it successful?  */
			if (!new_popularity_list)
			{
				if (popularity_list)
					*popularity_list =  0;
				if (list_size)
					*list_size =  0;
				return(1);
			}

			/* Copy the old list into the new.  */
			for (j = 0; j < event_types; j++)
			{

				/* Copy one entry from old to new list.  */
				new_popularity_list[j] =  current_popularity_list[j];
			}

			/* Now setup the new entry.  */
			new_popularity_list[event_types].tml_event_popularity_event_id =     event_id;
            new_popularity_list[event_types].tml_event_popularity_event_count =  1;
			event_types++;

			/* At this point free the current list.  */
			if (current_popularity_list)
				free(current_popularity_list);

			/* Update the current popularity list.  */
			current_popularity_list =  new_popularity_list;
		}
	}

	/* At this point, we need to sort the popularity list based on decending popularity.  */
	for (i = 0; i < event_types; i++)
	{

		for (j = i+1; j < event_types; j++)
		{

			/* Compare the counts of adjacent entries.  */
            if (current_popularity_list[i].tml_event_popularity_event_count < current_popularity_list[j].tml_event_popularity_event_count)
			{

				/* Need to swap entries.  */
				temp =  current_popularity_list[i];
				current_popularity_list[i] =  current_popularity_list[j];
				current_popularity_list[j] =  temp;
			}
		}
	}

	/* Determine what we need to return.  */
	if (popularity_list)
		*popularity_list =  current_popularity_list;
	if (list_size)
		*list_size =  event_types;

	return(0);
}


int  tml_thread_event_popularity_get(unsigned long thread_index, TML_EVENT_POPULARITY **popularity_list, unsigned long *list_size)
{

unsigned long			i, j;
unsigned long			total_events;
unsigned long			event_context;
unsigned long			event_id;
unsigned long			event_thread_priority;
unsigned long			event_time_stamp;
unsigned long			event_info_1;
unsigned long			event_info_2;
unsigned long			event_info_3;
unsigned long			event_info_4;
unsigned long			event_next_context;
_int64					event_relative_ticks;
TML_EVENT_POPULARITY	*current_popularity_list;
TML_EVENT_POPULARITY    *new_popularity_list;
TML_EVENT_POPULARITY	temp;
unsigned long			event_types;
char					*object_name;
unsigned long			object_address;
unsigned long			parameter_1;
unsigned long			parameter_2;


	/* Determine if the thread index is valid.  */
	if (thread_index >= tml_total_threads)
	{

		/* Exceeded total threads... error!  */
		return(1);
	}

	/* Pickup thread info.  */
	tml_object_thread_get(thread_index, &object_name, &object_address,
                                         &parameter_1, &parameter_2, 0, 0);

	/* Setup the total events.  */
	total_events =  tml_total_events;

	/* Initialize the variables.  */
	current_popularity_list =	0;
	new_popularity_list =		0;
	event_types =				0;

	/* Loop through all events to calculate the idle and interrupt total time.  */
	for (i = 0; i < total_events; i++)
	{


		/* Pickup event in the trace.  */
		tml_event_get(i, &event_context, &event_id,
						 &event_thread_priority, &event_time_stamp,
						 &event_info_1, &event_info_2, 
						 &event_info_3, &event_info_4,
						 &event_relative_ticks, &event_next_context, 0, 0, 0);

		/* Does this match the selected thread?  */
		if (object_address != event_context)
			continue;

		/* Determine if the entry was found.  */
		j =  0;
		while (j < event_types)
		{

			/* Do we have the same event?  */
			if (current_popularity_list[j].tml_event_popularity_event_id == event_id)
			{

				/* Yes, just increment the count of this event.  */
                current_popularity_list[j].tml_event_popularity_event_count++;

				/* Break out of the loop!  */
				break;
			}

			/* Otherwise, look at the next entry.  */
			j++;
		}

		/* Determine if we need to add a new entry in the list.  */
		if (j >= event_types)
		{

			/* Yup, we need to allocate space for the new event type that was found.  */

			/* Allocate size for new list. */
			if(event_types > ULONG_MAX - 1)
				return(-1);
			if(event_types + 1 > SIZE_MAX / sizeof(TML_EVENT_POPULARITY))
				return(-1);
			new_popularity_list =  (TML_EVENT_POPULARITY *) malloc((event_types+1)*sizeof(TML_EVENT_POPULARITY));

			/* Was it successful?  */
			if (!new_popularity_list)
			{
				if (popularity_list)
					*popularity_list =  0;
				if (list_size)
					*list_size =  0;
				return(1);
			}

			/* Copy the old list into the new.  */
			for (j = 0; j < event_types; j++)
			{

				/* Copy one entry from old to new list.  */
				new_popularity_list[j] =  current_popularity_list[j];
			}

			/* Now setup the new entry.  */
			new_popularity_list[event_types].tml_event_popularity_event_id =     event_id;
            new_popularity_list[event_types].tml_event_popularity_event_count =  1;
			event_types++;

			/* At this point free the current list.  */
			if (current_popularity_list)
				free(current_popularity_list);

			/* Update the current popularity list.  */
			current_popularity_list =  new_popularity_list;
		}
	}

	/* At this point, we need to sort the popularity list based on decending popularity.  */
	for (i = 0; i < event_types; i++)
	{

		for (j = i+1; j < event_types; j++)
		{

			/* Compare the counts of adjacent entries.  */
            if (current_popularity_list[i].tml_event_popularity_event_count < current_popularity_list[j].tml_event_popularity_event_count)
			{

				/* Need to swap entries.  */
				temp =  current_popularity_list[i];
				current_popularity_list[i] =  current_popularity_list[j];
				current_popularity_list[j] =  temp;
			}
		}
	}

	/* Determine what we need to return.  */
	if (popularity_list)
		*popularity_list =  current_popularity_list;
	if (list_size)
		*list_size =  event_types;

	return(0);
}


int  tml_thread_total_events_get(unsigned long thread_index, unsigned long *total_thread_events_ptr)
{

unsigned long           i;
unsigned long           total_events;
unsigned long           total_thread_events;
unsigned long           event_context;
unsigned long           event_id;
unsigned long           event_thread_priority;
unsigned long           event_time_stamp;
unsigned long           event_info_1;
unsigned long           event_info_2;
unsigned long           event_info_3;
unsigned long           event_info_4;
unsigned long           event_next_context;
_int64                  event_relative_ticks;
char                    *object_name;
unsigned long           object_address;
unsigned long           parameter_1;
unsigned long           parameter_2;


    /* Determine if the thread index is valid.  */
    if (thread_index >= tml_total_threads)
    {

        /* Set the total events to 0.  */
        *total_thread_events_ptr =  0;

        /* Exceeded total threads... error!  */
        return(1);
    }

    /* Pickup thread info.  */
    tml_object_thread_get(thread_index, &object_name, &object_address,
                                         &parameter_1, &parameter_2, 0, 0);

    /* Setup the total events.  */
    total_events =  tml_total_events;

    /* Initialize the total events for this thread.  */
    total_thread_events =  0;

    /* Loop through all events to calculate the idle and interrupt total time.  */
    for (i = 0; i < total_events; i++)
    {


        /* Pickup event in the trace.  */
        tml_event_get(i, &event_context, &event_id,
                         &event_thread_priority, &event_time_stamp,
                         &event_info_1, &event_info_2, 
                         &event_info_3, &event_info_4,
                         &event_relative_ticks, &event_next_context, 0, 0, 0);

        /* Does this match the selected thread?  */
        if (object_address == event_context)
        {
        
            /* Yes, this event is for this thread, increment the counter.  */
            total_thread_events++;
        }
    }

    /* Return the total number of events for this thread.  */
    *total_thread_events_ptr =  total_thread_events;

    return(0);
}


int  tml_thread_most_recent_priority_find(unsigned long thread_index, unsigned long event, unsigned long *priority, unsigned long *preemption_threshold)
{

unsigned long           i;
unsigned long           event_context;
unsigned long           event_id;
unsigned long           event_thread_priority;
unsigned long           event_thread_preemption_threshold;
unsigned long           event_time_stamp;
unsigned long           event_info_1;
unsigned long           event_info_2;
unsigned long           event_info_3;
unsigned long           event_info_4;
unsigned long           event_next_context;
_int64                  event_relative_ticks;
char                    *object_name;
unsigned long           object_address;
unsigned long           parameter_1;
unsigned long           parameter_2;
unsigned long           lowest_priority;
unsigned long           highest_priority;


    /* Determine if the thread index is valid.  */
    if (thread_index >= tml_total_threads)
    {

        /* Exceeded total threads... error!  */
        return(1);
    }

    /* Determine if the event is valid.  */
    if (event >= tml_total_events)
    {
    
        /* Exceeded total events... error!  */
        return(1);
    }

    /* Pickup thread info.  */
    tml_object_thread_get(thread_index, &object_name, &object_address,
                                         &parameter_1, &parameter_2, &lowest_priority, &highest_priority);

    /* Loop through all events to calculate the idle and interrupt total time.  */
    i =  event;
    do
    {
        /* Pickup event in the trace.  */
        tml_event_get(i, &event_context, &event_id,
                         &event_thread_priority, &event_time_stamp,
                         &event_info_1, &event_info_2, 
                         &event_info_3, &event_info_4,
                         &event_relative_ticks, &event_next_context, 0, 0, 0);

        /* Does this match the selected thread?  */
        if (object_address == event_context)
        {
        
            /* Return the priority.  */
            if (priority)
                *priority =  event_thread_priority;
                
            /* Pickup the preemption-threshold.  */
            tml_event_preemption_threshold_get(i, &event_thread_preemption_threshold);
            
            /* Return the preemption-threshold.  */
            if (preemption_threshold)
                *preemption_threshold =  event_thread_preemption_threshold;
                
            /* Return success!  */
            return(0);
        }

        /* Check for completion.  */
        if (i == 0)
            break;
            
        /* Otherwise, move backward.  */
        i--;
      
    } while (1);

    /* Otherwise, default to the possible priority found in the created object.  */
    if (priority)
        *priority =  lowest_priority;
        
    /* There is not preemption-threshold in the registry... so default to invalid.  */
    if (preemption_threshold)
        *preemption_threshold =  0xFFFFFFFF;

    /* Return success!  */
    return(0);
}


int  tml_uninitialize(void)
{

	/* Free all the memory resources allocated on the previous initialization.  */
	free(tml_object_array);
	free(tml_object_thread_list);
	free(tml_event_array);

    /* Loop to release all the thread status records.  */
    if (tml_thread_status_list)
    {
    unsigned long   i;
    
        /* Loop to release all the status lists.   */
        for (i = 0; i < tml_total_threads; i++)
        {

            /* Determine if there is a status list.  */
            if (tml_thread_status_list[i].tml_thread_status_summary_list)
                free(tml_thread_status_list[i].tml_thread_status_summary_list);        
        }
        
        /* Release the main list.  */
        free(tml_thread_status_list);
		tml_thread_status_list =            0;
    }

	/* Reset all globals in preparation for next initialization. */
	tml_header_trace_id =						0;
	tml_header_timer_valid_mask =				0;
	tml_header_trace_base_address =				0;
	tml_header_object_registry_start_address =	0;
	tml_header_reserved1 =						0;
    tml_header_object_name_size =				0;
    tml_header_object_registry_end_address =	0;
    tml_header_trace_buffer_start_address =		0;
    tml_header_trace_buffer_end_address =		0;
    tml_header_trace_buffer_current_address =	0;
    tml_header_reserved2 =						0;
    tml_header_reserved3 =						0;
    tml_header_reserved4 =						0;
    tml_object_array =							0;
    tml_object_thread_list =					0;
    tml_total_threads =							0;
 	tml_total_objects =							0;
	tml_max_objects =							0;
    tml_total_priority_inversions =				0;
	tml_total_bad_priority_inversions =			0;
    tml_event_array =							0;
	tml_total_events =							0;
	tml_relative_ticks =						0;

	return(0);
}


int     tml_thread_execution_status_get(unsigned long thread_index, unsigned long starting_event, unsigned long ending_event, unsigned long *execution_status_list, unsigned long max_status_pairs, unsigned long *status_pairs_returned)
{

unsigned long   *status_list;
unsigned long   changes;
unsigned long   change_index;
unsigned long   return_index;
unsigned long   pairs_found;


    /* Determine if there are errors in the input parameters.  */
    if (thread_index >= tml_total_threads)
        return(1);
    
    if (starting_event >= tml_total_events)
        return(1);
        
    if (ending_event >= tml_total_events)
        return(1);
       
    if (starting_event >= ending_event)
        return(1);

    if (execution_status_list == NULL)
        return(1);
        
    if (max_status_pairs < tml_total_events)
        return(1);

    if (status_pairs_returned == NULL)
        return(1);

    /* Check for no thread status summary.  */
    if (tml_thread_status_list == NULL)
    {
        execution_status_list[0] =  starting_event;
        execution_status_list[1] =  TML_THREAD_STATUS_UNKNOWN;
        *status_pairs_returned =    1;
        return(0);    
    }

    /* Pickup the status list.  */
    status_list =  tml_thread_status_list[thread_index].tml_thread_status_summary_list;

    /* Check for no thread status list.  */
    if (status_list == NULL)
    {
        
        execution_status_list[0] =  starting_event;
        execution_status_list[1] =  TML_THREAD_STATUS_UNKNOWN;
        *status_pairs_returned =    1;
        return(0);    
    }
    
    /* Pickup the number of status changes.  */
    changes =  tml_thread_status_list[thread_index].tml_thread_status_summary_status_changes;
    
    /* Pairs found.  */
    pairs_found =  0;
    
    /* Setup the indicies.  */
    change_index =  0;
    return_index =  0;
    
    /* Loop to find the thread status changes between the specified events.   */
    do
    {
        /* Is the current status change within the starting index?  */
        if (starting_event < status_list[change_index+2])
        {
            /* Yes, we have found a status pair that matches.  */
            execution_status_list[return_index] =    starting_event;
            execution_status_list[return_index+1] =  status_list[change_index+1];
            
            /* Increment the pairs found count.  */
            pairs_found++;
            
            /* Move the starting event to the next event.  */
            starting_event =  status_list[change_index+2];
            
            /* Determine if we are done.  */
            if ((starting_event == 0xFFFFFFFF) || (starting_event > ending_event))
                break;
                
            /* Move the return index.  */
            return_index =  return_index + 2;
        }
    
        /* Move the change index forward.  */
        change_index =  change_index + 2;
    
    } while ((change_index < (changes*2)) && ((change_index/2) < max_status_pairs));

    /* Return the pairs found.  */
    *status_pairs_returned =  pairs_found;
    return(0);
}


void  tml_time_to_ascii(_int64 time, char *string)
{
int m;

    string[20] =  0;

	for (m = 19; m >= 0; m--) 
	{

		string[m] =  (char) ((time % 10) + 0x30);
		time =  time / 10;
	}
}

char *tml_get_build_date()
{
    return __DATE__;
}

int  tml_raw_trace_file_dump(FILE *dump_file, char *tracex_version, char *input_file_name, char *dump_file_name)
{

int				status;
unsigned long   i, j;
unsigned long	total_threads;
unsigned long	total_objects;
unsigned long	total_events;
_int64			max_relative_ticks;
unsigned long	header_trace_id;
unsigned long	header_timer_valid_mask;
unsigned long	header_trace_base_address;
unsigned long	header_object_registry_start_address;
unsigned short	header_reserved1;
unsigned short	header_object_name_size;
unsigned long	header_object_registry_end_address;
unsigned long	header_trace_buffer_start_address;
unsigned long	header_trace_buffer_end_address;
unsigned long	header_trace_buffer_current_address;
unsigned long	header_reserved2;
unsigned long	header_reserved3;
unsigned long	header_reserved4;
char			*object_name;
unsigned long	object_address;
unsigned long	parameter_1;
unsigned long	parameter_2;
unsigned long	event_context;
unsigned long	event_next_context;
unsigned long	event_id;
unsigned long	event_thread_priority;
unsigned long	event_time_stamp;
unsigned long	event_info_1;
unsigned long	event_info_2;
unsigned long	event_info_3;
unsigned long	event_info_4;
unsigned long   event_preemption_threshold;
_int64			event_relative_ticks;
_int64			relative_ticks;
unsigned long	context_switches;
_int64			interrupt_ticks;
_int64			idle_ticks;
unsigned long   saved_event;
unsigned long	minimum_size;
unsigned long	thread_preemptions;
unsigned long   thread_resumptions;
unsigned long   thread_suspensions;
unsigned long   priority_inversions;
unsigned long   bad_priority_inversions;
unsigned long   interrupts;
unsigned long	time_slices;
unsigned long	nested_interrupts;
unsigned long   stack_size;
TML_EVENT_POPULARITY *popularity_list;
unsigned long	list_size;
unsigned long   thread_index;
unsigned long   priority_inversion;
unsigned long   bad_priority_inversion;
unsigned long	object_index;
unsigned long   lowest_priority;
unsigned long   highest_priority;
unsigned long   lowest_preemption_threshold;
unsigned long   highest_preemption_threshold;
char			time_string[21];
unsigned long   media_opens;
unsigned long   media_closes;
unsigned long   media_aborts;
unsigned long   media_flushes;
unsigned long   directory_reads;
unsigned long   directory_writes;
unsigned long   directory_cache_misses;
unsigned long   file_opens;
unsigned long   file_closes;
unsigned long   file_bytes_read;
unsigned long   file_bytes_written;
unsigned long   logical_sector_reads;
unsigned long   logical_sector_writes;
unsigned long   logical_sector_cache_misses;
unsigned long   arp_requests_sent;
unsigned long   arp_responses_sent;
unsigned long   arp_requests_received;
unsigned long   arp_responses_received;
unsigned long   packet_allocations;
unsigned long   packet_releases;
unsigned long   empty_allocations;
unsigned long   invalid_releases;
unsigned long   ip_packets_sent;
unsigned long   ip_bytes_sent; 
unsigned long   ip_packets_received;
unsigned long   ip_bytes_received;
unsigned long   pings_sent;
unsigned long   ping_responses;
unsigned long   tcp_client_connections;
unsigned long   tcp_server_connections;
unsigned long   tcp_packets_sent;
unsigned long   tcp_bytes_sent;
unsigned long   tcp_packets_received;
unsigned long   tcp_bytes_received;
unsigned long   udp_packets_sent;
unsigned long   udp_bytes_sent;
unsigned long   udp_packets_received;
unsigned long   udp_bytes_received;
unsigned long   total_thread_events;


    /* Pickup basic parameters.  */
    total_threads =       tml_total_threads;
    total_objects =       tml_total_objects;
    total_events =        tml_total_events;
    max_relative_ticks =  tml_relative_ticks;

	/* Print out the trace dump header.  */
    fprintf(dump_file,"******************************************************* TraceX Version %s ***************************************************************\n\n", tracex_version);
	fprintf(dump_file,"Input Trace File:     %s\n", input_file_name);
	fprintf(dump_file,"Output Trace File:    %s\n", dump_file_name);
	fprintf(dump_file,"\n\n");

	tml_header_info_get(&header_trace_id, 
						&header_timer_valid_mask,
						&header_trace_base_address,
						&header_object_registry_start_address,
						&header_reserved1,
						&header_object_name_size,
						&header_object_registry_end_address,
						&header_trace_buffer_start_address,
						&header_trace_buffer_end_address,
						&header_trace_buffer_current_address,
						&header_reserved2,
						&header_reserved3,
						&header_reserved4);

	/* Now print the trace file header information.  */
	fprintf(dump_file,"************** Trace File Header Information ****************\n");
	fprintf(dump_file,"\n");
	fprintf(dump_file,"Total Threads:       %lu\n", total_threads);
	fprintf(dump_file,"Total Events:        %lu\n", total_events);
	tml_time_to_ascii(max_relative_ticks, time_string);
	fprintf(dump_file,"Max Relative Ticks:  %20s\n", time_string);
	fprintf(dump_file,"Trace ID:            %08lX\n", header_trace_id);
	fprintf(dump_file,"Timer Mask:          %08lX\n", header_timer_valid_mask);
	fprintf(dump_file,"Object Name Size:    %lu\n", (unsigned long) header_object_name_size);
	fprintf(dump_file,"Trace Base Address:  %08lX\n", header_trace_base_address);
	fprintf(dump_file,"Object Start:        %08lX\n", header_object_registry_start_address);
	fprintf(dump_file,"Object End:          %08lX\n", header_object_registry_end_address);
	fprintf(dump_file,"Buffer Start:        %08lX\n", header_trace_buffer_start_address);
	fprintf(dump_file,"Buffer End:          %08lX\n", header_trace_buffer_end_address);
	fprintf(dump_file,"Buffer Oldest:       %08lX\n", header_trace_buffer_current_address);
	fprintf(dump_file,"\n\n");

	/* Now print out the threads in the trace.  */
    fprintf(dump_file,"****************************************** Threads in this Trace *****************************************\n");
    fprintf(dump_file,"   Address        Stack         Total Events               Name\n");
	fprintf(dump_file,"\n");
	for (i = 0; i < total_threads; i++)
	{

		tml_object_thread_get(i, &object_name, &object_address,
								 &parameter_1, &parameter_2, &lowest_priority, &highest_priority);
		fprintf(dump_file,"  %08lX", object_address);
		fprintf(dump_file,"  %08lX-%08lX", parameter_1, (parameter_1 + parameter_2));
        
        /* Get the total events.  */
        tml_thread_total_events_get(i, &total_thread_events);
        fprintf(dump_file,"  %8lu", total_thread_events);
        
        if (lowest_priority == 0xFFFFFFFF)
            fprintf(dump_file,"  %s (Priority: ?/?) ", object_name);
        else
            fprintf(dump_file,"  %s (Priority: %lu/%lu) ", object_name, lowest_priority, highest_priority);

        tml_object_thread_preemption_threshold_get(i, &lowest_preemption_threshold, &highest_preemption_threshold);
        if (lowest_preemption_threshold == 0xFFFFFFFF)
            fprintf(dump_file," (Preemption-Threshold: ?/?) \n");
        else
            fprintf(dump_file," (Preemption-Threshold: %lu/%lu) \n", lowest_preemption_threshold, highest_preemption_threshold);
	}
	fprintf(dump_file,"\n\n");

	/* Now print out the objects in the trace.  */
    fprintf(dump_file,"**************************************************************** Objects in Trace *****************************************************************\n");
    fprintf(dump_file,"   Address       Type                  Name                    Reserved1  Reserved2        Parameter 1                          Parameter 2\n");
	fprintf(dump_file,"\n");
	for (i = 0; i < total_objects; i++)
	{

		tml_object_get(i, &object_name, &object_address,
						 &parameter_1, &parameter_2);
		fprintf(dump_file,"  %08lX", object_address);

		if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_THREAD)
		{
			fprintf(dump_file,"   THREAD     ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_TIMER)
		{
			fprintf(dump_file,"   TIMER      ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_QUEUE)
		{
			fprintf(dump_file,"   QUEUE      ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_SEMAPHORE)
		{
			fprintf(dump_file,"   SEMAPHORE  ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_MUTEX)
		{
			fprintf(dump_file,"   MUTEX      ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_EVENT_FLAGS)
		{
			fprintf(dump_file,"   EVENT FLAGS");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_BLOCK_POOL)
		{
			fprintf(dump_file,"   BLOCK POOL ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_BYTE_POOL)
		{
			fprintf(dump_file,"   BYTE POOL  ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_MEDIA)
		{
			fprintf(dump_file,"   FX MEDIA   ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_FILE)
		{
			fprintf(dump_file,"   FX FILE    ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_IP)
		{
			fprintf(dump_file,"   NX IP      ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_PACKET_POOL)
		{
			fprintf(dump_file,"   NX PK POOL ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_TCP_SOCKET)
		{
			fprintf(dump_file,"   NX TCP SOC ");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_UDP_SOCKET)
		{
			fprintf(dump_file,"   NX UDP SOC ");
		}
		else
		{
			fprintf(dump_file,"   UNKNOWN");
		}
		
		
		fprintf(dump_file,"  %32s", object_name);

        fprintf(dump_file,"         %02lX", tml_object_array[i].tml_object_reserved1);
		fprintf(dump_file,"         %02lX", tml_object_array[i].tml_object_reserved2);

		if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_THREAD)
		{
			fprintf(dump_file,"       Stack Start:   %08lX", parameter_1);
			fprintf(dump_file,"               Stack Size:       %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_TIMER)
		{
			fprintf(dump_file,"       Initial Ticks: %8ld", parameter_1);
			fprintf(dump_file,"               Reschedule Ticks: %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_QUEUE)
		{
			fprintf(dump_file,"       Queue Size:    %8ld", parameter_1);
			fprintf(dump_file,"               Message Size:     %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_SEMAPHORE)
		{
			fprintf(dump_file,"       Initial Count: %8ld", parameter_1);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_MUTEX)
		{
			if (parameter_1 == 0)
				fprintf(dump_file,"       No Inheritance");
			else
				fprintf(dump_file,"       Priority Inheritance Enabled");
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_BLOCK_POOL)
		{
			fprintf(dump_file,"       Total Blocks:  %8ld", parameter_1);
			fprintf(dump_file,"               Block Size:       %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_BYTE_POOL)
		{
			fprintf(dump_file,"       Total Size:    %8ld", parameter_1);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_IP)
		{
			fprintf(dump_file,"       Stack Address: %08lX", parameter_1);
			fprintf(dump_file,"               Stack Size:       %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_PACKET_POOL)
		{
			fprintf(dump_file,"       Packet Size:   %8ld", parameter_1);
			fprintf(dump_file,"               Number of Packets:%ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_TCP_SOCKET)
		{
			fprintf(dump_file,"       Type of Serv:  %08lX", parameter_1);
			fprintf(dump_file,"               Window Size:      %ld", parameter_2);
		}
		else if (tml_object_array[i].tml_object_type == TML_TRACE_OBJECT_TYPE_UDP_SOCKET)
		{
			fprintf(dump_file,"       Type of Serv:  %08lX", parameter_1);
			fprintf(dump_file,"               Receive Queue Max:%ld", parameter_2);
		}

		fprintf(dump_file,"\n");
	}
	fprintf(dump_file,"\n\n");

	/* Get the performance statistics.  */
	tml_system_performance_statistics_get(&context_switches, &thread_preemptions, &time_slices, 
										  &thread_suspensions, &thread_resumptions, &interrupts, &nested_interrupts,
										  &priority_inversions, &bad_priority_inversions);

	fprintf(dump_file,"************** Performance Statistics ***********************\n");
	fprintf(dump_file,"\n");
	fprintf(dump_file,"Context Switches:    %lu\n", context_switches);
	fprintf(dump_file,"Thread Preemptions:  %lu\n", thread_preemptions);
	fprintf(dump_file,"Time Slices:         %lu\n", time_slices);
	fprintf(dump_file,"Thread Suspensions:  %lu\n", thread_suspensions);
	fprintf(dump_file,"Thread Resumptions:  %lu\n", thread_resumptions);
	fprintf(dump_file,"Interrupts:          %lu\n", interrupts);
	fprintf(dump_file,"Nested Interrupts:   %lu\n", nested_interrupts);
	fprintf(dump_file,"Priority Inversions: %lu/%lu\n", priority_inversions, bad_priority_inversions); 
	fprintf(dump_file,"\n\n");

	/* Now print out the thread execution.  */
	fprintf(dump_file,"************** Thread Execution *****************************\n");
	fprintf(dump_file,"   Total Ticks      Percentage          Name\n");
	fprintf(dump_file,"\n");

	/* Pickup interrupt and idle ticks.  */
	tml_system_execution_profile_get(&interrupt_ticks, &idle_ticks);

	/* Loop through all events to calculate this thread's total time.  */
	for (j = 0; j < total_threads; j++)
	{

		/* Thread's relative time to 0.  */
		relative_ticks =  0;

		tml_thread_execution_profile_get(j, &relative_ticks);

		tml_time_to_ascii(relative_ticks, time_string);
		fprintf(dump_file," %20s", time_string);
		relative_ticks =  (relative_ticks * 100);
		if ((relative_ticks) && (relative_ticks < max_relative_ticks))
			fprintf(dump_file,"        <1%%");
		else
		{
			if (max_relative_ticks == 0)
				relative_ticks =  0;
			else
				relative_ticks =  relative_ticks/max_relative_ticks;
			fprintf(dump_file,"        %02lu%%", (unsigned long) (relative_ticks));
		}
	
		/* Pickup thread info.  */
		tml_object_thread_get(j, &object_name, &object_address,
												&parameter_1, &parameter_2, &lowest_priority, &highest_priority);

		fprintf(dump_file,"  %s\n", object_name);
	}

	relative_ticks =  idle_ticks;
	tml_time_to_ascii(relative_ticks, time_string);
	fprintf(dump_file," %20s", time_string);
	relative_ticks =  (relative_ticks * 100);
	if ((relative_ticks) && (relative_ticks < max_relative_ticks))
		fprintf(dump_file,"        <1%%");
	else
	{
	   if (max_relative_ticks == 0)
		   relative_ticks =  0;
	   else
		   relative_ticks =  relative_ticks/max_relative_ticks;
	   fprintf(dump_file,"        %02lu%%", (unsigned long) (relative_ticks));
	}
	fprintf(dump_file,"  Idle System\n");

	relative_ticks =  interrupt_ticks;
	tml_time_to_ascii(relative_ticks, time_string);
	fprintf(dump_file," %20s", time_string);
	relative_ticks =  (relative_ticks * 100);
	if ((relative_ticks) && (relative_ticks < max_relative_ticks))
		fprintf(dump_file,"        <1%%");
	else
	{
	   if (max_relative_ticks == 0)
	       relative_ticks = 0;
	   else
		   relative_ticks =  relative_ticks/max_relative_ticks;
	   fprintf(dump_file,"        %02lu%%", (unsigned long) (relative_ticks));
	}
	fprintf(dump_file,"  Interrupt\n");
	fprintf(dump_file,"\n\n");

	fprintf(dump_file,"************** Thread Stack Usage ***************************\n");
	fprintf(dump_file,"   Stack Size	Minimum   Event ID        Name\n");
	fprintf(dump_file,"\n");
	for (i = 0; i < total_threads; i++)
	{

		/* Pickup thread info.  */
		tml_object_thread_get(i, &object_name, &object_address,
										 &parameter_1, &parameter_2, &lowest_priority, &highest_priority);

		/* Calculate thread stack usage.  */
		tml_thread_stack_usage_get(i, &stack_size, &minimum_size, &saved_event);

		fprintf(dump_file," %8lu", stack_size);
		if (stack_size == minimum_size)
			fprintf(dump_file,"          ---       ---");
		else
		{
			fprintf(dump_file,"     %8lu", minimum_size);
			fprintf(dump_file,"  %8lu", saved_event);
		}
		fprintf(dump_file,"     %s\n", object_name);
	}
	fprintf(dump_file,"\n\n");

    /* Get the FileX performance statistics.  */    
    tml_system_filex_performance_statistics_get(&media_opens, &media_closes, &media_aborts, &media_flushes,
                                                &directory_reads, &directory_writes, &directory_cache_misses,
                                                &file_opens, &file_closes, 
                                                &file_bytes_read, &file_bytes_written, &logical_sector_reads, 
                                                &logical_sector_writes, &logical_sector_cache_misses);

    fprintf(dump_file,"************** FileX Performance Statistics *****************************\n");
    fprintf(dump_file,"\n");
    fprintf(dump_file,"Media Statistics:\n");
    fprintf(dump_file,"  Opens:         %lu\n", media_opens);
    fprintf(dump_file,"  Closes:        %lu\n", media_closes);
    fprintf(dump_file,"  Aborts:        %lu\n", media_aborts);
    fprintf(dump_file,"  Flushes:       %lu\n", media_flushes);
    fprintf(dump_file,"\nDirectory Statistics:\n");
    fprintf(dump_file,"  Reads:         %lu\n", directory_reads);
    fprintf(dump_file,"  Writes:        %lu\n", directory_writes);
    fprintf(dump_file,"  Cache Misses:  %lu\n", directory_cache_misses);
    fprintf(dump_file,"\nFile Statistics:\n");
    fprintf(dump_file,"  Opens:         %lu\n", file_opens);
    fprintf(dump_file,"  Closes:        %lu\n", file_closes);
    fprintf(dump_file,"  Bytes Read:    %lu\n", file_bytes_read);
    fprintf(dump_file,"  Bytes Written: %lu\n", file_bytes_written);
    fprintf(dump_file,"\nLogical Sector Statistics:\n");
    fprintf(dump_file,"  Reads:         %lu\n", logical_sector_reads); 
    fprintf(dump_file,"  Writes:        %lu\n", logical_sector_writes); 
    fprintf(dump_file,"  Cache Misses:  %lu\n", logical_sector_cache_misses); 
    fprintf(dump_file,"\n\n");

    /* Get the NetX performance statistics.  */
    tml_system_netx_performance_statistics_get(&arp_requests_sent, &arp_responses_sent, &arp_requests_received, &arp_responses_received,
                                              &packet_allocations, &packet_releases, &empty_allocations, &invalid_releases,
                                              &ip_packets_sent, &ip_bytes_sent, &ip_packets_received, &ip_bytes_received, 
                                              &pings_sent, &ping_responses,
                                              &tcp_client_connections, &tcp_server_connections,
                                              &tcp_packets_sent, &tcp_bytes_sent, &tcp_packets_received, &tcp_bytes_received, 
                                              &udp_packets_sent, &udp_bytes_sent, &udp_packets_received, &udp_bytes_received);
    
    fprintf(dump_file,"************** NetX Performance Statistics *****************************\n");
    fprintf(dump_file,"\n");
    fprintf(dump_file,"\nARP Statistics:\n");
    fprintf(dump_file,"  Requests Sent:              %lu\n", arp_requests_sent);
    fprintf(dump_file,"  Responses Sent:             %lu\n", arp_responses_sent);
    fprintf(dump_file,"  Requests Received:          %lu\n", arp_requests_received);
    fprintf(dump_file,"  Responses Received:         %lu\n", arp_responses_received);
    fprintf(dump_file,"\nPacket Pool Statistics:\n");
    fprintf(dump_file,"  Allocations:                %lu\n", packet_allocations);
    fprintf(dump_file,"  Releases:                   %lu\n", packet_releases);
    fprintf(dump_file,"  Empty Allocation Requests:  %lu\n", empty_allocations);
    fprintf(dump_file,"  Invalid Releases:           %lu\n", invalid_releases);
    fprintf(dump_file,"\nPing Statistics:\n");
    fprintf(dump_file,"  Pings Sent:                 %lu\n", pings_sent);
    fprintf(dump_file,"  Ping Responses:             %lu\n", ping_responses);
    fprintf(dump_file,"\nIP Statistics:\n");
    fprintf(dump_file,"  Packets Sent:               %lu\n", ip_packets_sent);
    fprintf(dump_file,"  Total Bytes Sent:           %lu\n", ip_bytes_sent);
    fprintf(dump_file,"  Packets Received:           %lu\n", ip_packets_received);
    fprintf(dump_file,"  Total Bytes Received:       %lu\n", ip_bytes_received);
    fprintf(dump_file,"\nTCP Statistics:\n");
    fprintf(dump_file,"  Client Connections:         %lu\n", tcp_client_connections);
    fprintf(dump_file,"  Server Connections:         %lu\n", tcp_server_connections);
    fprintf(dump_file,"  Packets Sent:               %lu\n", tcp_packets_sent);
    fprintf(dump_file,"  Total Bytes Sent:           %lu\n", tcp_bytes_sent);
    fprintf(dump_file,"  Packets Received:           %lu\n", tcp_packets_received);
    fprintf(dump_file,"  Total Bytes Received:       %lu\n", tcp_bytes_received);
    fprintf(dump_file,"\nUDP Statistics:\n");
    fprintf(dump_file,"  Packets Sent:               %lu\n", udp_packets_sent);
    fprintf(dump_file,"  Total Bytes Sent:           %lu\n", udp_bytes_sent);
    fprintf(dump_file,"  Packets Received:           %lu\n", udp_packets_received);
    fprintf(dump_file,"  Total Bytes Received:       %lu\n", udp_bytes_received);
    fprintf(dump_file,"\n\n");

	fprintf(dump_file,"************** Popular Services *****************************\n");
	fprintf(dump_file,"   Service ID        Count \n");
	fprintf(dump_file,"\n");

	tml_system_event_popularity_get(&popularity_list, &list_size);

	for (i = 0; i < list_size; i++)
	{

		event_id =  popularity_list[i].tml_event_popularity_event_id;
		if (event_id < 438)
			fprintf(dump_file,"%s(%5d)", tml_event_type[event_id], event_id);
		else
            fprintf(dump_file,"USER EVENT             (%5d)", event_id);
        fprintf(dump_file,"        %8lu\n", popularity_list[i].tml_event_popularity_event_count);
	}
	fprintf(dump_file,"\n\n");
	free(popularity_list);

    fprintf(dump_file,"*************************************************************************************************************************************** Trace Events ********************************************************************************************************************************************************\n");
    fprintf(dump_file,"  Event Number     Relative Time                   Event                          Context                        Priority                Next Context                    Info1           Info2           Info3           Info4        Raw Time Stamp    Priority Inversion   Raw Event        Raw Priority\n\n");

	for (i = 0; i < total_events; i++)
	{

		/* Get the event.  */
		status =  tml_event_get(i, &event_context, &event_id,
									&event_thread_priority, &event_time_stamp,
									&event_info_1, &event_info_2, 
									&event_info_3, &event_info_4,
									&event_relative_ticks, 
									&event_next_context, 
									&thread_index, &priority_inversion, &bad_priority_inversion);

		/* Make sure the status is good.  */
		if (status)
		{

			fprintf(dump_file,"Error reading event %d\n", i);
			exit(1);
		}

		/* Print the event out.  */
		fprintf(dump_file,"%8lu", i);

		/* Print the relative time out.  */
		tml_time_to_ascii(event_relative_ticks, time_string);
		fprintf(dump_file,"        %20s", time_string);

		/* Print the event out.  */
		if (event_id < 1000)
			fprintf(dump_file,"    %s (%5d)", tml_event_type[event_id], event_id);
		else
            fprintf(dump_file,"    USER EVENT              (%5d)", event_id);

		/* Print out the context.  */
		if (event_context == 0xF0F0F0F0)
			fprintf(dump_file,"       Initialization             ");
		else if (event_context == 0xFFFFFFFF)
			fprintf(dump_file,"       Interrupt                  ");
		else if (event_context == 0x00000000)
			fprintf(dump_file,"       Idle                       ");
		else
		{

			/* Pickup the thread information based on the thread index.  */
			status =  tml_object_thread_get(thread_index, &object_name, &object_address,
										&parameter_1, &parameter_2, &lowest_priority, &highest_priority);

			/* Make sure the status is good.  */
			if (status)
			{

				/* Pickup the thread information based on the thread index.  */
				status =  tml_object_thread_get(thread_index, &object_name, &object_address,
										&parameter_1, &parameter_2, &lowest_priority, &highest_priority);

				fprintf(dump_file,"Error getting thread %d\n", i);
				exit(2);
			}

			fprintf(dump_file, "       ");
			for (j = 0; j < 16; j++)
			{
				if (object_name[j])
					fprintf(dump_file, "%c", object_name[j]);
				else
					break;
			}
			fprintf(dump_file, " (%08lX)", object_address);
			for (;j < 16; j++)
				fprintf(dump_file, " ");
		}

		/* Print the priority out.  */
		if ((event_context != 0xF0F0F0F0) && (event_context != 0xFFFFFFFF))
        {
    
            /* Pickup the preemption-threshold.  */    
            tml_event_preemption_threshold_get(i, &event_preemption_threshold);
        
            /* Is there a valid preemption-threshold?  */
            if (event_preemption_threshold == 0xFFFFFFFF)
                fprintf(dump_file,"    %8lu (   ?)    ", event_thread_priority);
            else
                fprintf(dump_file,"    %8lu (%4lu)    ", event_thread_priority, event_preemption_threshold);
        }
		else if (event_context == 0xFFFFFFFF)
			fprintf(dump_file,"    %08lX    ", event_thread_priority);
		else
			fprintf(dump_file,"           -    ");
		
		/* Print out the next context.  */
		if (event_next_context == 0xF0F0F0F0)
			fprintf(dump_file,"       Initialization             ");
		else if (event_next_context == 0xFFFFFFFF)
			fprintf(dump_file,"       Interrupt                  ");
		else if (event_next_context == 0)
			fprintf(dump_file,"       Idle System                ");
		else
		{

			/* Find the next thread context.  */
			status =  tml_object_by_address_find(event_next_context, &object_index);

			/* Check to see if we found it.  */
			if (status == 0)
			{

				/* Now get the name of the next context thread.  */
				tml_object_get(object_index, &object_name, &object_address,
										&parameter_1, &parameter_2);

				fprintf(dump_file, "       ");
				for (j = 0; j < 16; j++)
				{
					if (object_name[j])
						fprintf(dump_file, "%c", object_name[j]);
					else
						break;
				}
				fprintf(dump_file, " (%08lX)", object_address);
				for (;j < 16; j++)
					fprintf(dump_file, " ");

			}
			else
			{
				/* No Name, just print the address.  */
				fprintf(dump_file,"       No Name Found (%08lX)    ", object_address);
			}
		}

		/* Print the info fields.  */
		fprintf(dump_file,"    I1: %08lX", event_info_1);
		fprintf(dump_file,"    I2: %08lX", event_info_2);
		fprintf(dump_file,"    I3: %08lX", event_info_3);
		fprintf(dump_file,"    I4: %08lX", event_info_4);

		/* Print the raw time stamp.  */
		fprintf(dump_file,"        %08lX", event_time_stamp);

		/* Print the preemption fields.  */
		fprintf(dump_file,"            %d-%d", priority_inversion, bad_priority_inversion);

        /* Print the raw event ID field.  */
        if (event_id != TML_TRACE_RUNNING)
        fprintf(dump_file,"             %08lX", tml_event_array[i].tml_event_raw_id);
        else
            fprintf(dump_file,"             --------");
        
        /* Print the raw priority field.  */
        if (event_id != TML_TRACE_RUNNING)
        fprintf(dump_file,"          %08lX", tml_event_array[i].tml_event_raw_priority);
        else
            fprintf(dump_file,"          --------");

		/* Print a new line.  */
		fprintf(dump_file,"\n");
	}
	
    fprintf(dump_file,"**************************************************************************************************************************************** Trace Done ********************************************************************************************************************************************************\n\n");

    /* Return success!  */
    return(0);
}


int  tml_convert_file(FILE *source_file, FILE **new_file)
{

int		in_byte;
int		in_byte2;
int		out_byte;
int		length;
int		record_type;
int		address_bytes;
int		i;
FILE    *converted_file;

    /* Set the new file to NULL.  */
    *new_file =  NULL;

    /* Attempt to open the new file for writing.  */
    converted_file =  fopen("trxtempfile501.txt", "wb+");

    /* Determine if the new file was opened properly.  */
    if (converted_file == NULL)
    {

        /* If not, simply return an error.  */
        return(1);
    }

    /* Position to the beginning of the source file.  */
    fseek(source_file, 0, SEEK_SET);

	/* Read the first byte of the file, it must be a ":" character.  */	
	in_byte =  fgetc(source_file);

	/* At this point, loop to convert the source hex file into a raw binary TraceX file.  */
	while (in_byte == (int) ':')
	{
		
		/* Read the length of the record.  */
		in_byte =  fgetc(source_file);
		in_byte2 = fgetc(source_file);

		/* Check for end of file.  */
		if ((in_byte == EOF) || (in_byte2 == EOF))
	    {

			fclose(converted_file);
			return(4);
		}
			
		/* Compute the length.  */
		in_byte =  tml_convert_from_ascii(in_byte);
		in_byte2 = tml_convert_from_ascii(in_byte2);

		/* Check for error condition.  */
		if ((in_byte == 0xFFFF) || (in_byte2 == 0xFFFF))
	    {

			fclose(converted_file);
			return(5);
		}

		/* Calculate the length.  */
		length =  (in_byte * 16) + in_byte2;

		/* Read the next four bytes of address - these are simply 
		   thrown away.  */
		in_byte =  fgetc(source_file);
		in_byte2 = fgetc(source_file);
		in_byte =  fgetc(source_file);
		in_byte2 = fgetc(source_file);

		/* Now read the type of HEX record.  */
		in_byte =  fgetc(source_file);
		in_byte2 = fgetc(source_file);

		/* Check for end of file.  */
		if ((in_byte == EOF) || (in_byte2 == EOF))
	    {

			fclose(converted_file);
			return(6);
		}

		/* Look for the end of HEX code.  */
		if ((in_byte == (int) '0') && (in_byte2 == (int) '1'))
		{

            /* Setup the new file.  */
            *new_file =  converted_file;
			return(0);
		}

		/* Determine if this is a data record.  */
		if ((in_byte == (int) '0') && (in_byte2 == (int) '0'))
		{

			/* Yup, this is a data record. Loop through the 
			   input file and write the raw data out to the 
			   new file.  */
			for (i = 0; i < length; i++)
			{

				/* Get two bytes since two ASCII characters makes 
				   one binary byte.  */
				in_byte =   fgetc(source_file);
				in_byte2 =  fgetc(source_file);

				/* Determine if an EOF is present.  */
				if ((in_byte == EOF) || (in_byte2 == EOF))
			    {

					fclose(converted_file);
					return(7);
				}

                /* Convert the character to raw binary.  */
				in_byte =   tml_convert_from_ascii(in_byte);
				in_byte2 =  tml_convert_from_ascii(in_byte2);

				/* Determine if it is invalid.  */
				if ((in_byte == 0xFFFF) || (in_byte2 == 0xFFFF))
			    {

					fclose(converted_file);
					return(8);
				}

				/* Calculate the output byte.  */
				out_byte =  (in_byte * 16) + in_byte2;

				/* Write the raw character out to the converted file.  */
				fputc((int) (0xFF & out_byte), converted_file);
			}
		}

		/* At this point, loop until we have a another start record.  */
		do
		{

			/* Get a byte.  */
			in_byte =  fgetc(source_file);
		} while ((in_byte != EOF) && (in_byte != (int) ':'));
	}

	/* At this point, loop to convert the source mot file into a raw binary TraceX file.  */
	while (in_byte == (int) 'S')
	{

		/* Get the record type.  */
		record_type =  fgetc(source_file);

		/* Check for end of file.  */
		if (record_type == EOF)
	    {

			fclose(converted_file);
			return(9);
		}
		
		/* Read the length of the record.  */
		in_byte =  fgetc(source_file);
		in_byte2 = fgetc(source_file);

		/* Check for end of file.  */
		if ((in_byte == EOF) || (in_byte2 == EOF))
	    {

			fclose(converted_file);
			return(10);
		}
			
		/* Compute the length.  */
		in_byte =  tml_convert_from_ascii(in_byte);
		in_byte2 = tml_convert_from_ascii(in_byte2);

		/* Check for error condition.  */
		if ((in_byte == 0xFFFF) || (in_byte2 == 0xFFFF))
	    {

			fclose(converted_file);
			return(11);
		}

		/* Calculate the length.  */
		length =  (in_byte * 16) + in_byte2;

		/* Determine the how to adjust the length.  */
		if (record_type == (int) '1')
			address_bytes =  2;
		else if (record_type == (int) '2')
			address_bytes =  3;
		else if (record_type == (int) '3')
			address_bytes =  4;
		else
			address_bytes =  2;
		
		/* Adjust the length to account for the address bytes and
		   the checksum byte.  */
		length =  length - (address_bytes + 1);


		/* Read and throw away the address bytes  - these are simply 
		   thrown away.  */
		for (i = 0; i < address_bytes; i++)
		{
			/* Read one byte - two digits.  */
			in_byte =  fgetc(source_file);
			in_byte2 = fgetc(source_file);
		}

		/* Look for the end of mot code.  */
		if ((record_type == (int) '8') || (record_type == (int) '9') || 
			(record_type == (int) '7'))
		{
		
            /* Return a good status and the new file pointer.  */
            *new_file =  converted_file;
			return(0);
		}

		/* Determine if this is a data record.  */
		if ((record_type == (int) '1') || (record_type == (int) '2') || 
			(record_type == (int) '3'))
		{

			/* Yup, this is a data record. Loop through the 
			   input file and write the raw data out to the 
			   new file.  */
			for (i = 0; i < length; i++)
			{

				/* Get two bytes since two ASCII characters makes 
				   one binary byte.  */
				in_byte =   fgetc(source_file);
				in_byte2 =  fgetc(source_file);

				/* Determine if an EOF is present.  */
				if ((in_byte == EOF) || (in_byte2 == EOF))
			    {

					fclose(converted_file);
					return(13);
				}

                /* Convert the character to raw binary.  */
				in_byte =   tml_convert_from_ascii(in_byte);
				in_byte2 =  tml_convert_from_ascii(in_byte2);

				/* Determine if it is invalid.  */
				if ((in_byte == 0xFFFF) || (in_byte2 == 0xFFFF))
			    {

					fclose(converted_file);
					return(14);
				}

				/* Calculate the output byte.  */
				out_byte =  (in_byte * 16) + in_byte2;

				/* Write the raw character out to the converted file.  */
				fputc((int) (0xFF & out_byte), converted_file);
			}
		}

		/* At this point, loop until we have a another start record.  */
		do
		{

			/* Get a byte.  */
			in_byte =  fgetc(source_file);
		} while ((in_byte != EOF) && (in_byte != (int) 'S'));
	}

    /* Look for RAW ascii HEX dump...  */
    do
    {
    
        /* Determine if the character is CR or LF.  */
        while ((in_byte == 0x0D) || (in_byte == 0x0A))
        {
        
            /* Get the next character.  */
            in_byte =  fgetc(source_file);
        }

        /* Pickup the second nibble we need to make a byte.  */
        in_byte2 =  fgetc(source_file);
    
        /* Check for EOF.  */
        if ((in_byte == EOF) || (in_byte2 == EOF))
        {
        
            /* Assume all is good.  */
            *new_file =  converted_file;
            return(0);
        }
    
        /* Determine if the character is valid.  */
        if (((in_byte  < '0') || (in_byte  > '9')) &&
            ((in_byte  < 'A') || (in_byte  > 'F')) &&
            ((in_byte  < 'a') || (in_byte  > 'f')) &&
            ((in_byte2 < '0') || (in_byte2 > '9')) &&
            ((in_byte2 < 'A') || (in_byte2 > 'F')) &&
            ((in_byte2 < 'a') || (in_byte2 > 'f')))
        {
        
            /* Invalid character... get out!  */
            break;
        }
        
        /* Valid characters... */ 
            
        /* Convert the characters to raw binary.  */
        in_byte =   tml_convert_from_ascii(in_byte);
        in_byte2 =  tml_convert_from_ascii(in_byte2);

        /* Determine if it is invalid.  */
        if ((in_byte == 0xFFFF) || (in_byte2 == 0xFFFF))
        {

            /* Error!  */
            break;
        }

        /* Calculate the output byte.  */
        out_byte =  (in_byte * 16) + in_byte2;

        /* Write out the raw character.  */
        fputc((int) (0xFF & out_byte), converted_file);
        
        /* Get the next character.  */
        in_byte =  fgetc(source_file);
    } while (1);

    /* Close temporary file and return error.  */
	fclose(converted_file);
    return(15);
}


int  tml_convert_from_ascii(int byte)
{

int		number;

	/* Determine if the number is in the numeric ASCII range.  */
	if ((byte >= (int) '0') && (byte <= '9'))
	{

		number =  byte - (int) '0';
	}
	else if ((byte >= (int) 'A') && (byte <= 'F'))
	{

		number =  10 + (byte - (int) 'A');
	}
	else if ((byte >= (int) 'a') && (byte <= 'f'))
	{

		number =  10 + (byte - (int) 'a');
	}
	else
	{

		/* Invalid HEX digit - return all FFFFs!  */
		number = 0xFFFF;
	}

	return(number);
}
		
