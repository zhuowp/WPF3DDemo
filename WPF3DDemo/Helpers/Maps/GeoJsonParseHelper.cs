using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPF3DDemo.Models.GeoJsons;

namespace WPF3DDemo.Helpers
{
    public class GeoJsonParseHelper
    {
        public static GeoJson<GeoJsonGeometry> DecodeGeoJson(string geoJson)
        {
            string dealedGeoJson = geoJson.Replace("coordinates\":[[", "coordinates\":[").Replace("\"]],", "\"],").Replace("encodeOffsets\":[[[", "encodeOffsets\":[[").Replace("]]]},\"properties", "]]},\"properties");
            GeoJson<GeoJsonGeometryCoded> geoJsonCodedModel = JsonConvert.DeserializeObject<GeoJson<GeoJsonGeometryCoded>>(dealedGeoJson);

            List<GeoJsonFeature<GeoJsonGeometryCoded>> features = geoJsonCodedModel.Features;
            foreach (GeoJsonFeature<GeoJsonGeometryCoded> feature in features)
            {
                if (feature != null && feature.Properties != null && feature.Properties.CapitalLocation != null && feature.Properties.CapitalLocation.Count == 2)
                {
                    feature.Properties.CapitalLocationPoint = new Point() { X = feature.Properties.CapitalLocation[0], Y = feature.Properties.CapitalLocation[1] };
                }
            }

            GeoJson<GeoJsonGeometry> geoJsonModel = new GeoJson<GeoJsonGeometry>();
            geoJsonModel.Type = geoJsonCodedModel.Type;
            geoJsonModel.Features = new List<GeoJsonFeature<GeoJsonGeometry>>();

            //解压
            foreach (GeoJsonFeature<GeoJsonGeometryCoded> feature in features)
            {
                GeoJsonGeometryCoded geometry = feature.Geometry;
                List<string> coordinates = geometry.Coordinates;
                List<List<double>> encodeOffsets = geometry.EncodeOffsets;

                List<List<Point>> gemeryPointList = new List<List<Point>>();
                for (int i = 0; i < coordinates.Count; i++)
                {
                    List<Point> pointList = DecodePolygon(coordinates[i], encodeOffsets[i]);
                    gemeryPointList.Add(pointList);
                }

                GeoJsonFeature<GeoJsonGeometry> geojsonFeature = new GeoJsonFeature<GeoJsonGeometry>();
                geojsonFeature.Id = feature.Id;
                geojsonFeature.Type = feature.Type;
                geojsonFeature.Properties = feature.Properties;
                geojsonFeature.Geometry = new GeoJsonGeometry();

                geojsonFeature.Geometry.Type = geometry.Type;
                geojsonFeature.Geometry.PointList = gemeryPointList;
                geoJsonModel.Features.Add(geojsonFeature);
            }

            return geoJsonModel;
        }

        public static GeoJson<GeoJsonGeometry> DecodeStandardGeoJson(string geoJson)
        {
            GeoJson<GeoJsonGeometryStandard> geoJsonCodedModel = JsonConvert.DeserializeObject<GeoJson<GeoJsonGeometryStandard>>(geoJson);

            GeoJson<GeoJsonGeometry> geoJsonModel = new GeoJson<GeoJsonGeometry>();
            geoJsonModel.Type = geoJsonCodedModel.Type;
            geoJsonModel.Features = new List<GeoJsonFeature<GeoJsonGeometry>>();

            List<GeoJsonFeature<GeoJsonGeometryStandard>> features = geoJsonCodedModel.Features;
            foreach (GeoJsonFeature<GeoJsonGeometryStandard> feature in features)
            {
                List<List<List<double>>> coordinates = feature.Geometry.Coordinates;

                List<List<Point>> geometryPointsList = new List<List<Point>>();
                for (int k = 0; k < coordinates.Count; k++)
                {
                    List<Point> pointList = new List<Point>();
                    geometryPointsList.Add(pointList);

                    for (int i = 0; i < coordinates[0].Count; i++)
                    {
                        Point point = new Point();
                        point.X = coordinates[k][i][0];
                        point.Y = coordinates[k][i][1];

                        pointList.Add(point);
                    }
                }

                GeoJsonGeometry geoJsonGeometry = new GeoJsonGeometry();
                geoJsonGeometry.PointList = geometryPointsList;

                GeoJsonFeature<GeoJsonGeometry> geoJsonFeature = new GeoJsonFeature<GeoJsonGeometry>();
                geoJsonFeature.Geometry = geoJsonGeometry;

                geoJsonModel.Features.Add(geoJsonFeature);
            }
            return geoJsonModel;
        }

        /// <summary>
        /// 特征点数据解压（解码）
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="encodeOffsets"></param>
        /// <returns></returns>
        public static List<Point> DecodePolygon(string coordinate, List<double> encodeOffsets)
        {
            List<Point> pointList = new List<Point>();

            var prevX = encodeOffsets[0];
            var prevY = encodeOffsets[1];

            char[] coordinageCharArray = coordinate.ToCharArray();
            for (var i = 0; i < coordinageCharArray.Length; i += 2)
            {
                int x = coordinageCharArray[i] - 64;
                int y = coordinageCharArray[i + 1] - 64;

                // ZigZag decoding
                x = (x >> 1) ^ (-(x & 1));
                y = (y >> 1) ^ (-(y & 1));

                prevX += x;
                prevY += y;

                // Dequantize
                pointList.Add(new Point(prevX / 1024, prevY / 1024));
            }

            return pointList;
        }
    }
}
