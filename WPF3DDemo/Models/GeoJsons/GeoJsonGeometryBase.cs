using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonGeometryBase
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
