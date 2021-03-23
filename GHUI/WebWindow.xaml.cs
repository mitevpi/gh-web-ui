using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using GHUI.Models;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for the WPF WebBrowser
    /// </summary>
    //[ComVisible(true)]
    public partial class WebWindow : Window
    {
        /// <summary>
        /// The path of the HTML file which is being served as the user interface.
        /// </summary>
        private string _htmlPath;

        /// <summary>
        /// The directory where the HTML file used for the UI lives.
        /// </summary>
        private string Directory => Dispatcher.Invoke(() => Path.GetDirectoryName(_htmlPath));

        /// <summary>
        /// A special "temp" folder where WebView2 does the execution. This should be created in the
        /// Grasshopper/Libraries directory.
        /// </summary>
        private string ExecutingLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp";

        private string DomQueryScript => File.ReadAllText(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
            "\\QueryInputElementsInDOM.js");

        /// <summary>
        ///The WebView2 instance which is being executed in this component.
        /// </summary>
        private WebView2 _webView;

        /// <summary>
        /// Class for watching file changes in the source HTML file. Allows for reload triggering
        /// when the file is updated.
        /// </summary>
        private FileSystemWatcher _watcher;

        /// <summary>
        /// A collection of DomInputModel classes - representing the relevant data from the HTML
        /// `input` elements. 
        /// </summary>
        private List<DomInputModel> _domInputModels;

        /// <summary>
        /// Developer-focused tooling that exposes some more DOM-specific events and utilities.
        /// </summary>
        private DevToolsProtocolHelper _cdpHelper;

        private Timer _timer;

        // PUBLIC FIELDS
        /// <summary>
        /// List of the current values of all the input elements in the DOM.
        /// </summary>
        public List<string> InputValues => _domInputModels?.Select(s => s.value).ToList();

        /// <summary>
        /// List of the id properties of all the input elements in the DOM.
        /// </summary>
        public List<string> InputIds => _domInputModels?.Select(s => s.id).ToList();

        /// <summary>
        /// The WPF Container for the WebBrowser element which renders the user's HTML.
        /// </summary>
        /// <param name="htmlPath">Path of the HTML file to render as UI.</param>
        public WebWindow(string htmlPath)
        {
            _htmlPath = htmlPath;
            InitializeComponent();
            InitializeWebView();
            _webView.CoreWebView2InitializationCompleted += Navigate;
            ListenHtmlChange();
        }


        /// <summary>
        /// Run the DOM Query script (JS) to get all the input elements.
        /// </summary>
        async void RunDomInputQuery()
        {
            string scriptResult = await _webView.ExecuteScriptAsync(DomQueryScript);

            dynamic deserializedDomModels = JsonConvert.DeserializeObject(scriptResult);
            List<DomInputModel> domInputModels = new List<DomInputModel>();
            foreach (var s in deserializedDomModels)
            {
                DomInputModel domInputModel = JsonConvert.DeserializeObject<DomInputModel>(s.ToString());
                domInputModels.Add(domInputModel);
            }

            _domInputModels = domInputModels;
        }

        /// <summary>
        /// Initialize the timer which will query the DOM at the specified interval.
        /// TODO: Figure out how to run this only when the user interacts with the DOM.
        /// </summary>
        private void InitializeTimer()
        {
            _timer = new Timer();
            _timer.Elapsed += DisplayTimeEvent;
            _timer.Interval = 1000; // 1000 ms is one second
            _timer.Start();
        }

        /// <summary>
        /// Run the DOM query method every tick of the timer.
        /// </summary>
        private void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            Dispatcher.BeginInvoke(new Action(() => { RunDomInputQuery(); }));
        }

        /// <summary>
        /// Subscribe to the DocumentUpdated event of WebView2.
        /// </summary>
        private async void SubscribeToDocumentUpdated()
        {
            await _cdpHelper.DOM.EnableAsync();
            _cdpHelper.DOM.DocumentUpdated += OnDocumentUpdated;
        }

        /// <summary>
        /// What to do when the Document is updated. Currently somewhat redundant functionality with
        /// the FileWatcher doing the same essentially.
        /// </summary>
        private void OnDocumentUpdated(object sender, DOM.DocumentUpdatedEventArgs args)
        {
            InitializeTimer();
            //RunDomInputQuery();
        }

        /// <summary>
        /// Initialize the DevTools helper class.
        /// </summary>
        private void InitializeDevToolsProtocolHelper()
        {
            if (_webView == null || _webView.CoreWebView2 == null)
            {
                throw new Exception("Initialize WebView before using DevToolsProtocolHelper.");
            }

            if (_cdpHelper == null)
            {
                _cdpHelper = _webView.CoreWebView2.GetDevToolsProtocolHelper();
            }
        }

        /// <summary>
        /// Programatically initialize the WebView2 component.
        /// </summary>
        private async void InitializeWebView()
        {
            _webView = new WebView2();

            // clear everything in the WPF dock panel container
            Docker.Children.Clear();
            Docker.Children.Add(_webView);

            // initialize the webview 2 instance
            try
            {
                var env = await CoreWebView2Environment.CreateAsync(null, ExecutingLocation);
                await _webView.EnsureCoreWebView2Async(env);
                InitializeDevToolsProtocolHelper();
                SubscribeToDocumentUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Navigate to a new HTML file path.
        /// </summary>
        private void Navigate(object o, EventArgs e)
        {
            if (_webView?.CoreWebView2 != null)
            {
                _webView.Source = new Uri(_htmlPath);
            }
        }

        /// <summary>
        /// Navigate to a new HTML file path.
        /// </summary>
        /// <param name="newPath">The file path of the new HTML file to load.</param>
        public void Navigate(string newPath)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _htmlPath = newPath;
                _webView.Source = new Uri(_htmlPath);
            }));
        }

        /// <summary>
        /// Initialize a file-watcher object on the HTML file being used.
        /// </summary>
        private void ListenHtmlChange()
        {
            _watcher = new FileSystemWatcher(Directory)
            {
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.CreationTime
                               | NotifyFilters.Size
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.Attributes
                               | NotifyFilters.Security,
                //Filter = "*.html"
            };
            _watcher.Changed += OnHtmlChanged;
            _watcher.EnableRaisingEvents = true;
            _watcher.IncludeSubdirectories = true;
        }

        /// <summary>
        /// Method handler for when a change is detected in the HTML file. This method will
        /// trigger a reload on the HTML file.
        /// </summary>
        private void OnHtmlChanged(object source, FileSystemEventArgs e)
        {
            Dispatcher.Invoke(() => { _webView.Reload(); });
        }
    }
}