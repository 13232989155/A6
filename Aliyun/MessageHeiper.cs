﻿using System;
using System.Collections.Generic;
using System.Text;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
namespace Aliyun
{


    /// <summary>
    /// 阿里云发送短信服务
    /// </summary>
    public static class MessageHeiper
    {
        /// <summary>
        /// accessKeyId
        /// </summary>
        private static string accessKeyId = "LTAIuNjwltjI0Zpe";

        /// <summary>
        /// 你的accessKeySecret
        /// </summary>
        private static string accessKeySecret = "CtirSiII3qAxHHBWBMOW0PiQ8D8wfL";

        /// <summary>
        /// 短信签名
        /// </summary>
        private static string SignName = "极安科技";

        /// <summary>
        /// 发送短信验证码
        /// </summary>
        /// <param name="PhoneNumbers"></param>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static bool SendCode( string PhoneNumbers, string Code)
        {
            bool res = false;

            string product = "Dysmsapi";//短信API产品名称（短信产品名固定，无需修改）
            string domain = "dysmsapi.aliyuncs.com";//短信API产品域名（接口地址固定，无需修改）
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", accessKeyId, accessKeySecret);
            //IAcsClient client = new DefaultAcsClient(profile);
            // SingleSendSmsRequest request = new SingleSendSmsRequest();
            //初始化ascClient,暂时不支持多region（请勿修改）
            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", product, domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为1000个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式，发送国际/港澳台消息时，接收号码格式为00+国际区号+号码，如“0085200000000”
                request.PhoneNumbers = PhoneNumbers;
                //必填:短信签名-可在短信控制台中找到
                request.SignName = SignName;
                //必填:短信模板-可在短信控制台中找到，发送国际/港澳台消息时，请使用国际/港澳台短信模版
                request.TemplateCode = "SMS_156471344";
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = "{\"code\":\"" + Code + "\"}";
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                //request.OutId = "yourOutId";
                //请求失败这里会抛ClientException异常
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);

                if ( sendSmsResponse.Message == "OK")
                {
                    res = true;
                }

            }
            catch
            {
                
            }

            return res;
        }




    }
}
