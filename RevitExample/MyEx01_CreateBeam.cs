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
    public class MyEx01_CreateBeam : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {

            UIApplication app = commandData.Application;
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            View active_view = doc.ActiveView;
            Level active_level = doc.ActiveView.GenLevel;



            double length_mm = 20000;

            XYZ pt_start = new XYZ(0, 0, 0);
            XYZ pt_end = new XYZ(UnitUtils.ConvertToInternalUnits(length_mm, UnitTypeId.Millimeters), 0, 0);
            Line line = Line.CreateBound(pt_start, pt_end);

            FamilySymbol familySymbol = new FilteredElementCollector(doc)               
                    .OfClass(typeof(FamilySymbol))
                    .OfCategory(BuiltInCategory.OST_StructuralFraming)
                    .Cast<FamilySymbol>()
                    .First(it => it.FamilyName == "ADSK_Балка_Двутавр_ГОСТ Р 57837-2017" && it.Name == "18Б1");

            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Start");
                if(!familySymbol.IsActive)
                {
                    familySymbol.Activate();
                }
                doc.Create.NewFamilyInstance(line, familySymbol, active_level, Autodesk.Revit.DB.Structure.StructuralType.Beam);
                transaction.Commit();
            }
            return Result.Succeeded;
        }
    }
}
