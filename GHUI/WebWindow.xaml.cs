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
        private readonly string _path;

        /// HTML VALUE
        /// <summary>
        /// List of the current values of all the input elements in the DOM.
        /// </summary>
        public List<string> InputValues;

        /// <summary>
        /// List of the id properties of all the input elements in the DOM.
        /// </summary>
        public List<string> InputIds;
        private WebView2 _webView;
        private string _location = "";


        /// <summary>
        /// The WPF Container for the WebBrowser element which renders the user's HTML.
        /// </summary>
        /// <param name="path">Path of the HTML file to render as UI.</param>
        public WebWindow(string path)
        {
            _path = path;
            _location = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\temp";
            InitializeComponent();

            //WebBrowser.Source = new Uri(_path);
            //WebBrowser.NavigateToString(HtmlString);
            //WebBrowser.LoadCompleted += BrowserLoaded;
        }


        internal async void Navigate()
        {
            _webView = new WebView2();  //Create a webview2 control programatically

            this.Docker.Children.Clear();
            this.Docker.Children.Add(_webView);  //Add the webview2 to the dock panel

            await InitializeAsync(_webView);

            if (_webView != null && _webView.CoreWebView2 != null)
            {
                //navigate to website 
                _webView.Source = new Uri(_path);
                //webView.Source = new Uri("https://www.bing.com/");
            }
        }

        private async Task InitializeAsync(WebView2 webView)
        {
            // wait for coreWebView2 initialization
            try
            {
                MessageBox.Show($"Location {_location}");
                var env = await CoreWebView2Environment.CreateAsync(null, _location);
                await webView.EnsureCoreWebView2Async(env);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Attempt to navigate to gh page..");
            Navigate();
        }
    }
}