using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement
{
    public class EventComparer : IComparer<TmlEvent>
    {
        #region IComparer<TmlEvent> Members

        public int Compare(TmlEvent x, TmlEvent y)
        {
            return (int)(x.RelativeTicks - y.RelativeTicks);
        }

        #endregion IComparer<TmlEvent> Members
    }
}
