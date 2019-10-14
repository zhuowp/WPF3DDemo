using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using WPF3DDemo.Models;

namespace WPF3DDemo.Helpers.Visual3Ds
{
    public class Visual3DHelper
    {
        public static List<T> GetAllElement3DInViewPoirt3D<T>(Viewport3D viewport3D) where T : UIElement3D
        {
            List<T> uiElement3DList = new List<T>();
            foreach (Visual3D visual3D in viewport3D.Children)
            {
                T uiElement3D = visual3D as T;
                if (uiElement3D != null)
                {
                    uiElement3DList.Add(uiElement3D);
                }
            }

            return uiElement3DList;
        }
    }
}
