using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildButtonComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML button input component.
        /// </summary>
        public BuildButtonComponent()
            : base("Create Button", "Button",
                "Create a HTML Button Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the button input component.", GH_ParamAccess.item,
                "button");
            pManager.AddTextParameter("ID", "id", "The id of the button input component.", GH_ParamAccess.item,
                "button");
            pManager.AddTextParameter("Value", "val", "The starting value of the button input component.",
                GH_ParamAccess.item, "Button");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created button input.",
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

            // create a valid HTML string from the inputs for our button
            string buttonString =
                $"<input type='button' id='{id}' name='{name}' value='{value}' style='{cssStyle}'>";

            da.SetData(0, buttonString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.button;

        public override Guid ComponentGuid => new Guid("6a8a21e2-2e48-2a7b-f37e-a2612de312b7");
    }
}