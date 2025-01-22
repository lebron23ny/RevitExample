using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class MyEx10_Rotate_Transform : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            double angle = 90;


            XYZ projectPoint = new XYZ(3254400, 3950300, 468700);

            XYZ origin = XYZ.Zero;
            XYZ vectorX = XYZ.BasisX;
            XYZ vectorY = XYZ.BasisY;
            XYZ vectorZ = XYZ.BasisZ;

            Transform transformGlobal = Transform.Identity;
            transformGlobal.BasisX = vectorX;
            transformGlobal.BasisY = vectorY;
            transformGlobal.BasisZ = vectorZ;
            transformGlobal.Origin = origin;

            Transform rotate = Transform.CreateRotation(XYZ.BasisZ, Math.PI * angle / 180);
            Transform result = transformGlobal.Multiply(rotate);

            XYZ newPoint = result.Inverse.OfPoint(projectPoint);
            return Result.Succeeded;
        }
    }
}
