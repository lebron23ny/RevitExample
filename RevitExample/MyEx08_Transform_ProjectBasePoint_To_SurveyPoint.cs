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
    public class MyEx08_Transform_ProjectBasePoint_To_SurveyPoint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document doc = uIDocument.Document;
            XYZ projectPoint = new XYZ(3950300, 3254400, 468700);
            // Получение активного местоположения проекта
            ProjectLocation projectLocation = doc.ActiveProjectLocation;

            // Получение трансформации из Project Base Point в Survey Point
            Transform projectToSurveyTransform = projectLocation.GetTotalTransform();

            // Преобразование точки в систему "Истинный север" (Survey Point)
            XYZ surveyPoint = projectToSurveyTransform.OfPoint(projectPoint);

            // Вывод преобразованных координат
            TaskDialog.Show("Координаты", $"Survey Point: X={surveyPoint.X}, Y={surveyPoint.Y}, Z={surveyPoint.Z}");

            ProjectLocation _projectLocation = doc.ActiveProjectLocation;
            // Точка в системе "Истинный север" (Survey Point)
            XYZ _surveyPoint = new XYZ(3950300, 3254400, 468700);

            // Получение трансформации из Survey Point в Project Base Point
            Transform _surveyToProjectTransform = _projectLocation.GetTotalTransform().Inverse;

            // Преобразование точки в систему "Условный сервер" (Project Base Point)
            XYZ _projectPoint = _surveyToProjectTransform.OfPoint(_surveyPoint);

            // Вывод преобразованных координат
            TaskDialog.Show("Координаты", $"Project Base Point: X={_projectPoint.X}, Y={_projectPoint.Y}, Z={_projectPoint.Z}");
            return Result.Succeeded;
        }
    }
}
