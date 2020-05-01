using System.Diagnostics;
using System.Threading.Tasks;
using CefSharp;
using CefSharp.Wpf;

namespace GHUI
{
    /// <summary>
    /// Object which is used to interoperate between Rhino and CEF
    /// </summary>
    public class Interop
    {
        public ChromiumWebBrowser Browser { get; private set; }

        // default ctor
        public Interop()
        {
        }

        public Interop(ChromiumWebBrowser browser) : this()
        {
            Browser = browser;
        }

        public void ShowDev()
        {
            Browser.ShowDevTools();
        }

        #region To UI (Generic)

        // from https://github.com/speckleworks/SpeckleRhino/blob/dev/SpeckleRhinoPlugin/src/Interop.cs#L340

        public void NotifyFrame(string EventType, string EventInfo)
        {
            string script = string.Format("window.EventBus.$emit('{0}', '{1}')", EventType, EventInfo);
            try
            {
                Browser.GetMainFrame().EvaluateScriptAsync(script);
            }
            catch
            {
                Debug.WriteLine("For some reason, this browser was not initialised.");
            }
        }

        #endregion

        #region To UI

        public void PushPreview(string data)
        {
            //NotifyFrame("push-preview", data);
            Browser?.GetMainFrame().EvaluateScriptAsync("onGhObjectAdded(" + data + ");");
        }

        #endregion

        #region To Rhino

        #endregion
    }
}