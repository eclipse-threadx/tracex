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


/* Include necessary system files.  */

#ifndef TML_LIBRARY_H
#define TML_LIBRARY_H


#include <stdio.h>
#include <stdlib.h>

#include "ELTMLManaged.h"

/* Define the typedefs and constants for the Trace Management Library (TML).  */


/* Define the trace buffer ID.  */

#define TML_TRACE_VALID_BE                                   0x54585442UL
#define TML_TRACE_VALID_LE                                   0x42545854UL


/* Define the object name types as well as the contents of any additional parameters that might be useful in 
   trace analysis.  */

#define TML_TRACE_OBJECT_TYPE_NOT_VALID                      0           /* Object is not valid														*/ 
#define TML_TRACE_OBJECT_TYPE_THREAD                         1           /* P1 = stack start address, P2 = stack size        						*/
#define TML_TRACE_OBJECT_TYPE_TIMER                          2           /* P1 = initial ticks, P2 = reschedule ticks								*/ 
#define TML_TRACE_OBJECT_TYPE_QUEUE                          3           /* P1 = queue size, P2 = message size                                      */ 
#define TML_TRACE_OBJECT_TYPE_SEMAPHORE                      4           /* P1 = initial instances													*/ 
#define TML_TRACE_OBJECT_TYPE_MUTEX                          5           /* P1 = priority inheritance flag											*/ 
#define TML_TRACE_OBJECT_TYPE_EVENT_FLAGS                    6           /* none																	*/ 
#define TML_TRACE_OBJECT_TYPE_BLOCK_POOL                     7           /* P1 = total blocks, P2 = block size                                      */ 
#define TML_TRACE_OBJECT_TYPE_BYTE_POOL                      8           /* P1 = total bytes														*/ 
#define TML_TRACE_OBJECT_TYPE_MEDIA                          9           /* none                                                                    */ 
#define TML_TRACE_OBJECT_TYPE_FILE                           10          /* none                                                                    */ 
#define TML_TRACE_OBJECT_TYPE_IP                             11          /* P1 = stack start address, P2 = stack size                               */ 
#define TML_TRACE_OBJECT_TYPE_PACKET_POOL                    12          /* P1 = packet size, P2 = number of packets                                */ 
#define TML_TRACE_OBJECT_TYPE_TCP_SOCKET                     13          /* P1 = type of service, P2 = window size                                  */ 
#define TML_TRACE_OBJECT_TYPE_UDP_SOCKET                     14          /* P1 = type of service, P2 = receive queue maximum                        */ 



/* Define the basic ThreadX thread scheduling events first.  */

#define TML_TRACE_THREAD_RESUME                              1           /* I1 = thread ptr, I2 = previous_state, I3 = stack ptr, I4 = next thread   */ 
#define TML_TRACE_THREAD_SUSPEND                             2           /* I1 = thread ptr, I2 = new_state, I3 = stack ptr  I4 = next thread        */ 
#define TML_TRACE_ISR_ENTER                                  3           /* I1 = stack_ptr, I2 = ISR number, I3 = system state, I4 = current thread  */ 
#define TML_TRACE_ISR_EXIT                                   4           /* I1 = stack_ptr, I2 = ISR number, I3 = system state, I4 = preempt disable */ 
#define TML_TRACE_TIME_SLICE								 5			 /* I1 = next thread ptr, I2 = system state, I3 = preempt disable, I4 = stack*/
#define TML_TRACE_RUNNING                                    6           /* None																	 */ 


/* Define the rest of the ThreadX system events.  */

#define TML_TRACE_BLOCK_ALLOCATE                             10          /* I1 = pool ptr, I2 = memory ptr, I3 = wait option, I4 = remaining blocks  */ 
#define TML_TRACE_BLOCK_POOL_CREATE                          11          /* I1 = pool ptr, I2 = pool_start, I3 = total blocks, I4 = block size       */ 
#define TML_TRACE_BLOCK_POOL_DELETE                          12          /* I1 = pool ptr, I2 = stack ptr                                            */ 
#define TML_TRACE_BLOCK_POOL_INFO_GET                        13          /* I1 = pool ptr                                                            */
#define TML_TRACE_BLOCK_POOL_PERFORMANCE_INFO_GET            14          /* I1 = pool ptr                                                            */ 
#define TML_TRACE_BLOCK_POOL_PERFORMANCE_SYSTEM_INFO_GET     15          /* None                                                                     */ 
#define TML_TRACE_BLOCK_POOL_PRIORITIZE                      16          /* I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
#define TML_TRACE_BLOCK_RELEASE                              17          /* I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = stack ptr           */ 
#define TML_TRACE_BYTE_ALLOCATE                              20          /* I1 = pool ptr, I2 = memory ptr, I3 = size requested, I4 = wait option    */ 
#define TML_TRACE_BYTE_POOL_CREATE                           21          /* I1 = pool ptr, I2 = start ptr, I3 = pool size, I4 = stack ptr            */ 
#define TML_TRACE_BYTE_POOL_DELETE                           22          /* I1 = pool ptr, I2 = stack ptr                                            */ 
#define TML_TRACE_BYTE_POOL_INFO_GET                         23          /* I1 = pool ptr                                                            */
#define TML_TRACE_BYTE_POOL_PERFORMANCE_INFO_GET             24          /* I1 = pool ptr                                                            */
#define TML_TRACE_BYTE_POOL_PERFORMANCE_SYSTEM_INFO_GET      25          /* None                                                                     */
#define TML_TRACE_BYTE_POOL_PRIORITIZE                       26          /* I1 = pool ptr, I2 = suspended count, I3 = stack ptr                      */
#define TML_TRACE_BYTE_RELEASE                               27          /* I1 = pool ptr, I2 = memory ptr, I3 = suspended, I4 = available bytes     */ 
#define TML_TRACE_EVENT_FLAGS_CREATE                         30          /* I1 = group ptr, I2 = stack ptr                                           */ 
#define TML_TRACE_EVENT_FLAGS_DELETE                         31          /* I1 = group ptr, I2 = stack ptr                                           */ 
#define TML_TRACE_EVENT_FLAGS_GET                            32          /* I1 = group ptr, I2 = requested flags, I3 = current flags, I4 = get option*/ 
#define TML_TRACE_EVENT_FLAGS_INFO_GET                       33          /* I1 = group ptr                                                           */
#define TML_TRACE_EVENT_FLAGS_PERFORMANCE_INFO_GET           34          /* I1 = group ptr                                                           */
#define TML_TRACE_EVENT_FLAGS_PERFORMANCE_SYSTEM_INFO_GET    35          /* None                                                                     */
#define TML_TRACE_EVENT_FLAGS_SET                            36          /* I1 = group ptr, I2 = flags to set, I3 = set option, I4= suspended count  */
#define TML_TRACE_EVENT_FLAGS_SET_NOTIFY                     37          /* I1 = group ptr                                                           */
#define TML_TRACE_INTERRUPT_CONTROL                          40          /* I1 = new interrupt posture, I2 = stack ptr                               */
#define TML_TRACE_MUTEX_CREATE                               50          /* I1 = mutex ptr, I2 = inheritance, I3 = stack ptr                         */
#define TML_TRACE_MUTEX_DELETE                               51          /* I1 = mutex ptr, I2 = stack ptr                                           */
#define TML_TRACE_MUTEX_GET                                  52          /* I1 = mutex ptr, I2 = wait option, I3 = owning thread, I4 = own count     */
#define TML_TRACE_MUTEX_INFO_GET                             53          /* I1 = mutex ptr                                                           */
#define TML_TRACE_MUTEX_PERFORMANCE_INFO_GET                 54          /* I1 = mutex ptr                                                           */
#define TML_TRACE_MUTEX_PERFORMANCE_SYSTEM_INFO_GET          55          /* None                                                                     */
#define TML_TRACE_MUTEX_PRIORITIZE                           56          /* I1 = mutex ptr, I2 = suspended count, I3 = stack ptr                     */
#define TML_TRACE_MUTEX_PUT                                  57          /* I1 = mutex ptr, I2 = owning thread, I3 = own count, I4 = stack ptr       */
#define TML_TRACE_QUEUE_CREATE                               60          /* I1 = queue ptr, I2 = message size, I3 = queue start, I4 = queue size     */
#define TML_TRACE_QUEUE_DELETE                               61          /* I1 = queue ptr, I2 = stack ptr                                           */
#define TML_TRACE_QUEUE_FLUSH                                62          /* I1 = queue ptr, I2 = stack ptr                                           */
#define TML_TRACE_QUEUE_FRONT_SEND                           63          /* I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued         */
#define TML_TRACE_QUEUE_INFO_GET                             64          /* I1 = queue ptr                                                           */
#define TML_TRACE_QUEUE_PERFORMANCE_INFO_GET                 65          /* I1 = queue ptr                                                           */
#define TML_TRACE_QUEUE_PERFORMANCE_SYSTEM_INFO_GET          66          /* None                                                                     */
#define TML_TRACE_QUEUE_PRIORITIZE                           67          /* I1 = queue ptr, I2 = suspended count, I3 = stack ptr                     */
#define TML_TRACE_QUEUE_RECEIVE                              68          /* I1 = queue ptr, I2 = destination ptr, I3 = wait option, I4 = enqueued    */
#define TML_TRACE_QUEUE_SEND                                 69          /* I1 = queue ptr, I2 = source ptr, I3 = wait option, I4 = enqueued         */
#define TML_TRACE_QUEUE_SEND_NOTIFY                          70          /* I1 = queue ptr                                                           */
#define TML_TRACE_SEMAPHORE_CEILING_PUT                      80          /* I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4 =ceiling */
#define TML_TRACE_SEMAPHORE_CREATE                           81          /* I1 = semaphore ptr, I2 = initial count, I3 = stack ptr                   */
#define TML_TRACE_SEMAPHORE_DELETE                           82          /* I1 = semaphore ptr, I2 = stack ptr                                       */
#define TML_TRACE_SEMAPHORE_GET                              83          /* I1 = semaphore ptr, I2 = wait option, I3 = current count, I4 = stack ptr */
#define TML_TRACE_SEMAPHORE_INFO_GET                         84          /* I1 = semaphore ptr                                                       */
#define TML_TRACE_SEMAPHORE_PERFORMANCE_INFO_GET             85          /* I1 = semaphore ptr                                                       */
#define TML_TRACE_SEMAPHORE_PERFORMANCE_SYSTEM_INFO_GET      86          /* None                                                                     */
#define TML_TRACE_SEMAPHORE_PRIORITIZE                       87          /* I1 = semaphore ptr, I2 = suspended count, I2 = stack ptr                 */
#define TML_TRACE_SEMAPHORE_PUT                              88          /* I1 = semaphore ptr, I2 = current count, I3 = suspended count,I4=stack ptr*/
#define TML_TRACE_SEMAPHORE_PUT_NOTIFY                       89          /* I1 = semaphore ptr                                                       */
#define TML_TRACE_THREAD_CREATE                              100         /* I1 = thread ptr, I2 = priority, I3 = stack ptr, I4 = stack_size          */
#define TML_TRACE_THREAD_DELETE                              101         /* I1 = thread ptr, I2 = stack ptr                                          */
#define TML_TRACE_THREAD_ENTRY_EXIT_NOTIFY                   102         /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
#define TML_TRACE_THREAD_IDENTIFY                            103         /* None                                                                     */
#define TML_TRACE_THREAD_INFO_GET                            104         /* I1 = thread ptr, I2 = thread state                                       */
#define TML_TRACE_THREAD_PERFORMANCE_INFO_GET                105         /* I1 = thread ptr, I2 = thread state                                       */
#define TML_TRACE_THREAD_PERFORMANCE_SYSTEM_INFO_GET         106         /* None                                                                     */
#define TML_TRACE_THREAD_PREEMPTION_CHANGE                   107         /* I1 = thread ptr, I2 = new threshold, I3 = old threshold, I4 =thread state*/
#define TML_TRACE_THREAD_PRIORITY_CHANGE                     108         /* I1 = thread ptr, I2 = new priority, I3 = old priority, I4 = thread state */
#define TML_TRACE_THREAD_RELINQUISH                          109         /* I1 = stack ptr, I2 = next thread                                         */
#define TML_TRACE_THREAD_RESET                               110         /* I1 = thread ptr, I2 = thread state                                       */
#define TML_TRACE_THREAD_RESUME_API                          111         /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
#define TML_TRACE_THREAD_SLEEP                               112         /* I1 = sleep value, I2 = thread state, I3 = stack ptr                      */
#define TML_TRACE_THREAD_STACK_ERROR_NOTIFY                  113         /* None                                                                     */
#define TML_TRACE_THREAD_SUSPEND_API                         114         /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
#define TML_TRACE_THREAD_TERMINATE                           115         /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
#define TML_TRACE_THREAD_TIME_SLICE_CHANGE                   116         /* I1 = thread ptr, I2 = new timeslice, I3 = old timeslice                  */
#define TML_TRACE_THREAD_WAIT_ABORT                          117         /* I1 = thread ptr, I2 = thread state, I3 = stack ptr                       */
#define TML_TRACE_TIME_GET                                   120         /* I1 = current time, I2 = stack ptr                                        */
#define TML_TRACE_TIME_SET                                   121         /* I1 = new time                                                            */
#define TML_TRACE_TIMER_ACTIVATE                             122         /* I1 = timer ptr                                                           */
#define TML_TRACE_TIMER_CHANGE                               123         /* I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks                 */ 
#define TML_TRACE_TIMER_CREATE                               124         /* I1 = timer ptr, I2 = initial ticks, I3= reschedule ticks, I4 = enable    */
#define TML_TRACE_TIMER_DEACTIVATE                           125         /* I1 = timer ptr, I2 = stack ptr                                           */
#define TML_TRACE_TIMER_DELETE                               126         /* I1 = timer ptr                                                           */
#define TML_TRACE_TIMER_INFO_GET                             127         /* I1 = timer ptr, I2 = stack ptr                                           */
#define TML_TRACE_TIMER_PERFORMANCE_INFO_GET                 128         /* I1 = timer ptr                                                           */
#define TML_TRACE_TIMER_PERFORMANCE_SYSTEM_INFO_GET          129         /* None                                                                     */

