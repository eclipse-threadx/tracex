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

namespace AzureRTOS.TraceManagement.Code
{
    public class Indicator
    {
        Canvas headCanvas;
        Canvas tailCanvas;

        Indicator() { }

        Canvas headPart;
        Canvas tailPart;

        private Rectangle rectTail;

        public static Indicator Create(Canvas headCanvas,Canvas tailCanvas)
        {
            Indicator obj = new Indicator();
            obj.headCanvas = headCanvas;
            obj.tailCanvas = tailCanvas;
            obj.Init();
            return obj;
        }
        public void Show(double left)
        {            
            Canvas.SetLeft(this.headPart, left);
            Canvas.SetLeft(this.tailPart, left);
            if (null != rectTail) rectTail.Height = this.tailCanvas.ActualHeight+3000;
                
        }

        public void AddToCanvas()
        {
            this.headCanvas.Children.Add(this.headPart);
            this.tailCanvas.Children.Add(this.tailPart);
        }


        private void Init()
        {
            this.headPart = new Canvas();

            Rectangle rectHead = new Rectangle();
            this.headPart.Children.Add(rectHead);
            Canvas.SetLeft(rectHead, 0);
            rectHead.Width = 2;
            rectHead.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            rectHead.Fill.Opacity = 0.5;
            rectHead.Height = 100;


            Polygon triangle = new Polygon();
            triangle.Points = new PointCollection(3);
            triangle.Points.Add(new Point(-3, 0));
            triangle.Points.Add(new Point(5, 0));
            triangle.Points.Add(new Point(1, 4));
            triangle.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
            this.headPart.Children.Add(triangle);

           
            Canvas.SetZIndex(this.headPart, 10000);
            Canvas.SetLeft(this.headPart, -100);


            this.tailPart = new Canvas();
            this.rectTail = new Rectangle();
            this.tailPart.Children.Add(rectTail);
            Canvas.SetLeft(rectTail, 0);
            rectTail.Width = 2;
            rectTail.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            rectTail.Fill.Opacity = 0.5;
            rectTail.Height = 3000; //tailCanvas.Height;

           
            Canvas.SetZIndex(this.tailPart, 1);
            Canvas.SetLeft(this.tailPart, -100);          
        }
    }

    
}
