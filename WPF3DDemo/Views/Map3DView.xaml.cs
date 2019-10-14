using HelixToolkit.Wpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF3DDemo.Helpers;
using WPF3DDemo.Helpers.Visual3Ds;
using WPF3DDemo.Models;
using WPF3DDemo.Models.GeoJsons;
using WPF3DDemo.Models.Map2Ds;
using WPF3DDemo.Models.Maps;

namespace WPF3DDemo.Views
{
    /// <summary>
    /// Map3DView.xaml 的交互逻辑
    /// </summary>
    public partial class Map3DView : UserControl
    {
        #region Fields

        private Point _mousePosition;
        private bool _isMouseLeftButtonDown = false;
        private double _verticalRotateAngle = 0;
        private Point3D _center = new Point3D();

        #endregion

        #region Constructors

        public Map3DView()
        {
            InitializeComponent();

            Task.Factory.StartNew(() =>
            {
                string provinceName = "广东";
                string cityName = "南沙区";
                string fileName = cityName;

                #region 读取Geojson转Map2D Json
                ////string geoJsonString = File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\GeoJson\china.json", provinceName, cityName));

                //string geoJsonString = File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\GeoJson\{0}\{1}.json", provinceName, cityName));
                //GeoJson<GeoJsonGeometry> geoJsonModel = GeoJsonParseHelper.DecodeGeoJson(geoJsonString);

                //string geoJsonString1 = File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\GeoJson\{0}\{1}.json", provinceName, "大铲"));
                //GeoJson<GeoJsonGeometry> geoJsonModel1 = GeoJsonParseHelper.DecodeStandardGeoJson(geoJsonString1);
                //geoJsonModel1.Features[0].Id = "9999";

                //geoJsonModel.Features.Add(geoJsonModel1.Features[0]);
                //List<Map2DModel> map2DModelList = MapDataConvertHelper.GeoJsonModelToMap2DList(geoJsonModel);

                //string map2DJson = JsonConvert.SerializeObject(map2DModelList);
                //File.WriteAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", cityName), map2DJson);
                #endregion

                #region 南沙区 Map2D json

                //string map2DData = File.ReadAllText(@"C:\Users\zhuowp\Desktop\南沙区.txt");
                //List<List<mapdata>> mapdataList = JsonConvert.DeserializeObject<List<List<mapdata>>>(map2DData);
                //List<Map2DModel> map2DModelList = new List<Map2DModel>();
                //List<List<Point>> pointListList = new List<List<Point>>();
                //foreach (var areaList in mapdataList)
                //{
                //    List<Point> pointList = new List<Point>();
                //    foreach (var lngLat in areaList)
                //    {
                //        Point point = new Point() { X = lngLat.lng, Y = lngLat.lat };
                //        pointList.Add(point);
                //    }

                //    pointListList.Add(pointList);
                //}

                //Map2DModel map2DModel = MapDataConvertHelper.LngLatPointListToMap2DModel(pointListList);
                //map2DModelList.Add(map2DModel);

                //string map2DJson = JsonConvert.SerializeObject(map2DModelList);
                //File.WriteAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", cityName), map2DJson);

                #endregion

                #region 获取已有Map2D json
                string map2DJson = File.ReadAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m2d", fileName));
                List<Map2DModel> map2DModelList = JsonConvert.DeserializeObject<List<Map2DModel>>(map2DJson);
                #endregion

                List<List<List<Point>>> pointListss = map2DModelList.Select(p => p.GeometryPointList).ToList();
                List<double> bound = GraphicHelper.GetRectBound(pointListss);
                double centerX = (bound[2] - bound[0]) / 2 + bound[0];
                double centerY = (bound[3] - bound[1]) / 2 + bound[1];

                double zoomFactor = 1000 / Math.Max(bound[2] - bound[0], (bound[3] - bound[1]) * 1.78);
                for (int i = 0; i < map2DModelList.Count; i++)
                {
                    Map2DModel map2D = map2DModelList[i];
                    for (int k = map2D.GeometryPointList.Count - 1; k >= 0; k--)
                    {
                        List<Point> pointList = map2D.GeometryPointList[k];
                        for (int j = 0; j < pointList.Count; j++)
                        {
                            Point point = pointList[j];
                            point.X = (point.X - centerX) * zoomFactor;
                            point.Y = (point.Y - centerY) * zoomFactor;

                            pointList[j] = point;
                        }

                        if (pointList.Count < 3)
                        {
                            map2D.GeometryPointList.RemoveAt(k);
                        }
                    }
                }

                List<Map3DModel> map3DModelList = new List<Map3DModel>();
                for (int i = 0; i < map2DModelList.Count; i++)
                {
                    Map2DModel map2D = map2DModelList[i];
                    Map3DModel map3D = Map2DTo3DHelper.Map2DTo3DModel(map2D, 0, -10);

                    map3D.MinLongitude = map2D.MinLongitude;
                    map3D.MaxLongitude = map2D.MaxLongitude;
                    map3D.MinLatitude = map2D.MinLatitude;
                    map3D.MaxLatitude = map2D.MaxLatitude;

                    map3D.ZoomFactor = zoomFactor;
                    //map3D.Id = (i + 1).ToString();
                    map3DModelList.Add(map3D);
                }

                //string map3DsJsonString = JsonConvert.SerializeObject(map3DModelList);
                //File.WriteAllText(string.Format(@"C:\Users\zhuowp\Desktop\haiguan\map data\{0}.m3d", fileName), map3DsJsonString);

                Dispatcher.Invoke(() =>
                {
                    List<ContainerUIElement3D> uIElement3Ds = new List<ContainerUIElement3D>();
                    foreach (Map3DModel map3D in map3DModelList)
                    {
                        ContainerUIElement3D uiElement = Map2DTo3DHelper.Map3DModelToUIElement3D(map3D);
                        uIElement3Ds.Add(uiElement);
                    }

                    foreach (ContainerUIElement3D uiElement in uIElement3Ds)
                    {
                        uiElement.MouseLeftButtonDown += Container_MouseLeftButtonDown;
                        viewport3D.Children.Add(uiElement);
                    }
                });
            });
        }

