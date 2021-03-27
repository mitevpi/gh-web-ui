using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using Grasshopper.Kernel;

namespace GHUI
{
    public class StyleGridComponent : GH_Component
    {
        /// <summary>
        /// Component for building a HTML time input component.
        /// </summary>
        public StyleGridComponent()
            : base("Create Grid Container", "Grid",
                "Create a CSS Definition for a Grid Container.",
                "UI", "Style")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddNumberParameter("Columns", "cols", "The number of columns to add to the the grid definition.",
                GH_ParamAccess.item, 2);
            pManager.AddNumberParameter("Column Sizes", "colSizes", "The sizes of the columns to add.",
                GH_ParamAccess.list);
            pManager.AddTextParameter("Column Units", "colUnit",
                "The units to use for the specified sizes ('px' or '%').",
                GH_ParamAccess.item, "px");
            pManager[1].Optional = true;
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("CSS", "css", "The CSS code for the container grid input.",
                GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess da)
        {
            // get input from gh component inputs
            double cols = 2;
            List<double> colSizes = new List<double>();
            string colUnit = null;
            string styleString = "display: grid;";
            string styleString2 = "";

            da.GetData(0, ref cols);
            da.GetDataList(1, colSizes);
            da.GetData(2, ref colUnit);

            // if there is a list of column sizes coming in, hardcode those dimensions into the 
            // style. otherwise, make the span `auto` compute.
            if (colSizes.Count > 0)
            {
                styleString2 = "grid-template-columns:";
                colSizes.ForEach(s =>
                {
                    styleString2 += $"{s}{colUnit} ";
                });
                styleString2 += ";";
            }
            else
            {
                // generate auto-spaced columns
                styleString2 = "grid-template-columns:" + string.Concat(Enumerable.Repeat("auto ", (int) cols)) + ";";
            }


            //string textString =
            //    $"<input type='time' id='{id}' name='{name}' value='{value}' style='{cssStyle}'>";

            da.SetData(0, styleString + styleString2);
        }

        protected override System.Drawing.Bitmap Icon => Properties.Resources.grid;

        public override Guid ComponentGuid => new Guid("d6957972-a034-4e62-8eb8-c09a38484d16");
    }
}