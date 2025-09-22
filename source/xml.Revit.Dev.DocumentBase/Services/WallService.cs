/* 作    者: xml
** 创建时间: 2024/5/31 21:16:01
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
using xml.Revit.Dev.DocumentBase.Extensions;
using xml.Revit.Dev.DocumentBase.Models;
using xml.Revit.Dev.DocumentBase.StorageFactory;

namespace xml.Revit.Dev.DocumentBase.Services
{
    public interface IWallService : IXmlElement<Wall, WallModel> { }

    internal class WallService : IWallService
    {
        public List<Wall> Create(IEnumerable<WallModel> models)
        {
            var doc = XmlDoc.Doc;

            var wallTypes = doc.OfClass<WallType>();
            var levels = doc.OfClass<Level>();
            // 不包含幕墙
            var walls = doc.OfClass<Wall>()
                .Where(o => o.WallType.Kind != WallKind.Curtain)
                .ToList();

            List<Wall> result = [];

            var storageFactory = Host.GetService<StorageFactoryElement>();

            foreach (var model in models)
            {
                // 避免重复创建
                if (walls.Any(o => model.UniqueId.Equals(storageFactory.Load(o))))
                {
                    continue;
                }

                var curve = default(Curve);
                if (model.IsArc)
                {
                    curve = Arc.Create(
                        model.EndPoint0.ToXYZ(),
                        model.EndPoint1.ToXYZ(),
                        model.Center.ToXYZ()
                    );
                }
                else
                {
                    curve = Line.CreateBound(model.EndPoint0.ToXYZ(), model.EndPoint1.ToXYZ());
                }

                var walltype =
                    wallTypes.FirstOrDefault(o => o.Name.Equals(model.ElementTypeName))
                    ?? wallTypes.FirstOrDefault();
                var baseLevel =
                    levels.FirstOrDefault(o => o.Name.Equals(model.BaseLevelName))
                    ?? levels.FirstOrDefault();

                if (model.TopLevelName.IsNullOrEmpty())
                {
                    // 墙顶部未设置约束
                    var wall = Wall.Create(
                        doc,
                        curve,
                        walltype.Id,
                        baseLevel.Id,
                        model.Height,
                        model.BaseOffset,
                        false,
                        model.Structural == 1
                    );

                    storageFactory.Save(wall, model.UniqueId);

                    result.Add(wall);
                }
                else
                {
                    var topLevel =
                        levels.FirstOrDefault(o => o.Name.Equals(model.TopLevelName))
                        ?? levels.FirstOrDefault(o => o.Elevation > baseLevel.Elevation);

                    // 设置顶标高
                    var wall = Wall.Create(doc, curve, baseLevel.Id, model.Structural == 1);
                    wall.WallType = walltype;

                    storageFactory.Save(wall, model.UniqueId);

                    doc.SubTransaction(ts =>
                    {
                        // 设置顶部约束后顶部偏移可用
                        wall.get_Parameter(BuiltInParameter.WALL_BASE_CONSTRAINT)
                            .Set(baseLevel.Id);
                        wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(topLevel.Id);
                    });

                    wall.get_Parameter(BuiltInParameter.WALL_BASE_OFFSET).Set(model.BaseOffset);
                    wall.get_Parameter(BuiltInParameter.WALL_TOP_OFFSET).Set(model.TopOffset);

                    result.Add(wall);
                }
            }

            return result;
        }

        public List<WallModel> GetElementModels()
        {
            var doc = XmlDoc.Doc;
            var walls = doc.OfClass<Wall>();
            Debug.Write("Wall count:" + walls.Count);

            return walls.Select(o => new WallModel(o)).ToList();
        }
    }
}
