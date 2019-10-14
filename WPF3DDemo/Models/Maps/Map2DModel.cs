using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.Map2Ds
{
    public class Map2DModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Point CapitalLocation { get; set; }

        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }

        public List<List<Point>> GeometryPointList { get; set; }
    }
}