        #endregion

        #region Event Methods

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;

            _mousePosition = e.GetPosition(null);
            _isMouseLeftButtonDown = true;

            Mouse.Capture(grid);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseLeftButtonDown)
            {
                Point mousePotion = e.GetPosition(null);

                double verticalMouseMove = mousePotion.Y - _mousePosition.Y;
                double verticalRotateAngle = _verticalRotateAngle + verticalMouseMove;
                if (verticalRotateAngle > 0 || verticalRotateAngle < -60)
                {
                    return;
                }

                (camera as PerspectiveCamera)?.VerticalRotateAroundCenter(verticalMouseMove, _center);

                //double horizontalMouseMove = mousePotion.X - _mousePosition.X;
                //(camera as PerspectiveCamera)?.HorizontalRotateAroundCenter(horizontalMouseMove, _center);

                _verticalRotateAngle = verticalRotateAngle;
                _mousePosition = mousePotion;
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid grid = sender as Grid;

            _isMouseLeftButtonDown = false;
            grid.ReleaseMouseCapture();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = e.Delta > 0 ? 1 : -1;
            PerspectiveCameraTransformHelper.ZoomInInSitu(camera, -zoomFactor, 5, 150);
        }

        private void BtnCombine_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = Helpers.Visual3Ds.Visual3DHelper.GetAllElement3DInViewPoirt3D<ContainerUIElement3D>(viewport3D);
            foreach (UIElement3D uiElment3D in uiElement3DList)
            {
                uiElment3D.Visibility = Visibility.Visible;

                Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                doubleAnimationTranslateTransform.To = 0;
                doubleAnimationTranslateTransform.EasingFunction = new BackEase()
                {
                    Amplitude = 0.3,
                    EasingMode = EasingMode.EaseOut,
                };

                DoubleAnimation doubleAnimationTranslateTransformY = new DoubleAnimation();
                doubleAnimationTranslateTransformY.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransformY.Duration = TimeSpan.FromMilliseconds(500);
                doubleAnimationTranslateTransformY.From = translateTransform3D.OffsetY;
                doubleAnimationTranslateTransformY.To = 0;
                doubleAnimationTranslateTransformY.EasingFunction = new BackEase()
                {
                    Amplitude = 0.3,
                    EasingMode = EasingMode.EaseOut,
                };

                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetYProperty, doubleAnimationTranslateTransformY);
            }
        }

        private void BtnSeparate_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = Helpers.Visual3Ds.Visual3DHelper.GetAllElement3DInViewPoirt3D<ContainerUIElement3D>(viewport3D);
            foreach (ContainerUIElement3D uiElement3D in uiElement3DList)
            {
                var visual = uiElement3D.Children[0] as Viewport2DVisual3D;
                var geometry = visual.Geometry as MeshGeometry3D;
                Point3D point = geometry.Positions[0];
                double x = point.X - _center.X;
                double y = point.Y - _center.Y;

                double a = Math.Sqrt(2000 * 2000 / (x * x + y * y));

                Transform3DGroup transform3DGroup = uiElement3D.Transform as Transform3DGroup;
                TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(3000);
                doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                doubleAnimationTranslateTransform.To = x * a;
                doubleAnimationTranslateTransform.Completed += (s, args) => uiElement3D.Visibility = Visibility.Collapsed;
                doubleAnimationTranslateTransform.EasingFunction = new BackEase()
                {
                    Amplitude = 0.3,
                    EasingMode = EasingMode.EaseIn,
                };

                DoubleAnimation doubleAnimationTranslateTransformY = new DoubleAnimation();
                doubleAnimationTranslateTransformY.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransformY.Duration = TimeSpan.FromMilliseconds(3000);
                doubleAnimationTranslateTransformY.From = translateTransform3D.OffsetX;
                doubleAnimationTranslateTransformY.To = y * a;
                doubleAnimationTranslateTransformY.EasingFunction = new BackEase()
                {
                    Amplitude = 0.3,
                    EasingMode = EasingMode.EaseIn,
                };

                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetYProperty, doubleAnimationTranslateTransformY);
            }
        }

        private void BtnSeparate2_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = Helpers.Visual3Ds.Visual3DHelper.GetAllElement3DInViewPoirt3D<ContainerUIElement3D>(viewport3D);
            foreach (ContainerUIElement3D uiElement3D in uiElement3DList)
            {
                var visual = uiElement3D.Children[0] as Viewport2DVisual3D;
                var geometry = visual.Geometry as MeshGeometry3D;
                Point3D point = geometry.Positions[0];
                double x = point.X - _center.X;
                double y = point.Y - _center.Y;

                double a = Math.Sqrt(1000 * 1000 / (x * x + y * y));

                Transform3DGroup transform3DGroup = uiElement3D.Transform as Transform3DGroup;
                TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(3000);
                doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                doubleAnimationTranslateTransform.To = x * a;
                doubleAnimationTranslateTransform.Completed += (s, args) => uiElement3D.Visibility = Visibility.Collapsed;
                doubleAnimationTranslateTransform.EasingFunction = new CircleEase()
                {
                    EasingMode = EasingMode.EaseInOut,
                };

                DoubleAnimation doubleAnimationTranslateTransformY = new DoubleAnimation();
                doubleAnimationTranslateTransformY.BeginTime = new TimeSpan(0, 0, 0);
                doubleAnimationTranslateTransformY.Duration = TimeSpan.FromMilliseconds(3000);
                doubleAnimationTranslateTransformY.From = translateTransform3D.OffsetX;
                doubleAnimationTranslateTransformY.To = y * a;
                doubleAnimationTranslateTransformY.EasingFunction = new CircleEase()
                {
                    EasingMode = EasingMode.EaseInOut,
                };

                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetYProperty, doubleAnimationTranslateTransformY);
            }
        }

        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ContainerUIElement3D container = sender as ContainerUIElement3D;

            Transform3DGroup transform3DGroup = container.Transform as Transform3DGroup;
            TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;
            ScaleTransform3D scaleTransform3D = transform3DGroup.Children[1] as ScaleTransform3D;
            RotateTransform3D rotateTransform3D = transform3DGroup.Children[2] as RotateTransform3D;

            bool isClicked = false;
            if (translateTransform3D.OffsetZ == 0)
            {
                isClicked = false;
            }
            else
            {
                isClicked = true;
            }

            //位置变换
            DoubleAnimation doubleAnimationZ = new DoubleAnimation();
            doubleAnimationZ.BeginTime = new TimeSpan(0, 0, 0);
            doubleAnimationZ.Duration = TimeSpan.FromMilliseconds(500);
            doubleAnimationZ.From = translateTransform3D.OffsetZ;
            doubleAnimationZ.EasingFunction = new ExponentialEase() { EasingMode = EasingMode.EaseInOut };
            if (isClicked)
            {
                doubleAnimationZ.To = 0;
            }
            else
            {
                doubleAnimationZ.To = -95;
            }
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationZ);

            //缩放
            DoubleAnimation scaleTransformAnimation = new DoubleAnimation();
            scaleTransformAnimation.BeginTime = new TimeSpan(0, 0, 0);
            scaleTransformAnimation.Duration = TimeSpan.FromMilliseconds(500);
            scaleTransformAnimation.From = scaleTransform3D.ScaleX;
            if (isClicked)
            {
                scaleTransformAnimation.To = 1;
            }
            else
            {
                scaleTransformAnimation.To = 1.5;
            }
            scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleXProperty, scaleTransformAnimation);
            scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleYProperty, scaleTransformAnimation);
            scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleZProperty, scaleTransformAnimation);

            ////点击变色
            //Color newsurfaceColor;
            //if (isClicked)
            //{
            //    newsurfaceColor = Color.FromArgb(0x99, 0x00, 0xff, 0xff);
            //}
            //else
            //{
            //    newsurfaceColor = Color.FromArgb(0xff, 0xff, 0x00, 0x00);
            //}

            //for (int i = 1; i < container.Children.Count; i = i + 3)
            //{
            //    var visualElement = container.Children[i] as Viewport2DVisual3D;
            //    if (visualElement != null)
            //    {
            //        visualElement.Material = new DiffuseMaterial(new SolidColorBrush(newsurfaceColor));
            //    }
            //}

            isClicked = !isClicked;
        }

        #endregion

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            var d = new SaveFileDialog();
            d.Filter = Exporters.Filter;
            d.DefaultExt = Exporters.DefaultExtension;
            if (!d.ShowDialog().Value)
            {
                return;
            }

            Viewport3DHelper.Export(viewport3D, d.FileName);
        }
    }
}
