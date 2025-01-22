using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Visual;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitExample
{
    [Transaction(TransactionMode.Manual)]
    public class MyEx03_CreateMaterial : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;
            string matName = "Material_01";
            string matClass = "Masonry";
            string assetName = "AppearenceAsset_01";


            #region Вкладка Graphics
            string surforePat = "ADSK_Брус фасадный_140 мм";
            string surbackPat = "ADSK_Бетон_Разрез_Вверх_1.5 мм";
            string cutforePat = "ADSK_Бетон_Разрез_Вверх_1.5 мм";
            string cutbackPat = "ADSK_Бетон_Разрез_Вверх_1.5 мм";
           
            Color uniColor = new Color(255, 230, 230);
            Color patColor = new Color(0, 0, 0);
            int transPar = 0;
            #endregion

            //Some tooltips for the user
            TaskDialog tipPed = new TaskDialog("Texture");
            tipPed.MainContent = "Chouse your texture!";
            tipPed.Show();

            //Create and show FileOpenDialog
            FileOpenDialog textBitmap = new FileOpenDialog("Image Files|*.jpg;*jpeg;*.png");
            textBitmap.Show();

            //Set the file path to texture
            string texturePath = ModelPathUtils.ConvertModelPathToUserVisiblePath(textBitmap.GetSelectedModelPath());

            //Modify document within a transaction
            using(Transaction transaction = new Transaction(doc))
            {
                transaction.Start("Transaction Name");
                ElementId newMatId = Material.Create(doc, matName);

                //Harvest the newly created material from Revit
                Material solMat = doc.GetElement(newMatId) as Material;

                //Set the material class
                solMat.MaterialClass = matClass;

                //Set ther color
                solMat.Color = uniColor;

                //Set the material transparency
                solMat.Transparency = transPar;

                //Set the SurfaceForegroundPattern and Color
                solMat.SurfaceForegroundPatternId = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Model, surforePat).Id;
                solMat.SurfaceForegroundPatternColor = patColor;

                //Set the SurfaceForegroundPattern and Color
                solMat.SurfaceBackgroundPatternId = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, surbackPat).Id;
                solMat.SurfaceBackgroundPatternColor = uniColor;

                //Set the CutForegroundPattern and Color
                solMat.CutForegroundPatternId = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, cutforePat).Id;
                solMat.CutForegroundPatternColor = patColor;

                //Set the CutBackgroundPattern and Color
                solMat.CutBackgroundPatternId = FillPatternElement.GetFillPatternElementByName(doc, FillPatternTarget.Drafting, cutbackPat).Id;
                solMat.CutBackgroundPatternColor = uniColor;

                //Get your starting ApperanceAssetElement and duplicate it

                AppearanceAssetElement temlAsset = AppearanceAssetElement.GetAppearanceAssetElementByName(doc, "ADSK_Бетон");
                AppearanceAssetElement newAsset = temlAsset.Duplicate(assetName);

                solMat.AppearanceAssetId = newAsset.Id;

                //Change the Image In The ApperanceAsset
                using(AppearanceAssetEditScope editEd = new AppearanceAssetEditScope(newAsset.Document))
                {
                    Asset editableAsset = editEd.Start(newAsset.Id);
                    

                    int size = editableAsset.Size;
                    List<string> listAssetProperty = new List<string>();
                    AssetProperty assetProperty = editableAsset.Get(0);
                    for(int idx = 0; idx < size; idx++)
                    {
                        AssetProperty assetProperty1 = editableAsset.Get(idx);
                        listAssetProperty.Add(assetProperty1.Name);
                    }


                    
                    AssetProperty texTure = editableAsset.FindByName("generic_diffuse");
                    Asset connectedAsset = texTure.GetSingleConnectedAsset();

                    string name = connectedAsset.Name;

                    if(connectedAsset.Name == "UnifiedBitmapSchema")
                    {
                        AssetPropertyString path = connectedAsset.FindByName(UnifiedBitmap.UnifiedbitmapBitmap) as AssetPropertyString;
                        if(path.IsValidValue(texturePath))
                            path.Value = texturePath;


                    }


                    editEd.Commit(true);
                }


                transaction.Commit();   
            }


            return Result.Succeeded;
        }
    }
}
