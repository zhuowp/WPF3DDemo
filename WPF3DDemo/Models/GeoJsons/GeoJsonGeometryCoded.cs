using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonGeometryCoded : GeoJsonGeometryBase
    {
        [JsonProperty("coordinates")]
        public List<string> Coordinates { get; set; }

        [JsonProperty("encodeOffsets")]
        public List<List<double>> EncodeOffsets { get; set; }
    }
}
