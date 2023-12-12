using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for ZoomToolBar.xaml
    /// </summary>
    public partial class ZoomToolbar : UserControl
    {
        public EventHandler<Code.ZoomEventArgs> ZoomChanged;

        private double multiple = 1.0;
        private const double min = 1.0;
        private const double max = 16.0;
        private bool sequentialView = true;
        public static bool inputTextSet = false;
        private bool zoomInLockOutSet = false;
        private bool zoomOutLockOutSet = false;
        private double maxZoom4View = 1;
        public static ulong currentZoomG = 0;
        public static ulong pastZoomG = 0;

        public ZoomToolbar()
        {
            InitializeComponent();
            txtZoomFactor.Text = "0";

            btnZoomOut.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnZoomOut_IsEnabledChanged);
            btnZoomIn.IsEnabledChanged += new DependencyPropertyChangedEventHandler(btnZoomIn_IsEnabledChanged);
        }

        private void btnZoomOut_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((!btnZoomOut.IsEnabled) || (!btnZoomIn.IsEnabled))
            {
                // currentZoomG = Convert.ToInt32(txtZoomFactor.Text);
            }
        }

        private void btnZoomIn_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((!btnZoomOut.IsEnabled) || (!btnZoomIn.IsEnabled))
            {
                //currentZoomG = Convert.ToInt32(txtZoomFactor.Text);
            }
        }

        private void btnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonState(true, true);

            if (btnZoomIn.IsEnabled)
            {
                multiple *= 2;
            }
            pastZoomG = (ulong)(multiple * 100);

            if (multiple >= maxZoom4View)
            {
                //multiple = maxZoom4View;
                //UpdateButtonState(false, true);
            }

            /* Yuxin, Code underneath fixes the problem. Also look at btnZoomOut_Click function  */
            ulong integralPercentage = (ulong)(multiple * 100);
            //int integralPercentage = (int)(multiple * 100);
            multiple = (double)integralPercentage / 100;
            txtZoomFactor.Text = integralPercentage.ToString(CultureInfo.CurrentCulture);
            Code.ZoomEventArgs ze = new Code.ZoomEventArgs(this.multiple);

            if (null != ZoomChanged)
            {
                ZoomChanged(this, ze);
            }

            if (!btnZoomIn.IsEnabled)
            {
                zoomInLockOutSet = true;
                txtZoomFactor.Text = pastZoomG.ToString(CultureInfo.CurrentCulture);
                multiple = ((double)pastZoomG / (double)100);
            }
            else
            {
                zoomInLockOutSet = false;
            }
        }

        private void btnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            UpdateButtonState(true, true);

            multiple /= 2;

            if (multiple < min)
            {
                multiple = min;
                //UpdateButtonState(true, false);
            }

            /* Yuxin, Code underneath fixes the problem. Also look at btnZoomIn_Click function  */
            ulong integralPercentage = (ulong)(multiple * 100);
            //int integralPercentage = (int)(multiple * 100);
            multiple = (double)integralPercentage / 100;
            txtZoomFactor.Text = integralPercentage.ToString(CultureInfo.CurrentCulture);
            Code.ZoomEventArgs ze = new Code.ZoomEventArgs(this.multiple);
            if (null != ZoomChanged)
            {
                ZoomChanged(this, ze);
            }

            if (!btnZoomOut.IsEnabled)
            {
                zoomOutLockOutSet = true;
            }
            else
            {
                zoomOutLockOutSet = false;
            }
        }

        public void UpdateButtonState(bool enableZoomIn, bool enbleZoomOut)
        {
            if (enableZoomIn)
            {
                btnZoomIn.Visibility = Visibility.Visible;
                btnZoomInDisabled.Visibility = Visibility.Hidden;
            }
            else
            {
                btnZoomIn.Visibility = Visibility.Hidden;
                btnZoomInDisabled.Visibility = Visibility.Visible;
            }

            if (enbleZoomOut)
            {
                btnZoomOut.Visibility = Visibility.Visible;
                btnZoomOutDisabled.Visibility = Visibility.Hidden;
            }
            else
            {
                btnZoomOut.Visibility = Visibility.Hidden;
                btnZoomOutDisabled.Visibility = Visibility.Visible;
            }

            btnZoomIn.IsEnabled = enableZoomIn;
            btnZoomOut.IsEnabled = enbleZoomOut;
        }

        public void InitializeZoomFactors(bool sequentialView, double currentFactor, bool enableZoomIn, bool enableZoomOut)
        {
            this.sequentialView = sequentialView;
            multiple = currentFactor;
            txtZoomFactor.Text = string.Format(CultureInfo.CurrentCulture, "{0}", currentFactor * 100);
            //maxZoom4View = sequentialView == true ? 1.0 : 16.0;
            UpdateButtonState(enableZoomIn, enableZoomOut);

            if (!btnZoomOut.IsEnabled)
            {
                zoomOutLockOutSet = true;
            }
            else
            {
                zoomOutLockOutSet = false;
            }

            if (!btnZoomIn.IsEnabled)
            {
                zoomInLockOutSet = true;
            }
            else
            {
                zoomInLockOutSet = false;
            }

            if (Convert.ToUInt64(Convert.ToDecimal(txtZoomFactor.Text, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture) >= 0x7FFFFFFF)
            {
                currentZoomG = 0x7FFFFFFF;
            }
            else
            {
                currentZoomG = Convert.ToUInt64((ulong)Convert.ToDecimal(txtZoomFactor.Text, CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
            }
            pastZoomG = currentZoomG;
        }

        private void txtZoomFactor_PreviewKeyDown(object sender, KeyEventArgs e)
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

        private void txtZoomFactor_KeyDown(object sender, KeyEventArgs e)
        {
            Code.ZoomEventArgs ze;

            if (Key.Enter == e.Key)
            {
                inputTextSet = true;

                double newFactor = 100;

                Double.TryParse(txtZoomFactor.Text, out newFactor);
                if (newFactor < min * 100 || newFactor > maxZoom4View * 100)
                {
                }
                else if ((newFactor == min * 100) || (newFactor == maxZoom4View * 100))
                {
                }
                else
                {
                    UpdateButtonState(true, true);
                }

                multiple = newFactor / 100;
                ze = new Code.ZoomEventArgs(this.multiple);
                if (null != ZoomChanged)
                {
                    ZoomChanged(this, ze);
                }

                if ((!btnZoomIn.IsEnabled) && (inputTextSet))
                {
                    if (!zoomInLockOutSet)
                    {
                        UpdateButtonState(true, btnZoomOut.IsEnabled);
                    }
                    else
                    {
                        //currentZoomG = currentZoomG * 2;
                    }

                    txtZoomFactor.Text = currentZoomG.ToString(CultureInfo.CurrentCulture);
                    multiple = (double)currentZoomG / (double)100;

                    ze = new Code.ZoomEventArgs(this.multiple);
                    if (null != ZoomChanged)
                    {
                        ZoomChanged(this, ze);
                    }
                }
                else if ((!btnZoomOut.IsEnabled) && (inputTextSet))
                {
                    //txtZoomFactor.Text = currentZoomG.ToString(CultureInfo.CurrentCulture);

                    if (!zoomOutLockOutSet)
                    {
                        UpdateButtonState(btnZoomIn.IsEnabled, true);
                    }
                    else
                    {
                        //currentZoomG = currentZoomG / 2;
                    }

                    txtZoomFactor.Text = currentZoomG.ToString(CultureInfo.CurrentCulture);
                    multiple = (double)currentZoomG / (double)100;

                    ze = new Code.ZoomEventArgs(this.multiple);
                    if (null != ZoomChanged)
                    {
                        ZoomChanged(this, ze);
                    }
                }

                inputTextSet = false;
            }
        }
    }
}
