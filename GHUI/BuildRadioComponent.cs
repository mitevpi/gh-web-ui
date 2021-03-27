using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildRadioComponent : GH_ComponentTemplate
    {
        private bool _value;
        private string _label;

        /// <summary>
        /// Component for building a HTML radio input component.
        /// </summary>
        public BuildRadioComponent()
            : base("Create Radio Input", "Radio",
                "Create a HTML Radio Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            RegisterDefaultInputParams(pManager);
            pManager.AddBooleanParameter("Value", "val", "The starting value of the radio input component.",
                GH_ParamAccess.item, false);
            pManager.AddTextParameter("Label", "label", "The starting label of the radio input component.",
                GH_ParamAccess.item, "");
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Label", ref _label);

            string textString =
                $"<input type='radio' id='{id}' name='{name}' checked='{_value}' style='{cssStyle}'>";

            if (_label != "")
            {
                textString = textString + $"<label for='{id}' id='{id}-label'>{_label}</label>";
            }


            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.radio;

        public override Guid ComponentGuid => new Guid("4b5a9f29-ecee-417a-ad97-76f93ce41d01");
    }
}