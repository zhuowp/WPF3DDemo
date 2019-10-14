using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF3DDemo.Helpers;
using WPF3DDemo.Models.GeoJsons;
using WPF3DDemo.Models.Map2Ds;

namespace WPF3DDemo.Views
{
    /// <summary>
    /// MapFeatureEditView.xaml 的交互逻辑
    /// </summary>
    public partial class MapFeatureEditView : UserControl
    {
        List<Map2DModel> map2DModelList;

        string provinceName = "广东";
        string cityName = "南沙区";

        public MapFeatureEditView()
        {
            InitializeComponent();

            //string geoJsonString = System.IO.File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\GeoJson\{0}\{1}.json", provinceName, cityName));

            //GeoJson<GeoJsonGeometry> geoJsonModel = GeoJsonParseHelper.DecodeGeoJson(geoJsonString);
            //List<Map2DModel> map2DModelList = MapDataConvertHelper.GeoJsonModelToMap2DList(geoJsonModel);
            //string map2DJson = JsonConvert.SerializeObject(map2DModelList);
            //System.IO.File.WriteAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", cityName), map2DJson);

            string map2DJson = System.IO.File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", cityName));
            map2DModelList = JsonConvert.DeserializeObject<List<Map2DModel>>(map2DJson);
            double zoomFactor = 2650;
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            foreach (Map2DModel map2DModel in map2DModelList)
            {
                //if (map2DModel.Name != "南海区")
                //{
                //    continue;
                //}
                List<Polygon> polygons = new List<Polygon>();
                foreach (List<Point> mapAreaPoints in map2DModel.GeometryPointList)
                {
                    //Path path = CreatePath(mapAreaPoints, new SolidColorBrush(Colors.Red), null);
                    //canvas.Children.Add(path);

                    var polygon = new Polygon { StrokeThickness = 1, Fill = null, Stroke = new SolidColorBrush(Colors.Red) };
                    foreach (Point point in mapAreaPoints)
                    {
                        if (point.X < minX)
                        {
                            minX = point.X;
                        }

                        if (point.X > maxX)
                        {
                            maxX = point.X;
                        }

                        if (point.Y < minY)
                        {
                            minY = point.Y;
                        }

                        if (point.Y > maxY)
                        {
                            maxY = point.Y;
                        }
                        polygon.Points.Add(new Point(point.X * zoomFactor, point.Y * zoomFactor));
                    }
                    canvas.Children.Add(polygon);
                    polygons.Add(polygon);

                    //foreach (Point point in mapAreaPoints)
                    //{
                    //    Ellipse ellipse = new Ellipse();
                    //    ellipse.StrokeThickness = 1;
                    //    ellipse.Width = 4;
                    //    ellipse.Height = 4;
                    //    ellipse.Fill = new SolidColorBrush(Colors.Blue);
                    //    ellipse.Stroke = new SolidColorBrush(Colors.Blue);
                    //    ellipse.MouseLeftButtonDown += (s, e) =>
                    //    {
                    //        Ellipse clickedEllipsed = s as Ellipse;
                    //        //if (e.ClickCount == 2)
                    //        {
                    //            canvas.Children.Remove(clickedEllipsed);
                    //            bool result = polygon.Points.Remove(new Point(point.X * zoomFactor, point.Y * zoomFactor));
                    //            mapAreaPoints.Remove(point);
                    //        }
                    //    };
                    //    Canvas.SetLeft(ellipse, point.X * zoomFactor - ellipse.Width / 2);
                    //    Canvas.SetTop(ellipse, point.Y * zoomFactor - ellipse.Height / 2);
                    //    canvas.Children.Add(ellipse);
                    //}
                }

            }

            var polygonBorder = new Polygon { StrokeThickness = 1, Fill = null, Stroke = new SolidColorBrush(Colors.Blue) };
            polygonBorder.Points.Add(new Point(minX * zoomFactor, minY * zoomFactor));
            polygonBorder.Points.Add(new Point(maxX * zoomFactor, minY * zoomFactor));
            polygonBorder.Points.Add(new Point(maxX * zoomFactor, maxY * zoomFactor));
            polygonBorder.Points.Add(new Point(minX * zoomFactor, maxY * zoomFactor));
            canvas.Children.Add(polygonBorder);
        }

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private Path CreatePath(List<Point> points, Brush outlineBrush, Brush fillBrush)
        {
            Path path = new Path
            {
                Stroke = outlineBrush,
                StrokeThickness = 1,
                Fill = fillBrush,
                Stretch = Stretch.Uniform,
                SnapsToDevicePixels = true,
            };

            GeometryGroup geometryGroup = new GeometryGroup();
            path.Data = geometryGroup;

            //指定Path的形状
            PathGeometry geometry = new PathGeometry();
            geometryGroup.Children.Add(geometry);

            PathFigure pathFigure = new PathFigure
            {
                StartPoint = new Point(points[0].X * 6000, points[0].Y * 6000),
                IsClosed = true,
                IsFilled = true,
            };
            geometry.Figures.Add(pathFigure);

            for (int i = 1; i < points.Count; i++)
            {
                LineSegment lineSegment = new LineSegment() { IsSmoothJoin = true, Point = new Point(points[i].X * 6000, points[i].Y * 6000) };
                pathFigure.Segments.Add(lineSegment);
            }

            return path;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            string map2DJson = JsonConvert.SerializeObject(map2DModelList);
            System.IO.File.WriteAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", cityName), map2DJson);
        }

        void CaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource imageSource = CreateElementScreenshot(canvas);
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageSource));
            using (System.IO.FileStream stream = new System.IO.FileStream(string.Format("{0}.png", cityName), System.IO.FileMode.Create))
                encoder.Save(stream);
        }

        private BitmapSource CreateElementScreenshot(Visual visual)
        {
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)RenderSize.Width, (int)RenderSize.Height, 96, 96, PixelFormats.Default);
            bmp.Render(visual);
            return bmp;
        }

        private BitmapSource CreateNotRanderElementScreenshot(UIElement element)
        {
            var wantRanderSize = new Size(300, 300);
            element.Measure(wantRanderSize);
            element.Arrange(new Rect(new Point(0, 0), wantRanderSize));
            return CreateElementScreenshot(element);
        }
    }
}