#define TML_FX_TRACE_INTERNAL_LOG_SECTOR_CACHE_MISS          201         /* I1 = media ptr, I2 = sector, I3 = total misses, I4 = cache size          */
#define TML_FX_TRACE_INTERNAL_DIR_CACHE_MISS                 202         /* I1 = media ptr, I2 = total misses                                        */
#define TML_FX_TRACE_INTERNAL_MEDIA_FLUSH                    203         /* I1 = media ptr, I2 = dirty sectors                                       */
#define TML_FX_TRACE_INTERNAL_DIR_ENTRY_READ                 204         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_INTERNAL_DIR_ENTRY_WRITE                205         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_READ                 206         /* I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer         */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_WRITE                207         /* I1 = media ptr, I2 = sector, I3 = number of sectors, I4 = buffer         */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_FLUSH                208         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_ABORT                209         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_INIT                 210         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_BOOT_READ            211         /* I1 = media ptr, I2 = buffer                                              */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_RELEASE_SECTORS      212         /* I1 = media ptr, I2 = sector, I3 = number of sectors                      */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_BOOT_WRITE           213         /* I1 = media ptr, I2 = buffer                                              */
#define TML_FX_TRACE_INTERNAL_IO_DRIVER_UNINIT               214         /* I1 = media ptr                                                           */
#define TML_FX_TRACE_DIRECTORY_ATTRIBUTES_READ               220         /* I1 = media ptr, I2 = directory name, I3 = attributes                     */ 
#define TML_FX_TRACE_DIRECTORY_ATTRIBUTES_SET                221         /* I1 = media ptr, I2 = directory name, I3 = attributes                     */ 
#define TML_FX_TRACE_DIRECTORY_CREATE                        222         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_DEFAULT_GET                   223         /* I1 = media ptr, I2 = return path name                                    */ 
#define TML_FX_TRACE_DIRECTORY_DEFAULT_SET                   224         /* I1 = media ptr, I2 = new path name                                       */ 
#define TML_FX_TRACE_DIRECTORY_DELETE                        225         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_FIRST_ENTRY_FIND              226         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_FIRST_FULL_ENTRY_FIND         227         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_INFORMATION_GET               228         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_LOCAL_PATH_CLEAR              229         /* I1 = media ptr                                                           */ 
#define TML_FX_TRACE_DIRECTORY_LOCAL_PATH_GET                230         /* I1 = media ptr, I2 = return path name                                    */ 
#define TML_FX_TRACE_DIRECTORY_LOCAL_PATH_RESTORE            231         /* I1 = media ptr, I2 = local path ptr                                      */ 
#define TML_FX_TRACE_DIRECTORY_LOCAL_PATH_SET                232         /* I1 = media ptr, I2 = local path ptr, I3 = new path name                  */ 
#define TML_FX_TRACE_DIRECTORY_LONG_NAME_GET                 233         /* I1 = media ptr, I2 = short file name, I3 = long file name                */ 
#define TML_FX_TRACE_DIRECTORY_NAME_TEST                     234         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_NEXT_ENTRY_FIND               235         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_NEXT_FULL_ENTRY_FIND          236         /* I1 = media ptr, I2 = directory name                                      */ 
#define TML_FX_TRACE_DIRECTORY_RENAME                        237         /* I1 = media ptr, I2 = old directory name, I3 = new directory name         */
#define TML_FX_TRACE_DIRECTORY_SHORT_NAME_GET                238         /* I1 = media ptr, I2 = long file name, I3 = short file name                */ 
#define TML_FX_TRACE_FILE_ALLOCATE                           239         /* I1 = file ptr, I2 = size I3 = previous size, I4 = new size               */ 
#define TML_FX_TRACE_FILE_ATTRIBUTES_READ                    240         /* I1 = media ptr, I2 = file name, I3 = attributes                          */ 
#define TML_FX_TRACE_FILE_ATTRIBUTES_SET                     241         /* I1 = media ptr, I2 = file name, I3 = attributes                          */ 
#define TML_FX_TRACE_FILE_BEST_EFFORT_ALLOCATE               242         /* I1 = file ptr, I2 = size, I3 = actual_size_allocated                     */ 
#define TML_FX_TRACE_FILE_CLOSE                              243         /* I1 = file ptr, I3 = file size                                            */ 
#define TML_FX_TRACE_FILE_CREATE                             244         /* I1 = media ptr, I2 = file name                                           */ 
#define TML_FX_TRACE_FILE_DATE_TIME_SET                      245         /* I1 = media ptr, I2 = file name, I3 = year, I4 = month                    */ 
#define TML_FX_TRACE_FILE_DELETE                             246         /* I1 = media ptr, I2 = file name                                           */ 
#define TML_FX_TRACE_FILE_OPEN                               247         /* I1 = media ptr, I2 = file ptr, I3 = file name, I4 = open type            */ 
#define TML_FX_TRACE_FILE_READ                               248         /* I1 = file ptr, I2 = buffer ptr, I3 = request size I4 = actual size       */ 
#define TML_FX_TRACE_FILE_RELATIVE_SEEK                      249         /* I1 = file ptr, I2 = byte offset, I3 = seek from, I4 = previous offset    */ 
#define TML_FX_TRACE_FILE_RENAME                             250         /* I1 = media ptr, I2 = old file name, I3 = new file name                   */ 
#define TML_FX_TRACE_FILE_SEEK                               251         /* I1 = file ptr, I2 = byte offset, I3 = previous offset                    */ 
#define TML_FX_TRACE_FILE_TRUNCATE                           252         /* I1 = file ptr, I2 = size, I3 = previous size, I4 = new size              */ 
#define TML_FX_TRACE_FILE_TRUNCATE_RELEASE                   253         /* I1 = file ptr, I2 = size, I3 = previous size, I4 = new size              */ 
#define TML_FX_TRACE_FILE_WRITE                              254         /* I1 = file ptr, I2 = buffer ptr, I3 = size, I4 = bytes written            */ 
#define TML_FX_TRACE_MEDIA_ABORT                             255         /* I1 = media ptr                                                           */ 
#define TML_FX_TRACE_MEDIA_CACHE_INVALIDATE                  256         /* I1 = media ptr                                                           */ 
#define TML_FX_TRACE_MEDIA_CHECK                             257         /* I1 = media ptr, I2 = scratch memory, I3 = scratch memory size, I4 =errors*/
#define TML_FX_TRACE_MEDIA_CLOSE                             258         /* I1 = media ptr                                                           */ 
#define TML_FX_TRACE_MEDIA_FLUSH                             259         /* I1 = media ptr                                                           */ 
#define TML_FX_TRACE_MEDIA_FORMAT                            260         /* I1 = media ptr, I2 = root entries, I3 = sectors, I4 = sectors per cluster*/
#define TML_FX_TRACE_MEDIA_OPEN                              261         /* I1 = media ptr, I2 = media driver, I3 = memory ptr, I4 = memory size     */ 
#define TML_FX_TRACE_MEDIA_READ                              262         /* I1 = media ptr, I2 = logical sector, I3 = buffer ptr, I4 = bytes read    */ 
#define TML_FX_TRACE_MEDIA_SPACE_AVAILABLE                   263         /* I1 = media ptr, I2 = available bytes ptr, I3 = available clusters        */ 
#define TML_FX_TRACE_MEDIA_VOLUME_GET                        264         /* I1 = media ptr, I2 = volume name, I3 = volume source                     */ 
#define TML_FX_TRACE_MEDIA_VOLUME_SET                        265         /* I1 = media ptr, I2 = volume name                                         */
#define TML_FX_TRACE_MEDIA_WRITE                             266         /* I1 = media ptr, I2 = logical_sector, I3 = buffer_ptr, I4 = byte written  */ 
#define TML_FX_TRACE_SYSTEM_DATE_GET                         267         /* I1 = year, I2 = month, I3 = day                                          */ 
#define TML_FX_TRACE_SYSTEM_DATE_SET                         268         /* I1 = year, I2 = month, I3 = day                                          */ 
#define TML_FX_TRACE_SYSTEM_INITIALIZE                       269         /* None                                                                     */ 
#define TML_FX_TRACE_SYSTEM_TIME_GET                         270         /* I1 = hour, I2 = minute, I3 = second                                      */ 
#define TML_FX_TRACE_SYSTEM_TIME_SET                         271         /* I1 = hour, I2 = minute, I3 = second                                      */ 
#define TML_FX_TRACE_UNICODE_DIRECTORY_CREATE                272         /* I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short_name */
#define TML_FX_TRACE_UNICODE_DIRECTORY_RENAME                273         /* I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new_name   */
#define TML_FX_TRACE_UNICODE_FILE_CREATE                     274         /* I1 = media ptr, I2 = source unicode, I3 = source length, I4 = short name */ 
#define TML_FX_TRACE_UNICODE_FILE_RENAME                     275         /* I1 = media ptr, I2 = source unicode, I3 = source length, I4 = new name   */ 
#define TML_FX_TRACE_UNICODE_LENGTH_GET                      276         /* I1 = unicode name, I2 = length                                           */ 
#define TML_FX_TRACE_UNICODE_NAME_GET                        277         /* I1 = media ptr, I2 = source short name, I3 = unicode name, I4 = length   */ 
#define TML_FX_TRACE_UNICODE_SHORT_NAME_GET                  278         /* I1 = media ptr, I2 = source unicode name, I3 = length, I4 =  short name  */ 

