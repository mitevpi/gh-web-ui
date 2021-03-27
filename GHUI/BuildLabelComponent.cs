using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildLabelComponent : GH_ComponentTemplate
    {
        private string _value;
        private double _scale;

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
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the header component.",
                GH_ParamAccess.item, "header");
            pManager.AddNumberParameter("Scale", "scale", "The scale of heading to create (1-4).",
                GH_ParamAccess.item, 1);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Scale", ref _scale);

            // create a valid HTML string from the inputs for our header
            string labelString =
                $"<h{_scale} id='{id}' name='{name}' style='{cssStyle}'>{_value}</h{_scale}>";

            da.SetData(0, labelString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.label;

        public override Guid ComponentGuid => new Guid("8f3e21c9-3e16-2a7e-f37e-e2392da362c7");
    }
}