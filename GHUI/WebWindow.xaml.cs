using System.Collections.Generic;
using System.Windows;
using GHUI.Classes;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for the WPF WebBrowser
    /// </summary>
    //[ComVisible(true)]
    public partial class WebWindow : Window
    {
        private WebView2Wrapper _webView2WrapperInstance;

        // PUBLIC FIELDS
        /// <summary>
        /// List of the current values of all the input elements in the DOM.
        /// </summary>
        public List<string> InputValues => _webView2WrapperInstance.InputValues;

        /// <summary>
        /// List of the id properties of all the input elements in the DOM.
        /// </summary>
        public List<string> InputIds => _webView2WrapperInstance.InputIds;
        public List<string> InputNames => _webView2WrapperInstance.InputNames;
        public List<string> InputTypes => _webView2WrapperInstance.InputTypes;

        /// <summary>
        /// The WPF Container for the WebBrowser element which renders the user's HTML.
        /// </summary>
        /// <param name="htmlPath">Path of the HTML file to render as UI.</param>
        public WebWindow(string htmlPath)
        {
            InitializeComponent();
            _webView2WrapperInstance = new WebView2Wrapper(htmlPath, Dispatcher);
            _webView2WrapperInstance.InitializeWebView(Docker);
            _webView2WrapperInstance.SubscribeToHtmlChanged();
        }

        public void Navigate(string newPath)
        {
            _webView2WrapperInstance.Navigate(newPath);
        }

        public void HandleSetters(Dictionary<string, string> setters)
        {
            _webView2WrapperInstance.HandleValueSetters(setters);
        }
    }
}