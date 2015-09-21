using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Amway.OA.ETOffine
{

    [Serializable]
    public class ReturnValue
    {
        private int result = 0;
        public int Result
        {
            get
            {
                return result;
            }
            set
            {
                result = value;
            }
        }

        private string message = string.Empty;
        /// <summary>
        /// 最长200字符
        /// </summary>
        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
            }
        }
        public ReturnValue()
        { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="rs">结果</param>
        /// <param name="msg">信息</param>
        public ReturnValue(int rs, string msg)
        {
            this.result = rs;
            this.message = msg;
        }
    }

}
