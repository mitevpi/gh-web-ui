using System;
using System.Threading;
using System.Windows.Threading;
using Grasshopper.Kernel;

namespace GHUI
{
    public class GhuiComponent : GH_Component
    {
        public bool Initialized;
        public WebWindow WebWindow;

        // ModelessForm instance
        private WebWindow _webWindow;

        // Separate thread to run Ui on
        private Thread _uiThread;

        /// <summary>
        /// Each implementation of GH_Component must provide a public 
        /// constructor without any arguments.
        /// Category represents the Tab in which the component will appear, 
        /// Subcategory the panel. If you use non-existing tab or panel names, 
        /// new tabs/panels will automatically be created.
        /// </summary>
        public GhuiComponent()
            : base("GrasshopperUI", "GHUI",
                "Launch a UI Window.",
                "UI", "Window")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("HTML Path", "path", "Where to look for the HTML interface.",
                GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Input Values", "vals", "Value of HTML Inputs", GH_ParamAccess.list);
            pManager.AddTextParameter("Input Ids", "ids", "Ids of HTML Inputs", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            string path = null;

            if (!da.GetData(0, ref path))
            {
                return;
            }

            if (Initialized)
            {
                // TODO: ADD SOME CHECK IF THE PATH ACTUALLY CHANGED
                _webWindow.Navigate(path);
                //_webWindow.Refresh();
                da.SetDataList(0, WebWindow.InputValues);
                da.SetDataList(1, WebWindow.InputIds);
            }
            else
            {
                //WebWindow = new WebWindow(path);
                //WebWindow.Show();
                LaunchWindow(path);
                Initialized = true;
            }

            GH_Document doc = OnPingDocument();
            doc?.ScheduleSolution(500, ScheduleCallback);
            //this.ExpireSolution(true);
        }

        private void LaunchWindow(string path)
        {
            if (!(_uiThread is null) && _uiThread.IsAlive) return;
            _uiThread = new Thread(() =>
            {
                SynchronizationContext.SetSynchronizationContext(
                    new DispatcherSynchronizationContext(
                        Dispatcher.CurrentDispatcher));
                // The dialog becomes the owner responsible for disposing the objects given to it.
                _webWindow = WebWindow = new WebWindow(path);
                _webWindow.Closed += (s, e) => Dispatcher.CurrentDispatcher.InvokeShutdown();
                _webWindow.Show();
                Dispatcher.Run();
            });

            _uiThread.SetApartmentState(ApartmentState.STA);
            _uiThread.IsBackground = true;
            _uiThread.Start();
        }

        private void ScheduleCallback(GH_Document document)
        {
            ExpireSolution(false);
        }

        /// <summary>
        /// Icon
        /// </summary>
        protected override System.Drawing.Bitmap Icon => Properties.Resources.web_window;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// guid
        /// </summary>
        public override Guid ComponentGuid => new Guid("1c7a71f6-4e49-4a7b-a67d-b7691dc381b4");
    }
}