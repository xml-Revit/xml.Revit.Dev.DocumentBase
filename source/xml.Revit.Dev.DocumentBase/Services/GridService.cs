/* 作    者: xml
** 创建时间: 2024/5/26 15:01:09
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

namespace xml.Revit.Dev.DocumentBase.Services
{
    public interface IGridService : IXmlElement<Grid, GridModel> { }

    internal class GridService : IGridService
    {
        public List<Grid> Create(IEnumerable<GridModel> models)
        {
            List<Grid> results = [];
            if (!models.Any())
            {
                return results;
            }

            var doc = XmlDoc.Doc;
            var grids = doc.OfClass<Grid>();
            var gridTypes = doc.OfClass<GridType>();
            foreach (var item in models)
            {
                var grid = grids.FirstOrDefault(o => o.Name.Equals(item.Name));
                if (grid == null)
                {
                    if (item.IsArc)
                    {
                        var arc = Arc.Create(
                            item.EndPoint0.ToXYZ(),
                            item.EndPoint1.ToXYZ(),
                            item.Center.ToXYZ()
                        );
                        grid = Grid.Create(doc, arc);
                    }
                    else
                    {
                        var line = Line.CreateBound(item.EndPoint0.ToXYZ(), item.EndPoint1.ToXYZ());
                        grid = Grid.Create(doc, line);
                    }
                    grid.Maximize3DExtents();
                    grid.Name = item.Name;
                }

                if (grid != null)
                {
                    var gridType = gridTypes.FirstOrDefault(o =>
                        o.Name.Equals(item.ElementTypeName)
                    );
                    if (gridType != null)
                    {
                        grid.ChangeTypeId(gridType.Id);
                    }

                    results.Add(grid);
                }
            }

            return results;
        }

        public List<GridModel> GetElementModels()
        {
            var doc = XmlDoc.Doc;
            var grids = doc.OfClass<Grid>();
            Debug.Write("Grid count:" + grids.Count);

            return grids.Select(o => new GridModel(o)).ToList();
        }
    }
}
