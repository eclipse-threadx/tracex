using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Automation.Peers;
using System.Windows.Automation;

using AzureRTOS.Tml;
using AzureRTOS.TraceManagement.Code;

namespace AzureRTOS.TraceManagement.Components
{
    public class TraceViewControl : ContentControl, ITraceView
    {
        // constants for event icons

        protected double iconWidth = 12;
        protected double OldEventDisplayHeight = 0.0;

        protected const double MIN_ICON_WIDTH = 1;

        protected double DrawingScaling = 1.0; // used for drawing

        protected bool enableZoomOut = false;
        protected bool enableZoomIn = false;

        protected int tw1 = 0;

        private Dictionary<string, RoutedEventHandler> miDict;
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new FrameworkElementAutomationPeer(this);
        }

        public double SiblingViewPortWidth { get; set; }

        public double SiblingViewPortHeight { get; set; }

        public bool EnableZoomOut
        {
            get
            {
                return enableZoomOut;
            }
        }

        public bool EnableZoomIn
        {
            get
            {
                return enableZoomIn;
            }
        }

        protected Code.TraceFileInfo tfi;
        protected Navigator navigator;
        protected Code.Indicator indicator;
        protected List<DetailsWindow> detailsWindowList = new List<DetailsWindow>();

        protected int NumberOfThreadsToDisplay = 0;
        protected const int MinimumThreadsToDisplay = 20;
        protected double EventDisplayHeight { get; set; }
        public int GetThreadNameDisplayWidth()
        {
            if (null == tfi) return 265;

            tw1 = 0;

            // Go through once to get the longest string name.
            // KGM this seems messed up, the grid column width and the button width do not have to match in this case, 
            // we are calculating these widths in completely different ways.

            for (int j = 0; j < tfi.Threads.Count; j++)
            {
                int mult = (int)(GetEventDisplayHeight() / 24 * 10);
                TmlThread tThrd = tfi.Threads[j];

                if (8 + ((tThrd.Name.Length + tThrd.Address.ToString(CultureInfo.InvariantCulture).Length) * mult) > tw1)
                {
                    tw1 = 8 + ((tThrd.Name.Length + tThrd.Address.ToString(CultureInfo.InvariantCulture).Length + "Priority: ".Length + tThrd.HighestPriority.ToString(CultureInfo.InvariantCulture).Length) * mult);
                }
            }
            return tw1;
        }

        protected bool _viewActivated = false;
        protected bool _threadsOrderChanged = false;

        // delta related variables
        protected bool _deltaStarted = false;
        protected Point _deltaStartPoint;
        protected Rectangle _deltaMask = new Rectangle();

        public EventHandler<Code.DeltaEventArgs> DeltaChanged;
        public EventHandler<Code.ZoomEventArgs> ZoomLimitReached;

        protected Dictionary<string, uint> dictThread;
        protected Dictionary<string, uint> dictThreadOffset;

        protected static int BeginInversionIndex = 0;
        protected static int EndInversionIndex = 0;

        public EventHandler<EventArgs> SplitterPositionChanged;

        protected double ZoomFactor = 1.0; // to remmber the zoom state
        protected int EventIndicatorIndex = 0;


        public TraceViewControl()
        {
            // Initialize thread menu dictionary.
            miDict = new Dictionary<string, RoutedEventHandler>()
            {
                { "Thread Info", ThreadInfoClickEventHandler },
                { "Move to top", TopClickEventHandler },
                { "Move to bottom", BottomClickEventHandler },
                { "Alphabetic Sort", AlphaSortClickEventHandler },
                { "Creation Order Sort", CreationOrderSortClickEventHandler },
                { "Execution Time Sort", ExecutionTimeSortClickEventHandler },
                { "Lowest Priority Sort", LoPrioritySortClickEventHandler },
                { "Highest Priority Sort", HiPrioritySortClickEventHandler },
                { "Most Events Sort", MostEventsSortClickEventHandler },
                { "Least Events Sort", LeastEventsSortClickEventHandler }
            };
        }

        public virtual void Zoom(double multiple)
        {
            if (ZoomLimitReached != null)
            {
                ZoomLimitReached(this, new AzureRTOS.TraceManagement.Code.ZoomEventArgs(enableZoomOut, enableZoomIn));
            }
        }

        public virtual int GetNextPageStartIndex(int currentIndex) { return 0; }
        public virtual int GetPreviousPageStartIndex(int currentIndex) { return 0; }
        public virtual void Initialize(Code.TraceFileInfo tfi, Navigator navigator)
        {
        }

        public virtual void MoveToEvent(int eventIndex, bool centerMarker) { }

