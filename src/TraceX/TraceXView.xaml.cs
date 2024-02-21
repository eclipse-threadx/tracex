using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AzureRTOS.Tml;
using Microsoft.Win32;
using WinForms = System.Windows.Forms;

namespace AzureRTOS.TraceManagement
{
    /// <summary>
    /// Interaction logic for TraceXView.xaml
    /// </summary>

    public partial class TraceXView : Window
    {
        public const string _tracexVersion = "Eclipse ThreadX TraceX 6.4.0.0";

        private List<Window> _infoWindows = new List<Window>();
        private const double _eventDisplayWidth = 12;
        private double oldEventHeight = 0.0;
        private List<TmlEvent> _events = new List<TmlEvent>();
        private List<TmlThread> _threads = new List<TmlThread>();
        private List<TmlObject> _objects = new List<TmlObject>();
        private TmlTrace _traceInfo = new TmlTrace();
        private ArrayList _customEventList = new ArrayList();
        private double _fileToolBarGroupWidth = 260;
        private string _ticksPerMicroSecondText;
        private string _fileName;

        private Code.TraceFileInfo _traceFileInfo;

        public MenuItem tempItem = new MenuItem();
 
        /// <summary>
        /// Constructor.
        /// </summary>
        public TraceXView()
        {
            Splash.ShowMe();

            InitializeComponent();

            Title = _tracexVersion + "- [No Trace File loaded]";

            searchByValueButton.Visibility = Visibility.Hidden;
            searchByValueButton.IsEnabled = false;
            searchByValueButtonDisabled.Visibility = Visibility.Visible;
            searchByValueButtonDisabled.IsEnabled = false;

            statusLinesReady.Checked += new RoutedEventHandler(StatusLinesReady_Checked);
            statusLinesReady.Unchecked += new RoutedEventHandler(StatusLinesReady_Unchecked);

            statusLinesAll.Checked += new RoutedEventHandler(StatusLinesAll_Checked);
            statusLinesAll.Unchecked += new RoutedEventHandler(StatusLinesAll_Unchecked);

            statusLinesOff.Checked += new RoutedEventHandler(StatusLinesOff_Checked);
            statusLinesOff.Unchecked += new RoutedEventHandler(StatusLinesOff_Unchecked);

            // Set flag indicating the application has no file open.
            App.HasOpenFile = false;

            RecentFileList.MenuClick += (s, e) => OpenFile(e.Filepath);

            miTickMapping.SubmenuClosed += new RoutedEventHandler(MiTickMapping_SubmenuClosed);

            // If the file is double clicked on instead of chosen from the file menu.
            if (!string.IsNullOrEmpty(App.TraceFile))
            {
                // Load the file and show the information.
                EnableTabView(false);
                zoomToolbar.Loaded += new RoutedEventHandler(ZoomToolbar_Loaded);
            }
            else
            {
                // No double click, keep tabs unselected.
                EnableTabView(false);
            }
        }

        private void OnContentRendered(object sender, EventArgs e)
        {
            CheckInvokeInstallDialog();
        }

        private void CheckInvokeInstallDialog()
        {
            string keyName;

            string installPath = Path.GetDirectoryName(Application.ResourceAssembly.Location);
            //string message = "Install Path: ";
            //message += installPath;
            //MessageBox.Show(message);

            if (!installPath.Contains("WindowsApps"))
            {
                // installed from script, just return
                return;
            }

            keyName = "Software\\" +
            AzureRTOS.TraceManagement.Components.RecentFileList.ApplicationAttributes.CompanyName + "\\" +
            AzureRTOS.TraceManagement.Components.RecentFileList.ApplicationAttributes.ProductName;

            RegistryKey k = Registry.CurrentUser.OpenSubKey(keyName, true);
            if (k == null)
            {
                Registry.CurrentUser.CreateSubKey(keyName);
                k = Registry.CurrentUser.OpenSubKey(keyName, true);
            }

            string version = (string) k.GetValue("Version");
            string installDir = (string) k.GetValue("InstallDir");

            // If the version key doesn't exist, or it has changed, or if the InstallDir
            // key doesn't exist, then invoke the dialog to copy the example files
            // to user accessible location:

            
            if (version != _tracexVersion || installDir == null)
            {
                // Create the default directory, otherwise the Browser dialog
                // will not allow us to start there:

                string default_path = "C:\\Eclipse_ThreadX\\TraceX";

                if (!Directory.Exists(default_path))
                {
                    Directory.CreateDirectory(default_path);
                }

                WinForms.FolderBrowserDialog dlg = new WinForms.FolderBrowserDialog
                {
                    Description = "TraceX will copy example trace data files and a custom_events file to the directory you choose below, or select Cancel to skip this operation.",
                    ShowNewFolderButton = true,
                    SelectedPath = default_path,
                    RootFolder = Environment.SpecialFolder.MyComputer
                };

                WinForms.DialogResult result = dlg.ShowDialog();

                if (result == WinForms.DialogResult.OK)
                {
                    string userPath = dlg.SelectedPath;
                    CopyExampleFiles(userPath);
                    k.SetValue("Version", _tracexVersion);
                    k.SetValue("InstallDir", userPath);
                }
            }
        }

