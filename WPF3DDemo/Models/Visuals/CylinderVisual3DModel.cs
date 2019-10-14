using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.Visuals
{
    public class CylinderVisual3DModel
    {
        public Geometry3DModel UpperUndersurface { get; set; }
        public Geometry3DModel LowerUndersurface { get; set; }
        public Geometry3DModel SideSurface { get; set; }

        public Material3DModel UpperUndersurfaceMaterial { get; set; }
        public Material3DModel LowerUndersurfaceMaterial { get; set; }
        public Material3DModel SideSurfaceMaterial { get; set; }
    }
}
