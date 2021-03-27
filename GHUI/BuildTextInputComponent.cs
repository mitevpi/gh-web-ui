using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTextInputComponent : GH_Component
    {
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
            pManager.AddTextParameter("Name", "name", "The name of the text input component.", GH_ParamAccess.item,
                "text");
            pManager.AddTextParameter("ID", "id", "The id of the text input component.", GH_ParamAccess.item,
                "text");
            pManager.AddTextParameter("Value", "val", "The starting value of the text input component.",
                GH_ParamAccess.item, "Text");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.", GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created text input.",
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

            // create a valid HTML string from the inputs for our text
            string textString =
                $"<input type='text' id='{id}' name='{name}' value='{value}' style='{cssStyle}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.text_input;

        public override Guid ComponentGuid => new Guid("1c2a48b2-9a21-9c7e-a22f-b2624ac392b2");
    }
}