        private void CopyExampleFiles(string userPath)
        {
            string targetPath;
            string installPath = Path.GetDirectoryName(Application.ResourceAssembly.Location);
            int index = installPath.LastIndexOf("TraceX");
            installPath = installPath.Substring(0, index);

            // If we are running locally from the debugger, the ResourceAssembly.Location just returns our
            // local build directory, so we have to adjust the resource path slightly
#if DEBUG
            installPath = Path.Combine(installPath, "tracex_package");
#endif
            string sourcePath = Path.Combine(installPath, "TraceFiles");

            if (Directory.Exists(sourcePath))
            {
                targetPath = Path.Combine(userPath, "TraceFiles");

                if (!File.Exists(Path.Combine(targetPath, "demo_threadx.trx")))
                {
                    Directory.CreateDirectory(targetPath);
                    string[] files = Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = Path.GetFileName(s);
                        string destFile = Path.Combine(targetPath, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
            }

            sourcePath = Path.Combine(installPath, "CustomEvents");

            if (Directory.Exists(sourcePath))
            {
                targetPath = Path.Combine(userPath, "CustomEvents");

                if (!File.Exists(targetPath + "\\tracex_custom.trxc"))
                {
                    Directory.CreateDirectory(targetPath);

                    string[] files = Directory.GetFiles(sourcePath);

                    // Copy the files and overwrite destination files if they already exist.
                    foreach (string s in files)
                    {
                        // Use static Path methods to extract only the file name from the path.
                        string fileName = Path.GetFileName(s);
                        string destFile = Path.Combine(targetPath, fileName);
                        File.Copy(s, destFile, true);
                    }
                }
            }
        }


        private void ZoomToolbar_Loaded(object sender, EventArgs e)
        {
            RecentFileList.InsertFile(App.TraceFile);
            OpenFile(App.TraceFile);
        }

        // Enable tabviews.
        private void EnableTabView(bool enabled)
        {
            tabViews.IsEnabled = enabled;
            zoomToolbar.IsEnabled = enabled;
        }

        //  Event handler for File -> Open.
        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            // Open file.  Set extension and filters.
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".trx", // Default file extension
                Filter = "TraceX Files (*.trx)|*.trx|All Files (*.*)|*.*" // Filter files by extension
            };

            if (Properties.Settings.Default.FilesPath.Length == 0)
            {
                // Default directory to tracefiles folder.
                dlg.InitialDirectory = GetExampleTraceFilesFolder();
                Properties.Settings.Default.FilesPath = dlg.InitialDirectory;
            }
            else
            {
                // Else, use the last selected directory.
                dlg.InitialDirectory = Properties.Settings.Default.FilesPath;
            }

            // Make the navigator strip hidden.
            navigator.IsEnabled = false;
            zoomToolbar.IsEnabled = false;

            // Set flag to restore directory and get result if dialog is shown.
            dlg.RestoreDirectory = true;
            bool? result = dlg.ShowDialog();

            // Check if there is a reasonable result.
            if (result.HasValue && !result.Value)
            {
                // Check if the application already has an open file.
                if (App.HasOpenFile)
                {
                    // It does. Keep navigator and tab views visible and return.
                    navigator.MoveTo(0, false);
                    navigator.IsEnabled = true;
                    zoomToolbar.IsEnabled = true;
                    tabViews.Visibility = Visibility.Visible;

                    currentEventDisabled.Visibility = Visibility.Hidden;
                    currentEventDisabled.IsEnabled = false;
                    currentEvent.IsEnabled = true;
                    currentEvent.Visibility = Visibility.Visible;

                    searchByValueButtonDisabled.Visibility = Visibility.Hidden;
                    searchByValueButtonDisabled.IsEnabled = false;
                    searchByValueButton.Visibility = Visibility.Visible;
                    searchByValueButton.IsEnabled = true;

                    executionProfileDisabled.Visibility = Visibility.Hidden;
                    executionProfileDisabled.IsEnabled = false;
                    executionProfile.IsEnabled = true;
                    executionProfile.Visibility = Visibility.Visible;

                    performanceStatisticsDisabled.Visibility = Visibility.Hidden;
                    performanceStatisticsDisabled.IsEnabled = false;
                    performanceStatistics.IsEnabled = true;
                    performanceStatistics.Visibility = Visibility.Visible;

                    threadStackUsageDisabled.Visibility = Visibility.Hidden;
                    threadStackUsageDisabled.IsEnabled = false;
                    threadStackUsage.IsEnabled = true;
                    threadStackUsage.Visibility = Visibility.Visible;
                }

                Cursor = Cursors.Arrow;
                return;
            }

            RecentFileList.InsertFile(dlg.FileName);

            // Set the default tabview page to 0 - SequentialView.
            OpenFile(dlg.FileName);
        }

        public void ReopenFile()
        {
            if (App.HasOpenFile == true && _fileName.Length > 0)
            {
                OpenFile(_fileName);
            }
        }

