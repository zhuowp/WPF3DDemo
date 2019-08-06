using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF3DDemo.Models.Map2Ds
{
    public class Map2DPropertyInfo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("cp")]
        public List<double> CapitalLocation
        {
            get
            {
                if (CapitalLocationPoint == null)
                {
                    return null;
                }
                else
                {
                    return new List<double>() { CapitalLocationPoint.X, CapitalLocationPoint.Y };
                }
            }
            set
            {
                if (value == null)
                {
                    CapitalLocationPoint = new Point();
                }
                else
                {
                    CapitalLocationPoint = new Point() { X = value[0], Y = value[1] };
                }
            }
        }

        [JsonIgnore]
        public Point CapitalLocationPoint { get; set; }

        [JsonProperty("childNum")]
        public int ChildrenCount { get; set; }
    }
}
