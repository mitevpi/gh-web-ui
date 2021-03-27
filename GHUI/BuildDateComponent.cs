using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildDateComponent : GH_ComponentTemplate
    {
        private string _value;
        private string _min;
        private string _max;

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
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the date input component.",
                GH_ParamAccess.item, "");
            pManager.AddTextParameter("Min", "min", "The min value of the date input component.",
                GH_ParamAccess.item, "1900-12-31");
            pManager.AddTextParameter("Max", "max", "The max value of the date input component.",
                GH_ParamAccess.item, "2021-01-02");
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Min", ref _min);
            da.GetData("Max", ref _max);

            string textString =
                $"<input type='date' id='{id}' name='{name}' value='{_value}' min='{_min}' max='{_max}' style='{cssStyle}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.date;

        public override Guid ComponentGuid => new Guid("3164dd20-ddff-4af4-a452-7a5a2bd12caa");
    }
}