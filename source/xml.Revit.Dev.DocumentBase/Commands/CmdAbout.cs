/* 作    者: xml
** 创建时间: 2024/6/2 13:42:14
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

namespace xml.Revit.Dev.DocumentBase.Commands
{
    /// <summary>
    /// 关于
    /// </summary>
    [XmlHelpUrl("https://mp.weixin.qq.com/s/Y9ZiOBsHAfOB-JavjT_IgQ")]
    [Xml("关于", Application._tooltip, Application._description)]
    [Transaction(TransactionMode.Manual)]
    public sealed class CmdAbout : IExternalCommand
    {
        public Result Execute(
            ExternalCommandData commandData,
            ref string message,
            ElementSet elements
        )
        {
            TaskDialog mainDialog =
                new("关于") { MainInstruction = "关于使用", MainContent = "搜索微信小程序:Revit二次开发教程" };
            mainDialog.CommonButtons = TaskDialogCommonButtons.Ok;
            mainDialog.DefaultButton = TaskDialogResult.Ok;
            mainDialog.Show();

            return Result.Succeeded;
        }
    }
}
