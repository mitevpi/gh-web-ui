using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
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
        private string _htmlPath;
        // GENERIC SETUP
        public event PropertyChangedEventHandler PropertyChanged;
        private static string Path => Assembly.GetExecutingAssembly().Location;
        private static readonly string Directory = System.IO.Path.GetDirectoryName(Path);

        // HTML QUERY
        private HTMLDocument Doc => (HTMLDocument) WebBrowser.Document;
        private IHTMLInputTextElement Input => Doc.getElementById("fname") as IHTMLInputTextElement;

        // HTML READ
        private FileSystemWatcher _watcher;

        /// HTML STRING
        public string HtmlString { get; set; }

        /// HTML VALUE
        public string Value
        {
            get
            {
                OnPropertyChanged();
                return Input?.value;
            }
        }


        public WebWindow(string path)
        {
            _htmlPath = path;
            InitializeComponent();
            HtmlString = ReadHtml();
            MonitorTailOfFile();
            WebBrowser.NavigateToString(HtmlString);
            WebBrowser.LoadCompleted += BrowserLoaded;
        }

        private void BrowserLoaded(object o, EventArgs e)
        {
            // add click handler
            HTMLDocumentEvents2_Event iEvent = (HTMLDocumentEvents2_Event) Doc;
            iEvent.onclick += ClickEventHandler;
        }

        private bool ClickEventHandler(IHTMLEventObj e)
        {
            Debug.WriteLine("CLICKED");
            return true;
        }

        private string ReadHtml()
        {
            if (_htmlPath != null)
            {
                return File.ReadAllText(_htmlPath);
            }

            string file = System.IO.Path.Combine(Directory, "Window.html");
            return !File.Exists(file) ? "" : File.ReadAllText(file);
        }

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
            _watcher.Changed += OnHtmlChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnHtmlChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            _watcher.Dispose();
            _watcher = null;
            Thread.Sleep(1000);
            Dispatcher.Invoke(() =>
            {
                HtmlString = ReadHtml();
                WebBrowser.NavigateToString(HtmlString);
                MonitorTailOfFile();
            });
        }
    }
}