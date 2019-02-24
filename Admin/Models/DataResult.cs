using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
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

        public int errno { set; get; }

        ///// <summary>
        ///// 返回的数据类型
        ///// </summary>
        //public int dataType
        //{
        //    get;
        //    set;
        //}

        public DataResult()
        {

        }

        public DataResult(dynamic data)
        {
            this.code = "200";
            this.data = data;
        }

        public DataResult(string msg, string code)
        {
            this.code = code;
            this.msg = msg;
        }
    }
}