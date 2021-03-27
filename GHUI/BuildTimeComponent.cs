using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTimeComponent : GH_ComponentTemplate
    {
        private string _value;

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
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the time input component.",
                GH_ParamAccess.item, "");
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);

            string timeInputHtml =
                $"<input type='time' id='{id}' name='{name}' value='{_value}' style='{cssStyle}'>";

            da.SetData(0, timeInputHtml);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.time;

        public override Guid ComponentGuid => new Guid("4f4c2dc7-88fb-4caa-85f0-c1d5f5ec4383");
    }
}