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
    public class Example01_Transaction : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document document = uiDoc.Document;

            XYZ vectorZero = XYZ.Zero;
            XYZ vectorX = XYZ.BasisX;
            XYZ vectorY = XYZ.BasisY;
            XYZ vectorZ = vectorX.CrossProduct(vectorY);

            CurveArray curveArray = new CurveArray();
            curveArray.Append(Arc.Create(XYZ.Zero, 10, 0, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY));
            curveArray.Append(Arc.Create(XYZ.Zero, 8, Math.PI, 2 * Math.PI, XYZ.BasisX, XYZ.BasisY));
            curveArray.Append(Arc.Create(new XYZ(-8, 0, 0), new XYZ(8, 0, 0), new XYZ(0, -3, 0)));
            curveArray.Append(Arc.Create(new XYZ(-5, 2, 0), 3, 0, Math.PI, XYZ.BasisX, XYZ.BasisY));
            curveArray.Append(Arc.Create(new XYZ(5, 2, 0), 3, 0, Math.PI, XYZ.BasisX, XYZ.BasisY));

            View activView = document.ActiveView as View;

            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("Safety transaction");
                document.Create.NewDetailCurveArray(activView, curveArray);
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
