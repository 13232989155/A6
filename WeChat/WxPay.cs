using BLL;
using Entity;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace WeChat
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WxPay
    {

        /// <summary>
        /// 课程支付
        /// </summary>
        /// <param name="courseOrder"></param>
        /// <returns></returns>
        public static PayResult PayCourse(CourseOrderEntity courseOrder)
        {
            PayResult payResult = new PayResult();
            try
            {
                //获取请求数据
                Dictionary<string, string> strParam = new Dictionary<string, string>();
                //小程序ID
                strParam.Add("appid", LoginHelper.AppID);
                //附加数据
                strParam.Add("attach", PayEntity.attach);
                //商品描述
                strParam.Add("body", PayEntity.body);
                //商户号
                strParam.Add("mch_id", PayEntity.MchId);
                //随机字符串
                strParam.Add("nonce_str", PayEntity.nonceStr);
                //通知地址 (异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。)
                strParam.Add("notify_url", PayEntity.notifyUrl);
                //商户订单号
                string outTradeNo = courseOrder.orderNo;
                strParam.Add("out_trade_no", outTradeNo);
                //终端IP
                strParam.Add("spbill_create_ip", PayEntity.ip);
                //标价金额
                string totalFee = Convert.ToInt32(courseOrder.orderTotal * 100).ToString();
                strParam.Add("total_fee", totalFee);
                //交易类型
                strParam.Add("trade_type", PayEntity.tradeType);
                strParam.Add("sign", PayHelper.GetSignInfo(strParam));
                //获取预支付ID
                string preInfo = sendPost(PayHelper.CreateXmlParam(strParam));
                string strCode = PayHelper.GetXmlValue(preInfo, "return_code");
                string strMsg = PayHelper.GetXmlValue(preInfo, "return_msg");

                if (strCode == "SUCCESS")
                {
                    //再次签名
                    string nonecStr = PayEntity.nonceStr;
                    string timeStamp = PayHelper.getTime().ToString();
                    Dictionary<string, string> singInfo = new Dictionary<string, string>();
                    singInfo.Add("appId", LoginHelper.AppID);
                    singInfo.Add("nonceStr", nonecStr);
                    singInfo.Add("package", PayEntity.packageStr);
                    singInfo.Add("signType", "MD5");
                    singInfo.Add("timeStamp", timeStamp);
                    //返回参数

                    payResult.timestamp = timeStamp;
                    payResult.noncestr = PayEntity.nonceStr;
                    payResult.package = PayEntity.packageStr;
                    payResult.sign = PayHelper.GetSignInfo(singInfo);
                    payResult.partnerid = PayEntity.MchId;
                    payResult.prepayid = PayHelper.GetXmlValue(preInfo, "prepay_id");
                    payResult.appid = LoginHelper.AppID;
                    payResult.code = "200";
                    return payResult;
                }
                else
                {
                    payResult.msg = "获取预支付ID失败";
                    payResult.code = "201";
                    return payResult;
                }

            }
            catch (Exception ex)
            {
                payResult.msg = ex.Message;
                payResult.code = "999";
                return payResult;
            }
        }

        /// <summary>
        /// wx统一下单请求数据
        /// </summary>
        /// <param name="URL">请求地址</param>
        /// <param name="urlArgs">参数</param>
        /// <returns></returns>
        private static string sendPost( string urlArgs)
        {

            System.Net.WebClient wCient = new System.Net.WebClient();
            wCient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //byte[] postData = System.Text.Encoding.ASCII.GetBytes(urlArgs);  如果微信签名中有中文会签名失败
            byte[] postData = System.Text.Encoding.UTF8.GetBytes(urlArgs);
            byte[] responseData = wCient.UploadData(PayEntity.url, "POST", postData);

            string returnStr = System.Text.Encoding.UTF8.GetString(responseData);//返回接受的数据 
            return returnStr;
        }

        /// <summary>
        /// 处理结果通知
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static string CourseNotify(string strXml)
        {
            string strResult = string.Empty;

            //判断是否请求成功
            if (PayHelper.GetXmlValue(strXml, "return_code") == "SUCCESS")
            {
                //判断是否支付成功
                if (PayHelper.GetXmlValue(strXml, "result_code") == "SUCCESS")
                {
                    //获得签名
                    string getSign = PayHelper.GetXmlValue(strXml, "sign");

                    //进行签名
                    string sign = PayHelper.GetSignInfo(PayHelper.GetFromXml(strXml));
                    if (sign == getSign)
                    {
                        //校验订单信息
                        string payNO = PayHelper.GetXmlValue(strXml, "transaction_id"); //微信订单号
                        string orderNO = PayHelper.GetXmlValue(strXml, "out_trade_no");    //商户订单号
                        string orderTotal = PayHelper.GetXmlValue(strXml, "total_fee");   //订单金额
                        string payDate = PayHelper.GetXmlValue(strXml, "time_end");   //支付完成时间	

                        CourseOrderBLL courseOrderBLL = new CourseOrderBLL();
                        CourseOrderEntity courseOrderEntity = courseOrderBLL.GetByOrderNO(orderNO);
                        //校验订单是否存在
                        if (courseOrderEntity.orderTotal * 100 == Convert.ToDecimal(orderTotal))
                        {
                            //2.更新订单的相关状态
                            if (courseOrderEntity.state == 1)
                            {
                                courseOrderEntity.payNo = payNO;
                                courseOrderEntity.payDate = DateTime.ParseExact(payDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                courseOrderEntity.state = 2;
                                courseOrderEntity.modifyDate = DateTime.Now;
                                courseOrderEntity.realTotal = Convert.ToDecimal(orderTotal) / 100;
                                courseOrderEntity.payChannel = "微信支付-APP";
                                int rows = courseOrderBLL.ActionDal.ActionDBAccess.Updateable(courseOrderEntity).ExecuteCommand();

                                //3.返回一个xml格式的结果给微信服务器
                                if (rows > 0)
                                {
                                    strResult = PayHelper.GetReturnXml("SUCCESS", "OK");
                                }
                                else
                                {
                                    strResult = PayHelper.GetReturnXml("FAIL", "订单状态更新失败");
                                }
                            }
                            else
                            {
                                strResult = PayHelper.GetReturnXml("SUCCESS", "OK");
                            }
                        }
                        else
                        {
                            strResult = PayHelper.GetReturnXml("FAIL", "支付结果中微信订单号数据库不存在！");
                        }
                    }
                    else
                    {
                        strResult = PayHelper.GetReturnXml("FAIL", "签名不一致!");
                    }
                }
                else
                {
                    strResult = PayHelper.GetReturnXml("FAIL", "支付通知失败!");
                }
            }
            else
            {
                strResult = PayHelper.GetReturnXml("FAIL", "支付通知失败!");
            }

            return strResult;
        }
    }
}