        private void OpenFile(string filePath)
        {
            foreach (Window window in _infoWindows)
            {
                window.Close();
            }

            if (!File.Exists(filePath))
            {
                MessageBox.Show(
                    $"This file has moved or does not exist:\n{filePath}",
                    "Can Not Find File!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Exclamation);
                return;
            }

            _infoWindows.RemoveRange(0, _infoWindows.Count);
            Dialogs.FindByValue.FindByValueIndex = 0;

            tabViews.SelectedIndex = 0;

            // While the file is opening, indicate it on the title bar(for longer loading files).
            Title = String.Format(CultureInfo.CurrentCulture, _tracexVersion + " - [Opening file: {0}]", filePath);
            Properties.Settings.Default.FilesPath = Path.GetDirectoryName(filePath);

            // Enable tabViews and begin the sequence to load the rest of the file.
            EnableTabView(true);
            _fileName = filePath;
            ShowTraceInfo(filePath);

            if (_traceFileInfo != null)
            {
                _traceFileInfo.CurrentEventIndex = 0;
            }
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;

            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
        }

        // Load and show the trace file.
        private void ShowTraceInfo(string fileName)
        {
            Cursor previousCursor = Cursor;
            Cursor = Cursors.Wait;

            try
            {
                // Clear threads, objects, and events.
                _threads.Clear();
                _objects.Clear();
                _events.Clear();

                ELTMLManaged.TMLFunctions.Uninitialize();

                // If the extraction of the trace info from the stand alone trace module fails.
                if (!ELTMLManaged.TMLFunctions.ExtractTraceInfo(fileName, _traceInfo, _threads, _objects, _events))
                {
                    // Clear the threads, objects, and events.  Reset the tab selection to 0.
                    // Disable tabViews, set the application to no longer having an open file.
                    // Disable the view menu items and call a clean up from sequentialView.
                    _threads.Clear();
                    _objects.Clear();
                    _events.Clear();

                    if (App.HasOpenFile)
                    {
                        sview1.CleanUpView();
                    }

                    tabViews.SelectedIndex = 0;
                    EnableTabView(false);
                    App.HasOpenFile = false;
                    MenuItemView.IsEnabled = false;
                    MenuItemOptions.IsEnabled = false;

                    currentEventDisabled.Visibility = Visibility.Visible;
                    currentEventDisabled.IsEnabled = false;
                    currentEvent.IsEnabled = false;
                    currentEvent.Visibility = Visibility.Hidden;

                    navigator.IsEnabled = false;

                    searchByValueButtonDisabled.Visibility = Visibility.Visible;
                    searchByValueButtonDisabled.IsEnabled = false;
                    searchByValueButton.IsEnabled = false;
                    searchByValueButton.Visibility = Visibility.Hidden;

                    executionProfileDisabled.Visibility = Visibility.Visible;
                    executionProfileDisabled.IsEnabled = false;
                    executionProfile.IsEnabled = false;
                    executionProfile.Visibility = Visibility.Hidden;

                    performanceStatisticsDisabled.Visibility = Visibility.Visible;
                    performanceStatisticsDisabled.IsEnabled = false;
                    performanceStatistics.IsEnabled = false;
                    performanceStatistics.Visibility = Visibility.Hidden;

                    threadStackUsageDisabled.Visibility = Visibility.Visible;
                    threadStackUsageDisabled.IsEnabled = false;
                    threadStackUsage.IsEnabled = false;
                    threadStackUsage.Visibility = Visibility.Hidden;

                    // Set the title to read the invalid.  Show the error and return.
                    Title = String.Format(CultureInfo.CurrentCulture, _tracexVersion + " - Invalid", fileName);
                    MessageBox.Show(ELTMLManaged.TMLFunctions.ErrorMessage);
                    Cursor = previousCursor;
                    return;
                }

                // Set the application to having an open file and enable the view menu item.
                App.HasOpenFile = true;
                MenuItemView.IsEnabled = true;
                MenuItemOptions.IsEnabled = true;

                searchByValueButton.IsEnabled = true;
                searchByValueButton.Visibility = Visibility.Visible;
                searchByValueButtonDisabled.Visibility = Visibility.Hidden;
                searchByValueButtonDisabled.IsEnabled = false;
                sview1.cleanDeltaMask();

                var dictThreadExecutionStatus = new Dictionary<uint, List<TmlExecutionStatus>>();

                // it seems that we don't need to sort because it is already based on relative time ticks
                // Set the current Trace File Information.  Set the range of the navigator.
                //  Initialize the sequential and time components.
                _traceFileInfo = new Code.TraceFileInfo(_traceInfo, _threads, _objects, _events, dictThreadExecutionStatus);
                navigator.SetTraceEventInfo(_traceFileInfo);
                navigator.IndexChanged = new EventHandler<Code.IndexEventArgs>(NavigatorIndexChange);
                navigator.PageChanged = new EventHandler<Code.PageEventArgs>(NavigatorPageChange);

                Dialogs.FindByValue.IndexChanged = new EventHandler<Code.IndexEventArgs>(FindByValueIndexChange);
                Dialogs.FindByValue.PageChanged = new EventHandler<Code.PageEventArgs>(FindByValuePageChange);

                // Set the title to the current file.
                Title = String.Format(CultureInfo.CurrentCulture, _tracexVersion + " - [{0}]", fileName);

                currentEventDisabled.Visibility = Visibility.Hidden;
                currentEventDisabled.IsEnabled = false;
                currentEvent.IsEnabled = true;
                currentEvent.Visibility = Visibility.Visible;

                executionProfileDisabled.Visibility = Visibility.Hidden;
                executionProfileDisabled.IsEnabled = false;
                executionProfile.IsEnabled = true;
                executionProfile.Visibility = Visibility.Visible;

                performanceStatisticsDisabled.Visibility = Visibility.Hidden;
                performanceStatisticsDisabled.IsEnabled = false;
                performanceStatistics.IsEnabled = true;
                performanceStatistics.Visibility = Visibility.Visible;

                threadStackUsageDisabled.Visibility = Visibility.Hidden;
                threadStackUsageDisabled.IsEnabled = false;
                threadStackUsage.IsEnabled = true;
                threadStackUsage.Visibility = Visibility.Visible;

                tabViews.Visibility = Visibility.Visible;
                zoomToolbar.ZoomChanged = new EventHandler<Code.ZoomEventArgs>(ZoomChange);

                tbxDeltaTicks.Content = "0";

                int n = 0;
                sview1.PriorityInversionArray.Clear();
                tview1.PriorityInversionArray.Clear();

                if (muSecondTextBox.Text != "")
                {
                    int getLongest = 0;
                    int tmpLongest = 0;

                    for (int i = 0; i < _traceFileInfo.Events.Count; i++)
                    {
                        TmlEvent tEvent = _traceFileInfo.Events[i];

                        if (tEvent.BadPriorityInversion > 0
                            || tEvent.PriorityInversion > 0)
                        {
                            sview1.PriorityInversionArray.Add(i);
                            tview1.PriorityInversionArray.Add(i);
                            n++;
                        }

                        tmpLongest = tEvent.RelativeTicks.ToString(CultureInfo.InvariantCulture).Length;
                        if (tmpLongest > getLongest)
                        {
                            getLongest = tmpLongest;
                        }
                    }

                    tbxDeltaTicks.Width = getLongest * 15;
                    tbxDeltaTicks.Content = "0 (0.00 µs)";
                }
                else
                {
                    for (int i = 0; i < _traceFileInfo.Events.Count; i++)
                    {
                        TmlEvent tEvent = _traceFileInfo.Events[i];

                        if (tEvent.BadPriorityInversion > 0
                            || tEvent.PriorityInversion > 0)
                        {
                            sview1.PriorityInversionArray.Add(i);
                            tview1.PriorityInversionArray.Add(i);
                            n++;
                        }
                    }
                }

                // sequential view delta changed event handler
                tview1.DeltaChanged = sview1.DeltaChanged = new EventHandler<Code.DeltaEventArgs>(DeltaChange);
                tview1.ZoomLimitReached += new EventHandler<Code.ZoomEventArgs>(ZoomLimitReached);
                tview1.SplitterPositionChanged += new EventHandler<EventArgs>(ChildSplitterPositionChanged);
                sview1.SplitterPositionChanged += new EventHandler<EventArgs>(ChildSplitterPositionChanged);
                sview1.ZoomLimitReached += new EventHandler<Code.ZoomEventArgs>(ZoomLimitReached);

                sview1.Initialize(_traceFileInfo, navigator);
                tview1.SiblingViewPortWidth = sview1.SiblingViewPortWidth;
                tview1.SiblingViewPortHeight = sview1.SiblingViewPortHeight;
                tview1.Initialize(_traceFileInfo, navigator);

                // Make the navigator and tabviews visible.
                navigator.MoveTo(0, false);
                navigator.IsEnabled = true;

                tabItemTime.IsEnabled = true;

                GetLineInfo();
                SetNavigatorInfo();
                Code.Event.SetCustomEventList(_customEventList);

                if (_traceFileInfo.Events.Count > 0)
                {
                    if (_traceFileInfo.Events[1].RelativeTicks == 0)
                    {
                        if (_traceFileInfo.Events.Count >= 1)
                        {
                            if (_traceFileInfo.Events[_traceFileInfo.Events.Count - 1].TimeStamp - _traceFileInfo.Events[0].TimeStamp == 0)
                            {
                                tabItemTime.IsEnabled = false;
                                MessageBox.Show(
                                    "The time source is static - disabling Time View.\nPlease see the TraceX User Guide to\nproperly set the time source.", "TraceX: Delta Values - Time View Disabled",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Exclamation);
                            }
                        }
                        else
                        {
                            tabItemTime.IsEnabled = false;
                            MessageBox.Show(
                                "The time source shows only one event - disabling Time View.\nPlease see the TraceX User Guide to\nproperly set the time source.", "TraceX: Delta Values",
                                MessageBoxButton.OK,
                                MessageBoxImage.Exclamation);
                        }
                    }
                }
                zoomToolbar.InitializeZoomFactors(true, sview1.GetZoomFactor(), sview1.EnableZoomIn, sview1.EnableZoomOut);
            }
            catch (Exception ex)
            {
                // If there is an exception.  Clear and show the error.
                _threads.Clear();
                _objects.Clear();
                _events.Clear();
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Cursor = previousCursor;
        }

        private void ContextCreation_Click(object sender, RoutedEventArgs e)
        {
            var newContextMenuItem = (MenuItem)e.OriginalSource;
            string[] Address = newContextMenuItem.Header.ToString().Split('(');
            Address[1] = Address[1].Remove(0, 2);
            string AddressC = Address[1].TrimEnd(')');
            navigator.SetInformation(newContextMenuItem.Header.ToString(), AddressC, "Context", 1);
        }

        private void ObjCreation_Click(object sender, RoutedEventArgs e)
        {
            var newObjMenuItem = (MenuItem)e.OriginalSource;
            string[] Address = newObjMenuItem.Header.ToString().Split('(');
            Address[1] = Address[1].Remove(0, 2);
            string AddressC = Address[1].TrimEnd(')');
            navigator.SetInformation(newObjMenuItem.Header.ToString(), AddressC, "Object", 2);
        }

        private void IDCreation_Click(object sender, RoutedEventArgs e)
        {
            var newIdMenuItem = (MenuItem)e.OriginalSource;
            string[] Address = newIdMenuItem.Header.ToString().Split('(');
            string AddressC = Address[1].TrimEnd(')');
            navigator.SetInformation(newIdMenuItem.Header.ToString(), AddressC, "ID", 4);
        }

        private void SetNavigatorInfo()
        {
            var threads = new ArrayList();
            var objects = new ArrayList();
            navigator.miContext.Items.Clear();
            navigator.miID.Items.Clear();
            navigator.miObject.Items.Clear();

            var dictObjects = new Dictionary<string, string>();
            var dictThreads = new Dictionary<string, string>();
            var dictEvents = new Dictionary<string, string>();

            navigator.SetInformation("Event", "Event", "Event", 0);
            foreach (TmlThread tt in _traceFileInfo.Threads)
            {
                if (tt.Address != 0x00000000
                    && tt.Address != 0xFFFFFFFF
                    && tt.Address != 0xF0F0F0F0)
                {
                    if (!dictThreads.ContainsKey(tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")"))
                    {
                        dictThreads.Add(tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture) + ")", tt.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    var contextCreation = new MenuItem();
                    contextCreation.Header = tt.Name + " (0x" + tt.Address.ToString("x8", CultureInfo.CurrentCulture).ToUpper(CultureInfo.CurrentCulture) + ")";
                    contextCreation.Click += new RoutedEventHandler(ContextCreation_Click);
                    navigator.miContext.Items.Add(contextCreation);
                }
                threads.Add(tt.Name);
            }

            // Sort the dictionary.
            var sortedThreads = (from entry in dictThreads orderby entry.Key ascending select entry);

            foreach (KeyValuePair<string, string> key in sortedThreads)
            {
                var contextCreation = new MenuItem();
                contextCreation.Header = key.Key;
                contextCreation.Click += new RoutedEventHandler(ContextCreation_Click);
                navigator.miContext.Items.Add(contextCreation);
            }

            // Get the objects into a dictionary.
            foreach (TmlObject to in _traceFileInfo.Objects)
            {
                if ((!threads.Contains(to.ObjectName)) && (!objects.Contains(to.ObjectName)))
                {
                    dictObjects.Add(to.ObjectName, to.Address.ToString("x8", CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));
                    objects.Add(to.ObjectName);
                }
            }

            // Sort the dictionary.
            var sortedObj = (from entry in dictObjects orderby entry.Key ascending select entry);

            // Fill the menu item with the sorted values.
            foreach (KeyValuePair<string, string> key in sortedObj)
            {
                var objCreation = new MenuItem();
                objCreation.Header = key.Key + " (0x" + key.Value + ")";
                objCreation.Click += new RoutedEventHandler(ObjCreation_Click);
                navigator.miObject.Items.Add(objCreation);
            }

            threads.Clear();
            threads = null;

            var ids = new ArrayList();
            foreach (TmlEvent te in _traceFileInfo.Events)
            {
                if (!ids.Contains(te.Id))
                {
                    var e = Code.Event.CreateInstance(te, string.Empty, _traceFileInfo);

                    string text = e.EventTypeName.ToString(CultureInfo.CurrentCulture).Replace("_", "__");
                    dictEvents.Add(text, te.Id.ToString(CultureInfo.InvariantCulture));
                    ids.Add(te.Id);
                }
            }

            var sortedEvents = (from entry in dictEvents orderby entry.Key ascending select entry);

            foreach (KeyValuePair<string, string> key in sortedEvents)
            {
                var idCreation = new MenuItem
                {
                    Header = key.Key
                };
                idCreation.Click += new RoutedEventHandler(IDCreation_Click);
                navigator.miID.Items.Add(idCreation);
            }
        }

        private void GetLineInfo()
        {
            string fileName = Path.Combine(GetInstallDir(), @"CustomEvents\tracex_custom.trxc");
            var fi = new FileInfo(fileName);
            string msg;

            try
            {
                if (fi.Exists)
                {
                    using (var sr = new StreamReader(fileName))
                    {
                        string line;
                        bool isStart = true;
                        bool isNotStart = false;
                        bool passesCheck = false;
                        string status = "none";

                        int lineNumber = 0;

                        if (isStart)
                        {
                            while ((line = sr.ReadLine()) != "Start")
                            {
                                if (sr.EndOfStream)
                                {
                                    isNotStart = true;
                                    break;
                                }
                                lineNumber++;
                                continue;
                            }
                            isStart = false;
                        }

                        if (!isNotStart)
                        {
                            while ((line = sr.ReadLine()) != "End")
                            {
                                lineNumber++;
                                string[] checkLine = line.Split(',');

                                status = "There are not enough elements to the line of syntax.";
                                if (checkLine.Length > 12)
                                {
                                    status = "The syntax of the first entry is invalid.";
                                    // Event ID
                                    if ((checkLine[0] != "") && (checkLine[0] != null))
                                    {
                                        string str = checkLine[0];
                                        bool isNum = double.TryParse(str, out double Num);
                                        status = " The syntax of the first entry is not a number.";
                                        if (isNum)
                                        {
                                            status = "The syntax of the second entry is incorrect - Event Name is not valid.";
                                            // Event Name
                                            if ((checkLine[1] != "") && (checkLine[1] != null))
                                            {
                                                status = "The syntax of the third entry is incorrect - Two Letter Abreviation is not valid.";
                                                // Two letter abreviation
                                                if ((checkLine[2] != "") && (checkLine[1] != null))
                                                {
                                                    status = "The syntax of the third entry is incorrect - Two Letter Abreviation is more than 2 letters.";
                                                    // Make sure at least 2 letters.
                                                    if ((checkLine[2].Length > 0) && (checkLine[2].Length <= 3))
                                                    {
                                                        status = "The syntax of the fourth entry is incorrect - Color 1 (1 of 3)";
                                                        // Colors1 1 of 3
                                                        if ((checkLine[3] != "") && (checkLine[3] != null))
                                                        {
                                                            status = "The syntax of the fourth entry - Color 1 (1 of 3) is missing a Parenthesis";
                                                            if (checkLine[3].Contains("("))
                                                            {
                                                                str = checkLine[3].Trim().TrimStart('(');
                                                                isNum = double.TryParse(str, out Num);

                                                                status = "The syntax of the fourth entry - Color 1 (1 of 3) is not a number (range - 0 to 255).";
                                                                if (isNum)
                                                                {
                                                                    status = "The syntax of the fifth entry is incorrect - Color 1 (2 of 3)";
                                                                    // Colors1 2 of 3
                                                                    if ((checkLine[4] != "") && (checkLine[4] != null))
                                                                    {
                                                                        str = checkLine[4].Trim();
                                                                        isNum = double.TryParse(str, out Num);

                                                                        status = "The syntax of the fifth entry - Color 1 (2 of 3) is not a number (range - 0 to 255).";
                                                                        if (isNum)
                                                                        {
                                                                            status = "The syntax of the sixth entry is incorrect - Color 1 (3 of 3)";
                                                                            // Colors1 3 of 3
                                                                            if ((checkLine[5] != "") && (checkLine[5] != null))
                                                                            {
                                                                                status = "The syntax of the sixth entry - Color 1 (3 of 3) is missing a Parenthesis";
                                                                                if (checkLine[5].Contains(")"))
                                                                                {
                                                                                    str = checkLine[5].Trim().TrimEnd(')');
                                                                                    isNum = double.TryParse(str, out Num);

                                                                                    status = "The syntax of the sixth entry - Color 1 (3 of 3) is not a number (range - 0 to 255).";
                                                                                    if (isNum)
                                                                                    {
                                                                                        // // // // // // // // // // //
                                                                                        // Colors2 1 of 3
                                                                                        status = "The syntax of the seventh entry is incorrect - Color 2 (1 of 3)";
                                                                                        if ((checkLine[6] != "") && (checkLine[6] != null))
                                                                                        {
                                                                                            status = "The syntax of the seventh entry - Color 2 (1 of 3) is missing a Parenthesis";
                                                                                            if (checkLine[6].Contains("("))
                                                                                            {
                                                                                                str = checkLine[6].Trim().TrimStart('(');
                                                                                                isNum = double.TryParse(str, out Num);
                                                                                                status = "The syntax of the seventh entry - Color 2 (1 of 3) is not a number (range - 0 to 255).";
                                                                                                if (isNum)
                                                                                                {
                                                                                                    status = "The syntax of the eighth entry is incorrect - Color 2 (2 of 3)";
                                                                                                    // Colors2 2 of 3
                                                                                                    if ((checkLine[7] != "") && (checkLine[7] != null))
                                                                                                    {
                                                                                                        str = checkLine[7].Trim();
                                                                                                        isNum = double.TryParse(str, out Num);
                                                                                                        status = "The syntax of the eighth entry - Color 2 (2 of 3) is not a number (range - 0 to 255).";
                                                                                                        if (isNum)
                                                                                                        {
                                                                                                            status = "The syntax of the nineth entry is incorrect - Color 2 (3 of 3)";
                                                                                                            // Colors2 3 of 3
                                                                                                            if ((checkLine[8] != "") && (checkLine[8] != null))
                                                                                                            {
                                                                                                                status = "The syntax of the nineth entry - Color 2 (3 of 3) is missing a Parenthesis";
                                                                                                                if (checkLine[8].Contains(")"))
                                                                                                                {
                                                                                                                    str = checkLine[8].Trim().TrimEnd(')');
                                                                                                                    isNum = double.TryParse(str, out Num);
                                                                                                                    status = "The syntax of the nineth entry - Color 2 (3 of 3) is not a number (range - 0 to 255).";
                                                                                                                    if (isNum)
                                                                                                                    {
                                                                                                                        status = "The syntax of the tenth entry - info 1 is incorrect and requires a value";
                                                                                                                        if ((checkLine[9] != "") && (checkLine[9] != null))
                                                                                                                        {
                                                                                                                            status = "The syntax of the eleventh entry - info 2 is incorrect and requires a value";
                                                                                                                            if ((checkLine[10] != "") && (checkLine[10] != null))
                                                                                                                            {
                                                                                                                                status = "The syntax of the twelfth entry - info 3 is incorrect and requires a value";
                                                                                                                                if ((checkLine[11] != "") && (checkLine[11] != null))
                                                                                                                                {
                                                                                                                                    status = "The syntax of the thirteenth entry - info 4 is incorrect and requires a value";
                                                                                                                                    if ((checkLine[12] != "") && (checkLine[12] != null))
                                                                                                                                    {
                                                                                                                                        passesCheck = true;
                                                                                                                                    }
                                                                                                                                }
                                                                                                                            }
                                                                                                                        }
                                                                                                                    }
                                                                                                                }
                                                                                                            }
                                                                                                        }
                                                                                                    }
                                                                                                }
                                                                                            }
                                                                                        }
                                                                                    }
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (!passesCheck)
                                {
                                    msg = string.Format(CultureInfo.CurrentCulture,
                                        "Custom Event syntax is incorrect!\n" +
                                        "The following Error was found:\n" +
                                        "{0}\n" +
                                        "On line number: {1}",
                                        status, lineNumber);

                                    MessageBox.Show(
                                        msg,
                                        string.Format(CultureInfo.CurrentCulture, "Custom Event Error"),
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                                    break;
                                }

                                _customEventList.Add(line);
                                if (sr.EndOfStream)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            msg = string.Format(CultureInfo.CurrentCulture,
                                "Custom Event syntax is incorrect! \n" +
                                "There needs to be the words Start and the words End listed\n" +
                                "with the syntax between Start and End.");

                            MessageBox.Show(
                                msg,
                                string.Format(CultureInfo.CurrentCulture, "Custom Event Error"),
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }

                        fi = null;
                        isStart = false;
                    }
                }
                else
                {
                    msg = string.Format(CultureInfo.CurrentCulture,
                        "Problem loading the Custom Event File.\n" +
                        "Please check the location of the file.\n" +
                        "{0}\n" +
                        "CustomEvents\\tracex_custom.trxc\n\n" +
                        "If the problem persists please contact:\n" +
                        "https://aka.ms/azrtos-support.",
                        GetInstallDir());

                    MessageBox.Show(
                        msg,
                        string.Format(CultureInfo.CurrentCulture, "Error Opening Custom Event File!"),
                        MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch
            {
                msg = string.Format(CultureInfo.CurrentCulture,
                    "Problem loading the Custom Event File.\n" +
                    "Please check the location of the file.\n" +
                    "{0}\n" +
                    "CustomEvents\\tracex_custom.trxc\n\n" +
                    "If the problem persists please contact:\n" +
                    "https://aka.ms/azrtos-support.", GetInstallDir());

                MessageBox.Show(
                    msg,
                    string.Format(CultureInfo.CurrentCulture, "Error Opening Custom Event File!"),
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void RawDumpTraceFile(string sFileName)
        {
            if (!ELTMLManaged.TMLFunctions.RawTraceFileDump(sFileName, _tracexVersion, _fileName, sFileName))
            {
                MessageBox.Show(
                    string.Format(CultureInfo.CurrentCulture, "Write Error"),
                    string.Format(CultureInfo.CurrentCulture, "Error writing dump file!"),
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeltaChange(object sender, Code.DeltaEventArgs e)
        {
            switch (e.UnitsIndex)
            {
                case 0:
                    if (e.RelativeTime != 0)
                    {
                        tbxDeltaTicks.Content = e.DeltaTicks.ToString(CultureInfo.CurrentCulture) + "  " + "(" + e.RelativeTime.ToString(CultureInfo.CurrentCulture) + "µs)";
                    }
                    else
                    {
                        tbxDeltaTicks.Content = e.DeltaTicks.ToString(CultureInfo.CurrentCulture);

                        if (e.DeltaTicks == 0)
                        {
                            if (muSecondTextBox.Text == "0")
                            {
                                tbxDeltaTicks.Content = "0";
                            }
                            else if (muSecondTextBox.Text != "")
                            {
                                tbxDeltaTicks.Content = "0 (0.00 µs)";
                            }
                        }
                    }
                    lblDeltaTicks.Content = "Delta";
                    break;

                case 1:
                    tbxDeltaTicks.Content = (e.DeltaTicks * Properties.Settings.Default.uSecPerTick / 1000000).ToString(CultureInfo.CurrentCulture);
                    lblDeltaTicks.Content = "Delta Sec";
                    break;

                case 2:
                    tbxDeltaTicks.Content = (e.DeltaTicks * Properties.Settings.Default.uSecPerTick / 1000).ToString(CultureInfo.CurrentCulture);
                    lblDeltaTicks.Content = "Delta mSec";
                    break;

                case 3:
                    tbxDeltaTicks.Content = (e.DeltaTicks * Properties.Settings.Default.uSecPerTick).ToString(CultureInfo.CurrentCulture);
                    lblDeltaTicks.Content = "Delta uSec";
                    break;

                default:
                    tbxDeltaTicks.Content = e.DeltaTicks.ToString(CultureInfo.CurrentCulture);
                    lblDeltaTicks.Content = "Delta";
                    break;
            }
        }

        // Handler for a change in the navigator index.
        private void NavigatorIndexChange(object sender, Code.IndexEventArgs e)
        {
            sview1.MoveToEvent(e.Index, e.CenterMarker);
            tview1.MoveToEvent(e.Index, e.CenterMarker);

            if (Dialogs.CurrentEventInfo.hasCurrentEventWindow())
            {
                Dialogs.CurrentEventInfo.Show(_traceFileInfo);
            }

            Dialogs.FindByValue.FindByValueIndex = e.Index;
        }

        // Handler for page changes.
        private void NavigatorPageChange(object sender, Code.PageEventArgs e)
        {
            if (Code.Direction.Up == e.PageDirection)
            {
                if (tabViews.SelectedIndex == 0)
                {
                    e.Index = sview1.GetPreviousPageStartIndex(e.Index);
                }
                else
                {
                    e.Index = tview1.GetPreviousPageStartIndex(e.Index);
                }
            }
            else
            {
                if (tabViews.SelectedIndex == 0)
                {
                    e.Index = sview1.GetNextPageStartIndex(e.Index);
                }
                else
                {
                    e.Index = tview1.GetNextPageStartIndex(e.Index);
                }
            }

            Dialogs.FindByValue.FindByValueIndex = e.Index;
        }

        private void FindByValueIndexChange(object sender, Code.IndexEventArgs e)
        {
            sview1.MoveToEvent(e.Index, e.CenterMarker);
            tview1.MoveToEvent(e.Index, e.CenterMarker);
            if (Dialogs.CurrentEventInfo.hasCurrentEventWindow())
            {
                Dialogs.CurrentEventInfo.Show(_traceFileInfo);
            }
        }

        private void FindByValuePageChange(object sender, Code.PageEventArgs e)
        {
            if (Code.Direction.Up == e.PageDirection)
            {
                if (tabViews.SelectedIndex == 0)
                {
                    e.Index = sview1.GetPreviousPageStartIndex(e.Index);
                }
                else
                {
                    e.Index = tview1.GetPreviousPageStartIndex(e.Index);
                }
            }
            else
            {
                if (tabViews.SelectedIndex == 0)
                {
                    e.Index = sview1.GetNextPageStartIndex(e.Index);
                }
                else
                {
                    e.Index = tview1.GetNextPageStartIndex(e.Index);
                }
            }
        }

        // Handler for Zoom in/out
        private void ZoomChange(object sender, Code.ZoomEventArgs e)
        {
            Cursor = Cursors.Wait;
            tview1.Zoom(e.Multiple);
            sview1.Zoom(e.Multiple);
            Cursor = Cursors.Arrow;
        }

        private void ZoomLimitReached(object sender, Code.ZoomEventArgs e)
        {
            zoomToolbar.UpdateButtonState(e.EnableZoomIn, e.EnableZoomOut);
        }

        // Gets the application folder.
        public static string GetInstallDir()
        {
            // default to something reasonable in case registry key lookup fails
            string installDir = "C:\\Eclipse_ThreadX\\TraceX";

            string installPath = Path.GetDirectoryName(Application.ResourceAssembly.Location);
            if (!installPath.Contains("WindowsApps"))
            {
                // installed from script, just use the executable location as the install path
                return installPath;
            }

            string KeyName =
                "Software\\" +
                AzureRTOS.TraceManagement.Components.RecentFileList.ApplicationAttributes.CompanyName + "\\" +
                AzureRTOS.TraceManagement.Components.RecentFileList.ApplicationAttributes.ProductName;

            RegistryKey k = null;
            k = Registry.CurrentUser.OpenSubKey(KeyName);

            if (k != null)
            {
                installDir = (string)k.GetValue("InstallDir");
            }
            return installDir;
        }

        // Get the trace files folder.
        public static string GetExampleTraceFilesFolder()
        {
            string sourcePath = GetInstallDir();
            sourcePath += "\\TraceFiles";
            return sourcePath;
        }

        // Handler for file -> Exit.
        private void OnExit(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        // Handler for Help -> Manual.
        private void OnOpenManual(object sender, RoutedEventArgs e)
        {
            string url = "https://github.com/eclipse-threadx/rtos-docs";
            System.Diagnostics.Process.Start(url);
        }

        // Handler for Help -> About.
        private void OnAbout(object sender, RoutedEventArgs e)
        {
            new Splash
            {
                ShowCloseButton = true,
            }.ShowDialog();
        }

        private void OnValueSearch(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.FindByValue.Show(_traceFileInfo, Dialogs.FindByValue.FindByValueIndex, _threads));
            }
        }

        private void MiTickMapping_SubmenuClosed(object sender, RoutedEventArgs e)
        {
            string str = muSecondTextBox.Text.Trim();
            double Num;
            bool isNum = double.TryParse(str, out Num);

            if (muSecondTextBox.Text == "0")
            {
                Code.Event.SetRelativeTickMapping(0, false);
                tview1.DrawEventsOutside(0, false);
                sview1.drawEventsOutside(0, false);
                tbxDeltaTicks.Content = "0";
            }
            else if (muSecondTextBox.Text != "")
            {
                if (muSecondTextBox.Text.Contains("0x"))
                {
                    muSecondTextBox.Text.Remove(0, 2);
                    muSecondTextBox.Text = Convert.ToUInt32(muSecondTextBox.Text, 16).ToString(CultureInfo.CurrentCulture);
                    str = muSecondTextBox.Text.Trim();
                    isNum = double.TryParse(str, out Num);
                }

                if (isNum)
                {
                    int getLongest = 0;
                    int tmpLongest = 0;
                    for (int i = 0; i < _traceFileInfo.Events.Count; i++)
                    {
                        TmlEvent tEvent = _traceFileInfo.Events[i];

                        tmpLongest = tEvent.RelativeTicks.ToString(CultureInfo.InvariantCulture).Length;
                        if (tmpLongest > getLongest)
                        {
                            getLongest = tmpLongest;
                        }
                    }

                    if (getLongest < 10)
                    {
                        tbxDeltaTicks.Width = getLongest * 26;
                    }
                    else
                    {
                        tbxDeltaTicks.Width = 9 * 26;
                    }
                    tbxDeltaTicks.Content = "0 (0.00 µs)";
                    _ticksPerMicroSecondText = muSecondTextBox.Text;

                    tview1.DrawEventsOutside(Convert.ToDouble(muSecondTextBox.Text, CultureInfo.InvariantCulture), true);
                    sview1.drawEventsOutside(Convert.ToDouble(muSecondTextBox.Text, CultureInfo.InvariantCulture), true);
                }
                else
                {
                    muSecondTextBox.Text = "";
                }
            }
            else
            {
                Code.Event.SetRelativeTickMapping(0, false);
            }
        }

        // Handler for the window closing event handler.
        private void Window_Closed(object sender, EventArgs e)
        {
            if (App.HasOpenFile)
            {
                ELTMLManaged.TMLFunctions.Uninitialize();
            }
            Properties.Settings.Default.Save();
            base.Close();
            Application.Current.Shutdown();
        }

        // Handler for View -> Popular services.
        private void OnPopServ(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.PopularServices.Show(_traceFileInfo));
            }
        }

        // Handler for View -> Execution Profile.
        private void OnExecProf(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.ExecutionProfile.Show(_traceFileInfo));
            }
        }

        // Handler for View -> Stack Usage.
        private void OnStackUsage(object sender, RoutedEventArgs e)
        {
            _infoWindows.Add(Dialogs.StackUsage.ShowMe(_threads));
        }

        // Handler for View -> Performance Statistics.
        private void OnPerfStatistics(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.PerformStats.Show(_traceFileInfo, _threads));
            }
        }

        // Handler for View -> Trace File Info.
        private void OnTraceFileInfo(object sender, RoutedEventArgs e)
        {
            _infoWindows.Add(Dialogs.HeaderInfo.ShowMe());
        }

        private void OnCurrentEvent(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.CurrentEventInfo.Show(_traceFileInfo));
            }
        }

        private void OnFileXStatistics(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.FileXStatistics.Show(_traceFileInfo, _threads));
            }
        }

        private void OnNetXStatistics(object sender, RoutedEventArgs e)
        {
            if (null != _traceFileInfo)
            {
                _infoWindows.Add(Dialogs.NetXStatistics.Show(_traceFileInfo, _threads));
            }
        }

        // Handler for View -> Raw Trace Dump.
        private void OnRawDump(object sender, RoutedEventArgs e)
        {
            // Open file.  Set extension and filters.
            var sdlg = new SaveFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*", // Filter files by extension
                AddExtension = true,
                Title = "Where do you want to save the raw dump file?",
            };

            if (Properties.Settings.Default.FilesPath.Length == 0)
            {
                // Default directory to tracefiles folder.
                sdlg.InitialDirectory = GetExampleTraceFilesFolder();
                Properties.Settings.Default.FilesPath = sdlg.InitialDirectory;
            }
            else
            {
                // Else, use the last selected directory.
                sdlg.InitialDirectory = Properties.Settings.Default.FilesPath;
            }

            // Set flag to restore directory and get result if dialog is shown.
            sdlg.RestoreDirectory = true;
            bool? result = sdlg.ShowDialog();

            // Check if there is a reasonable result.
            if (result.HasValue && !result.Value)
            {
                return;
            }

            RawDumpTraceFile(sdlg.FileName);

            string sfileName = sdlg.FileName;
            var fi = new FileInfo(sfileName);
            if (fi.Exists)
            {
                Process.Start(fi.FullName);
            }
        }

        private void OnMapTime(object sender, RoutedEventArgs e)
        {
            muSecondTextBox.Focus();
        }

        private void OnOpenExecutionProfile(object sender, RoutedEventArgs e)
        {
            OnExecProf(sender, e);
        }

        private void OnOpenPerformanceStatistics(object sender, RoutedEventArgs e)
        {
            OnPerfStatistics(sender, e);
        }

        private void OnOpenThreadStackUsage(object sender, RoutedEventArgs e)
        {
            OnStackUsage(sender, e);
        }

        // Handler for View -> Legend.
        private void MiLegend_Click(object sender, RoutedEventArgs e)
        {
            var tx = "tx";
            _infoWindows.Add(Dialogs.Legend.Show(_traceFileInfo, tx));
        }

        private void FxLegend_Click(object sender, RoutedEventArgs e)
        {
            var fx = "fx";
            _infoWindows.Add(Dialogs.Legend.Show(_traceFileInfo, fx));
        }

        private void NxLegend_Click(object sender, RoutedEventArgs e)
        {
            _infoWindows.Add(Dialogs.Legend.Show(_traceFileInfo, "nx"));
        }

        private void UxLegend_Click(object sender, RoutedEventArgs e)
        {
            _infoWindows.Add(Dialogs.Legend.Show(_traceFileInfo, "ux"));
        }

        private void StatusLinesOff_Unchecked(object sender, RoutedEventArgs e)
        {
            statusLinesOff.IsChecked = !statusLinesAll.IsChecked
                && !statusLinesReady.IsChecked;

            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;

            RedrawThreads();
        }

        private void StatusLinesOff_Checked(object sender, RoutedEventArgs e)
        {
            statusLinesAll.IsChecked = false;
            statusLinesReady.IsChecked = false;
            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;

            RedrawThreads();
        }

        private void StatusLinesReady_Checked(object sender, RoutedEventArgs e)
        {
            statusLinesAll.IsChecked = false;
            statusLinesOff.IsChecked = false;
            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;

            RedrawThreads();
        }

        private void StatusLinesReady_Unchecked(object sender, RoutedEventArgs e)
        {
            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            statusLinesOff.IsChecked = !statusLinesReady.IsChecked
                && !statusLinesAll.IsChecked;

            RedrawThreads();
        }

        private void StatusLinesAll_Checked(object sender, RoutedEventArgs e)
        {
            statusLinesReady.IsChecked = false;
            statusLinesOff.IsChecked = false;
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            sview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;
            tview1.statusLinesReadyOnlySet = statusLinesReady.IsChecked;

            RedrawThreads();
        }

        private void StatusLinesAll_Unchecked(object sender, RoutedEventArgs e)
        {
            sview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            tview1.statusLinesAllOnSet = statusLinesAll.IsChecked;
            statusLinesOff.IsChecked = !statusLinesReady.IsChecked
                && !statusLinesAll.IsChecked;

            RedrawThreads();
        }

        // Handler for Tab View Selection Changed
        private void TabViews_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                if (e.AddedItems[0] is TabItem tabItem)
                {
                    string headerText = Convert.ToString(tabItem.Header, CultureInfo.CurrentCulture);
                    if (0 == string.Compare(headerText, "Time View", StringComparison.Ordinal)) // Time View
                    {
                        tbxDeltaTicks.Content = "0";

                        if (muSecondTextBox.Text == "0")
                        {
                            tbxDeltaTicks.Content = "0";
                        }
                        else if (muSecondTextBox.Text != "")
                        {
                            tbxDeltaTicks.Content = "0 (0.00 µs)";
                        }

                        zoomToolbar.InitializeZoomFactors(false, tview1.GetZoomFactor(), tview1.EnableZoomIn, tview1.EnableZoomOut);

                        _fileToolBarGroupWidth = tview1.GetLeftViewPortWidth();
                        SetToolbarLocation();
                        tview1.cleanDeltaMask();
                        tview1.RedrawThreads();
                        tview1.FindThreadOrderClean();

                        tview1.ActivateView(true);
                        sview1.ActivateView(false);

                        return;
                    }
                    else // Sequential View
                    {
                        tbxDeltaTicks.Content = "0";

                        if (muSecondTextBox.Text == "0")
                        {
                            tbxDeltaTicks.Content = "0";
                        }
                        else if (muSecondTextBox.Text != "")
                        {
                            tbxDeltaTicks.Content = "0 (0.00 µs)";
                        }
                        zoomToolbar.InitializeZoomFactors(true, sview1.GetZoomFactor(), sview1.EnableZoomIn, sview1.EnableZoomOut);

                        sview1.cleanDeltaMask();
                        _fileToolBarGroupWidth = sview1.GetLeftViewPortWidth();
                        SetToolbarLocation();
                        sview1.RedrawThreads();
                        sview1.ActivateView(true);
                        tview1.ActivateView(false);
                        sview1.findThreadOrderClean();
                    }
                }
            }
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckEventHeightChanged();
            SetToolbarLocation();
        }

        private void SetToolbarLocation()
        {
            //gridTraceXView.RowDefinitions[0].MinHeight = SystemFonts.MessageFontSize * 3;
            //gridTraceXView.RowDefinitions[0].MaxHeight = SystemFonts.MessageFontSize * 3;
            //gridTraceXView.RowDefinitions[0].MinHeight = 180;
            //gridTraceXView.RowDefinitions[0].MaxHeight = 180;

            double viewportWidth = ActualWidth - _fileToolBarGroupWidth;
            viewportWidth = viewportWidth < 0 ? 0 : viewportWidth;
            Canvas.SetLeft(canvasFiles, 0);

            Canvas.SetLeft(canvasZooms, _fileToolBarGroupWidth);

            double navLeft = _fileToolBarGroupWidth + (viewportWidth - canvasNavigator.Width) / 2;
            double minNavLeft = _fileToolBarGroupWidth + canvasZooms.Width;
            navLeft = navLeft > minNavLeft ? navLeft : minNavLeft;
            Canvas.SetLeft(canvasNavigator, navLeft);

            double deltaLeft = ActualWidth - canvasDelta.Width;
            double minDeltaLeft = minNavLeft + canvasNavigator.Width;
            deltaLeft = deltaLeft > minDeltaLeft ? deltaLeft : minDeltaLeft;
            Canvas.SetLeft(canvasDelta, deltaLeft);
        }

        public double GetEventDisplayHeight()
        {
            double size = SystemFonts.MessageFontSize;
            double height = size * 2;
            return height;
        }
 

        public bool CheckEventHeightChanged()
        {
            if (GetEventDisplayHeight() != oldEventHeight)
            {
                if (oldEventHeight != 0.0)
                {
                    oldEventHeight = GetEventDisplayHeight();
                    //gridTraceXView.ColumnDefinitions[0].Width = new GridLength(260 * oldEventHeight / 24);
                    gridTraceXView.ColumnDefinitions[0].Width = new GridLength(sview1.GetThreadNameDisplayWidth());
                    //ReopenFile();
                    RedrawThreads();
                    return true;
                }
                else
                {
                    oldEventHeight = GetEventDisplayHeight();
                    gridTraceXView.RowDefinitions[0].Height = new GridLength(oldEventHeight * 4);
                }
            }
            return false;
        }

        private void ChildSplitterPositionChanged(object sender, EventArgs e)
        {
            if (sender is Components.TimeView tView)
            {
                _fileToolBarGroupWidth = tView.GetLeftViewPortWidth();
            }
            else if (sender is Components.SequentialView sView)
            {
                _fileToolBarGroupWidth = sView.GetLeftViewPortWidth();
            }

            SetToolbarLocation();
        }

        private void RedrawThreads()
        {
            sview1.SetThreadOrderChanged();
            tview1.SetThreadOrderChanged();
            var tabItem = tabViews.Items[0] as TabItem;
            if (tabItem.IsSelected)
            {
                sview1.RedrawThreads();
            }
            else
            {
                tview1.RedrawThreads();
            }
        }
    }
}
