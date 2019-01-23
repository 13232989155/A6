using System;
using System.Collections.Generic;
using System.Text;

namespace WeChat
{
    public class GetWXUsersHelper
    {
        /// <summary> 
        /// 获取链接返回数据 
        /// </summary> 
        /// <param name="Url">链接</param> 
        /// <param name="type">请求类型</param> 
        /// <returns></returns> 
        public string GetUrltoHtml(string Url, string type)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
                // Get the response instance. 
                System.Net.WebResponse wResp = wReq.GetResponse();
                System.IO.Stream respStream = wResp.GetResponseStream();
                // Dim reader As StreamReader = New StreamReader(respStream) 
                using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.GetEncoding(type)))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (System.Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
