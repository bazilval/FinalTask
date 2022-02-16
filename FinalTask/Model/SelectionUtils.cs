using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask
{
    public class SelectionUtils
    {
        public static List<XYZ> GetPoints(ExternalCommandData commandData, string message, ObjectSnapTypes snapType, int pointsCount = -1)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;

            List<XYZ> points = new List<XYZ>();

            if (pointsCount == -1)
            {
                while (true)
                {
                    XYZ pickedPoint = null;
                    try
                    {
                        pickedPoint = uidoc.Selection.PickPoint(snapType, message);
                    }
                    catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
                    {
                        break;
                    }
                    points.Add(pickedPoint);
                }
                return points;
            }
            else
            {
                for (int i = 0; i < pointsCount; i++)
                {
                    XYZ pickedPoint = null;
                    try
                    {
                        pickedPoint = uidoc.Selection.PickPoint(snapType, message);
                    }
                    catch (Autodesk.Revit.Exceptions.OperationCanceledException ex)
                    {
                        break;
                    }
                    points.Add(pickedPoint);
                }
                return points;
            }
        }

        public static List<T> GetSymbols<T>(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var list = new FilteredElementCollector(doc)
                .OfClass(typeof(T))
                .Cast<T>()
                .ToList();

            return list;
        }

        public static List<Element> SelectElements(ExternalCommandData commandData, Type elementT, string message = "Выберите элементы")
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            IList<Reference> refList = null;

            try
            {
            refList = uidoc.Selection.PickObjects(ObjectType.Element, new UserFilter(elementT), message);
            }
            catch (Exception)
            {
                if (refList == null)
                {
                    return null;
                }
            }
            var elemList = new List<Element>();
            elemList = refList.Select(ob => doc.GetElement(ob)).ToList();

            return elemList;
        }

        public static List<Element> SelectAllElements(ExternalCommandData commandData, Type elementT)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementFilter Filter = new ElementClassFilter(elementT);

            var list = new FilteredElementCollector(doc)
                .WherePasses(Filter)
                .WhereElementIsNotElementType()
                .ToList();

            return list;
        }

        public static List<Element> SelectAllElements(ExternalCommandData commandData, BuiltInCategory elementCat)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            ElementFilter Filter = new ElementCategoryFilter(elementCat);

            var list = new FilteredElementCollector(doc)
                .WherePasses(Filter)
                .WhereElementIsNotElementType()
                .WhereElementIsViewIndependent()
                .ToList();
            return list;
        }

        public static List<FamilySymbol> GetSymbolsOfCat(ExternalCommandData commandData, BuiltInCategory elementCat)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var list = new FilteredElementCollector(doc)
                            .WhereElementIsElementType()
                            .OfClass(typeof(FamilySymbol))
                            .OfCategory(elementCat)
                            .Cast<object>()
                            .Cast<FamilySymbol>()
                            .ToList();

            return list;
        }

        public static List<FamilySymbol> GetFamilySymbols(ExternalCommandData commandData)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            var familySymbols = new FilteredElementCollector(doc)
                                    .OfClass(typeof(FamilySymbol))
                                    .Cast<FamilySymbol>()
                                    .ToList();
            return familySymbols;
        }

    }
}
