using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.Exceptions;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class Example11_PickPoint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;

            View activeView = doc.ActiveView;
            if(!(activeView is ViewPlan))
            {
                TaskDialog errorDialog = new TaskDialog("Ошибка")
                {
                    MainInstruction = "Данная команда предназначена только для работы на планах",
                   VerificationText = "Дополнительное окно",
                   CommonButtons = TaskDialogCommonButtons.Yes | TaskDialogCommonButtons.No,
                   DefaultButton = TaskDialogResult.Yes,
                   FooterText = "<a href=\"https://bim.vc/edu/courses\">" + "Получить дополнительные сведения</a>"
                };

                errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Открыть первый попавшийся план");
                errorDialog.AddCommandLink(TaskDialogCommandLinkId.CommandLink2, "Завершить работу команды");
                TaskDialogResult dialogResult = errorDialog.Show();

                if(errorDialog.WasVerificationChecked())
                {
                    TaskDialog.Show("Дополнительное окно", "Привет!)");
                }

                if(dialogResult == TaskDialogResult.CommandLink1 || dialogResult == TaskDialogResult.Yes)
                {
                    activeView = new FilteredElementCollector(doc).OfClass(typeof(ViewPlan)).FirstElement() as ViewPlan;
                    uIDocument.ActiveView = activeView;
                }
                else if(dialogResult == TaskDialogResult.No)
                {
                    TaskDialog.Show("Предупреждение", "Как это нет? Ээээх");
                    return Result.Cancelled;
                }
                else
                    return Result.Failed;
            }

            Selection selection = uIDocument.Selection;
            ObjectSnapTypes snapTypes = ObjectSnapTypes.Centers | ObjectSnapTypes.Midpoints;

            using(Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Create polyline");
                XYZ lastPoint = null;
                while(true)
                {
                    try
                    {
                        if(lastPoint == null)
                        {
                            lastPoint = selection.PickPoint(snapTypes, "Укажите начальную точку (Esc - отмена)");
                        }

                        XYZ currentPoint = selection.PickPoint(snapTypes, "Укажите слелующую точку (Esc - запершить)");
                        Line line = Line.CreateBound(lastPoint, currentPoint);
                        lastPoint = currentPoint;

                        doc.Create.NewDetailCurve(activeView, line);
                        doc.Regenerate();
                    }
                    catch(OperationCanceledException e)
                    {
                        break;
                    }
                }
                transaction.Commit();
            }

            return Result.Succeeded;
        }
    }
}
