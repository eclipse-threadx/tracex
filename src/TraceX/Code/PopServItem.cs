using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AzureRTOS.TraceManagement.Code
{
    public class PopServItem
    {
        string name;
        string stringcount;

        public string Name
        {
            get
            {
                return name;
            }
            set { name = value; }
        }

        long count;

        public long Count
        {
            get { return count; }
            set { count = value; }
        }

        public string stringCount
        {
            get
            {
                return stringcount;
            }
            set { stringcount = value; }
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

        public override string ToString()
        {
            return $"Thread Id: {Name},  Percent Usage: {percent}";
        }
    }
}
