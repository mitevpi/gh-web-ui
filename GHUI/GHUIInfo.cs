using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GHUI
{
    public class GhuiInfo : GH_AssemblyInfo
    {
        public override string Name => "GHUI";

        public override Bitmap Icon =>
            //Return a 24x24 pixel bitmap to represent this GHA library.
            null;

        public override string Description =>
            //Return a short string describing the purpose of this GHA library.
            "Package for building and serving web-based user interfaces (UI).";

        public override Guid Id => new Guid("df3904bc-7b3e-4099-a88f-95ab4145e4b5");

        public override string AuthorName =>
            //Return a string identifying you or your company.
            "Petar Mitev";

        public override string AuthorContact =>
            //Return a string representing your preferred contact details.
            "p.mitevpi@gmail.com | @mitevpi on GitHub/Twitter";
    }
}
