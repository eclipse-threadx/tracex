using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;


namespace AzureRTOS.TraceManagement
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Indexed command line args using hash table
        //public static Hashtable CommandLineArgs = new Hashtable();
        public static string TraceFile = string.Empty;
        public static bool HasOpenFile = false;

        private void AppStartup(object sender, StartupEventArgs e)
        {
            // Don't bother if no command line args were passed
            // NOTE: e.Args is never null - if no command line args were passed, 
            //       the length of e.Args is 0.
            if (e.Args.Length == 0) return;

            TraceFile = e.Args[0];
            if (TraceFile.IndexOf(":\\", StringComparison.Ordinal) == -1)
            {
                TraceFile = System.IO.Path.GetDirectoryName(Application.ResourceAssembly.Location) + "\\" + e.Args[0];
            }
        }
    }
}
