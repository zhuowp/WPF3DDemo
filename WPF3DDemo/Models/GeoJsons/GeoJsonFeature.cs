using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonFeature<T> where T : GeoJsonGeometryBase
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public GeoJsonFeatureProperty Properties { get; set; }

        [JsonProperty("geometry")]
        public T Geometry { get; set; }
    }
}
