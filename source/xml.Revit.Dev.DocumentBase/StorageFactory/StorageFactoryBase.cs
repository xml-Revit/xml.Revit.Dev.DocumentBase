using Autodesk.Revit.DB.ExtensibleStorage;

namespace xml.Revit.Dev.DocumentBase.StorageFactory
{
    /// <summary>
    /// StorageFactoryBase 拓展参数基类
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public abstract class StorageFactoryBase<TElement>
        where TElement : Element
    {
        #region Schema

        /// <summary>
        /// Guid
        /// </summary>
        public abstract Guid Guid { get; }

        /// <summary>
        /// Field Name
        /// </summary>
        public abstract string FieldName { get; }

        /// <summary>
        /// Schema Name
        /// </summary>
        public string SchemaName { get; } = "xml";

        /// <summary>
        /// Documentation
        /// </summary>
        public string Documentation { get; } = "广州明周科技有限公司";

        /// <summary>
        /// ReadAccessLevel
        /// </summary>
        public AccessLevel ReadAccessLevel { get; } = AccessLevel.Public;

        /// <summary>
        /// WriteAccessLevel
        /// </summary>
        public AccessLevel WriteAccessLevel { get; } = AccessLevel.Public;

        #endregion

        #region Save / Load / Reset
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="element"></param>
        /// <param name="data"></param>
        public void Save(TElement element, string data)
        {
            using var entity = GetSchemaEntity(element);
            entity.Set(FieldName, data);
            element.SetEntity(entity);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public string Load(TElement element)
        {
            using var entity = GetSchemaEntity(element);
            var storage = entity.Get<string>(FieldName);
            return storage;
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="element"></param>
        public void Reset(TElement element)
        {
            using var entity = GetSchemaEntity(element);
            element.DeleteEntity(entity.Schema);
        }

        #endregion

        #region Select

        /// <summary>
        /// 获取添加拓展参数的实例
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual IEnumerable<TElement> Select(Document document)
        {
            var filter = new ExtensibleStorageFilter(Guid);
            var elements = new FilteredElementCollector(document)
                .WhereElementIsNotElementType()
                .WherePasses(filter)
                .OfType<TElement>();
            return elements;
        }
        #endregion

        #region Private
        private Entity GetSchemaEntity(Element element)
        {
            var schema = Schema.Lookup(Guid) ?? CreateSchema(Guid);
            var entity = element.GetEntity(schema);
            if (entity.Schema == null)
            {
                entity = new Entity(schema);
            }

            return entity;
        }

        private Schema CreateSchema(Guid Guid)
        {
            var builder = new SchemaBuilder(Guid);
            builder.SetReadAccessLevel(ReadAccessLevel);
            builder.SetWriteAccessLevel(WriteAccessLevel);
            builder.SetSchemaName(SchemaName);
            builder.SetDocumentation(Documentation);
            builder.AddSimpleField(FieldName, typeof(string));
            return builder.Finish();
        }

        #endregion
    }
}
