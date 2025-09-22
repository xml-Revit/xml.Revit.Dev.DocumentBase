/* 作    者: xml
** 创建时间: 2024/6/3 17:39:20
**
** Copyright 2024 by zedmoster
** Permission to use, copy, modify, and distribute this software in
** object code form for any purpose and without fee is hereby granted,
** provided that the above copyright notice appears in all copies and
** that both that copyright notice and the limited warranty and
** restricted rights notice below appear in all supporting
** documentation.
*/

namespace xml.Revit.Dev.DocumentBase.StorageFactory
{
    /// <summary>
    /// 缓存ID
    /// </summary>
    internal sealed class StorageFactoryElement : StorageFactoryBase<Element>
    {
        public override Guid Guid => new("6EEF2492-3436-4126-9566-1C099E29D385");

        public override string FieldName => "Id";
    }
}
