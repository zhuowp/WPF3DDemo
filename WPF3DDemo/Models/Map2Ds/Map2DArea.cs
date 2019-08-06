using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.Map2Ds
{
    public class Map2DArea
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Map2DPropertyInfo Properties { get; set; }

        [JsonProperty("geometry")]
        public Map2DGeometryInfo Geometry { get; set; }
    }
}
