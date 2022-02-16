using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    [Transaction(TransactionMode.Manual)]
    public class Main : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            var window = new MainWindow(commandData);
            if (window.Error!="")
            {
                TaskDialog.Show("Ошибка", window.Error);
                return Result.Failed;
            }
            window.ShowDialog();
            return Result.Succeeded;
        }
    }
}
