/* 作    者: xml
** 创建时间: 2024/5/30 20:17:54
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using xml.Revit.Dev.DocumentBase.Extensions;

namespace xml.Revit.Dev.DocumentBase.Models
{
    /// <summary>
    /// 基本墙
    /// </summary>
    public sealed class WallModel : ElementModel
    {
        public WallModel() { }

        public WallModel(Element element)
        {
            if (element is not Wall wall)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var curve = wall.GetLocationCurve();

            UniqueId = wall.UniqueId;
            Name = wall.Name;
            ElementTypeName = GetElementTypeName(wall);
            EndPoint0 = curve.GetEndPoint(0).ToPointModel();
            Center = curve.Evaluate(0.5, true).ToPointModel();
            EndPoint1 = curve.GetEndPoint(1).ToPointModel();
            Structural = wall.get_Parameter(BuiltInParameter.WALL_STRUCTURAL_SIGNIFICANT)
                .AsInteger();

            IsArc = curve is Arc;

            Width = wall.Width;
            BaseOffset = wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).AsDouble();
            TopOffset = wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).AsDouble();
            var baseLevelId = wall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT)
                .AsElementId();
            var topLevelId = wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).AsElementId();

            var doc = wall.Document;
            BaseLevelName = baseLevelId.ToElement<Level>(doc).Name;
            Height = wall.get_Parameter(BuiltInParameter.WALL_USER_HEIGHT_PARAM).AsDouble();
            if (topLevelId != ElementId.InvalidElementId)
            {
                TopLevelName = topLevelId?.ToElement<Level>(doc).Name;
            }
        }

        public double Width { get; set; }

        public string BaseLevelName { get; set; }
        public string TopLevelName { get; set; } = "";
        public double BaseOffset { get; set; } = 0;
        public double TopOffset { get; set; } = 0;

        /// <summary>
        /// 墙高
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 是否是结构墙
        /// <para>0:不是</para>
        /// <para>1:是</para>
        /// <para>参数来源 WALL_STRUCTURAL_SIGNIFICANT</para>
        /// </summary>
        public int Structural { get; set; }
    }
}
