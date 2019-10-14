using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF3DDemo.Models.GeoJsons;
using WPF3DDemo.Models.Map2Ds;

namespace WPF3DDemo.Helpers
{
    public class MapDataConvertHelper
    {
        public static List<double> GetMinMaxLongLatFromGeoJson(GeoJson<GeoJsonGeometry> geoJsonModel)
        {
            double minLatitude = 180;
            double maxLatitude = 0;
            double minLongitude = 180;
            double maxLongitude = 0;

            foreach (GeoJsonFeature<GeoJsonGeometry> feature in geoJsonModel.Features)
            {
                foreach (List<Point> pointList in feature.Geometry.PointList)
                {
                    foreach (Point point in pointList)
                    {
                        if (point.X < minLongitude)
                        {
                            minLongitude = point.X;
                        }
                        if (point.X > maxLongitude)
                        {
                            maxLongitude = point.X;
                        }

                        if (point.Y < minLatitude)
                        {
                            minLatitude = point.Y;
                        }
                        if (point.Y > maxLatitude)
                        {
                            maxLatitude = point.Y;
                        }
                    }
                }
            }

            List<double> minMaxValueList = new List<double>();

            minMaxValueList.Add(minLongitude);
            minMaxValueList.Add(maxLongitude);
            minMaxValueList.Add(minLatitude);
            minMaxValueList.Add(maxLatitude);

            return minMaxValueList;
        }

        public static List<double> GetMinMaxLongLat(List<List<Point>> mapLongLatPointList)
        {
            double minLatitude = double.MaxValue;
            double maxLatitude = double.MinValue;
            double minLongitude = double.MaxValue;
            double maxLongitude = double.MinValue;

            foreach (List<Point> pointList in mapLongLatPointList)
            {
                foreach (Point point in pointList)
                {
                    if (point.X < minLongitude)
                    {
                        minLongitude = point.X;
                    }
                    if (point.X > maxLongitude)
                    {
                        maxLongitude = point.X;
                    }

                    if (point.Y < minLatitude)
                    {
                        minLatitude = point.Y;
                    }
                    if (point.Y > maxLatitude)
                    {
                        maxLatitude = point.Y;
                    }
                }
            }

            List<double> minMaxValueList = new List<double>();

            minMaxValueList.Add(minLongitude);
            minMaxValueList.Add(maxLongitude);
            minMaxValueList.Add(minLatitude);
            minMaxValueList.Add(maxLatitude);

            return minMaxValueList;
        }

        public static Map2DModel GeoJsonFeaureToMap2DModel(GeoJsonFeature<GeoJsonGeometry> feature, List<double> minMaxLatLong)
        {
            double longLenth = minMaxLatLong[1] - minMaxLatLong[0];
            double latLength = minMaxLatLong[3] - minMaxLatLong[2];

            Map2DModel map2D = new Map2DModel(); map2D.Id = feature.Id;
            map2D.Name = feature.Properties == null ? "" : feature.Properties.Name;
            map2D.CapitalLocation = feature.Properties == null ? default(Point) : feature.Properties.CapitalLocationPoint;

            map2D.MinLongitude = minMaxLatLong[0];
            map2D.MaxLongitude = minMaxLatLong[1];
            map2D.MinLatitude = minMaxLatLong[2];
            map2D.MaxLatitude = minMaxLatLong[3];

            List<List<Point>> areaList = new List<List<Point>>();
            foreach (List<Point> area in feature.Geometry.PointList)
            {
                List<Point> pointList = new List<Point>();
                areaList.Add(pointList);

                foreach (Point point in area)
                {
                    Point newPoint = LongLatPointToNormalPoint(minMaxLatLong, point);

                    pointList.Add(newPoint);
                }
            }

            map2D.GeometryPointList = areaList;
            return map2D;
        }

        public static Point LongLatPointToNormalPoint(List<double> minMaxLatLong, Point point)
        {
            Point newPoint = new Point();
            newPoint.X = point.X - minMaxLatLong[0];
            newPoint.Y = minMaxLatLong[3] - point.Y;
            return newPoint;
        }

        public static List<Map2DModel> GeoJsonModelToMap2DList(GeoJson<GeoJsonGeometry> geoJsonModel)
        {
            List<double> minMaxLongLatList = GetMinMaxLongLatFromGeoJson(geoJsonModel);

            List<Map2DModel> map2DList = new List<Map2DModel>();
            foreach (GeoJsonFeature<GeoJsonGeometry> feature in geoJsonModel.Features)
            {
                Map2DModel map2D = GeoJsonFeaureToMap2DModel(feature, minMaxLongLatList);


                map2DList.Add(map2D);
            }

            return map2DList;
        }

        public static Map2DModel LngLatPointListToMap2DModel(List<List<Point>> lngLatPointList)
        {
            List<double> minMaxLatLong = GetMinMaxLongLat(lngLatPointList);

            double longLenth = minMaxLatLong[1] - minMaxLatLong[0];
            double latLength = minMaxLatLong[3] - minMaxLatLong[2];

            Map2DModel map2D = new Map2DModel();
            map2D.Id = "1";
            map2D.MinLongitude = minMaxLatLong[0];
            map2D.MaxLongitude = minMaxLatLong[1];
            map2D.MinLatitude = minMaxLatLong[2];
            map2D.MaxLatitude = minMaxLatLong[3];

            List<List<Point>> areaList = new List<List<Point>>();
            foreach (List<Point> area in lngLatPointList)
            {
                List<Point> pointList = new List<Point>();
                areaList.Add(pointList);

                foreach (Point point in area)
                {
                    Point newPoint = LongLatPointToNormalPoint(minMaxLatLong, point);

                    pointList.Add(newPoint);
                }
            }

            map2D.GeometryPointList = areaList;
            return map2D;
        }
    }
}
