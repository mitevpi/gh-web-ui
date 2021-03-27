using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildDivComponent : GH_Component
    {
        /// <summary>
        /// Component for wrapping some HTML into a <div></div>.
        /// </summary>
        public BuildDivComponent()
            : base("Wrap in <div>", "<div>",
                "Wrap HTML within a <div> component.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML to wrap within a <div> element.", GH_ParamAccess.item,
                "");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the <div> and its children elements.", GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code wrapped within a <div> element.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string htmlToWrap = null;
            string cssStyle = null;

            if (!da.GetData(0, ref htmlToWrap)) return;
            da.GetData(1, ref cssStyle);


            string sliderString = $"<div style='{cssStyle}'>{htmlToWrap}</div>";
            da.SetData(0, sliderString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.div;

        public override Guid ComponentGuid => new Guid("9a2c42e6-2f48-2d5b-c17a-b6621dc312e3");
    }
}