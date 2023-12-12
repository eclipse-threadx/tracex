using System;
using System.Collections.Generic;
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
using System.Windows.Automation.Peers;

namespace AzureRTOS.TraceManagement.Components
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    
    public class DetailWindowAutomationPeer: FrameworkElementAutomationPeer
    {
        public DetailWindowAutomationPeer(DetailsWindow owner) :
            base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Window;
        }
    }

    public partial class DetailsWindow : ContentControl
    {
        double beginX;
        double beginY;
        bool isMouseDown = false;

        Code.Event relatedEvent;
        ITraceView manager;
        Canvas canvasEvents;

        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new DetailWindowAutomationPeer(this);
        }

        public DetailsWindow(ITraceView manager, Canvas canvasEvents)
        {
            InitializeComponent();
            this.manager = manager;
            this.canvasEvents = canvasEvents;
        }

        public void Initialize(Code.Event relatedEvent, Code.TraceFileInfo tfi)
        {
            this.relatedEvent = relatedEvent;
            txtDetails.Text = this.relatedEvent.GetEventDetailString(tfi);
            titleText.Text = "Event Details";
        }

        public void ShiftPosition(int delta_x, int delta_y)
        {
            Canvas.SetLeft(this, Canvas.GetLeft(this) + delta_x);
            Canvas.SetTop(this, Canvas.GetTop(this) + delta_y);

            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.X2 = Canvas.GetLeft(this) + (this.Width / 2);
                    line.Y2 = Canvas.GetTop(this);
                }
            }
        }

        private void titleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(null);
            // Set the beginning position of the mouse.
            this.beginX = point.X;
            this.beginY = point.Y;
            this.isMouseDown = true;
            // Ensure this object is the only one receiving mouse events.
            UIElement elem = (UIElement)sender;

            elem.CaptureMouse();
        }

        private void titleBar_MouseMove(object sender, MouseEventArgs e)
        {
            // Determine whether the mouse button is down.
            // If so, move the object.
            if (this.isMouseDown == true)
            {
                // Retrieve the current position of the mouse.
                Point point = e.GetPosition(null);
                Point pointNew = new Point(
                    Canvas.GetLeft(this) + point.X - this.beginX,
                    Canvas.GetTop(this) + point.Y - this.beginY
                    );

                // Update the beginning position of the mouse.
                this.beginX = point.X;
                this.beginY = point.Y;

                this.SetPosition(pointNew);
            }
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
                        line.X2 = pointNew.X + (this.Width / 2);
                        line.Y2 = pointNew.Y ;
                    }
                }
            }

        

        public void MoveX(double delta, double scaling, double baseValue, double eventWidth)
        {
            double newX = Canvas.GetLeft(this) + delta;
            Canvas.SetLeft(this, newX);
            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.X1 = (relatedEvent.RelativeTicks * scaling - baseValue) + scaling * eventWidth / 2;
                    line.X2 += delta;
                }
            }
        }

        public void MoveX(double delta, double baseValue, double eventWidth)
        {
            double newX = Canvas.GetLeft(this) + delta;
            Canvas.SetLeft(this, newX);
            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.X1 = (relatedEvent.Index * eventWidth - baseValue) + eventWidth / 2;
                    line.X2 += delta;
                }
            }
        }

        public void UpdateY(double newY)
        {
            if (null != this.Tag)
            {
                if (this.Tag.GetType() == typeof(Line))
                {
                    Line line = (Line)this.Tag;
                    line.Y1 = newY;
                }
            }
        }

        private void titleBar_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

            this.isMouseDown = false;

            // Allow all objects to receive mouse events.
            // Ensure this object is the only one receiving mouse events.
            UIElement elem = (UIElement)sender;

            elem.ReleaseMouseCapture();
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            Shape cross = (Shape)sender;
            cross.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            cross.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        }

        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            Shape cross = (Shape)sender;
            cross.Stroke = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            cross.Fill = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        }

        private void btnClose_ClickHandler(object sender, RoutedEventArgs e)
        {
            this.manager.RemoveDetailsWindow(this, canvasEvents);
        }
        private void btnClose_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.manager.RemoveDetailsWindow(this, canvasEvents);
        }

        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

      
        public uint RelatedEventIndex
        {
            get { return this.relatedEvent.Index; }
        }

        private void txtDetails_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (IsVisible)
            {
                if (FrameworkElementAutomationPeer.ListenerExists(AutomationEvents.LiveRegionChanged))
                {
                    var peer = FrameworkElementAutomationPeer.FromElement(txtDetails);

                    if (peer == null)
                    {
                        peer = FrameworkElementAutomationPeer.CreatePeerForElement(txtDetails);
                    }

                    if (peer != null)
                    {
                        peer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                    }
                }
            }
        }
    }
}
