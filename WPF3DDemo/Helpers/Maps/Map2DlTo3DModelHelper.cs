using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using WPF3DDemo.Helpers.Visual3Ds;
using WPF3DDemo.Models;
using WPF3DDemo.Models.Map2Ds;
using WPF3DDemo.Models.Maps;
using WPF3DDemo.Models.Visuals;

namespace WPF3DDemo.Helpers
{
    public class Map2DTo3DHelper
    {
        private static List<Point> CleanPointList(List<Point> polygon)
        {
            List<Point> cleanPointList = new List<Point>();
            cleanPointList.Add(polygon[0]);

            for (int i = 1; i < polygon.Count - 1; i++)
            {
                if (polygon[i] != polygon[i - 1])
                {
                    cleanPointList.Add(polygon[i]);
                }
            }

            if (polygon.Last() != polygon.First())
            {
                cleanPointList.Add(polygon.Last());
            }

            return cleanPointList;
        }

        public static Map3DModel Map2DTo3DModel(Map2DModel map2D, double upperZValue, double lowerZValue)
        {
            Map3DModel map3D = new Map3DModel();
            map3D.Id = map2D.Id;
            map3D.Name = map2D.Name;

            List<CylinderVisual3DModel> cylinderVisual3DModelList = new List<CylinderVisual3DModel>();
            map3D.Geometries = cylinderVisual3DModelList;

            Rect rect = GetBoundaryRectOfMap2D(map2D);

            foreach (List<Point> map2DPathFeaturePoints in map2D.GeometryPointList)
            {
                List<Point> cleanPointList = CleanPointList(map2DPathFeaturePoints);
                //CylinderVisual3DModel cylinderVisual3DModel = Visual2DTo3DHelper.Closed2DAreaToCylinderVisual3DModel(cleanPointList, upperZValue, lowerZValue);
                CylinderVisual3DModel cylinderVisual3DModel = Visual2DTo3DHelper.Closed2DAreaToCylinderVisual3DModel(cleanPointList, upperZValue, lowerZValue, rect);
                if (cylinderVisual3DModel != null)
                {
                    cylinderVisual3DModelList.Add(cylinderVisual3DModel);
                }
            }

            //map3D.Materials = new List<List<Material3DModel>>();
            //for (int i = 0; i < map2D.GeometryPointList.Count; i++)
            //{
            //    List<Material3DModel> materialList = new List<Material3DModel>()
            //    {
            //         new Material3DModel(){ MaterialType = MaterialType.ColorDiffuse, MaterialData = Color.FromArgb(0x99, 0x00, 0xff, 0xff) },
            //         new Material3DModel(){ MaterialType = MaterialType.ColorDiffuse, MaterialData = Color.FromArgb(0x99, 0x00, 0xff, 0xff) },
            //         new Material3DModel(){ MaterialType = MaterialType.ColorDiffuse, MaterialData = Color.FromArgb(0xff, 0x48, 0x3d, 0x88) }
            //    };

            //    map3D.Materials.Add(materialList);
            //}

            return map3D;
        }

        private static Rect GetBoundaryRectOfMap2D(Map2DModel map2D)
        {
            List<Point> allFeaturePoints = new List<Point>();
            foreach (List<Point> map2DPathFeaturePoints in map2D.GeometryPointList)
            {
                allFeaturePoints.AddRange(map2DPathFeaturePoints);
            }
            Rect rect = FeaturePointHelper.GetFeaturePointsBoundaryRect(allFeaturePoints);
            return rect;
        }

        public static ContainerUIElement3D Map3DModelToUIElement3D(Map3DModel map3D)
        {
            List<Viewport2DVisual3D> visual3DList = new List<Viewport2DVisual3D>();
            for (int i = 0; i < map3D.Geometries.Count; i++)
            {
                CylinderVisual3DModel cylinderVisual3DModel = map3D.Geometries[i];

                DiffuseMaterial imageMaterial = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(string.Format(@"Images\MapTextures\{0}.png", "1"), UriKind.Relative))));
                //MeshGeometry3D upperGeometry3D = Visual3DCreateHelper.CreateGeometry3D(cylinderVisual3DModel.UpperUndersurface);
                //Material upperMaterial = Visual3DCreateHelper.CreateMaterial3D(map3D.Materials[i][0]);

                //MaterialGroup mg = new MaterialGroup();
                //mg.Children.Add(upperMaterial);
                ////mg.Children.Add(imageMaterial);

                //Viewport2DVisual3D upperVisual3D = Visual3DCreateHelper.CreateVisual3D(upperGeometry3D, mg);
                //visual3DList.Add(upperVisual3D);

                MeshGeometry3D lowerGeometry3D = Visual3DCreateHelper.CreateGeometry3D(cylinderVisual3DModel.LowerUndersurface);
                //Material lowerMaterial = Visual3DCreateHelper.CreateMaterial3D(map3D.Materials[i][1]);
                //MaterialGroup mg1 = new MaterialGroup();
                //mg1.Children.Add(lowerMaterial);
                //mg1.Children.Add(imageMaterial);
                Viewport2DVisual3D lowerVisual3D = Visual3DCreateHelper.CreateVisual3D(lowerGeometry3D, imageMaterial);
                visual3DList.Add(lowerVisual3D);


                //MeshGeometry3D sideGeometry3D = Visual3DCreateHelper.CreateGeometry3D(cylinderVisual3DModel.SideSurface);
                //Material sideMaterial = Visual3DCreateHelper.CreateMaterial3D(map3D.Materials[i][2]);
                //Viewport2DVisual3D sideVisual3D = Visual3DCreateHelper.CreateVisual3D(sideGeometry3D, sideMaterial);
                //visual3DList.Add(sideVisual3D);
            }
            ContainerUIElement3D uIElement3D = Visual3DCreateHelper.CreateUIElement3D(visual3DList);

            return uIElement3D;
        }

    }
}
