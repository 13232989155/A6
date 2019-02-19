using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    /// <summary>
    /// 登录返回数据
    /// </summary>
    public class LoginResult
    {
        /// <summary>
        /// 用户token
        /// </summary>
        public string token { set; get; }

        /// <summary>
        /// 用户信息
        /// </summary>
        public AdminEntity adminEntity { set; get; }
    }
}
