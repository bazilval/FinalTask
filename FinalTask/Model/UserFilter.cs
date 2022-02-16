using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    public class UserFilter : ISelectionFilter
    {
        private Type type;

        public UserFilter(Type type)
        {
            this.type = type;
        }

        public bool AllowElement(Element elem)
        {
            return elem.GetType() == type;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            throw new NotImplementedException();
        }
    }
}