#define TML_NX_TRACE_INTERNAL_ARP_REQUEST_RECEIVE            300         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr                     */ 
#define TML_NX_TRACE_INTERNAL_ARP_REQUEST_SEND               301         /* I1 = ip ptr, I2 = destination IP address, I3 = packet ptr                */ 
#define TML_NX_TRACE_INTERNAL_ARP_RESPONSE_RECEIVE           302         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr                     */
#define TML_NX_TRACE_INTERNAL_ARP_RESPONSE_SEND              303         /* I1 = ip ptr, I2 = destination IP address, I3 = packet ptr                */
#define TML_NX_TRACE_INTERNAL_ICMP_RECEIVE                   304         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0 */
#define TML_NX_TRACE_INTERNAL_ICMP_SEND                      305         /* I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = header 0 */
#define TML_NX_TRACE_INTERNAL_IGMP_RECEIVE                   306         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = header word 0 */

#define TML_NX_TRACE_INTERNAL_IP_RECEIVE                     308         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = packet length */
#define TML_NX_TRACE_INTERNAL_IP_SEND                        309         /* I1 = ip ptr, I2 = destination IP address, I3 = packet ptr, I4 = length   */
#define TML_NX_TRACE_INTERNAL_TCP_DATA_RECEIVE               310         /* I1 = ip ptr, I2 = source IP address, I3 = packet ptr, I4 = sequence      */
#define TML_NX_TRACE_INTERNAL_TCP_DATA_SEND                  311         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_FIN_RECEIVE                312         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_FIN_SEND                   313         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_RESET_RECEIVE              314         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_RESET_SEND                 315         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_SYN_RECEIVE                316         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_TCP_SYN_SEND                   317         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = sequence             */
#define TML_NX_TRACE_INTERNAL_UDP_RECEIVE                    318         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = header word 0        */
#define TML_NX_TRACE_INTERNAL_UDP_SEND                       319         /* I1 = ip ptr, I2 = socket_ptr, I3 = packet ptr, I4 = header 0             */
#define TML_NX_TRACE_INTERNAL_RARP_RECEIVE                   320         /* I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1 */
#define TML_NX_TRACE_INTERNAL_RARP_SEND                      321         /* I1 = ip ptr, I2 = target IP address, I3 = packet ptr, I4 = header word 1 */
#define TML_NX_TRACE_INTERNAL_TCP_RETRY                      322         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = number of retries    */
#define TML_NX_TRACE_INTERNAL_TCP_STATE_CHANGE               323         /* I1 = ip ptr, I2 = socket ptr, I3 = previous state, I4 = new state        */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_PACKET_SEND          324         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_INITIALIZE           325         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_LINK_ENABLE          326         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_LINK_DISABLE         327         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_PACKET_BROADCAST     328         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_ARP_SEND             329         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_ARP_RESPONSE_SEND    330         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_RARP_SEND            331         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_MULTICAST_JOIN       332         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_MULTICAST_LEAVE      333         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_STATUS           334         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_SPEED            335         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_DUPLEX_TYPE      336         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_ERROR_COUNT      337         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_RX_COUNT         338         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_TX_COUNT         339         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_GET_ALLOC_ERRORS     340         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_UNINITIALIZE         341         /* I1 = ip ptr                                                              */
#define TML_NX_TRACE_INTERNAL_IO_DRIVER_DEFERRED_PROCESSING  342         /* I1 = ip ptr, I2 = packet ptr, I3 = packet size                           */
#define TML_NX_TRACE_ARP_DYNAMIC_ENTRIES_INVALIDATE          350         /* I1 = ip ptr, I2 = entries invalidated                                    */ 
#define TML_NX_TRACE_ARP_DYNAMIC_ENTRY_SET                   351         /* I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw       */ 
#define TML_NX_TRACE_ARP_ENABLE                              352         /* I1 = ip ptr, I2 = arp cache memory, I3 = arp cache size                  */ 
#define TML_NX_TRACE_ARP_GRATUITOUS_SEND                     353         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_ARP_HARDWARE_ADDRESS_FIND               354         /* I1 = ip ptr, I2 = ip_address, I3 = physical msw, I4 = physical lsw       */ 
#define TML_NX_TRACE_ARP_INFO_GET                            355         /* I1 = ip ptr, I2 = arps sent, I3 = arp responses, I3 = arps received      */ 
#define TML_NX_TRACE_ARP_IP_ADDRESS_FIND                     356         /* I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical lsw       */ 
#define TML_NX_TRACE_ARP_STATIC_ENTRIES_DELETE               357         /* I1 = ip ptr, I2 = entries deleted                                        */ 
#define TML_NX_TRACE_ARP_STATIC_ENTRY_CREATE                 358         /* I1 = ip ptr, I2 = ip address, I3 = physical msw, I4 = physical_lsw       */ 
#define TML_NX_TRACE_ARP_STATIC_ENTRY_DELETE                 359         /* I1 = ip ptr, I2 = ip address, I3 = physical_msw, I4 = physical_lsw       */  
#define TML_NX_TRACE_ICMP_ENABLE                             360         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_ICMP_INFO_GET                           361         /* I1 = ip ptr, I2 = pings sent, I3 = ping responses, I4 = pings received   */ 
#define TML_NX_TRACE_ICMP_PING                               362         /* I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size              */ 
#define TML_NX_TRACE_IGMP_ENABLE                             363         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IGMP_INFO_GET                           364         /* I1 = ip ptr, I2 = reports sent, I3 = queries received, I4 = groups joined*/
#define TML_NX_TRACE_IGMP_LOOPBACK_DISABLE                   365         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IGMP_LOOPBACK_ENABLE                    366         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IGMP_MULTICAST_JOIN                     367         /* I1 = ip ptr, I2 = group address                                          */ 
#define TML_NX_TRACE_IGMP_MULTICAST_LEAVE                    368         /* I1 = ip ptr, I2 = group_address                                          */ 
#define TML_NX_TRACE_IP_ADDRESS_CHANGE_NOTIFY                369         /* I1 = ip ptr, I2 = ip address change notify, I3 = additional info         */ 
#define TML_NX_TRACE_IP_ADDRESS_GET                          370         /* I1 = ip ptr, I2 = ip address, I3 = network_mask                          */ 
#define TML_NX_TRACE_IP_ADDRESS_SET                          371         /* I1 = ip ptr, I2 = ip address, I3 = network_mask                          */ 
#define TML_NX_TRACE_IP_CREATE                               372         /* I1 = ip ptr, I2 = ip address, I3 = network mask, I4 = default_pool       */  
#define TML_NX_TRACE_IP_DELETE                               373         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_DRIVER_DIRECT_COMMAND                374         /* I1 = ip ptr, I2 = command, I3 = return value                             */ 
#define TML_NX_TRACE_IP_FORWARDING_DISABLE                   375         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_FORWARDING_ENABLE                    376         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_FRAGMENT_DISABLE                     377         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_FRAGMENT_ENABLE                      378         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_GATEWAY_ADDRESS_SET                  379         /* I1 = ip ptr, I2 = gateway address                                        */ 
#define TML_NX_TRACE_IP_INFO_GET                             380         /* I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = packets dropped  */ 
#define TML_NX_TRACE_IP_RAW_PACKET_DISABLE                   381         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_RAW_PACKET_ENABLE                    382         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_IP_RAW_PACKET_RECEIVE                   383         /* I1 = ip ptr, I2 = packet ptr, I3 = wait option                           */ 
#define TML_NX_TRACE_IP_RAW_PACKET_SEND                      384         /* I1 = ip ptr, I2 = packet ptr, I3 = destination ip, I4 = type of service  */ 
#define TML_NX_TRACE_IP_STATUS_CHECK                         385         /* I1 = ip ptr, I2 = needed status, I3 = actual status, I4 = wait option    */ 
#define TML_NX_TRACE_PACKET_ALLOCATE                         386         /* I1 = pool ptr, I2 = packet ptr, I3 = packet type, I4 = available packets */ 
#define TML_NX_TRACE_PACKET_COPY                             387         /* I1 = packet ptr, I2 = new packet ptr, I3 = pool ptr, I4 = wait option    */ 
#define TML_NX_TRACE_PACKET_DATA_APPEND                      388         /* I1 = packet ptr, I2 = data start, I3 = data size, I4 = pool ptr          */ 
#define TML_NX_TRACE_PACKET_DATA_RETRIEVE                    389         /* I1 = packet ptr, I2 = buffer start, I3 = bytes copied                    */ 
#define TML_NX_TRACE_PACKET_LENGTH_GET                       390         /* I1 = packet ptr, I2 = length                                             */ 
#define TML_NX_TRACE_PACKET_POOL_CREATE                      391         /* I1 = pool ptr, I2 = payload size, I3 = memory ptr, I4 = memory_size      */ 
#define TML_NX_TRACE_PACKET_POOL_DELETE                      392         /* I1 = pool ptr                                                            */ 
#define TML_NX_TRACE_PACKET_POOL_INFO_GET                    393         /* I1 = pool ptr, I2 = total_packets, I3 = free packets, I4 = empty requests*/ 
#define TML_NX_TRACE_PACKET_RELEASE                          394         /* I1 = packet ptr, I2 = packet status, I3 = available packets              */ 
#define TML_NX_TRACE_PACKET_TRANSMIT_RELEASE                 395         /* I1 = packet ptr, I2 = packet status, I3 = available packets              */ 
#define TML_NX_TRACE_RARP_DISABLE                            396         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_RARP_ENABLE                             397         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_RARP_INFO_GET                           398         /* I1 = ip ptr, I2 = requests sent, I3 = responses received, I4 = invalids  */ 
#define TML_NX_TRACE_SYSTEM_INITIALIZE                       399         /* none                                                                     */ 
#define TML_NX_TRACE_TCP_CLIENT_SOCKET_BIND                  400         /* I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option                */ 
#define TML_NX_TRACE_TCP_CLIENT_SOCKET_CONNECT               401         /* I1 = ip ptr, I2 = socket ptr, I3 = server ip, I4 = server port           */ 
#define TML_NX_TRACE_TCP_CLIENT_SOCKET_PORT_GET              402         /* I1 = ip ptr, I2 = socket ptr, I3 = port                                  */ 
#define TML_NX_TRACE_TCP_CLIENT_SOCKET_UNBIND                403         /* I1 = ip ptr, I2 = socket ptr                                             */ 
#define TML_NX_TRACE_TCP_ENABLE                              404         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_TCP_FREE_PORT_FIND                      405         /* I1 = ip ptr, I2 = port, I3 = free port                                   */ 
#define TML_NX_TRACE_TCP_INFO_GET                            406         /* I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets  */ 
#define TML_NX_TRACE_TCP_SERVER_SOCKET_ACCEPT                407         /* I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state        */ 
#define TML_NX_TRACE_TCP_SERVER_SOCKET_LISTEN                408         /* I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = listen queue size          */ 
#define TML_NX_TRACE_TCP_SERVER_SOCKET_RELISTEN              409         /* I1 = ip ptr, I2 = port, I3 = socket ptr, I4 = socket state               */ 
#define TML_NX_TRACE_TCP_SERVER_SOCKET_UNACCEPT              410         /* I1 = ip ptr, I2 = socket ptr, I3 = socket state                          */ 
#define TML_NX_TRACE_TCP_SERVER_SOCKET_UNLISTEN              411         /* I1 = ip ptr, I2 = port                                                   */ 
#define TML_NX_TRACE_TCP_SOCKET_CREATE                       412         /* I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = window size     */ 
#define TML_NX_TRACE_TCP_SOCKET_DELETE                       413         /* I1 = ip ptr, I2 = socket ptr, I3 = socket state                          */ 
#define TML_NX_TRACE_TCP_SOCKET_DISCONNECT                   414         /* I1 = ip ptr, I2 = socket ptr, I3 = wait option, I4 = socket state        */ 
#define TML_NX_TRACE_TCP_SOCKET_INFO_GET                     415         /* I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received       */  
#define TML_NX_TRACE_TCP_SOCKET_MSS_GET                      416         /* I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 = socket state                */ 
#define TML_NX_TRACE_TCP_SOCKET_MSS_PEER_GET                 417         /* I1 = ip ptr, I2 = socket ptr, I3 = peer_mss, I4 = socket state           */ 
#define TML_NX_TRACE_TCP_SOCKET_MSS_SET                      418         /* I1 = ip ptr, I2 = socket ptr, I3 = mss, I4 socket state                  */ 
#define TML_NX_TRACE_TCP_SOCKET_RECEIVE                      419         /* I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = rx sequence          */ 
#define TML_NX_TRACE_TCP_SOCKET_RECEIVE_NOTIFY               420         /* I1 = ip ptr, I2 = socket ptr, I3 = receive notify                        */ 
#define TML_NX_TRACE_TCP_SOCKET_SEND                         421         /* I1 = socket ptr, I2 = packet ptr, I3 = length, I4 = tx sequence          */ 
#define TML_NX_TRACE_TCP_SOCKET_STATE_WAIT                   422         /* I1 = ip ptr, I2 = socket ptr, I3 = desired state, I4 = previous state    */ 
#define TML_NX_TRACE_TCP_SOCKET_TRANSMIT_CONFIGURE           423         /* I1 = ip ptr, I2 = socket ptr, I3 = queue depth, I4 = timeout             */  
#define TML_NX_TRACE_UDP_ENABLE                              424         /* I1 = ip ptr                                                              */ 
#define TML_NX_TRACE_UDP_FREE_PORT_FIND                      425         /* I1 = ip ptr, I2 = port, I3 = free port                                   */ 
#define TML_NX_TRACE_UDP_INFO_GET                            426         /* I1 = ip ptr, I2 = bytes sent, I3 = bytes received, I4 = invalid packets  */ 
#define TML_NX_TRACE_UDP_SOCKET_BIND                         427         /* I1 = ip ptr, I2 = socket ptr, I3 = port, I4 = wait option                */ 
#define TML_NX_TRACE_UDP_SOCKET_CHECKSUM_DISABLE             428         /* I1 = ip ptr, I2 = socket ptr                                             */ 
#define TML_NX_TRACE_UDP_SOCKET_CHECKSUM_ENABLE              429         /* I1 = ip ptr, I2 = socket ptr                                             */ 
#define TML_NX_TRACE_UDP_SOCKET_CREATE                       430         /* I1 = ip ptr, I2 = socket ptr, I3 = type of service, I4 = queue maximum   */ 
#define TML_NX_TRACE_UDP_SOCKET_DELETE                       431         /* I1 = ip ptr, I2 = socket ptr                                             */ 
#define TML_NX_TRACE_UDP_SOCKET_INFO_GET                     432         /* I1 = ip ptr, I2 = socket ptr, I3 = bytes sent, I4 = bytes received       */ 
#define TML_NX_TRACE_UDP_SOCKET_PORT_GET                     433         /* I1 = ip ptr, I2 = socket ptr, I3 = port                                  */ 
#define TML_NX_TRACE_UDP_SOCKET_RECEIVE                      434         /* I1 = ip ptr, I2 = socket ptr, I3 = packet ptr, I4 = packet size          */ 
#define TML_NX_TRACE_UDP_SOCKET_RECEIVE_NOTIFY               435         /* I1 = ip ptr, I2 = socket ptr, I3 = receive notify                        */ 
#define TML_NX_TRACE_UDP_SOCKET_SEND                         436         /* I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address      */ 
#define TML_NX_TRACE_UDP_SOCKET_UNBIND                       437         /* I1 = ip ptr, I2 = socket ptr, I3 = port                                  */ 
#define TML_NX_TRACE_UDP_SOURCE_EXTRACT                      438         /* I1 = packet ptr, I2 = ip address, I3 = port                              */ 


