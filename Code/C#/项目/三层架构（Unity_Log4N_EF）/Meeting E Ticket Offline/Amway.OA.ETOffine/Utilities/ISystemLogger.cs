using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine
{
    /// <summary>
    /// 系统日志接口
    /// </summary>
    public interface ISystemLogger
    {
        /// <summary>
        /// 系统日志级别
        /// </summary>
        SystemLogLevel LogLevel { get; set; }

        /// <summary>
        /// 记录Error级别的系统日志
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        void Error(string message);

        /// <summary>
        /// 记录Warning级别的系统日志
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        void Warning(string message);

        /// <summary>
        /// 记录Info级别的系统日志
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        void Info(string message);

        /// <summary>
        /// 记录Debug级别的系统日志
        /// </summary>
        /// <param name="message">需要记录的信息</param>
        void Debug(string message);

    }
}
