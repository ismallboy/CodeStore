using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.MSGM
{
    public static class ObjectExtension
    {
        public static string ToNullString ( this object obj )
        {
            if ( obj == null )
            {
                return string.Empty;
            }

            return obj.ToString ();
        }
     

        #region Int 扩展
        /// <summary>
        /// 将obj转化成Int值，如果转化不成功则返回 int.MinValue
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>int</returns>
        public static int ToInt ( this object obj )
        {
            return obj.ToInt ( int.MinValue );
        }
        public static int ToInt ( this object obj, int defaultValue )
        {
            int i = defaultValue;
            if ( int.TryParse ( obj.ToNullString (), out i ) )
            {
                return i;
            } else
            {
                return defaultValue;
            }
        }
        #endregion

        #region DateTime 扩展
        /// <summary>
        /// 将字符串转化成DateTime值，如果转化不成功则返回 DateTime.MinValue
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime ( this object obj )
        {
            return obj.ToDateTime ( DateTime.MinValue );
        }
        public static DateTime ToDateTime ( this object obj, DateTime defaultValue )
        {
            DateTime i = defaultValue;
            if ( DateTime.TryParse ( obj.ToNullString (), out i ) )
            {
                return i;
            } else
            {
                return defaultValue;
            }
        }
       
        #endregion

        public static Decimal ToDecimal(this object obj)
        {
            Decimal i = 0.0m;
            if (Decimal.TryParse(obj.ToNullString(), out i))
            {
                return i;
            }
            else
            {
                return i;
            }
        }
    }

    public static class DecimalExtension
    {
        public static int ToInt ( this decimal obj )
        {
            return Convert.ToInt32 ( obj );
        }

        public static string ToIntString ( this decimal? obj )
        {
            if ( !obj.HasValue )
            {
                return string.Empty;
            }
            return obj.Value.ToInt ().ToString ();
        }
        public static string ToNumberString(this decimal? obj, string format)
        {
            if (!obj.HasValue)
            {
                return string.Empty;
            }
            return obj.Value.ToString(format);
        }
    }

    public static class DateTimeExtension
    {
        public static string ToMonthString(this DateTime? obj)
        {
            return obj.ToDateString("yyyy-MM");
        }
        public static string ToDateString ( this DateTime? obj )
        {
            return obj.ToDateString ( "yyyy-MM-dd" );
        }
        public static string ToDateString ( this DateTime? obj, string format )
        {
            if ( !obj.HasValue )
            {
                return string.Empty;
            }
            return obj.Value.ToString ( format );
        }
    }

    public static class BooleanExtension
    {
        public static string ToBooleanString ( this bool? obj )
        {
            if ( !obj.HasValue )
            {
                return string.Empty;
            }
            return obj.Value ? "Y" : "N";
        }
    }

    public static class StringExtension
    {
        #region Int 扩展

        public static int? ToNullInt ( this string obj )
        {
            if ( string.IsNullOrEmpty ( obj ) )
            {
                return null;
            }
            var convertValue = ToInt ( obj, int.MinValue );
            if ( convertValue == int.MinValue )
            {
                return null;
            }
            return convertValue;
        }

        public static string ToNullIntString ( this string obj )
        {
            if ( string.IsNullOrEmpty ( obj ) )
            {
                return null;
            }
            var convertValue = ToInt ( obj, int.MinValue );
            if ( convertValue == int.MinValue )
            {
                return null;
            }
            return convertValue.ToString ();
        }

        /// <summary>
        /// 将字符串转化成Int值，如果转化不成功则返回 int.MinValue
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>int</returns>
        public static int ToInt ( this string obj )
        {
            return obj.ToInt ( int.MinValue );
        }
        public static int ToInt ( this string obj, int defaultValue )
        {
            int i = defaultValue;
            if ( int.TryParse ( obj, out i ) )
            {
                return i;
            } else
            {
                return defaultValue;
            }
        }

        #endregion

        #region Decimal 扩展

        public static decimal? ToNullDecimal ( this string obj )
        {
            if ( string.IsNullOrEmpty ( obj ) )
            {
                return null;
            }
            var convertValue = ToDecimal ( obj, decimal.MinValue );
            if ( convertValue == decimal.MinValue )
            {
                return null;
            }
            return convertValue;
        }
        /// <summary>
        /// 将字符串转化成decimal值，如果转化不成功则返回 decimal.MinValue
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>decimal</returns>
        public static decimal ToDecimal ( this string obj )
        {
            return obj.ToDecimal ( decimal.MinValue );
        }
        public static decimal ToDecimal ( this string obj, decimal defaultValue )
        {
            decimal i = defaultValue;
            if ( decimal.TryParse ( obj, out i ) )
            {
                return i;
            } else
            {
                return defaultValue;
            }
        }

        #endregion

        #region DateTime 扩展

        public static DateTime? ToNullDateTime ( this string obj )
        {
            if ( string.IsNullOrEmpty ( obj ) )
            {
                return null;
            }
            var convertValue = ToDateTime ( obj, DateTime.MinValue );
            if ( convertValue == DateTime.MinValue )
            {
                return null;
            }
            return convertValue;
        }
        /// <summary>
        /// 将字符串转化成DateTime值，如果转化不成功则返回 DateTime.MinValue
        /// </summary>
        /// <param name="obj">字符串</param>
        /// <returns>DateTime</returns>
        public static DateTime ToDateTime ( this string obj )
        {
            return obj.ToDateTime ( DateTime.MinValue );
        }
        public static DateTime ToDateTime ( this string obj, DateTime defaultValue )
        {
            DateTime i = defaultValue;
            if ( DateTime.TryParse ( obj, out i ) )
            {
                return i;
            } else
            {
                return defaultValue;
            }
        }

        #endregion

        #region Boolean 扩展

        public static bool? ToNullBoolean ( this string obj )
        {
            if ( string.IsNullOrEmpty ( obj ) )
            {
                return null;
            }
            obj = obj.ToLower ();
            if ( obj == "y" || obj == "yes" || obj == "true" )
            {
                return true;

            } else if ( obj == "n" || obj == "no" || obj == "false" )
            {
                return false;

            } else
            {
                return null;
            }
        }

        #endregion

        #region Format 扩展

        public static string FormatWith ( this string obj, object arg0 )
        {
            return string.Format ( obj, arg0 );
        }
        public static string FormatWith ( this string obj, object arg0, object arg1 )
        {
            return string.Format ( obj, arg0, arg1 );
        }
        public static string FormatWith ( this string obj, object arg0, object arg1, object arg2 )
        {
            return string.Format ( obj, arg0, arg1, arg2 );
        }
        public static string FormatWith ( this string obj, params object [] args )
        {
            return string.Format ( obj, args );
        }
        #endregion
    }

    public static class DictionaryExtension
    {
        /// <summary>
        /// 添加或者更新字典值，每次添加时需判断字典是否有该key,如果已经存在就更新该value
        /// 需慎用，如果字典里面数量比较大时
        /// </summary>
        /// <param name="source"></param>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public static void TryAdd ( this Dictionary<string, object> source, string key, object value )
        {
            if ( !source.ContainsKey ( key ) )
            {
                source.Add ( key, value );
            } else
            {
                source [key] = value;
            }
        }
    }
}
