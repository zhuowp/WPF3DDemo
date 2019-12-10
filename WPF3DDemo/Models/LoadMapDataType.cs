using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models
{
    public enum LoadMapDataType
    {
        Unknown = 0,
        GeoJson = 1,
        EncodedGeoJson = 2,
        Txt = 3,
        Map2D = 4,
        Map3D = 5,
    }
}
