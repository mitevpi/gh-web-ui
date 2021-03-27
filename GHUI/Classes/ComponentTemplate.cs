using Grasshopper.Kernel;

namespace GHUI.Classes
{
    public abstract class ComponentTemplate : GH_Component
    {
        private string _name;
        private string _id;
        private string _cssStyle;

        public ComponentTemplate(string name, string nickName, string description, string category, string subCategory)
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
            da.GetData(0, ref _name);
            da.GetData(1, ref _id);
            da.GetData(2, ref _cssStyle);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            da.GetData(0, ref _name);
            da.GetData(1, ref _id);
            da.GetData(2, ref _cssStyle);
        }
    }
}