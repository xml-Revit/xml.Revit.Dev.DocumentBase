/* 作    者: xml
** 创建时间: 2024/2/16 20:26:06
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

namespace xml.Revit.Dev.DocumentBase
{
    /// <summary>
    /// Application
    /// </summary>
    public sealed class Application : XmlExternalApplication
    {
        public override string PanelName => "微信公众号:Revit二次开发教程";

        public const string _tooltip = "搜索微信小程序Revit二次开发教程";
        public const string _description = "隐藏福利交流群QQ群:17075104";

        public override void OnStartup()
        {
            var panel = UIapp.CreateRibbonPanel(PanelName);
            panel.AddItem(typeof(Commands.CmdAbout).GetPushButtonData());
            panel.AddSeparator();
            panel.AddItem(typeof(Commands.CmdExport).GetPushButtonData());
            panel.AddItem(typeof(Commands.CmdImport).GetPushButtonData());
        }
    }
}
