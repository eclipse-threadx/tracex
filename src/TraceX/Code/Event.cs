using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using AzureRTOS.Tml;


namespace AzureRTOS.TraceManagement.Code
{
    public class Event
    {
        private readonly TmlEvent _internalEvent;
        private string _iconCaption = "UE";
        private Color _foreground = Color.FromArgb(255, 255, 255, 255);
        private Color _background = Color.FromArgb(255, 90, 90, 253);
        private Color _foreground2 = Color.FromArgb(255, 255, 255, 255);
        private Color _background2 = Color.FromArgb(255, 0, 0, 0);

        public static double EventDisplayWidth = 12;
        public static double ExtremeZoomOutFactor = 1.00;
        public static ArrayList CustomEvents = new ArrayList();
        public static bool TickMappingSet = false;
        public static double TickMappingValue = 0;

        protected Event(TmlEvent tmlEvent)
        {
            _internalEvent = tmlEvent;
        }

        public string ThreadName { get; private set; }
        public List<Information> Informations { get; } = new List<Information>();
        public uint Index => _internalEvent.Index;
        public uint Context => _internalEvent.Context;
        public uint ThreadPriority => _internalEvent.ThreadPriority;
        public uint Id => _internalEvent.Id;
        public string EventTypeName { get; private set; }
        public uint TimeStamp => _internalEvent.TimeStamp;
        public long RelativeTicks => _internalEvent.RelativeTicks;
        public uint ThreadIndex => _internalEvent.ThreadIndex;

        public FrameworkElement CreateIcon()
        {
            return DrawIcon(_iconCaption, _foreground, _foreground2, _background, _background2, 12, true);
        }

        public FrameworkElement CreateIcon(int width)
        {
            return DrawIcon(_iconCaption, _foreground, _foreground2, _background, _background2, width, false);
        }

