using System;
using System.Windows.Forms;
using Grasshopper.Kernel;

namespace GHUI
{
    public class GhuiComponent : GH_Component
    {
        public bool Initialized;
        public WebWindow Wwindow;

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
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Refresh", "R", "Whether or not to refresh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Value", "val", "Value of HTML Text Input", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="da">The DA object can be used to retrieve data from input parameters and 
        /// to store data in output parameters.</param>
        protected override void SolveInstance(IGH_DataAccess da)
        {
            bool refresh = false;

            // Use the DA object to retrieve the data inside the input parameters.
            // If the retrieval fails (for example if there is no data) we need to abort.
            if (!da.GetData(0, ref refresh))
            {
                return;
            }

            if (Initialized)
            {
                da.SetData(0, Wwindow.Value);
            }
            else
            {
                Wwindow = new WebWindow();
                Wwindow.Show();
                da.SetData(0, Wwindow.Value);
                Initialized = true;
            }
        }

        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1c7a71f6-4e49-4a7b-a67d-b7691dc381b4"); }
        }
    }
}