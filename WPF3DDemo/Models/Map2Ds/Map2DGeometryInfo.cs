using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.Map2Ds
{
    public class Map2DGeometryInfo
    {
        [JsonProperty("type")]
        public string GeometryType { get; set; }

        private List<List<List<double>>> _geometryPointDataList = null;

        [JsonProperty("coordinates")]
        public List<List<List<double>>> GeometryPointDataList
        {
            set
            {
                if(value == null || value.Count == 0 || value[0] == null || value[0].Count == 0)
                {
                    GeometryPointList = null;
                }
                else
                {
                    GeometryPointList = new List<Point>();
                    foreach (List<double> pointData in value[0])
                    {
                        Point point = new Point() { X = pointData[0], Y = pointData[1] };
                        GeometryPointList.Add(point);
                    }
                }
            }
            get
            {
                if(GeometryPointList == null)
                {
                    return null;
                }
                List<List<List<double>>> geometryPointDataPoint = new List<List<List<double>>>() { new List<List<double>>()};
                
                foreach (Point point in GeometryPointList)
                {
                    geometryPointDataPoint[0].Add(new List<double>() { point.X, point.Y });
                }
                return geometryPointDataPoint;
            }
        }

        [JsonIgnore]
        public List<Point> GeometryPointList { get; set; }
    }
}
