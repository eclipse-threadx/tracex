using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for Navigator.xaml
    /// </summary>
    public partial class Navigator : UserControl
    {
        private int _currentIndex = 0;
        private int _minIndex = 0;
        private int _maxIndex = 0;
        private Code.TraceFileInfo _traceFileInfo;

        public static Code.menuInformation Information = new Code.menuInformation();

        public EventHandler<Code.IndexEventArgs> IndexChanged;
        public EventHandler<Code.PageEventArgs> PageChanged;

        /// <summary>
        /// Constructor for Navigator.
        /// </summary>
        public Navigator()
        {
            InitializeComponent();
            textIndex.Text = "0";
            UpdateButtonState();
        }

        public int GetCurrentIndex() => _currentIndex;

        public void SetInformation(string name, string address, string description, int index)
        {
            Information.Caption = name;
            Information.Value = address;
            Information.Description = description;
            Information.Index = index;
            miSelection.Header = Information.Description.ToString(CultureInfo.CurrentCulture);
            UpdateButtonState();
        }

        public void SetInformation(string name, string address, string description, int index, bool selectorManual)
        {
            if (selectorManual)
            {
                Information.Caption = name;
                Information.Value = address;
                Information.Description = description;
                Information.Index = index;
                miSelection.Header = Information.Description.ToString(CultureInfo.CurrentCulture);
                UpdateButtonState();
            }
        }

        private void MiContext_Click(object sender, RoutedEventArgs e)
        {
            //setInformation(tfi.Events[currentIndex].Context.ToString(CultureInfo.CurrentCulture), tfi.Events[currentIndex].Context.ToString(CultureInfo.CurrentCulture), "Context", 1);
        }

        private void MiEvent_Click(object sender, RoutedEventArgs e)
        {
            MenuItem newEventMenuItem = new MenuItem();
    
            newEventMenuItem = (MenuItem)e.OriginalSource;
            SetInformation(newEventMenuItem.Header.ToString(), newEventMenuItem.Header.ToString(), "Event", 0);
        }

        private void MiSwitches_Click(object sender, RoutedEventArgs e)
        {
            MenuItem newSwitchesMenuItem = new MenuItem();
            newSwitchesMenuItem = (MenuItem)e.OriginalSource;
            SetInformation(newSwitchesMenuItem.Header.ToString(), newSwitchesMenuItem.Header.ToString(), "Switches", 3);
        }

        public void SetIndex(String index)
        {
            textIndex.Text = index;
            _currentIndex = Convert.ToInt32(index, CultureInfo.InvariantCulture);
        }

        public void SetTraceEventInfo(Code.TraceFileInfo tfi)
        {
            _traceFileInfo = tfi;
            _maxIndex = tfi.Events.Count - 1;
            _minIndex = 0;
            _currentIndex = _minIndex;
            textIndex.Text = string.Empty;
            UpdateButtonState();
        }

        // Dead.
        private void UpdateButtonState()
        {
            bool isEvent = (Information.Index == 0);

            if (!isEvent)
            {
                btnFirst.Visibility = Visibility.Hidden;
                btnFirstDisabled.Visibility = Visibility.Visible;
                btnPageUp.Visibility = Visibility.Hidden;
                btnPageUpDisabled.Visibility = Visibility.Visible;

                btnPageDown.Visibility = Visibility.Hidden;
                btnPageDownDisabled.Visibility = Visibility.Visible;
                btnLast.Visibility = Visibility.Hidden;
                btnLastDisabled.Visibility = Visibility.Visible;
            }
            else
            {
                btnFirst.Visibility = Visibility.Visible;
                btnFirstDisabled.Visibility = Visibility.Hidden;
                btnPageUp.Visibility = Visibility.Visible;
                btnPageUpDisabled.Visibility = Visibility.Hidden;

                btnPageDown.Visibility = Visibility.Visible;
                btnPageDownDisabled.Visibility = Visibility.Hidden;
                btnLast.Visibility = Visibility.Visible;
                btnLastDisabled.Visibility = Visibility.Hidden;
            }

            btnFirst.IsEnabled = isEvent;
            btnPageUp.IsEnabled = isEvent;
            textIndex.IsEnabled = isEvent;
            btnPageDown.IsEnabled = isEvent;
            btnLast.IsEnabled = isEvent;
        }

        // Function to refresh the text in the box and the current index.
        private void Refresh(bool userNumber)
        {
            textIndex.Text = _currentIndex.ToString(CultureInfo.CurrentCulture);
            Code.IndexEventArgs e = new Code.IndexEventArgs(_currentIndex, userNumber);
            if (null != IndexChanged) IndexChanged(this, e);

            UpdateButtonState();
        }

        // To quickly move to an index.
        public void MoveTo(int index, bool userNumber)
        {
            _currentIndex = index;
            Refresh(userNumber);
        }

        // To move to a previous index.
        public void MovePrevious()
        {
            if (_currentIndex > _minIndex)
            {
                --_currentIndex;
                Refresh(false);
            }
        }

        // To move to the next index.
        public void MoveNext()
        {
            if (_currentIndex < _maxIndex)
            {
                ++_currentIndex;
                Refresh(false);
            }
        }

        // To skip to the beginning index (0).
        public void MoveFirst()
        {
            _currentIndex = _minIndex;
            Refresh(false);
        }

        // To skip to the last index/event.
        public void MoveLast()
        {
            _currentIndex = _maxIndex;
            Refresh(false);
        }

        // To move to the previous page.
        public void PageUp()
        {
            var arg = new Code.PageEventArgs(Code.Direction.Up)
            {
                Index = _currentIndex
            };
            PageChanged?.Invoke(this, arg);
            MoveTo(arg.Index, false);
        }

        // To move to the next page.
        public void PageDown()
        {
            var arg = new Code.PageEventArgs(Code.Direction.Down)
            {
                Index = _currentIndex
            };
            PageChanged?.Invoke(this, arg);
            MoveTo(arg.Index, false);
        }

        // Handler for button click.
        private void BtnFirst_Click(object sender, RoutedEventArgs e)
        {
            MoveFirst();
            textIndex.Focus();
        }

        // Handler for button click.
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            switch (Information.Index)
            {
                case 0:
                    MovePrevious();
                    textIndex.Focus();
                    break;

                case 1:
                    BtnPreviousThread_Click(sender, e);
                    break;

                case 2:
                    BtnPreviousObject_Click(sender, e);
                    break;

                case 3:
                    BtnPreviousSwitch_Click(sender, e);
                    break;

                case 4:
                    BtnPreviousSameID_Click(sender, e);
                    break;
            }
        }

        // Handler for button click.
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            switch (Information.Index)
            {
                case 0:
                    MoveNext();
                    textIndex.Focus();
                    break;

                case 1:
                    BtnNextThread_Click(sender, e);
                    break;

                case 2:
                    BtnNextObject_Click(sender, e);
                    break;

                case 3:
                    BtnNextSwitch_Click(sender, e);
                    break;

                case 4:
                    BtnNextSameID_Click(sender, e);
                    break;
            }
        }

        // Handler for button click.
        private void BtnLast_Click(object sender, RoutedEventArgs e)
        {
            MoveLast();
            textIndex.Focus();
        }

        // Handler for button click.
        private void TextIndex_KeyDown(object sender, KeyEventArgs e)
        {
            if (Key.Enter == e.Key)
            {
                int index = 0;
                try
                {
                    index = Convert.ToInt32(textIndex.Text, CultureInfo.InvariantCulture);
                }
                catch { }

                if (index < _minIndex) index = _minIndex;
                if (index > _maxIndex) index = _maxIndex;
                textIndex.Text = Convert.ToString(index, CultureInfo.CurrentCulture);
                MoveTo(index, true);
            }
        }

        // Handler for button click.
        private void TextIndex_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key < Key.D0 || e.Key > Key.D9) && (e.Key < Key.NumPad0 || e.Key > Key.NumPad9))
            {
                switch (e.Key)
                {
                    case Key.Enter:
                    case Key.Delete:
                    case Key.Back:
                    case Key.Left:
                    case Key.Right:
                    case Key.Home:
                    case Key.End:
                    case Key.Tab:
                        break;

                    default:
                        e.Handled = true;
                        break;
                }
            }
        }

        // Handler for button click.
        private void BtnPageUp_Click(object sender, RoutedEventArgs e)
        {
            PageUp();
            textIndex.Focus();
        }

        // Handler for button click.
        private void BtnPageDown_Click(object sender, RoutedEventArgs e)
        {
            PageDown();
            textIndex.Focus();
        }

        // Handler for button click for previous event on the same thread.
        private void BtnPreviousThread_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex > 0)
            {
                nextIndex--;
                if (Convert.ToUInt32(Information.Value, 16) == _traceFileInfo.Events[nextIndex].Context)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for next event on the same thread.
        private void BtnNextThread_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex < _traceFileInfo.Events.Count - 1)
            {
                nextIndex++;

                if (Convert.ToUInt32(Information.Value, 16) == _traceFileInfo.Events[nextIndex].Context)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for previous event of the same object type.
        private void BtnPreviousObject_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex > 0)
            {
                nextIndex--;
                if (Convert.ToUInt32(Information.Value, 16) == _traceFileInfo.Events[nextIndex].Info1)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        /* Handler for button click for next event of the same object type.  */
        private void BtnNextObject_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex < _traceFileInfo.Events.Count - 1)
            {
                nextIndex++;
                if (Convert.ToUInt32(Information.Value, 16) == _traceFileInfo.Events[nextIndex].Info1)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for previous context switch
        private void BtnPreviousSwitch_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex > 1)
            {
                nextIndex--;
                if (_traceFileInfo.Events[nextIndex - 1].Context != _traceFileInfo.Events[nextIndex - 1].NextContext)
                {

                    if (nextIndex > 0)
                    {
                        _currentIndex = nextIndex;
                    }
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for next context switch.
        private void BtnNextSwitch_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex < _traceFileInfo.Events.Count - 1)
            {
                nextIndex++;
                if (_traceFileInfo.Events[nextIndex].Context != _traceFileInfo.Events[nextIndex].NextContext)
                {
                    if (nextIndex < _traceFileInfo.Events.Count - 1)
                    {
                        _currentIndex = nextIndex + 1;
                    }

                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for previous event of the same event ID
        private void BtnPreviousSameID_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex > 0)
            {
                nextIndex--;
                if (Convert.ToUInt32(Information.Value, CultureInfo.InvariantCulture) == _traceFileInfo.Events[nextIndex].Id)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }

        // Handler for button click for next event of the same event ID.
        private void BtnNextSameID_Click(object sender, RoutedEventArgs e)
        {
            int nextIndex = _currentIndex;
            while (nextIndex < _traceFileInfo.Events.Count - 1)
            {
                nextIndex++;
                if (Convert.ToUInt32(Information.Value, CultureInfo.InvariantCulture) == _traceFileInfo.Events[nextIndex].Id)
                {
                    _currentIndex = nextIndex;
                    Refresh(false);
                    break;
                }
            }

            textIndex.Focus();
        }
    }
}
