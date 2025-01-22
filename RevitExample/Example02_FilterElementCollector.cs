
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class Example02_FilterElementCollector : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            List<CurveElement> curveElements = new FilteredElementCollector(document).OfCategory(BuiltInCategory.OST_Lines).
                    Cast<CurveElement>().Where(it=>it.CurveElementType == CurveElementType.DetailCurve).ToList();
            using(Transaction transaction = new Transaction(document))
            {
                transaction.Start("Delete all detail curves");
                foreach(CurveElement curveElement in curveElements)
                {
                    document.Delete(curveElement.Id);
                }
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
