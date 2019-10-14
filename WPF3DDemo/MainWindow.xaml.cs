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
using WPF3DDemo.Models.GeoJsons;
using WPF3DDemo.Models.Map2Ds;

namespace WPF3DDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        //private Wave3D _wave3D = null;

        public MainWindow()
        {
            InitializeComponent();

            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\china.json");
            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\广东.json");
            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\广州海关关区.json");
            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\广州市.json");
            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\nansha.json");
            //string geoJsonString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\map (3) (1).geojson");

            ////GeoJson<GeoJsonGeometry> geoJsonModel = GeoJsonParseHelper.DecodeGeoJson(geoJsonString);
            //GeoJson<GeoJsonGeometry> geoJsonModel = GeoJsonParseHelper.DecodeStandardGeoJson(geoJsonString);
            ////string jsonString = JsonConvert.SerializeObject(geoJsonModel);
            //List<Map2DModel> map2DModelList = MapDataConvertHelper.GeoJsonModelToMap2DList(geoJsonModel);
            //string map2DJson = JsonConvert.SerializeObject(map2DModelList);

            //File.WriteAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\guangdongmap2d.txt", map2DJson);
            //File.WriteAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\guangzhoucustomsmap2d.txt", map2DJson);
            //File.WriteAllText(@"C:\Users\zhuowp\Desktop\haiguan\map data\guangzhoumap2d.txt", map2DJson);

            /*
            string map2DJson = File.ReadAllText(@"C:\Users\zhuowp\Desktop\guangdong.txt");
            List<Map2DModel> map2DModelList = JsonConvert.DeserializeObject<List<Map2DModel>>(map2DJson);

            string map2DModelJson = JsonConvert.SerializeObject(map2DModelList);
            List<ContainerUIElement3D> uIElement3Ds = new List<ContainerUIElement3D>();
            foreach (Map2DModel map2D in map2DModelList)
            {
                List<List<Point>> areaPointList = new List<List<Point>>();
                foreach (List<Point> pointList in map2D.GeometryPointList)
                {
                    List<Point> fixedPointList = new List<Point>();
                    areaPointList.Add(fixedPointList);
                    foreach (Point longLatPoint in pointList)
                    {
                        Point point = new Point() { X = longLatPoint.X * 100, Y = longLatPoint.Y * 100 };
                        fixedPointList.Add(point);
                    }
                }
                ContainerUIElement3D container = Visual2DTo3DHelper.MultiClosedAreaToContainerUIElement3D(areaPointList, 0, -50, map2D.Texture);

                container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
                uIElement3Ds.Add(container);
            }

            foreach (ContainerUIElement3D container in uIElement3Ds)
            {
                viewport3D.Children.Add(container);
            }*/

            //List<ContainerUIElement3D> uIElement3Ds = new List<ContainerUIElement3D>();
            //foreach (GeoJsonFeature map2DArea in geoJsonModel.Features)
            //{
            //    List<List<Point>> areaPointList = new List<List<Point>>();
            //    foreach (List<Point> pointList in map2DArea.Geometry.PointList)
            //    {
            //        List<Point> fixedPointList = new List<Point>();
            //        areaPointList.Add(fixedPointList);
            //        foreach (Point longLatPoint in pointList)
            //        {
            //            Point point = new Point() { X = (longLatPoint.X - 105) * 100, Y = (longLatPoint.Y - 20) * 100 };
            //            fixedPointList.Add(point);
            //        }
            //    }
            //    ContainerUIElement3D container = Visual3DHelper.MultiClosedAreaToContainerUIElement3D(areaPointList, 0, -50);

            //    container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
            //    uIElement3Ds.Add(container);
            //}

            //foreach (ContainerUIElement3D container in uIElement3Ds)
            //{
            //    viewport3D.Children.Add(container);
            //}

            //List<List<Point>> keyPointList = InitMapKeyPoints();
            //foreach (var keyPoints in keyPointList)
            //{
            //    ContainerUIElement3D container = Visual3DHelper.ClosedPathToContainerUIElement3D(keyPoints, 0, -51);

            //    container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
            //    container.MouseEnter += Container_MouseEnter;
            //    container.MouseLeave += Container_MouseLeave;

            //    viewport3D.Children.Add(container);
            //}

            //Map2DModel map2DInfo = GetMap2DInfo();
            //foreach (Map2DArea map2DArea in map2DInfo.MapAreaList)
            //{
            //    List<Point> areaPointList = new List<Point>();
            //    foreach(Point longLatPoint in map2DArea.Geometry.GeometryPointList)
            //    {
            //        Point point = new Point() { X = (longLatPoint.X - 105) * 100, Y = (longLatPoint.Y - 20) *100 };
            //        areaPointList.Add(point);
            //    }
            //    ContainerUIElement3D container = Visual3DHelper.ClosedPathToContainerUIElement3D(areaPointList, 0, -50);

            //    container.MouseLeftButtonDown += Container_MouseLeftButtonDown;
            //    viewport3D.Children.Add(container);
            //}

            //_wave3D = new Wave3D(new Point(200, 200), new Point(800, 800), 250, -50, geometryModel3DWaves);

            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromMilliseconds(3000);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        //private void Timer_Tick(object sender, EventArgs e)
        //{
        //    //_wave3D.CreateCircularWaveSource(150, 150, -10, 1);
        //}

        //private List<List<Point>> InitMapKeyPoints()
        //{
        //    string allPointsString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\china.txt");
        //    //string allPointsString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\guangdong.txt").Trim();
        //    string[] allPointsStringArray = allPointsString.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        //    List<List<Point>> allPointList = new List<List<Point>>();
        //    for (int k = 0; k < allPointsStringArray.Length; k++)
        //    {
        //        string pointsString = allPointsStringArray[k];
        //        string[] pointStringArray = pointsString.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

        //        List<Point> pointList = new List<Point>();
        //        for (int i = 0; i < pointStringArray.Length; i++)
        //        {
        //            string[] pointString = pointStringArray[i].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
        //            if (pointString.Length == 2)
        //            {
        //                Point point = new Point();
        //                point.X = double.Parse(pointString[0]);
        //                point.Y = double.Parse(pointString[1]);

        //                pointList.Add(point);
        //            }
        //        }
        //        allPointList.Add(pointList);
        //    }
        //    return allPointList;
        //}

        //private Map2DModel GetMap2DInfo()
        //{
        //    string map2DInfoString = File.ReadAllText(@"C:\Users\zhuowp\Desktop\guangdong.json");
        //    Map2DModel map2DInfo = JsonConvert.DeserializeObject<Map2DModel>(map2DInfoString);

        //    return map2DInfo;
        //}
    }
}