        public virtual void RedrawThreads()
        {
        }

        public double GetEventDisplayHeight()
        {
            iconWidth = SystemFonts.MessageFontSize;
            double height = iconWidth * 2;

            return height;
        }

        public double GetEventDisplayWidth()
        {
           return iconWidth;
        }

        protected void CalcNumberOfThreadsToDisplay(double height)
        {
            if (tfi == null)
                return;

            NumberOfThreadsToDisplay = (int)(height / (double)GetEventDisplayHeight()) + 1;
            //NumberOfThreadsToDisplay = NumberOfThreadsToDisplay < MinimumThreadsToDisplay ? MinimumThreadsToDisplay : NumberOfThreadsToDisplay;
            NumberOfThreadsToDisplay = NumberOfThreadsToDisplay > tfi.Threads.Count ? tfi.Threads.Count : NumberOfThreadsToDisplay;
            if (tfi.Threads.Count * GetEventDisplayHeight() <= height * 1.5)
            {
                NumberOfThreadsToDisplay = tfi.Threads.Count;
            }

            //EventDisplayHeight = (NumberOfThreadsToDisplay + 5) * GetEventDisplayHeight();
            EventDisplayHeight = (NumberOfThreadsToDisplay + 1) * GetEventDisplayHeight();

        }

        public double GetZoomFactor()
        {
            return ZoomFactor;
        }

        public void cleanDeltaMask()
        {
            //this.indicator.Show(0);
            //this.navigator.MoveTo(0);

            this._deltaMask.Visibility = Visibility.Hidden;
        }

        public void ActivateView(bool activate)
        {
            this._viewActivated = activate;
            if (this._viewActivated && ZoomLimitReached != null)
            {
                ZoomLimitReached(this, new AzureRTOS.TraceManagement.Code.ZoomEventArgs(enableZoomOut, enableZoomIn));
            }
        }

        public void SetThreadOrderChanged()
        {
            _threadsOrderChanged = true;
        }

