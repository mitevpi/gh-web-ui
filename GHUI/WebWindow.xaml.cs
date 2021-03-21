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
            InitializeWebView();
        }

        private async Task InitializeWebViewAsync(WebView2 webView)
        {
            try
            {
                //MessageBox.Show($"Location {_location}");
                var env = await CoreWebView2Environment.CreateAsync(null, _location);
                await webView.EnsureCoreWebView2Async(env);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        internal async void InitializeWebView()
        {
            _webView = new WebView2();  //Create a webview2 control programatically

            this.Docker.Children.Clear();
            this.Docker.Children.Add(_webView);  //Add the webview2 to the dock panel

            await InitializeWebViewAsync(_webView);
        }


        internal void Navigate()
        {
            if (_webView != null && _webView.CoreWebView2 != null)
            {
                //navigate to website 
                _webView.Source = new Uri(_path);
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Navigate();
        }
    }
}