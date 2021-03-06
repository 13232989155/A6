﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
    public class WxUserEntity
    {
        public string openid { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string headimgurl { get; set; }
        public string unionId { get; set; }
        public dynamic privilege { get; set; }
        public string errcode { get; set; }
    }
}