#define NX_TRACE_IP_INTERFACE_ATTACH						439         /* I1 = ip ptr, I2 = ip address, I3 = interface index                       */
#define NX_TRACE_UDP_SOCKET_BYTES_AVAILABLE					440         /* I1 = ip ptr, I2 = socket ptr, I3 = bytes available                       */
#define NX_TRACE_IP_STATIC_ROUTE_ADD                        441          /* I1 = ip_ptr, I2 = network_address, I3 = net_mask, I4 = next_hop          */
#define NX_TRACE_IP_STATIC_ROUTE_DELETE						442         /* I1 = ip_ptr, I2 = network_address, I3 = net_mask                         */
#define NX_TRACE_TCP_SOCKET_PEER_INFO_GET					443         /* I1 = socket ptr, I2 = network_address, I3 = port                         */
#define NX_TRACE_TCP_SOCKET_WINDOW_UPDATE_NOTIFY_SET        444         /* I1 = socket ptr,                                                         */
#define NX_TRACE_UDP_SOCKET_INTERFACE_SET                   445          /* I1 = socket_ptr, I2 = interface_index,                                   */
#define NX_TRACE_IP_INTERFACE_INFO_GET                      446         /* I1 = ip_ptr, I2 = ip_address, I3 = mtu_size, I4 = interface_index        */
#define NX_TRACE_PACKET_DATA_EXTRACT_OFFSET                 447         /* I1 = packet_ptr, I2 = buffer_length, I3 = bytes_copied,                  */
#define NX_TRACE_TCP_SOCKET_BYTES_AVAILABLE                 448         /* I1 = ip ptr, I2 = socket ptr, I3 = bytes available                       */


