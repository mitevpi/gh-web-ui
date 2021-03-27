using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildTextComponent : GH_ComponentTemplate
    {
        private string _value;
        private double _fontSize;

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
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("Value", "val", "The starting value of the text component.",
                GH_ParamAccess.item, "text");
            pManager.AddNumberParameter("Font Size", "font", "The size of the desired font. (pixels)",
                GH_ParamAccess.item, 16);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Font Size", ref _fontSize);

            // create a valid HTML string from the inputs for our text
            cssStyle = $"font-size: {_fontSize}px;" + cssStyle;
            string textHtml =
                $"<p id='{id}' name='{name}' style='{cssStyle}'>{_value}</p>";

            da.SetData(0, textHtml);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.label;

        public override Guid ComponentGuid => new Guid("8d14a42d-5ccf-4de9-9e12-3c484794c6a5");
    }
}