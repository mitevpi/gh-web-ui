using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildDateComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML date input component.
        /// </summary>
        public BuildDateComponent()
            : base("Create Date Input", "Date",
                "Create a HTML Date Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the date input component.", GH_ParamAccess.item,
                "date");
            pManager.AddTextParameter("ID", "id", "The id of the date input component.", GH_ParamAccess.item,
                "date");
            pManager.AddTextParameter("Value", "val", "The starting value of the date input component.",
                GH_ParamAccess.item, "");
            pManager.AddTextParameter("Min", "min", "The min value of the date input component.",
                GH_ParamAccess.item, "1900-12-31");
            pManager.AddTextParameter("Max", "max", "The max value of the date input component.",
                GH_ParamAccess.item, "2021-01-02");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created date input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string name = null;
            string id = null;
            string value = null;
            string min = null;
            string max = null;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref min);
            da.GetData(4, ref max);
            da.GetData(4, ref cssStyle);

            string textString =
                $"<input type='date' id='{id}' name='{name}' value='{value}' min='{min}' max='{max}' style='{cssStyle}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.date;

        public override Guid ComponentGuid => new Guid("3164dd20-ddff-4af4-a452-7a5a2bd12caa");
    }
}