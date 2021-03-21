using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
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
        private readonly string _executingLocation = "";
        private WebView2 _webView;


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
            _executingLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp";
            InitializeComponent();
            InitializeWebView();
            _webView.CoreWebView2InitializationCompleted += Navigate;
            _webView.SourceUpdated += Refresh;
            _webView.SourceChanged += Refresh;
        }

        private async Task InitializeWebViewAsync(WebView2 webView)
        {
            try
            {
                var env = await CoreWebView2Environment.CreateAsync(null, _executingLocation);
                await webView.EnsureCoreWebView2Async(env);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void InitializeWebView()
        {
            _webView = new WebView2();

            this.Docker.Children.Clear();
            this.Docker.Children.Add(_webView);

            await InitializeWebViewAsync(_webView);
        }

        private void Navigate(object o, EventArgs e)
        {
            if (_webView != null && _webView.CoreWebView2 != null)
            {
                //navigate to website 
                _webView.Source = new Uri(_htmlPath);
            }
        }

        public void Navigate(string newPath)
        {
            Dispatcher.Invoke(() =>
            {
                _htmlPath = newPath;
                _webView.Source = new Uri(_htmlPath);
            });
        }

        private void Refresh(object o, EventArgs e)
        {
            _webView.Reload();
        }

        public void Refresh()
        {
            Dispatcher.Invoke(() => _webView.Reload());
        }
    }
}