        protected void findThreadOrder()
        {
            if (tfi != null)
            {
                int j = 0;
                dictThread.Clear();
                for (int i = 0; i < tfi.Threads.Count; i++)
                {
                    if ((tfi.Threads[i].Address != 0xFFFFFFFF) && (tfi.Threads[i].Address != 0x00000000) && (tfi.Threads[i].Address != 0xF0F0F0F0))
                    {
                        string threadKey = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[i].Name, tfi.Threads[i].Address);

                        if (!dictThread.ContainsKey(threadKey))
                        {
                            dictThread.Add(threadKey, (uint)j);
                        }
                        j++;
                    }
                }
            }
        }

        protected void findThreadChangeOrder()
        {
            int j = 0;
            dictThreadOffset.Clear();
            for (int i = 0; i < tfi.Threads.Count; i++)
            {
                string threadKey = string.Format(CultureInfo.InvariantCulture, "{0} {1}", tfi.Threads[i].Name, tfi.Threads[i].Address);

                if (!dictThreadOffset.ContainsKey(threadKey))
                {
                    dictThreadOffset.Add(threadKey, (uint)(i));
                }

                if ((tfi.Threads[i].Address == 0xFFFFFFFF) || (tfi.Threads[i].Address == 0x00000000) || (tfi.Threads[i].Address == 0xF0F0F0F0))
                {
                    j++;
                }
            }
        }

        protected double EventIconPosYGet(int threadIndex)
        {
            return (threadIndex + 1) * GetEventDisplayHeight();
        }

        protected void MoveEventDetailsWindow(int eventIndex, Code.Direction direction)
        {
            TmlEvent tmlEvent = tfi.Events[eventIndex];

            foreach (DetailsWindow wnd in detailsWindowList)
            {
                if (wnd.RelatedEventIndex == tmlEvent.Index)
                {

                    switch (direction)
                    {
                        case Code.Direction.Left:
                            wnd.ShiftPosition(-1, 0);
                            break;

                        case Code.Direction.Right:
                            wnd.ShiftPosition(1, 0);
                            break;

                        case Code.Direction.Up:
                            wnd.ShiftPosition(0, -1);
                            break;

                        case Code.Direction.Down:
                            wnd.ShiftPosition(0, 1);
                            break;
                    }
                    break;
                }
            }
        }

        protected new string PreviewKeyDown(object sender, KeyEventArgs e)
        {
            string msg = "";

            switch (e.Key)
            {
                case Key.Left:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                    {
                        MoveEventDetailsWindow(navigator.GetCurrentIndex(), Code.Direction.Left);
                    }
                    else
                    {
                        navigator.MovePrevious();
                        msg = "event " + navigator.GetCurrentIndex().ToString();
                    }
                    e.Handled = true;
                    break;

                case Key.Right:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                    {
                        MoveEventDetailsWindow(navigator.GetCurrentIndex(), Code.Direction.Right);
                    }
                    else
                    {
                        navigator.MoveNext();
                        msg = "event " + navigator.GetCurrentIndex().ToString();
                    }
                    e.Handled = true;
                    break;

                case Key.Up:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                    {
                        MoveEventDetailsWindow(navigator.GetCurrentIndex(), Code.Direction.Up);
                    }
                    e.Handled = true;
                    break;

                case Key.Down:
                    if (e.KeyboardDevice.Modifiers == ModifierKeys.Shift)
                    {
                        MoveEventDetailsWindow(navigator.GetCurrentIndex(), Code.Direction.Down);
                    }
                    e.Handled = true;
                    break;

                case Key.PageUp:
                    navigator.PageUp();
                    msg = "event " + navigator.GetCurrentIndex().ToString();
                    e.Handled = true;
                    break;

                case Key.PageDown:
                    navigator.PageDown();
                    msg = "event " + navigator.GetCurrentIndex().ToString();
                    e.Handled = true;
                    break;

                case Key.Home:
                    navigator.MoveFirst();
                    msg = "event " + navigator.GetCurrentIndex().ToString();
                    e.Handled = true;
                    break;

                case Key.End:
                    navigator.MoveLast();
                    msg = "event " + navigator.GetCurrentIndex().ToString();
                    e.Handled = true;
                    break;
            }

            return msg;
        }

        #region IDetailsWindowManager Members

        public void RemoveDetailsWindow(Components.DetailsWindow detailsWindow, Canvas canvasEvents)
        {
            this.detailsWindowList.Remove(detailsWindow);
            if (null != detailsWindow.Tag)
            {
                Line line = (Line)detailsWindow.Tag;
                canvasEvents.Children.Remove(line);
            }

            canvasEvents.Children.Remove(detailsWindow);
        }

        protected void UpdateDetailsWindows(Canvas canvasEvents)
        {
            foreach (UIElement ui in canvasEvents.Children)
            {
                Type type = ui.GetType();
                if (type == typeof(DetailsWindow))
                {
                    DetailsWindow dw = (DetailsWindow)ui;
                    if (dw != null)
                    {
                        TmlEvent relatedEvent = tfi.Events[(int)dw.RelatedEventIndex];
                        int index = tfi.FindThreadIndex(relatedEvent);
                        dw.UpdateY(index * GetEventDisplayHeight());
                    }
                }
            }
        }
        
        // Get thread caption info.
        protected string MakeThreadCaptionInfo(int threadIndex)
        {
            TmlThread tmlThrd = tfi.Threads[threadIndex];
            string caption = tmlThrd.Name;
            uint address = tmlThrd.Address;
            uint highestPriority = tmlThrd.HighestPriority;

            // As long as there is caption information.
            if (caption.Length > 0)
            {
                char[] arr = caption.ToCharArray();
                string tmp = new string(arr[0], 1);
                tmp = tmp.ToUpper(CultureInfo.CurrentUICulture);
                arr[0] = tmp[0];
                caption = new string(arr);
            }

            // If it is not initialize/idle or interrupt.
            if ((address != 0xF0F0F0F0) && (address != 0xFFFFFFFF))
            {
                if (highestPriority != 0xFFFFFFFF)
                {
                    caption += " (0x" + address.ToString("X8", CultureInfo.CurrentCulture) + ")" + " [Priority: " + highestPriority.ToString(CultureInfo.CurrentCulture) + "]";
                }
                else
                {
                    caption += " (0x" + address.ToString("X8", CultureInfo.CurrentCulture) + ")" + " [Priority: ?]";
                }
            }

            return caption;
        }

        private void ThreadDragOverEventHandler(object sender, DragEventArgs e)
        {
            Canvas threadCanvas = (Canvas)sender;
            Line line = (Line)threadCanvas.Children[threadCanvas.Children.Count - 1];
            Point pt = e.GetPosition(threadCanvas);
            if (pt.Y > GetEventDisplayHeight() / 2)
            {
                line.Y1 = line.Y2 = GetEventDisplayHeight();
            }
            else
            {
                line.Y1 = line.Y2 = 0;
            }
            line.Visibility = Visibility.Visible;
        }
        
        private void ThreadDragLeaveEventHandler(object sender, DragEventArgs e)
        {
            Canvas threadCanvas = (Canvas)sender;
            threadCanvas.Children[threadCanvas.Children.Count - 1].Visibility = Visibility.Collapsed;
        }

        private void ThreadDropEventHandler(object sender, DragEventArgs e)
        {
            Canvas threadCanvas = (Canvas)sender;
            int srcIndex = Convert.ToInt32(e.Data.GetData(typeof(int)), CultureInfo.InvariantCulture);
            int destInex = Convert.ToInt32(threadCanvas.Tag, CultureInfo.InvariantCulture);

            Line line = (Line)threadCanvas.Children[threadCanvas.Children.Count - 1];
            Point pt = e.GetPosition(threadCanvas);
            if (pt.Y < GetEventDisplayHeight() / 2)
            {
                destInex -= 1;
            }
            line.Visibility = Visibility.Collapsed;

            ExchangeThreadPostion(srcIndex, destInex);
        }

        // Exchange thread position.
        protected virtual void ExchangeThreadPostion(Int32 srcIndex, Int32 destInex) { }

        private void ThreadLeftButtonDownEventHandler(object sender, MouseButtonEventArgs e)
        {
            Canvas threadCanvas = (Canvas)sender;

            DragDrop.DoDragDrop(threadCanvas, new DataObject(threadCanvas.Tag), DragDropEffects.Move);
        }

        private void ThreadKeyDownEventHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Canvas threadCanvas = (Canvas)sender;
                threadCanvas.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                threadCanvas.ContextMenu.IsOpen = true;
            }
        }

        // Add context menu to canvas object.
        private void AddCanvasContextMenu(Canvas canvas, int threadIndex)
        {
            canvas.ContextMenu = new ContextMenu();
            canvas.ContextMenu.Tag = threadIndex;
            canvas.ContextMenu.PlacementTarget = canvas;
            canvas.ContextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;

            foreach (KeyValuePair<string, RoutedEventHandler> kvp in miDict)
            {
                MenuItem miTop = new MenuItem();
                miTop.Tag = threadIndex;
                miTop.Click += new RoutedEventHandler(kvp.Value);
                miTop.Header = kvp.Key;
                canvas.ContextMenu.Items.Add(miTop);
            }
        }

        // Make the object canvas.
        protected Canvas MakeCanvasObject(int threadIndex)
        {
            TmlThread tmlThrd = tfi.Threads[threadIndex];

            // Create the canvas and set the height.
            Canvas canvas = new Canvas();
            canvas.AllowDrop = true;   // Enable AllowDrop.
            canvas.ToolTip = tfi.GetThreadDetailString(tmlThrd);
            

            canvas.DragOver += new DragEventHandler(ThreadDragOverEventHandler);
            canvas.DragLeave += new DragEventHandler(ThreadDragLeaveEventHandler);
            canvas.Drop += new DragEventHandler(ThreadDropEventHandler);
            canvas.MouseLeftButtonDown += new MouseButtonEventHandler(ThreadLeftButtonDownEventHandler);
            canvas.KeyDown += new KeyEventHandler(ThreadKeyDownEventHandler);
            AutomationProperties.SetHelpText(canvas, "Thread Id Button");

            canvas.Height = GetEventDisplayHeight();

            // Add context menu.
            AddCanvasContextMenu(canvas, threadIndex);

            // Create the button for each thread name.
            Rectangle rect = new Rectangle();
            rect.RadiusX = rect.RadiusY = 2;
            rect.Stroke = new SolidColorBrush(Color.FromArgb(255, 64, 64, 64));
            rect.StrokeThickness = 0.5;
            GradientStopCollection gsc = new GradientStopCollection();
            gsc.Add(new GradientStop(Color.FromArgb(255, 242, 242, 242), 0.0));
            gsc.Add(new GradientStop(Color.FromArgb(255, 242, 242, 242), 0.48));
            gsc.Add(new GradientStop(Color.FromArgb(255, 207, 207, 207), 0.52));
            gsc.Add(new GradientStop(Color.FromArgb(255, 207, 207, 207), 1.0));
            rect.Fill = new LinearGradientBrush(gsc, 90.0);
            rect.Height = GetEventDisplayHeight() - 4;
            rect.Width = tw1;
           
            Canvas.SetTop(rect, 2);
            Canvas.SetLeft(rect, 0);
            canvas.Children.Add(rect);

            TextBlock tb = new TextBlock();

            // Set button properties
            tb.Text = MakeThreadCaptionInfo(threadIndex);
            AutomationProperties.SetName(tb, tb.Text + ". Clickable button");
            //AutomationProperties.SetHelpText(tb, "Button");
            tb.Focusable = true;
            tb.MinWidth = 220;

            // Since the button background color is hard-coded, we need to
            // hard code the text color as well.
            tb.Foreground = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));


            Canvas.SetTop(tb, 3);
            Canvas.SetLeft(tb, 5);
            canvas.Children.Add(tb);
            Canvas.SetTop(canvas, GetEventDisplayHeight() * threadIndex);
            canvas.Tag = threadIndex;

            Line line = new Line();
            line.Visibility = Visibility.Collapsed;
            line.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 255));
            line.X1 = 0;
            line.X2 = 400;
            line.Y1 = line.Y2 = 0;
            line.StrokeThickness = 1.0;

            canvas.Children.Add(line);

            return canvas;
        }

        // Show thread info.
        protected void ThreadInfoClickEventHandler(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;

            // Get thread index.
            int threadIndex = Convert.ToInt32(mi.Tag, CultureInfo.InvariantCulture);
            TmlThread tmlThrd = tfi.Threads[threadIndex];
            string infoString = tfi.GetThreadDetailString(tmlThrd);

            // Show thread information.
            MessageBox.Show(infoString, "Thread Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Move thread to top.
        protected void TopClickEventHandler(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            int srcIndex = Convert.ToInt32(mi.Tag, CultureInfo.InvariantCulture);
            int destInex = -1;
            ExchangeThreadPostion(srcIndex, destInex);
        }

        // Move thread to bottom.
        protected void BottomClickEventHandler(object sender, RoutedEventArgs e)
        {
            MenuItem mi = (MenuItem)sender;
            int srcIndex = Convert.ToInt32(mi.Tag, CultureInfo.InvariantCulture);
            int destInex = tfi.Threads.Count - 1;
            ExchangeThreadPostion(srcIndex, destInex);
        }

        // Sort thread by alphabetical order
        protected void AlphaSortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByAlphabetic);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by creation order.
        protected void CreationOrderSortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByCreationOrder);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by excution time
        protected void ExecutionTimeSortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByExecutionTime);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by priority ascending order
        protected void LoPrioritySortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByLowestPriority);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by priority decending order
        protected void HiPrioritySortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByHighestPriority);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by events count decending order
        protected void MostEventsSortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByMostEvents);
            _threadsOrderChanged = true;
            RedrawThreads();
        }

        // Sort thread by priority ascending order
        protected void LeastEventsSortClickEventHandler(object sender, RoutedEventArgs e)
        {
            tfi.SortThreads(ThreadSorting.ByLeastEvents);
            _threadsOrderChanged = true;
            RedrawThreads();
        }
        #endregion IDetailsWindowManager Members

        #region static functions

        // Static function for drawing the grid in timeView mode.
        protected static void DrawGrid(Canvas canvas, double width, double height, double offsetH, double offsetV)
        {
            // If the application does not have an open file return.
            //if (!App.HasOpenFile)
            //    return;
            double eventHeight = SystemFonts.MessageFontSize * 2;
            double eventWidth = eventHeight / 2;
            long startIndex = (long)Math.Floor(offsetH / eventWidth);
            long endIndex = (long)Math.Ceiling((offsetH + width) / eventWidth);

            /* Loop through and create the grid by switching colors.  */
            for (long i = startIndex; i <= endIndex + 1; ++i)
            {
                Rectangle rect = new Rectangle();
                if (i % 10 == 0)
                {
                    rect.Fill = new SolidColorBrush(Color.FromArgb(255, 246, 246, 246));
                }
                else
                {
                    rect.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                }
                rect.Width = eventWidth;
                rect.Height = height + offsetV;
                Canvas.SetLeft(rect, i * eventHeight - offsetH);
                canvas.Children.Add(rect);

                Line line = new Line();
                line.X1 = line.X2 = i * eventWidth - 0.5 - offsetH;
                line.Y1 = 0;
                line.Y2 = height + offsetV;
                line.Stroke = new SolidColorBrush(Color.FromArgb(255, 212, 213, 218));
                line.StrokeThickness = 0.5;
                canvas.Children.Add(line);

                rect = null;
                line = null;
            }

            for (double y = eventHeight; y <= height + offsetV; y += eventHeight)
            {
                Line line = new Line();
                line.X1 = 0;
                line.Y1 = line.Y2 = y;
                line.X2 = width;
                line.Stroke = new SolidColorBrush(Color.FromArgb(255, 212, 213, 218));
                line.StrokeThickness = 0.5;
                canvas.Children.Add(line);
                line = null;
            }
        }

        #endregion static functions
    }
}
