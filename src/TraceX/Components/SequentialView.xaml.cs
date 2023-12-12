using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Automation.Peers;
using AzureRTOS.Tml;
using AzureRTOS.TraceManagement.Code;
using System.Globalization;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for SequentialView.xaml
    /// </summary>
    public partial class SequentialView : TraceViewControl
    {
        /* Constants.  */

        static Line sep1 = new Line();
        static Line sep2 = new Line();

        public static double realMultiple = 0.0;
        public bool statusLinesReadyOnlySet = false;
        public bool statusLinesAllOnSet = false;
        public double tickMappingValue;
        public bool tickMappingSet = false;
        public ArrayList PriorityInversionArray = new ArrayList();

        /// <summary>
        /// Zoom in /out
        /// </summary>
        /// <param name="multiple"></param>
        public override void Zoom(double multiple)
        {
            if (!_viewActivated)
                return;

            double oldDrawingScale = DrawingScaling;
            double minEventWidth = GetMinEventWidth();
            enableZoomIn = true;
            enableZoomOut = true;

            if (multiple <= Event.ExtremeZoomOutFactor)
            {
                enableZoomOut = false;
                if (minEventWidth < GetEventDisplayWidth())
                {
                    enableZoomIn = true;
                    ZoomFactor = Event.ExtremeZoomOutFactor;
                    ZoomToolbar.currentZoomG = (ulong)(ZoomFactor * 100);
                    iconWidth = minEventWidth;
                    DrawingScaling = minEventWidth / GetEventDisplayWidth();
                }
                else
                {
                    ZoomToolbar.currentZoomG = (ulong)(ZoomFactor * 100);
                    enableZoomIn = false;
                }
            }
            else
            {
                if (minEventWidth < GetEventDisplayWidth())
                {
                    ZoomFactor = multiple;
                    if (!ZoomToolbar.inputTextSet)
                    {
                        ZoomToolbar.currentZoomG = (ulong)(ZoomFactor * 100);
                    }
                    iconWidth = minEventWidth * multiple;

                    /* We subtract .01 to allow for a .99 to be rounded up.  */
                    if (iconWidth >= (GetEventDisplayWidth() - .01))
                    {
                        multiple = GetEventDisplayWidth() / minEventWidth;
                        ZoomFactor = multiple;

                        realMultiple = multiple;

                        enableZoomIn = false;
                        enableZoomOut = true;
                        iconWidth = GetEventDisplayWidth();
                    }

                    DrawingScaling = iconWidth / GetEventDisplayWidth();
                }
                else
                {
                    ZoomToolbar.currentZoomG = (ulong)(ZoomFactor * 100);
                    enableZoomIn = false;
                    enableZoomOut = false;
                }
            }

            base.Zoom(multiple);

            double ratio = (DrawingScaling / oldDrawingScale);
            _deltaMask.Width *= ratio;

            double deltaMaskLeft = Canvas.GetLeft(_deltaMask);
            deltaMaskLeft = (deltaMaskLeft + sbEvents.Value) * ratio - sbEvents.Value;
            Canvas.SetLeft(_deltaMask, deltaMaskLeft);


            this.updateHScrollBar();
            sbEvents_ValueChanged(svEvent, new RoutedPropertyChangedEventArgs<double>(sbEvents.Value, sbEvents.Value));
            this.MoveToEvent(tfi.CurrentEventIndex, true);
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SequentialView()
        {
            /* Standard Initialization of component.  */
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
            return svRight.ViewportWidth;// -sbEvents.Value;
        }

        public double GetMinEventWidth()
        {
            if (tfi != null)
            {
                //return (svRight.ViewportWidth - sbEvents.Value) / tfi.Events.Count;
                double viewPortWidth = svRight.ViewportWidth <= 0 ? grid1.ColumnDefinitions[0].ActualWidth : svRight.ViewportWidth;
                viewPortWidth = viewPortWidth == 0 ? (AzureRTOS.TraceManagement.Properties.Settings.Default.MainWidth + 265) : viewPortWidth;
                return viewPortWidth / tfi.Events.Count;
            }

            return AzureRTOS.TraceManagement.Code.Event.EventDisplayWidth;
        }

        public void ResetForFileOpen()
        {
            DrawingScaling = 1.0;
            ZoomFactor = GetEventDisplayWidth() / GetMinEventWidth();
            ZoomFactor = ((int)(ZoomFactor * 100 + 0.5)) / 100.0;
            iconWidth = GetEventDisplayWidth();
            EventIndicatorIndex = 0;
            sbEvents.Value = 0;
            svRight.ScrollToVerticalOffset(0);
  
            if (tfi != null)
            {
                tfi.CurrentEventIndex = 0;
            }
        }

        /* When jumping pages this function will find the next index to start at.  */

        public override int GetNextPageStartIndex(int currentIndex)
        {
            /* Set the width equal to the size of the viewport.  */
            double width = svRight.ViewportWidth;

            /* Get the new index.  */
            int index = currentIndex + Convert.ToInt32(Math.Floor(width / iconWidth), CultureInfo.InvariantCulture);
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
            int index = currentIndex - Convert.ToInt32(Math.Floor(width / iconWidth), CultureInfo.InvariantCulture);
            index -= 1;

            /* If the index is smaller than 0, set it to the first event (0).  */
            if (index < 0) index = 0;

            /* Return the new index.  */
            return index;
        }

        /* If an event number is directly plugged in this function will move you to the correct position.  */

        public override void MoveToEvent(int eventIndex, bool centerMarker)
        {
            /* Check to see if a file is loaded. If there is not one return.  */
            if (null == this.tfi) return;
            /* Possible indicator move.  */
            /* Put the red line seletion indicator in the correct position.  */
            //this.indicator.Show(eventIndex * iconWidth);
            /* Set the current index.  */
            tfi.CurrentEventIndex = eventIndex;

            /* Calculate out the width as the viewport width and get the current offset.  */
            double width = svRight.ViewportWidth;
            double offset = this.sbEvents.Value;

            /* Calculate out the start and end index.  */
            int startIndex = Convert.ToInt32(Math.Ceiling(offset / iconWidth), CultureInfo.InvariantCulture);
            int endIndex = startIndex + Convert.ToInt32(Math.Floor(width / iconWidth), CultureInfo.InvariantCulture);

            /* Boundary condition if endIndex is set to larger than the last event.  */
            if (endIndex > (tfi.Events.Count - 1)) endIndex = (tfi.Events.Count - 1);

            /* Boundary check if we are at index 0 then set the offset to zero.  */
            if (eventIndex == 0)
            {
                sbEvents.Value = 0;
            }
            else
            {
                if (centerMarker)
                {
                    sbEvents.Value = eventIndex * iconWidth - width / 2;
                }
                else
                {
                    /* If the current event is less than our previously calculated start Index.  */
                    if (eventIndex < startIndex)
                    {
                        /* Scroll to our horizontal offset.  */
                        sbEvents.Value = eventIndex * iconWidth - iconWidth;
                    }
                    /* If the current event is greater than the end index.  */
                    else if (eventIndex >= endIndex)
                    {
                        /* Scroll to our horizontal offset.  */
                        sbEvents.Value = eventIndex * iconWidth - width + iconWidth;
                    }
                    else
                    {
                    }
                }
            }

            double indicatorLocation = eventIndex * iconWidth - sbEvents.Value;// +iconWidth;
            this.indicator.Show(indicatorLocation);
            navigator.SetIndex(tfi.CurrentEventIndex.ToString(CultureInfo.CurrentCulture));
        }

        /* Clean up the current view.  */

        public void CleanUpView()
        {
            /* Hide the context name, event bar, event id name, and all corresponding canvases.  */
            this.tbxEventId.Visibility = Visibility.Hidden;
            this.canvasEventId.Visibility = Visibility.Hidden;

            /* Clear all objects, events, and event id information.  */
            canvasObjects.Children.Clear();
            canvasEvents.Children.Clear();
            canvasEventId.Children.Clear();

            /* Create a new indicator line for later.  */
            this.indicator = Code.Indicator.Create(this.canvasEventId, this.canvasEvents);

            /* Clear the details window list.  */
            this.detailsWindowList.Clear();

            /* Draw blank events and threads to clear the information.  */
            double width = svRight.ViewportWidth;
            double height = svRight.ViewportHeight;
            double offsetH = svRight.HorizontalOffset;
            double offsetV = svRight.VerticalOffset;

            tfi.Threads.Clear();

            DrawThreads();
            DrawEvents(0);
        }

        /* Initialize the sequential view.  */

        public override void Initialize(Code.TraceFileInfo tfi, Navigator navigator)
        {
            /* Clear the current trace file information just in case it had previous information.  */
            this.tfi = null;

            DrawingScaling = 1.0;

            /*  Set the scrollable horizontal offset to zero.  */
            sbEvents.Value = 0;

            /* Set the rate at which the scroll changes for large and small changes.  */
            sbEvents.SmallChange = GetEventDisplayWidth();
            sbEvents.LargeChange = GetEventDisplayWidth() * 10;

            /* Clear the details window list.  */
            this.detailsWindowList.Clear();

            /* Just in case the trace file has no count then return.  */
            if (tfi.Events.Count == 0)
            {
                return;
            }

            /* Set the current (this) to what navigator and tfi.  */
            this.navigator = navigator;
            this.tfi = tfi;

            DrawingScaling = iconWidth / GetEventDisplayWidth();

            /* Set everything back to visible.  */
            this.tbxContext.Visibility = Visibility.Visible;
            this.tbxEvent.Visibility = Visibility.Visible;
            this.tbxEventId.Visibility = Visibility.Visible;
            this.canvasContext.Visibility = Visibility.Visible;
            this.canvasEvent.Visibility = Visibility.Visible;
            this.canvasEventId.Visibility = Visibility.Visible;

            /* KGM FixMe for font size change */

            SiblingViewPortHeight = grid1.RowDefinitions[3].ActualHeight;
            SiblingViewPortWidth = svRight.ViewportWidth;

            CalcNumberOfThreadsToDisplay(grid1.RowDefinitions[3].ActualHeight);

            /* Set the height of the total of events and objects.  */
            canvasEvents.Height = tfi.Threads.Count * GetEventDisplayHeight(); // +40;
            canvasObjects.Height = canvasEvents.Height;// +20;
            canvasObjects.Width = GetThreadNameDisplayWidth();


            /* Set the total width of th events.  */
            canvasEvents.Width = tfi.Events.Count * iconWidth;
            canvasEvent.Width = canvasEvents.Width;// +20;
            canvasEventId.Width = canvasContext.Width = canvasEvent.Width;

            /* Set the background to white.  */
            canvasEvent.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            canvasEventId.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));

            canvasObjects.MouseWheel += new MouseWheelEventHandler(canvasObjects_MouseWheel);

            /* Clear the objects and events.  */
            canvasObjects.Children.Clear();
            canvasEvents.Children.Clear();

            this.indicator = Code.Indicator.Create(this.canvasEventId, this.canvasEvents);

            this.updateHScrollBar();

            /* Draw the threads, the indicator, the events, and start at event zero.  */
            if (tfi != null)
            {
                tfi.CurrentEventIndex = 0;
            }
            EventIndicatorIndex = 0;
            DrawThreads();
            findThreadOrder();
            findThreadChangeOrder();
            DrawEvents(sbEvents.Value);
            MoveToEvent(EventIndicatorIndex, false);

            //DrawStatusLines(0, tfi.Events.Count-1);

            ZoomFactor = GetEventDisplayWidth() / GetMinEventWidth();
            ZoomFactor = (double)(((int)(ZoomFactor * 100 + 0.5)) / 100.0);
            if (ZoomLimitReached != null)
            {
                enableZoomIn = true;
                enableZoomOut = true;

                if (tfi.Events.Count * iconWidth < GetRightViewPortWidth())
                {
                    enableZoomIn = false;
                    enableZoomOut = false;
                }
                ZoomLimitReached(this, new ZoomEventArgs(enableZoomOut, enableZoomIn));
            }
        }

        private void canvasObjects_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            e.Handled = true;
            //Do Nothing... We do not want to allow this scroll to occur.
        }


        /* Function to update the horizontal scroll bar width and visibility.  */

        private void updateHScrollBar()
        {
            double width = grid1.ColumnDefinitions[1].ActualWidth;
            double contenSize = width;
            if (tfi != null)
            {
                contenSize = tfi.Events.Count * iconWidth + GetEventDisplayWidth();
            }
            sbEvents.Minimum = 0;
            if (contenSize > width)
            {
                sbEvents.Visibility = Visibility.Visible;
                if (this.svRight.ViewportHeight < this.canvasEvents.Height)
                {
                    sbEvents.Maximum = contenSize - width + 16; // add 16 to make up the width of hscrollbar
                }
                else
                {
                    sbEvents.Maximum = contenSize - width;
                }
            }
            else
            {
                sbEvents.Visibility = Visibility.Hidden;
            }

            sbEvents.ViewportSize = grid1.ColumnDefinitions[1].ActualWidth;
        }

 
        // Function to draw the thread names alone the left hand side of the application window.

        private void DrawThreads()
        {
            //double width = svLeft.ViewportWidth;
            double width = grid1.ColumnDefinitions[0].Width.Value;
            double height = svLeft.ViewportHeight;
            double offsetV = svLeft.VerticalOffset;

            // Clear the objects.
            canvasObjects.Children.Clear();

            // Create a rectangle of white to clear any thread name that may be in there.
            Rectangle rect = new Rectangle();
            rect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            rect.Width = width;
            rect.Height = height + offsetV;
            canvasObjects.Children.Add(rect);

            // Draw a box around the thread names.
            for (double y = GetEventDisplayHeight(); y <= height + offsetV; y += GetEventDisplayHeight())
            {
                Line line = new Line();
                line.Stroke = SystemColors.ActiveBorderBrush; 
                line.StrokeThickness = 0.5;
                line.X1 = 0;
                line.Y1 = line.Y2 = y;
                line.X2 = width;

                canvasObjects.Children.Add(line);
            }

            // Check to see if there is trace file information... If there is not return.
            
            if (null == tfi) return;

            GetThreadNameDisplayWidth();

            // Draw in each thread name and address.
            for (int i = 0; i < tfi.Threads.Count; ++i)
            {
                canvasObjects.Children.Add(MakeCanvasObject(i));
            }
        }

        public void findThreadOrderClean()
        {
            //tfi.SortThreads(ThreadSorting.ByCreationOrder);

            if (this.navigator != null)
            {
                _threadsOrderChanged = true;
                RedrawThreads();
            }
        }

        protected override void ExchangeThreadPostion(Int32 srcIndex, Int32 destInex)
        {
            if (srcIndex != destInex)
            {
                TmlThread threadMoved = tfi.Threads[srcIndex];
                tfi.Threads.Remove(threadMoved);
                if (srcIndex > destInex)
                {
                    tfi.Threads.Insert(destInex + 1, threadMoved);
                }
                else
                {
                    tfi.Threads.Insert(destInex, threadMoved);
                }
                EventIndicatorIndex = this.navigator.GetCurrentIndex();

                findThreadChangeOrder();

                this.detailsWindowList.Clear();
                DrawThreads();
                DrawEvents(sbEvents.Value);
                MoveToEvent(EventIndicatorIndex, false);
            }
        }

        public override void RedrawThreads()
        {
            if (_threadsOrderChanged && (this.navigator != null))
            {
                EventIndicatorIndex = this.navigator.GetCurrentIndex();

                //  findThreadOrder();
                findThreadChangeOrder();
                this.detailsWindowList.Clear();
                DrawThreads();
                DrawEvents(sbEvents.Value);
                MoveToEvent(EventIndicatorIndex, false);
                _threadsOrderChanged = false;
            }
        }

        public void drawEventsOutside(double msValue, bool tickMapSet)
        {
            tickMappingSet = tickMapSet;
            tickMappingValue = msValue;
            Event.SetRelativeTickMapping(tickMappingValue, tickMappingSet);
            this.DrawEvents(sbEvents.Value);
        }

        /* Handler for scroll changed.  */

        private void OnScrollChanged(Object sender, ScrollChangedEventArgs e)
        {
            /* Scroll everything to the proper offset.  */
            svLeft.ScrollToVerticalOffset(e.VerticalOffset);
            DrawEvents(sbEvents.Value);
        }

        /* Handler for loading the event id. */
        private void tbEventId_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock tbEventId = (TextBlock)sender;
            int index = (int)(tbEventId).Tag;
            Canvas.SetLeft(tbEventId, index * iconWidth - (tbEventId.ActualWidth - iconWidth) / 2 - sbEvents.Value);
            Canvas.SetTop(tbEventId, 0);
        }

        /* Function for drawing the events.  */

        public void DrawEvents(double offsetH)
        {
            /* Set width, height, and offsets.  */
            double width = svRight.ViewportWidth;
            double height = svRight.ViewportHeight;

            //double width = canvasEvents.Width;
            //double height = canvasEvents.Height;

            double offsetV = svRight.VerticalOffset;

            /* Check first to ensure that there is trace file information.  */
            if (null != this.tfi)
            {
                /* Clear the context.  */
                canvasContext.Children.Clear();

                /* add a Rectangle to canvasContext to mask garbage characters at end of "Context Summary".  */
                Rectangle rectMask = new Rectangle();
                rectMask.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                rectMask.Width = 50;
                rectMask.Height = GetEventDisplayHeight();
                Canvas.SetLeft(rectMask, canvasEvents.Width);
                Canvas.SetZIndex(rectMask, 1000);
                canvasContext.Children.Add(rectMask);

                /* Clear the events and corresponding ids.  */
                canvasEvent.Children.Clear();
                canvasEventId.Children.Clear();
                canvasEvents.Children.Clear();

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


                /* Add the indicator to the canvas.  */
                this.indicator.AddToCanvas();

                /* Set the start and end index.  Check the start and end index.  */
                int startIndex = Convert.ToInt32(Math.Floor(offsetH / iconWidth), CultureInfo.InvariantCulture);
                if (startIndex > (tfi.Events.Count - 1)) startIndex = tfi.Events.Count - 1;
                int endIndex = startIndex + Convert.ToInt32(Math.Ceiling(width / iconWidth), CultureInfo.InvariantCulture) + 1;
                if (endIndex > (tfi.Events.Count - 1)) endIndex = (tfi.Events.Count - 1);

                if (startIndex < 0) startIndex = 0;
                if (endIndex < 0) endIndex = 0;

                /* Draw the event id bar.  */
                DrawEventIdBar(startIndex, endIndex);

                /* Draw the background grid.  */
                DrawGrid(canvasEvents, width, height, offsetH, offsetV);

                int prevIndex = -1;
                int ec = 0;
                bool alt = false;

                /* Loop through to create the necessary tml events.  */
                for (int i = 0; i < startIndex; ++i)
                {
                    /* If we have reached the end of the events count finish the loop.  */
                    if (i >= tfi.Events.Count)
                        continue;

                    /* Create a tmlevent for each tfi index.  */
                    TmlEvent tmlEvent = tfi.Events[i];

                    /* Also place the index at the current thread index.  */
                    int index = tfi.FindThreadIndex(tmlEvent);

                    if (prevIndex != -1 && index != prevIndex)
                    {
                        alt = !alt;

                        /* Reset event count.  */
                        ec = 0;
                    }

                    /* Set the previos Index to the index.  */
                    prevIndex = index;

                    /* Increment event count.  */
                    ec += 1;
                }

                BeginInversionIndex = -1;

                // determine whether to skipp drawing the events
                double drawingIconWidth = iconWidth;
                int iconNumberIncrement = 1;
                double testWidth = iconWidth;
                if (iconWidth < MIN_ICON_WIDTH)
                {
                    drawingIconWidth = MIN_ICON_WIDTH;
                }

                DrawStatusLines(startIndex, endIndex);

                /* Go through from current start index to current end index.  */
                for (int i = startIndex; i <= endIndex; i += iconNumberIncrement)
                {
                    if (i >= tfi.Events.Count)
                    {
                        continue;
                    }

                    /* Create a tmlEvent for each one in between startIndex & endIndex.  */
                    TmlEvent tmlEvent = tfi.Events[i];
                    int index = tfi.FindThreadIndex(tmlEvent);

                    double threadPos = index * GetEventDisplayHeight() - svRight.VerticalOffset;


                    /* Set the threadName to empty.  */
                    string threadName = String.Empty;

                    //////////

                    //////////

                    /* Set the thread name.  */
                    if (-1 != index)
                    {
                        threadName = tfi.Threads[index].Name;
                    }

                    bool inversionMarked = false;

                    // check for good priority inversion
                    if (tmlEvent.PriorityInversion > 0)
                    {
                        if (BeginInversionIndex < 0)
                        {
                            BeginInversionIndex = index;
                        }

                        DrawInversionRegion(i, (int)tmlEvent.PriorityInversion, true);
                        inversionMarked = true;
                    }

                    // check for bad priority inversion
                    if (tmlEvent.BadPriorityInversion > 0)
                    {
                        if (BeginInversionIndex < 0)
                        {
                            BeginInversionIndex = index;
                        }

                        DrawInversionRegion(i, (int)tmlEvent.BadPriorityInversion, false);
                        inversionMarked = true;
                    }

                    //Left off here do if /else above to catch this... Make sure it is only on startindex other wise skip.

                    if ((i == startIndex) && (inversionMarked == false))
                    {
                        if (BeginInversionIndex < 0)
                        {
                            if (PriorityInversionArray.Count > 0)
                            {
                                for (int n = 0; n < (int)PriorityInversionArray.Count; n++)
                                {
                                    if ((startIndex >= (int)PriorityInversionArray[n]) && ((startIndex < tfi.Events[(int)PriorityInversionArray[n]].BadPriorityInversion) || (startIndex < tfi.Events[(int)PriorityInversionArray[n]].PriorityInversion)))
                                    {
                                        TmlEvent tmlEventI = tfi.Events[(int)PriorityInversionArray[n]];
                                        if (tmlEventI.PriorityInversion > 0)
                                        {
                                            BeginInversionIndex = index;
                                            DrawInversionRegion(i, (int)tmlEventI.PriorityInversion, true);
                                        }
                                        else if (tmlEventI.BadPriorityInversion > 0)
                                        {
                                            BeginInversionIndex = index;
                                            DrawInversionRegion(i, (int)tmlEventI.BadPriorityInversion, false);
                                        }
                                    }
                                    else if (startIndex < (int)PriorityInversionArray[n])
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    /* Create a new instance for each event add the icon and set its corresponding tooltip text.  */
                    Code.Event evt = Code.Event.CreateInstance(tmlEvent, threadName, tfi);
                    string tooltip = evt.GetEventDetailString(tfi);
                    FrameworkElement icon = evt.CreateIcon((int)drawingIconWidth);
                    Canvas.SetLeft(icon, i * iconWidth - sbEvents.Value);
                    canvasEvent.Children.Add(icon);
                    icon.ToolTip = tooltip;

                    if (threadPos >= 0 && threadPos < EventDisplayHeight)
                    {
                        icon = evt.CreateIcon((int)drawingIconWidth);
                        canvasEvents.Children.Add(icon);
                        Canvas.SetLeft(icon, i * iconWidth - sbEvents.Value);
                        Canvas.SetTop(icon, (index * GetEventDisplayHeight()));
                        icon.ToolTip = tooltip;
                    }

                    /* check to see if we have a case where a joint-line exists.  */
                    if (prevIndex != -1 && Math.Abs(index - prevIndex) > 1)
                    {
                        /* Draw the joint line.  */
                        Rectangle line = new Rectangle();
                        line.Width = 1;
                        line.Height = Math.Abs(index - prevIndex) * GetEventDisplayHeight();
                        line.StrokeThickness = 1.0;
                        line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                        canvasEvents.Children.Add(line);
                        Canvas.SetLeft(line, i * iconWidth - offsetH);
                        Canvas.SetTop(line, (index < prevIndex ? index : prevIndex) * GetEventDisplayHeight());
                    }

                    /* If the event count is not 0 and the previous index is not equal to current index.  */
                    /* Detect a context switch after this event.  */
                    if (ec != 0 && prevIndex != index)
                    {
                        canvasContext.Children.Add(MakeContextBar(i - ec, i - 1, alt));
                        alt = !alt;

                        /* reset event count.  */
                        ec = 0;
                    }

                    /* Increment the event count.  */
                    ec += 1;

                    /* Set previous index to index.  */
                    prevIndex = index;
                }

                if (ec != 0)
                {
                    canvasContext.Children.Add(MakeContextBar(endIndex + 1 - ec, endIndex, alt));
                    ec = 0;
                }

                /* For each detail window that currently exist.  */
                foreach (DetailsWindow dw in detailsWindowList)
                {
                    /* Add the window to the canvas.  */
                    canvasEvents.Children.Add(dw);

                    /* If the details window tag is not null.  */
                    if (null != dw.Tag)
                    {
                        /* Add the line attached to the details window if a line exists.  */
                        if (dw.Tag.GetType() == typeof(Line))
                        {
                            Line line = (Line)dw.Tag;
                            canvasEvents.Children.Add(line);
                        }
                    }
                }

                // add delta Mask to events canvas
                canvasEvents.Children.Add(_deltaMask);
            }
            else
            {
                /* Set the width, height, and draw the grid.  */
                canvasEvents.Width = width;
                canvasEvents.Height = height;
                DrawGrid(canvasEvents, width, height, offsetH, offsetV);
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
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[threadKey], (uint)startIndex, (uint)endIndex, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;

                        for (int m = 0; m < executionStatusList.Count; m++)
                        {
                            check = new Rectangle();
                            check.Height = 1;

                            int color = Convert.ToInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            int currentIndex = Convert.ToInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture) - startIndex;

                            if (executionStatusList.Count == 1)
                            {
                                //check.Width = GetEventDisplayWidth() * (endIndex - startIndex);
                                check.Width = iconWidth * (endIndex - startIndex);
                                currentIndex = startIndex;
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
                                currentIndex = startPoint;
                                if (endPoint >= startPoint)
                                {
                                    //check.Width = GetEventDisplayWidth() * (endPoint - startPoint);
                                    check.Width = iconWidth * (endPoint - startPoint);
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
                            Canvas.SetLeft(check, currentIndex * iconWidth - sbEvents.Value);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[threadKey]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));
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
                        //executionStatusList = tfi.DictThreadExecutionStatus[(uint)dictThread[threadKey]];
                        ELTMLManaged.TMLFunctions.ThreadExecutionStatus((uint)dictThread[threadKey], (uint)startIndex, (uint)endIndex, executionStatusList, ((uint)tfi.Events.Count * 2));
                        Rectangle check;
                        for (int m = 0; m < executionStatusList.Count; m++)
                        {
                            //if (executionStatusList[m].eventNumber < startIndex && executionStatusList[m].eventNumber > endIndex)
                            //    continue;

                            check = new Rectangle();
                            check.Height = 1;

                            int color = Convert.ToInt32(executionStatusList[m].Status, CultureInfo.InvariantCulture);
                            int currentIndex = Convert.ToInt32(executionStatusList[m].EventNumber, CultureInfo.InvariantCulture) - startIndex;

                            if (executionStatusList.Count == 1)
                            {
                                //check.Width = GetEventDisplayWidth() * (endIndex - startIndex);
                                check.Width = iconWidth * (endIndex - startIndex);
                                currentIndex = startIndex;
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
                                currentIndex = startPoint;
                                if (endPoint >= startPoint)
                                {
                                    //check.Width = GetEventDisplayWidth() * (endPoint - startPoint);
                                    check.Width = iconWidth * (endPoint - startPoint);
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
                            Canvas.SetLeft(check, currentIndex * iconWidth - sbEvents.Value);
                            Canvas.SetTop(check, (((uint)dictThreadOffset[threadKey]) * GetEventDisplayHeight()) + (GetEventDisplayHeight() - 1));
                        }

                        a++;
                    }
                }
            }
        }

        private void DrawInversionRegion(int beginIndex, int endIndex, bool bGoodInversion)
        {
            Rectangle _rectInversion = new Rectangle();
            _rectInversion.Height = svRight.ViewportHeight + sbEvents.Value;
            if (_rectInversion.Height < canvasEvents.Height)
            {
                _rectInversion.Height = canvasEvents.Height;
            }
            _rectInversion.Width = (endIndex - beginIndex) * iconWidth;
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
            //_rectInversion.Opacity = 0.3;
            Canvas.SetLeft(_rectInversion, beginIndex * iconWidth - sbEvents.Value);
            Canvas.SetTop(_rectInversion, 0);
            canvasEvents.Children.Add(_rectInversion);
        }

        /* Function to draw the eventID bar.  */

        private void DrawEventIdBar(int startIndex, int endIndex)
        {
            /* check to ensure the endIndex is greater than the startIndex.  */
            if (endIndex < startIndex)
                return;

            double height = this.canvasEventId.Height;

            /* Create a white rectangle to clear any old icon.  */
            Rectangle rectBack = new Rectangle();
            rectBack.Height = this.canvasEventId.Height;
            rectBack.Width = (endIndex - startIndex + 1) * iconWidth;
            rectBack.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            this.canvasEventId.Children.Add(rectBack);
            //Canvas.SetLeft(rectBack, startIndex * iconWidth - sbEvents.Value);
            Canvas.SetLeft(rectBack, 0);

            /* Trans Line.  */
            Line lineTrans = new Line();
            lineTrans.StrokeThickness = 2;
            lineTrans.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            lineTrans.X1 = startIndex * iconWidth - sbEvents.Value;
            lineTrans.X2 = (endIndex + 1) * iconWidth - sbEvents.Value;// canvasTicks.Width;
            lineTrans.Y1 = lineTrans.Y2 = this.canvasEventId.Height;
            this.canvasEventId.Children.Add(lineTrans);

            int segments = (int)((endIndex - startIndex) / 100);
            int indexIncrement = segments + 1;
            int segmentIncrement = 10 * segments + 10;
            if (segments > 0)
            {
                indexIncrement = segments;
                segmentIncrement = 10 * (segments - 1) + 10;
            }

            int newStartIndex = startIndex;
            if (startIndex % indexIncrement != 0)
            {
                newStartIndex = ((int)(startIndex / indexIncrement) + 1) * indexIncrement;
            }

            /* Loop through to draw the event number ticks.  */
            for (int i = newStartIndex; i <= endIndex; i += indexIncrement)
            {
                Line line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                line.Y2 = this.canvasEventId.Height;

                // KGM Fix these hard-coded height differences
                if (0 == i % segmentIncrement)
                {
                    line.Y1 = this.canvasEventId.Height - 22;
                }
                else
                {
                    line.Y1 = this.canvasEventId.Height - 10;
                }
                line.X1 = line.X2 = i * iconWidth - sbEvents.Value;
                this.canvasEventId.Children.Add(line);

                /* Draw the event id number.  */
                if (i % segmentIncrement == 0)
                {
                    TextBlock tbEventId = new TextBlock(new Run(Convert.ToString(i, CultureInfo.CurrentCulture)));
                    tbEventId.FontSize = 14;
                    tbEventId.FontWeight = FontWeight.FromOpenTypeWeight(900);
                    tbEventId.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    tbEventId.Tag = i;
                    //Canvas.SetLeft(tbEventId, i * iconWidth - sbEvents.Value);
                    //Canvas.SetTop(tbEventId, 0);
                    canvasEventId.Children.Add(tbEventId);

                    tbEventId.Loaded += new RoutedEventHandler(tbEventId_Loaded);
                }
            }
        }

        /* Function to Create the context bar.  */

        private Canvas MakeContextBar(int startIndex, int endIndex, bool alt)
        {
            Canvas cnvs = new Canvas();
            Canvas.SetLeft(cnvs, startIndex * iconWidth - sbEvents.Value);

            /* Boundary check the startIndex.  */
            if (startIndex == endIndex)
                return cnvs;

            /* Boundary check the width.  */
            double width = (endIndex - startIndex + 1) * iconWidth;

            /* Create a rectangle to draw the colors of the context bar.  */
            Rectangle rt = new Rectangle();
            //rt.Height = 20;
            rt.Height = GetEventDisplayHeight();
            rt.Width = width;
            rt.StrokeThickness = 0.5;
            rt.Stroke = new SolidColorBrush(Color.FromArgb(255, 100, 100, 100));

            /* Alternate the color of the conext from white to gray.  */
            if (alt)
            {
                rt.Fill = new SolidColorBrush(Color.FromArgb(255, 200, 200, 255));
            }
            else
            {
                rt.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }

            /* Add it.  */
            cnvs.Children.Add(rt);

            if (width < 100)
                return cnvs;

            TextBlock tb = new TextBlock();

            TmlEvent tmlEvent = tfi.Events[startIndex];
            int idx = tfi.FindThreadIndex(tmlEvent);

            /* Add the text to the bar.  */
            if (idx != -1)
            {
                if (tfi.Threads[idx].Address == 0x00000000)
                    tb.Text = "Idle";
                else if (tfi.Threads[idx].Address == 0xF0F0F0F0)
                    tb.Text = "Initialize";
                else
                    tb.Text = tfi.Threads[idx].Name;

                if ((tfi.Threads[idx].Address != 0XF0F0F0F0) &&
                    (tfi.Threads[idx].Address != 0) &&
                    (tfi.Threads[idx].Address != 0xFFFFFFFF))
                    tb.Text += " (0x" + tfi.Threads[idx].Address.ToString("X8", CultureInfo.CurrentCulture) + ")";
            }

            cnvs.Children.Add(tb);
            cnvs.ToolTip = tb.Text;
            return cnvs;
        }

        /* Handler for a left button click on the mouse.  */

        private void canvasEvent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _deltaMask.Width = 0;

            if (null != this.DeltaChanged)
                DeltaChanged(this, new Code.DeltaEventArgs(0, 0, 0));

            if (null != this.indicator)
            {
                Point point = e.GetPosition(canvasEvents);
                int eventIndex = Convert.ToInt32(Math.Floor((point.X + sbEvents.Value) / iconWidth), CultureInfo.InvariantCulture);
                if (eventIndex < tfi.Events.Count)
                {
                    this.indicator.Show((point.X + sbEvents.Value));
                    this.tfi.CurrentEventIndex = eventIndex;

                    this.navigator.MoveTo(eventIndex, false);
                }
            }
        }

        /* Handler for a left button click on the mouse.  */

        private void canvasEvents_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
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
                DeltaChanged(this, new Code.DeltaEventArgs(0, 0, 0));

            /* If the indicator isn't already the current indicator.  */
            if (null != this.indicator)
            {
                /* Get the current eventIndex, set the current event index of the thread file info and moveTo.  */
                int eventIndex = Convert.ToInt32(Math.Floor((point.X + sbEvents.Value) / iconWidth), CultureInfo.InvariantCulture);
                //setInformation(string name, string address, string description, int index, bool selectorManual)
                if (eventIndex < tfi.Events.Count && eventIndex >= 0)
                //if (eventIndex < tfi.Events.Count)
                {
                    /* Show the indicator.  */
                    //this.indicator.Show(eventIndex * iconWidth + iconWidth / 2);
                    this.indicator.Show(point.X + sbEvents.Value);

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
        }

        private void canvasEvents_MouseMove(object sender, MouseEventArgs e)
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

                int beginEventIndex;
                int endEventIndex;
                //synchronize position & size of _deltaMask
                if (point.X >= _deltaStartPoint.X)
                {
                    Canvas.SetLeft(_deltaMask, _deltaStartPoint.X - sbEvents.Value);
                    _deltaMask.Width = point.X - _deltaStartPoint.X;

                    beginEventIndex = Convert.ToInt32(Math.Floor((_deltaStartPoint.X) / iconWidth), CultureInfo.InvariantCulture);
                    endEventIndex = Convert.ToInt32(Math.Floor((point.X) / iconWidth), CultureInfo.InvariantCulture);
                }
                else
                {
                    Canvas.SetLeft(_deltaMask, point.X - sbEvents.Value);
                    _deltaMask.Width = _deltaStartPoint.X - point.X;

                    beginEventIndex = Convert.ToInt32(Math.Floor((point.X) / iconWidth), CultureInfo.InvariantCulture);
                    endEventIndex = Convert.ToInt32(Math.Floor((_deltaStartPoint.X) / iconWidth), CultureInfo.InvariantCulture);
                }

                if (endEventIndex >= tfi.Events.Count)
                    endEventIndex = tfi.Events.Count - 1;

                if (beginEventIndex >= tfi.Events.Count)
                    beginEventIndex = tfi.Events.Count - 1;

                if (beginEventIndex < 0)
                    beginEventIndex = 0;

                if (endEventIndex < 0)
                    endEventIndex = 0;

                // call delta changed event handler
                //long val = Properties.Settings.Default.uSecPerTick;
                if (null != this.DeltaChanged)
                {
                    long deltaTicks = tfi.Events[endEventIndex].RelativeTicks - tfi.Events[beginEventIndex].RelativeTicks;

                    string x = (deltaTicks / tickMappingValue).ToString("0.00", CultureInfo.InvariantCulture);

                    if (tickMappingValue != 0)
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, Convert.ToDouble(x, CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, 0));
                    }
                }
            }
        }

        private void canvasEvents_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
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

                int beginEventIndex;
                int endEventIndex;
                //synchronize position & size of _deltaMask
                if (point.X >= _deltaStartPoint.X)
                {
                    double offDist = point.X - width;
                    Canvas.SetLeft(_deltaMask, _deltaStartPoint.X - sbEvents.Value);
                    _deltaMask.Width = point.X - _deltaStartPoint.X;

                    beginEventIndex = Convert.ToInt32(Math.Floor((_deltaStartPoint.X) / iconWidth), CultureInfo.InvariantCulture);
                    endEventIndex = Convert.ToInt32(Math.Floor((point.X) / iconWidth), CultureInfo.InvariantCulture);
                }
                else
                {
                    Canvas.SetLeft(_deltaMask, point.X - sbEvents.Value);
                    _deltaMask.Width = _deltaStartPoint.X - point.X;

                    beginEventIndex = Convert.ToInt32(Math.Floor((point.X) / iconWidth), CultureInfo.InvariantCulture);
                    endEventIndex = Convert.ToInt32(Math.Floor((_deltaStartPoint.X) / iconWidth), CultureInfo.InvariantCulture);
                }

                if (endEventIndex >= tfi.Events.Count)
                    endEventIndex = tfi.Events.Count - 1;

                if (beginEventIndex >= tfi.Events.Count)
                    beginEventIndex = tfi.Events.Count - 1;

                if (beginEventIndex < 0)
                    beginEventIndex = 0;

                if (endEventIndex < 0)
                    endEventIndex = 0;

                // call delta changed event handler
                //long val = Properties.Settings.Default.uSecPerTick;

                if (null != this.DeltaChanged)
                {
                    long deltaTicks = tfi.Events[endEventIndex].RelativeTicks - tfi.Events[beginEventIndex].RelativeTicks;
                    string x = (deltaTicks / tickMappingValue).ToString("0.00", CultureInfo.InvariantCulture);

                    if (tickMappingValue != 0)
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, Convert.ToDouble(x, CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        DeltaChanged(this, new Code.DeltaEventArgs(deltaTicks, 0, 0));
                    }
                }

                _deltaStarted = false;
            }

            canvasEvents.ReleaseMouseCapture();
        }

        private void canvasEvents_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ShowDelta(e);
        }

        private void canvasEvent_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            //ShowDelta(e);
        }

        private void ShowDelta(MouseButtonEventArgs e)
        {
            /* Check if a file is loaded.  */
            if (this.tfi == null) return;

            /* Get the current point.  */
            Point point = e.GetPosition(canvasEvents);
            int eventIndex = Convert.ToInt32(Math.Floor(point.X / iconWidth), CultureInfo.InvariantCulture);
            if (eventIndex < 0 || eventIndex > tfi.Events.Count - 1) return;

            TmlEvent tmlEndEvent = tfi.Events[eventIndex];
            TmlEvent tmlBeginEvent = tfi.Events[this.tfi.CurrentEventIndex];

            DeltaWindow dlg = new DeltaWindow(tmlBeginEvent, tmlEndEvent);
            dlg.Owner = (Window)((Grid)((DockPanel)((TabControl)((TabItem)this.Parent).Parent).Parent).Parent).Parent;
            dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            bool? bRes = dlg.ShowDialog();
            return;
        }

        /* Handler for the window / grid changing size.  */

        private void grid1_SizeChanged(object sender, SizeChangedEventArgs e)
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
            svEventId.Width = svContext.Width = svEvent.Width = svRight.Width = width;
            sbEvents.ViewportSize = grid1.ColumnDefinitions[1].ActualWidth;

            //CalcNumberOfThreadsToDisplay(height);

            if (tfi != null)
            {
                height = tfi.Threads.Count * eHeight;

                /* Set the height of the total of events and objects.  */
                canvasEvents.Height = canvasObjects.Height = height; // +40;
                svLeft.Height = svRight.Height = Height;

                if (SplitterPositionChanged != null)
                    SplitterPositionChanged(this, null);

            }

            canvasEvents.Width = canvasEvent.Width = canvasContext.Width = width;
            updateHScrollBar();
        }

        /* Handler for the scroll change - redraw the threads.  */

        private void svLeft_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {

            if (SplitterPositionChanged != null)
                SplitterPositionChanged(this, null);
        }

        // Handler for the mouse double click event.

        private void svRight_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Check if a file is loaded.
            if (this.tfi == null) return;

            // Get the current point.
            Point point = e.GetPosition(canvasEvents);
            double realX = point.X + sbEvents.Value;
            int eventIndex = Convert.ToInt32(Math.Floor(realX / iconWidth), CultureInfo.InvariantCulture);
            if (eventIndex < 0 || eventIndex > tfi.Events.Count - 1) return;
            TmlEvent tmlEvent = tfi.Events[eventIndex];
            uint threadIndex = (uint)Convert.ToInt32(Math.Floor((point.Y) / GetEventDisplayHeight()), CultureInfo.InvariantCulture);

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
            double delta_x = point.X - EventIconPosXGet(eventIndex);
            double delta_y = point.Y >= -64 && point.Y <= -40 ? point.Y + 80 : point.Y + 30 - EventIconPosYGet(index);

            ShowEventDetailsWindow(eventIndex, delta_x, delta_y);
        }

        // Handler for a value change on the events.

        private void sbEvents_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double offsetH = e.NewValue;

            // Move _detalMask when vscroll changed
            double deltaMaskLeft = Canvas.GetLeft(_deltaMask);
            deltaMaskLeft = deltaMaskLeft - e.NewValue + e.OldValue;
            Canvas.SetLeft(_deltaMask, deltaMaskLeft);

            // Check if we have valid trace file information.
            if (tfi != null)
            {
                DrawEvents(offsetH);

                // Recalculate detail window's X position.
                double delta = e.OldValue - e.NewValue;

                // Move the details window as necessary.
                foreach (DetailsWindow dwnd in detailsWindowList)
                {
                    dwnd.MoveX(delta, sbEvents.Value, iconWidth);
                }

                // Check if the curerntEventIndex is greater than 0.
                if (tfi.CurrentEventIndex >= 0)
                {
                    TmlEvent tmlEvent = tfi.Events[tfi.CurrentEventIndex];
                    this.indicator.Show(tfi.CurrentEventIndex * iconWidth - offsetH);
                }
            }
        }

        private double EventIconPosXGet(int eventIndex)
        {
            return eventIndex * iconWidth + iconWidth / 2 - sbEvents.Value;
        }

        private void ShowEventDetailsWindow(int eventIndex, double deltaX, double deltaY)
        {
            if (eventIndex < 0 || eventIndex > tfi.Events.Count - 1) return;
            TmlEvent tmlEvent = tfi.Events[eventIndex];
            int threadIndex = tfi.FindThreadIndex(tmlEvent);
            string threadName = "Unknown";

            if (threadIndex == -1)
            {
                return;
            }

            threadName = tfi.Threads[threadIndex].Name;

            HideEventDetailsWindow(eventIndex);

            Code.Event evt = Code.Event.CreateInstance(tmlEvent, threadName, tfi);

            // Create a new instance of details window.
            DetailsWindow dw = new DetailsWindow(this, canvasEvents);

            // Create the line to attach to the details window.
            Line line = new Line();
            line.X1 = EventIconPosXGet(eventIndex);
            line.Y1 = EventIconPosYGet(threadIndex);

            double left = line.X1 + deltaX;
            double top = line.Y1 + deltaY;

            // Prevent the window poping up off screen.
            if ((left + dw.Width) > canvasEvents.Width)
            {
                left = canvasEvents.Width - dw.Width;
            }

            // Create the line to attach to the details window.
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
            string msg;

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
