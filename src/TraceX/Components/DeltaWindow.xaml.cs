using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DeltaWindow : Window
    {
        TmlEvent beginEvent;
        TmlEvent endEvent;

        public DeltaWindow(TmlEvent bEvent, TmlEvent eEvent)
        {
            InitializeComponent();
            beginEvent = bEvent;
            endEvent = eEvent;
            txtTickDiff.Text = "Difference in Relative Ticks: " + (endEvent.RelativeTicks - beginEvent.RelativeTicks).ToString(CultureInfo.CurrentCulture);
            txtTimeDiff.Text = "Difference in Timestamps: " + (endEvent.TimeStamp - beginEvent.TimeStamp).ToString(CultureInfo.CurrentCulture);
        }

        private void SetPosition(Point pointNew)
        {
            Canvas.SetLeft(this, pointNew.X);
            Canvas.SetTop(this, pointNew.Y);
            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.X2 = pointNew.X + 100;
                    line.Y2 = pointNew.Y;
                }
            }
        }

        public void MoveX(double delta)
        {
            double newX = Canvas.GetLeft(this) + delta;
            Canvas.SetLeft(this, newX);
            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.X1 += delta;
                    line.X2 += delta;
                }
            }
        }
    }
}
