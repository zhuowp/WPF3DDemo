using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.Visuals
{
    public enum MaterialType : int
    {
        ColorDiffuse = 0,
        ImageDiffuse = 1,
        Emissive = 2,
        Specular = 3,
        MaterialGroup = 4,
    }
}
