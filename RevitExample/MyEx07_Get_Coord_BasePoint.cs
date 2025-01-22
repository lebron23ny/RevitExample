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
    public class MyEx07_Get_Coord_BasePoint : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uIDocument = commandData.Application.ActiveUIDocument;
            Document document = uIDocument.Document;

            XYZ projectPoint = new XYZ(3254400, 3950300, 468700);
            XYZ sharedPoint = document.ActiveProjectLocation.GetTotalTransform().OfPoint(projectPoint);
            XYZ backToProjectPoint = document.ActiveProjectLocation.GetTotalTransform().Inverse.OfPoint(sharedPoint);

            BasePoint basePoint1 = BasePoint.GetProjectBasePoint(document);
            XYZ positionBP = basePoint1.SharedPosition;
            XYZ pos = basePoint1.Position;

            double _x_BasePoint = UnitUtils.ConvertFromInternalUnits(positionBP.X, UnitTypeId.Millimeters);
            double _y_BasePoint = UnitUtils.ConvertFromInternalUnits(positionBP.Y, UnitTypeId.Millimeters);
            double _z_BasePoint = UnitUtils.ConvertFromInternalUnits(positionBP.Z, UnitTypeId.Millimeters);

            var _projectPosition = document.ProjectInformation;
            var _projectLocation = document.ProjectLocations;

            ProjectLocation projectLocation = document.ActiveProjectLocation;
            ProjectPosition position = projectLocation.GetProjectPosition(XYZ.Zero);

            double eastWest = UnitUtils.ConvertFromInternalUnits(position.EastWest, UnitTypeId.Millimeters); 
            double northSouth = UnitUtils.ConvertFromInternalUnits(position.NorthSouth, UnitTypeId.Millimeters);
            double elevation = UnitUtils.ConvertFromInternalUnits(position.Elevation, UnitTypeId.Millimeters);
            double angleInRadians = position.Angle;
            double angleInDegrees = Math.Round(angleInRadians * (180 / Math.PI), 5);

            TaskDialog.Show("Сообщение", $"Базовая точка проекта:\n" +
                $"X:{_x_BasePoint}\n Y:{_y_BasePoint}\n Z:{_z_BasePoint}");

            TaskDialog.Show("Сообщение", $"Базовая точка проекта:\n В/З:  {eastWest}\n C/Ю: {northSouth}\n Высотная отметка: {elevation}\n Направление истинного угла на север: {angleInDegrees}");

            return Result.Succeeded;
        }
    }
}
