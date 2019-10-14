using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Helpers.Visual3Ds
{
    public class FeaturePointHelper
    {
        public static Rect GetFeaturePointsBoundaryRect(IEnumerable<Point> featurePoints)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;

            foreach(Point point in featurePoints)
            {
                minX = minX > point.X ? point.X : minX;
                minY = minY > point.Y ? point.Y : minY;
                maxX = maxX < point.X ? point.X : maxX;
                maxY = maxY < point.Y ? point.Y : maxY;
            }

            double width = maxX - minX;
            double height = maxY - minY;

            Rect rect = new Rect() { X = minX, Y = minY, Width = width, Height = height };
            return rect;
        }
    }
}
