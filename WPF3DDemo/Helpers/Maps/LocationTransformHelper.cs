using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Helpers
{
    public class LocationTransformHelper
    {
        //public static ArrayList<Point> point = new ArrayList<Point>();
        //public static double minLongitude = ReadFile.longitude.get(0);
        //public static double maxLatitude = ReadFile.latitude.get(0);

        #region Fields

        private readonly Point _originalPoint;
        private readonly double _transformAreaWidth = 0;
        private readonly double _transformAreaHeight = 0;

        private readonly double _minLongitude = 0;
        private readonly double _maxLongitude = 0;

        private readonly double _minLatitude = 0;
        private readonly double _maxLatitude = 0;

        private readonly double _xTransformRatio = 0;
        private readonly double _yTransformRation = 0;

        #endregion

        #region Constructors

        public LocationTransformHelper(double minLongitude, double maxLongitude, double minLatitude, double maxLatitude, Point originalPoint, double transformAreaWidth, double transformAreaHeight)
        {
            _originalPoint = originalPoint;

            _minLongitude = minLongitude;
            _maxLongitude = maxLongitude;

            _minLatitude = minLatitude;
            _maxLatitude = maxLatitude;

            _transformAreaWidth = transformAreaWidth;
            _transformAreaHeight = transformAreaHeight;


        }

        #endregion

        public void makeScreenPoint(String fileName)
        {
            //for (int i = 0; i < ReadFile.longitude.size(); i++)
            //{
            //    minLongitude = Math.min(minLongitude, ReadFile.longitude.get(i));
            //    maxLatitude = Math.max(maxLatitude, ReadFile.latitude.get(i));
            //}

            ///************************************************************/
            ////获得屏幕大小
            //Dimension screensize = Toolkit.getDefaultToolkit().getScreenSize();
            //int screenWidth = (int)screensize.getWidth();
            //int screenHeight = (int)screensize.getHeight();
            ////求距离参照点最远的点
            //double maxDistance = DistanceLongLat(minLongitude, ReadFile.longitude.get(0),
            //        maxLatitude, ReadFile.latitude.get(0));
            //for (int i = 0; i < ReadFile.longitude.size(); i++)
            //{
            //    maxDistance = Math.max(maxDistance, distanceLongLat(minLongitude,
            //        ReadFile.longitude.get(i), maxLatitude, ReadFile.latitude.get(i)));
            //}
            ////转换比率
            //double ratio = 2 * maxDistance / Math.sqrt((screenWidth * screenWidth
            //        + screenHeight * screenHeight));

            ////将经纬度转换成屏幕坐标   
            //for (int i = 0; i < ReadFile.longitude.size(); i++)
            //{
            //    Point p = new Point();

            //    p.x = (int)(distanceLongLat(minLongitude,
            //            ReadFile.longitude.get(i), maxLatitude, maxLatitude) / ratio);
            //    p.y = (int)(distanceLongLat(minLongitude, minLongitude,
            //            maxLatitude, ReadFile.latitude.get(i)) / ratio);

            //    point.add(p);
            //}
        }
        #region Public Methods

        public Point LatLongToScreenPoint(double longitude, double latitude)
        {
            Point point = new Point();
            return point;
        }

        public static double DistanceLongLat(double lng1, double lng2, double lat1, double lat2)
        {
            double dx = lng1 - lng2; // 经度差值
            double dy = lat1 - lat2; // 纬度差值
            double b = (lat1 + lat2) / 2.0; // 平均纬度
            double Lx = dx * Math.PI / 180 * 6367000.0 * Math.Cos(b * Math.PI / 180); // 东西距离
            double Ly = 6367000.0 * dy * Math.PI / 180; // 南北距离
            return Math.Sqrt(Lx * Lx + Ly * Ly);  // 用平面的矩形对角距离公式计算总距离
        }

        #endregion

    }
}
