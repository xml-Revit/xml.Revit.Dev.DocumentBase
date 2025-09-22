using System.IO;
using xml.Revit.Dev.DocumentBase.Extensions;
using xml.Revit.Dev.DocumentBase.Helper;
using xml.Revit.Dev.DocumentBase.Services;

namespace xml.Revit.Dev.DocumentBase.Commands
{
    /// <summary>
    /// 导出数据
    /// </summary>
    [XmlHelpUrl("https://mp.weixin.qq.com/s/Y9ZiOBsHAfOB-JavjT_IgQ")]
    [Xml("导出数据", Application._tooltip, Application._description)]
    [Transaction(TransactionMode.Manual)]
    public sealed class CmdExport : XmlExternalCommand
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
            var doc = uidoc.Document;
            if (doc.PathName.IsNullOrEmpty())
            {
                XmlDoc.Print("请先保存项目到本地");
                return;
            }

            ILevelService levelService = Host.GetService<ILevelService>();
            var levelModels = levelService.GetElementModels();

            IGridService gridService = Host.GetService<IGridService>();
            var gridModels = gridService.GetElementModels();

            IViewPlanService viewPlanService = Host.GetService<IViewPlanService>();
            var viewPlanModels = viewPlanService.GetElementModels();

            IWallService wallService = Host.GetService<IWallService>();
            var wallModels = wallService.GetElementModels();

            var db = doc.GetDBFileName();
            if (File.Exists(db))
            {
                if (
                    XmlDoc.Print("是否覆盖已存在的数据?", System.Windows.MessageBoxButton.OKCancel)
                    != System.Windows.MessageBoxResult.OK
                )
                {
                    return;
                }
                LiteDBHelper.Delete(db);
            }
            if (levelModels.Count > 0)
            {
                LiteDBHelper.Upsert(db, levelModels);
                XmlDoc.ShowBalloon("导出标高总计:" + levelModels.Count, "导出数据");
                LiteDBHelper.Upsert(db, gridModels);
                XmlDoc.ShowBalloon("导出轴网总计:" + gridModels.Count, "导出数据");
                LiteDBHelper.Upsert(db, viewPlanModels);
                XmlDoc.ShowBalloon("导出楼层平面总计:" + viewPlanModels.Count, "导出数据");

                LiteDBHelper.Upsert(db, wallModels);
                XmlDoc.ShowBalloon("导出墙总计:" + wallModels.Count, "导出数据");
            }

            XmlDoc.ShowBalloon("导出完成:\n" + db, "DB");
        }
    }
}
