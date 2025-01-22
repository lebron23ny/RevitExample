using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Linq;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class MyEx05_PaintFace : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;
            Reference myRef = uidoc.Selection.PickObject(ObjectType.Face);

            Material material = new FilteredElementCollector(doc).OfClass(typeof(Material)).First(mat=>mat.Name == "ADSK_Бетон_Железобетон_В25_лампочка") as Material;

            Element element = doc.GetElement(myRef);

            Options options = new Options()
            {
                View = doc.ActiveView,
                ComputeReferences = true,
            };
            GeometryElement geometry = element.get_Geometry(options);

            using (Transaction t = new Transaction(doc, "Раскраска элементов"))
            {
                t.Start();
                foreach(GeometryObject obj  in geometry)
                {
                    if(obj is Solid)
                    {
                        var solid = obj as Solid;
                        foreach(Face face in solid.Faces)
                        {
                            if (face.Reference.EqualTo(myRef))
                                doc.Paint(element.Id, face, material.Id);
                        }
                    }
                }
                t.Commit();
            }    

            return Result.Succeeded;
        }
    }
}
