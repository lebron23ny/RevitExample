using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit;
using Autodesk.Revit.Attributes;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class Example00_MyFirstCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            TaskDialog.Show("Сообщение", "Привет, мир!");
            return Result.Succeeded;
        }
    }
}
