using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using WPF3DDemo.Models;
using WPF3DDemo.Models.Visuals;

namespace WPF3DDemo.Helpers.Visual3Ds
{
    public class Visual2DTo3DHelper
    {
        //public static ContainerUIElement3D ClosedPathToContainerUIElement3D(List<Point> pathPoint, double topZValue, double bottomZValue)
        //{
        //    ContainerUIElement3D element3D = new ContainerUIElement3D();
        //    element3D.Transform = Visual3DCreateHelper.CreateTransform3DGroup();

        //    //绘制周边
        //    Viewport2DVisual3D surroundingVisual3D = CreateSurroundingSurfaceVisual3D(pathPoint, topZValue, bottomZValue);
        //    element3D.Children.Add(surroundingVisual3D);

        //    //绘制区域上表面
        //    Viewport2DVisual3D topAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, topZValue, 1, "");
        //    element3D.Children.Add(topAreaVisual3D);

        //    //绘制区域下表面
        //    Viewport2DVisual3D bottomAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, bottomZValue, -1, "");
        //    element3D.Children.Add(bottomAreaVisual3D);

        //    return element3D;
        //}

        //public static ContainerUIElement3D MultiClosedAreaToContainerUIElement3D(List<List<Point>> pathPoints, double topZValue, double bottomZValue, string texture)
        //{
        //    ContainerUIElement3D element3D = new ContainerUIElement3D();
        //    element3D.Transform = Visual3DCreateHelper.CreateTransform3DGroup();

        //    foreach (List<Point> pathPoint in pathPoints)
        //    {
        //        //绘制周边
        //        Viewport2DVisual3D surroundingVisual3D = CreateSurroundingSurfaceVisual3D(pathPoint, topZValue, bottomZValue);
        //        element3D.Children.Add(surroundingVisual3D);

        //        //绘制区域上表面
        //        Viewport2DVisual3D topAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, topZValue, 1, texture);
        //        element3D.Children.Add(topAreaVisual3D);

        //        //绘制区域下表面
        //        Viewport2DVisual3D bottomAreaVisual3D = CreateAreaSurfaceVisual3D(pathPoint, bottomZValue, -1, texture);
        //        element3D.Children.Add(bottomAreaVisual3D);
        //    }

        //    return element3D;
        //}

        //private static Viewport2DVisual3D CreateSurroundingSurfaceVisual3D(List<Point> pathPoint, double topZValue, double bottomZValue)
        //{
        //    Viewport2DVisual3D visual3DModel = new Viewport2DVisual3D();
        //    visual3DModel.Geometry = CreateSideSurfaceGeometry3D(pathPoint, topZValue, bottomZValue);
        //    visual3DModel.Material = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(0xaa, 0x00, 0x00, 0xff)));

        //    return visual3DModel;
        //}

        //private static Viewport2DVisual3D CreateAreaSurfaceVisual3D(List<Point> pathPoint, double zValue, int positiveDirection, string texture)
        //{
        //    Viewport2DVisual3D visual3DModel = new Viewport2DVisual3D();
        //    visual3DModel.Geometry = CreatePathAreaGeometry3D(pathPoint, zValue, positiveDirection);

        //    MaterialGroup materials = new MaterialGroup();
        //    DiffuseMaterial diffuseMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(0x99, 0x00, 0xff, 0xff)));
        //    materials.Children.Add(diffuseMaterial);
        //    if (!string.IsNullOrEmpty(texture))
        //    {
        //        DiffuseMaterial imageMaterial = new DiffuseMaterial(new ImageBrush(new BitmapImage(new Uri(string.Format(@"Images\MapTextures\{0}.png", texture), UriKind.Relative))));
        //        materials.Children.Add(imageMaterial);
        //    }
        //    visual3DModel.Material = materials;

        //    visual3DModel.Transform = Visual3DCreateHelper.CreateTransform3DGroup();

