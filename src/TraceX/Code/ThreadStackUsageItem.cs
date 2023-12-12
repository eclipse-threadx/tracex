using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AzureRTOS.TraceManagement.Code
{
    public class ThreadStackUsageItem
    {
        string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        uint stacksize;
        public uint StackSize
        {
            get {return stacksize;}
            set { stacksize = value; }
        }

        uint minAvailability;

        public uint Availability
        {
            get {return minAvailability;}
            set { minAvailability = value; }
        }

        double percent;

        public double Percent
        {
            get { return percent; }
            set { percent = value; }
        }
        public double Percent100
        {
            get { return percent * 100; }
        }
        public string PercentString
        {
            get
            {
                return (percent).ToString("0.##%", CultureInfo.InvariantCulture);
            }
        }
        uint EventID;

        public uint eventID
        {
            get { return EventID; }
            set { EventID = value; }
        }

        public string eventIDString
        {
            get
            {
                if (EventID != 0)
                    return EventID.ToString(CultureInfo.InvariantCulture);

                return "None";
            }
        }
    }
}
