using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;


namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class Example12_Selection_PickObject : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            Selection selection = uIDocument.Selection;

            Reference elementRef = null;
            try
            {
                elementRef = selection.PickObject(ObjectType.PointOnElement, "Выберите перегородку кабельного лотка (Esc - отмена)");
            }
            catch(OperationCanceledException e)
            {
                return Result.Cancelled;
            }

            Element selectedElement = document.GetElement(elementRef);

            TaskDialog.Show("Результаты анализа", $"Пользователь указал элемент: \n{selectedElement.Name}\n" +
                $"\nКоординаты точки:\n{elementRef.GlobalPoint}");

            using (Transaction transaction = new Transaction(document))
            {
                transaction.Start("Delete element");
                document.Delete(elementRef.ElementId);
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    public class Example12_Selection_PickObjects : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;
            Selection selection = uIDocument.Selection;
            List<Reference> elementRefs = null;
            try
            {
                elementRefs = selection.PickObjects(ObjectType.Element, new SeparatorSelectionFilter(), "Выберите стальные балки (Esc - отмена)").ToList();
            }
            catch(OperationCanceledException e)
            {
                return Result.Cancelled;
            }
            using(Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Delete elements");
                elementRefs.ForEach(x=>doc.Delete(x.ElementId));
                transaction.Commit();   
            }
            return Result.Succeeded;
        }
    }


    [Transaction(TransactionMode.Manual)]
    public class Example12_Selection_PickElementsByRectangle : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;
            Selection selection = uIDocument.Selection;
            List<Element> selectedElements = null;
            try
            {
                selectedElements = selection.PickElementsByRectangle(new SeparatorSelectionFilter(),
                    "!!!Выберите стальные балки (Esc - отмена").ToList();
            }
            catch(OperationCanceledException e)
            {
                return Result.Cancelled;
            }
            selection.SetElementIds(selectedElements.Select(it=>it.Id).ToList());
            return Result.Succeeded;
        }
    }

    public class SeparatorSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            return elem is FamilyInstance instance && instance.Symbol.FamilyName == "ADSK_Балка_Двутавр_ГОСТ Р 57837-2017";
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
