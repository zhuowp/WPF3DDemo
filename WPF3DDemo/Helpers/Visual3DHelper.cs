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

namespace WPF3DDemo.Helpers
{
    public class Visual3DHelper
    {
        public static ContainerUIElement3D ClosedPathToContainerUIElement3D(List<Point> pathPoint, double topZValue, double bottomZValue)
        {
            ContainerUIElement3D element3D = new ContainerUIElement3D();
            element3D.Transform = InitTransform3DGroupData();

            //绘制周边
            Viewport2DVisual3D surroundingVisual3D = CreateSurroundingSurfaceVisual3D(pathPoint, topZValue, bottomZValue);
            element3D.Children.Add(surroundingVisual3D);

            //绘制区域上表面
            Viewport2DVisual3D topAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, topZValue, 1);
            element3D.Children.Add(topAreaVisual3D);

            //绘制区域下表面
            Viewport2DVisual3D bottomAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, bottomZValue, -1);
            element3D.Children.Add(bottomAreaVisual3D);

            return element3D;
        }

        private static Viewport2DVisual3D CreateSurroundingSurfaceVisual3D(List<Point> pathPoint, double topZValue, double bottomZValue)
        {
            Viewport2DVisual3D visual3DModel = new Viewport2DVisual3D();
            visual3DModel.Geometry = CreateSurroundingGeometry3D(pathPoint, topZValue, bottomZValue);
            visual3DModel.Material = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(0x99, 0x00, 0x00, 0xff)));

            return visual3DModel;
        }

        private static Viewport2DVisual3D CreateAreaSurfaceVisual3D(List<Point> pathPoint, double zValue, int positiveDirection)
        {
            Viewport2DVisual3D visual3DModel = new Viewport2DVisual3D();
            visual3DModel.Geometry = CreatePathAreaGeometry3D(pathPoint, zValue, positiveDirection);

            MaterialGroup materials = new MaterialGroup();
            DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(0x99, 0x00, 0xff, 0xff)));
            materials.Children.Add(diffuseMaterial);
            DiffuseMaterial imageMaterial = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(@"guangdong.png", UriKind.RelativeOrAbsolute))));
            materials.Children.Add(imageMaterial);
            visual3DModel.Material = materials;

            visual3DModel.Transform = InitTransform3DGroupData();

            return visual3DModel;
        }

        private static Geometry3D CreatePathAreaGeometry3D(List<Point> keyPoints, double zValue, int positiveDirection)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            List<Point3D> point3DList = ClosedPathPointsToPoint3Ds(keyPoints, zValue);

            List<Vertex2D> vertexList = new List<Vertex2D>();
            for (int i = 0; i < keyPoints.Count; i++)
            {
                Vertex2D vertex = new Vertex2D()
                {
                    Index = i,
                    X = keyPoints[i].X,
                    Y = keyPoints[i].Y
                };

                vertexList.Add(vertex);
            }

            foreach (Point3D point3D in point3DList)
            {
                mesh.Positions.Add(point3D);
            }

            List<int> trianglePointIndexList = PolygonTriangulationHelper.ResolveToTriangles(vertexList);
            if (trianglePointIndexList != null)
            {
                for (int i = 0; i < trianglePointIndexList.Count; i = i + 3)
                {
                    int[] addIndexArray;

                    if (positiveDirection < 0)
                    {
                        addIndexArray = new int[] { i, i + 1, i + 2 };
                    }
                    else
                    {
                        addIndexArray = new int[] { i + 1, i, i + 2 };
                    }

                    for (int k = 0; k < 3; k++)
                    {
                        mesh.TriangleIndices.Add(trianglePointIndexList[addIndexArray[k]]);
                    }
                }
            }

            mesh.Freeze();
            return mesh;
        }

        private static Geometry3D CreateSurroundingGeometry3D(IList<Point> closedPathPoints, double topZValue, double bottomZValue)
        {
            if (closedPathPoints == null || closedPathPoints.Count < 3)
            {
                throw new Exception("The count of closed path points must large than or equal to 3.");
            }

            List<Point3D> point3DList = ClosedPathPointsToSurroundingSurfacePoint3Ds(closedPathPoints, topZValue, bottomZValue);

            MeshGeometry3D mesh = new MeshGeometry3D();
            foreach (Point3D point3D in point3DList)
            {
                mesh.Positions.Add(point3D);
            }

            for (int i = 0; i < mesh.Positions.Count / 2 - 1; i++)
            {
                int startIndex = i * 2;
                mesh.TriangleIndices.Add(startIndex);
                mesh.TriangleIndices.Add(startIndex + 1);
                mesh.TriangleIndices.Add(startIndex + 3);

                mesh.TriangleIndices.Add(startIndex);
                mesh.TriangleIndices.Add(startIndex + 3);
                mesh.TriangleIndices.Add(startIndex + 2);

                mesh.TextureCoordinates.Add(new Point(startIndex, startIndex + 1));
            }

            //连接首尾
            mesh.TriangleIndices.Add(mesh.Positions.Count - 2);
            mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
            mesh.TriangleIndices.Add(1);

            mesh.TriangleIndices.Add(mesh.Positions.Count - 2);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(0);

            mesh.Freeze();
            return mesh;
        }

        private static List<Point3D> ClosedPathPointsToSurroundingSurfacePoint3Ds(IList<Point> closedPathPoints, double topZValue, double bottomZValue)
        {
            List<Point3D> point3DList = new List<Point3D>();
            foreach (Point point in closedPathPoints)
            {
                Point3D topPoint3D = PointToPoint3D(point, topZValue);
                point3DList.Add(topPoint3D);

                Point3D bottomPoint3D = PointToPoint3D(point, bottomZValue);
                point3DList.Add(bottomPoint3D);
            }

            return point3DList;
        }

        private static List<Point3D> ClosedPathPointsToPoint3Ds(IList<Point> closedPathPoints, double zValue)
        {
            List<Point3D> point3DList = new List<Point3D>();
            foreach (Point point in closedPathPoints)
            {
                Point3D topPoint3D = PointToPoint3D(point, zValue);
                point3DList.Add(topPoint3D);
            }

            return point3DList;
        }

        /// <summary>
        /// 2D点转3D点
        /// </summary>
        /// <param name="point"></param>
        /// <param name="zValue"></param>
        /// <returns></returns>
        private static Point3D PointToPoint3D(Point point, double zValue)
        {
            Point3D point3D = new Point3D(point.X, point.Y, zValue);
            return point3D;
        }

        private static bool Test(Point3D point1, Point3D point2, Point3D point3)
        {
            Vector3D vector1 = new Vector3D(point2.X - point1.X, point2.Y - point1.Y, 0);
            Vector3D vector2 = new Vector3D(point3.X - point2.X, point3.Y - point2.Y, 0);

            Vector3D vector3 = Vector3D.CrossProduct(vector1, vector2);
            if (vector3.Z > 0)
            {
                return true;
            }
            return false;
        }

        public static Transform3DGroup InitTransform3DGroupData()
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
    }
}
