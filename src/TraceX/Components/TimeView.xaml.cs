/* Using Directives.  */

using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Automation.Peers;
using AzureRTOS.Tml;
using System.Globalization;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for TimeView.xaml
    /// </summary>
    public partial class TimeView : TraceViewControl
    {
        /* Constants.  */
        private const double EVENT_ICON_OVERLAP = -11;
        public double tickMappingValue;
        public bool tickMappingSet = false;
        public bool statusLinesReadyOnlySet = false;
        public bool statusLinesAllOnSet = false;
        static Line sep1 = new Line();
        static Line sep2 = new Line();


        public ArrayList PriorityInversionArray = new ArrayList();

        //private static bool goodInversion = true;

        /// <summary>
        /// Zoom in /out
        /// </summary>
        /// <param name="multiple"></param>
        public override void Zoom(double multiple)
        {
            if (!_viewActivated)
                return;

            //GC.WaitForPendingFinalizers();
            //GC.Collect();

            double oldMultiple = DrawingScaling;

            if (multiple <= AzureRTOS.TraceManagement.Code.Event.ExtremeZoomOutFactor)
            {
                ZoomFactor = AzureRTOS.TraceManagement.Code.Event.ExtremeZoomOutFactor;
                DrawingScaling = GetMinTickWidth();
                enableZoomIn = true;
                enableZoomOut = false;
            }
            else
            {
                enableZoomIn = true;
                enableZoomOut = true;

                double minTickScale = GetMinTickWidth();
                DrawingScaling = minTickScale * multiple;// +(1 - minTickScale) * (multiple - AzureRTOS.TraceManagement.Code.Event.ExtremeZoomOutFactor)

                if (minTickScale * multiple > GetEventDisplayWidth())
                {
                    enableZoomIn = false;
                    enableZoomOut = true;
                }
                else
                {
                    ZoomFactor = multiple;
                }
            }

            base.Zoom(ZoomFactor);

            double ratio = (DrawingScaling / oldMultiple);
            _deltaMask.Width *= ratio;
            double deltaMaskLeft = Canvas.GetLeft(_deltaMask);
            deltaMaskLeft = (deltaMaskLeft + sbEvents.Value) * ratio - sbEvents.Value;
            Canvas.SetLeft(_deltaMask, deltaMaskLeft);

            this.UpdateHScrollBar();
            SbEvents_ValueChanged(svEvent, new RoutedPropertyChangedEventArgs<double>(sbEvents.Value, sbEvents.Value));
            this.MoveToEvent(tfi.CurrentEventIndex, true);
        }

        /// <summary>
        /// Constructor for TimeView.
        /// </summary>
        public TimeView()
        {
            enableZoomIn = true;

            /* Standard initialization of the component.  */
            InitializeComponent();
            dictThread = new Dictionary<string, uint>();
            dictThreadOffset = new Dictionary<string, uint>();

        }

        public double GetLeftViewPortWidth()
        {
            return grid1.ColumnDefinitions[0].ActualWidth == 0 ? grid1.ColumnDefinitions[0].Width.Value : grid1.ColumnDefinitions[0].ActualWidth;
        }

        public double GetRightViewPortWidth()
        {
            double viewPortWidth = svRight.ViewportWidth;
            viewPortWidth = viewPortWidth <= 0 ? SiblingViewPortWidth : viewPortWidth;
            viewPortWidth = viewPortWidth == 0 ? (AzureRTOS.TraceManagement.Properties.Settings.Default.MainWidth + 265) : viewPortWidth;
            return viewPortWidth;// -svRight.HorizontalOffset;
        }

        public double GetMinTickWidth()
        {
            return GetRightViewPortWidth() / tfi.Events[tfi.Events.Count - 1].RelativeTicks;
        }

        public double GetTicksPerPixel()
        {
            return tfi.Events[tfi.Events.Count - 1].RelativeTicks / GetRightViewPortWidth();
        }

        public void ResetForFileOpen()
        {
            DrawingScaling = 1.0;
            ZoomFactor = 1.0 / GetMinTickWidth();
            ZoomFactor = (double)(((long)(ZoomFactor * 100 + 0.5)) / 100.0);
            EventIndicatorIndex = 0;
            sbEvents.Value = 0;
            svRight.ScrollToVerticalOffset(0);
        }

        public void FindThreadOrderClean()
        {
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        /* When jumping pages this function will find the next index to start at.  */

        public override int GetNextPageStartIndex(int currentIndex)
        {
            /* Set the width equal to the size of the viewport.  */
            double width = svRight.ViewportWidth;

            /* Get the new index.  */
            int index = currentIndex;
            double testWidth = 0;
            while (testWidth < width && index < tfi.Events.Count - 1)
            {
                testWidth = (tfi.Events[++index].RelativeTicks - tfi.Events[currentIndex].RelativeTicks) * DrawingScaling;
            }

            index += 1;

            /* If the index is larger than the number of events set it to the last event.  */
            if (index > this.tfi.Events.Count - 1) index = this.tfi.Events.Count - 1;

            /* Return the new index.  */
            return index;
        }

        /* When jumping pages this function will find the previous index to start at.  */

        public override int GetPreviousPageStartIndex(int currentIndex)
        {
            /* Set the width equal to the size of the viewport.  */
            double width = svRight.ViewportWidth;

            /* Get the new index.  */
            int index = currentIndex;
            double testWidth = 0;
            while (testWidth < width && index > 0)
            {
                testWidth = (tfi.Events[currentIndex].RelativeTicks - tfi.Events[--index].RelativeTicks) * DrawingScaling;
            }

            index -= 1;

            /* If the index is smaller than 0, set it to the first event (0).  */
            if (index < 0) index = 0;

            /* Return the new index.  */
            return index;
        }

         /* To initialize everything in timeView.  */

        public override void Initialize(Code.TraceFileInfo tfi, Navigator navigator)
        {
            /* Set trace file information to null to clear out anything that it may be set to.  */
            this.tfi = null;

            /* Set the offset to 0 and the events value to zero.  */
            sbEvents.Value = 0;
            sbEvents.ViewportSize = grid1.ColumnDefinitions[1].ActualWidth;

            /* Set the rate at which the scroll changes for large and small changes.  */
            sbEvents.SmallChange = GetEventDisplayWidth();
            sbEvents.LargeChange = GetEventDisplayWidth() * 10;

            /* Clear any details Windows that may be present.  */
            this.detailsWindowList.Clear();

            /* Ensure that the trace file contains events. */
            if (tfi.Events.Count == 0)
            {
                /* It does not return.  */
                return;
            }

            /* Set the current to the global value.  */
            this.tfi = tfi;

            /* Set the current to the global navigator.  */
            this.navigator = navigator;

            /* Set visibility to all fields.  */
            canvasContextCaption.Visibility = Visibility.Visible;
            canvasEventCaption.Visibility = Visibility.Visible;
            canvasTicksCaption.Visibility = Visibility.Visible;

            CalcNumberOfThreadsToDisplay(SiblingViewPortHeight);
            //CalcNumberOfThreadsToDisplay(grid1.RowDefinitions[3].ActualHeight);

            /* Set width and height.  */
            double height = SiblingViewPortHeight;
            if (SiblingViewPortWidth == 0)
                canvasEvents.Width = canvasEvent.Width = canvasContext.Width = canvasTicks.Width = 5000;
            else
                canvasEvents.Width = canvasEvent.Width = canvasContext.Width = canvasTicks.Width = SiblingViewPortWidth;

            canvasEvents.Height = tfi.Threads.Count * GetEventDisplayHeight() + 40;
            canvasObjects.Height = canvasEvents.Height;// +20;
            canvasObjects.Width = GetThreadNameDisplayWidth();


            /* Set background to white.  */
            canvasEvent.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            
            /* If the Canvas height is to large set it to the height.  */
            if (canvasEvents.Height < height)
            {
                canvasEvents.Height = height;
            }

            this.indicator = Code.Indicator.Create(this.canvasTicks, this.canvasEvents);

            DrawingScaling = 1.0;
            tickMappingSet = false;

            this.DrawThreads();
            findThreadOrder();
            findThreadChangeOrder();

            if (_viewActivated)
            {
                /* Draw the threads, events, indicator, move to event 0.  */
                this.DrawEvents(sbEvents.Value);
                MoveToEvent(EventIndicatorIndex, false);
            }

            /* Update the horizontal scroll bar.  */
            this.UpdateHScrollBar();

            /* Set visibility to all.  */
            canvasObjects.Visibility = Visibility.Visible;
            canvasEvents.Visibility = Visibility.Visible;
            canvasContext.Visibility = Visibility.Visible;
            canvasEvent.Visibility = Visibility.Visible;

            canvasObjects.MouseWheel += new MouseWheelEventHandler(CanvasObjects_MouseWheel);

            ZoomFactor = 1.0 / GetMinTickWidth();
            ZoomFactor = (double)(((long)(ZoomFactor * 100 + 0.5)) / 100.0);

            if (ZoomLimitReached != null)
            {
                enableZoomIn = true;
                enableZoomOut = true;

                if (tfi.TmlTraceInfo.MaxRelativeTicks * DrawingScaling < GetRightViewPortWidth())
                {
                    enableZoomIn = true;
                    enableZoomOut = false;
                }
                ZoomLimitReached(this, new AzureRTOS.TraceManagement.Code.ZoomEventArgs(enableZoomOut, enableZoomIn));
            }
        }

        private void CanvasObjects_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            //Do Nothing... We do not want to allow this scroll to occur.
        }

        /* Function to update the horizontal scroll bar width and visibility.  */

        private void UpdateHScrollBar()
        {
            double width = SiblingViewPortWidth;
            double ticks = tfi.TmlTraceInfo.MaxRelativeTicks * DrawingScaling + GetEventDisplayWidth();
            sbEvents.Minimum = 0;
            if (ticks > width)
            {
                sbEvents.Visibility = Visibility.Visible;
                if (this.svRight.ViewportHeight < this.canvasEvents.Height)
                {
                    sbEvents.Maximum = ticks - width + 16; // add 16 to make up the width of hscrollbar
                }
                else
                {
                    sbEvents.Maximum = ticks - width;
                }
            }
            else
            {
                sbEvents.Visibility = Visibility.Hidden;
            }

            sbEvents.ViewportSize = SiblingViewPortWidth;
        }

        // Function to draw the thread names alone the left hand side of the application window.
        private void DrawThreads()
        {
            /* Get width, height, and offset.  */
            double width = grid1.ColumnDefinitions[0].Width.Value;
            double height = svLeft.ViewportHeight;
            double offsetV = svLeft.VerticalOffset;

            /* Clear objects.  */
            canvasObjects.Children.Clear();

            /* Create a white rectangle to clear any other information.  */
            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            rect.Width = width;
            rect.Height = height + offsetV;
            canvasObjects.Children.Add(rect);

            /* Loop through and create a line to go around the thread names.  */
            for (double y = GetEventDisplayHeight(); y <= height + offsetV; y += GetEventDisplayHeight())
            {
                Line line = new Line();
                line.Stroke = new SolidColorBrush(Color.FromArgb(255, 212, 213, 218));
                line.StrokeThickness = 0.5;
                line.X1 = 0;
                line.Y1 = line.Y2 = y;
                line.X2 = width;

                canvasObjects.Children.Add(line);
            }

            /* Check to ensure that the trace file information is available.  */
            if (null == this.tfi) return;

            GetThreadNameDisplayWidth();

            /* Insert the thread names into the buttons (see makeobjectcanvas).  */
            for (int i = 0; i < this.tfi.Threads.Count; ++i)
            {
                 canvasObjects.Children.Add(MakeCanvasObject(i));
            }
        }

        protected override void ExchangeThreadPostion(Int32 srcIndex, Int32 destIndex)
        {
            if (srcIndex != destIndex)
            {
                TmlThread threadMoved = tfi.Threads[srcIndex];
                tfi.Threads.Remove(threadMoved);
                if (srcIndex > destIndex)
                {
                    tfi.Threads.Insert(destIndex + 1, threadMoved);
                }
                else
                {
                    tfi.Threads.Insert(destIndex, threadMoved);
                }

                EventIndicatorIndex = tfi.CurrentEventIndex;
                this.detailsWindowList.Clear();
                findThreadChangeOrder();

                this.DrawThreads();
                this.DrawEvents(sbEvents.Value);
                MoveToEvent(EventIndicatorIndex, false);
            }
        }

        public void DrawEventsOutside(double msValue, bool tickMapSet)
        {
            tickMappingSet = tickMapSet;
            tickMappingValue = msValue;
            Code.Event.SetRelativeTickMapping(tickMappingValue, tickMappingSet);
            this.DrawEvents(sbEvents.Value);
        }

        public override void RedrawThreads()
        {
            if (_threadsOrderChanged && this.navigator != null)
            {
                EventIndicatorIndex = tfi.CurrentEventIndex;

                findThreadChangeOrder();
                this.detailsWindowList.Clear();
                this.DrawThreads();
                this.DrawEvents(sbEvents.Value);
                MoveToEvent(EventIndicatorIndex, false);
                _threadsOrderChanged = false;
            }
        }

        /* To take firstEventbyPoint with only one var.  */

        private int FindFirstEventByPointX(double pointX)
        {
            return FindLeftEventByPointX(0, pointX);
        }

        /* Helper function to find the first event by a given point.  */

        private int FindLeftEventByPointX(int searchStart, double pointX)
        {
            // max events selected
            if (tfi.Events[tfi.Events.Count - 1].RelativeTicks * DrawingScaling < pointX)
                return tfi.Events.Count - 1;

            /* Loop through and find the event by the given point.  */
            for (int i = searchStart; i < tfi.Events.Count; i++)
            {
                TmlEvent evt = tfi.Events[i];
                if (evt.RelativeTicks * DrawingScaling + evt.IconWidth >= pointX)
                {
                    return i;
                }
            }

            /* If nothing is returned, send an error.  */
            return 0;
        }

        /* Helper function to find the first event by a given point.  */

        private int FindRightEventByPointX(int searchStart, double pointX)
        {
            // max events selected
            if (tfi.Events[tfi.Events.Count - 1].RelativeTicks * DrawingScaling <= pointX)
                return tfi.Events.Count - 1;

            /* Loop through and find the event by the given point.  */
            for (int i = searchStart; i < tfi.Events.Count; i++)
            {
                TmlEvent evt = tfi.Events[i];
                if (evt.RelativeTicks * DrawingScaling > pointX)
                {
                    return i - 1;
                }
            }

            /* If nothing is returned, send an error.  */
            return 0;
        }

        /* Function to draw events.  */

        private void DrawEvents(double offsetH)
        {
            /* Set width, height, offset.  */
            double width = canvasEvents.Width;
            double height = canvasEvents.Height;
            double offsetV = svRight.VerticalOffset;

            double viewport_height = grid1.RowDefinitions[3].ActualHeight;
            int startThreadsIndex = Convert.ToInt32(Math.Floor(offsetV / GetEventDisplayHeight()), CultureInfo.InvariantCulture);
            int endThreadsIndex = Convert.ToInt32(Math.Ceiling((offsetV + viewport_height) / GetEventDisplayHeight()), CultureInfo.InvariantCulture);

            int startEventID = 0;
            int endEventID = -1;
            bool includeStartEvent = true;
            bool includeEndEvent = true;

            /* clean up.  */
            canvasContext.Children.Clear();
            canvasEvent.Children.Clear();
            canvasTicks.Children.Clear();
            canvasEvents.Children.Clear();

            /* Check if trace file information exists.  */
            if (null != this.tfi)
            {
                /* Add the indicator to the canvas.  */
                this.indicator.AddToCanvas();

                /* Clear the context and event information.  */
                canvasContext.Children.Clear();
                canvasEvent.Children.Clear();

                /* Set the ticks bar width and draw ticks bar.  */
                DrawTicksBar(width, offsetH);


                if (canvasContextCaption.Children.Contains(sep1))
                {
                    canvasContextCaption.Children.Remove(sep1);
                }

                // draw a line under "Context Summary"
                sep1.X1 = 0;
                sep1.Y1 = canvasContextCaption.ActualHeight - 1;
                sep1.X2 = sep1.X1 + canvasContextCaption.ActualWidth;
                sep1.Y2 = sep1.Y1;
                sep1.Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
                sep1.StrokeThickness = 1;
                canvasContextCaption.Children.Add(sep1);

                if (canvasEventCaption.Children.Contains(sep2))
                {
                    canvasEventCaption.Children.Remove(sep2);
                }

                // draw a line under "Event Summary"
                sep2.X1 = 0;
                sep2.Y1 = canvasEventCaption.ActualHeight - 1;
                sep2.X2 = sep2.X1 + canvasEventCaption.ActualWidth;
                sep2.Y2 = sep2.Y1;
                sep2.Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 192, 192));
                sep2.StrokeThickness = 1;
                canvasEventCaption.Children.Add(sep2);

                /* Get the start index.  */
                int startIndex = FindFirstEventByPointX(offsetH);

                // trc 1.7.22.bin crashes on startIndex = -1
                /* Ensure that the startIndex is valid.  */
                if (-1 == startIndex)
                    return;

                /* Also check if the endindex is valid.  */
                int endIndex = FindLeftEventByPointX(startIndex + 1, (offsetH + width));
                if (-1 == endIndex)
                {
                    endIndex = tfi.Events.Count - 1;
                }

                /* Draw the background grid.  */
                DrawGrid(canvasEvents, width, height, offsetH, offsetV);

                int prevIndex = -1;
                bool alt = false;

                /* Loop through to create the tmlEvents.  */
                for (int i = 0; i < startIndex; ++i)
                {
                    TmlEvent tmlEvent = tfi.Events[i];
                    int index = tfi.FindThreadIndex(tmlEvent);

                    /* At this point, prevIndex the Context of the green bar before this event. *
                     *  index is the context of thisevent.                                      *
                     *  Detect a context switch.                                                */
                    if ((prevIndex != -1) && (index != prevIndex))
                    {
                        alt = !alt;
                    }

                    /* Obtain the green bar context AFTER this event.  */
                    prevIndex = tfi.FindNextContextIndex(tmlEvent);

                    /* Detect a context switch after this event.  */
                    if (index != prevIndex)
                    {
                        alt = !alt;
                    }

                    tmlEvent = null;
                }

                /* Reset the startEventID.  */
                if (startIndex == 0)
                    startEventID = startIndex;
                else
                    startEventID = startIndex - 1;

                /* handling the blank start.  */
                double startOffset = tfi.Events[startIndex].RelativeTicks * DrawingScaling - offsetH;

                int threadinBlank = 0;
                UIElement tBlankIcon;
                /* If there is any startOffset.  */
                if (startOffset > 0)
                {
                    UIElement blankStartIcon = Code.Event.CreateBlankIcon(startOffset);
                    Canvas.SetLeft(blankStartIcon, 0);
                    canvasEvent.Children.Add(blankStartIcon);
                    blankStartIcon = null;

                    /* Loop through to place the blank icon (for offset).  */
                    if (startIndex > 0 && startIndex < tfi.Events.Count)
                    {
                        int idxEventHght = startIndex - 1;

                        int idxThreadInBlank = tfi.FindNextContextIndex(tfi.Events[idxEventHght]);
                        UIElement threadInBlankIcon = Code.Event.CreateBlankIcon(startOffset);

                        threadinBlank = idxThreadInBlank;
                        tBlankIcon = threadInBlankIcon;

                        Canvas.SetLeft(threadInBlankIcon, 0);
                        Canvas.SetTop(threadInBlankIcon, idxThreadInBlank * GetEventDisplayHeight());
                        canvasEvents.Children.Add(threadInBlankIcon);

                        threadInBlankIcon = null;
                        /* The starting green bar is located at prevIndex.  */
                        prevIndex = idxThreadInBlank;
                    }
                    includeStartEvent = false;
                }
                else
                    includeStartEvent = true;

                /* Determine whether or not to draw a green bar after this event.  */
                endIndex = endIndex < tfi.Events.Count ? endIndex : endIndex - 1;

                // start drawing events
                {
                    DrawEventsWithIcon(startIndex, endIndex, prevIndex, offsetH, ref alt,
                        ref includeStartEvent, ref includeEndEvent, ref startEventID, ref endEventID,
                        startThreadsIndex, endThreadsIndex);
                }

                /* Make up for the first event that may be missing its status because it is not on the screen.  */
                if (startIndex > 0)
                {
                    startIndex = startIndex - 1;
                }

                // add delta Mask to events canvas
                canvasEvents.Children.Add(_deltaMask);

                if (startOffset > 0)
                {
                    UIElement blankStartIcon = Code.Event.CreateBlankIcon(startOffset);
                    Canvas.SetLeft(blankStartIcon, 0);
                    canvasEvent.Children.Add(blankStartIcon);
                    blankStartIcon = null;

                    /* Loop through to place the blank icon (for offset).  */
                    if (startIndex > 0 && startIndex < tfi.Events.Count)
                    {
                        int idxEventHght = startIndex - 1;

                        int idxThreadInBlank = tfi.FindNextContextIndex(tfi.Events[idxEventHght]);
                        UIElement threadInBlankIcon = Code.Event.CreateBlankIcon(startOffset);

                        Canvas.SetLeft(threadInBlankIcon, 0);
                        Canvas.SetTop(threadInBlankIcon, threadinBlank * GetEventDisplayHeight());
                        canvasEvents.Children.Add(threadInBlankIcon);

                        threadInBlankIcon = null;
                        /* The starting green bar is located at prevIndex.  */
                        prevIndex = idxThreadInBlank;
                    }
                    includeStartEvent = false;
                }
                else
                    includeStartEvent = true;

                DrawStatusLines(startIndex, endIndex);
            }
            else
            {
                /* Background grid.  */
                DrawGrid(canvasContext, width, GetEventDisplayHeight(), offsetH, offsetV);
                DrawGrid(canvasEvent, width, GetEventDisplayHeight(), offsetH, offsetV);

                /* Draw Time Ticks Bar.  */
                DrawTicksBar(width, offsetH);
                DrawGrid(canvasEvents, width, height, offsetH, offsetV);
            }

            /* Draw any detailswindows that should be present.  */
            foreach (DetailsWindow dw in detailsWindowList)
            {
                canvasEvents.Children.Add(dw);
                if (null != dw.Tag)
                {
                    if (dw.Tag.GetType() == typeof(Line))
                    {
                        Line line = (Line)dw.Tag;
                        canvasEvents.Children.Add(line);
                    }
                }
            }
        }

        private void DrawEventsWithIcon(int startIndex, int endIndex, int prevIndex,
            double offsetH, ref bool alt, ref bool includeStartEvent, ref bool includeEndEvent,
            ref int startEventID, ref int endEventID, int startThreadsIndex, int endThreadsIndex)
        {
            int iconWidth = 0;
            int maxIconWidth = (int)MIN_ICON_WIDTH;

            for (int i = startIndex; i <= endIndex; ++i)
            {
                TmlEvent tmlEvent = tfi.Events[i];
                int index = tfi.FindThreadIndex(tmlEvent);
                if (index < 0)
                    continue;

                bool inversionMarked = false;

                // check for good priority inversion
                if (tmlEvent.PriorityInversion > 0)
                {
                    if (BeginInversionIndex <= 0)
                    {
                        BeginInversionIndex = index;
                        EndInversionIndex = (int)tmlEvent.PriorityInversion;
                    }

                    TmlEvent tmlEventEI = tfi.Events[(int)tmlEvent.PriorityInversion];
                    DrawInversionRegion(startIndex, offsetH, tmlEvent, tmlEventEI, true);
                    tmlEventEI = null;
                    inversionMarked = true;
                }

                // check for bad priority inversion
                if (tmlEvent.BadPriorityInversion > 0)
                {
                    if (BeginInversionIndex < 0)
                    {
                        BeginInversionIndex = index;
                        EndInversionIndex = (int)tmlEvent.PriorityInversion;
                    }

                    TmlEvent tmlEventEI = tfi.Events[(int)tmlEvent.BadPriorityInversion];
                    DrawInversionRegion(startIndex, offsetH, tmlEvent, tmlEventEI, false);
                    tmlEventEI = null;
                    inversionMarked = true;
                }

                if ((i == startIndex) && (inversionMarked == false))
                {
                    if (PriorityInversionArray.Count > 0)
                    {
                        for (int n = 0; n < (int)PriorityInversionArray.Count; n++)
                        {
                            if ((startIndex >= (int)PriorityInversionArray[n]) && ((startIndex <= tfi.Events[(int)PriorityInversionArray[n]].BadPriorityInversion) || (startIndex <= tfi.Events[(int)PriorityInversionArray[n]].PriorityInversion)))
                            {
                                TmlEvent tmlEventI = tfi.Events[(int)PriorityInversionArray[n]];
                                TmlEvent tmlPrevious = tfi.Events[i];
                                if (tmlEventI.PriorityInversion > 0)
                                {
                                    TmlEvent tmlEventEI = tfi.Events[(int)tmlEventI.PriorityInversion];

                                    if (startIndex != 0)
                                    {
                                        tmlPrevious = tfi.Events[i - 1];
                                    }
                                    BeginInversionIndex = index;
                                    EndInversionIndex = (int)tmlEventI.PriorityInversion;
                                    DrawInversionRegion(startIndex, offsetH, tmlPrevious, tmlEventEI, true);
                                    tmlEventEI = null;
                                }
                                else if (tmlEventI.BadPriorityInversion > 0)
                                {
                                    TmlEvent tmlEventEI = tfi.Events[(int)tmlEventI.BadPriorityInversion];
                                    if (startIndex != 0)
                                    {
                                        tmlPrevious = tfi.Events[i - 1];
                                    }

                                    BeginInversionIndex = index;
                                    EndInversionIndex = (int)tmlEventI.BadPriorityInversion;
                                    DrawInversionRegion(startIndex, offsetH, tmlPrevious, tmlEventEI, false);
                                    tmlEventEI = null;
                                }
                            }
                            else if (startIndex < (int)PriorityInversionArray[n])
                            {
                                break;
                            }
                        }
                    }
                }

                String tName = tfi.Threads[index].Name;
                Code.Event evt = Code.Event.CreateInstance(tmlEvent, tName, tfi);
                tName = null;

                double eventBlankWidth = 1; // initialize to greater than 0;
                if (i < tfi.Events.Count - 1)
                {
                    eventBlankWidth = (tfi.Events[i + 1].RelativeTicks - tfi.Events[i].RelativeTicks) * DrawingScaling - GetEventDisplayWidth();
                }

                double iconWidthD = eventBlankWidth >= 0 ? GetEventDisplayWidth() : (GetEventDisplayWidth() + eventBlankWidth);

                iconWidth = iconWidthD < MIN_ICON_WIDTH ? (int)MIN_ICON_WIDTH : (int)iconWidthD;
                double iconLeft = tmlEvent.RelativeTicks * DrawingScaling - offsetH;
                double blankLeft = iconLeft + iconWidth;

                if (i < tfi.Events.Count - 1 && iconWidth > maxIconWidth)
                {
                    maxIconWidth = iconWidth;
                }

                if (i == tfi.Events.Count - 1)
                {
                    iconWidth = maxIconWidth;
                    if (iconWidth == MIN_ICON_WIDTH)
                    {
                        double lastGap = (tfi.Events[tfi.Events.Count - 1].RelativeTicks - tfi.Events[tfi.Events.Count - 2].RelativeTicks) * DrawingScaling;
                        if (lastGap > svRight.ViewportWidth)
                        {
                            iconWidth = (int)GetEventDisplayWidth();
                        }
                    }
                }

                tfi.Events[i].IconWidth = iconWidth;

                int nextEvent = i + 1;
                int nextEventCopy = i + 1;
                if (index == prevIndex)
                {
                    // determine whether to skipp drawing the next event
                    if (iconWidthD < MIN_ICON_WIDTH)
                    {
                        while (iconWidthD < MIN_ICON_WIDTH && nextEventCopy < tfi.Events.Count - 1)
                        {
                            iconWidthD = (tfi.Events[++nextEventCopy].RelativeTicks - tfi.Events[i].RelativeTicks) * DrawingScaling;
                        }
                    }
                }

                double blankWidth = 0;
                if (nextEvent <= tfi.Events.Count - 1)
                    blankWidth = (tfi.Events[nextEvent].RelativeTicks - tfi.Events[i].RelativeTicks) * DrawingScaling - iconWidth;

                /* First draw an icon on the summary bar.  */
                FrameworkElement icon = evt.CreateIcon(iconWidth);
                Canvas.SetLeft(icon, iconLeft);
                icon.ToolTip = evt.GetEventDetailString(tfi);
                canvasEvent.Children.Add(icon);
                //icon = null;
                /* Determine whether or not to draw a green bar after this event.  */
                UIElement blankIcon = null;
                if (blankWidth > 0)
                {
                    blankIcon = Code.Event.CreateBlankIcon(blankWidth);
                    Canvas.SetLeft(blankIcon, blankLeft);
                    canvasEvent.Children.Add(blankIcon);
                    blankIcon = null;
                }

                /* Locate the thread this event belongs to.  */
                /* Prepare to draw an event on the main event pane.  */

                /* Determine whether or not we need to draw a context switch line before this event.  */
                if (index != prevIndex)
                {
                    /* Need to draw a context switch line before this event.  */
                    Rectangle line = new Rectangle();
                    line.Width = 1;
                    line.Height = Math.Abs(index - prevIndex) * GetEventDisplayHeight();
                    line.StrokeThickness = 1.0;
                    line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

                    canvasEvents.Children.Add(line);
                    Canvas.SetLeft(line, iconLeft);
                    Canvas.SetTop(line, (index < prevIndex ? index : prevIndex) * GetEventDisplayHeight());

                    /* Update the status line.                                  *
                     * This is a context switch at the begining of this event.  *
                     * Therefore set the endEventID to the previous event,      *
                     * and also include the Green bar before this event.        */
                    endEventID = i;
                    includeEndEvent = false;

                    line = null;
                    canvasContext.Children.Add(MakeContextBar(startEventID, includeStartEvent,
                                                              endEventID, includeEndEvent,
                                                          alt, offsetH, iconWidth));

                    /* Start the next cycle.  */
                    startEventID = i;
                    includeStartEvent = true;

                    /* Rest endEventID. */
                    endEventID = -1;

                    prevIndex = index;
                    alt = !alt;
                }

                /* Paint an event.  */
                double threadPos = index * GetEventDisplayHeight() - svRight.VerticalOffset;

                if (threadPos >= 0 && threadPos < EventDisplayHeight)
                {
                    icon = evt.CreateIcon(iconWidth);
                    Canvas.SetLeft(icon, iconLeft);
                    Canvas.SetTop(icon, index * GetEventDisplayHeight());
                    icon.ToolTip = evt.GetEventDetailString(tfi);

                    canvasEvents.Children.Add(icon);

                    /* draw the green bar.  */
                    if (blankWidth > 0)
                    {
                        blankIcon = Code.Event.CreateBlankIcon(blankWidth);
                        canvasEvents.Children.Add(blankIcon);
                        Canvas.SetLeft(blankIcon, blankLeft);
                        index = tfi.FindNextContextIndex(tmlEvent);
                        Canvas.SetTop(blankIcon, index * GetEventDisplayHeight());
                        if (tmlEvent.Context == tmlEvent.NextContext)
                        {
                            Canvas.SetTop(blankIcon, index * GetEventDisplayHeight());
                        }
                    }
                }

                if (index != prevIndex)
                {
                    /* Draw the joint line.  */
                    Rectangle line = new Rectangle();
                    line.Width = 1;
                    line.Height = Math.Abs(index - prevIndex) * GetEventDisplayHeight();
                    line.StrokeThickness = 1.0;
                    line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    canvasEvents.Children.Add(line);
                    Canvas.SetLeft(line, blankLeft);
                    Canvas.SetTop(line, (index < prevIndex ? index : prevIndex) * GetEventDisplayHeight());

                    /* Update the status line.                                       *
                     * This is a context switch at the end of this event (before the *
                     * green bar that follows.                                       *
                     * Therefore set the endEventID to the this event,               *
                     * and also exclude the Green bar after this event.              */
                    endEventID = i;
                    includeEndEvent = true;

                    canvasContext.Children.Add(MakeContextBar(startEventID, includeStartEvent,
                                                              endEventID, includeEndEvent,
                                                      alt, offsetH, iconWidth));

                    /* Start the next cycle.  */
                    startEventID = i;
                    includeStartEvent = false;

                    /* Reset endEventID.  */
                    endEventID = -1;
                    alt = !alt;
                    prevIndex = index;

                    line = null;
                }

                // determine whether to skipp drawing the next events
                if (nextEvent > i + 1)
                {
                    // skip to next event
                    i = nextEvent - 1;
                }

                tmlEvent = null;
            }

            /* If we are not at a reset position on the eventID.  */
            if (endEventID == -1)
            {
                /* Set eventId to the index.  */
                endEventID = endIndex;

                /* Boundary check to include the end event.  */
                if (endEventID == tfi.Events.Count - 1)
                {
                    includeEndEvent = true;
                }
                else
                {
                    endEventID++;
                    includeEndEvent = false;
                }

                /* Add events to the context bar.  */
                canvasContext.Children.Add(MakeContextBar(startEventID, includeStartEvent,
                                                      endEventID, includeEndEvent, alt, offsetH, iconWidth));

                alt = !alt;
            }
        }

        private void DrawStatusLines(int startIndex, int endIndex)
        {
            int a = 0;
            int overRun = 0;

            if (statusLinesAllOnSet)
            {
                a = 0;
                //if(tfi
                /* Loop through the threads to draw colored lines... subtract interrupt and initialize.  */
                for (int k = 0; k < tfi.Threads.Count - 2; k++)
                {
                    if (overRun < 3)
                    {
                        if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                        {
                            a++;
                            overRun++;
                            if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                            {
                                a++;
                                overRun++;
                                if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                                {
                                    a++;
                                    overRun++;
                                }
                            }
                        }
                    }

                    List<TmlExecutionStatus> executionStatusList = new List<TmlExecutionStatus>();

                    string threadKey = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[a].Name, tfi.Threads[a].Address);
                    if (dictThread.ContainsKey(threadKey))
                    {
                        int offsetIndex = 1;
                        if (endIndex == tfi.Events.Count - 1)
                        {
                            offsetIndex = 0;
                        }
                        //executionStatusList = tfi.DictThreadExecutionStatus[(uint)dictThread[threadKey]];
                        //ELTMLManaged::TMLFunctions::ThreadExecutionStatus(unsigned long thread_index, unsigned long starting_event, unsigned long ending_event, List<TmlExecutionStatusList^> ^executionStatusList, unsigned long max_status_pairs)
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[threadKey], (uint)startIndex, (uint)endIndex + (uint)offsetIndex, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;
                        for (int m = 0; m < executionStatusList.Count; m++)
                        {

                            check = new Rectangle();
                            check.Height = 1;

                            int color = Convert.ToInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            double iconLeft = 0;

                            if (executionStatusList.Count == 1)
                            {
                                TmlEvent tmlEvent = tfi.Events[startIndex];
                                TmlEvent tmlEvent1 = tfi.Events[endIndex];
                                check.Width = (tmlEvent1.RelativeTicks - tmlEvent.RelativeTicks) * DrawingScaling;
                                iconLeft = tmlEvent.RelativeTicks * DrawingScaling - sbEvents.Value;
                            }
                            else
                            {
                                int endPoint;
                                if (m + 1 == executionStatusList.Count)
                                {
                                    endPoint = endIndex;
                                }
                                else
                                {
                                    endPoint = Convert.ToInt32(executionStatusList[m + 1].EventNumber, CultureInfo.InvariantCulture);
                                }
                                int startPoint = Convert.ToInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture);
                                if (endPoint >= startPoint)
                                {
                                    TmlEvent tmlEvent = tfi.Events[startPoint];
                                    TmlEvent tmlEvent1 = tfi.Events[endPoint];
                                    check.Width = (tmlEvent1.RelativeTicks - tmlEvent.RelativeTicks) * DrawingScaling;
                                    iconLeft = tmlEvent.RelativeTicks * DrawingScaling - sbEvents.Value;
                                }
                            }
                            /* Get color.  */
                            if ((color == 0) || (color == 1) || (color == 2))
                            {
                                break;
                            }
                            else if (color == 3)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                            }
                            else if (color == 4)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                            }
                            else if (color == 5)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 25, 150));
                            }
                            else if (color == 6)
                            {
                                /* Semaphore.  */
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 61, 197, 175));
                            }
                            else if (color == 7)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 128, 0));
                            }
                            else if (color == 8)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 100, 255, 235));
                            }
                            else if (color == 9)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 50, 150, 200));
                            }
                            else if (color == 12)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 128, 128, 255));
                            }
                            else if (color == 13)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 115, 10, 80));
                            }
                            else if (color == 99)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                            }
                            else
                            {
                                break;
                            }

                            canvasEvents.Children.Add(check);
                            Canvas.SetLeft(check, iconLeft);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[threadKey]) * GetEventDisplayHeight()) + (GetEventDisplayHeight()));
                        }

                        a++;
                    }
                }
            }
            else if (statusLinesReadyOnlySet)
            {
                a = 0;

                for (int k = 0; k < tfi.Threads.Count - 2; k++)
                {
                    List<TmlExecutionStatus> executionStatusList = new List<TmlExecutionStatus>();

                    if (overRun < 3)
                    {
                        if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                        {
                            a++;
                            overRun++;
                            if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                            {
                                a++;
                                overRun++;
                                if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                                {
                                    a++;
                                    overRun++;
                                }
                            }
                        }
                    }

                    string threadKey = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[a].Name, tfi.Threads[a].Address);
                    if (dictThread.ContainsKey(threadKey))
                    {
                        int offsetIndex = 1;
                        if (endIndex == tfi.Events.Count - 1)
                        {
                            offsetIndex = 0;
                        }
                        //executionStatusList = tfi.DictThreadExecutionStatus[(uint)dictThread[threadKey]];
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[threadKey], (uint)startIndex, (uint)endIndex + (uint)offsetIndex, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;
                        for (int m = 0; m < executionStatusList.Count; m++)
                        {
                            //if (executionStatusList[m].eventNumber < startIndex && executionStatusList[m].eventNumber > endIndex)
                            //    continue;

                            check = new Rectangle();
                            check.Height = 1;

                            int color = Convert.ToInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            double iconLeft = 0;

                            if (executionStatusList.Count == 1)
                            {
                                TmlEvent tmlEvent = tfi.Events[startIndex];
                                TmlEvent tmlEvent1 = tfi.Events[endIndex];
                                check.Width = (tmlEvent1.RelativeTicks - tmlEvent.RelativeTicks) * DrawingScaling;
                                iconLeft = tmlEvent.RelativeTicks * DrawingScaling - sbEvents.Value;
                            }
                            else
                            {
                                int endPoint;
                                if (m + 1 == executionStatusList.Count)
                                {
                                    endPoint = endIndex;
                                }
                                else
                                {
                                    endPoint = Convert.ToInt32(executionStatusList[m + 1].EventNumber, CultureInfo.InvariantCulture);
                                }
                                int startPoint = Convert.ToInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture);
                                if (endPoint >= startPoint)
                                {
                                    TmlEvent tmlEvent = tfi.Events[startPoint];
                                    TmlEvent tmlEvent1 = tfi.Events[endPoint];
                                    check.Width = (tmlEvent1.RelativeTicks - tmlEvent.RelativeTicks) * DrawingScaling;
                                    iconLeft = tmlEvent.RelativeTicks * DrawingScaling - sbEvents.Value;
                                }
                            }
                            /* Get color.  */
                            if ((color != 99))
                            {
                                continue;
                            }
                            else if (color == 99)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                            }
                            else
                            {
                                break;
                            }

                            canvasEvents.Children.Add(check);
                            Canvas.SetLeft(check, iconLeft);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[threadKey]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));
                        }

                        a++;
                    }
                }
            }
        }

        private void DrawStatusLines(int startIndex, int endIndex, double offsetH, int iconWidth, int i, double iconLeft, double blankWidth)
        {
            int a = 0;
            int overRun = 0;

            if (statusLinesAllOnSet)
            {
                /* Loop through the threads to draw colored lines... subtract interrupt and initialize.  */
                for (int k = 0; k < tfi.Threads.Count - 2; k++)
                {
                    if (overRun < 3)
                    {
                        if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                        {
                            a++;
                            overRun++;
                            if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                            {
                                a++;
                                overRun++;
                                if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                                {
                                    a++;
                                    overRun++;
                                }
                            }
                        }
                    }

                    List<TmlExecutionStatus> executionStatusList = new List<TmlExecutionStatus>();
                    string strThreadId = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[a].Name, tfi.Threads[a].Address);

                    if (dictThread.ContainsKey(strThreadId))
                    {
                        //executionStatusList = tfi.DictThreadExecutionStatus[(uint)dictThread[strThreadId]];
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[strThreadId], (uint)i, (uint)i + 1, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;
                        for (int m = 0; m < executionStatusList.Count; m++)
                        //for (int m = i; m < i+1; m++)
                        {

                            check = new Rectangle();
                            check.Height = 1;
                            if (blankWidth > 0)
                            {
                                check.Width = iconWidth + blankWidth;
                            }
                            else
                            {
                                check.Width = iconWidth;
                                double checker = tfi.Events[i].IconWidth;
                            }

                            int color = Convert.ToInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            int currentIndex = i;// Convert.ToInt32(executionStatusList[m].eventNumber, CultureInfo.InvariantCulture) - i;

                             int endPoint;
                            if (m + 1 == executionStatusList.Count)
                            {
                                endPoint = endIndex;
                            }
                            else
                            {
                                endPoint = Convert.ToInt32(executionStatusList[m + 1].EventNumber, CultureInfo.InvariantCulture);
                            }
                            int startPoint = Convert.ToInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture);
                            currentIndex = startPoint;
 
                            /* Get color.  */
                            if ((color == 0) || (color == 1) || (color == 2))
                            {
                                break;
                            }
                            else if (color == 3)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                            }
                            else if (color == 4)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                            }
                            else if (color == 5)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 25, 150));
                            }
                            else if (color == 6)
                            {
                                /* Semaphore.  */
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 61, 197, 175));
                            }
                            else if (color == 7)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 128, 0));
                            }
                            else if (color == 8)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 100, 255, 235));
                            }
                            else if (color == 9)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 50, 150, 200));
                            }
                            else if (color == 12)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 128, 128, 255));
                            }
                            else if (color == 13)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 115, 10, 80));
                            }
                            else if (color == 99)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                            }
                            else
                            {
                                break;
                            }

                            canvasEvents.Children.Add(check);

                            Canvas.SetLeft(check, iconLeft);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[strThreadId]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));

                            //check = null;

                            if ((i > 0) && (iconLeft > 0) && i == startIndex)
                            {
                                TmlEvent tmlEv = tfi.Events[i - 1];
                                //double position = (tmlEv.RelativeTicks * DrawingScaling);//+ offsetH;
                                double position = (tmlEv.RelativeTicks * DrawingScaling) - (offsetH - 12);

                                if (position > iconLeft)
                                {
                                    continue;
                                }
                                List<TmlExecutionStatus> executionStatusList2 = new List<TmlExecutionStatus>();

                                ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[strThreadId], (uint)i - 1, (uint)i, executionStatusList2, ((uint)tfi.Events.Count * 2));

                                Rectangle check2;
                                for (int n = i - 1; n < executionStatusList2.Count; n++)
                                {
                                    check2 = new Rectangle();
                                    check2.Height = 1;
                                    check2.Width = iconLeft - position;

                                    color = Convert.ToInt32(executionStatusList2[n].Status, CultureInfo.InvariantCulture);
                                    currentIndex = i;//Convert.ToInt32(executionStatusList[m].eventNumber, CultureInfo.InvariantCulture) - startIndex;

                                    /* Get color.  */
                                    if ((color == 0) || (color == 1) || (color == 2))
                                    {
                                        break;
                                    }
                                    else if (color == 3)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                                    }
                                    else if (color == 4)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 128));
                                    }
                                    else if (color == 5)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 25, 150));
                                    }
                                    else if (color == 6)
                                    {
                                        /* Semaphore.  */
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 61, 197, 175));
                                    }
                                    else if (color == 7)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 128, 0));
                                    }
                                    else if (color == 8)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 100, 255, 235));
                                    }
                                    else if (color == 9)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 50, 150, 200));
                                    }
                                    else if (color == 12)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 128, 128, 255));
                                    }
                                    else if (color == 13)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 70, 30));
                                    }
                                    else if (color == 99)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    canvasEvents.Children.Add(check2);
                                    Canvas.SetLeft(check2, position);
                                    Canvas.SetTop(check2, (((uint)dictThreadOffset[strThreadId]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));

                                    check2 = null;
                                }

                                executionStatusList2.Clear();
                                executionStatusList2 = null;
                                tmlEv = null;
                            }

                            check = null;
                        }
                        a++;
                        executionStatusList.Clear();
                        executionStatusList = null;
                    }
                }
            }
            else if (statusLinesReadyOnlySet)
            {
                /* Loop through the threads to draw colored lines... subtract interrupt and initialize.  */
                for (int k = 0; k < tfi.Threads.Count - 2; k++)
                {
                    List<TmlExecutionStatus> executionStatusList = new List<TmlExecutionStatus>();
                    if (overRun < 3)
                    {
                        if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                        {
                            a++;
                            overRun++;
                            if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                            {
                                a++;
                                overRun++;
                                if ((tfi.Threads[a].Address == 0xFFFFFFFF) || (tfi.Threads[a].Address == 0xF0F0F0F0) || (tfi.Threads[a].Address == 0x00000000))
                                {
                                    a++;
                                    overRun++;
                                }
                            }
                        }
                    }

                    string strThreadId = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[a].Name, tfi.Threads[a].Address);

                    if (dictThread.ContainsKey(strThreadId))
                    {
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[strThreadId], (uint)i, (uint)i + 1, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;
                        for (int m = 0; m < executionStatusList.Count; m++)
                        {
                            check = new Rectangle();
                            check.Height = 1;
                            if (blankWidth > 0)
                            {
                                check.Width = iconWidth + blankWidth;
                            }
                            else
                            {
                                check.Width = iconWidth;

                                double checker = tfi.Events[i].IconWidth;
                            }

                            uint color = Convert.ToUInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            uint currentIndex = (uint)i;// (Convert.ToInt32(executionStatusList[m].eventNumber) - i, CultureInfo.InvariantCulture);

                            int endPoint;
                            if (m + 1 == executionStatusList.Count)
                            {
                                endPoint = endIndex;
                            }
                            else
                            {
                                endPoint = Convert.ToInt32(executionStatusList[m + 1].EventNumber, CultureInfo.InvariantCulture);
                            }
                            uint startPoint = Convert.ToUInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture);
                            currentIndex = startPoint;
                            /* Get color.  */
                            if ((color != 99))
                            {
                                //break;
                                continue;
                            }
                            else if (color == 99)
                            {
                                check.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                            }
                            else
                            {
                                break;
                            }

                            canvasEvents.Children.Add(check);

                            Canvas.SetLeft(check, iconLeft);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[strThreadId]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));

                            //check = null;

                            if ((i > 0) && (iconLeft > 0) && i == startIndex)
                            //if ((i >= 0) && (iconLeft >= 0) && i == startIndex)
                            {
                                TmlEvent tmlEv = tfi.Events[i - 1];
                                //double position = (tmlEv.RelativeTicks * DrawingScaling);//+ offsetH;
                                double position = (tmlEv.RelativeTicks * DrawingScaling) - (offsetH - 12);

                                if (position > iconLeft)
                                {
                                    continue;
                                }
                                List<TmlExecutionStatus> executionStatusList2 = new List<TmlExecutionStatus>();

                                Rectangle check2;
                                for (int n = i - 1; n < executionStatusList2.Count; n++)
                                {
                                    check2 = new Rectangle();
                                    check2.Height = 1;
                                    check2.Width = iconLeft - position;

                                    color = Convert.ToUInt32(executionStatusList2[n].Status, CultureInfo.InvariantCulture);
                                    currentIndex = (uint)i;//Convert.ToInt32(executionStatusList[m].eventNumber, CultureInfo.InvariantCulture) - startIndex;

                                    /* Get color.  */
                                    if ((color != 99))
                                    {
                                        //break;
                                        continue;
                                    }
                                    else if (color == 99)
                                    {
                                        check2.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 220, 5));
                                    }
                                    else
                                    {
                                        break;
                                    }
                                    canvasEvents.Children.Add(check2);
                                    Canvas.SetLeft(check2, position);
                                    Canvas.SetTop(check2, (((uint)dictThreadOffset[strThreadId]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));

                                    check2 = null;
                                }

                                executionStatusList2.Clear();
                                executionStatusList2 = null;
                                tmlEv = null;
                            }

                            check = null;
                        }
                        a++;
                        executionStatusList.Clear();
                        executionStatusList = null;
                    }
                }
            }
        }


        private void DrawInversionRegion(int startIndex, double offsetH, TmlEvent tmlEventBI, TmlEvent tmlEventEI, bool bGoodInversion)
        {
            double startOffset;
            if (startIndex > 0)
            {
                startOffset = tfi.Events[(startIndex)].RelativeTicks * DrawingScaling - offsetH;
            }

            Rectangle _rectInversion = new Rectangle();
            _rectInversion.Height = svRight.ViewportHeight + svRight.VerticalOffset;
            if (_rectInversion.Height < canvasEvents.Height)
            {
                _rectInversion.Height = canvasEvents.Height;
            }
            _rectInversion.Width = (tmlEventEI.RelativeTicks - tmlEventBI.RelativeTicks) * DrawingScaling;
            if (bGoodInversion)
            {
                _rectInversion.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 100, 130));
                _rectInversion.Opacity = 0.3;
            }
            else
            {
                _rectInversion.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
                _rectInversion.Opacity = 0.4;
            }

            Canvas.SetLeft(_rectInversion, tmlEventBI.RelativeTicks * DrawingScaling - offsetH);
            Canvas.SetTop(_rectInversion, 0);
            canvasEvents.Children.Add(_rectInversion);
        }

        /* Function for drawing the time - ticks bar.  */

        private void DrawTicksBar(double width, double offsetH)
        {
            /* Ensure the application has a file.  */
            if (!App.HasOpenFile)
                return;

            int skipValue = 0;

            // KGM Fix this hard-coded height
            double height = 40;

            /* Create a white background rectangle to clear any old information.  */
            Rectangle rectBack = new Rectangle();
            rectBack.Height = height;
            rectBack.Width = width;
            rectBack.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            this.canvasTicks.Children.Add(rectBack);
            Canvas.SetLeft(rectBack, 0);

            /* trans line.  */
            Line lineTrans = new Line();
            lineTrans.StrokeThickness = 2;
            lineTrans.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            lineTrans.X1 = 0;
            lineTrans.X2 = width;
            lineTrans.Y1 = lineTrans.Y2 = this.canvasTicks.Height;
            this.canvasTicks.Children.Add(lineTrans);

            long tickStart = (long)(offsetH / DrawingScaling);
            long tickEnd = (long)((offsetH + width) / DrawingScaling);
            long tickDiff = tickEnd - tickStart;
            long segments = (long)(tickDiff / 1000);

            long tickIncrement = 10 * segments + 10;

            if (tickStart % tickIncrement != 0)
            {
                tickStart = ((long)(tickStart / tickIncrement) + 1) * tickIncrement;
            }

            /* Loop through to draw numbers.  */
            for (long l = tickStart; l < tickEnd; l += tickIncrement)
            {
                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                line.Y2 = this.canvasTicks.Height;
                if (0 == l % (tickIncrement * 10))
                {
                    line.Y1 = this.canvasTicks.Height - 25;

                    TextBlock tb = new TextBlock();
                    long uSecPerTick = Properties.Settings.Default.uSecPerTick;
                    if (cbbTime.SelectedIndex == 1)
                    {
                        tb.Text = (l * uSecPerTick / 1000000).ToString(CultureInfo.CurrentCulture);
                    }
                    else if (cbbTime.SelectedIndex == 2)
                    {
                        tb.Text = (l * uSecPerTick / 1000).ToString(CultureInfo.CurrentCulture);
                    }
                    else if (cbbTime.SelectedIndex == 3)
                    {
                        tb.Text = (l * uSecPerTick).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        if (tickMappingSet)
                        {
                            if (((tfi.Events[tfi.Events.Count - 1].RelativeTicks * tickMappingValue).ToString(CultureInfo.InvariantCulture).Length > 3) && ((tfi.Events[tfi.Events.Count - 1].RelativeTicks * tickMappingValue).ToString(CultureInfo.InvariantCulture).Length < 9))
                            {
                                if (skipValue % 2 == 0)
                                {
                                    /* Limit the number of decimal places to 2.  */
                                    tb.Text = l.ToString(CultureInfo.CurrentCulture) + " (" + (l / tickMappingValue).ToString("0.00", CultureInfo.CurrentCulture) + "µs)";//"µ Sec)";
                                }
                                skipValue++;
                            }
                            else if (((tfi.Events[tfi.Events.Count - 1].RelativeTicks * tickMappingValue).ToString(CultureInfo.InvariantCulture).Length >= 9))
                            {
                                if (skipValue % 3 == 0)
                                {
                                    /* Limit the number of decimal places to 2.  */
                                    tb.Text = l.ToString(CultureInfo.CurrentCulture) + " (" + (l / tickMappingValue).ToString("0.00", CultureInfo.CurrentCulture) + "µs)";//"µ Sec)";
                                }
                                skipValue++;
                            }
                            else
                            {
                                /* Limit the number of decimal places to 2.  */
                                tb.Text = l.ToString(CultureInfo.CurrentCulture) + " (" + (l / tickMappingValue).ToString("0.00", CultureInfo.CurrentCulture) + "µs)";//"µ Sec)";
                            }
                        }
                        else
                        {
                            tb.Text = l.ToString(CultureInfo.CurrentCulture);
                        }
                    }
                    Canvas.SetLeft(tb, l * DrawingScaling - offsetH);
                    Canvas.SetTop(tb, 2);
                    this.canvasTicks.Children.Add(tb);

                    //tb = null;
                }
                else
                {
                    line.Y1 = this.canvasTicks.Height - 10;
                }
                line.X1 = line.X2 = l * DrawingScaling - offsetH;
                this.canvasTicks.Children.Add(line);

                //line = null;
            }

            rectBack = null;
            lineTrans = null;
        }

        /* Make the context bar.  */

        private Canvas MakeContextBar(
            int startIndex, bool includeStartEvent, int endIndex, bool includeEndEvent,
            bool alt, double offsetH, double iconWidth)
        {
            /* Create a new canvas and a rectangle.  */
            Canvas cnvs = new Canvas();
            Rectangle rt = new Rectangle();
            rt.Height = GetEventDisplayHeight();
            double left = 0;

            /* Boundary check the startIndex.  */
            if (startIndex == endIndex)
                return cnvs;

            /* Boundary check the width.  */
            double width = (this.tfi.Events[endIndex].RelativeTicks - this.tfi.Events[startIndex].RelativeTicks) * DrawingScaling;
            if (width < 0)
                return cnvs;

            rt.Width = width;
            left = this.tfi.Events[startIndex].RelativeTicks * DrawingScaling - offsetH;

            /* If we want to include the end event.  */
            if (includeEndEvent == true)
            {
                rt.Width += iconWidth;
            }

            /* Exclude the width of the StartID icon.  */
            if (includeStartEvent == false)
            {
                double remainWidth = rt.Width - iconWidth;
                rt.Width = remainWidth > 0 ? remainWidth : 0;
                left += iconWidth;
            }

            Canvas.SetLeft(cnvs, left);

            rt.StrokeThickness = 0.5;
            rt.Stroke = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));

            /* Alternate the color from gray to white.  */
            if (alt)
            {
                rt.Fill = new SolidColorBrush(Color.FromArgb(255, 200, 200, 255));
            }
            else
            {
                rt.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            cnvs.Children.Add(rt);

            if (rt.Width < 100)
                return cnvs;

            TextBlock tb = new TextBlock();

            /* Draw in the appropriate name.  */
            if (startIndex < tfi.Events.Count)
            {
                TmlEvent tmlEvent;
                int idx;
                tmlEvent = tfi.Events[startIndex];
                if (includeStartEvent)
                {
                    if (tmlEvent.Context == 0x00000000)
                        tb.Text = "Idle";
                    else if (tmlEvent.Context == 0xF0F0F0F0)
                        tb.Text = "Initialize";
                    else if (tmlEvent.Context == 0xFFFFFFFF)
                        tb.Text = "Interrupt";
                    else
                    {
                        idx = tfi.FindThreadIndex(tmlEvent);

                        if (idx != -1)
                        {
                            tb.Text = tfi.Threads[idx].Name + " (0x" + tfi.Threads[idx].Address.ToString("X8", CultureInfo.CurrentCulture) + ")";
                        }
                    }
                }
                else
                {
                    /* Use the context information from the NextContext       *
                     * (green bar) after the start event.                     */
                    if (tmlEvent.NextContext == 0x00000000)
                        tb.Text = "Idle";
                    else if (tmlEvent.NextContext == 0xF0F0F0F0)
                        tb.Text = "Initialize";
                    else if (tmlEvent.NextContext == 0xFFFFFFFF)
                        tb.Text = "Interrupt";
                    else
                    {
                        idx = tfi.FindNextContextIndex(tmlEvent);

                        if (idx != -1)
                        {
                            tb.Text = tfi.Threads[idx].Name + " (" + tfi.Threads[idx].Address.ToString("X8", CultureInfo.CurrentCulture) + ")";
                        }
                    }
                }
            }

            cnvs.ToolTip = tb.Text;
            cnvs.Children.Add(tb);

            /* Clear memory. */
            tb = null;
            rt = null;
            /* Clear memory. */
            return cnvs;
        }

        /* Time view event hanlder for scroll change.  */

        private void OnScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            svLeft.ScrollToVerticalOffset(e.VerticalOffset);
            DrawEvents(sbEvents.Value);
        }

        /* Handler for change in grid size.  */

        private void Grid1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double eHeight = GetEventDisplayHeight();
            grid1.RowDefinitions[0].Height = new GridLength(eHeight);
            grid1.RowDefinitions[1].Height = new GridLength(eHeight);
            grid1.ColumnDefinitions[0].Width = new GridLength(eHeight * 260 / 24);
            canvasObjects.Width = GetThreadNameDisplayWidth();
            tbxContext.Height = eHeight;

            double height = grid1.RowDefinitions[3].ActualHeight;
            double width = grid1.ColumnDefinitions[1].ActualWidth;
            svLeft.Height = svRight.Height = height;
            //svRight.Height = height;
            //svLeft.Height = svRight.Height + grid1.RowDefinitions[4].ActualHeight;
            sbEvents.ViewportSize = grid1.ColumnDefinitions[1].ActualWidth;

            if (tfi != null)
            {
                height = tfi.Threads.Count * eHeight;

                /* Set the height of the total of events and objects.  */
                canvasEvents.Height = canvasObjects.Height = height; // +40;
                svLeft.Height = svRight.Height = Height;
                //svRight.Height = Height;
                //svLeft.Height = svRight.Height + grid1.RowDefinitions[4].ActualHeight;
            }

            canvasEvents.Width = canvasEvent.Width = canvasContext.Width = canvasTicks.Width = width;
            this.UpdateHScrollBar();
        }

        /* Handler for mouse click.  */

        private void CanvasEvent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _deltaMask.Width = 0;

            if (null != this.DeltaChanged)
                DeltaChanged(this, new Code.DeltaEventArgs(0, 0, 0));

            if (null != this.indicator)
            {
                Point point = e.GetPosition(canvasEvents);
                int eventIndex = FindFirstEventByPointX(point.X + sbEvents.Value);
                this.tfi.CurrentEventIndex = eventIndex;
                this.navigator.MoveTo(eventIndex, false);

                var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
                if (peer != null)
                {
                    StatusBlock.Text = "event " + navigator.GetCurrentIndex().ToString();
                    peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                }
            }
        }

        /* Handler for a left button click on the mouse.  */

        private void CanvasEvents_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this._deltaMask.Visibility = Visibility.Visible;
            _deltaMask.Height = svRight.ViewportHeight + svRight.VerticalOffset;
            if (_deltaMask.Height < canvasEvents.Height)
            {
                _deltaMask.Height = canvasEvents.Height;
            }
            _deltaMask.Width = 0;
            _deltaMask.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            _deltaMask.Opacity = 0.1;

            /* Get the point that the mouse is at.  */
            Point point = e.GetPosition(canvasEvents);
            canvasEvents.CaptureMouse();
            _deltaStarted = true;
            _deltaStartPoint = point;
            _deltaStartPoint.Offset(sbEvents.Value, 0);

            if (null != this.DeltaChanged)
                DeltaChanged(this, new Code.DeltaEventArgs(0, cbbTime.SelectedIndex, 0));
            /* If the indicator isn't already the current indicator.  */
            if (null != this.indicator)
            {
                double location = point.X + sbEvents.Value;
                int IndicatorIndex = 0;
                int rightIndex = FindFirstEventByPointX(location);
                if (rightIndex >= tfi.Events.Count - 1)
                {
                    IndicatorIndex = tfi.Events.Count - 1;
                }
                else if (rightIndex <= 0)
                {
                    IndicatorIndex = 0;
                }
                else
                {
                    double leftGap = location - tfi.Events[rightIndex - 1].RelativeTicks * DrawingScaling;
                    double rightGap = tfi.Events[rightIndex].RelativeTicks * DrawingScaling - location;
                    if (leftGap <= rightGap)
                    {
                        IndicatorIndex = rightIndex - 1;
                    }
                    else
                    {
                        IndicatorIndex = rightIndex;
                    }
                }
                double indicatorLoc = tfi.Events[IndicatorIndex].RelativeTicks * DrawingScaling + GetEventDisplayWidth() / 2;
            }
        }

        private void CanvasEvents_MouseMove(object sender, MouseEventArgs e)
        {
            if (_deltaStarted)
            {
                /* Set width, height, and offsets.  */
                double width = svRight.ViewportWidth;
                double height = svRight.ViewportHeight;
                double offsetV = svRight.VerticalOffset;
                double offsetH = sbEvents.Value;
                Point point = e.GetPosition(canvasEvents);
                point.Offset(offsetH, 0);
                if (point.X > (offsetH + width))
                {
                    sbEvents.Value = (point.X - width);
                }
                else if (point.X < offsetH && point.X >= 0)
                {
                    sbEvents.Value = point.X;
                }

                //synchronize position & size of _deltaMask
                if (point.X >= _deltaStartPoint.X)
                {
                    Canvas.SetLeft(_deltaMask, _deltaStartPoint.X - offsetH);
                    _deltaMask.Width = point.X - _deltaStartPoint.X;
                }
                else
                {
                    Canvas.SetLeft(_deltaMask, point.X - offsetH);
                    _deltaMask.Width = _deltaStartPoint.X - point.X;
                }

                if (null != this.DeltaChanged)
                {
                    long deltaTicks = (long)(_deltaMask.Width / DrawingScaling);
                    if (tickMappingValue != 0)
                    {
                        string x = (deltaTicks / tickMappingValue).ToString("0.00", CultureInfo.InvariantCulture);
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, Convert.ToDouble(x, CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, 0));
                    }
                }
            }
        }

        private void CanvasEvents_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_deltaStarted)
            {
                /* Set width, height, and offsets.  */
                double width = svRight.ViewportWidth;
                double height = svRight.ViewportHeight;
                double offsetV = svRight.VerticalOffset;
                double offsetH = sbEvents.Value;
                Point point = e.GetPosition(canvasEvents);
                point.Offset(offsetH, 0);
                if (point.X > (offsetH + width))
                {
                    sbEvents.Value = (point.X - width);
                }
                else if (point.X < offsetH && point.X >= 0)
                {
                    sbEvents.Value = (point.X);
                }

                int beginEventIndex;
                int endEventIndex;
                long deltaTicks = 0;
                //synchronize position & size of _deltaMask
                if (point.X >= _deltaStartPoint.X)
                {
                    beginEventIndex = FindLeftEventByPointX(0, _deltaStartPoint.X);
                    endEventIndex = FindRightEventByPointX(beginEventIndex, point.X);

                    double offDist = point.X - (offsetH + width);
                    if (offDist > 0 && endEventIndex < tfi.Events.Count - 1)
                    {
                        Canvas.SetLeft(_deltaMask, tfi.Events[beginEventIndex].RelativeTicks * DrawingScaling - offsetH - offDist);
                    }
                    else
                    {
                        Canvas.SetLeft(_deltaMask, tfi.Events[beginEventIndex].RelativeTicks * DrawingScaling - offsetH);
                    }

                    deltaTicks = tfi.Events[endEventIndex].RelativeTicks - tfi.Events[beginEventIndex].RelativeTicks;

                    if (null != this.indicator)
                    {
                        this.tfi.CurrentEventIndex = beginEventIndex;
                        this.navigator.MoveTo(beginEventIndex, false);
                    }
                }
                else
                {
                    beginEventIndex = FindRightEventByPointX(0, _deltaStartPoint.X);
                    endEventIndex = FindLeftEventByPointX(0, point.X);
                    Canvas.SetLeft(_deltaMask, tfi.Events[endEventIndex].RelativeTicks * DrawingScaling - offsetH);

                    double offDist = offsetH - point.X;
                    if (offDist > 0 && endEventIndex > 0)
                    {
                        //Canvas.SetLeft(_deltaMask, tfi.Events[beginEventIndex].RelativeTicks * DrawingScaling - offsetH - offDist);
                        deltaTicks = tfi.Events[beginEventIndex].RelativeTicks - tfi.Events[endEventIndex].RelativeTicks + (long)(offDist / DrawingScaling);
                    }
                    else
                    {
                        //Canvas.SetLeft(_deltaMask, tfi.Events[endEventIndex].RelativeTicks * DrawingScaling - offsetH);
                        deltaTicks = tfi.Events[beginEventIndex].RelativeTicks - tfi.Events[endEventIndex].RelativeTicks;
                    }

                    if (null != this.indicator)
                    {
                        this.tfi.CurrentEventIndex = beginEventIndex;
                        this.navigator.MoveTo(beginEventIndex, false);
                    }
                }

                deltaTicks = deltaTicks < 0 ? 0 : deltaTicks;

                _deltaMask.Width = deltaTicks * DrawingScaling;

                string x = (deltaTicks / tickMappingValue).ToString("0.00", CultureInfo.InvariantCulture);
                // call delta changed event handler
                if (null != this.DeltaChanged)

                    if (tickMappingValue != 0)
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, cbbTime.SelectedIndex, Convert.ToDouble(x, CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, cbbTime.SelectedIndex, 0));
                    }

                _deltaStarted = false;
            }

            canvasEvents.ReleaseMouseCapture();
        }

        /* Helper function to move to a specified event.  */

        public override void MoveToEvent(int eventIndex, bool centerMarker)
        {
            _deltaMask.Width = 0;
            if (null != this.DeltaChanged)
            {
                DeltaChanged(this, new Code.DeltaEventArgs(0, 0, 0));
            }

            /* Check if a file is loaded and we are at a valid event.  If not return.  */
            if (null == this.tfi || eventIndex < 0 || eventIndex >= this.tfi.Events.Count) return;

            /* Create an instance of tmlEvent.  */
            TmlEvent tmlEvent = this.tfi.Events[eventIndex];

            tfi.CurrentEventIndex = eventIndex;

            /* Set width equal to the viewPort.  */
            double width = svRight.ViewportWidth;

            /* If the current viewPort is closed or zero.  */
            if (0 == width)
            {
                /* Set a new value and return.  */
                sbEvents.Value = tmlEvent.RelativeTicks * DrawingScaling;
                this.indicator.Show(tmlEvent.RelativeTicks * DrawingScaling);
                return;
            }

            /* Set the offset, start, and end index.  */
            double offset = this.sbEvents.Value;
            double offsetH = offset;
            int startIndex = FindFirstEventByPointX(offset);
            int endIndex = FindLeftEventByPointX(startIndex, (offset + width));

            /* Boundary check endindex.  */
            if (endIndex > (tfi.Events.Count - 1)) endIndex = (tfi.Events.Count - 1);


            /* Check if the eventIndex is less than the start index.  */
            if (eventIndex < startIndex)
            {
                if (centerMarker)
                {
                    offsetH = tmlEvent.RelativeTicks * DrawingScaling - width / 2;
                }
                else
                {
                    /* It is, create a new offset.  */
                    offsetH = tmlEvent.RelativeTicks * DrawingScaling;
                }
            }

            /* Check if the eventIndex is larger than the endIndex.  */
            else if (eventIndex >= endIndex)
            {
                if (centerMarker)
                {
                    offsetH = tmlEvent.RelativeTicks * DrawingScaling - width / 2;
                }
                else
                {
                    /* It is, create a new offset.  */
                    int nextIndex = eventIndex + 1;
                    nextIndex = nextIndex > tfi.Events.Count - 1 ? tfi.Events.Count - 1 : nextIndex;
                    TmlEvent tmlEvent1 = this.tfi.Events[nextIndex];
                    offsetH = tmlEvent1.RelativeTicks * DrawingScaling - width + GetEventDisplayWidth();
                }
            }

            /* boundary check the offset values.  */
            if (offsetH > sbEvents.Maximum) offsetH = sbEvents.Maximum;
            if (offsetH < 0 || eventIndex == 0) offsetH = 0;
            if (sbEvents.Value != offsetH) sbEvents.Value = offsetH;
            /* Possible indicator shift.  */
            this.indicator.Show(tmlEvent.RelativeTicks * DrawingScaling - offsetH);
        }

        /* Handler for a scroll change.  */

        private void SvLeft_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (SplitterPositionChanged != null)
                SplitterPositionChanged(this, null);
        }

        // Handler for mouse double click.

        private void SvRight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // If the trace file information is not available return.
            if (this.tfi.TmlTraceInfo == null) return;
            Point point = e.GetPosition(canvasEvents);
            double realX = point.X + sbEvents.Value;
            int eventIndex = FindFirstEventByPointX(realX);

            // Boundary check on eventIndex.
            if (eventIndex < 0 || eventIndex > this.tfi.Events.Count - 1) return;

            // Create new tmlEvent instance...Based on eventIndex.
            TmlEvent tmlEvent = this.tfi.Events[eventIndex];

            long x = (long)(realX / DrawingScaling);
            if (x < tmlEvent.RelativeTicks) return;

            int threadIndex = Convert.ToInt32(Math.Floor(point.Y / GetEventDisplayHeight()), CultureInfo.InvariantCulture);

            int index = tfi.FindThreadIndex(tmlEvent);
            if (-1 != index)
            {
                if (threadIndex != index)
                {
                    // Don't display details window when event summary icons not clicked.
                    if (point.Y < -64 || point.Y > -40)
                        return;
                }
            }
            else
            {
                return;
            }

            // Determine where to display event details window when event summary icons clicked.
            double delta_x = point.X - EventIconPosXGet(tmlEvent.RelativeTicks);
            double delta_y = point.Y >= -64 && point.Y <= -40 ? point.Y + 80 : (point.Y + 30) - EventIconPosYGet(index);

            ShowEventDetailsWindow(eventIndex, delta_x, delta_y);
        }

        /* Handler for a value change on the events.  */

        private void SbEvents_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double offsetH = e.NewValue;

            // move _detalMask when vscroll changed
            double deltaMaskLeft = Canvas.GetLeft(_deltaMask);
            deltaMaskLeft = deltaMaskLeft - e.NewValue + e.OldValue;
            Canvas.SetLeft(_deltaMask, deltaMaskLeft);

            /* Check if we have valid trace file information.  */
            if (tfi != null)
            {
                DrawEvents(offsetH);

                /* Recalculate detail window's X position.  */
                double delta = e.OldValue - e.NewValue;

                /* Move the details window as necessary.  */
                foreach (DetailsWindow dwnd in this.detailsWindowList)
                {
                    dwnd.MoveX(delta, DrawingScaling, sbEvents.Value, GetEventDisplayWidth());
                }

                /* Check if the curerntEventIndex is greater than 0.  */
                if (tfi.CurrentEventIndex >= 0)
                {
                    TmlEvent tmlEvent = tfi.Events[tfi.CurrentEventIndex];
                    this.indicator.Show(tmlEvent.RelativeTicks * DrawingScaling - offsetH);
                }
            }
        }

        private void CbbTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (null != canvasTicks)
            {
                canvasTicks.Children.Clear();
                this.DrawTicksBar(canvasEvents.Width + 40, sbEvents.Value);
            }
        }

        private void BtSet_Click(object sender, RoutedEventArgs e)
        {
            long val = Properties.Settings.Default.uSecPerTick;
            try
            {
                val = Convert.ToInt64(tbUSecs.Text, CultureInfo.InvariantCulture);
            }
            catch { }
            Properties.Settings.Default.uSecPerTick = val;
            tbUSecs.Text = val.ToString(CultureInfo.CurrentCulture);
        }

        private double EventIconPosXGet(long eventTicks)
        {
            return (eventTicks * DrawingScaling - sbEvents.Value) + GetEventDisplayWidth() / 2;
        }

        private void ShowEventDetailsWindow(int eventIndex, double deltaX, double deltaY)
        {
            // If the trace file information is not available return.
            if (this.tfi.TmlTraceInfo == null) return;
            
            /* Boundary check on eventIndex.  */
            if (eventIndex < 0 || eventIndex > this.tfi.Events.Count - 1) return;

            // Create new tmlEvent instance...Based on eventIndex.
            TmlEvent tmlEvent = this.tfi.Events[eventIndex];

            string threadName = string.Empty;
            int index = tfi.FindThreadIndex(tmlEvent);
            if (index == -1)
            {
                return;
            }

            threadName = tfi.Threads[index].Name;

            HideEventDetailsWindow(eventIndex);

            Code.Event evt = Code.Event.CreateInstance(tmlEvent, threadName, tfi);

            // Create a new instance of details window.
            DetailsWindow dw = new DetailsWindow(this, canvasEvents);

            // Create the line to attach to the details window.
            Line line = new Line();
            line.X1 = EventIconPosXGet(tmlEvent.RelativeTicks);
            line.Y1 = EventIconPosYGet(index);

            double left = line.X1 + deltaX;
            double top = line.Y1 + deltaY;

            // Prevent the window poping up off screen.
            if ((left + dw.Width) > canvasEvents.Width)
            {
                left = canvasEvents.Width - dw.Width;
            }

            line.X2 = left + dw.Width / 2;
            line.Y2 = top;
            line.Stroke = new SolidColorBrush(Color.FromArgb(255, 192, 192, 240));
            line.StrokeThickness = 2;
            canvasEvents.Children.Add(line);

            // Initialize the details window and add it to the canvas.
            dw.Initialize(evt, tfi);
            dw.Tag = line;
            Canvas.SetLeft(dw, left);
            Canvas.SetTop(dw, top);
            canvasEvents.Children.Add(dw);
            detailsWindowList.Add(dw);
        }

        private bool HideEventDetailsWindow(int eventIndex)
        {
            TmlEvent tmlEvent = tfi.Events[eventIndex];

            foreach (DetailsWindow wnd in detailsWindowList)
            {
                if (wnd.RelatedEventIndex == tmlEvent.Index)
                {
                    RemoveDetailsWindow(wnd, canvasEvents);
                    return true;
                }
            }

            return false;
        }

        private void svRight_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            ScrollViewer sv = sender as ScrollViewer;
            string msg = "";

            if (sv != null)
            {
                if (sv.IsFocused)
                {
                    switch (e.Key)
                    {
                    case Key.Enter:
                        ShowEventDetailsWindow(navigator.GetCurrentIndex(), 0, 30);
                        e.Handled = true;
                        break;

                    case Key.Escape:
                        HideEventDetailsWindow(navigator.GetCurrentIndex());
                        e.Handled = true;
                        break;

                    default:
                        msg = PreviewKeyDown(sender, e);

                        if (msg.Length != 0)
                        {
                            var peer = FrameworkElementAutomationPeer.FromElement(StatusBlock);
                            if (peer != null)
                            {
                                StatusBlock.Text = msg;
                                peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                            }
                        }
                        break;
                    }
                }
            }
        }
    }
}