#define NX_TRACE_IPSEC_ENABLE                               469         /* I1 = ip_ptr                                                              */
#define NXD_TRACE_ICMP_ENABLE                               470         /* I1 = ip ptr                                                              */ 
#define NX_TRACE_ICMP_PING6                                 471         /* I1 = ip ptr, I2 = ip_address, I3 = data ptr, I4 = data size              */ 
#define NXD_TRACE_UDP_SOURCE_EXTRACT                        472         /* I1 = packet ptr, I2 = IP Version (4 or 6), I3 = ip address, I4 = port    */ 
#define NXD_TRACE_UDP_SOCKET_SET_INTERFACE                  473         /* I1 = udp_socket_ptr, I2 = interface_id                                   */
#define NXD_TRACE_TCP_SOCKET_SET_INTERFACE                  474         /* I1 = tcp_socket_ptr, I2 = interface_id                                   */
#define NXD_TRACE_UDP_SOCKET_SEND                           475         /* I1 = socket ptr, I2 = packet ptr, I3 = packet size, I4 = ip address      */ 
#define NXD_TRACE_ND_CACHE_DELETE                           476         /* I1 = dest_ip                                                             */ 
#define NXD_TRACE_ND_CACHE_ENTRY_SET                        477         /* I1 = IP address, I2 = physical msw, I3 = physical lsw                    */
#define NX_TRACE_ND_CACHE_IP_ADDRESS_FIND                   478         /* I1 = ip_ptr, I2 = IP address, I3 = physical msw, I4 = physical lsw       */
#define NXD_TRACE_ND_CACHE_INVALIDATE                       479         /* I1 = ip_ptr                                                              */
#define NXD_TRACE_IPV6_GLOBAL_ADDRESS_GET                   480         /* I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
#define NXD_TRACE_IPV6_GLOBAL_ADDRESS_SET                   481         /* I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
//#define NX_TRACE_IPSTATIC_ROUTE_ADD                         482         /* I1 = ip_ptr, I2 = network address, I3 = net_mask, I4 = next hop address  */
//#define NX_TRACE_IP_STATIC_ROUTING_ENABLE                   483         /* I1 = ip_ptr                                                              */
//#define NX_TRACE_IP_STATIC_ROUTING_DISABLE                  484         /* I1 = ip_ptr                                                              */
#define NX_TRACE_IPV6_ENABLE                                485         /* I1 = ip_ptr                                                              */
#define NXD_TRACE_IPV6_RAW_PACKET_SEND                      486         /* I1 = ip_ptr, I2 = ip address lsw, I3 = protocol, I4 = packet_ptr         */
#define NXD_TRACE_IP_RAW_PACKET_SEND                        487         /* I1 = ip_ptr, I2 = ip address lsw, I3 = type of serveice, I4 = packet_ptr */
#define NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_GET                488         /* I1 = ip_ptr, I2 = IP address lsw                                         */
#define NXD_TRACE_IPV6_LINKLOCAL_ADDRESS_SET                489         /* I1 = ip_ptr, I2 = IP address lsw, I3 = prefix length                     */
#define NXD_TRACE_IPV6_INITIATE_DAD_PROCESS                 490         /* I1 = ip_ptr                                                              */
#define NXD_TRACE_IPV6_DEFAULT_ROUTER_ADD                   491         /* I1 = ip_ptr, I2 = router addr lsw, I3 = router lifetime                  */
#define NXD_TRACE_IPV6_DEFAULT_ROUTER_DELETE                492         /* I1 = ip_ptr, I2 = router addr lsw,                                       */


#define NXD_TRACE_IPV6_INTERFACE_ADDRESS_GET            493         /* I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index */
#define NXD_TRACE_IPV6_INTERFACE_ADDRESS_SET            494         /* I1 = ip_ptr, I2 = IP address lsw,I3 = prefix length,I4 = interface_index */
#define NXD_TRACE_TCP_SOCKET_PEER_INFO_GET              495         /* I1 = socket_ptr, I2 = Peer IP address, I3 = peer_port                    */
#define NXD_TRACE_IP_MAX_PAYLOAD_SIZE_FIND              496    //Value missing from file given to me.


/* Newly Defined.  */
//#define nx_ip_static_route_add								465
//#define nx_ip_static_route_delete							466
//#define nx_ip_ raw_packet_interface_send					467
//#define nx_tcp_socket_bytes_available						468
//#define nx_tcp_socket_peer_info_get							469
//#define nx_tcp_socket_window_update_notify_set				470
//#define nx_udp_socket_interface_send						471
//#define nx_udp_packet_info_extract							472
//#define nx_udp_socket_bytes_available						473
//#define nx_igmp_multicast_interface_join					474
//#define nx_ip_interface_attach								475
//#define nx_ip_interface_address_get							476
//#define nx_ip_interface_address_set							477
//#define nx_ip_interface_info_get							478
//#define nx_ip_interface_status_check						479
//#define nx_packet_data_extract_offset						480

/* end newly defined.  */

/* Define the UsbX host stack events first.  */																						    																             
#define TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_CREATE						(600 + 1)				/* I1 = class           , I2 = class instance                                                       */    
#define TML_UX_TRACE_HOST_STACK_CLASS_INSTANCE_DESTROY						(600 + 2)				/* I1 = class           , I2 = class instance                                                       */    
#define TML_UX_TRACE_HOST_STACK_CONFIGURATION_DELETE   						(600 + 3)				/* I1 = configuration                                                                               */    
#define TML_UX_TRACE_HOST_STACK_CONFIGURATION_ENUMERATE						(600 + 4)				/* I1 = device                                                                                      */    
#define TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_CREATE				(600 + 5)				/* I1 = configuration                                                                               */    
#define TML_UX_TRACE_HOST_STACK_CONFIGURATION_INSTANCE_DELETE				(600 + 6)				/* I1 = configuration                                                                               */    
#define TML_UX_TRACE_HOST_STACK_CONFIGURATION_SET             				(600 + 7)				/* I1 = configuration                                                                               */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_ADDRESS_SET             				(600 + 8)				/* I1 = device          , I2 = device address                                                       */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_GET       				(600 + 9)				/* I1 = device          , I2 = configuration                                                        */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_CONFIGURATION_SELECT   				(600 + 10)				/* I1 = device          , I2 = configuration                                                        */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_DESCRIPTOR_READ        				(600 + 11)				/* I1 = device                                                                                      */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_GET                    				(600 + 12)				/* I1 = device index                                                                                */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_REMOVE                 				(600 + 13)				/* I1 = hcd             , I2 = parent          , I3 = port index        , I4 = device               */    
#define TML_UX_TRACE_HOST_STACK_DEVICE_RESOURCE_FREE          				(600 + 14)				/* I1 = device                                                                                      */    
#define TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_CREATE      				(600 + 15)				/* I1 = device          , I2 = endpoint                                                             */    
#define TML_UX_TRACE_HOST_STACK_ENDPOINT_INSTANCE_DELETE      				(600 + 16)				/* I1 = device          , I2 = endpoint                                                             */    
#define TML_UX_TRACE_HOST_STACK_ENDPOINT_RESET       	        			(600 + 17)				/* I1 = device          , I2 = endpoint                                                             */    
#define TML_UX_TRACE_HOST_STACK_ENDPOINT_TRANSFER_ABORT       				(600 + 18)				/* I1 = endpoint                                                                                    */    
#define TML_UX_TRACE_HOST_STACK_HCD_REGISTER                  				(600 + 19)				/* I1 = hcd name                                                                                    */    
#define TML_UX_TRACE_HOST_STACK_INITIALIZE                    				(600 + 20)				/*                                                                                                  */       
#define TML_UX_TRACE_HOST_STACK_INTERFACE_ENDPOINT_GET         				(600 + 21)				/* I1 = interface       , I2 = endpoint index                                                       */    
#define TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_CREATE      				(600 + 22)				/* I1 = interface                                                                                   */    
#define TML_UX_TRACE_HOST_STACK_INTERFACE_INSTANCE_DELETE      				(600 + 23)				/* I1 = interface                                                                                   */    
#define TML_UX_TRACE_HOST_STACK_INTERFACE_SET                  				(600 + 24)				/* I1 = interface                                                                                   */    
#define TML_UX_TRACE_HOST_STACK_INTERFACE_SETTING_SELECT       				(600 + 25)				/* I1 = interface                                                                                   */    
#define TML_UX_TRACE_HOST_STACK_NEW_CONFIGURATION_CREATE					(600 + 26)				/* I1 = device          , I2 = configuration                                                        */    
#define TML_UX_TRACE_HOST_STACK_NEW_DEVICE_CREATE       					(600 + 27)				/* I1 = hcd             , I2 = device owner    , I3 = port index        , I4 = device               */    
#define TML_UX_TRACE_HOST_STACK_NEW_ENDPOINT_CREATE       					(600 + 28)				/* I1 = interface       , I2 = endpoint                                                             */    
#define TML_UX_TRACE_HOST_STACK_RH_CHANGE_PROCESS         					(600 + 29)				/* I1 = port index                                                                                  */       
#define TML_UX_TRACE_HOST_STACK_RH_DEVICE_EXTRACTION      					(600 + 30)				/* I1 = hcd             , I2 = port index                                                           */    
#define TML_UX_TRACE_HOST_STACK_RH_DEVICE_INSERTION       					(600 + 31)				/* I1 = hcd             , I2 = port index                                                           */    
#define TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST          					(600 + 32)				/* I1 = device          , I2 = endpoint        , I3 = transfer request                              */    
#define TML_UX_TRACE_HOST_STACK_TRANSFER_REQUEST_ABORT     					(600 + 33)				/* I1 = device          , I2 = endpoint        , I3 = transfer request                              */    
																																	    																             
