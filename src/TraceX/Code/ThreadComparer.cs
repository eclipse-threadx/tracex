using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement
{
    public class ThreadSorting
    {
        public const int ByCreationOrder = 1;
        public const int ByAlphabetic = 2;
        public const int ByExecutionTime = 3;
        public const int ByLowestPriority = 4;
        public const int ByHighestPriority = 5;
        public const int ByMostEvents = 6;
        public const int ByLeastEvents = 7;
    }

    public class ThreadCreationOrderComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            if (x.Address == 0xFFFFFFFF)
            {
                if (y.Address == 0xF0F0F0F0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (x.Address == 0xF0F0F0F0)
            {
                if (y.Address == 0xFFFFFFFF)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y.Address == 0xFFFFFFFF)
            {
                if (x.Address == 0xF0F0F0F0)
                {
                    //should never enter here.
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (y.Address == 0xF0F0F0F0)
            {
                if (x.Address == 0xFFFFFFFF)
                {
                    //should never enter here.
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            return (int)(x.Index - y.Index);
        }

        #endregion IComparer<TmlThread> Members
    }

    public class ThreadNameComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            if (x.Address == 0xFFFFFFFF)
            {
                if (y.Address == 0xF0F0F0F0)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (x.Address == 0xF0F0F0F0)
            {
                if (y.Address == 0xFFFFFFFF)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else if (y.Address == 0xFFFFFFFF)
            {
                if (x.Address == 0xF0F0F0F0)
                {
                    //should never enter here.
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else if (y.Address == 0xF0F0F0F0)
            {
                if (x.Address == 0xFFFFFFFF)
                {
                    //should never enter here.
                    return 0;
                }
                else
                {
                    return 1;
                }
            }

            return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
        }

        #endregion IComparer<TmlThread> Members
    }

    public class ThreadExecutionTimeComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            //return (int)(x.Usage - y.Usage);

            if (x.Usage == 0xFFFFFFFF)
            {
                if (y.Usage != 0xFFFFFFFF)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (y.Usage == 0xFFFFFFFF)
            {
                if (x.Usage != 0xFFFFFFFF)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                // Keep Interrupt and initialize/idle on top.
                if (x.Address == 0xFFFFFFFF)
                {
                    if (y.Address == 0xF0F0F0F0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Address == 0xF0F0F0F0)
                {
                    if (y.Address == 0xFFFFFFFF)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.Address == 0xFFFFFFFF)
                {
                    if (x.Address == 0xF0F0F0F0)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (y.Address == 0xF0F0F0F0)
                {
                    if (x.Address == 0xFFFFFFFF)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (x.Usage.CompareTo(y.Usage) == 1)
            {
                return -1;
            }
            else if (x.Usage.CompareTo(y.Usage) == -1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    #endregion IComparer<TmlThread> Members

    public class ThreadLowestPriorityComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            //return (int)(x.lowestPriority - y.lowestPriority);

            if (x.LowestPriority == 0xFFFFFFFF)
            {
                if (y.LowestPriority != 0xFFFFFFFF)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (y.LowestPriority == 0xFFFFFFFF)
            {
                if (x.LowestPriority != 0xFFFFFFFF)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (x.Address == 0xFFFFFFFF)
                {
                    if (y.Address == 0xF0F0F0F0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Address == 0xF0F0F0F0)
                {
                    if (y.Address == 0xFFFFFFFF)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.Address == 0xFFFFFFFF)
                {
                    if (x.Address == 0xF0F0F0F0)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (y.Address == 0xF0F0F0F0)
                {
                    if (x.Address == 0xFFFFFFFF)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (x.LowestPriority.CompareTo(y.LowestPriority) == 1)
            {
                return -1;
            }
            else if (x.LowestPriority.CompareTo(y.LowestPriority) == -1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion IComparer<TmlThread> Members
    }

    public class ThreadHighestPriorityComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            //return (int)(x.highestPriority - y.highestPriority);

            // Start by taking care of unknown priorities.
            if (x.HighestPriority == 0xFFFFFFFF)
            {
                if (y.HighestPriority != 0xFFFFFFFF)
                {
                    return 1;
                    //return -1;
                }
                else
                {
                    return 0;
                }
            }
            else if (y.HighestPriority == 0xFFFFFFFF)
            {
                if (x.HighestPriority != 0xFFFFFFFF)
                {
                    return -1;
                    //return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                // Keep Interrupt and initialize/idle on top.
                if (x.Address == 0xFFFFFFFF)
                {
                    if (y.Address == 0xF0F0F0F0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Address == 0xF0F0F0F0)
                {
                    if (y.Address == 0xFFFFFFFF)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.Address == 0xFFFFFFFF)
                {
                    if (x.Address == 0xF0F0F0F0)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (y.Address == 0xF0F0F0F0)
                {
                    if (x.Address == 0xFFFFFFFF)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (x.HighestPriority.CompareTo(y.HighestPriority) == 1)
            {
                return 1;
            }
            else if (x.HighestPriority.CompareTo(y.HighestPriority) == -1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        #endregion IComparer<TmlThread> Members
    }

    public class ThreadHighestNumberOfEventsComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            //return (int)(x.lowestPriority - y.lowestPriority);

            if (x.LowestPriority == 0xFFFFFFFF)
            {
                if (y.LowestPriority != 0xFFFFFFFF)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (y.LowestPriority == 0xFFFFFFFF)
            {
                if (x.LowestPriority != 0xFFFFFFFF)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (x.Address == 0xFFFFFFFF)
                {
                    if (y.Address == 0xF0F0F0F0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Address == 0xF0F0F0F0)
                {
                    if (y.Address == 0xFFFFFFFF)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.Address == 0xFFFFFFFF)
                {
                    if (x.Address == 0xF0F0F0F0)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (y.Address == 0xF0F0F0F0)
                {
                    if (x.Address == 0xFFFFFFFF)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (x.LowestPriority.CompareTo(y.LowestPriority) == 1)
            {
                return -1;
            }
            else if (x.LowestPriority.CompareTo(y.LowestPriority) == -1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        #endregion IComparer<TmlThread> Members
    }

    public class ThreadLowestNumberOfEventsComparer : IComparer<TmlThread>
    {
        #region IComparer<TmlThread> Members

        public int Compare(TmlThread x, TmlThread y)
        {
            //return (int)(x.highestPriority - y.highestPriority);

            // Start by taking care of unknown priorities.
            if (x.HighestPriority == 0xFFFFFFFF)
            {
                if (y.HighestPriority != 0xFFFFFFFF)
                {
                    return 1;
                    //return -1;
                }
                else
                {
                    return 0;
                }
            }
            else if (y.HighestPriority == 0xFFFFFFFF)
            {
                if (x.HighestPriority != 0xFFFFFFFF)
                {
                    return -1;
                    //return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                // Keep Interrupt and initialize/idle on top.
                if (x.Address == 0xFFFFFFFF)
                {
                    if (y.Address == 0xF0F0F0F0)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Address == 0xF0F0F0F0)
                {
                    if (y.Address == 0xFFFFFFFF)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (y.Address == 0xFFFFFFFF)
                {
                    if (x.Address == 0xF0F0F0F0)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (y.Address == 0xF0F0F0F0)
                {
                    if (x.Address == 0xFFFFFFFF)
                    {
                        //should never enter here.
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (x.HighestPriority.CompareTo(y.HighestPriority) == 1)
            {
                return 1;
            }
            else if (x.HighestPriority.CompareTo(y.HighestPriority) == -1)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }

        #endregion IComparer<TmlThread> Members
    }
}
