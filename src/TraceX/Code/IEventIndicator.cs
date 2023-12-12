using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpressLogic.TraceManagement.Code
{
    public interface  IEventIndicator
    {
        void MoveToEvent(int eventIndex, bool userEntry);
    }
}
