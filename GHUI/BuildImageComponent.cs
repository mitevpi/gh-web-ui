using System;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildImageComponent : GH_Component
    {
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
            pManager.AddTextParameter("Name", "name", "The name of the image component.", GH_ParamAccess.item,
                "image");
            pManager.AddTextParameter("ID", "id", "The id of the image component.", GH_ParamAccess.item,
                "image");
            pManager.AddTextParameter("URL", "url", "The url of the image to show.",
                GH_ParamAccess.item, "https://vuejs.org/images/logo.png");
            pManager.AddNumberParameter("Height", "height", "The desired height of the image (pixels).", GH_ParamAccess.item, 300);
            pManager.AddNumberParameter("Width", "width", "The desired width of the image (pixels).", GH_ParamAccess.item, 300);
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item, "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created image input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            string name = null;
            string id = null;
            string url = null;
            double height = 200;
            double width = 200;
            string cssStyle = null;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref url);
            da.GetData(3, ref height);
            da.GetData(4, ref width);
            da.GetData(5, ref cssStyle);

            string textString =
                $"<img id='{id}' height='{height}' width='{width}' src='{url}' alt='{name}'>";

            da.SetData(0, textString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.image;

        public override Guid ComponentGuid => new Guid("89365268-cda2-4a8b-a8bb-10449e58f5cb");
    }
}