using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Events;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class Example03_1_NewFamilyInstanceFurniture : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
           
            
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;

            Document document = uIDocument.Document;

            FamilySymbol familySymbol = new FilteredElementCollector(document).OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_Furniture)
                .Cast<FamilySymbol>()
                .First(it=>it.FamilyName=="Диван-Pensi" && it.Name == "1650 мм");

            XYZ counchLocation = XYZ.Zero;

            Level level = new FilteredElementCollector(document).OfClass(typeof(Level)).FirstElement() as Level;

            using(Transaction transaction = new Transaction(document))
            {
                transaction.Start("Insert couch");
                if(!familySymbol.IsActive)
                {
                    familySymbol.Activate();
                }
                document.Create.NewFamilyInstance(counchLocation, familySymbol, level, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                transaction.Commit();
            }

            return Result.Succeeded;
        }


    }






    [Transaction(TransactionMode.Manual)]
    public class Example03_2_NewFamilyInstanceDoor : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;
            FamilySymbol doorSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).
                OfCategory(BuiltInCategory.OST_Doors).Cast<FamilySymbol>().First(it => it.FamilyName == "Дверь-Одинарная-Панель" && it.Name == "750 х 2000 мм");

            IEnumerable<Element> walls = new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToElements();

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert doors");
                if (!doorSymbol.IsActive)
                {
                    doorSymbol.Activate();
                }
                foreach (Element wall in walls)
                {
                    Curve wallCurve = (wall.Location as LocationCurve).Curve;
                    XYZ wallCenter = wallCurve.Evaluate(0.5, true);
                    doc.Create.NewFamilyInstance(wallCenter, doorSymbol, wall, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
                }
            }
            return Result.Succeeded;
        }
    }
}
