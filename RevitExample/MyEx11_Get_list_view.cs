using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class MyEx11_Get_list_view : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            List<View> list = new FilteredElementCollector(document).OfClass(typeof(View)).Cast<View>().ToList();
            foreach (View view in list)
            {
                string name = view.Name;
                string titel = view.Title;
                ElementId id = view.Id;
            }

            return Result.Succeeded;
        }
    }
}
