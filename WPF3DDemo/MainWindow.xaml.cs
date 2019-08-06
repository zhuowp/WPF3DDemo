using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WPF3DDemo.Helpers;
using WPF3DDemo.Models.Map2Ds;

namespace WPF3DDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private Wave3D _wave3D = null;

        public MainWindow()
        {
            InitializeComponent();
            List<List<Point>> keyPointList = InitMapKeyPoints();
            Map2DInfo map2DInfo = GetMap2DInfo();

            foreach (var keyPoints in keyPointList)
            {
                ContainerUIElement3D container = Visual3DHelper.ClosedPathToContainerUIElement3D(keyPoints, 0, -50);

                container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
                container.MouseEnter += Container_MouseEnter;
                container.MouseLeave += Container_MouseLeave;

                viewport3D.Children.Add(container);
            }

            //foreach (Map2DArea map2DArea in map2DInfo.MapAreaList)
            //{
            //    ContainerUIElement3D container = Visual3DHelper.ClosedPathToContainerUIElement3D(map2DArea.Geometry.GeometryPointList, 0, -50);

            //    container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
            //    viewport3D.Children.Add(container);
            //}

            _wave3D = new Wave3D(new Point(200, 200), new Point(800, 800), 250, -50, geometryModel3DWaves);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(3000);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Container_MouseLeave(object sender, MouseEventArgs e)
        {
            //ContainerUIElement3D container = sender as ContainerUIElement3D;

            //Transform3DGroup transform3DGroup = container.Transform as Transform3DGroup;
            //TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;
            //ScaleTransform3D scaleTransform3D = transform3DGroup.Children[1] as ScaleTransform3D;
            //RotateTransform3D rotateTransform3D = transform3DGroup.Children[2] as RotateTransform3D;

            ////位置变换
            //DoubleAnimation doubleAnimationZ = new DoubleAnimation();
            //doubleAnimationZ.BeginTime = new TimeSpan(0, 0, 0);
            //doubleAnimationZ.Duration = TimeSpan.FromMilliseconds(500);
            //doubleAnimationZ.From = translateTransform3D.OffsetZ;
            //doubleAnimationZ.To = 0;

            //translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationZ);
        }

        private void Container_MouseEnter(object sender, MouseEventArgs e)
        {
            //ContainerUIElement3D container = sender as ContainerUIElement3D;

            //Transform3DGroup transform3DGroup = container.Transform as Transform3DGroup;
            //TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;
            //ScaleTransform3D scaleTransform3D = transform3DGroup.Children[1] as ScaleTransform3D;
            //RotateTransform3D rotateTransform3D = transform3DGroup.Children[2] as RotateTransform3D;

            ////位置变换
            //DoubleAnimation doubleAnimationZ = new DoubleAnimation();
            //doubleAnimationZ.BeginTime = new TimeSpan(0, 0, 0);
            //doubleAnimationZ.Duration = TimeSpan.FromMilliseconds(500);
            //doubleAnimationZ.From = translateTransform3D.OffsetZ;
            //doubleAnimationZ.To = -100;

            //translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationZ);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            //_wave3D.CreateCircularWaveSource(150, 150, -10, 1);
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
            if (isClicked)
            {
                doubleAnimationZ.To = 0;
            }
            else
            {
                doubleAnimationZ.To = -45;
            }
            translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetZProperty, doubleAnimationZ);

            ////缩放
            //DoubleAnimation scaleTransformAnimation = new DoubleAnimation();
            //scaleTransformAnimation.BeginTime = new TimeSpan(0, 0, 0);
            //scaleTransformAnimation.Duration = TimeSpan.FromMilliseconds(500);
            //scaleTransformAnimation.From = scaleTransform3D.ScaleX;
            //if (isClicked)
            //{
            //    scaleTransformAnimation.To = 1;
            //}
            //else
            //{
            //    scaleTransformAnimation.To = 1.5;
            //}
            //scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleXProperty, scaleTransformAnimation);
            //scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleYProperty, scaleTransformAnimation);
            //scaleTransform3D.BeginAnimation(ScaleTransform3D.ScaleZProperty, scaleTransformAnimation);

            //点击变色
            Color newsurfaceColor;
            if (isClicked)
            {
                newsurfaceColor = Color.FromArgb(0x99, 0x00, 0xff, 0xff);
            }
            else
            {
                newsurfaceColor = Color.FromArgb(0xff, 0xff, 0x00, 0x00);
            }
            for (int i = 1; i < container.Children.Count; i++)
            {
                var visualElement = container.Children[i] as Viewport2DVisual3D;
                if (visualElement != null)
                {
                    visualElement.Material = new DiffuseMaterial(new SolidColorBrush(newsurfaceColor));
                }
            }

            isClicked = !isClicked;
        }

        private List<List<Point>> InitMapKeyPoints()
        {
            string allPointsString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\china.txt");
            //string allPointsString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\guangdong.txt").Trim();
            string[] allPointsStringArray = allPointsString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
            List<List<Point>> allPointList = new List<List<Point>>();
            for (int k = 0; k < allPointsStringArray.Length; k++)
            {
                string pointsString = allPointsStringArray[k];
                string[] pointStringArray = pointsString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                List<Point> pointList = new List<Point>();
                for (int i = 0; i < pointStringArray.Length; i++)
                {
                    string[] pointString = pointStringArray[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (pointString.Length == 2)
                    {
                        Point point = new Point();
                        point.X = double.Parse(pointString[0]);
                        point.Y = double.Parse(pointString[1]);

                        pointList.Add(point);
                    }
                }
                allPointList.Add(pointList);
            }
            return allPointList;
        }

        private Map2DInfo GetMap2DInfo()
        {
            string map2DInfoString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\guangdong.json");
            Map2DInfo map2DInfo = JsonConvert.DeserializeObject<Map2DInfo>(map2DInfoString);

            return map2DInfo;
        }

        private Point _mousePosition;
        private bool _isMouseLeftButtonDown = false;

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _mousePosition = e.GetPosition(null);
            _isMouseLeftButtonDown = true;

            //Mouse.Capture(this);
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseLeftButtonDown)
            {
                Point mousePotion = e.GetPosition(null);

                double verticalMouseMove = mousePotion.Y - _mousePosition.Y;
                (camera as PerspectiveCamera)?.VerticalRotateAroundCenter(verticalMouseMove, new Point3D(400, 330, 0));

                //double horizontalMouseMove = mousePotion.X - _mousePosition.X;
                //(camera as PerspectiveCamera)?.HorizontalRotateAroundCenter(horizontalMouseMove, new Point3D(400, 330, 0));

                _mousePosition = mousePotion;
            }
        }

        private void Grid_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isMouseLeftButtonDown = false;

            //this.ReleaseMouseCapture();
        }

        private void Grid_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double zoomFactor = e.Delta > 0 ? 1 : -1;
            PerspectiveCameraTransformHelper.ZoomInInSitu((PerspectiveCamera)camera, -zoomFactor, 5, 150);
        }

        private void BtnSeparate_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = GetAllUIElement3DInViewPoirt3D(viewport3D);
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in uiElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = 0;
                        doubleAnimationTranslateTransform.To = 2000;

                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;
                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(20);
                }
            });
        }

        private List<ContainerUIElement3D> GetAllUIElement3DInViewPoirt3D(Viewport3D viewport3D)
        {
            List<ContainerUIElement3D> uiElement3DList = new List<ContainerUIElement3D>();
            foreach (Visual3D visual3D in viewport3D.Children)
            {
                ContainerUIElement3D uiElement3D = visual3D as ContainerUIElement3D;
                if (uiElement3D != null)
                {
                    uiElement3DList.Add(uiElement3D);
                }
            }

            return uiElement3DList;
        }

        private void BtnCombine_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = GetAllUIElement3DInViewPoirt3D(viewport3D);
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in uiElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                        doubleAnimationTranslateTransform.To = 0;

                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(10);
                }
            });
        }

        private void BtnSeparate2_Click(object sender, RoutedEventArgs e)
        {
            List<ContainerUIElement3D> uiElement3DList = GetAllUIElement3DInViewPoirt3D(viewport3D);
            List<UIElement3D> upLeftElement3DList = new List<UIElement3D>();
            List<UIElement3D> upRightElement3DList = new List<UIElement3D>();
            List<UIElement3D> downLeftElement3DList = new List<UIElement3D>();
            List<UIElement3D> downRightElement3DList = new List<UIElement3D>();
            foreach (UIElement3D uiElment3D in uiElement3DList)
            {
                GeneralTransform3DTo2D transform3To2 = uiElment3D.TransformToAncestor(viewport3D);
                Point point = transform3To2.Transform(new Point3D());

                if (point.X < 400 && point.Y < 330)
                {
                    upLeftElement3DList.Add(uiElment3D);
                }
                else if (point.X > 400 && point.Y < 330)
                {
                    upRightElement3DList.Add(uiElment3D);
                }
                else if (point.X < 400 && point.Y > 330)
                {
                    downLeftElement3DList.Add(uiElment3D);
                }
                else
                {
                    downRightElement3DList.Add(uiElment3D);
                }
            }
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in upLeftElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                        doubleAnimationTranslateTransform.To = 2000;

                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(10);
                }
            });
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in upRightElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                        doubleAnimationTranslateTransform.To = 2000;

                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(10);
                }
            });
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in downLeftElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                        doubleAnimationTranslateTransform.To = 2000;

                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(10);
                }
            });
            Task.Factory.StartNew(() =>
            {
                foreach (UIElement3D uiElment3D in downRightElement3DList)
                {
                    Dispatcher.Invoke(() =>
                    {
                        Transform3DGroup transform3DGroup = uiElment3D.Transform as Transform3DGroup;
                        TranslateTransform3D translateTransform3D = transform3DGroup.Children[0] as TranslateTransform3D;

                        DoubleAnimation doubleAnimationTranslateTransform = new DoubleAnimation();
                        doubleAnimationTranslateTransform.BeginTime = new TimeSpan(0, 0, 0);
                        doubleAnimationTranslateTransform.Duration = TimeSpan.FromMilliseconds(500);
                        doubleAnimationTranslateTransform.From = translateTransform3D.OffsetX;
                        doubleAnimationTranslateTransform.To = 2000;

                        translateTransform3D.BeginAnimation(TranslateTransform3D.OffsetXProperty, doubleAnimationTranslateTransform);
                    });

                    Thread.Sleep(10);
                }
            });
        }

        private void BtnCombine2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnWave_Click(object sender, RoutedEventArgs e)
        {
            _wave3D.CreateCircularWaveSource(150, 150, -10, 1);
        }
    }
}
