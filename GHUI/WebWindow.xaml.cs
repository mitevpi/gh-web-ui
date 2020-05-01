﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Grasshopper.Kernel;
using mshtml;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [ComVisible(true)]
    public partial class WebWindow : Window
    {
        // STUFF
        private IGH_DataAccess _gh = null;

        // GENERIC SETUP
        public event PropertyChangedEventHandler PropertyChanged;
        private static string Path => Assembly.GetExecutingAssembly().Location;
        private static readonly string Directory = System.IO.Path.GetDirectoryName(Path);

        // HTML QUERY
        private mshtml.HTMLDocument Doc => (mshtml.HTMLDocument) webBrowser1.Document;
        private mshtml.HTMLInputTextElement Input => Doc.getElementById("fname") as mshtml.HTMLInputTextElement;

        // HTML READ
        private string _htmlString2;
        private string _htmlStringContainer = "";
        private FileSystemWatcher _watcher;

        public string Value
        {
            get
            {
                OnPropertyChanged();
                return Input?.value;
            }
        }

        //public string Value => Input?.value;


        public WebWindow(IGH_DataAccess da)
        {
            _gh = da;
            InitializeComponent();
            _htmlString2 = ReadHtml();
            webBrowser1.NavigateToString(_htmlString2);
            MonitorTailOfFile();
            //HTMLInputTextElementEvents_onchangeEventHandler temp = (HTMLInputTextElementEvents_onchangeEventHandler) Input.onchange;
            //Input.onchange += inputOnChange();
        }


        private void OnchangeInput(object sender, EventArgs e)
        {
            //HtmlHandler htmlHandler = (HtmlHandler)sender;
            //IHTMLElement element = htmlHandler.SourceHTMLWindow.@event.srcElement;
        }

        private string ReadHtml()
        {
            string file = System.IO.Path.Combine(Directory, "Window.html");
            if (!File.Exists(file)) return _htmlStringContainer;
            _htmlStringContainer = file;

            return File.ReadAllText(_htmlStringContainer);
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //_gh.SetData(0, Value);
        }

        public void MonitorTailOfFile()
        {
            _watcher = new FileSystemWatcher
            {
                Path = Directory,
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName,
                Filter = "*.html"
            };
            _watcher.Changed += new FileSystemEventHandler(OnChanged);
            _watcher.EnableRaisingEvents = true;

        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            _watcher.Dispose();
            _watcher = null;
            _htmlString2 = ReadHtml();
            //webBrowser1.NavigateToString(HtmlString2);
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");
            MonitorTailOfFile();
        }
    }
}