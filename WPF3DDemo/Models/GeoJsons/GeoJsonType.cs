using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.GeoJsons
{
    public enum GeoJsonType : int
    {
        Unknown = 0,
        Point = 1,
        MultiPoint = 2,
        LineString = 3,
        MultiLineString = 4,
        Polygon = 5,
        MultiPolygon = 6,
        GeometryCollection = 7,
        Feature = 8,
        FeatureCollection = 9,
    }
}
