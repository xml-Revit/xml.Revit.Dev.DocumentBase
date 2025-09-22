using System.Linq.Expressions;
using LiteDB;
using xml.Revit.Dev.DocumentBase.Models;

namespace xml.Revit.Dev.DocumentBase.Helper
{
    /// <summary>
    /// LiteDB Helper
    /// </summary>
    public static class LiteDBHelper
    {
        /// <summary>
        /// 密码
        /// </summary>
        public const string PASSWORD = "微信公众号:Revit二次教程";

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="levelFileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static string GetConnectionString(string levelFileName, string password)
        {
            var connectionString = $"Filename={levelFileName};Mode=Exclusive";
            if (!string.IsNullOrEmpty(password))
            {
                connectionString += $";Password={password}";
            }
            return connectionString;
        }

        /// <summary>
        /// 保存数据,如果存在覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelFileName"></param>
        /// <param name="value"></param>
        /// <param name="password"></param>
        public static bool Upsert<T>(string levelFileName, T value, string password = null)
            where T : ElementModel
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.Upsert(value);
            }
        }

        /// <summary>
        /// 保存数据,如果存在覆盖
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelFileName"></param>
        /// <param name="values"></param>
        /// <param name="password"></param>
        public static bool Upsert<T>(
            string levelFileName,
            IEnumerable<T> values,
            string password = null
        )
            where T : ElementModel
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                foreach (var item in values)
                {
                    collection.Upsert(item);
                }
                return true;
            }
        }

        /// <summary>
        /// 读取全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelFileName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static List<T> Query<T>(string levelFileName, string password = null)
            where T : ElementModel
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.FindAll().ToList();
            }
        }

        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelFileName"></param>
        /// <param name="predicate">查询条件</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static List<T> Query<T>(
            string levelFileName,
            Expression<Func<T, bool>> predicate,
            string password = null
        )
            where T : ElementModel
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.Find(predicate).ToList();
            }
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="levelFileName"></param>
        /// <param name="predicate">删除条件</param>
        /// <param name="password"></param>
        public static int Delete<T>(
            string levelFileName,
            Expression<Func<T, bool>> predicate,
            string password = null
        )
            where T : ElementModel
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                var collection = db.GetCollection<T>(typeof(T).Name);
                return collection.DeleteMany(predicate);
            }
        }

        /// <summary>
        /// 删除所有数据表
        /// </summary>
        /// <param name="levelFileName"></param>
        /// <param name="password"></param>
        public static bool Delete(string levelFileName, string password = null)
        {
            var connectionString = GetConnectionString(levelFileName, password);
            using var db = new LiteDatabase(connectionString);
            {
                foreach (var name in db.GetCollectionNames())
                {
                    db.DropCollection(name);
                }
                return true;
            }
        }
    }
}
