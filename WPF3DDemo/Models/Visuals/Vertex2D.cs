using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models
{
    public class Vertex2D
    {
        public double X { get; set; }
        public double Y { get; set; }

        /// <summary>
        /// 是凸点
        /// </summary>
        public bool IsConvex { get; set; }

        /// <summary>
        /// 可分离
        /// </summary>
        public bool IsSeparable { get; set; }

        /// <summary>
        /// 所在多边形的索引
        /// </summary>
        public int Index { get; set; }

        public Vertex2D()
        { }

        public Vertex2D(Point point) : this(point.X, point.Y, false, false, 0)
        {

        }

        public Vertex2D(double x, double y) : this(x, y, false, false, 0)
        {
        }

        public Vertex2D(double x, double y, bool isConvex, bool isSeparable, int index)
        {
            X = x;
            Y = y;
            IsConvex = isConvex;
            IsSeparable = isSeparable;
            Index = index;
        }

        public static double Cross(Vertex2D v1, Vertex2D v2)
        {
            return v1.X * v2.Y - v1.Y * v2.X;
        }

        public static Vertex2D operator -(Vertex2D v1, Vertex2D v2)
        {
            return new Vertex2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static bool operator ==(Vertex2D v1, Vertex2D v2)
        {
            return v1.X == v2.X && v1.Y == v2.Y;
        }
        public static bool operator !=(Vertex2D v1, Vertex2D v2)
        {
            return v1.X != v2.X || v1.Y != v2.Y;
        }
    }
}
