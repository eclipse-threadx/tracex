using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureRTOS.TraceManagement.Code
{
    public class Information
    {
        string caption;
        string description;
        string value;

        public Information(string caption, string value)
            : this(caption, value, string.Empty)
        {
        }
        public Information(string caption, string value, string description)
        {
            this.caption = caption;
            this.value = value;
            this.description = description;
        }

        public String Caption
        {
            get { return caption; }
        }
        public String Description
        {
            get { return this.description; }
        }
        public String Value
        {
            get { return this.value; }
        }

    }
}
