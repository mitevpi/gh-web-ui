using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace GHUI
{
    public class GHUIInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "GHUI";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("df3904bc-7b3e-4099-a88f-95ab4145e4b5");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
