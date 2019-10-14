using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonGeometryStandard : GeoJsonGeometryBase
    {
        [JsonProperty("coordinates")]
        public List<List<List<double>>> Coordinates { get; set; }
    }
}
