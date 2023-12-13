using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AzureRTOS.TraceManagement.Code
{
    public class menuInformation
    {
        string caption;
        string description;
        string value;
        int index;

        public menuInformation()
        {
        }
        public menuInformation(string caption, string value)
        {
        }
        public menuInformation(string caption, string value, string description, int index)
        {
            this.caption = caption;
            this.value = value;
            this.description = description;
            this.index = index;

        }

        public String Caption
        {
            get { return this.caption; }
            set { this.caption = value; }

        }
        public String Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public int Index
        {
            get { return this.index; }
            set { this.index = value; }
        }





    }
}
