using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GHUI.Classes.Models;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Newtonsoft.Json;
using Timer = System.Timers.Timer;

namespace GHUI.Classes
{
    public class WebView2Wrapper
    {
        /// <summary>
        /// The path of the HTML file which is being served as the user interface.
        /// </summary>
        private string _htmlPath;

        /// <summary>
        /// The dispatcher handling the execution of the WPF host Window.
        /// </summary>
        private Dispatcher _dispatcher;

        /// <summary>
        /// The directory where the HTML file used for the UI lives.
        /// </summary>
        private string Directory => _dispatcher.Invoke(() => Path.GetDirectoryName(_htmlPath));

        /// <summary>
        /// A special "temp" folder where WebView2Wrapper does the execution. This should be created in the
        /// Grasshopper/Libraries directory.
        /// </summary>
        private string ExecutingLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp";

        /// <summary>
        /// The JS script to be injected at runtime to query the DOM for changes
        /// as the user interacts with the inputs.
        /// </summary>
        private string DomQueryScript => File.ReadAllText(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) +
            "\\QueryInputElementsInDOM.js");

        /// <summary>
        ///The WebView2Wrapper instance which is being executed in this component.
        /// </summary>
        private Microsoft.Web.WebView2.Wpf.WebView2 _webView;

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

        /// <summary>
        /// Timer for scheduling the recompute on the DOM query so the user can get
        /// "real-time" inputs. TODO: fix to query only on DOM change.
        /// </summary>
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

        public List<string> InputNames => _domInputModels?.Select(s => s.name).ToList();
        public List<string> InputTypes => _domInputModels?.Select(s => s.type).ToList();

        public WebView2Wrapper(string htmlPath, Dispatcher dispatcher)
        {
            _htmlPath = htmlPath;
            _dispatcher = dispatcher;
            InitializeTimer();
        }

        /// <summary>
        /// Run the DOM Query script (JS) to get all the input elements.
        /// </summary>
        private async void RunDomInputQuery()
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
        /// Initialize the DevTools helper class.
        /// </summary>
        public void InitializeDevToolsProtocolHelper()
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
        /// Programatically initialize the WebView2Wrapper component.
        /// </summary>
        public async void InitializeWebView(DockPanel docker)
        {
            _webView = new Microsoft.Web.WebView2.Wpf.WebView2();

            // clear everything in the WPF dock panel container
            docker.Children.Clear();
            docker.Children.Add(_webView);

            // initialize the webview 2 instance
            try
            {
                var env = await CoreWebView2Environment.CreateAsync(null, ExecutingLocation);
                await _webView.EnsureCoreWebView2Async(env);
                _webView.CoreWebView2InitializationCompleted += Navigate;
                //InitializeDevToolsProtocolHelper();
                //SubscribeToDocumentUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Run the DOM query method every tick of the timer.
        /// </summary>
        private void DisplayTimeEvent(object source, ElapsedEventArgs e)
        {
            _dispatcher.BeginInvoke(new Action(() => { RunDomInputQuery(); }));
        }

        /// <summary>
        /// Subscribe to the DocumentUpdated event of WebView2Wrapper.
        /// </summary>
        public async void SubscribeToDocumentUpdated()
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
            _dispatcher.BeginInvoke(new Action(() =>
            {
                _htmlPath = newPath;
                _webView.Source = new Uri(_htmlPath);
            }));
        }

        /// <summary>
        /// Initialize a file-watcher object on the HTML file being used.
        /// </summary>
        public void ListenHtmlChange()
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
            _dispatcher.Invoke(() => { _webView.Reload(); });
        }
    }
}