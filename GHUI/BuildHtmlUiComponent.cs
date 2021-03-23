using System;
using System.IO;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildHtmlUiComponent : GH_Component
    {
        private string _oldString = null;

        /// <summary>
        /// Component for building a Vue.js UI into a HTML file within
        /// Grasshopper.
        /// </summary>
        public BuildHtmlUiComponent()
            : base("Build HTML UI", "HTML UI",
                "Build a Web UI to a HTML file.",
                "UI", "Main")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("HTML Path", "path", "Where to create the Vue HTML interface.",
                GH_ParamAccess.item);
            pManager.AddTextParameter("HTML String", "html", "The HTML string to write to a file.",
                GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "out", "Status of HTML file creation.", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "path", "The path at which the HTML file has been written.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string path = null;
            string htmlString = null;

            if (!da.GetData(0, ref path)) return;
            if (!da.GetData(1, ref htmlString)) return;

            // if the html string is the same, do nothing
            if (_oldString == htmlString)
            {
            }
            // otherwise try writing to the html file and inform the user
            // of the successful write
            else
            {
                try
                {
                    File.WriteAllText(path, htmlString);
                    _oldString = htmlString;
                }
                catch (Exception e)
                {
                    da.SetData(0, false);
                    da.SetData(1, "");
                }
            }

            // set status message and path output if component executed successfully
            da.SetData(0, true);
            da.SetData(1, path);

            GH_Document doc = OnPingDocument();
            doc?.ScheduleSolution(500, ScheduleCallback);
        }


        private void ScheduleCallback(GH_Document document)
        {
            ExpireSolution(false);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.web_window;

        public override Guid ComponentGuid => new Guid("1c7a41f6-2e46-4a7b-a67d-b7621dc312b4");
    }
}