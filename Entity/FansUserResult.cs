using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 粉丝或关注用户返回值
    /// </summary>
    public partial class FansUserResult
    {
        public FansUserResult()
        {

        }

        public int userId { set; get; }
        public string name { set; get; }
        public int gender { set; get; }
        public DateTime birthday { set; get; }
        public string portrait { set; get; }
        public string signature { set; get; }
        public bool attention { set; get; }



    }
}
