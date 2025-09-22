/* 作    者: xml
** 创建时间: 2024/5/26 8:50:50
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using System.Diagnostics;
using xml.Revit.Dev.DocumentBase.Models;

namespace xml.Revit.Dev.DocumentBase.Services
{
    public interface ILevelService : IXmlElement<Level, LevelModel> { }

    internal class LevelService : ILevelService
    {
        public List<LevelModel> GetElementModels()
        {
            var doc = XmlDoc.UIdoc.Document;
            var levels = doc.OfClass<Level>();
            Debug.Write("Level count:" + levels.Count);

            return levels.Select(level => new LevelModel(level)).ToList();
        }

        public List<Level> Create(IEnumerable<LevelModel> models)
        {
            List<Level> results = [];
            if (!models.Any())
            {
                return results;
            }

            var doc = XmlDoc.Doc;
            var levels = doc.OfClass<Level>();
            var levelTypes = doc.OfClass<LevelType>();
            foreach (var item in models)
            {
                var level = levels.FirstOrDefault(o => o.Name.Equals(item.Name));
                if (level == null)
                {
                    level = Level.Create(doc, item.Elevation);
                    level.Name = item.Name;
                }
                else
                {
                    level.Elevation = item.Elevation;
                }
                if (level != null)
                {
                    var levelType = levelTypes.FirstOrDefault(o =>
                        o.Name.Equals(item.ElementTypeName)
                    );
                    if (levelType != null)
                    {
                        level.ChangeTypeId(levelType.Id);
                    }
                    level.get_Parameter(BuiltInParameter.LEVEL_IS_BUILDING_STORY).Set(item.ALevel);
                    level.get_Parameter(BuiltInParameter.LEVEL_IS_STRUCTURAL).Set(item.SLevel);

                    results.Add(level);
                }
            }

            return results;
        }
    }
}
