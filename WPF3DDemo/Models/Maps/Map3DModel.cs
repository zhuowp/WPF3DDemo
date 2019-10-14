using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;
using WPF3DDemo.Models.Visuals;

namespace WPF3DDemo.Models.Maps
{
    public class Map3DModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public double MinLongitude { get; set; }
        public double MaxLongitude { get; set; }
        public double MinLatitude { get; set; }
        public double MaxLatitude { get; set; }
        public double ZoomFactor { get; set; }
        public List<CylinderVisual3DModel> Geometries { get; set; }
        public Point3D CapitalLocation { get; set; }
        public List<List<Material3DModel>> Materials { get; set; }
    }
}
