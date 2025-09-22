/* 作    者: xml
** 创建时间: 2024/5/26 16:41:19
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
    /// 轴网
    /// </summary>
    public sealed class GridModel : ElementModel
    {
        public GridModel() { }

        public GridModel(Element element)
        {
            if (element is not Grid grid)
            {
                throw new ArgumentNullException(nameof(element));
            }

            var curve = grid.Curve;

            UniqueId = grid.UniqueId;
            Name = grid.Name;
            ElementTypeName = GetElementTypeName(grid);
            EndPoint0 = curve.GetEndPoint(0).ToPointModel();
            Center = curve.Evaluate(0.5, true).ToPointModel();
            EndPoint1 = curve.GetEndPoint(1).ToPointModel();
            IsArc = grid.IsCurved;
        }
    }
}
