using Newtonsoft.Json;

namespace xml.Revit.Dev.DocumentBase.Models
{
    /// <summary>
    /// 点
    /// </summary>
    public sealed class PointModel
    {
        [JsonProperty("x")]
        public double X { get; set; }

        [JsonProperty("y")]
        public double Y { get; set; }

        [JsonProperty("z")]
        public double Z { get; set; }

        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({X},{Y},{Z})";
        }

        public PointModel() { }
    }
}
