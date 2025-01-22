using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExample
{
    internal class Example06_AdaptiveComponentInstanceUtils : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            List<FamilyInstance> supports =new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
                .OfCategory(BuiltInCategory.OST_GenericModel)
                .Cast<FamilyInstance>()
                .Where(x=>x.Symbol.FamilyName == "VC_Опора")
                .ToList();

            FamilySymbol wireSymbol = new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol))
                .OfCategory(BuiltInCategory.OST_GenericModel).Cast<FamilySymbol>().First(x => x.FamilyName == "VC_Провод");


            using (Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Insert adaptive component");
                if (!wireSymbol.IsActive)
                {
                    wireSymbol.Activate();
                }

                for(int supportIndex = 0; supportIndex < supports.Count - 1; supportIndex++)
                {
                    FamilyInstance currentSupport = supports[supportIndex];
                    FamilyInstance nextSupport = supports[supportIndex + 1];
                    
                    List<FamilyInstance> currentConnectors = currentSupport.GetSubComponentIds()
                        .Select(x => doc.GetElement(x) as FamilyInstance)
                        .Where(x => x.Symbol.FamilyName == "VC_Коннектор провода")
                        .ToList();

                    List<FamilyInstance> nextConnectors = nextSupport.GetSubComponentIds()
                        .Select (x => doc.GetElement(x)as FamilyInstance)
                        .Where(x=>x.Symbol.FamilyName == "VC_Коннектор провода")
                        .ToList();

                    XYZ currentA = (currentConnectors.First(x => x.Symbol.Name == "A").Location as LocationPoint).Point,
                        currentB = (currentConnectors.First(x => x.Symbol.Name == "B").Location as LocationPoint).Point,
                        currentC = (currentConnectors.First(x => x.Symbol.Name == "C").Location as LocationPoint).Point,
                        nextA = (nextConnectors.First(x => x.Symbol.Name == "A").Location as LocationPoint).Point,
                        nextB = (nextConnectors.First(x => x.Symbol.Name == "B").Location as LocationPoint).Point,
                        nextC = (nextConnectors.First(x => x.Symbol.Name == "C").Location as LocationPoint).Point;

                    FamilyInstance wireA = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol);
                    FamilyInstance wireB = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol);
                    FamilyInstance wireC = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance(doc, wireSymbol);

                    List<ReferencePoint> wireAPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireA)
                        .Select (x => doc.GetElement(x) as ReferencePoint).ToList();
                    List<ReferencePoint> wireBPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireB)
                        .Select(x => doc.GetElement(x)as ReferencePoint).ToList();
                    List<ReferencePoint> wireCPlacementPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds(wireC)
                        .Select(x=>doc.GetElement(x) as ReferencePoint).ToList();

                    XYZ currentTranslationA = currentA - wireAPlacementPoints[0].Position;
                    XYZ currentTranslationB = currentB - wireBPlacementPoints[0].Position;
                    XYZ currentTranslationC = currentC - wireCPlacementPoints[0].Position;
                    XYZ nextTranslationA = nextA - wireAPlacementPoints[1].Position;
                    XYZ nextTranslationB = nextB - wireBPlacementPoints[1].Position;
                    XYZ nextTranslationC = nextC - wireCPlacementPoints[1].Position;

                    wireAPlacementPoints[0].Location.Move(currentTranslationA);
                    wireAPlacementPoints[1].Location.Move(nextTranslationA);
                    
                    wireBPlacementPoints[0].Location.Move(currentTranslationB);
                    wireBPlacementPoints[1].Location.Move(nextTranslationB);

                    wireCPlacementPoints[0].Location.Move(currentTranslationC);
                    wireCPlacementPoints[1].Location.Move(nextTranslationC);
                }
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
