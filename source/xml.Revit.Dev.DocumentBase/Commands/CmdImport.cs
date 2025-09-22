/* 作    者: xml
** 创建时间: 2024/5/26 9:21:34
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
using Microsoft.Win32;
using xml.Revit.Dev.DocumentBase.Helper;
using xml.Revit.Dev.DocumentBase.Models;
using xml.Revit.Dev.DocumentBase.Services;

namespace xml.Revit.Dev.DocumentBase.Commands
{
    /// <summary>
    /// 导入数据
    /// </summary>
    [XmlHelpUrl("https://mp.weixin.qq.com/s/Y9ZiOBsHAfOB-JavjT_IgQ")]
    [Xml("导入数据", Application._tooltip, Application._description)]
    [Transaction(TransactionMode.Manual)]
    public sealed class CmdImport : XmlExternalCommand
    {
        public override bool SetCommandAvailability(
            UIApplication applicationData,
            CategorySet selectedCategories
        )
        {
            return applicationData.IsOpenDocument();
        }

        protected override void Execute(ref string message, ElementSet elements)
        {
            OpenFileDialog dialog =
                new()
                {
                    Multiselect = false,
                    Title = "请选择导出数据",
                    Filter = "数据文件(*.db)|*.db"
                };
            if (dialog.ShowDialog().Value == false)
            {
                Debug.Write("已取消");
                return;
            }

            var db = dialog.FileName;

            var levelModels = LiteDBHelper.Query<LevelModel>(db);
            var gridModels = LiteDBHelper.Query<GridModel>(db);
            var viewPlanModels = LiteDBHelper.Query<ViewPlanModel>(db);

#if DEBUG
            var wallModels = LiteDBHelper.Query<WallModel>(db);
#endif

            var doc = uidoc.Document;
            doc.TransactionGroup(
                tg =>
                {
                    ILevelService levelService = Host.GetService<ILevelService>();
                    doc.Transaction(t => levelService.Create(levelModels));
                    XmlDoc.ShowBalloon("导入标高总计:" + levelModels.Count, "导入数据");
                    IGridService gridService = Host.GetService<IGridService>();
                    doc.Transaction(t => gridService.Create(gridModels));
                    XmlDoc.ShowBalloon("导入轴网总计:" + gridModels.Count, "导入数据");
                    IViewPlanService viewPlanService = Host.GetService<IViewPlanService>();
                    doc.Transaction(t => viewPlanService.Create(viewPlanModels));
                    XmlDoc.ShowBalloon("导入楼层平面总计:" + viewPlanModels.Count, "导入数据");
#if DEBUG
                    IWallService wallService = Host.GetService<IWallService>();
                    doc.Transaction(t => wallService.Create(wallModels));
                    XmlDoc.ShowBalloon("导入墙总计:" + wallModels.Count, "导入数据");
#endif
                },
                "导入数据"
            );
        }
    }
}
