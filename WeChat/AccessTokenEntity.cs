using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
    public class AccessTokenEntity
    {
        public string access_token { set; get; }
        public string openid { set; get; }
        public string unionid { set; get; }
        public string refresh_token { set; get; }
        public string scope { set; get; }
        public string expires_in { set; get; }
        public string errcode { set; get; }
        public string errmsg { set; get; }

    }
}
