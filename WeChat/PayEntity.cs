using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
    public class PayEntity
    {
        /// <summary>
        /// 商户号
        /// </summary>
        public static string MchId = "1528304651";
        /// <summary>
        /// api秘钥
        /// </summary>
        public static string Key = "qazwsxplokm123456IJNUHBWERTLOD8G";
        //附加数据
        public static string attach = "理赔学堂";
        //商品描述
        public static string body = "理赔学堂-购买课程";
        //随机字符串
        public static string nonceStr = PayHelper.GetRandomString(32);
        //通知地址 (异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。)
        public static string notifyUrl = "https://jy.geekann.com/api/WxChat/CoursePayNotify";
        //终端IP
        public static string ip = PayHelper.GetIp();
        //交易类型
        public static string tradeType = "APP";
        //微信统一下单请求地址
        public static string url = "https://api.mch.weixin.qq.com/pay/unifiedorder";

        public static string packageStr = "Sign=WXPay";

    }

    public class PayResult
    {
        public string appid { set; get; }

        public string partnerid { set; get; }

        public string prepayid { set; get; }

        public string package { set; get; }

        public string noncestr { set; get; }

        public string timestamp { set; get; }

        public string sign { set; get; }

        public string msg { set; get; }

        public string code { set; get; }
    }
}
