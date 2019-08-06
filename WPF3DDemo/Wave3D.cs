using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WPF3DDemo.Helpers;

namespace WPF3DDemo
{
    public class Wave3D
    {
        #region Consts

        private const int MIN_DIMENSION = 5;
        private const double RENDER_INTERVAL = 60;

        #endregion

        #region Fields

        private double _lastTimeRendered = 0;


        private readonly double _damping = 0.9;
        private readonly Point _beginPoint;
        private readonly Point _endPoint;
        private readonly int _dimension;
        private readonly double _zValue = 0;
        private readonly MeshGeometry3D _meshGeometry3D;
        private readonly GeometryModel3D _geometryModel3D;
        private readonly TextureMapHelper _textureMapping;

        private Int32Collection _triangleIndices;
        private Point3DCollection _point3DBuffer1;
        private Point3DCollection _point3DBuffer2;
        private Point3DCollection _originalPoint3DBuffer;

        private Point3DCollection _currentPoint3DBuffer;
        private Point3DCollection _previousPoint3DBuffer;

        private bool _hasWaves = false;
        private object _updateHasWavesLock = new object();

        #endregion

        #region Properties

        /// <summary>
        /// Access to underlying grid data
        /// </summary>
        public Point3DCollection Points
        {
            get { return _currentPoint3DBuffer; }
        }

        /// <summary>
        /// Access to underlying triangle index collection
        /// </summary>
        public Int32Collection TriangleIndices
        {
            get { return _triangleIndices; }
        }

        /// <summary>
        /// Dimension of grid--same dimension for both X & Y
        /// </summary>
        public int Dimension
        {
            get { return _dimension; }
        }

        #endregion

        #region Constructors

        public Wave3D(Point beginPoint, Point endPoint, int dimension, double zValue, GeometryModel3D geometryModel3D)
        {
            if (dimension < MIN_DIMENSION)
            {
                throw new ApplicationException(string.Format("Dimension must be at least {0}", MIN_DIMENSION.ToString()));
            }

            _dimension = dimension;
            _zValue = zValue;
            _beginPoint = beginPoint;
            _endPoint = endPoint;

            _textureMapping = new TextureMapHelper();
            _geometryModel3D = geometryModel3D;
            _meshGeometry3D = geometryModel3D.Geometry as MeshGeometry3D;

            (_geometryModel3D.Material as MaterialGroup).Children.Add(_textureMapping.m_material);
            InitializeWaveMedium();

            _meshGeometry3D.Positions = Points;
            _meshGeometry3D.TriangleIndices = TriangleIndices;

            _currentPoint3DBuffer = _point3DBuffer2;
            _previousPoint3DBuffer = _point3DBuffer1;

            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        #endregion

        #region Private Methods

        private void InitializeWaveMedium()
        {
            _point3DBuffer1 = new Point3DCollection(_dimension * _dimension);
            _triangleIndices = new Int32Collection((_dimension - 1) * (_dimension - 1) * 2);

            double columnUnit = (_endPoint.X - _beginPoint.X) / _dimension;
            double rowUnit = (_endPoint.Y - _beginPoint.Y) / _dimension;

            int currentIndex = 0;
            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    _point3DBuffer1.Add(new Point3D(col * columnUnit, row * rowUnit, _zValue));

                    if ((row > 0) && (col > 0))
                    {
                        // Triangle 1
                        _triangleIndices.Add(currentIndex - _dimension - 1);
                        _triangleIndices.Add(currentIndex);
                        _triangleIndices.Add(currentIndex - _dimension);

                        // Triangle 2
                        _triangleIndices.Add(currentIndex - _dimension - 1);
                        _triangleIndices.Add(currentIndex - 1);
                        _triangleIndices.Add(currentIndex);
                    }

                    currentIndex++;
                }
            }

