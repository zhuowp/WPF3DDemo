using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using WPF3DDemo.Models;
using WPF3DDemo.Models.Visuals;

namespace WPF3DDemo.Helpers.Visual3Ds
{
    public class Visual3DCreateHelper
    {
        public static Transform3DGroup CreateTransform3DGroup()
        {
            Transform3DGroup transform3DGroup = new Transform3DGroup();

            TranslateTransform3D translateTransform3D = new TranslateTransform3D();
            transform3DGroup.Children.Add(translateTransform3D);

            ScaleTransform3D scaleTransform3D = new ScaleTransform3D();
            transform3DGroup.Children.Add(scaleTransform3D);

            RotateTransform3D rotateTransform3D = new RotateTransform3D();
            transform3DGroup.Children.Add(rotateTransform3D);

            return transform3DGroup;
        }

        public static MeshGeometry3D CreateGeometry3D(Geometry3DModel geometry3DModel)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            if (geometry3DModel == null)
            {
                return mesh;
            }

            if (geometry3DModel.Positions != null)
            {
                foreach (Point3D point3D in geometry3DModel.Positions)
                {
                    mesh.Positions.Add(point3D);
                }
            }

            if (geometry3DModel.TriangleIndices != null)
            {
                foreach (int pointIndice in geometry3DModel.TriangleIndices)
                {
                    mesh.TriangleIndices.Add(pointIndice);
                }
            }

            if (geometry3DModel.TextureCoordinates != null)
            {
                foreach (Point point in geometry3DModel.TextureCoordinates)
                {
                    mesh.TextureCoordinates.Add(point);
                }
            }

            mesh.Freeze();
            return mesh;
        }

        public static Viewport2DVisual3D CreateVisual3D(MeshGeometry3D geometry3D, Material material)
        {
            Viewport2DVisual3D visual3DModel = new Viewport2DVisual3D();
            visual3DModel.Geometry = geometry3D;

            if (material != null)
            {
                visual3DModel.Material = material;
            }

            visual3DModel.Transform = CreateTransform3DGroup();

            return visual3DModel;
        }

        public static Material CreateMaterial3D(Material3DModel materialModel)
        {
            Material material = null;
            switch (materialModel.MaterialType)
            {
                case MaterialType.ColorDiffuse:
                    Color color = (Color)ColorConverter.ConvertFromString(materialModel.MaterialData.ToString());// (Color)materialModel.MaterialData;
                    material = new DiffuseMaterial(new SolidColorBrush(color));
                    break;
                case MaterialType.ImageDiffuse:
                    string url = materialModel.MaterialData as string;
                    material = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(string.Format(url, UriKind.Relative)))));
                    break;
                case MaterialType.MaterialGroup:
                    MaterialGroup materialGroup = new MaterialGroup();
                    List<Material3DModel> material3DModelList = materialModel.MaterialData as List<Material3DModel>;
                    if (material3DModelList != null)
                    {
                        foreach (Material3DModel childMaterial3DModel in material3DModelList)
                        {
                            Material childMaterial = CreateMaterial3D(childMaterial3DModel);
                            materialGroup.Children.Add(childMaterial);
                        }
                    }
                    material = materialGroup;
                    break;
            }

            return material;
        }

        public static ContainerUIElement3D CreateUIElement3D(List<Viewport2DVisual3D> visual3DList)
        {
            ContainerUIElement3D element3D = new ContainerUIElement3D();
            element3D.Transform = CreateTransform3DGroup();

            foreach (Viewport2DVisual3D visual3D in visual3DList)
            {
                element3D.Children.Add(visual3D);
            }

            return element3D;
        }

    }
}
