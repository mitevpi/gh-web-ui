using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using Grasshopper.Kernel;
using mshtml;
using HTMLInputTextElementEvents_onchangeEventHandler = mshtml.HTMLInputTextElementEvents_onchangeEventHandler;

namespace GHUI
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    [ComVisible(true)]
    public partial class WebWindow : Window
    {
        // STUFF
        private IGH_DataAccess _gh = null;

        // GENERIC SETUP
        public event PropertyChangedEventHandler PropertyChanged;
        private static string Path => Assembly.GetExecutingAssembly().Location;
        private static readonly string Directory = System.IO.Path.GetDirectoryName(Path);

        // HTML QUERY
        private HTMLDocument Doc => (HTMLDocument) webBrowser1.Document;
        private IHTMLInputTextElement Input => Doc.getElementById("fname") as IHTMLInputTextElement;

        // HTML READ
        private string _htmlStringContainer = "";
        private FileSystemWatcher _watcher;

        /// HTML STRING
        private string _htmlString;
        public string HtmlString
        {
            get => _htmlString;
            set
            {
                _htmlString = value;
                OnHtmlChanged();
            }
        }
        
        /// HTML VALUE
        private string _value;
        public string Value
        {
            get
            {
                OnPropertyChanged();
                return Input?.value;
            }
        }


        public WebWindow(IGH_DataAccess da)
        {
            _gh = da;
            InitializeComponent();
            HtmlString = ReadHtml();
            webBrowser1.NavigateToString(HtmlString);
            MonitorTailOfFile();
            //Input.onchange += new System.EventHandler(InputChanged);
        }

        private void InputChanged(object o, EventArgs e)
        {
            object o1 = o;
            EventArgs e1 = e;
            Debug.WriteLine("HI");
        }

        private string ReadHtml()
        {
            string file = System.IO.Path.Combine(Directory, "Window.html");
            if (!File.Exists(file)) return _htmlStringContainer;
            _htmlStringContainer = file;

            return File.ReadAllText(_htmlStringContainer);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //_gh.SetData(0, Value);
        }

        protected void OnHtmlChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //webBrowser1.NavigateToString(HtmlString);
        }

        public void MonitorTailOfFile()
        {
            _watcher = new FileSystemWatcher
            {
                Path = Directory,
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName,
                Filter = "*.html"
            };
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            _watcher.Dispose();
            _watcher = null;
            Thread.Sleep(1000);
            Dispatcher.Invoke(() =>
            {
                HtmlString = ReadHtml();
                webBrowser1.NavigateToString(HtmlString);
                MonitorTailOfFile();
            });
        }
    }
}