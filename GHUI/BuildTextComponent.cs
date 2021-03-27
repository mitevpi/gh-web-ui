using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTextComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML text component.
        /// </summary>
        public BuildTextComponent()
            : base("Create Text", "Text",
                "Create a HTML Text Field",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the text component.", GH_ParamAccess.item,
                "text");
            pManager.AddTextParameter("ID", "id", "The id of the text component.", GH_ParamAccess.item,
                "text");
            pManager.AddTextParameter("Value", "val", "The starting value of the text component.",
                GH_ParamAccess.item, "text");
            pManager.AddNumberParameter("Font Size", "font", "The size of the desired font. (pixels)",
                GH_ParamAccess.item, 16);
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
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
            double fontSize = 1;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref fontSize);
            da.GetData(4, ref cssStyle);

            // create a valid HTML string from the inputs for our text

            cssStyle = $"font-size: {fontSize}px;" + cssStyle;
            string textString =
                $"<p id='{id}' name='{name}' style='{cssStyle}'>{value}</p>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.label;

        public override Guid ComponentGuid => new Guid("8d14a42d-5ccf-4de9-9e12-3c484794c6a5");
    }
}