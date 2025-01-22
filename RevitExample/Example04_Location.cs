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
    public class Example04_Location_Move : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            Wall wall = new FilteredElementCollector(document).OfClass(typeof(Wall)).First() as Wall;

            LocationCurve wallLocation = wall.Location as LocationCurve;
            Curve locationCurve = wallLocation.Curve as Curve;
            double wallLength = UnitUtils.ConvertFromInternalUnits(locationCurve.Length, UnitTypeId.Meters);
            TaskDialog.Show("Результаты анализа", $"Длина стены: {wallLength:f2} м");
            XYZ wallTranslation = new XYZ(0, -3, 0);
            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("Move wall");
                wallLocation.Move(wallTranslation);
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Example05_Location_Rotate : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument= commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            
            FamilyInstance couch = new FilteredElementCollector(document)
                .OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_Furniture)
                .First() as FamilyInstance;

            LocationPoint couchLocation = couch.Location as LocationPoint;
            XYZ couchLocationPoint = couchLocation.Point;
            TaskDialog.Show("Результаты анализа", $"Координаты дивана (футы): {couchLocationPoint}");
            Line rotationAxis = Line.CreateBound(XYZ.Zero, XYZ.Zero + new XYZ(0, 0, 1));
            using(Transaction transaction = new Transaction(document))
            {
                transaction.Start("Rotate couch");
                couchLocation.Rotate(rotationAxis, System.Math.PI / 4);
                transaction.Commit();
            }    
            return Result.Succeeded;
        }
    }

}
