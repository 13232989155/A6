using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeChat;

namespace Api.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CoursePayResult
    {
        /// <summary>
        /// 
        /// </summary>
        public CourseOrderEntity courseOrderEntity { set; get; }

        /// <summary>
        /// 
        /// </summary>
        public string payResult { set; get; }
    }
}
