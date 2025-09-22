using xml.Revit.Dev.DocumentBase.Extensions;

namespace xml.Revit.Dev.DocumentBase.Models
{
    /// <summary>
    /// element model base
    /// </summary>
    public abstract class ElementModel
    {
        /// <summary>
        /// UniqueId
        /// </summary>
        [LiteDB.BsonId]
        public string UniqueId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 起点
        /// </summary>
        public PointModel EndPoint0 { get; set; }

        /// <summary>
        /// 弧线中心点
        /// </summary>
        public PointModel Center { get; set; }

        /// <summary>
        /// 终点
        /// </summary>
        public PointModel EndPoint1 { get; set; }

        /// <summary>
        /// 定位线是否为弧线
        /// </summary>
        public bool IsArc { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string ElementTypeName { get; set; }

        /// <summary>
        /// 获取定位线
        /// </summary>
        /// <returns></returns>
        public Curve GetLocationCurve()
        {
            if (IsArc)
            {
                return Arc.Create(EndPoint0.ToXYZ(), EndPoint1.ToXYZ(), Center.ToXYZ());
            }
            else
            {
                return Line.CreateBound(EndPoint0.ToXYZ(), EndPoint1.ToXYZ());
            }
        }

        /// <summary>
        /// 获取类型名称
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected static string GetElementTypeName(Element element)
        {
            return element.GetTypeId().ToElement<ElementType>(XmlDoc.UIdoc.Document)?.Name;
        }
    }
}
