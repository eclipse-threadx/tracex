using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using AzureRTOS.Tml;

namespace AzureRTOS.TraceManagement.Dialogs
{
    /// <summary>
    /// Interaction logic for FindByValue.xaml
    /// </summary>
    public partial class FindByValue : Window
    {
        /* Global variables.  */
        int currentIndex = 0;
        int nextIndex = 0;
        int minIndex = 0;
        int maxIndex = 0;
        uint contextIndex = 0;
        string contextName = string.Empty;
        public static int FindByValueIndex = 0;
        Code.TraceFileInfo tfi;

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public static EventHandler<Code.IndexEventArgs> IndexChanged;
        public static EventHandler<Code.PageEventArgs> PageChanged;

        public FindByValue(Code.TraceFileInfo tfi, List<TmlThread> threads)
        {
            this.tfi = tfi;
            InitializeComponent();
            SetTraceEventInfo(tfi);
            FindByValueIndex = currentIndex;

            this.searchResults.Text = String.Empty;

            this.dictThread = new Dictionary<string, uint>();

            int NameAddition = 0;
            int i = 0;

            contextInput.Items.Clear();

            foreach (TmlThread tt in threads)
            {
                if (i == 0)
                {
                    contextInput.Items.Add("Any Context");
                    contextInput.Items.Add("Interrupt");
                    contextInput.Items.Add("Idle");
                    contextInput.Items.Add("Initialize");
                    i++;
                }

                string threadName = tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")";

                if (dictThread.ContainsKey(threadName))
                {
                    threadName = threadName + NameAddition.ToString(CultureInfo.InvariantCulture);
                    NameAddition++;
                }

                dictThread.Add(threadName, tt.Address);
            }

            var sortedDict = (from entry in dictThread orderby entry.Key ascending select entry);

            i = 0;
            //foreach (string key in dictThread.Keys)
            foreach (KeyValuePair<string, uint> key in sortedDict)
            {
                if (i == 0)
                {
                    contextName = key.Key;
                    contextIndex = dictThread[key.Key];
                }
                i++;
                contextInput.Items.Add(key.Key);
            }
            contextInput.SelectedIndex = 0;

            dictThread.Add("Any Context", 0);
            dictThread.Add("Interrupt", 0xFFFFFFFF);
            dictThread.Add("Idle", 0x00000000);
            dictThread.Add("Initialize", 0xF0F0F0F0);
        }

        Dictionary<string, uint> dictThread;

        public static Window Show(Code.TraceFileInfo tfi, int index, List<TmlThread> threads)
        {
            FindByValue fbvDlg = new FindByValue(tfi, threads);
            fbvDlg.Show();
            FindByValueIndex = index;
            return fbvDlg;
        }

        public void SetTraceEventInfo(Code.TraceFileInfo tfi)
        {
            this.tfi = tfi;
            this.maxIndex = this.tfi.Events.Count - 1;
            this.minIndex = 0;
            this.currentIndex = this.minIndex;
            //this.textIndex.Text = string.Empty;
            this.contextInput.Text = string.Empty;
            this.eventIDInput.Text = string.Empty;
            this.info1Input.Text = string.Empty;
            this.info2Input.Text = string.Empty;
            this.info3Input.Text = string.Empty;
            this.info4Input.Text = string.Empty;
            this.matchAllFields.IsChecked = false;
            this.matchAnyField.IsChecked = true;
            updateButtonState();
        }

        /* Function to refresh the text in the box and the current index.  */
        private void refresh(bool userNumber)
        {
            FindByValueIndex = this.currentIndex;
            Code.IndexEventArgs e = new Code.IndexEventArgs(this.currentIndex, userNumber);
            if (null != IndexChanged) IndexChanged(this, e);
            updateButtonState();
        }

        private void matchAllFields_Unchecked(object sender, RoutedEventArgs e)
        {
            matchAllFields.IsChecked = false;
            matchAnyField.IsChecked = true;
        }

        private void matchAllFields_Checked(object sender, RoutedEventArgs e)
        {
            matchAllFields.IsChecked = true;
            matchAnyField.IsChecked = false;
        }

