using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using mshtml;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for the WPF WebBrowser
    /// </summary>
    [ComVisible(true)]
    public partial class WebWindow : Window
    {
        // GENERIC SETUP
        public event PropertyChangedEventHandler PropertyChanged;
        //private static string Path => Assembly.GetExecutingAssembly().Location;

        private readonly string _path;
        private string Directory => Dispatcher.Invoke(() => Path.GetDirectoryName(_path));


        // HTML QUERY
        private HTMLDocument Doc => Dispatcher.Invoke(() => (HTMLDocument) WebBrowser.Document);
        private IHTMLElementCollection DocElements => Dispatcher.Invoke(() => Doc.getElementsByTagName("HTML"));
        private IHTMLElementCollection DocInputElements => Dispatcher.Invoke(() => Doc.getElementsByTagName("input"));

        // HTML READ
        private FileSystemWatcher _watcher;
        private string HtmlString { get; set; }

        /// HTML VALUE
        /// <summary>
        /// List of the current values of all the input elements in the DOM.
        /// </summary>
        public List<string> InputValues => Dispatcher.Invoke(GetInputValues);

        /// <summary>
        /// List of the id properties of all the input elements in the DOM.
        /// </summary>
        public List<string> InputIds => Dispatcher.Invoke(GetInputIds);


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

        private List<string> GetInputValues()
        {
            return (from HTMLInputElement vElement in DocInputElements
                select vElement.value).ToList();
        }

        private List<string> GetInputIds()
        {
            return (from HTMLInputElement vElement in DocInputElements
                select vElement.id).ToList();
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
            Debug.WriteLine("CLICK");

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

        //protected void OnPropertyChanged([CallerMemberName] string name = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //    //_gh.SetData(0, Value);
        //}

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
            Thread.Sleep(500);
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