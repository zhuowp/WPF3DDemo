using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WPF3DDemo.Helpers.Visual3Ds
{
    public class GraphicHelper
    {
        public static List<double> GetRectBound(List<List<List<Point>>> pointListss)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach (var pointLists in pointListss)
            {
                foreach (var pointList in pointLists)
                {
                    for (int i = 0; i < pointList.Count; i++)
                    {
                        Point point = pointList[i];
                        if (minX > point.X)
                        {
                            minX = point.X;
                        }
                        if (maxX < point.X)
                        {
                            maxX = point.X;
                        }
                        if (minY > point.Y)
                        {
                            minY = point.Y;
                        }
                        if (maxY < point.Y)
                        {
                            maxY = point.Y;
                        }
                    }
                }
            }
            return new List<double>() { minX, minY, maxX, maxY };
        }
    }
}
