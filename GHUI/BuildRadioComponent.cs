using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildRadioComponent : GH_Component
    {
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
            pManager.AddTextParameter("Name", "name", "The name of the radio input component.", GH_ParamAccess.item,
                "radio");
            pManager.AddTextParameter("ID", "id", "The id of the radio input component.", GH_ParamAccess.item,
                "radio");
            pManager.AddBooleanParameter("Value", "val", "The starting value of the radio input component.",
                GH_ParamAccess.item, false);
            pManager.AddTextParameter("Label", "label", "The starting label of the radio input component.",
                GH_ParamAccess.item, "");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created radio input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string name = null;
            string id = null;
            bool value = false;
            string label = null;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref label);
            da.GetData(4, ref cssStyle);

            string textString =
                $"<input type='radio' id='{id}' name='{name}' checked='{value}' style='{cssStyle}'>";

            if (label != "")
            {
                textString = textString + $"<label for='{id}' id='{id}-label'>{label}</label>";
            }


            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.radio;

        public override Guid ComponentGuid => new Guid("4b5a9f29-ecee-417a-ad97-76f93ce41d01");
    }
}