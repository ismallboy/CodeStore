using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine
{
    public class Log4NetLogger : ISystemLogger
    {
        private SystemLogLevel _LogLevel = SystemLogLevel.Debug;
        public SystemLogLevel LogLevel
        {
            get
            {
                return _LogLevel;
            }
            set
            {
                _LogLevel = value;
            }
        }

        private log4net.ILog _SysLogger;

        public Log4NetLogger()
        {
            _SysLogger = log4net.LogManager.GetLogger("SystemLog");
        }

        /// <summary>
        /// Error
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            _SysLogger.Error(message);
        }

        /// <summary>
        /// Warning
        /// </summary>
        /// <param name="message"></param>
        public void Warning(string message)
        {
            _SysLogger.Warn(message);
        }

        /// <summary>
        /// Info
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            _SysLogger.Info(message);
        }

        /// <summary>
        /// Debug
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            _SysLogger.Debug(message);
        }
    }
}