/* Define the UsbX host class events first.  */																						    																             
#define TML_UX_TRACE_HOST_CLASS_ASIX_ACTIVATE								(650 + 1)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_ASIX_DEACTIVATE								(650 + 2)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_ASIX_INTERRUPT_NOTIFICATION					(650 + 3)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_ASIX_READ                  					(650 + 4)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */       
#define TML_UX_TRACE_HOST_CLASS_ASIX_WRITE                 					(650 + 5)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */       					    
																		 										    																             
#define TML_UX_TRACE_HOST_CLASS_AUDIO_ACTIVATE								(650 + 10)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_GET						(650 + 11)				/* I1 = class instance                                                                              */       					    														        
#define TML_UX_TRACE_HOST_CLASS_AUDIO_CONTROL_VALUE_SET						(650 + 12)				/* I1 = class instance  , I2 = audio control                                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_AUDIO_DEACTIVATE 							(650 + 13)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_AUDIO_READ                  				(650 + 14)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */       
#define TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_GET  				(650 + 15)				/* I1 = class instance                                                                              */       					    														        
#define TML_UX_TRACE_HOST_CLASS_AUDIO_STREAMING_SAMPLING_SET  				(650 + 16)				/* I1 = class instance  , I2 = audio sampling                                                       */					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_AUDIO_WRITE                 				(650 + 17)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */       
																		 								    											     			                   
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_ACTIVATE   							(650 + 20)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_DEACTIVATE   						(650 + 21)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_CODING				(650 + 22)				/* I1 = class instance  , I2 = parameter                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_LINE_CODING				(650 + 23)				/* I1 = class instance  , I2 = parameter                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SET_LINE_STATE 				(650 + 24)				/* I1 = class instance  , I2 = parameter                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_SEND_BREAK     				(650 + 25)				/* I1 = class instance  , I2 = parameter                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_IN_PIPE  				(650 + 26)				/* I1 = class instance  , I2 = endpoint                                                             */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_ABORT_OUT_PIPE  				(650 + 27)				/* I1 = class instance  , I2 = endpointr                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_NOTIFICATION_CALLBACK			(650 + 28)				/* I1 = class instance  , I2 = parameter                                                            */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_IOCTL_GET_DEVICE_STATUS 			(650 + 29)				/* I1 = class instance  , I2 = device status                                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_READ                    			(650 + 30)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_START         			(650 + 31)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_RECEPTION_STOP          			(650 + 32)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_CDC_ACM_WRITE                   			(650 + 33)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */
																		 								    											     			                   
#define TML_UX_TRACE_HOST_CLASS_HID_ACTIVATE       							(650 + 40)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_HID_CLIENT_REGISTER							(650 + 41)				/* I1 = hid client name                                                                             */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_DEACTIVATE     							(650 + 42)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_HID_IDLE_GET       							(650 + 43)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_IDLE_SET       							(650 + 44)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_ACTIVATE						(650 + 45)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_KEYBOARD_DEACTIVATE						(650 + 46)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_MOUSE_ACTIVATE							(650 + 47)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_MOUSE_DEACTIVATE						(650 + 48)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_ACTIVATE					(650 + 49)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_REMOTE_CONTROL_DEACTIVATE 				(650 + 50)				/* I1 = class instance  , I2 = hid client instance                                                  */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_REPORT_GET                				(650 + 51)				/* I1 = class instance  , I2 = client report                                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_HID_REPORT_SET                				(650 + 52)				/* I1 = class instance  , I2 = client report                                                        */					    														        
																		 							    											     			                   
#define TML_UX_TRACE_HOST_CLASS_HUB_ACTIVATE       							(650 + 60)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_HUB_CHANGE_DETECT  							(650 + 62)				/* I1 = class instance                                                                              */       					    														        
#define TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_CONNECTION_PROCESS 			(650 + 63)				/* I1 = class instance  , I2 = port            , I3 = port status                                   */
#define TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_ENABLE_PROCESS 				(650 + 64)				/* I1 = class instance  , I2 = port            , I3 = port status                                   */       					    														        
#define TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_OVER_CURRENT_PROCESS 		(650 + 65)				/* I1 = class instance  , I2 = port            , I3 = port status                                   */
#define TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_RESET_PROCESS 				(650 + 66)				/* I1 = class instance  , I2 = port            , I3 = port status                                   */
#define TML_UX_TRACE_HOST_CLASS_HUB_PORT_CHANGE_SUSPEND_PROCESS 			(650 + 67)				/* I1 = class instance  , I2 = port            , I3 = port status                                   */
#define TML_UX_TRACE_HOST_CLASS_HUB_DEACTIVATE     							(650 + 68)				/* I1 = class instance                                                                              */       
																		 										    											     			                   
#define TML_UX_TRACE_HOST_CLASS_PIMA_ACTIVATE       						(650 + 70)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_PIMA_DEACTIVATE       						(650 + 71)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_INFO_GET   						(650 + 72)				/* I1 = class instance  , I2 = pima device                                                          */       
#define TML_UX_TRACE_HOST_CLASS_PIMA_DEVICE_RESET      						(650 + 73)				/* I1 = class instance                                                                              */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_NOTIFICATION      						(650 + 74)				/* I1 = class instance  , I2 = event code      , I3 = transaction ID    , I4 = parameter1           */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_NUM_OBJECTS_GET   						(650 + 75)				/* I1 = class instance                                                                              */       					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_CLOSE   						(650 + 76)				/* I1 = class instance  , I2 = object                                                               */       					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_COPY    						(650 + 77)				/* I1 = class instance  , I2 = object handle                                                        */       					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_DELETE  						(650 + 78)				/* I1 = class instance  , I2 = object handle                                                        */       					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_GET     						(650 + 79)				/* I1 = class instance  , I2 = object handle   , I3 = object                                        */       					    														        					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_GET   						(650 + 80)				/* I1 = class instance  , I2 = object handle   , I3 = object                                        */       					    														        					    														        					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_INFO_SEND  						(650 + 81)				/* I1 = class instance  , I2 = object                                                               */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_MOVE    						(650 + 82)				/* I1 = class instance  , I2 = object handle                                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_SEND    						(650 + 83)				/* I1 = class instance  , I2 = object          , I3 = object_buffer     , I4 = object length        */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_OBJECT_TRANSFER_ABORT					(650 + 84)				/* I1 = class instance  , I2 = object handle   , I3 = object                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_READ                 					(650 + 85)				/* I1 = class instance  , I2 = data pointer    , I3 = data length                                   */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_REQUEST_CANCEL        					(650 + 86)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_CLOSE         					(650 + 87)				/* I1 = class instance  , I2 = pima session                                                         */					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_SESSION_OPEN          					(650 + 88)				/* I1 = class instance  , I2 = pima session                                                         */					    														        					    														        					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_IDS_GET       					(650 + 89)				/* I1 = class instance  , I2 = storage ID array, I3 = storage ID length                             */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_STORAGE_INFO_GET      					(650 + 90)				/* I1 = class instance  , I2 = storage ID      , I3 = storage                                       */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_THUMB_GET      						(650 + 91)				/* I1 = class instance  , I2 = object handle                                                        */					    														        
#define TML_UX_TRACE_HOST_CLASS_PIMA_WRITE             						(650 + 92)				/* I1 = class instance  , I2 = data pointer    , I3 = data length                                   */					    														        
																		 										    														                    
#define TML_UX_TRACE_HOST_CLASS_PRINTER_ACTIVATE       						(650 + 100)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_PRINTER_DEACTIVATE     						(650 + 101)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_PRINTER_NAME_GET       						(650 + 102)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PRINTER_READ           						(650 + 103)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PRINTER_WRITE          						(650 + 104)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PRINTER_SOFT_RESET     						(650 + 105)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PRINTER_STATUS_GET     						(650 + 106)				/* I1 = class instance  , I2 = printer status                                                       */					    														               
																		 										    														                      
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_ACTIVATE       					(650 + 110)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_DEACTIVATE     					(650 + 111)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_CODING				(650 + 112)             /* I1 = class instance  , I2 = parameter                                                            */   											               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_LINE_CODING				(650 + 113)				/* I1 = class instance  , I2 = parameter                                                            */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SET_LINE_STATE 				(650 + 114)				/* I1 = class instance  , I2 = parameter                                                            */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_PURGE          				(650 + 115)				/* I1 = class instance  , I2 = parameter                                                            */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_SEND_BREAK     				(650 + 116)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_IN_PIPE  				(650 + 117)				/* I1 = class instance  , I2 = endpoint                                                             */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_ABORT_OUT_PIPE  				(650 + 118)				/* I1 = class instance  , I2 = endpointr                                                            */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_REPORT_DEVICE_STATUS_CHANGE	(650 + 119)				/* I1 = class instance  , I2 = parameter                                                            */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_IOCTL_GET_DEVICE_STATUS 			(650 + 120)				/* I1 = class instance  , I2 = device status                                                        */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_READ                   		 	(650 + 121)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_START        		 	(650 + 122)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_RECEPTION_STOP         			(650 + 123)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_PROLIFIC_WRITE                  			(650 + 124)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
																		 								    														                      
