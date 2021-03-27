using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildImageComponent : GH_ComponentTemplate
    {
        private string _url;
        private double _width = 300;
        private double _height = 300;

        /// <summary>
        /// Component for building a HTML image component.
        /// </summary>
        public BuildImageComponent()
            : base("Create Image Input", "Image",
                "Create an HTML Image.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            RegisterDefaultInputParams(pManager);
            pManager.AddTextParameter("URL", "url", "The url of the image to show.",
                GH_ParamAccess.item, "https://vuejs.org/images/logo.png");
            pManager.AddNumberParameter("Height", "height", "The desired height of the image (pixels).",
                GH_ParamAccess.item, 300);
            pManager.AddNumberParameter("Width", "width", "The desired width of the image (pixels).",
                GH_ParamAccess.item, 300);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("URL", ref _url);
            da.GetData("Width", ref _width);
            da.GetData("Height", ref _height);

            string textString =
                $"<img id='{id}' height='{_height}' width='{_width}' src='{_url}' alt='{name}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.image;

        public override Guid ComponentGuid => new Guid("89365268-cda2-4a8b-a8bb-10449e58f5cb");
    }
}