        //    return visual3DModel;
        //}

        //private static Geometry3D CreatePathAreaGeometry3D(List<Point> keyPoints, double zValue, int positiveDirection)
        //{
        //    MeshGeometry3D mesh = new MeshGeometry3D();
        //    List<Point3D> point3DList = ClosedPathPointsToPoint3Ds(keyPoints, zValue);

        //    double minX = 0;
        //    double minY = 0;
        //    double maxX = 0;
        //    double maxY = 0;

        //    for (int i = 0; i < keyPoints.Count; i++)
        //    {
        //        minX = minX > keyPoints[i].X ? keyPoints[i].X : minX;
        //        minY = minY > keyPoints[i].Y ? keyPoints[i].Y : minY;
        //        maxX = maxX < keyPoints[i].X ? keyPoints[i].X : maxX;
        //        maxY = maxY < keyPoints[i].Y ? keyPoints[i].Y : maxY;
        //    }

        //    double xLength = maxX - minX;
        //    double yLength = maxY - minY;

        //    List<Vertex2D> vertexList = new List<Vertex2D>();
        //    for (int i = 0; i < keyPoints.Count; i++)
        //    {
        //        Vertex2D vertex = new Vertex2D()
        //        {
        //            Index = i,
        //            X = keyPoints[i].X,
        //            Y = keyPoints[i].Y
        //        };

        //        vertexList.Add(vertex);
        //    }

        //    foreach (Point3D point3D in point3DList)
        //    {
        //        mesh.Positions.Add(point3D);
        //        mesh.TextureCoordinates.Add(new Point((point3D.X - minX) / xLength, (point3D.Y - minY) / yLength));
        //    }

        //    List<int> trianglePointIndexList = PolygonTriangulationHelper.ResolveToTriangles(vertexList);
        //    if (trianglePointIndexList != null)
        //    {
        //        for (int i = 0; i < trianglePointIndexList.Count; i = i + 3)
        //        {
        //            int[] addIndexArray;

        //            if (positiveDirection < 0)
        //            {
        //                addIndexArray = new int[] { i, i + 1, i + 2 };
        //            }
        //            else
        //            {
        //                addIndexArray = new int[] { i + 1, i, i + 2 };
        //            }

        //            for (int k = 0; k < 3; k++)
        //            {
        //                mesh.TriangleIndices.Add(trianglePointIndexList[addIndexArray[k]]);
        //            }
        //        }
        //    }

        //    mesh.Freeze();
        //    return mesh;
        //}

        //private static MeshGeometry3D CreateSideSurfaceGeometry3D(IList<Point> closedPathPoints, double topZValue, double bottomZValue)
        //{
        //    if (closedPathPoints == null || closedPathPoints.Count < 3)
        //    {
        //        throw new Exception("The count of closed path points must large than or equal to 3.");
        //    }

        //    List<Point3D> point3DList = ClosedAreaFeaturePointsToSideSurfacePoint3Ds(closedPathPoints, topZValue, bottomZValue);

        //    MeshGeometry3D mesh = new MeshGeometry3D();
        //    foreach (Point3D point3D in point3DList)
        //    {
        //        mesh.Positions.Add(point3D);
        //    }

        //    for (int i = 0; i < mesh.Positions.Count / 2 - 1; i++)
        //    {
        //        int startIndex = i * 2;
        //        mesh.TriangleIndices.Add(startIndex);
        //        mesh.TriangleIndices.Add(startIndex + 1);
        //        mesh.TriangleIndices.Add(startIndex + 3);

        //        mesh.TriangleIndices.Add(startIndex);
        //        mesh.TriangleIndices.Add(startIndex + 3);
        //        mesh.TriangleIndices.Add(startIndex + 2);

        //        mesh.TextureCoordinates.Add(new Point(startIndex, startIndex + 1));
        //    }

