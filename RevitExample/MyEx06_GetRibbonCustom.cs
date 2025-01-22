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
    public class MyEx06_GetRibbonCustom : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;
            UIApplication app = commandData.Application;

            List<RibbonPanel> listRibbonPanel = app.GetRibbonPanels("Базовый курс");
            foreach (RibbonPanel panel in listRibbonPanel)
            {
                //panel.Enabled = false;
                string name = panel.Name;
                string title = panel.Title;
                bool enabled = panel.Enabled;
                IList<RibbonItem> item = panel.GetItems();
                foreach (RibbonItem itemItem in item)
                {
                    string nameRibbon = itemItem.Name;
                    string toolTipRibbon = itemItem.ToolTip;
                    string longDescriptionRibbon = itemItem.LongDescription;
                    string itemText = itemItem.ItemText;
                    bool enablrdRibbon = itemItem.Enabled;
                    bool visiableRibbont = itemItem.Visible;
                    if (itemText == "Приветствие" || itemText == "Перенос")
                    {
                        if (enablrdRibbon)
                            itemItem.Enabled = false;
                        else
                            itemItem.Enabled = true;
                    }
                    if (itemText == "Выбрать рамкой")
                    {
                        if (visiableRibbont)
                            itemItem.Visible = false;
                        else
                            itemItem.Visible = true;
                    }
                }
            }
            string a = "";
            return Result.Succeeded;
        }

    }
}
