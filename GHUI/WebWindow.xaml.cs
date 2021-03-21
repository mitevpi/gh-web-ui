using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for the WPF WebBrowser
    /// </summary>
    //[ComVisible(true)]
    public partial class WebWindow : Window
    {
        private string _htmlPath;
        private string Directory => Dispatcher.Invoke(() => Path.GetDirectoryName(_htmlPath));
        private string ExecutingLocation => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp";
        private WebView2 _webView;
        private FileSystemWatcher _watcher;

        /// HTML VALUE
        /// <summary>
        /// List of the current values of all the input elements in the DOM.
        /// </summary>
        public List<string> InputValues;

        /// <summary>
        /// List of the id properties of all the input elements in the DOM.
        /// </summary>
        public List<string> InputIds;


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
            Dispatcher.Invoke(() =>
            {
                _htmlPath = newPath;
                _webView.Source = new Uri(_htmlPath);
            });
        }

        /// <summary>
        /// Reload the current HTML file.
        /// </summary>
        private void Refresh(object o, EventArgs e)
        {
            _webView.Reload();
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
            Dispatcher.Invoke(() =>
            {
                _webView.Reload();
            });
        }
    }
}