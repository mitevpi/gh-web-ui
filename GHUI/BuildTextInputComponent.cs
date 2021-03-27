using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTextInputComponent : GH_ComponentTemplate
    {
        private string _value;

        /// <summary>
        /// Component for building a HTML text input component.
        /// </summary>
        public BuildTextInputComponent()
            : base("Create Text Input", "Text",
                "Create a HTML Text Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the text input component.",
                GH_ParamAccess.item, "Text");
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);

            // create a valid HTML string from the inputs for our text
            string textInputHtml =
                $"<input type='text' id='{id}' name='{name}' value='{_value}' style='{cssStyle}'>";

            da.SetData(0, textInputHtml);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.text_input;

        public override Guid ComponentGuid => new Guid("1c2a48b2-9a21-9c7e-a22f-b2624ac392b2");
    }
}