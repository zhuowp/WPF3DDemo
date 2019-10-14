using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJson<T> where T : GeoJsonGeometryBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("features")]
        public List<GeoJsonFeature<T>> Features { get; set; }

        [JsonProperty("UTF8Encoding")]
        public bool IsUTF8Encoding { get; set; }
    }
}
