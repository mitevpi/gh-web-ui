using Grasshopper.Kernel;

namespace GHUI.Classes
{
    public abstract class GH_ComponentTemplate : GH_Component
    {
        protected string name;
        protected string id;
        protected string cssStyle;

        public GH_ComponentTemplate(string name, string nickName, string description, string category, string subCategory)
            : base(name, nickName, description, category, subCategory)
        {
        }

        protected void RegisterDefaultInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the input component.", GH_ParamAccess.item,
                "slider");
            pManager.AddTextParameter("ID", "id", "The id of the input component.", GH_ParamAccess.item,
                "slider");
            pManager.AddTextParameter("CSS", "css", "The `style` attribute to apply to the element and its children.",
                GH_ParamAccess.item,
                "");
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created slider input.",
                GH_ParamAccess.list);
        }

        protected void GetStandardInputs(IGH_DataAccess da)
        {
            da.GetData("Name", ref name);
            da.GetData("ID", ref id);
            da.GetData("CSS", ref cssStyle);
        }
    }
}