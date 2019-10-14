using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonGeometry : GeoJsonGeometryBase
    {
        public List<List<Point>> PointList { get; set; }
    }
}
