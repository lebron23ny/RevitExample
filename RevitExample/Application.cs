using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Events;
using System.Windows.Forms;

namespace RevitExample
{
    public class Application : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {


            #region Регистрация события

            //application.SelectionChanged += Application_SelectionChanged;
            //try
            //{
            //    application.ControlledApplication.DocumentSavedAs += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedAsEventArgs>(Document_SaveAS);
            //    application.ControlledApplication.DocumentSaved += new EventHandler<Autodesk.Revit.DB.Events.DocumentSavedEventArgs>(Document_Saved);
            //}
            //catch (Exception ex)
            //{
            //    return Result.Failed;
            //}
            #endregion


            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string iconsDirectoryPath = Path.GetDirectoryName(assemblyLocation) + @"\icons\";
            string tabName = "Базовый курс";
            application.CreateRibbonTab(tabName);
            #region Первый плагин

            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Первый плагин");
                panel.AddItem(new PushButtonData(nameof(Example00_MyFirstCommand), "Приветствие", assemblyLocation,
                     typeof(Example00_MyFirstCommand).FullName)
                    { LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))});
            }
            #endregion
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Изменение модели");
                #region Вставка смайлика
                panel.AddItem(new PushButtonData(nameof(Example01_Transaction), "Транзакция", assemblyLocation,
                    typeof(Example01_Transaction).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                }
                    );
                panel.AddSeparator();

                #endregion


                #region FilterElementCollector

                panel.AddItem(new PushButtonData(nameof(Example02_FilterElementCollector), "Поиск", assemblyLocation,
                    typeof(Example02_FilterElementCollector).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                }
                    );
                panel.AddSeparator();
                #endregion

                #region Вставка семейства
                panel.AddItem(new PushButtonData(nameof(Example03_1_NewFamilyInstanceFurniture),
                    "Разместить\nдиван",
                    assemblyLocation,
                    typeof(Example03_1_NewFamilyInstanceFurniture).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                }
                    );
                #endregion



                #region Location
                panel.AddItem(new PushButtonData(nameof(Example04_Location_Move), "Location\nПеренос", assemblyLocation, typeof(Example04_Location_Move).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                }
                    );
                panel.AddSeparator();
                #endregion


                #region Transform
                panel.AddItem(new PushButtonData(nameof(Example07_Transform_Translation), "Перенос", assemblyLocation, typeof(Example07_Transform_Translation).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                }
                    );
                panel.AddItem(new PushButtonData(nameof(Example07_Tranform_Rotation), "Вращение", assemblyLocation, typeof(Example07_Tranform_Rotation).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png"))
                }
                    );
                panel.AddItem(new PushButtonData(nameof(Example07_Transform_Complex), "Перенос", assemblyLocation, typeof(Example07_Transform_Complex).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                }
                    );
                panel.AddSeparator();
                #endregion


                #region PickPoint
                panel.AddItem(new PushButtonData(nameof(Example11_PickPoint), "Создать\nполилинию", assemblyLocation, typeof(Example11_PickPoint).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "orange.png")),
                    LongDescription = "LongDescription",
                    ToolTip = "Tool tip"
                }
                    );
                panel.AddSeparator();
                #endregion
            }
            {
                RibbonPanel panel = application.CreateRibbonPanel(tabName, "Взаимодействие");
                #region Selection
                panel.AddItem(new PushButtonData(nameof(Example12_Selection_PickObject), "Выбрать\nодин", assemblyLocation, typeof(Example12_Selection_PickObject).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "green.png"))
                }
                    );
                panel.AddItem(new PushButtonData(nameof(Example12_Selection_PickObjects), "Выбрать\nмного", assemblyLocation, typeof(Example12_Selection_PickObjects).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "blue.png"))
                }
                    );
                panel.AddItem(new PushButtonData(nameof(Example12_Selection_PickElementsByRectangle), "Выбрать рамкой", assemblyLocation, typeof(Example12_Selection_PickElementsByRectangle).FullName)
                {
                    LargeImage = new BitmapImage(new Uri(iconsDirectoryPath + "red.png"))
                }
                    );


                panel.AddSeparator();
                #endregion
            }



            
            return Result.Succeeded;
        }

        //private void Application_SelectionChanged(object sender, Autodesk.Revit.UI.Events.SelectionChangedEventArgs e)
        //{
        //    MessageBox.Show($"Произошло событие изменение выделения");
        //}

        private void Document_Saved(object sender, DocumentSavedEventArgs e)
        {
            MessageBox.Show($"Документ {e.Document.Title} сохранен");
        }

        private void Document_SaveAS(object sender, DocumentSavedAsEventArgs e)
        {
            MessageBox.Show($"Документ сохранен как {e.Document.Title}"); 
        }
    }
}
