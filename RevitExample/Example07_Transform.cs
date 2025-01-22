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
    public class Example07_Transform_Translation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                .Cast<FamilyInstance>().Where(x=>x.Name == "20Ш2").FirstOrDefault() as FamilyInstance;

            Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5));

            using(Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");
                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, translation, true);
                transaction.Commit();
            }
            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Example07_Tranform_Rotation : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>().Where(x => x.Name == "20Ш2").FirstOrDefault() as FamilyInstance;

            Transform rotation = Transform.CreateRotation(XYZ.BasisZ, Math.PI / 4);
            using( Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");
                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, rotation, true); 
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class Example07_Transform_Complex : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            FamilyInstance adaptiveInstance = new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                        .Cast<FamilyInstance>().Where(x => x.Name == "20Ш2").FirstOrDefault() as FamilyInstance;

            Transform translation = Transform.CreateTranslation(new XYZ(0, 0, 5));
            Transform rotation = Transform.CreateRotation(XYZ.BasisZ, Math.PI/4);
            Transform complex = translation * rotation;

            using(Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Translate adaptive component");
                AdaptiveComponentInstanceUtils.MoveAdaptiveComponentInstance(adaptiveInstance, complex, true);
                transaction.Commit();
            }    

            return Result.Succeeded;
        }
    }


}
