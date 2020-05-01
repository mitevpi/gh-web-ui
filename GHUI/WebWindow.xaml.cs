using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using CefSharp;
using CefSharp.Wpf;
using mshtml;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class WebWindow : Window
    {
        public static ChromiumWebBrowser Browser;
        public Interop Interop;
        public string value => _input?.value;

        private mshtml.HTMLDocument _doc => (mshtml.HTMLDocument) webBrowser1.Document;
        private mshtml.HTMLInputTextElement _input => _doc.getElementById("fname") as mshtml.HTMLInputTextElement;

        //private string _inputValue => _input?.value;


        private string _htmlString =
            "<html><head></head><body>First row<br>Second row<br><input type='text' id='fname' name='fname'><br><br></body></html>";

        public WebWindow()
        {
            //InitializeChromium();
            //InitializeCef();
            InitializeComponent();
            webBrowser1.NavigateToString(_htmlString);

            // initialise one browser instance
            //InitializeBrowser();
        }

        private void Test()
        {
            //mshtml.HTMLDocument doc = (mshtml.HTMLDocument) webBrowser1.Document;
            //mshtml.HTMLInputTextElement input = doc.getElementById("fname") as mshtml.HTMLInputTextElement;
            //value = input?.value;
            //value = _inputValue;

            //Debug.WriteLine("HI");
        }

        public void InitializeChromium()
        {
            CefSettings settings = new CefSettings();
            // Initialize cef with the provided settings
            Cef.Initialize(settings);
        }

        private void InitializeCef()
        {
            Cef.EnableHighDPISupport();

            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyPath = Path.GetDirectoryName(assemblyLocation);
            string pathSubprocess = Path.Combine(assemblyPath, "CefSharp.BrowserSubprocess.exe");
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            CefSettings settings = new CefSettings
            {
                LogSeverity = LogSeverity.Verbose,
                LogFile = "ceflog.txt",
                BrowserSubprocessPath = pathSubprocess,
            };

            settings.CefCommandLineArgs.Add("allow-file-access-from-files", "1");
            settings.CefCommandLineArgs.Add("disable-web-security", "1");
            Cef.Initialize(settings);
        }

        private void InitializeBrowser()
        {
            string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string projectRootDir = Directory.GetParent(exePath).Parent.Parent.Parent.FullName;
            string indexPath = Path.Combine(projectRootDir, "InsideCEF.WebApp", "index.html");

            Browser = new ChromiumWebBrowser(indexPath);

            // Allow the use of local resources in the browser
            Browser.BrowserSettings = new BrowserSettings
            {
                FileAccessFromFileUrls = CefState.Enabled,
                UniversalAccessFromFileUrls = CefState.Enabled
            };

            //Browser.Dock = System.Windows.Forms.DockStyle.Fill;

            Interop = new Interop(Browser);
            Browser.RegisterAsyncJsObject("Interop", Interop);

            //Browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Browser.Dispose();
            Cef.Shutdown();

            base.OnClosing(e);
        }

        private void Button_OnClick(object sender, RoutedEventArgs e)
        {
            Test();
        }
    }
}