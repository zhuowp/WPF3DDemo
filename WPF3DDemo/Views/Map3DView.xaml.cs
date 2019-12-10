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

        private string _loadFilePath = "";
        List<Map2DModel> _map2DModelList = null;
        List<Map3DModel> _map3DModelList = null;

        private LoadMapDataType _loadMapDataType = LoadMapDataType.Unknown;

        #endregion

        #region Constructors

        public Map3DView()
        {
            InitializeComponent();
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

        private List<Map3DModel> Map2DTo3DData(List<Map2DModel> map2DModelList)
        {
            Rect rect = GetMap3DModelsBoundaryRect(map2DModelList);

            double zoomFactor = InflateMap2DShapeInBoundaryRectCenter(map2DModelList, rect);

            List<Map3DModel> map3DModelList = new List<Map3DModel>();
            for (int i = 0; i < map2DModelList.Count; i++)
            {
                Map3DModel map3D = Map2DTo3DHelper.Map2DTo3DModel(map2DModelList[i], 0, -10);
                map3D.ZoomFactor = zoomFactor;

                map3DModelList.Add(map3D);
            }

            return map3DModelList;
        }

        private double InflateMap2DShapeInBoundaryRectCenter(List<Map2DModel> map2DModelList, Rect rect)
        {
            Point centerPoint = FeaturePointHelper.GetRectCenter(ref rect);

            double zoomFactor = 1000 / Math.Max(rect.Width, rect.Height * 1.78);
            for (int i = 0; i < map2DModelList.Count; i++)
            {
                Map2DModel map2D = map2DModelList[i];
                for (int k = map2D.GeometryPointList.Count - 1; k >= 0; k--)
                {
                    List<Point> pointList = map2D.GeometryPointList[k];
                    for (int j = 0; j < pointList.Count; j++)
                    {
                        Point point = pointList[j];
                        point.X = (point.X - centerPoint.X) * zoomFactor;
                        point.Y = (point.Y - centerPoint.Y) * zoomFactor;

                        pointList[j] = point;
                    }

                    if (pointList.Count < 3)
                    {
                        map2D.GeometryPointList.RemoveAt(k);
                    }
                }
            }

            return zoomFactor;
        }

        private Rect GetMap3DModelsBoundaryRect(List<Map2DModel> map2DModelList)
        {
            List<Point> allFeaturePoints = new List<Point>();
            foreach (Map2DModel map2D in map2DModelList)
            {
                foreach (List<Point> points in map2D.GeometryPointList)
                {
                    allFeaturePoints.AddRange(points);
                }
            }
            Rect rect = FeaturePointHelper.GetFeaturePointsBoundaryRect(allFeaturePoints);
            return rect;
        }

        private void InitMap3DComponents(List<Map3DModel> map3DModelList)
        {
            Dispatcher.Invoke(() =>
            {
                for (int i = viewport3D.Children.Count - 1; i >= 0; i--)
                {
                    if (viewport3D.Children[i] is ContainerUIElement3D)
                    {
                        viewport3D.Children.Remove(viewport3D.Children[i]);
                    }
                }

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
        }

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

        private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            string path = GetSelectedFileFullPath();
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _loadFilePath = path;
            ttbSourceFilePath.Text = _loadFilePath;
        }

        private void BtnLoadMap3D_Click(object sender, RoutedEventArgs e)
        {
            if (_loadMapDataType == LoadMapDataType.Unknown)
            {
                MessageBox.Show("请选择加载文件的类型");
                return;
            }

            if (string.IsNullOrEmpty(_loadFilePath))
            {
                MessageBox.Show("请选择需要加载的文件");
                return;
            }

            string fileStr = File.ReadAllText(_loadFilePath);
            if (string.IsNullOrEmpty(fileStr))
            {
                MessageBox.Show("文件数据为空");
                return;
            }

            GeoJson<GeoJsonGeometry> geoJsonModel = null;

            switch (_loadMapDataType)
            {
                case LoadMapDataType.GeoJson:
                    geoJsonModel = GeoJsonParseHelper.DecodeStandardGeoJson(fileStr);
                    _map2DModelList = MapDataConvertHelper.GeoJsonModelToMap2DList(geoJsonModel);
                    _map3DModelList = Map2DTo3DData(_map2DModelList);
                    break;
                case LoadMapDataType.EncodedGeoJson:
                    geoJsonModel = GeoJsonParseHelper.DecodeGeoJson(fileStr);
                    _map2DModelList = MapDataConvertHelper.GeoJsonModelToMap2DList(geoJsonModel);
                    _map3DModelList = Map2DTo3DData(_map2DModelList);
                    break;
                case LoadMapDataType.Txt:
                    _map2DModelList = ParseMap2DDataFromLatLngString(fileStr);
                    _map3DModelList = Map2DTo3DData(_map2DModelList);
                    break;
                case LoadMapDataType.Map2D:
                    _map2DModelList = JsonConvert.DeserializeObject<List<Map2DModel>>(fileStr);
                    _map3DModelList = Map2DTo3DData(_map2DModelList);
                    break;
                case LoadMapDataType.Map3D:
                    _map3DModelList = JsonConvert.DeserializeObject<List<Map3DModel>>(fileStr);
                    break;
                default:
                    break;
            }

            InitMap3DComponents(_map3DModelList);
        }

        private void LoadTypeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton == null || string.IsNullOrEmpty(radioButton.Tag.ToString()))
            {
                return;
            }

            _loadMapDataType = (LoadMapDataType)int.Parse(radioButton.Tag.ToString());
        }

        public string GetSelectedFileFullPath()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "All files |*.*|Text |*.txt|Json |*.json"
            };

            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                return openFileDialog.FileName;
            }
            else
            {
                return "";
            }
        }

        private List<Map2DModel> ParseMap2DDataFromLatLngString(string latLngStr)
        {
            List<Map2DModel> map2DModelList = new List<Map2DModel>();
            List<List<Point>> pointListList = new List<List<Point>>();

            string[] featurePointStrings = latLngStr.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            if (featurePointStrings != null)
            {
                List<Point> pointList = new List<Point>();
                pointListList.Add(pointList);
                foreach (string lngLatStr in featurePointStrings)
                {
                    string[] lngLatStrArray = lngLatStr.Split(new string[] { ",", "，" }, StringSplitOptions.RemoveEmptyEntries);
                    if (lngLatStrArray == null || lngLatStrArray.Length != 2)
                    {
                        continue;
                    }
                    Point point = new Point() { X = double.Parse(lngLatStrArray[0]), Y = double.Parse(lngLatStrArray[1]) };
                    pointList.Add(point);
                }
            }

            Map2DModel map2DModel = MapDataConvertHelper.LngLatPointListToMap2DModel(pointListList);
            map2DModelList.Add(map2DModel);

            return map2DModelList;
        }

        private void btnSaveMap2D_Click(object sender, RoutedEventArgs e)
        {
            if (_map2DModelList == null)
            {
                MessageBox.Show("地图数据为空，无法保存");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Map2D文件|*.m2d";

            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;

                string map2DsJsonString = JsonConvert.SerializeObject(_map2DModelList);
                File.WriteAllText(fileName, map2DsJsonString);
                MessageBox.Show("保存成功");
            }
        }

        private void btnSaveMap3D_Click(object sender, RoutedEventArgs e)
        {
            if (_map3DModelList == null)
            {
                MessageBox.Show("地图数据为空，无法保存");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Map3D文件|*.m3d";

            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;

                string map3DsJsonString = JsonConvert.SerializeObject(_map3DModelList);
                File.WriteAllText(fileName, map3DsJsonString);
                MessageBox.Show("保存成功");
            }
        }
    }
}
