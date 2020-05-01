using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using mshtml;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WebWindow : Window
    {
        private static string Path => Assembly.GetExecutingAssembly().Location;
        private static readonly string Directory = System.IO.Path.GetDirectoryName(Path);
        public string Value => Input?.value;
        private mshtml.HTMLDocument Doc => (mshtml.HTMLDocument) webBrowser1.Document;
        private mshtml.HTMLInputTextElement Input => Doc.getElementById("fname") as mshtml.HTMLInputTextElement;

        private string HtmlString2 => ReadHtml();
        private string _htmlStringContainer = "";

        public WebWindow()
        {
            InitializeComponent();
            webBrowser1.NavigateToString(HtmlString2);
        }

        private string ReadHtml()
        {
            string file = System.IO.Path.Combine(Directory, "Window.html");
            if (!File.Exists(file)) return _htmlStringContainer;
            _htmlStringContainer = file;
            return File.ReadAllText(_htmlStringContainer);
        }


        private void Test()
        {
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Test();
        }
    }
}