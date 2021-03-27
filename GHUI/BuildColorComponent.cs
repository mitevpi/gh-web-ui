using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildColorComponent : GH_ComponentTemplate
    {
        private string _value;

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
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the color input component.",
                GH_ParamAccess.item, "");
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);

            string colorInputHtml =
                $"<input type='color' id='{id}' name='{name}' value='{_value}' style='{cssStyle}'>";

            da.SetData(0, colorInputHtml);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.color;

        public override Guid ComponentGuid => new Guid("dea8a036-7b2e-4f0b-9e60-ad24894d2b25");
    }
}