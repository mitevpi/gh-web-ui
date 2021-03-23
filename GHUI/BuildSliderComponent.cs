using System;
using System.IO;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildSliderComponent : GH_Component
    {
        private string _oldString = null;

        /// <summary>
        /// Component for building a Vue.js UI into a HTML file within
        /// Grasshopper.
        /// </summary>
        public BuildSliderComponent()
            : base("Create Slider", "Slider",
                "Create a HTML Slider Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "name", "The name of the slider input component.", GH_ParamAccess.item,
                "slider");
            pManager.AddTextParameter("ID", "id", "The id of the slider input component.", GH_ParamAccess.item,
                "slider");
            pManager.AddNumberParameter("Value", "val", "The starting value of the slider input component.",
                GH_ParamAccess.item, 50);
            pManager.AddNumberParameter("Min", "min", "The min value of the slider input component.",
                GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Max", "max", "The max value of the slider input component.",
                GH_ParamAccess.item, 100);
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("HTML", "html", "The HTML code for the created slider input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            string name = null;
            string id = null;
            double value = 50;
            double min = 0;
            double max = 100;

            da.GetData(0, ref name);
            da.GetData(1, ref id);
            da.GetData(2, ref value);
            da.GetData(3, ref min);
            da.GetData(4, ref max);

            // create a valid HTML string from the inputs for our slider
            string sliderString =
                $"<input type='range' id='{id}' name='{name}' value='{value}' min='{min}' max='{max}'>";

            da.SetData(0, sliderString);

            GH_Document doc = OnPingDocument();
            doc?.ScheduleSolution(500, ScheduleCallback);
        }


        private void ScheduleCallback(GH_Document document)
        {
            ExpireSolution(false);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.slider;

        public override Guid ComponentGuid => new Guid("1c2a47f6-2e46-2a7b-a17d-b2629dc312b3");
    }
}