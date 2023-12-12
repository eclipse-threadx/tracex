using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureRTOS.TraceManagement.Code
{
    public class IndexEventArgs:System.EventArgs
    {
        bool centerMarker = false;
        int index;
        public IndexEventArgs(int index)
        {
            this.index = index;
        }

        public IndexEventArgs(int index, bool centerMarker)
        {
            this.index = index;
            this.centerMarker = centerMarker;
        }

        public int Index
        {
            get { return this.index; }
        }

        public bool CenterMarker
        {
            get { return this.centerMarker; }
        }

    }

    public enum Direction
    {
        Up = 0,
        Down,
        Left,
        Right
    }

    public class PageEventArgs : System.EventArgs
    {
        int index = 0 ;
        Direction pageDirection;
        public PageEventArgs(Direction pageDirection)
        {
            this.pageDirection = pageDirection;
        }
        public Direction PageDirection
        {
            get { return this.pageDirection; }
        }
        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }
    }

    public class contextEventArgs : System.Windows.RoutedEventArgs
    {

        string name;
        int address;
        
        public contextEventArgs(string name, int address)
        {
            this.name = name;
            this.address = address;            
        }

        public string Name
        {
            get { return name; }
        }

        public int Address
        {
            get { return address; }
        }
    }

    // ZoomEventArgs
    public class ZoomEventArgs : EventArgs
    {
        bool enableZoomOut = false;
        bool enableZoomIn = false;
        double multiple = 1.0;

        public ZoomEventArgs(bool enableZoomOut, bool enableZoomIn)
        {
            this.enableZoomOut = enableZoomOut;
            this.enableZoomIn = enableZoomIn;
        }

        public ZoomEventArgs(double multiple)
        {
            this.multiple = multiple;
        }
        
        public double Multiple
        {
            get { return multiple; }
        }

        public bool EnableZoomOut
        {
            get { return enableZoomOut; }

        }

        public bool EnableZoomIn
        {
            get { return enableZoomIn; }
        }
    }

    public class DeltaEventArgs : EventArgs
    {
        long delta;
        double relativeTime;
        long unitsIndex;

        public DeltaEventArgs(long delta, long unitsIndex, double relativeTime)
        {
            this.delta = delta;
            this.unitsIndex = unitsIndex;
            this.relativeTime = relativeTime;
        }

        public long DeltaTicks
        {
            get { return delta; }            
        }

        public double RelativeTime
        {
            get { return relativeTime; }
        }

        public long UnitsIndex
        {
            get {
                //long deltaScale = 0;
                //Int64.TryParse(scale, out deltaScale);
                return unitsIndex; 
            }
        }


    }
}
