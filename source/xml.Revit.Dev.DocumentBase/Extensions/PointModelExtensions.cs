/* 作    者: xml
** 创建时间: 2024/5/26 16:57:59
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using xml.Revit.Dev.DocumentBase.Models;

namespace xml.Revit.Dev.DocumentBase.Extensions
{
    /// <summary>
    /// PointModel Extensions
    /// </summary>
    public static class PointModelExtensions
    {
        /// <summary>
        /// XYZ 转换为 PointModel
        /// </summary>
        /// <param name="point"></param>
        /// <param name="s">单位转换</param>
        /// <returns></returns>
        public static PointModel ToPointModel(this XYZ point, double s = 1)
        {
            return new PointModel
            {
                X = point.X * s,
                Y = point.Y * s,
                Z = point.Z * s,
            };
        }

        /// <summary>
        /// PointModel 转换为 XYZ 自动将
        /// </summary>
        /// <param name="pointModel"></param>
        /// <param name="s">单位转换</param>
        /// <returns></returns>
        public static XYZ ToXYZ(this PointModel pointModel, double s = 1)
        {
            return new XYZ(pointModel.X / s, pointModel.Y / s, pointModel.Z / s);
        }
    }
}
