using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class MyEx02_CreateBackground : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Document doc = commandData.Application.ActiveUIDocument.Document;
                View view = doc.ActiveView;
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;

                string iconsPath = @"C:\Users\lebro\source\repos\МОИ_ПРОЕКТЫ\БЛИЖАЙШИЕ РАЗРАБОТКИ\RevitAPI\RevitExample\RevitExample\bin\Debug\image\R1_logo.png";
                if(!File.Exists(iconsPath))
                {
                    TaskDialog.Show("Ошибка", "Файл изображения не найден по указанному пути.");
                    return Result.Failed;
                }
                UV imageOffset = new UV(0, 0);
                UV imageScales = new UV(1, 1);
                using (Transaction transaction = new Transaction(doc))
                {
                    transaction.Start("Insert background");
                    view.SetBackground(ViewDisplayBackground.CreateImage(iconsPath, ViewDisplayBackgroundImageFlags.None, imageOffset, imageScales));
                    transaction.Commit();
                }
                
                //using (Transaction transaction = new Transaction(doc, "Вставка изображения"))
                //{
                //    transaction.Start();
                //    ImageType imageType = ImageType.Create(doc, iconsPath);
                //    UV point = new UV(0, 0);
                //    if(imageType != null)
                //    {
                //        doc.Create.NewImageInstance(imageType, point, view.Id);
                //    }
                //}
                
                
                return Result.Succeeded;
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Ошибка", ex.Message);
                return Result.Failed;
            }
        }
    }
}
