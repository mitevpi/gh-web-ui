using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class GhuiComponent : GH_Component
    {
        public bool Initialized;
        public WebWindow WebWindow;

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
            pManager.AddTextParameter("Out", "out", "Value of HTML Text Input", GH_ParamAccess.item);
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
                da.SetData(0, WebWindow.Value);
            }
            else
            {
                WebWindow = new WebWindow(path);
                WebWindow.Show();
                Initialized = true;
                da.SetData(0, WebWindow.Value);
            }

            GH_Document doc = OnPingDocument();
            doc?.ScheduleSolution(500, ScheduleCallback);
            //this.ExpireSolution(true);
        }

        private void ScheduleCallback(GH_Document document)
        {
            ExpireSolution(false);
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon =>
            // You can add image files to your project resources and access them like this:
            //return Resources.IconForThisComponent;
            null;

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid => new Guid("1c7a71f6-4e49-4a7b-a67d-b7691dc381b4");
    }
}