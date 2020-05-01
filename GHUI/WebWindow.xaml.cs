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
        public string value => _input?.value;

        private mshtml.HTMLDocument _doc => (mshtml.HTMLDocument) webBrowser1.Document;
        private mshtml.HTMLInputTextElement _input => _doc.getElementById("fname") as mshtml.HTMLInputTextElement;

        private string _htmlString =
            "<html><head></head><body>First row<br>Second row<br><input type='text' id='fname' name='fname'><br><br></body></html>";

        private string _htmlString2 => ReadHtml();
        private string _htmlStringContainer;

        public WebWindow()
        {
            InitializeComponent();
            webBrowser1.NavigateToString(_htmlString2);
            ReadHtml();
        }

        private void Test()
        {
        }

        private string ReadHtml()
        {
            string path = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(path);

            string file = Path.Combine(directory, "Window.html");
            if (File.Exists(file))
            {
                _htmlStringContainer = file;
            }

            return File.ReadAllText(_htmlStringContainer);
        }


        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Test();
        }
    }
}