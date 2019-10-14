using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Media3D;

namespace WPF3DDemo.Models
{
    public class Geometry3DModel
    {
        #region Fields

        private string _id = string.Empty;
        private List<Point3D> _positions = new List<Point3D>();
        private List<int> _triangleIndices = new List<int>();
        private List<Point> _textureCoordinates = new List<Point>();

        #endregion

        #region Properites

        #endregion

        public string Id
        {
            get
            {
                return _id;
            }

            set
            {
                _id = value;
            }
        }

        public List<Point3D> Positions
        {
            get
            {
                return _positions;
            }

            set
            {
                _positions = value;
            }
        }

        public List<int> TriangleIndices
        {
            get
            {
                return _triangleIndices;
            }

            set
            {
                _triangleIndices = value;
            }
        }

        public List<Point> TextureCoordinates
        {
            get
            {
                return _textureCoordinates;
            }

            set
            {
                _textureCoordinates = value;
            }
        }
    }
}
