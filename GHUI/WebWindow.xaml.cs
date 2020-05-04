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
        // GENERIC SETUP
        public event PropertyChangedEventHandler PropertyChanged;
        //private static string Path => Assembly.GetExecutingAssembly().Location;

        private string _path;
        private string Directory => Path.GetDirectoryName(_path);


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


        /// <summary>
        /// The WPF Container for the WebBrowser element which renders the user's HTML.
        /// </summary>
        /// <param name="path">Path of the HTML file to render as UI.</param>
        public WebWindow(string path)
        {
            _path = path;
            InitializeComponent();
            HtmlString = ReadHtml();
            MonitorTailOfFile();
            WebBrowser.NavigateToString(HtmlString);
            WebBrowser.LoadCompleted += BrowserLoaded;
        }

        /// <summary>
        /// Event handler for when the WPF Web Browser is loaded and initialized.
        /// </summary>
        private void BrowserLoaded(object o, EventArgs e)
        {
            // add click handler
            HTMLDocumentEvents2_Event iEvent = (HTMLDocumentEvents2_Event) Doc;
            iEvent.onclick += ClickEventHandler;
        }

        /// <summary>
        /// Event handler for clicking on the UI. Placeholder for "real" event
        /// listeners that would update values of input elements on this instance.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool ClickEventHandler(IHTMLEventObj e)
        {
            Debug.WriteLine("CLICKED");
            return true;
        }

        /// <summary>
        /// Read the HTML as raw text.
        /// </summary>
        /// <returns>Raw text of the HTML UI</returns>
        private string ReadHtml()
        {
            if (_path != null)
            {
                return File.ReadAllText(_path);
            }

            string file = Path.Combine(Directory, "Window.html");
            return !File.Exists(file) ? "" : File.ReadAllText(file);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //_gh.SetData(0, Value);
        }

        /// <summary>
        /// Initialize watching for changes of the HTML file so it can be re-rendered.
        /// </summary>
        private void MonitorTailOfFile()
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

        /// <summary>
        /// Method handler for when a change is detected in the HTML file.
        /// </summary>
        private void OnHtmlChanged(object source, FileSystemEventArgs e)
        {
            // Destroy the watcher
            _watcher.Dispose();
            _watcher = null;

            // Wait 1 sec (hack-y preventing of thread conflicts by accessing the same file at 
            // the same time (Watcher and File Reader).
            Thread.Sleep(1000);
            Dispatcher.Invoke(() =>
            {
                // Reread the HTML, render it, and start another FileWatcher
                HtmlString = ReadHtml();
                WebBrowser.NavigateToString(HtmlString);
                MonitorTailOfFile();
            });
        }
    }
}