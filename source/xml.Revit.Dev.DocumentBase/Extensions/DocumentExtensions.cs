/* 作    者: xml
** 创建时间: 2024/5/26 9:27:54
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

namespace xml.Revit.Dev.DocumentBase.Extensions
{
    /// <summary>
    /// Docment 拓展方法
    /// </summary>
    public static class DocumentExtensions
    {
        /// <summary>
        /// 获取 DB 文件地址
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static string GetDBFileName(this Document doc)
        {
            if (doc is null)
            {
                throw new ArgumentNullException(nameof(doc));
            }

            var path = System.IO.Path.GetDirectoryName(doc.PathName);
            return System.IO.Path.Combine(path, doc.Title + ".db");
        }
    }
}
