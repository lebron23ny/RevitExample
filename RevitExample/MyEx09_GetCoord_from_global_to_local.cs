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
    public class MyEx09_GetCoord_from_global_to_local : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            XYZ projectPoint = new XYZ(3254400, 3950300, 468700);

            XYZ origin = new XYZ(0, 0 ,0);
            XYZ vectorX = XYZ.BasisY;
            XYZ vectorY = -XYZ.BasisX;
            XYZ vectorZ = XYZ.BasisZ;
            // Создание Transform
            Transform transform = Transform.Identity;

            // Установка базисных векторов
            transform.BasisX = vectorX;
            transform.BasisY = vectorY;
            transform.BasisZ = vectorZ;

            // Установка начала координат
            transform.Origin = origin;

            XYZ transformedPoint = transform.Inverse.OfPoint(projectPoint);
            TaskDialog.Show("Координаты", $"Project Base Point: X={transformedPoint.X}, Y={transformedPoint.Y}, Z={transformedPoint.Z}");

            return Result.Succeeded;
        }
    }
}
