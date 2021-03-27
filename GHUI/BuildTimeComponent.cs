using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTimeComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML time input component.
        /// </summary>
        public BuildTimeComponent()
            : base("Create Time Input", "Time",
                "Create a HTML Time Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the time input component.", GH_ParamAccess.item,
                "time");
            pManager.AddTextParameter("ID", "id", "The id of the time input component.", GH_ParamAccess.item,
                "time");
            pManager.AddTextParameter("Value", "val", "The starting value of the time input component.",
                GH_ParamAccess.item, "");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created time input.",
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
                $"<input type='time' id='{id}' name='{name}' value='{value}' style='{cssStyle}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.time;

        public override Guid ComponentGuid => new Guid("4f4c2dc7-88fb-4caa-85f0-c1d5f5ec4383");
    }
}