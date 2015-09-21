using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine
{
    /// <summary>
    /// 统一的日志
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug ( string msg )
        {
            ISystemLogger logger = ServiceLocator.GetService<ISystemLogger>();

            logger.Debug(msg);
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug ( string msg, object arg0 )
        {
            Debug ( string.Format ( msg, arg0 ) );
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug ( string msg, object arg0, object arg1 )
        {
            Debug ( string.Format ( msg, arg0, arg1 ) );
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug ( string msg, object arg0, object arg1, object arg2 )
        {
            Debug ( string.Format ( msg, arg0, arg1, arg2 ) );
        }

        /// <summary>
        /// Debug日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Debug ( string msg, params object [] args )
        {
            Debug ( string.Format ( msg, args ) );
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg )
        {
            ISystemLogger logger = ServiceLocator.GetService<ISystemLogger>();
            logger.Error(msg);
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg, object arg0 )
        {
            Error ( string.Format ( msg, arg0 ) );
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg, object arg0, object arg1 )
        {
            Error ( string.Format ( msg, arg0, arg1 ) );
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg, object arg0, object arg1, object arg2 )
        {
            Error ( string.Format ( msg, arg0, arg1, arg2 ) );
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg, object arg0, object arg1, object arg2, object arg3 )
        {
            Error ( string.Format ( msg, arg0, arg1, arg2, arg3 ) );
        }

        /// <summary>
        /// Error日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Error ( string msg, params object [] args )
        {
            Error ( string.Format ( msg, args ) );
        }

        /// <summary>
        /// Warning日志
        /// </summary>
        /// <param name="msg"></param>
        public static void Warning ( string msg )
        {
            ISystemLogger logger = ServiceLocator.GetService<ISystemLogger>();
            logger.Warning(msg);
        }
    }
}
