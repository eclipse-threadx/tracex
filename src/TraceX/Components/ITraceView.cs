using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace AzureRTOS.TraceManagement.Components
{
    public interface ITraceView
    {
        void Zoom(double multiple);
        int GetNextPageStartIndex(int currentIndex);
        int GetPreviousPageStartIndex(int currentIndex);
        void Initialize(Code.TraceFileInfo tfi, Navigator navigator);
        void MoveToEvent(int eventIndex, bool centerMarker);

        void RemoveDetailsWindow(Components.DetailsWindow detailsWindow, Canvas canvasEvents);

    }
}
