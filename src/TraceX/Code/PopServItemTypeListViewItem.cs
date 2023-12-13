using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureRTOS.TraceManagement.Code
{
    class PopServItemTypeListViewItem
    {
        public PopServItem Content { get; set; }
        public override string ToString()
        {
            //Make the content name as the accessible name
            return ("Context: " + Content.Name + "Usage Graph: " + Content.Percent.ToString("P2"));
        }
    }
}
