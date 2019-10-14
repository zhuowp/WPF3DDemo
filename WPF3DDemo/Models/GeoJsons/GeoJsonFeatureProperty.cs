using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.GeoJsons
{
    public class GeoJsonFeatureProperty
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cp")]
        public List<double> CapitalLocation { get; set; }

        [JsonProperty("childNum")]
        public int ChildrenCount { get; set; }

        [JsonIgnore]
        public Point CapitalLocationPoint { get; set; }

    }
}
