using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine
{
    /// <summary>
    /// 系统日志级别
    /// </summary>
    public enum SystemLogLevel : int
    {
        /// <summary>
        /// 停止记录级别
        /// </summary>
        None = -1,
        
        /// <summary>
        /// 错误级别
        /// </summary>
        Error = 0,

        /// <summary>
        /// 警告级别
        /// </summary>
        Warning = 1,

        /// <summary>
        /// 信息级别
        /// </summary>
        Info = 4,

        /// <summary>
        /// 调试级别
        /// </summary>
        Debug = 3,
    }
}