        //    //连接首尾
        //    mesh.TriangleIndices.Add(mesh.Positions.Count - 2);
        //    mesh.TriangleIndices.Add(mesh.Positions.Count - 1);
        //    mesh.TriangleIndices.Add(1);

        //    mesh.TriangleIndices.Add(mesh.Positions.Count - 2);
        //    mesh.TriangleIndices.Add(1);
        //    mesh.TriangleIndices.Add(0);

        //    mesh.Freeze();
        //    return mesh;
        //}

        public static CylinderVisual3DModel Closed2DAreaToCylinderVisual3DModel(List<Point> featurePoints, double upperZValue, double lowerZValue, Rect rect)
        {
            CylinderVisual3DModel cylinderVisual3D = new CylinderVisual3DModel();

            List<Vertex2D> vertexList = new List<Vertex2D>();
            for (int i = 0; i < featurePoints.Count; i++)
            {
                Vertex2D vertex = new Vertex2D(featurePoints[i]) { Index = i };
                vertexList.Add(vertex);
            }
            bool isCw = PolygonTriangulationHelper.IsClockwise(vertexList);
            if (!isCw)
            {
                featurePoints.Reverse();
            }
            List<Vec> vecList = new List<Vec>();
            for (int i = 0; i < featurePoints.Count; i++)
            {
                Vec vec = new Vec { x = (float)featurePoints[i].X, y = (float)featurePoints[i].Y };
                vecList.Add(vec);
            }

            List<int> trianglePointIndexList = PolygonHelper.Resolve(vecList);
            if (trianglePointIndexList == null)
            {
                return null;
            }

            List<int> lowerUnderSurfacePointIndexList = new List<int>();
            for (int i = 0; i < trianglePointIndexList.Count; i = i + 3)
            {
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i]);
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i + 2]);
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i + 1]);
            }

            Geometry3DModel upperGeometry3DModel = FeaturePointsToGeometry3DModel(featurePoints, upperZValue, rect, lowerUnderSurfacePointIndexList);
            cylinderVisual3D.UpperUndersurface = upperGeometry3DModel;

            Geometry3DModel lowerGeometry3DModel = FeaturePointsToGeometry3DModel(featurePoints, lowerZValue, rect, trianglePointIndexList);
            cylinderVisual3D.LowerUndersurface = lowerGeometry3DModel;

            cylinderVisual3D.SideSurface = FeaturePointsToSideSurefaceGeometry3DModel(featurePoints, upperZValue, lowerZValue);
            return cylinderVisual3D;
        }

        public static CylinderVisual3DModel Closed2DAreaToCylinderVisual3DModel(List<Point> featurePoints, double upperZValue, double lowerZValue)
        {
            CylinderVisual3DModel cylinderVisual3D = new CylinderVisual3DModel();
            Rect rect = FeaturePointHelper.GetFeaturePointsBoundaryRect(featurePoints);
            List<Vertex2D> vertexList = new List<Vertex2D>();
            for (int i = 0; i < featurePoints.Count; i++)
            {
                Vertex2D vertex = new Vertex2D(featurePoints[i]) { Index = i };
                vertexList.Add(vertex);
            }
            bool isCw = PolygonTriangulationHelper.IsClockwise(vertexList);
            if (!isCw)
            {
                featurePoints.Reverse();
            }
            //List<int> trianglePointIndexList = PolygonTriangulationHelper.ResolveToTriangles(vertexList);
            List<Vec> vecList = new List<Vec>();
            for (int i = 0; i < featurePoints.Count; i++)
            {
                Vec vec = new Vec { x = (float)featurePoints[i].X, y = (float)featurePoints[i].Y };
                vecList.Add(vec);
            }

            List<int> trianglePointIndexList = PolygonHelper.Resolve(vecList);
            if (trianglePointIndexList == null)
            {
                return null;
            }

            List<int> lowerUnderSurfacePointIndexList = new List<int>();
            for (int i = 0; i < trianglePointIndexList.Count; i = i + 3)
            {
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i]);
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i + 2]);
                lowerUnderSurfacePointIndexList.Add(trianglePointIndexList[i + 1]);
            }

            Geometry3DModel upperGeometry3DModel = FeaturePointsToGeometry3DModel(featurePoints, upperZValue, rect, lowerUnderSurfacePointIndexList);
            cylinderVisual3D.UpperUndersurface = upperGeometry3DModel;

            Geometry3DModel lowerGeometry3DModel = FeaturePointsToGeometry3DModel(featurePoints, lowerZValue, rect, trianglePointIndexList);
            cylinderVisual3D.LowerUndersurface = lowerGeometry3DModel;

            cylinderVisual3D.SideSurface = FeaturePointsToSideSurefaceGeometry3DModel(featurePoints, upperZValue, lowerZValue);
            return cylinderVisual3D;
        }

        private static Geometry3DModel FeaturePointsToSideSurefaceGeometry3DModel(IList<Point> featurePoints, double upperZValue, double lowerZValue)
        {
            if (featurePoints == null || featurePoints.Count < 3)
            {
                throw new Exception("The count of closed path points must large than or equal to 3.");
            }

            Geometry3DModel geometry3DModel = new Geometry3DModel();
            List<Point3D> point3DList = ClosedAreaFeaturePointsToSideSurfacePoint3Ds(featurePoints, upperZValue, lowerZValue);

            for (int i = 0; i < point3DList.Count; i++)
            {
                geometry3DModel.Positions.Add(point3DList[i]);
                geometry3DModel.TextureCoordinates.Add(new Point(i / 2.0 / point3DList.Count, i % 2.0 / point3DList.Count));
            }

            for (int i = 0; i < geometry3DModel.Positions.Count / 2 - 1; i++)
            {
                int startIndex = i * 2;
                geometry3DModel.TriangleIndices.Add(startIndex);
                geometry3DModel.TriangleIndices.Add(startIndex + 1);
                geometry3DModel.TriangleIndices.Add(startIndex + 3);

                geometry3DModel.TriangleIndices.Add(startIndex);
                geometry3DModel.TriangleIndices.Add(startIndex + 3);
                geometry3DModel.TriangleIndices.Add(startIndex + 2);
            }

            //连接首尾
            geometry3DModel.TriangleIndices.Add(geometry3DModel.Positions.Count - 2);
            geometry3DModel.TriangleIndices.Add(geometry3DModel.Positions.Count - 1);
            geometry3DModel.TriangleIndices.Add(1);

            geometry3DModel.TriangleIndices.Add(geometry3DModel.Positions.Count - 2);
            geometry3DModel.TriangleIndices.Add(1);
            geometry3DModel.TriangleIndices.Add(0);

            return geometry3DModel;
        }

        private static Geometry3DModel FeaturePointsToGeometry3DModel(IList<Point> featurePoints, double zValue, Rect rect, List<int> trianglePointIndexList)
        {
            Geometry3DModel geometry3DModel = new Geometry3DModel();
            geometry3DModel.TriangleIndices.AddRange(trianglePointIndexList);

            List<Point3D> point3DList = ClosedPathPointsToPoint3Ds(featurePoints, zValue);
            foreach (Point3D point3D in point3DList)
            {
                geometry3DModel.Positions.Add(point3D);

                double textureCoordinateX = (point3D.X - rect.X) / rect.Width;
                double textureCoordinateY = (point3D.Y - rect.Y) / rect.Height;

                Point point = new Point(textureCoordinateX, textureCoordinateY);
                geometry3DModel.TextureCoordinates.Add(point);
            }

            return geometry3DModel;
        }

        private static List<Point3D> ClosedAreaFeaturePointsToSideSurfacePoint3Ds(IList<Point> closedPathPoints, double topZValue, double bottomZValue)
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

    }
}
