using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildLabelComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML header component.
        /// </summary>
        public BuildLabelComponent()
            : base("Create Header", "Header",
                "Create a HTML Header.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the header component.", GH_ParamAccess.item,
                "header");
            pManager.AddTextParameter("ID", "id", "The id of the header component.", GH_ParamAccess.item,
                "header");
            pManager.AddTextParameter("Value", "val", "The starting value of the header component.",
                GH_ParamAccess.item, "header");
            pManager.AddNumberParameter("Scale", "scale", "The scale of heading to create (1-4).",
                GH_ParamAccess.item, 1);
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created header input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string name = null;
            string id = null;
            string value = null;
            double scale = 1;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref scale);
            da.GetData(4, ref cssStyle);

            // create a valid HTML string from the inputs for our header
            string labelString =
                $"<h{scale} id='{id}' name='{name}' style='{cssStyle}'>{value}</h{scale}>";

            da.SetData(0, labelString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.label;

        public override Guid ComponentGuid => new Guid("8f3e21c9-3e16-2a7e-f37e-e2392da362c7");
    }
}