using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildCheckBoxComponent : GH_ComponentTemplate
    {
        private bool _value = false;
        private string _label;

        /// <summary>
        /// Component for building a HTML checkbox input component.
        /// </summary>
        public BuildCheckBoxComponent()
            : base("Create Checkbox Input", "checkbox",
                "Create a HTML checkbox Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            RegisterDefaultInputParams(pManager);
            pManager.AddBooleanParameter("Value", "val", "The starting value of the checkbox input component.",
                GH_ParamAccess.item, false);
            pManager.AddTextParameter("Label", "label", "The starting label of the checkbox input component.",
                GH_ParamAccess.item, "");
        }


        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Label", ref _label);

            string checkboxHtml =
                $"<input type='checkbox' id='{id}' name='{name}' checked='{_value}' style='{cssStyle}'>";

            if (_label != "")
            {
                checkboxHtml += $"<label for='{id}' id='{id}-label' >{_label}</label>";
            }

            da.SetData(0, checkboxHtml);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.checkbox;

        public override Guid ComponentGuid => new Guid("d1123685-f181-40aa-8f75-d88bcbc49498");
    }
}