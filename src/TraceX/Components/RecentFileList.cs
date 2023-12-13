using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace AzureRTOS.TraceManagement.Components
{
    public class RecentFileList : Separator
    {
        public interface IPersist
        {
            List<string> RecentFiles(int max);
            void InsertFile(string filepath, int max);
            void RemoveFile(string filepath, int max);
        }

        public IPersist Persister { get; set; }

        public void UseRegistryPersister() { Persister = new RegistryPersister(); }
        public void UseRegistryPersister(string key) { Persister = new RegistryPersister(key); }

        public int MaxNumberOfFiles { get; set; }
        public int MaxPathLength { get; set; }
        public MenuItem FileMenu { get; private set; }

        /// <summary>
        /// Used in: String.Format(CultureInfo.CurrentCulture, MenuItemFormat, index, filepath, displayPath );
        /// Default = "_{0}:  {2}"
        /// </summary>
        public string MenuItemFormatOneToNine { get; set; }

        /// <summary>
        /// Used in: String.Format(CultureInfo.CurrentCulture, MenuItemFormat, index, filepath, displayPath );
        /// Default = "{0}:  {2}"
        /// </summary>
        public string MenuItemFormatTenPlus { get; set; }

        public delegate string GetMenuItemTextDelegate(int index, string filepath);
        public GetMenuItemTextDelegate GetMenuItemTextHandler { get; set; }

        public event EventHandler<MenuClickEventArgs> MenuClick;

        Separator _Separator = null;
        List<RecentFile> _RecentFiles = null;

        public RecentFileList()
        {
            Persister = new RegistryPersister();

            MaxNumberOfFiles = 9;
            MaxPathLength = 50;
            MenuItemFormatOneToNine = "_{0}:  {2}";
            MenuItemFormatTenPlus = "{0}:  {2}";

            this.Loaded += (s, e) => HookFileMenu();
        }

        void HookFileMenu()
        {
            MenuItem parent = Parent as MenuItem;
            if (parent == null) throw new ApplicationException("Parent must be a MenuItem");

            if (FileMenu == parent) return;

            if (FileMenu != null) FileMenu.SubmenuOpened -= _FileMenu_SubmenuOpened;

            FileMenu = parent;
            FileMenu.SubmenuOpened += _FileMenu_SubmenuOpened;
        }

        public List<string> RecentFiles { get { return Persister.RecentFiles(MaxNumberOfFiles); } }
        public void RemoveFile(string filepath) { Persister.RemoveFile(filepath, MaxNumberOfFiles); }
        public void InsertFile(string filepath) { Persister.InsertFile(filepath, MaxNumberOfFiles); }

        void _FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            SetMenuItems();
        }

        void SetMenuItems()
        {
            RemoveMenuItems();

            LoadRecentFiles();

            InsertMenuItems();
        }

        void RemoveMenuItems()
        {
            if (_Separator != null) FileMenu.Items.Remove(_Separator);

            if (_RecentFiles != null)
                foreach (RecentFile r in _RecentFiles)
                    if (r.MenuItem != null)
                        FileMenu.Items.Remove(r.MenuItem);

            _Separator = null;
            _RecentFiles = null;
        }

        void InsertMenuItems()
        {
            if (_RecentFiles == null) return;
            if (_RecentFiles.Count == 0) return;

            int iMenuItem = FileMenu.Items.IndexOf(this);
            foreach (RecentFile r in _RecentFiles)
            {
                string header = GetMenuItemText(r.Number + 1, r.Filepath, r.DisplayPath);

                r.MenuItem = new MenuItem { Header = header };
                r.MenuItem.Click += MenuItem_Click;

                FileMenu.Items.Insert(++iMenuItem, r.MenuItem);
            }

            _Separator = new Separator();
            FileMenu.Items.Insert(++iMenuItem, _Separator);
        }

        string GetMenuItemText(int index, string filepath, string displaypath)
        {
            GetMenuItemTextDelegate delegateGetMenuItemText = GetMenuItemTextHandler;
            if (delegateGetMenuItemText != null) return delegateGetMenuItemText(index, filepath);

            string format = (index < 10 ? MenuItemFormatOneToNine : MenuItemFormatTenPlus);

            string shortPath = ShortenPathname(displaypath, MaxPathLength);

            return String.Format(CultureInfo.CurrentCulture, format, index, filepath, shortPath);
        }

        // This method is taken from Joe Woodbury's article at: http://www.codeproject.com/KB/cs/mrutoolstripmenu.aspx

        /// <summary>
        /// Shortens a pathname for display purposes.
        /// </summary>
        /// <param labelName="pathname">The pathname to shorten.</param>
        /// <param labelName="maxLength">The maximum number of characters to be displayed.</param>
        /// <remarks>Shortens a pathname by either removing consecutive components of a path
        /// and/or by removing characters from the end of the filename and replacing
        /// then with three elipses (...)
        /// <para>In all cases, the root of the passed path will be preserved in it's entirety.</para>
        /// <para>If a UNC path is used or the pathname and maxLength are particularly short,
        /// the resulting path may be longer than maxLength.</para>
        /// <para>This method expects fully resolved pathnames to be passed to it.
        /// (Use Path.GetFullPath() to obtain this.)</para>
        /// </remarks>
        /// <returns></returns>
        static public string ShortenPathname(string pathname, int maxLength)
        {
            if (pathname.Length <= maxLength)
                return pathname;

            string root = Path.GetPathRoot(pathname);
            if (root.Length > 3)
                root += Path.DirectorySeparatorChar;

            string[] elements = pathname.Substring(root.Length).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            int filenameIndex = elements.GetLength(0) - 1;

            if (elements.GetLength(0) == 1) // pathname is just a root and filename
            {
                if (elements[0].Length > 5) // long enough to shorten
                {
                    // if path is a UNC path, root may be rather long
                    if (root.Length + 6 >= maxLength)
                    {
                        return root + elements[0].Substring(0, 3) + "...";
                    }
                    else
                    {
                        return pathname.Substring(0, maxLength - 3) + "...";
                    }
                }
            }
            else if ((root.Length + 4 + elements[filenameIndex].Length) > maxLength) // pathname is just a root and filename
            {
                root += "...\\";

                int len = elements[filenameIndex].Length;
                if (len < 6)
                    return root + elements[filenameIndex];

                if ((root.Length + 6) >= maxLength)
                {
                    len = 3;
                }
                else
                {
                    len = maxLength - root.Length - 3;
                }
                return root + elements[filenameIndex].Substring(0, len) + "...";
            }
            else if (elements.GetLength(0) == 2)
            {
                return root + "...\\" + elements[1];
            }
            else
            {
                int len = 0;
                int begin = 0;

                for (int i = 0; i < filenameIndex; i++)
                {
                    if (elements[i].Length > len)
                    {
                        begin = i;
                        len = elements[i].Length;
                    }
                }

                int totalLength = pathname.Length - len + 3;
                int end = begin + 1;

                while (totalLength > maxLength)
                {
                    if (begin > 0)
                        totalLength -= elements[--begin].Length - 1;

                    if (totalLength <= maxLength)
                        break;

                    if (end < filenameIndex)
                        totalLength -= elements[++end].Length - 1;

                    if (begin == 0 && end == filenameIndex)
                        break;
                }

                // assemble final string

                for (int i = 0; i < begin; i++)
                {
                    root += elements[i] + '\\';
                }

                root += "...\\";

                for (int i = end; i < filenameIndex; i++)
                {
                    root += elements[i] + '\\';
                }

                return root + elements[filenameIndex];
            }
            return pathname;
        }

        void LoadRecentFiles()
        {
            _RecentFiles = LoadRecentFilesCore();
        }

        List<RecentFile> LoadRecentFilesCore()
        {
            List<string> list = RecentFiles;

            List<RecentFile> files = new List<RecentFile>(list.Count);

            int i = 0;
            foreach (string filepath in list)
                files.Add(new RecentFile(i++, filepath));

            return files;
        }

        private class RecentFile
        {
            public int Number = 0;
            public string Filepath = "";
            public MenuItem MenuItem = null;

            public string DisplayPath
            {
                get
                {
                    return Path.Combine(
                        Path.GetDirectoryName(Filepath),
                        Path.GetFileNameWithoutExtension(Filepath));
                }
            }

            public RecentFile(int number, string filepath)
            {
                this.Number = number;
                this.Filepath = filepath;
            }
        }

        public class MenuClickEventArgs : EventArgs
        {
            public string Filepath { get; private set; }

            public MenuClickEventArgs(string filepath)
            {
                this.Filepath = filepath;
            }
        }

        void MenuItem_Click(object sender, EventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;

            OnMenuClick(menuItem);
        }

        protected virtual void OnMenuClick(MenuItem menuItem)
        {
            string filepath = GetFilepath(menuItem);

            if (String.IsNullOrEmpty(filepath)) return;

            EventHandler<MenuClickEventArgs> dMenuClick = MenuClick;
            if (dMenuClick != null) dMenuClick(menuItem, new MenuClickEventArgs(filepath));
        }

        string GetFilepath(MenuItem menuItem)
        {
            foreach (RecentFile r in _RecentFiles)
                if (r.MenuItem == menuItem)
                    return r.Filepath;

            return String.Empty;
        }

        //-----------------------------------------------------------------------------------------

        public static class ApplicationAttributes
        {
            static readonly Assembly _Assembly = null;

            static readonly AssemblyTitleAttribute _Title = null;
            static readonly AssemblyCompanyAttribute _Company = null;
            static readonly AssemblyCopyrightAttribute _Copyright = null;
            static readonly AssemblyProductAttribute _Product = null;

            public static string Title { get; private set; }
            public static string CompanyName { get; private set; }
            public static string Copyright { get; private set; }
            public static string ProductName { get; private set; }

            static Version _Version = null;
            public static string Version { get; private set; }

            static ApplicationAttributes()
            {
                try
                {
                    Title = String.Empty;
                    CompanyName = String.Empty;
                    Copyright = String.Empty;
                    ProductName = String.Empty;
                    Version = String.Empty;

                    _Assembly = Assembly.GetEntryAssembly();

                    if (_Assembly != null)
                    {
                        object[] attributes = _Assembly.GetCustomAttributes(false);

                        foreach (object attribute in attributes)
                        {
                            Type type = attribute.GetType();

                            if (type == typeof(AssemblyTitleAttribute)) _Title = (AssemblyTitleAttribute)attribute;
                            if (type == typeof(AssemblyCompanyAttribute)) _Company = (AssemblyCompanyAttribute)attribute;
                            if (type == typeof(AssemblyCopyrightAttribute)) _Copyright = (AssemblyCopyrightAttribute)attribute;
                            if (type == typeof(AssemblyProductAttribute)) _Product = (AssemblyProductAttribute)attribute;
                        }

                        _Version = _Assembly.GetName().Version;
                    }

                    if (_Title != null) Title = _Title.Title;
                    if (_Company != null) CompanyName = _Company.Company;
                    if (_Copyright != null) Copyright = _Copyright.Copyright;
                    if (_Product != null) ProductName = _Product.Product;
                    if (_Version != null) Version = _Version.ToString();
                }
                catch { }
            }
        }


        //-----------------------------------------------------------------------------------------

        private class RegistryPersister : IPersist
        {
            private bool recentListCleaned = false;

            public string RegistryKey { get; set; }

            public RegistryPersister()
            {
                RegistryKey =
                    "Software\\" +
                    ApplicationAttributes.CompanyName + "\\" +
                    ApplicationAttributes.ProductName + "\\" +
                    "RecentFileList";
            }

            public RegistryPersister(string key)
            {
                RegistryKey = key;
            }

            string Key(int i) { return i.ToString("00", CultureInfo.InvariantCulture); }

            public List<string> RecentFiles(int max)
            {
                string filename;
                RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey);
                if (k == null) k = Registry.CurrentUser.CreateSubKey(RegistryKey);

                // Loop to remove old references to "Roaming" directory:
                if (!recentListCleaned)
                {
                    for (int i = 0; i < max; i++)
                    {
                        filename = (string)k.GetValue(Key(i));
                        if (String.IsNullOrEmpty(filename)) break;
                        if (filename.Contains("Roaming"))
                        {
                            RemoveFile(filename, max);
                        }
                    }
                    recentListCleaned = true;
                }
                // If the list is empty, or if the list contains the old
                // AppData/Roaming directory, repopulate the list:

                filename = (string)k.GetValue(Key(0));
                if (String.IsNullOrEmpty(filename))
                {
                    // Query the installation directory to find the example
                    // trace files that have been installed

                    string sourcePath = TraceXView.GetExampleTraceFilesFolder();

                    if (Directory.Exists(sourcePath))
                    {
                        string[] files = Directory.GetFiles(sourcePath);
                        foreach (string name in files)
                        {
                            InsertFile(name, max);
                        }
                    }
                }

                List<string> list = new List<string>(max);

                for (int i = 0; i < max; i++)
                {
                    filename = (string)k.GetValue(Key(i));

                    if (String.IsNullOrEmpty(filename)) break;

                    list.Add(filename);
                }

                return list;
            }


            public void InsertFile(string filepath, int max)
            {
                RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey);
                if (k == null) Registry.CurrentUser.CreateSubKey(RegistryKey);
                k = Registry.CurrentUser.OpenSubKey(RegistryKey, true);

                RemoveFile(filepath, max);

                for (int i = max - 2; i >= 0; i--)
                {
                    string sThis = Key(i);
                    string sNext = Key(i + 1);

                    object oThis = k.GetValue(sThis);
                    if (oThis == null) continue;

                    k.SetValue(sNext, oThis);
                }

                k.SetValue(Key(0), filepath);
            }

            public void RemoveFile(string filepath, int max)
            {
                RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey);
                if (k == null) return;

                for (int i = 0; i < max; i++)
                {
again:
                    string s = (string)k.GetValue(Key(i));
                    if (s != null && s.Equals(filepath, StringComparison.CurrentCultureIgnoreCase))
                    {
                        RemoveFile(i, max);
                        goto again;
                    }
                }
            }

            void RemoveFile(int index, int max)
            {
                RegistryKey k = Registry.CurrentUser.OpenSubKey(RegistryKey, true);
                if (k == null) return;

                k.DeleteValue(Key(index), false);

                for (int i = index; i < max - 1; i++)
                {
                    string sThis = Key(i);
                    string sNext = Key(i + 1);

                    object oNext = k.GetValue(sNext);
                    if (oNext == null) break;

                    k.SetValue(sThis, oNext);
                    k.DeleteValue(sNext);
                }
            }
        }

        //-----------------------------------------------------------------------------------------

    }
}
