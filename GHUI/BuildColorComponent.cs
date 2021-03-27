using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildColorComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML color input component.
        /// </summary>
        public BuildColorComponent()
            : base("Create Color Input", "Color",
                "Create a HTML Color Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the color input component.", GH_ParamAccess.item,
                "color");
            pManager.AddTextParameter("ID", "id", "The id of the color input component.", GH_ParamAccess.item,
                "color");
            pManager.AddTextParameter("Value", "val", "The starting value of the color input component.",
                GH_ParamAccess.item, "");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created color input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string name = null;
            string id = null;
            string value = null;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref cssStyle);

            string textString =
                $"<input type='color' id='{id}' name='{name}' value='{value}' style='{cssStyle}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.color;

        public override Guid ComponentGuid => new Guid("dea8a036-7b2e-4f0b-9e60-ad24894d2b25");
    }
}