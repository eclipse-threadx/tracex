using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureRTOS.TraceManagement.Code
{
    class ThreadStackUsageItemTypeListViewItem
    {
        public ThreadStackUsageItem Content { get; set; }
        public override string ToString()
        {
            //Make the content name as the accessible name
            return ("Thread Name: " + Content.Name + ". Stack Size: " + Content.StackSize + ". Availability: " + Content.Availability + ". Usage Graph: " + Content.Percent.ToString("P2") + ". Event Id: " + Content.eventID);
        }
    }
}
