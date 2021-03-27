using System;
using GHUI.Classes;
using Grasshopper.Kernel;

namespace GHUI
{
    public class BuildSliderComponent : GH_ComponentTemplate
    {
        private double _value = 50;
        private double _min = 0;
        private double _max = 100;

        /// <summary>
        /// Component for building a HTML slider input component.
        /// </summary>
        public BuildSliderComponent()
            : base("Create Slider", "Slider",
                "Create a HTML Slider Input.",
                "UI", "Create")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            RegisterDefaultInputParams(pManager);
            pManager.AddNumberParameter("Value", "val", "The starting value of the slider input component.",
                GH_ParamAccess.item, 50);
            pManager.AddNumberParameter("Min", "min", "The min value of the slider input component.",
                GH_ParamAccess.item, 0);
            pManager.AddNumberParameter("Max", "max", "The max value of the slider input component.",
                GH_ParamAccess.item, 100);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            GetStandardInputs(da);
            da.GetData("Value", ref _value);
            da.GetData("Min", ref _min);
            da.GetData("Max", ref _max);

            // create a valid HTML string from the inputs for our slider
            string sliderString =
                $"<input type='range' id='{id}' name='{name}' value='{_value}' min='{_min}' max='{_max}' style='{cssStyle}'>";

            da.SetData(0, sliderString);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.slider;

        public override Guid ComponentGuid => new Guid("1c2a47f6-2e46-2a7b-a17d-b2629dc312b3");
    }
}