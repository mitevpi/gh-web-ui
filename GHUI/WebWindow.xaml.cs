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

        //private string _inputValue => _input?.value;


        private string _htmlString =
            "<html><head></head><body>First row<br>Second row<br><input type='text' id='fname' name='fname'><br><br></body></html>";

        public WebWindow()
        {
            InitializeComponent();
            webBrowser1.NavigateToString(_htmlString);
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