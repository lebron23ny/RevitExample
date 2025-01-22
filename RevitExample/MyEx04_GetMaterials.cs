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
    public class MyEx04_GetMaterials : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            List<string> listNameMaterial = new List<string>();

            List<Material> listMaterials = new FilteredElementCollector(document).OfClass(typeof(Material))
                .Cast<Material>().Where(x=>x.Name.Contains("Бетон")).OrderBy(y=>y.Name).ToList();

            foreach (Material material in listMaterials)
            {
                Material currentMaterail = material as Material;
                string name = material.Name;
                listNameMaterial.Add(name);
            }
            return Result.Succeeded;
        }
    }
}
