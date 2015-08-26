using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Amway.OA.MS.Utilities
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public class CommFunHelper
    {
        /// <summary>
        /// Table转换为List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> TableToList<T>(DataTable dt, Func<string,object,object> func = null) where T : new()
        {
            if (dt == null || dt.Rows.Count == 0)
            {
                return null;
            }
            // 定义集合    
            List<T> list = new List<T>();

            // 获得此模型的类型   
            Type type = typeof(T);
            string tempName = "";
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性      
                PropertyInfo[] propertys = t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;  // 检查DataTable是否包含此列    

                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter      
                        if (!pi.CanWrite) 
                        {
                            continue;
                        }
                        object value = dr[tempName];
                        if (value != DBNull.Value)
                        {
                            if (func != null)
                            {
                                value = func(tempName, value);
                            }
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                list.Add(t);
            }
            return list;
        }

        /// <summary>
        /// Table转换为Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T TableToModel<T>(DataTable dt) where T : new()
        {
            var list = TableToList<T>(dt);
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault();
            }
            return default(T);
        }
    }
}