        public string GetEventDetailString(Code.TraceFileInfo tfi)
        {
            var detail = new StringBuilder();
            detail.AppendFormat(CultureInfo.CurrentCulture, "Event ID:  {0}", Index);
            detail.AppendLine();

            if (_internalEvent.Context == 0xF0F0F0F0)
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "Context:   Initialize");
                detail.AppendLine();
            }
            else if (_internalEvent.Context == 0)
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "Context:   Idle");
                detail.AppendLine();
            }
            else if (_internalEvent.Context == 0xFFFFFFFF)
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "Context:   Interrupt");
                detail.AppendLine();
                if (ThreadPriority != 0)
                {
                    detail.AppendFormat(
                        CultureInfo.CurrentCulture,
                        "Interrupted Context:   0x{0:X8} {1}\r\n",
                        ThreadPriority,
                        tfi.FindObject(ThreadPriority));
                }
                // insert if priority is not null (0) then print out
            }
            else
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "Context:   {0} (Priority:{1})\r\n", ThreadName, ThreadPriority);
            }

            if (TickMappingSet)
            {
                detail.AppendFormat(
                    CultureInfo.CurrentCulture,
                    "Relative Time:   {0} ({1:0.00} µs)\r\n",
                    RelativeTicks,
                    RelativeTicks / TickMappingValue);
            }
            else
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "Relative Time:   {0}\r\n", RelativeTicks);
            }

            detail.AppendFormat(CultureInfo.CurrentCulture, "Raw Time Stamp:   0x{0:X8}\r\n", TimeStamp);
            detail.AppendFormat(CultureInfo.CurrentCulture, "Event:   {0:X8}\r\n", EventTypeName);

            foreach (Information info in this.Informations)
            {
                detail.AppendFormat(CultureInfo.CurrentCulture, "{0}:   {1}\r\n", info.Caption, info.Value);
            }

            detail.AppendFormat(
                CultureInfo.CurrentCulture,
                "Next Context:   {0} 0x{1:X8}\r\n",
                tfi.FindObject(_internalEvent.NextContext),
                _internalEvent.NextContext);

            int curIndex = (int)_internalEvent.Index;
            if (curIndex > 0)
            {
                long deltaPrev = tfi.Events[curIndex].RelativeTicks - tfi.Events[curIndex - 1].RelativeTicks;
                if (TickMappingSet)
                {
                    detail.AppendFormat(
                        CultureInfo.CurrentCulture,
                        "Delta Previous:   {0} ticks ({1:0.00} µs)\r\n",
                        deltaPrev,
                        deltaPrev / TickMappingValue);
                }
                else
                {
                    detail.AppendFormat(CultureInfo.CurrentCulture, "Delta Previous:   {0} ticks\r\n", deltaPrev);
                }
            }

            if (curIndex < tfi.Events.Count - 1 && curIndex >= 0)
            {
                long deltaNext = tfi.Events[curIndex + 1].RelativeTicks - tfi.Events[curIndex].RelativeTicks;

                if (TickMappingSet)
                {
                    detail.AppendFormat(
                        CultureInfo.CurrentCulture,
                        "Delta Next:   {0} ticks ({1:0.00} µs)\r\n",
                        deltaNext,
                        deltaNext / TickMappingValue);
                }
                else
                {
                    detail.AppendFormat(CultureInfo.CurrentCulture, "Delta Next:   {0} ticks\r\n", deltaNext);
                }
            }

            return detail.ToString();
        }

        public static Event CreateInstance(TmlEvent tmlEvent, string threadName, TraceFileInfo tfi)
        {
            var e = new Event(tmlEvent)
            {
                ThreadName = threadName
            };

            bool findEvent = false;

            TmlEventInfo eInfo;
            if(EventInfoDict.eventInfoDict.TryGetValue(tmlEvent.Id, out eInfo))
            {
                findEvent = true;

                e._iconCaption = eInfo.IconCaption;
                e._background = eInfo.ColorBackground;
                e._background2 = eInfo.ColorBackground2;
                e._foreground = eInfo.ColorForeground;
                e._foreground2 = eInfo.ColorForeground2;
                e.EventTypeName = eInfo.EventType;

                uint eventInfoType = 0;
                uint eventInfo = 0;
                for (uint index = 1; index < 5; index++)
                {
                    switch (index)
                    {
                        case 1:
                            eventInfoType = eInfo.Info1Type;
                            eventInfo = e._internalEvent.Info1;
                            break;

                        case 2:
                            eventInfoType = eInfo.Info2Type;
                            eventInfo = e._internalEvent.Info2;
                            break;

                        case 3:
                            eventInfoType = eInfo.Info3Type;
                            eventInfo = e._internalEvent.Info3;
                            break;

                        case 4:
                            eventInfoType = eInfo.Info4Type;
                            eventInfo = e._internalEvent.Info4;
                            break;
                    }

                    if (eventInfoType != (uint)EventInfoTypes.types.EVENT_INFO_NONE)
                    {
                        string value;
                        //value = "0x" + e._internalEvent.Info1.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);
                        value = "0x" + eventInfo.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture);

                        switch (eventInfoType)
                        {
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_NEXT_THREAD_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_STACK_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_POOL_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_GROUP_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_MUTEX_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_OWNING_THREAD:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_QUEUE_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_SEMAPHORE_PTR:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_SLEEP_VALUE:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_CURRENT_TIME:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_TIMER_PTR:
                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_MEDIA_PTR:
                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_FILE_PTR:
                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_BUFFER_PTR:
                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_MEMORY_PTR:
                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_AVAILABLE_BYTES_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_DATA_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_POOL_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_NEW_PACKET_PTR:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_MEMORY_PTR:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_PTR:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_PACKET_PTR:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_UDP_SOCKET_PTR:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_TCP_SOCKET_PTR:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_SOCKET_PTR:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_CLASS_INSTANCE:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_CONFIGURATION:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_HCD:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_PARENT:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_ENDPOINT:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_INTERFACE:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_DEVICE_OWNER:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_TRANSFER_REQUEST:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_AUDIO_CONTROL:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_AUDIO_SAMPLING:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_PARAMETER:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_HID_CLIENT_INSTANCE:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_CLIENT_REPORT:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_DEVICE:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_OBJECT:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_MEDIA:
                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_PIMA_EVENT:
                                // Append pointer name
                                value += tfi.FindObject(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_PREVIOUS_STATE:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_NEW_STATE:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_SYSTEM_STATE:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_THREAD_STATE:
                                // Append state field
                                value += tfi.FindStateField(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_WAIT_OPTION:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_WAIT_OPTION:
                                // Append wait status
                                value += tfi.FindWaitOption(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_INHERITANCE:
                                // Append inherit status
                                value += tfi.FindInheritOption(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_ENABLE:
                                // Append active status
                                value += tfi.FindAutoActivateOption(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.UX_EVENT_INFO_ERROR_TYPE:
                                // Append error type
                                value += tfi.FindUXErrorType(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_GET_OPTION:
                            case (uint)EventInfoTypes.types.TX_EVENT_INFO_SET_OPTION:
                                value += tfi.FindGetOption(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_ATTRIBUTES:
                                value += tfi.FindAttributes(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.FX_EVENT_INFO_OPEN_TYPE:
                                value += tfi.FindOpenType(eventInfo);
                                break;


                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_PREVIOUS_STATE:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_NEW_STATE:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_SOCKET_STATE:
                                value += tfi.FindSocketState(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_STATUS:
                                // Append packet status
                                value += tfi.FindPacketStatus(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_SOURCE_IP_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_TARGET_IP_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_IP_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_GROUP_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_MASK:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_GATEWAY_ADDRESS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_DESTINATION_IP:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_SERVER_IP:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_NETWORK_ADDRESS:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_DESTINATION_IP:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_IP_ADDRESS_LSW:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_ROUTER_ADDR_LSW:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_ADDRESS_LSW:
                            case (uint)EventInfoTypes.types.NXD_EVENT_INFO_PEER_IP_ADDRESS:
                                value += tfi.FindIPAddress(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_PACKET_TYPE:
                                value += tfi.FindPacketType(eventInfo);
                                break;

                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_NEEDED_STATUS:
                            case (uint)EventInfoTypes.types.NX_EVENT_INFO_ACTUAL_STATUS:
                                value += tfi.FindStatus(eventInfo);
                                break;

                        }

                        string caption = EventTypeDict.typeStringDict[eventInfoType];
                        e.Informations.Add(new Information(caption, value));
                    }
                }
            }

            // tmlEvent.ID = 1000;
            if (!findEvent)
            {
                if (CustomEvents != null)
                {
                    if (CustomEvents.Count > 0)
                    {
                        for (int i = 0; i < CustomEvents.Count; i++)
                        {
                            var cstmEvent = (CustomEvent)CustomEvents[i];

                            if (tmlEvent.Id == cstmEvent.EventNumber)
                            {
                                e._iconCaption = cstmEvent.TwoLetterAbbreviation.Trim();
                                e.EventTypeName = cstmEvent.EventName;
                                e._background2 = Color.FromArgb(255, (byte)cstmEvent.IconColor2[0], (byte)cstmEvent.IconColor2[1], (byte)cstmEvent.IconColor2[2]);
                                e._background = Color.FromArgb(255, (byte)cstmEvent.IconColor1[0], (byte)cstmEvent.IconColor1[1], (byte)cstmEvent.IconColor1[2]);
                                e.Informations.Add(new Information(cstmEvent.Info1, "0x" + e._internalEvent.Info1.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                                e.Informations.Add(new Information(cstmEvent.Info2, "0x" + e._internalEvent.Info2.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                                e.Informations.Add(new Information(cstmEvent.Info3, "0x" + e._internalEvent.Info3.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                                e.Informations.Add(new Information(cstmEvent.Info4, "0x" + e._internalEvent.Info4.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                            }
                        }
                    }
                    else
                    {
                        e._iconCaption = "UE";
                        e.EventTypeName = "User Event";
                        e.Informations.Add(new Information("Info 1", "0x" + e._internalEvent.Info1.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                        e.Informations.Add(new Information("Info 2", "0x" + e._internalEvent.Info2.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                        e.Informations.Add(new Information("Info 3", "0x" + e._internalEvent.Info3.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                        e.Informations.Add(new Information("Info 4", "0x" + e._internalEvent.Info4.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                    }
                }
                else
                {
                    e._iconCaption = "UE";
                    e.EventTypeName = "User Event";
                    e.Informations.Add(new Information("Info 1", "0x" + e._internalEvent.Info1.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                    e.Informations.Add(new Information("Info 2", "0x" + e._internalEvent.Info2.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                    e.Informations.Add(new Information("Info 3", "0x" + e._internalEvent.Info3.ToString("X8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                    e.Informations.Add(new Information("Info 4", "0x" + e._internalEvent.Info4.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture)));
                }
            }

            e.EventTypeName = e.EventTypeName + " (" + tmlEvent.Id.ToString(CultureInfo.InvariantCulture) + ")";
            return e;
        }

        public static void SetCustomEventList(ArrayList customEventList)
        {
            for (int i = 0; i < customEventList.Count; i++)
            {
                string[] items = customEventList[i].ToString().Split(',');
                var cstmEvent = new CustomEvent
                {
                    EventNumber = Convert.ToUInt32(items[0], CultureInfo.InvariantCulture),
                    EventName = items[1],
                    TwoLetterAbbreviation = items[2],
                    Info1 = items[9],
                    Info2 = items[10],
                    Info3 = items[11],
                    Info4 = items[12],
                };

                cstmEvent.IconColor2[0] = Convert.ToInt32(items[3].Trim().TrimStart('('), CultureInfo.InvariantCulture);
                cstmEvent.IconColor2[1] = Convert.ToInt32(items[4], CultureInfo.InvariantCulture);
                cstmEvent.IconColor2[2] = Convert.ToInt32(items[5].TrimEnd(')'), CultureInfo.InvariantCulture);

                cstmEvent.IconColor1[0] = Convert.ToInt32(items[6].Trim().TrimStart('('), CultureInfo.InvariantCulture);
                cstmEvent.IconColor1[1] = Convert.ToInt32(items[7], CultureInfo.InvariantCulture);
                cstmEvent.IconColor1[2] = Convert.ToInt32(items[8].Trim(')'), CultureInfo.InvariantCulture);

                CustomEvents.Add(cstmEvent);
            }
        }

        public static void SetRelativeTickMapping(double msValue, bool tickMapSet)
        {
            TickMappingSet = tickMapSet;
            TickMappingValue = msValue;
        }

        // Utility Functions
        private static FrameworkElement DrawIcon(string caption, Color forecolor, Color forecolor2, Color backcolor, Color backcolor2, int width, bool legend)
        {
            var canvas = new Canvas
            {
                Width = width,
                Height = width * 2,
            };

            var rect = new Rectangle
            {
                Width = width,
                Height = width * 2,
                Fill = new SolidColorBrush(backcolor),
                StrokeThickness = 1.0,
            };

            if (width <= 2)
            {
                rect.Stroke = new SolidColorBrush(backcolor);
            }
            else
            {
                rect.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            }

            canvas.Children.Add(rect);
            Canvas.SetTop(rect, 0);
            Canvas.SetLeft(rect, 0);

            if (legend)
            {
                Canvas.SetTop(rect, 6);
            }

            if (backcolor2 != Color.FromArgb(255, 0, 0, 0))
            {
                var rect2 = new Rectangle
                {
                    Width = width,
                    Height = width * 2,
                    Fill = new SolidColorBrush(backcolor2),
                    Stroke = rect.Stroke,
                    StrokeThickness = 1.0
                };

                canvas.Children.Add(rect2);
                Canvas.SetTop(rect2, 0);

                if (legend)
                {
                    Canvas.SetTop(rect2, 6);
                }

                Canvas.SetLeft(rect2, 0);
            }

            double fontSize = 10.0;

            if (!legend)
            {
                fontSize = SystemFonts.MessageFontSize - 2;
            }

            if (width > 6)
            {
                if (caption.Length > 0)
                {
                    var text = new TextBlock
                    {
                        Text = caption.Substring(0, 1),
                        FontFamily = new FontFamily("Tahoma"),
                        FontWeight = FontWeight.FromOpenTypeWeight(600),
                        Foreground = new SolidColorBrush(forecolor),
                        FontSize = fontSize
                    };
   
                    canvas.Children.Add(text);
                    Canvas.SetTop(text, 1);

                    if (legend)
                    {
                        Canvas.SetTop(text, 7);
                    }

                    Canvas.SetLeft(text, 2);
                }

                if (caption.Length > 1)
                {

                    var text = new TextBlock
                    {
                        Text = caption.Substring(1, 1),
                        FontFamily = new FontFamily("Tahoma"),
                        FontWeight = FontWeight.FromOpenTypeWeight(600),
                        Foreground = new SolidColorBrush(forecolor2),
                        FontSize = fontSize
                    };

                    canvas.Children.Add(text);
                    Canvas.SetTop(text, fontSize + 2);

                    if (legend)
                    {
                        Canvas.SetTop(text, 18);
                    }

                    Canvas.SetLeft(text, 2);
                }
            }
            return canvas;
        }

        public static UIElement CreateBlankIcon(double width)
        {
            var rect = new Rectangle
            {
                Height = SystemFonts.MessageFontSize * 2,
                Width = width,
                Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 128)),
                StrokeThickness = 1.0,
                Stroke = width <= 2
                    ? new SolidColorBrush(Color.FromArgb(255, 0, 255, 128))
                    : new SolidColorBrush(Color.FromArgb(255, 0, 0, 0)),
            };

            var canvas = new Canvas();
            canvas.Children.Add(rect);
            Canvas.SetTop(rect, 0);
            Canvas.SetLeft(rect, 0);
            return canvas;
        }
    }
}