            _point3DBuffer2 = _point3DBuffer1.Clone();
            _originalPoint3DBuffer = _point3DBuffer1.Clone();
        }

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            RenderingEventArgs rargs = (RenderingEventArgs)e;
            if ((rargs.RenderingTime.TotalMilliseconds - _lastTimeRendered) > RENDER_INTERVAL && _hasWaves)
            {
                bool needRefresh = RefreshWaveData();
                if (needRefresh)
                {
                    _meshGeometry3D.Positions = Points;

                    //着色
                    _meshGeometry3D.TextureCoordinates.Clear();
                    foreach (Point3D p3d in _meshGeometry3D.Positions)
                    {
                        double dev = Math.Abs(p3d.Z + 50) / 5;
                        Color color = Color.FromArgb((byte)(dev * 255), (byte)(dev * 255), 0, 0);
                        Point mapPt = _textureMapping.GetMappingPosition(color);
                        _meshGeometry3D.TextureCoordinates.Add(new Point(mapPt.X, mapPt.Y));
                    }

                    _lastTimeRendered = rargs.RenderingTime.TotalMilliseconds;
                }
                else
                {
                    _meshGeometry3D.Positions = _originalPoint3DBuffer.Clone();
                    _currentPoint3DBuffer = _originalPoint3DBuffer.Clone();
                    _previousPoint3DBuffer = _originalPoint3DBuffer.Clone();

                    _lastTimeRendered = rargs.RenderingTime.TotalMilliseconds;

                    lock (_updateHasWavesLock)
                    {
                        _hasWaves = false;
                    }
                }
            }
        }

        private void SwapPoint3DBuffers()
        {
            Point3DCollection temp = _currentPoint3DBuffer;
            _currentPoint3DBuffer = _previousPoint3DBuffer;
            _previousPoint3DBuffer = temp;
        }

        public void CreateCircularWaveSource(int row, int col, double peakValue, int peakWidth)
        {
            if ((row + (peakWidth - 1)) > (_dimension - 1))
            {
                row = _dimension - peakWidth;
            }

            if ((col + (peakWidth - 1)) > (_dimension - 1))
            {
                col = _dimension - peakWidth;
            }

            for (int ir = row; ir < (row + peakWidth); ir++)
            {
                for (int ic = col; ic < (col + peakWidth); ic++)
                {
                    int point3DIndex = (ir * _dimension) + ic;

                    Point3D point3D = _previousPoint3DBuffer[point3DIndex];
                    point3D.Z += peakValue;

                    _previousPoint3DBuffer[point3DIndex] = point3D;
                }
            }

            lock (_updateHasWavesLock)
            {
                _hasWaves = true;
            }
        }

        /// <summary>
        /// Set height of all points in mesh to 0.0.  Also resets buffers to 
        /// original state.
        /// </summary>
        public void InitWaveMedium()
        {
            Point3D pt;

            for (int i = 0; i < (_dimension * _dimension); i++)
            {
                pt = _point3DBuffer1[i];
                pt.Z = _zValue;
                _point3DBuffer1[i] = pt;
            }

            _point3DBuffer2 = _point3DBuffer1.Clone();
            _currentPoint3DBuffer = _point3DBuffer2;
            _previousPoint3DBuffer = _point3DBuffer1;
        }

        /// <summary>
        /// Determine next state of entire grid, based on previous two states.
        /// This will have the effect of propagating ripples outward.
        /// </summary>
        private bool RefreshWaveData()
        {
            if (_currentPoint3DBuffer.Count == 0)
            {
                return false;
            }

            double smoothed;    // Smoothed by adjacent cells
            double newHeight;
            int neighbors;

            int point3DIndex = 0;
            int refreshPointCount = 0;

            //Z value is the height (the value that we're animating)
            for (int row = 0; row < _dimension; row++)
            {
                for (int col = 0; col < _dimension; col++)
                {
                    smoothed = 0.0;

                    neighbors = 0;
                    if (row > 0)    // row-1, col
                    {
                        smoothed += _currentPoint3DBuffer[point3DIndex - _dimension].Z;
                        neighbors++;
                    }

                    if (row < (_dimension - 1))   // row+1, col
                    {
                        smoothed += _currentPoint3DBuffer[point3DIndex + _dimension].Z;
                        neighbors++;
                    }

                    if (col > 0)          // row, col-1
                    {
                        smoothed += _currentPoint3DBuffer[point3DIndex - 1].Z;
                        neighbors++;
                    }

                    if (col < (_dimension - 1))   // row, col+1
                    {
                        smoothed += _currentPoint3DBuffer[point3DIndex + 1].Z;
                        neighbors++;
                    }

                    smoothed /= neighbors;
                    double changedHeight = smoothed - _previousPoint3DBuffer[point3DIndex].Z;

                    // 阻尼
                    newHeight = changedHeight * _damping + _zValue;
                    if (newHeight - _currentPoint3DBuffer[point3DIndex].Z > 0.001)
                    {
                        refreshPointCount++;
                    }

                    Point3D newPoint3D = _previousPoint3DBuffer[point3DIndex];
                    newPoint3D.Z = newHeight;
                    _previousPoint3DBuffer[point3DIndex] = newPoint3D;

                    point3DIndex++;
                }
            }

            SwapPoint3DBuffers();

            if (refreshPointCount == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion

    }
}