#define TML_UX_TRACE_HOST_CLASS_STORAGE_ACTIVATE       						(650 + 130)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_STORAGE_DEACTIVATE     						(650 + 131)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_CAPACITY_GET					(650 + 132)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_FORMAT_CAPACITY_GET 			(650 + 133)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_MOUNT             			(650 + 134)				/* I1 = class instance  , I2 = sector                                                               */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_OPEN              			(650 + 135)				/* I1 = class instance  , I2 = media                                                                */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_READ              			(650 + 136)				/* I1 = class instance  , I2 = sector start    , I3 = sector count      , I4 = data pointer         */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_MEDIA_WRITE             			(650 + 137)				/* I1 = class instance  , I2 = sector start    , I3 = sector count      , I4 = data pointer         */						    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_REQUEST_SENSE           			(650 + 138)				/* I1 = class instance                                                                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_START_STOP              			(650 + 139)				/* I1 = class instance  , I2 = start stop signal                                                    */					    														               
#define TML_UX_TRACE_HOST_CLASS_STORAGE_UNIT_READY_TEST         			(650 + 140)				/* I1 = class instance                                                                              */					    														               
																		 										    														                      
#define TML_UX_TRACE_HOST_CLASS_DPUMP_ACTIVATE       						(650 + 150)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_DPUMP_DEACTIVATE     						(650 + 151)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_HOST_CLASS_DPUMP_READ           						(650 + 152)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
#define TML_UX_TRACE_HOST_CLASS_DPUMP_WRITE          						(650 + 153)				/* I1 = class instance  , I2 = data pointer    , I3 = requested length                              */					    														               
																																				    														                      
/* Define the UsbX device stack events first.  */																								    														                      
#define TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_GET 	   				(850 + 1)				/* I1 = interface value                                                                             */					    														               
#define TML_UX_TRACE_DEVICE_STACK_ALTERNATE_SETTING_SET 	   				(850 + 2)				/* I1 = interface value , I2 = alternate setting value                                              */					    														               
#define TML_UX_TRACE_DEVICE_STACK_CLASS_REGISTER        	   				(850 + 3)				/* I1 = class name      , I2 = interface number, I3 = parameter                                     */
#define TML_UX_TRACE_DEVICE_STACK_CLEAR_FEATURE         	   				(850 + 4)				/* I1 = request type    , I2 = request value   , I3 = request index                                 */
#define TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_GET        	   				(850 + 5)				/* I1 = configuration value                                                                         */
#define TML_UX_TRACE_DEVICE_STACK_CONFIGURATION_SET        	   				(850 + 6)				/* I1 = configuration value                                                                         */
#define TML_UX_TRACE_DEVICE_STACK_CONNECT                  	   				(850 + 7)				/*                                                                                                  */					    														               
#define TML_UX_TRACE_DEVICE_STACK_DESCRIPTOR_SEND          	   				(850 + 8)				/* I1 = descriptor type , I2 = request index                                                        */					    														               
#define TML_UX_TRACE_DEVICE_STACK_DISCONNECT               	   				(850 + 9)				/* I1 = device                                                                                      */
#define TML_UX_TRACE_DEVICE_STACK_ENDPOINT_STALL          	   				(850 + 10)				/* I1 = endpoint                                                                                    */
#define TML_UX_TRACE_DEVICE_STACK_GET_STATUS              	   				(850 + 11)				/* I1 = request type    , I2 = request value   , I3 = request index                                 */					    														               
#define TML_UX_TRACE_DEVICE_STACK_HOST_WAKEUP             	   				(850 + 12)				/*                                                                                                  */					    														               
#define TML_UX_TRACE_DEVICE_STACK_INITIALIZE              	   				(850 + 13)				/*                                                                                                  */					    														               
#define TML_UX_TRACE_DEVICE_STACK_INTERFACE_DELETE         	   				(850 + 14)				/* I1 = interface                                                                                   */					    														               
#define TML_UX_TRACE_DEVICE_STACK_INTERFACE_GET            	   				(850 + 15)				/* I1 = interface value                                                                             */
#define TML_UX_TRACE_DEVICE_STACK_INTERFACE_SET            	   				(850 + 16)				/* I1 = alternate setting value                                                                     */
#define TML_UX_TRACE_DEVICE_STACK_SET_FEATURE              	   				(850 + 17)				/* I1 = request value   , I2 = request index                                                        */
#define TML_UX_TRACE_DEVICE_STACK_TRANSFER_ABORT           	   				(850 + 18)				/* I1 = transfer request, I2 = completion code                                                      */
#define TML_UX_TRACE_DEVICE_STACK_TRANSFER_ALL_REQUEST_ABORT    			(850 + 19)				/* I1 = endpoint        , I2 = completion code                                                      */
#define TML_UX_TRACE_DEVICE_STACK_TRANSFER_REQUEST              			(850 + 20)				/* I1 = transfer request                                                                            */
																																				    														                      
/* Define the UsbX device stack events first.  */																								    														                      
#define TML_UX_TRACE_DEVICE_CLASS_DPUMP_ACTIVATE       						(900 + 1)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_DPUMP_DEACTIVATE     						(900 + 2)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_DPUMP_READ           						(900 + 3)				/* I1 = class instance  , I2 = buffer          , I3 = requested_length                              */
#define TML_UX_TRACE_DEVICE_CLASS_DPUMP_WRITE          						(900 + 4)				/* I1 = class instance  , I2 = buffer          , I3 = requested_length                              */
																		 									    														                      
#define TML_UX_TRACE_DEVICE_CLASS_CDC_ACTIVATE       						(900 + 10)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_CDC_DEACTIVATE     						(900 + 11)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_CDC_READ           						(900 + 12)				/* I1 = class instance  , I2 = buffer          , I3 = requested_length                              */
#define TML_UX_TRACE_DEVICE_CLASS_CDC_WRITE          						(900 + 13)				/* I1 = class instance  , I2 = buffer          , I3 = requested_length                              */
																		 										    														                      
#define TML_UX_TRACE_DEVICE_CLASS_HID_ACTIVATE       						(900 + 20)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_HID_DEACTIVATE     						(900 + 21)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_GET           					(900 + 22)				/* I1 = class instance  , I2 = hid event                                                            */
#define TML_UX_TRACE_DEVICE_CLASS_HID_EVENT_SET           					(900 + 23)				/* I1 = class instance  , I2 = hid event                                                            */
#define TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_GET           					(900 + 24)				/* I1 = class instance  , I2 = descriptor type , I3 = request index                                 */
#define TML_UX_TRACE_DEVICE_CLASS_HID_REPORT_SET           					(900 + 25)				/* I1 = class instance  , I2 = descriptor type , I3 = request index                                 */
#define TML_UX_TRACE_DEVICE_CLASS_HID_DESCRIPTOR_SEND      					(900 + 26)				/* I1 = class instance  , I2 = descriptor type , I3 = request index                                 */
																		 										    														                      
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_ACTIVATE       						(900 + 30)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_DEACTIVATE     						(900 + 31)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_DEVICE_INFO_SEND						(900 + 32)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_GET           					(900 + 33)				/* I1 = class instance  , I2 = pima event                                                           */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_EVENT_SET           					(900 + 34)				/* I1 = class instance  , I2 = pima event                                                           */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_ADD          					(900 + 35)				/* I1 = class instance  , I2 = object handle                                                        */       
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_GET     					(900 + 36)				/* I1 = class instance  , I2 = object handle                                                        */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DATA_SEND    					(900 + 37)				/* I1 = class instance  , I2 = object handle                                                        */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_DELETE       					(900 + 38)				/* I1 = class instance  , I2 = object handle                                                        */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_HANDLES_SEND  				(900 + 39)				/* I1 = class instance  , I2 = storage id      , I3 = object format code, I4 = object association   */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_GET     					(900 + 40)				/* I1 = class instance  , I2 = object handle                                                        */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECT_INFO_SEND    					(900 + 41)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_OBJECTS_NUMBER_SEND 					(900 + 42)				/* I1 = class instance  , I2 = storage id      , I3 = object format code, I4 = object association   */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_PARTIAL_OBJECT_DATA_GET				(900 + 43)				/* I1 = class instance  , I2 = object handle   , I3 = offset requested  , I4 = length requested     */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_RESPONSE_SEND 						(900 + 44)				/* I1 = class instance  , I2 = response code   , I3 = number parameter  , I4 = pima parameter 1     */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_ID_SEND 						(900 + 45)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_PIMA_STORAGE_INFO_SEND 					(900 + 46)				/* I1 = class instance                                                                              */
																		 										    														               
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_ACTIVATE       						(900 + 50)				/* I1 = class instance                                                                              */       		
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_DEACTIVATE     						(900 + 51)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_RECEIVE 						(900 + 52)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_PACKET_TRANSMIT 					(900 + 53)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_QUERY       					(900 + 54)				/* I1 = class instance  , I2 = rndis OID                                                            */
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_KEEP_ALIVE    					(900 + 55)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_RESET         					(900 + 56)				/* I1 = class instance                                                                              */
#define TML_UX_TRACE_DEVICE_CLASS_RNDIS_MSG_SET         					(900 + 57)				/* I1 = class instance  , I2 = rndis OID                                                            */
																		 										    														               
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_ACTIVATE       					(900 + 70)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_DEACTIVATE     					(900 + 71)				/* I1 = class instance                                                                              */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_FORMAT           					(900 + 72)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_INQUIRY          					(900 + 73)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SELECT      					(900 + 74)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_MODE_SENSE       					(900 + 75)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_PREVENT_ALLOW_MEDIA_REMOVAL 		(900 + 76)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ           					(900 + 77)				/* I1 = class instance  , I2 = lun             , I3 = sector              , I4 = number sectors     */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_CAPACITY    					(900 + 78)            	/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_FORMAT_CAPACITY				(900 + 79)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_READ_TOC         					(900 + 80)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_REQUEST_SENSE    					(900 + 81)				/* I1 = class instance  , I2 = lun             , I3 = sense key           , I4 = code               */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_TEST_READY       					(900 + 82)            	/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_START_STOP       					(900 + 83)            	/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_VERIFY           					(900 + 84)				/* I1 = class instance  , I2 = lun                                                                  */       
#define TML_UX_TRACE_DEVICE_CLASS_STORAGE_WRITE          					(900 + 85)				/* I1 = class instance  , I2 = lun             , I3 = sector              , I4 = number sectors     */       

