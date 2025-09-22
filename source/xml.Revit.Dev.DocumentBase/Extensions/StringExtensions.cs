using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace xml.Revit.Dev.DocumentBase.Extensions
{
    /// <summary>
    /// String Extensions
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 启动资源或打开进程
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static Process Start(this string fileName)
        {
            if (Directory.Exists(fileName))
            {
                return Process.Start("explorer.exe", fileName);
            }
#if NETCOREAPP
            ProcessStartInfo psi = new() { FileName = fileName, UseShellExecute = true };
            return Process.Start(psi);
#else
            ProcessStartInfo psi = new() { FileName = fileName };
            return Process.Start(psi);
#endif
        }

        /// <summary>
        /// IsNullOrEmpty
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// IsNullOrWhiteSpace
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 开头
        /// </summary>
        /// <param name="s"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool BeginWithAny(this string s, params char[] chars)
        {
            return !s.IsNullOrEmpty() && chars != null && chars.Contains(s[0]);
        }

        /// <summary>
        /// 否包含指定字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="comparison">默认忽略大小写</param>
        /// <returns></returns>
        [Pure]
        public static bool Contains(
            this string source,
            string value,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase
        )
        {
            if (source is null)
                return false;
            if (value is null)
                return false;
            return source.IndexOf(value, comparison) >= 0;
        }

        /// <summary>
        /// 移除空白字符
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimEx(this string value)
        {
            return value == null ? string.Empty : value.Trim();
        }

        /// <summary>
        /// 获取其中数字字符
        /// 多个分组仅获取第一个
        /// Hello123World456 => 123
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetNumbers(this string value)
        {
            string result = "";
            foreach (char c in value)
            {
                if (char.IsDigit(c))
                {
                    result += c;
                }
                else if (!result.IsNullOrEmpty())
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 移除非法文件名字符
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string SafeInvalidFileNameChars(this string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        /// <summary>
        /// 字符串 转列表
        /// </summary>
        /// <param name="value"> str</param>
        /// <param name="sep"> 分割字符</param>
        /// <returns> str列表</returns>
        public static IList<string> ToSplit(this string value, params char[] sep)
        {
            return value.IsNullOrEmpty() ? [] : value.Split(sep).Select(o => o.TrimEx()).ToList();
        }

        /// <summary>
        /// 指定字符是否包含列表中的关键字
        /// </summary>
        /// <param name="value"> str</param>
        /// <param name="list"> str列表</param>
        /// <returns></returns>
        public static bool IsContains(this string value, List<string> list)
        {
            if (value.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var item in list)
            {
                var val = item.Replace(" ", "");
                if (value.Contains(val))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 字符数组重组
        /// </summary>
        /// <param name="items">字符数组</param>
        /// <param name="separator">分隔符</param>
        /// <returns>连接后的字符串</returns>
        public static string ToStrings<T>(this IEnumerable<T> items, string separator = ",")
        {
            if (items == null)
            {
                return string.Empty;
            }
            return string.Join(separator, items);
        }

        /// <summary>
        /// 组合获取完整文件或文件夹地址
        /// </summary>
        /// <param name="source"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string AppendPath(this string source, string path)
        {
            return Path.Combine(source, path);
        }

        /// <summary>
        /// 组合获取完整文件或文件夹地址
        /// </summary>
        /// <param name="source"></param>
        /// <param name="paths"></param>
        /// <returns></returns>
        public static string AppendPath(this string source, params string[] paths)
        {
            var strings = new string[paths.Length + 1];
            strings[0] = source;
            for (var i = 1; i < strings.Length; i++)
            {
                strings[i] = paths[i - 1];
            }
            return Path.Combine(strings);
        }

        /// <summary>
        /// 判断 文件或路径
        /// </summary>
        /// <param name="filepath">文件或路径 (r)</param>
        /// <returns>是否存在</returns>
        public static bool Exists(this string filepath)
        {
            return File.Exists(filepath) || Directory.Exists(filepath);
        }

        /// <summary>
        /// 获取指定路径下的文件
        /// 不会获取到子目录的文件
        /// </summary>
        /// <param name="filePath"> 路径</param>
        /// <param name="pattern"> 文件类型</param>
        /// <returns> 文件列表</returns>
        public static IEnumerable<string> EnumerateFiles(this string filePath, string pattern)
        {
            try
            {
                return System.IO.Directory.EnumerateFiles(filePath, pattern);
            }
            catch
            {
                return [];
            }
        }

        /// <summary>
        /// 判断字符串是否包含任意给定列表关键字
        /// </summary>
        /// <param name="key"> 字符串</param>
        /// <param name="keys"> 列表关键字</param>
        /// <returns> 是否包含关键字</returns>
        public static bool ContainsAny(this string key, params string[] keys)
        {
            bool result = false;
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var k in keys)
            {
                if (key.Contains(k))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 判断字符串是否包含全部给定列表关键字
        /// </summary>
        /// <param name="key"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static bool ContainsAll(this string key, params string[] keys)
        {
            var result = true;
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            foreach (var k in keys)
            {
                if (!key.Contains(k))
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// string to XYZ
        /// </summary>
        /// <param name="pointStr"></param>
        /// <returns></returns>
        public static XYZ ToXYZ(this string pointStr)
        {
            XYZ result = null;
            if (!string.IsNullOrEmpty(pointStr))
            {
                pointStr = pointStr.Contains("(") ? pointStr.Replace("(", "") : pointStr;
                pointStr = pointStr.Contains(")") ? pointStr.Replace(")", "") : pointStr;
                string[] array = pointStr.Split(',');
                result = new XYZ(
                    double.Parse(array[0]),
                    double.Parse(array[1]),
                    double.Parse(array[2])
                );
            }
            return result;
        }
    }
}
