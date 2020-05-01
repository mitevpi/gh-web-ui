using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Grasshopper.Kernel;
using mshtml;

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
        private mshtml.HTMLDocument Doc => (mshtml.HTMLDocument) webBrowser1.Document;
        private mshtml.HTMLInputTextElement Input => Doc.getElementById("fname") as mshtml.HTMLInputTextElement;

        // HTML READ
        private string HtmlString2 => ReadHtml();
        private string _htmlStringContainer = "";

        public string Value
        {
            get
            {
                OnPropertyChanged();
                return Input?.value;
            }
        }

        //public string Value => Input?.value;


        public WebWindow(IGH_DataAccess da)
        {
            _gh = da;
            InitializeComponent();
            webBrowser1.NavigateToString(HtmlString2);
            MonitorTailOfFile(System.IO.Path.Combine(Directory, "Window.html"));
            //HTMLInputTextElementEvents_onchangeEventHandler temp = (HTMLInputTextElementEvents_onchangeEventHandler) Input.onchange;
            //Input.onchange += inputOnChange();
        }


        private void onchangeInput(object sender, EventArgs e)
        {
            //HtmlHandler htmlHandler = (HtmlHandler)sender;
            //IHTMLElement element = htmlHandler.SourceHTMLWindow.@event.srcElement;
        }

        private string ReadHtml()
        {
            string file = System.IO.Path.Combine(Directory, "Window.html");
            if (!File.Exists(file)) return _htmlStringContainer;
            _htmlStringContainer = file;

            //FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //using (StreamReader sr = new StreamReader(fs))
            //{
            //    sr.
            //}

            return File.ReadAllText(_htmlStringContainer);
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            //_gh.SetData(0, Value);
        }

        public static void MonitorTailOfFile(string filePath)
        {
            Task.Run(() =>
            {
                long initialFileSize = new FileInfo(filePath).Length;
                long lastReadLength = initialFileSize - 1024;
                if (lastReadLength < 0) lastReadLength = 0;

                while (true)
                {
                    try
                    {
                        long fileSize = new FileInfo(filePath).Length;
                        if (fileSize > lastReadLength)
                        {
                            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read,
                                FileShare.ReadWrite))
                            {
                                fs.Seek(lastReadLength, SeekOrigin.Begin);
                                byte[] buffer = new byte[1024];

                                while (true)
                                {
                                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                                    lastReadLength += bytesRead;

                                    if (bytesRead == 0)
                                        break;

                                    string text = ASCIIEncoding.ASCII.GetString(buffer, 0, bytesRead);

                                    Console.Write(text);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }

                    Thread.Sleep(1000);
                }
            });
        }
    }
}