        private void matchAnyField_Unchecked(object sender, RoutedEventArgs e)
        {
            matchAllFields.IsChecked = true;
            matchAnyField.IsChecked = false;
        }

        private void matchAnyField_Checked(object sender, RoutedEventArgs e)
        {
            matchAllFields.IsChecked = false;
            matchAnyField.IsChecked = true;
        }

        /* Dead. */
        private void updateButtonState()
        {
        }


        private void contextInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string contextIndexSelectedItem = contextInput.SelectedItem.ToString();

            foreach (string key in dictThread.Keys)
            {
                if (key == contextIndexSelectedItem)
                {
                    contextName = key;
                    contextIndex = dictThread[key];

                    //debug
                    //MessageBox.Show(contextName.ToString(CultureInfo.CurrentCulture) + contextIndex.ToString(CultureInfo.CurrentCulture));
                }
            }
        }

        /* To quickly move to an index.  */
        public void MoveTo(int index, bool userNumber)
        {
            this.currentIndex = index;
            this.refresh(userNumber);
        }

        public void updateIndexNext()
        {
            this.currentIndex = FindByValueIndex;
            if (nextIndex < this.maxIndex)
            {
                nextIndex = FindByValueIndex + 1;
            }
        }

        public void updateIndexPrevious()
        {
            this.currentIndex = FindByValueIndex;

            if (nextIndex >= this.minIndex)
            {
                nextIndex = FindByValueIndex - 1;
                if (nextIndex < 0)
                {
                    nextIndex = 0;
                }
            }
        }

