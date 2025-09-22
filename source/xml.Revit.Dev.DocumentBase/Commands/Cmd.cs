/* 作    者: xml
** 创建时间: 2024/6/15 10:13:08
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

using xml.Revit.Dev.DocumentBase.Services;

namespace xml.Revit.Dev.DocumentBase.Commands
{
    /// <summary>
    /// 测试
    /// </summary>
    internal sealed class Cmd : XmlExternalCommand
    {
        protected override void Execute(ref string message, ElementSet elements)
        {
            IViewPlanService viewPlanService = Host.GetService<IViewPlanService>();
            //var viewPlanModels = viewPlanService.GetElementModels();
        }
    }
}
