using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Models
{
    /// <summary>
    /// 数据结果
    /// </summary>
    public class DataResult
    {
        /// <summary>
        /// 返回代码
        /// </summary>
        public string code
        {
            get;
            set;
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string msg
        {
            get;
            set;
        }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic data
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DataResult()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public DataResult(dynamic data)
        {
            this.code = "200";
            this.data = data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="code"></param>
        public DataResult(string msg, string code)
        {
            this.code = code;
            this.msg = msg;
        }
    }
}
