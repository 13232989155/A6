using Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Admin.Models
{
    /// <summary>
    /// 工具
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// 把数组字符串转数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public List<string> GetStrList(string str)
        {

            List<string> lst = new List<string>();
            lst = JsonConvert.DeserializeObject<List<string>>(str);
            return lst;
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <returns></returns>
        public AdminEntity GetThisAdmin()
        {
            return null;
        }

    }
}
