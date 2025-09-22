/* 作    者: xml
** 创建时间: 2024/5/26 16:40:17
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

namespace xml.Revit.Dev.DocumentBase.Models
{
    /// <summary>
    /// 标高
    /// </summary>
    public sealed class LevelModel : ElementModel
    {
        public LevelModel() { }

        /// <summary>
        /// 读取项目标高构建标高模型
        /// </summary>
        /// <param name="element"></param>
        public LevelModel(Element element)
        {
            if (element is not Level level)
            {
                throw new ArgumentNullException(nameof(element));
            }

            UniqueId = level.UniqueId;
            Name = level.Name;
            Elevation = level.Elevation;
            ElementTypeName = GetElementTypeName(level);
            ALevel = level.get_Parameter(BuiltInParameter.LEVEL_IS_BUILDING_STORY).AsInteger();
            SLevel = level.get_Parameter(BuiltInParameter.LEVEL_IS_STRUCTURAL).AsInteger();
        }

        /// <summary>
        /// 高程
        /// </summary>
        public double Elevation { get; set; }

        /// <summary>
        /// 建筑楼层
        /// </summary>
        public int ALevel { get; set; }

        /// <summary>
        /// 结构楼层
        /// </summary>
        public int SLevel { get; set; }
    }
}
