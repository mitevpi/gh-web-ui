using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using GHUI.Classes.Models;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Core.DevToolsProtocolExtension;
using Newtonsoft.Json;

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
        private string DomQueryScript => Properties.Resources.QueryInputElementsInDOM;

        private string ListenerScript => Properties.Resources.AddDocumentClickListener;

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
                await _webView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(ListenerScript);
                _webView.CoreWebView2InitializationCompleted += Navigate;
                _webView.WebMessageReceived += OnWebViewInteraction;
                _webView.NavigationCompleted += OnWebViewNavigationCompleted;
                //InitializeDevToolsProtocolHelper();
                //SubscribeToDocumentUpdated();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void RunDomInputQuery(DomClickModel clickModel)
        {
            await RunDomInputQuery();
            // handle output for buttons
            if (clickModel.targetType == "button")
            {
                HandleButtonClick(clickModel);
            }
        }

        /// <summary>
        /// Run the DOM Query script (JS) to get all the input elements.
        /// </summary>
        private async Task RunDomInputQuery()
        {
            // get the results of the DOM `input` element query script, and abort if none found
            string scriptResult = await _webView.ExecuteScriptAsync(DomQueryScript);
            dynamic deserializedDomModels = JsonConvert.DeserializeObject(scriptResult);
            if (deserializedDomModels == null) return;

            _domInputModels = new List<DomInputModel>();
            foreach (dynamic s in deserializedDomModels)
            {
                DomInputModel domInputModel = JsonConvert.DeserializeObject<DomInputModel>(s.ToString());
                _domInputModels.Add(domInputModel);
            }
        }

        /// <summary>
        /// Queries the DOM when the interface is first loaded and also when it is Reloaded due to
        /// a change in the HTML.
        /// </summary>
        private void OnWebViewNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            RunDomInputQuery();
        }

        /// <summary>
        /// What to do when the listener script returns a value.
        /// </summary>
        private void OnWebViewInteraction(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            DomClickModel clickData = JsonConvert.DeserializeObject<DomClickModel>(e.WebMessageAsJson);
            _dispatcher.BeginInvoke(new Action(() => { RunDomInputQuery(clickData); }));
        }

        // TODO: OUTPUT RESULT OF BUTTON CLICK
        private void HandleButtonClick(DomClickModel clickModel)
        {
            //if (clickModel.targetType != "button") return;
            // TODO: need to ensure that there is a unique id for each button, even when users
            // are not using the id/name feature correctly. for now we loop over all the possible buttons
            var clickedButtons = _domInputModels.Where(m => m.type == clickModel.targetType &&
                                                            m.id == clickModel.targetId ||
                                                            m.name == clickModel.targetName);
            //if (clickedButtons == null) return;
            foreach (DomInputModel domInput in clickedButtons)
            {
                domInput.value = "true";
            }
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
        /// Initialize a file-watcher object on the HTML file being used.
        /// </summary>
        public void SubscribeToHtmlChanged()
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
            _dispatcher.Invoke(() =>
            {
                //RunDomInputQuery();
                _webView.Reload();
            });
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
    }
}