        /* To move to a previous index.  */
        public void MovePrevious()
        {
            string tempEventIDText = eventIDInput.Text;
            string tempinfo1Text = info1Input.Text;
            string tempinfo2Text = info2Input.Text;
            string tempinfo3Text = info3Input.Text;
            string tempinfo4Text = info4Input.Text;

            bool eventIDInputIsNumber = true;
            bool info1InputIsNumber = true;
            bool info2InputIsNumber = true;
            bool info3InputIsNumber = true;
            bool info4InputIsNumber = true;

            bool eventIDInputIsMatch = true;
            bool info1InputIsMatch = true;
            bool info2InputIsMatch = true;
            bool info3InputIsMatch = true;
            bool info4InputIsMatch = true;

            if (this.currentIndex != FindByValueIndex)
            {
                updateIndexPrevious();
            }

            if (this.currentIndex >= this.minIndex)
            {
                int tempIndex = this.currentIndex;
                if (nextIndex >= this.currentIndex)
                {
                    nextIndex = this.currentIndex - 1;
                    if (nextIndex < 0)
                    {
                        nextIndex = 0;
                    }
                }
                this.currentIndex = nextIndex;
                bool found = false;

                while (!found)
                {
                    if ((bool)this.matchAllFields.IsChecked)
                    {
                        if ((this.currentIndex < this.maxIndex) && (this.currentIndex >= this.minIndex))
                        {
                            if ((this.eventIDInput.Text == "") || (this.info1Input.Text == "") || (this.info2Input.Text == "")
                                || (this.info3Input.Text == "") || (this.info4Input.Text == ""))
                            {
                                if (this.eventIDInput.Text == "")
                                {
                                    eventIDInputIsNumber = false;
                                    eventIDInputIsMatch = true;
                                }
                                if (this.info1Input.Text == "")
                                {
                                    info1InputIsNumber = false;
                                    info2InputIsMatch = true;
                                }
                                if (this.info2Input.Text == "")
                                {
                                    info2InputIsNumber = false;
                                    info2InputIsMatch = true;
                                }
                                if (this.info3Input.Text == "")
                                {
                                    info3InputIsNumber = false;
                                    info3InputIsMatch = true;
                                }
                                if (this.info4Input.Text == "")
                                {
                                    info4InputIsNumber = false;
                                    info4InputIsMatch = true;
                                }
                            }

                            if (eventIDInputIsNumber)
                            {
                                if (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))
                                {
                                    eventIDInputIsMatch = true;
                                }
                                else
                                {
                                    eventIDInputIsMatch = false;
                                }
                            }

                            if (info1InputIsNumber)
                            {
                                if ((this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                                {
                                    info1InputIsMatch = true;
                                }
                                else
                                {
                                    info1InputIsMatch = false;
                                }
                            }

                            if (info2InputIsNumber)
                            {
                                if ((this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                                {
                                    info2InputIsMatch = true;
                                }
                                else
                                {
                                    info2InputIsMatch = false;
                                }
                            }

                            if (info3InputIsNumber)
                            {
                                if ((this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                                {
                                    info3InputIsMatch = true;
                                }
                                else
                                {
                                    info3InputIsMatch = false;
                                }
                            }

                            if (info4InputIsNumber)
                            {
                                if ((this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                                {
                                    info4InputIsMatch = true;
                                }
                                else
                                {
                                    info4InputIsMatch = false;
                                }
                            }

                            if (contextInput.Text == "Any Context")
                            {
                                if ((eventIDInputIsMatch) && (info1InputIsMatch) && (info2InputIsMatch) && (info3InputIsMatch) && (info4InputIsMatch))
                                {
                                    found = true;
                                    nextIndex--;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex > this.minIndex)
                                    {
                                        this.currentIndex--;
                                        nextIndex--;
                                    }
                                    else
                                    {
                                        //we have gone through and found nothing.
                                        //reset information.
                                        this.currentIndex = tempIndex;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if ((this.tfi.Events[this.currentIndex].Context == contextIndex) && (eventIDInputIsMatch) && (info1InputIsMatch) && (info2InputIsMatch) && (info3InputIsMatch) && (info4InputIsMatch))
                                {
                                    found = true;
                                    nextIndex--;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex > this.minIndex)
                                    {
                                        this.currentIndex--;
                                        nextIndex--;
                                    }
                                    else
                                    {
                                        //we have gone through and found nothing.
                                        //reset information.
                                        this.currentIndex = tempIndex;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Nothing found return index to original.
                            this.currentIndex = tempIndex;
                            break;
                        }
                    }
                    else if ((bool)matchAnyField.IsChecked)
                    {
                        if ((this.currentIndex < this.maxIndex) && (this.currentIndex >= this.minIndex))
                        {
                            if ((this.eventIDInput.Text == "") || (this.info1Input.Text == "") || (this.info2Input.Text == "")
                                || (this.info3Input.Text == "") || (this.info4Input.Text == ""))
                            {
                                if (this.eventIDInput.Text == "")
                                {
                                    eventIDInputIsNumber = false;
                                }
                                if (this.info1Input.Text == "")
                                {
                                    info1InputIsNumber = false;
                                }
                                if (this.info2Input.Text == "")
                                {
                                    info2InputIsNumber = false;
                                }
                                if (this.info3Input.Text == "")
                                {
                                    info3InputIsNumber = false;
                                }
                                if (this.info4Input.Text == "")
                                {
                                    info4InputIsNumber = false;
                                }
                            }

                            if (contextInput.Text == "Any Context")
                            {
                                if (((eventIDInputIsNumber) && (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info1InputIsNumber) && (this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info2InputIsNumber) && (this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info3InputIsNumber) && (this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info4InputIsNumber) && (this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))))
                                {
                                    found = true;
                                    nextIndex--;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex > this.minIndex)
                                    {
                                        this.currentIndex--;
                                        nextIndex--;
                                    }
                                    else
                                    {
                                        //we have gone through and found nothing.
                                        //reset information.
                                        this.currentIndex = tempIndex;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if ((this.tfi.Events[this.currentIndex].Context == contextIndex) ||
                                     ((eventIDInputIsNumber) && (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info1InputIsNumber) && (this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info2InputIsNumber) && (this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info3InputIsNumber) && (this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info4InputIsNumber) && (this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))))
                                {
                                    found = true;
                                    nextIndex--;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex > this.minIndex)
                                    {
                                        this.currentIndex--;
                                        nextIndex--;
                                    }
                                    else
                                    {
                                        //we have gone through and found nothing.
                                        //reset information.
                                        this.currentIndex = tempIndex;
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Nothing found return index to original.
                            this.currentIndex = tempIndex;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    this.searchResults.Text = "No more results";
                    System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    this.searchResults.Text = String.Empty;
                }

                eventIDInput.Text = tempEventIDText;
                info1Input.Text = tempinfo1Text;
                info2Input.Text = tempinfo2Text;
                info3Input.Text = tempinfo3Text;
                info4Input.Text = tempinfo4Text;

                this.refresh(false);
            }
        }

        /* To move to the next index.  */
        public void MoveNext()
        {
            string tempEventIDText = eventIDInput.Text;
            string tempinfo1Text = info1Input.Text;
            string tempinfo2Text = info2Input.Text;
            string tempinfo3Text = info3Input.Text;
            string tempinfo4Text = info4Input.Text;

            bool eventIDInputIsNumber = true;
            bool info1InputIsNumber = true;
            bool info2InputIsNumber = true;
            bool info3InputIsNumber = true;
            bool info4InputIsNumber = true;

            bool eventIDInputIsMatch = true;
            bool info1InputIsMatch = true;
            bool info2InputIsMatch = true;
            bool info3InputIsMatch = true;
            bool info4InputIsMatch = true;

            if (this.currentIndex != FindByValueIndex)
            {
                updateIndexNext();
            }

            if (this.currentIndex < this.maxIndex)
            {
                int tempIndex = this.currentIndex;
                if (nextIndex <= this.currentIndex)
                {
                    nextIndex = this.currentIndex + 1;
                }

                this.currentIndex = nextIndex;
                bool found = false;

                while (!found)
                {
                    if ((bool)this.matchAllFields.IsChecked)
                    {
                        if ((this.eventIDInput.Text == "") || (this.info1Input.Text == "") || (this.info2Input.Text == "")
                                || (this.info3Input.Text == "") || (this.info4Input.Text == ""))
                        {
                            if (this.eventIDInput.Text == "")
                            {
                                eventIDInputIsNumber = false;
                                eventIDInputIsMatch = true;
                            }
                            if (this.info1Input.Text == "")
                            {
                                info1InputIsNumber = false;
                                info2InputIsMatch = true;
                            }
                            if (this.info2Input.Text == "")
                            {
                                info2InputIsNumber = false;
                                info2InputIsMatch = true;
                            }
                            if (this.info3Input.Text == "")
                            {
                                info3InputIsNumber = false;
                                info3InputIsMatch = true;
                            }
                            if (this.info4Input.Text == "")
                            {
                                info4InputIsNumber = false;
                                info4InputIsMatch = true;
                            }
                        }

                        if (eventIDInputIsNumber)
                        {
                            if (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))
                            {
                                eventIDInputIsMatch = true;
                            }
                            else
                            {
                                eventIDInputIsMatch = false;
                            }
                        }

                        if (info1InputIsNumber)
                        {
                            if ((this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                            {
                                info1InputIsMatch = true;
                            }
                            else
                            {
                                info1InputIsMatch = false;
                            }
                        }

                        if (info2InputIsNumber)
                        {
                            if ((this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                            {
                                info2InputIsMatch = true;
                            }
                            else
                            {
                                info2InputIsMatch = false;
                            }
                        }

                        if (info3InputIsNumber)
                        {
                            if ((this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                            {
                                info3InputIsMatch = true;
                            }
                            else
                            {
                                info3InputIsMatch = false;
                            }
                        }

                        if (info4InputIsNumber)
                        {
                            if ((this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture)))
                            {
                                info4InputIsMatch = true;
                            }
                            else
                            {
                                info4InputIsMatch = false;
                            }
                        }

                        if (this.currentIndex < this.maxIndex)
                        {
                            if (contextInput.Text == "Any Context")
                            {
                                if ((eventIDInputIsMatch) && (info1InputIsMatch) && (info2InputIsMatch) && (info3InputIsMatch) && (info4InputIsMatch))
                                {
                                    found = true;
                                    nextIndex++;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex < this.maxIndex)
                                    {
                                        this.currentIndex++;
                                        nextIndex++;
                                    }
                                }
                            }
                            else
                            {
                                if ((this.tfi.Events[this.currentIndex].Context == contextIndex) && (eventIDInputIsMatch) && (info1InputIsMatch) && (info2InputIsMatch) && (info3InputIsMatch) && (info4InputIsMatch))
                                {
                                    found = true;
                                    nextIndex++;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex < this.maxIndex)
                                    {
                                        this.currentIndex++;
                                        nextIndex++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Nothing found return the old index.
                            this.currentIndex = tempIndex;
                            break;
                        }
                    }
                    else if ((bool)this.matchAnyField.IsChecked)
                    {
                        if (this.currentIndex < this.maxIndex)
                        {
                            if ((this.eventIDInput.Text == "") || (this.info1Input.Text == "") || (this.info2Input.Text == "")
                                || (this.info3Input.Text == "") || (this.info4Input.Text == ""))
                            {
                                if (this.eventIDInput.Text == "")
                                {
                                    eventIDInputIsNumber = false;
                                }
                                if (this.info1Input.Text == "")
                                {
                                    info1InputIsNumber = false;
                                }
                                if (this.info2Input.Text == "")
                                {
                                    info2InputIsNumber = false;
                                }
                                if (this.info3Input.Text == "")
                                {
                                    info3InputIsNumber = false;
                                }
                                if (this.info4Input.Text == "")
                                {
                                    info4InputIsNumber = false;
                                }
                            }

                            if (contextInput.Text == "Any Context")
                            {
                                if (((eventIDInputIsNumber) && (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info1InputIsNumber) && (this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info2InputIsNumber) && (this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info3InputIsNumber) && (this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info4InputIsNumber) && (this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))))
                                {
                                    found = true;
                                    nextIndex++;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex < this.maxIndex)
                                    {
                                        this.currentIndex++;
                                        nextIndex++;
                                    }
                                }
                            }
                            else
                            {
                                if ((this.tfi.Events[this.currentIndex].Context == contextIndex) ||
                                     ((eventIDInputIsNumber) && (this.tfi.Events[this.currentIndex].Id == Convert.ToUInt32(this.eventIDInput.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info1InputIsNumber) && (this.tfi.Events[this.currentIndex].Info1 == Convert.ToUInt32(this.info1Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info2InputIsNumber) && (this.tfi.Events[this.currentIndex].Info2 == Convert.ToUInt32(this.info2Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info3InputIsNumber) && (this.tfi.Events[this.currentIndex].Info3 == Convert.ToUInt32(this.info3Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))) ||
                                     ((info4InputIsNumber) && (this.tfi.Events[this.currentIndex].Info4 == Convert.ToUInt32(this.info4Input.Text.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture))))
                                {
                                    found = true;
                                    nextIndex++;
                                    MoveTo(this.currentIndex, false);
                                }
                                else
                                {
                                    if (nextIndex < this.maxIndex)
                                    {
                                        this.currentIndex++;
                                        nextIndex++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Nothing found return the old index.
                            this.currentIndex = tempIndex;
                            break;
                        }
                    }
                }

                if (!found)
                {
                    this.searchResults.Text = "No more results";
                    System.Media.SystemSounds.Beep.Play();
                }
                else
                {
                    this.searchResults.Text = String.Empty;
                }

                eventIDInput.Text = tempEventIDText;
                info1Input.Text = tempinfo1Text;
                info2Input.Text = tempinfo2Text;
                info3Input.Text = tempinfo3Text;
                info4Input.Text = tempinfo4Text;

                this.refresh(false);
            }
        }

        void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            string tempEventIDText = eventIDInput.Text;
            string tempinfo1Text = info1Input.Text;
            string tempinfo2Text = info2Input.Text;
            string tempinfo3Text = info3Input.Text;
            string tempinfo4Text = info4Input.Text;
            double Num;

            if ((eventIDInput.Text.StartsWith("0x", StringComparison.Ordinal)) || (info1Input.Text.StartsWith("0x", StringComparison.Ordinal)) || (info2Input.Text.StartsWith("0x", StringComparison.Ordinal)) ||
                (info3Input.Text.StartsWith("0x", StringComparison.Ordinal)) || (info4Input.Text.StartsWith("0x", StringComparison.Ordinal)))
            {
                if (eventIDInput.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    eventIDInput.Text = eventIDInput.Text.Remove(0, 2);
                    if (eventIDInput.Text.Length <= 8)
                    {
                        for (int i = 0; i < eventIDInput.Text.Length; i++)
                        {
                            eventIDInput.Text.ToCharArray();
                            if ((eventIDInput.Text[i] < 0x30) || ((eventIDInput.Text[i] > 0x39) && (eventIDInput.Text[i] < 0x41)) || ((eventIDInput.Text[i] > 0x5A) && (eventIDInput.Text[i] < 0x61)) ||
                                eventIDInput.Text[i] > 0x7A)
                            {
                                eventIDInput.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Event ID";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }

                        eventIDInput.Text = Convert.ToUInt32(eventIDInput.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        eventIDInput.Text = string.Empty;
                        //tempTextIndex = string.Empty;
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Event ID";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info1Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info1Input.Text = info1Input.Text.Remove(0, 2);
                    if (info1Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info1Input.Text.Length; i++)
                        {
                            info1Input.Text.ToCharArray();
                            if ((info1Input.Text[i] < 0x30) || ((info1Input.Text[i] > 0x39) && (info1Input.Text[i] < 0x41)) || ((info1Input.Text[i] > 0x5A) && (info1Input.Text[i] < 0x61)) ||
                                info1Input.Text[i] > 0x7A)
                            {
                                info1Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 1";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info1Input.Text = Convert.ToUInt32(info1Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info1Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 1";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info2Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info2Input.Text = info2Input.Text.Remove(0, 2);
                    if (info2Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info2Input.Text.Length; i++)
                        {
                            info2Input.Text.ToCharArray();
                            if ((info2Input.Text[i] < 0x30) || ((info2Input.Text[i] > 0x39) && (info2Input.Text[i] < 0x41)) || ((info2Input.Text[i] > 0x5A) && (info2Input.Text[i] < 0x61)) ||
                                info2Input.Text[i] > 0x7A)
                            {
                                info1Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 2";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info2Input.Text = Convert.ToUInt32(info2Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info2Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 2";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info3Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info3Input.Text = info3Input.Text.Remove(0, 2);
                    if (info3Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info3Input.Text.Length; i++)
                        {
                            info3Input.Text.ToCharArray();
                            if ((info3Input.Text[i] < 0x30) || ((info3Input.Text[i] > 0x39) && (info3Input.Text[i] < 0x41)) || ((info3Input.Text[i] > 0x5A) && (info3Input.Text[i] < 0x61)) ||
                                info3Input.Text[i] > 0x7A)
                            {
                                info3Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 3";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info3Input.Text = Convert.ToUInt32(info3Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info3Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 3";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info4Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info4Input.Text = info4Input.Text.Remove(0, 2);
                    if (info4Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info4Input.Text.Length; i++)
                        {
                            info4Input.Text.ToCharArray();
                            if ((info4Input.Text[i] < 0x30) || ((info4Input.Text[i] > 0x39) && (info4Input.Text[i] < 0x41)) || ((info4Input.Text[i] > 0x5A) && (info4Input.Text[i] < 0x61)) ||
                                info4Input.Text[i] > 0x7A)
                            {
                                info4Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 3";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }

                        info4Input.Text = Convert.ToUInt32(info4Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info4Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 4";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
            }
            else
            {
                if (eventIDInput.Text != "")
                {
                    if ((double.TryParse(eventIDInput.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(eventIDInput.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            eventIDInput.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Event ID";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        eventIDInput.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Event ID";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info1Input.Text != "")
                {
                    if ((double.TryParse(info1Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info1Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info1Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 1";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info1Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 1";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info2Input.Text != "")
                {
                    if ((double.TryParse(info2Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info2Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info2Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 2";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info2Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 2";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info3Input.Text != "")
                {
                    if ((double.TryParse(info3Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info3Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info3Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 3";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info3Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 3";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info4Input.Text != "")
                {
                    if ((double.TryParse(info4Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info4Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info4Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 4";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info4Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 4";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
            }

            MovePrevious();
            //textIndex.Text = tempTextIndex;
            eventIDInput.Text = tempEventIDText;
            info1Input.Text = tempinfo1Text;
            info2Input.Text = tempinfo2Text;
            info3Input.Text = tempinfo3Text;
            info4Input.Text = tempinfo4Text;
            //this.eventIDInput.Focus();
            //this.info1Input.Focus();
            //this.info2Input.Focus();
            //this.info3Input.Focus();
            //this.info4Input.Focus();
        }

        void btnNext_Click(object sender, RoutedEventArgs e)
        {
            string tempEventIDText = eventIDInput.Text;
            string tempinfo1Text = info1Input.Text;
            string tempinfo2Text = info2Input.Text;
            string tempinfo3Text = info3Input.Text;
            string tempinfo4Text = info4Input.Text;
            double Num;

            if ((eventIDInput.Text.StartsWith("0x", StringComparison.Ordinal)) || (info1Input.Text.StartsWith("0x", StringComparison.Ordinal)) || (info2Input.Text.StartsWith("0x", StringComparison.Ordinal)) ||
                (info3Input.Text.StartsWith("0x", StringComparison.Ordinal)) || (info4Input.Text.StartsWith("0x", StringComparison.Ordinal)))
            {
                if (eventIDInput.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    eventIDInput.Text = eventIDInput.Text.Remove(0, 2);
                    if (eventIDInput.Text.Length <= 8)
                    {
                        for (int i = 0; i < eventIDInput.Text.Length; i++)
                        {
                            eventIDInput.Text.ToCharArray();
                            if ((eventIDInput.Text[i] < 0x30) || ((eventIDInput.Text[i] > 0x39) && (eventIDInput.Text[i] < 0x41)) || ((eventIDInput.Text[i] > 0x5A) && (eventIDInput.Text[i] < 0x61)) ||
                                eventIDInput.Text[i] > 0x7A)
                            {
                                eventIDInput.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Event ID";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }

                        eventIDInput.Text = Convert.ToUInt32(eventIDInput.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        eventIDInput.Text = string.Empty;
                        //tempTextIndex = string.Empty;
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Event ID";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info1Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info1Input.Text = info1Input.Text.Remove(0, 2);
                    if (info1Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info1Input.Text.Length; i++)
                        {
                            info1Input.Text.ToCharArray();
                            if ((info1Input.Text[i] < 0x30) || ((info1Input.Text[i] > 0x39) && (info1Input.Text[i] < 0x41)) || ((info1Input.Text[i] > 0x5A) && (info1Input.Text[i] < 0x61)) ||
                                info1Input.Text[i] > 0x7A)
                            {
                                info1Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 1";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info1Input.Text = Convert.ToUInt32(info1Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info1Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 1";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info2Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info2Input.Text = info2Input.Text.Remove(0, 2);
                    if (info2Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info2Input.Text.Length; i++)
                        {
                            info2Input.Text.ToCharArray();
                            if ((info2Input.Text[i] < 0x30) || ((info2Input.Text[i] > 0x39) && (info2Input.Text[i] < 0x41)) || ((info2Input.Text[i] > 0x5A) && (info2Input.Text[i] < 0x61)) ||
                                info2Input.Text[i] > 0x7A)
                            {
                                info1Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 2";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info2Input.Text = Convert.ToUInt32(info2Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info2Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 2";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info3Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info3Input.Text = info3Input.Text.Remove(0, 2);
                    if (info3Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info3Input.Text.Length; i++)
                        {
                            info3Input.Text.ToCharArray();
                            if ((info3Input.Text[i] < 0x30) || ((info3Input.Text[i] > 0x39) && (info3Input.Text[i] < 0x41)) || ((info3Input.Text[i] > 0x5A) && (info3Input.Text[i] < 0x61)) ||
                                info3Input.Text[i] > 0x7A)
                            {
                                info3Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 3";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }
                        info3Input.Text = Convert.ToUInt32(info3Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info3Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 3";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
                if (info4Input.Text.StartsWith("0x", StringComparison.Ordinal))
                {
                    info4Input.Text = info4Input.Text.Remove(0, 2);
                    if (info4Input.Text.Length <= 8)
                    {
                        for (int i = 0; i < info4Input.Text.Length; i++)
                        {
                            info4Input.Text.ToCharArray();
                            if ((info4Input.Text[i] < 0x30) || ((info4Input.Text[i] > 0x39) && (info4Input.Text[i] < 0x41)) || ((info4Input.Text[i] > 0x5A) && (info4Input.Text[i] < 0x61)) ||
                                info4Input.Text[i] > 0x7A)
                            {
                                info4Input.Text = string.Empty;
                                //tempTextIndex = "0";
                                //textIndex.Text = "FFFFFFFF";
                                searchResults.Text = "Invalid Info 3";
                                System.Media.SystemSounds.Beep.Play();
                                return;
                            }
                        }

                        info4Input.Text = Convert.ToUInt32(info4Input.Text.ToString(CultureInfo.InvariantCulture), 16).ToString(CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        info4Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        searchResults.Text = "Invalid Info 4";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
            }
            else
            {
                if (eventIDInput.Text != "")
                {
                    if ((double.TryParse(eventIDInput.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(eventIDInput.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            eventIDInput.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Event ID";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        eventIDInput.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Event ID";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info1Input.Text != "")
                {
                    if ((double.TryParse(info1Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info1Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info1Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 1";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info1Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 1";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info2Input.Text != "")
                {
                    if ((double.TryParse(info2Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info2Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info2Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 2";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info2Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString(CultureInfo.CurrentCulture);
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 2";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info3Input.Text != "")
                {
                    if ((double.TryParse(info3Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info3Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info3Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 3";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info3Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 3";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }

                if (info4Input.Text != "")
                {
                    if ((double.TryParse(info4Input.Text.ToString(CultureInfo.InvariantCulture), out Num)))
                    {
                        if (Convert.ToDouble(info4Input.Text, CultureInfo.InvariantCulture) > Convert.ToUInt32("FFFFFFFF", 16))
                        {
                            info4Input.Text = string.Empty;
                            //tempTextIndex = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                            //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                            //tempTextIndex = "0";
                            //textIndex.Text = "0";
                            searchResults.Text = "Invalid Info 4";
                            System.Media.SystemSounds.Beep.Play();
                            return;
                        }
                    }
                    else
                    {
                        info4Input.Text = string.Empty;
                        //tempTextIndex = "0xFFFFFFFF";
                        //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString();
                        //tempTextIndex = "0";
                        //textIndex.Text = "0";
                        searchResults.Text = "Invalid Info 4";
                        System.Media.SystemSounds.Beep.Play();
                        return;
                    }
                }
            }
            //else
            //{
            //    //textIndex.Text = string.Empty;
            //    eventIDInput.Text = string.Empty;
            //    info1Input.Text = string.Empty;
            //    info2Input.Text = string.Empty;
            //    info3Input.Text = string.Empty;
            //    info4Input.Text = string.Empty;
            //    //tempTextIndex = "0xFFFFFFFF";
            //    //textIndex.Text = Convert.ToUInt32("FFFFFFFF", 16).ToString();
            //    //tempTextIndex = "0";
            //    //textIndex.Text = "0";
            //    searchResults.Text = "Invalid";
            //    System.Media.SystemSounds.Beep.Play();
            //    return;
            //}

            MoveNext();
            eventIDInput.Text = tempEventIDText;
            info1Input.Text = tempinfo1Text;
            info2Input.Text = tempinfo2Text;
            info3Input.Text = tempinfo3Text;
            info4Input.Text = tempinfo4Text;
            //this.eventIDInput.Focus();
            //this.info1Input.Focus();
            //this.info2Input.Focus();
            //this.info3Input.Focus();
            //this.info4Input.Focus();
            //textIndex.Text = tempTextIndex;
            //this.textIndex.Focus();
        }
    }
}
