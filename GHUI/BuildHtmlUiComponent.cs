using System;
using System.Collections.Generic;
using System.IO;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildHtmlUiComponent : GH_Component
    {
        private string _oldString = null;

        /// <summary>
        /// Component for building a HTML UI into a HTML file within
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
            pManager.AddTextParameter("Title", "title", "The title of your interface window.",
                GH_ParamAccess.item, "UI");
            pManager.AddTextParameter("CSS References", "css",
                "URL paths of any external CSS stylesheets to be injected at runtime.",
                GH_ParamAccess.list);
            pManager.AddTextParameter("JS References", "js",
                "URL paths of any external JavaScript to be injected at runtime.",
                GH_ParamAccess.list);
            pManager[3].Optional = true;
            pManager[2].Optional = true;
            pManager[4].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddBooleanParameter("Success", "out", "Status of HTML file creation.", GH_ParamAccess.item);
            pManager.AddTextParameter("Path", "path", "The path at which the HTML file has been written.",
                GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string path = null;
            string htmlString = null;
            string title = null;
            List<string> stylesheets = new List<string>();
            List<string> jsScripts = new List<string>();

            if (!da.GetData(0, ref path)) return;
            if(!System.IO.File.Exists(path))
            { 
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, $"File path: \"{path}\" not found on this computer");
                da.SetData(0, false);
                return;
            }
            if (!da.GetData(1, ref htmlString)) return;
            da.GetData(2, ref title);
            da.GetDataList(3, stylesheets);
            da.GetDataList(4, jsScripts);

            // add document-level properties like title and links to stylesheets
            string htmlTemplate =
                $"<!DOCTYPE html><html lang=en><meta charset=utf-8><title>{title}</title>";

            stylesheets?.ForEach(s => { htmlTemplate += $"<link href='{s}' rel=stylesheet>"; });
            jsScripts?.ForEach(s => { htmlTemplate += $"<script src='{s}' crossorigin='anonymous'></script>"; });
            htmlString = htmlTemplate + htmlString;

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
                    File.WriteAllText(path, htmlTemplate + htmlString);
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
        }


        protected override System.Drawing.Bitmap Icon => Properties.Resources.web_window;

        public override Guid ComponentGuid => new Guid("1c7a41f6-2e46-4a7b-a67d-b7621dc312b4");
    }
}