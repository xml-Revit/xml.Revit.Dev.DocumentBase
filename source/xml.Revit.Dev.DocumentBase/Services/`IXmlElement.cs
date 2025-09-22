using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using xml.Revit.Dev.DocumentBase.Models;

namespace xml.Revit.Dev.DocumentBase.Services
{
    /// <summary>
    /// 提取数据接口
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public interface IXmlElement<TElement, TModel>
        where TElement : Element
        where TModel : ElementModel
    {
        /// <summary>
        /// 读取 Element 模型数据
        /// </summary>
        /// <returns></returns>
        List<TModel> GetElementModels();

        /// <summary>
        /// 通过 Element 模型创建实例
        /// <para>需要启动Autodesk.Revit.DB.Transaction</para>
        /// </summary>
        /// <param name="models">模型数据</param>
        /// <returns></returns>
        List<TElement> Create(IEnumerable<TModel> models);
    }
}
