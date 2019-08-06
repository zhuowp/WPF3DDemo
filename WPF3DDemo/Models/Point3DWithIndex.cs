using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace WPF3DDemo
{
    public class Point3DWithIndex
    {
        public int Index { get; set; } = -1;
        public Point3D Point { get; set; }
        public bool RemoveFlag { get; set; }
    }
}