/* Define the UsbX Error Event.  */
#define TML_TML_UX_TRACE_ERROR		  											999

/* Event numbers 130-1024 are reserved for ThreadX, user event numbers start at 1025, as defined below.  */

#define TML_TRACE_USER_EVENT_START                           1025        /* I1, I2, I3, I4 are user defined                                      */ 


#define TML_OBJECT_NAME_SIZE  64

typedef struct TML_OBJECT_STRUCT
{

    unsigned long   tml_object_type;
    unsigned long   tml_object_parameter_1;
    unsigned long   tml_object_parameter_2;
    unsigned long   tml_object_address;
    unsigned char   tml_object_reserved1;
    unsigned char   tml_object_reserved2;
    char            tml_object_name[TML_OBJECT_NAME_SIZE];
    unsigned long   tml_object_lowest_priority;            /* Last five are only applicable for thread objects!  */
    unsigned long   tml_object_highest_priority;
    unsigned long   tml_object_lowest_preemption_threshold;
    unsigned long   tml_object_highest_preemption_threshold;
    unsigned long   tml_object_total_events;

} TML_OBJECT;


typedef struct TML_EVENT_STRUCT
{
    
    unsigned long       tml_event_context;
    unsigned long       tml_event_raw_id;
    unsigned long       tml_event_raw_priority;
    unsigned long       tml_event_thread_priority;
    unsigned long       tml_event_thread_preemption_threshold;
    unsigned long       tml_event_id;
    unsigned long       tml_event_time_stamp;
    unsigned long       tml_event_info_1;
    unsigned long       tml_event_info_2;
    unsigned long       tml_event_info_3;
    unsigned long       tml_event_info_4;
    unsigned long       tml_event_next_context;
    unsigned long       tml_event_thread_index;
    unsigned long       tml_event_priority_inversion;
    unsigned long       tml_event_bad_priority_inversion;
    _int64              tml_event_relative_ticks;

} TML_EVENT;


typedef struct TML_INVERSION_DETECTION_STRUCT
{

	unsigned long  tml_blocked_thread_address;
	unsigned long  tml_blocked_thread_priority;
	unsigned long  tml_owning_thread_address;
	unsigned long  tml_owning_thread_priority;

} TML_INVERSION_DETECTION;


typedef struct TML_EVENT_POPULARITY_STRUCT
{
	unsigned long	tml_event_popularity_event_id;
    unsigned long   tml_event_popularity_event_count;
} TML_EVENT_POPULARITY;


#define TML_THREAD_STATUS_UNKNOWN       0       /* Unknown, don't draw any line                                                         */
#define TML_THREAD_STATUS_COMPLETED     1       /* Completed, don't draw any line                                                       */
#define TML_THREAD_STATUS_TERMINATED    2       /* Terminated, don't draw any line                                                      */
#define TML_THREAD_STATUS_SUSPENDED     3       /* Suspended by tx_thread_suspend call, draw yellow line                                */
#define TML_THREAD_STATUS_SLEEPING      4       /* Suspended by tx_thread_sleep call, draw yellow line                                  */
#define TML_THREAD_STATUS_QUEUE         5       /* Suspended by Queue service, draw dark blue line associated with queues               */
#define TML_THREAD_STATUS_SEMAPHORE     6       /* Suspended by Semaphore service, draw light green line associated with semaphores     */
#define TML_THREAD_STATUS_EVENT_FLAG    7       /* Suspended by Event Flag service, draw maroon line associated with event flags        */
#define TML_THREAD_STATUS_BLOCK_POOL    8       /* Suspended by Block Pool service, draw aqua line associated with block pools          */
#define TML_THREAD_STATUS_BYTE_POOL     9       /* Suspended by Byte Pool service, draw light blue line associated with byte pools      */
#define TML_THREAD_STATUS_NETX          12      /* Suspended by NetX, draw light purple line associated with NetX services              */
#define TML_THREAD_STATUS_MUTEX         13      /* Suspended by Mutex service, draw orange line associated with mutexes                 */
#define TML_THREAD_STATUS_READY         99      /* Ready, draw a green line that matches the running event.                             */


extern "C"
{

typedef struct TML_THREAD_STATUS_SUMMARY_STRUCT
{

    unsigned long       tml_thread_status_summary_current_status;
    unsigned long       tml_thread_status_summary_status_changes;
    unsigned long       tml_thread_status_summary_address;
    unsigned long       *tml_thread_status_summary_list;

} TML_THREAD_STATUS_SUMMARY;
int		tml_initialize(FILE *source_trace_file, unsigned long *total_threads, unsigned long *total_objects, 
								unsigned long *total_events, _int64 *max_relative_ticks, char  **error_string);
int		tml_uninitialize(void);


int		tml_header_info_get(unsigned long  *header_trace_id, 
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
							unsigned long  *header_reserved4);


int		tml_object_get(unsigned long object_index, char **object_name, unsigned long *object_address,
										unsigned long *parameter_1, unsigned long *parameter_2);
int     tml_object_thread_get(unsigned long thread_index, char **object_name, unsigned long *object_address,
                                        unsigned long *parameter_1, unsigned long *parameter_2, 
                                        unsigned long *lowest_priority, unsigned long *highest_priority);
int     tml_object_thread_preemption_threshold_get(unsigned long thread_index, unsigned long *lowest_preemption_threshold, unsigned long *highest_preemption_threshold);
int     tml_object_by_address_find(unsigned long object_address, unsigned long *object_index);


int     tml_event_get(unsigned long event_index, unsigned long *event_context, unsigned long *event_id,
                                unsigned long *event_thread_priority, unsigned long *event_time_stamp,
                                unsigned long *event_info_1, unsigned long *event_info_2, 
                                unsigned long *event_info_3, unsigned long *event_info_4,
                                _int64 *event_relative_ticks, 
                                unsigned long *next_context, unsigned long *thread_index, 
                                unsigned long *priority_inversion_event, unsigned long *bad_priority_inversion_event);

int     tml_event_preemption_threshold_get(unsigned long event_index, unsigned long *preemption_threshold);

int     tml_event_by_relative_ticks_find(_int64 relative_ticks_start, _int64 relative_ticks_end, 
                                                                        unsigned long *event_index);

int     tml_system_performance_statistics_get(unsigned long *context_switches, unsigned long *thread_preemptions, unsigned long *time_slices, 
                                              unsigned long *thread_suspensions, unsigned long *thread_resumptions, 
                                              unsigned long *interrupts, unsigned long *nested_interrupts,
                                              unsigned long *deterministic_priority_inversions, unsigned long *undeterministic_priority_inversions);
int     tml_system_filex_performance_statistics_get(unsigned long *media_opens, unsigned long *media_closes, unsigned long *media_aborts, unsigned long *media_flushes,
                                              unsigned long *directory_reads, unsigned long *directory_writes, unsigned long *directory_cache_misses,
                                              unsigned long *file_opens, unsigned long *file_closes, 
                                              unsigned long *file_bytes_read, unsigned long *file_bytes_written,
                                              unsigned long *logical_sector_reads, unsigned long *logical_sector_writes, unsigned long *logical_sector_cache_misses);
int     tml_system_netx_performance_statistics_get(unsigned long *arp_requests_sent, unsigned long *arp_responses_sent,
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
                                              unsigned long *udp_packets_received, unsigned long *udp_bytes_received);
int     tml_thread_performance_statistics_get(unsigned long thread_index, unsigned long *thread_suspensions, unsigned long *thread_resumptions);
int     tml_system_execution_profile_get(_int64 *interrupt, _int64 *idle);
int     tml_thread_execution_profile_get(unsigned long thread_index, _int64 *thread_time);
int     tml_thread_stack_usage_get(unsigned long thread_index, unsigned long *stack_size, unsigned long *minimum_available, unsigned long *event);
int     tml_system_event_popularity_get(TML_EVENT_POPULARITY **popularity_list, unsigned long *list_size);
int     tml_thread_event_popularity_get(unsigned long thread_index, TML_EVENT_POPULARITY **popularity_list, unsigned long *list_size);
int     tml_thread_total_events_get(unsigned long thread_index, unsigned long *total_thread_events_ptr);

int     tml_thread_most_recent_priority_find(unsigned long thread_index, unsigned long event, unsigned long *priority, unsigned long *preemption_threshold);

void    tml_time_to_ascii(_int64 time, char *string);
int     tml_raw_trace_file_dump(FILE *dump_file, char *tracex_version, char *input_file_name, char *dump_file_name);
int     tml_convert_file(FILE *source_file, FILE **converted_file);
int     tml_convert_from_ascii(int byte);
int     tml_thread_execution_status_get(unsigned long thread_index, unsigned long starting_event, unsigned long ending_event, unsigned long *execution_status_list, unsigned long max_size, unsigned long *returned_status_pairs);
}
#endif

