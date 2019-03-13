using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace WeChat
{
    /// <summary>
    /// 
    /// </summary>
    public class PayHelper
    {

        /// <summary>
        /// 生成随机串    
        /// </summary>
        /// <param name="length">字符串长度</param>
        /// <returns></returns>
        public static string GetRandomString(int length)
        {
            const string key = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            if (length < 1)
                return string.Empty;

            Random rnd = new Random();
            byte[] buffer = new byte[8];

            ulong bit = 31;
            ulong result = 0;
            int index = 0;
            StringBuilder sb = new StringBuilder((length / 5 + 1) * 5);

            while (sb.Length < length)
            {
                rnd.NextBytes(buffer);

                buffer[5] = buffer[6] = buffer[7] = 0x00;
                result = BitConverter.ToUInt64(buffer, 0);

                while (result > 0 && sb.Length < length)
                {
                    index = (int)(bit & result);
                    sb.Append(key[index]);
                    result = result >> 5;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long getTime()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = (long)ts.TotalSeconds;
            return t;
        }

        /// <summary>
        /// 获取签名数据
        ///</summary>
        /// <param name="strParam"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetSignInfo(Dictionary<string, string> strParam)
        {
            int i = 0;
            string sign = string.Empty;
            StringBuilder sb = new StringBuilder();
            try
            {
                foreach (KeyValuePair<string, string> temp in strParam)
                {
                    if (temp.Value == "" || temp.Value == null || temp.Key.ToLower() == "sign")
                    {
                        continue;
                    }
                    i++;
                    sb.Append(temp.Key.Trim() + "=" + temp.Value.Trim() + "&");
                }
                sb.Append("key=" + PayEntity.Key.Trim() + "");
                sign = MD5(sb.ToString()).ToUpper();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return sign;
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string ipAddress = null;
            try
            {
                string hostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
                for (int i = 0; i < ipEntry.AddressList.Length; i++)
                {
                    if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ipEntry.AddressList[i].ToString();
                    }
                }
                return ipAddress;
            }
            catch (Exception ex)
            {
                return "获取IP出错:" + ex.Message;
            }
        }

        /// <summary>
        /// MD5签名方法  
        /// </summary>  
        /// <param name="inputText">加密参数</param>  
        /// <returns></returns>  
        private static string MD5(string inputText)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = System.Text.Encoding.UTF8.GetBytes(inputText);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }

        /// <summary>
        /// HMAC-SHA256签名方式
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        private static string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// 获取XML值
        /// </summary>
        /// <param name="strXml">XML字符串</param>
        /// <param name="strData">字段值</param>
        /// <returns></returns>
        public static string GetXmlValue(string strXml, string strData)
        {
            string xmlValue = string.Empty;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(strXml);
            var selectSingleNode = xmlDocument.DocumentElement.SelectSingleNode(strData);
            if (selectSingleNode != null)
            {
                xmlValue = selectSingleNode.InnerText;
            }
            return xmlValue;
        }

        /// <summary>
        /// 返回通知 XML
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="returnMsg"></param>
        /// <returns></returns>
        public static string GetReturnXml(string returnCode, string returnMsg)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<xml>");
            sb.Append("<return_code><![CDATA[" + returnCode + "]]></return_code>");
            sb.Append("<return_msg><![CDATA[" + returnMsg + "]]></return_msg>");
            sb.Append("</xml>");
            return sb.ToString();
        }

        /// <summary>
        /// 集合转换XML数据 (拼接成XML请求数据)
        /// </summary>
        /// <param name="strParam">参数</param>
        /// <returns></returns>
        public static string CreateXmlParam(Dictionary<string, string> strParam)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append("<xml>");
                foreach (KeyValuePair<string, string> k in strParam)
                {
                    if (k.Key == "attach" || k.Key == "body" || k.Key == "sign")
                    {
                        sb.Append("<" + k.Key + "><![CDATA[" + k.Value + "]]></" + k.Key + ">");
                    }
                    else
                    {
                        sb.Append("<" + k.Key + ">" + k.Value + "</" + k.Key + ">");
                    }
                }
                sb.Append("</xml>");
            }
            catch
            {
                
            }

            return sb.ToString();
        }

        /// <summary>
        /// XML数据转换集合（XML数据拼接成字符串)
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFromXml(string xmlString)
        {
            Dictionary<string, string> sParams = new Dictionary<string, string>();
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlString);
                XmlElement root = doc.DocumentElement;
                int len = root.ChildNodes.Count;
                for (int i = 0; i < len; i++)
                {
                    string name = root.ChildNodes[i].Name;
                    if (!sParams.ContainsKey(name))
                    {
                        sParams.Add(name.Trim(), root.ChildNodes[i].InnerText.Trim());
                    }
                }
            }
            catch
            {
                
            }
            return sParams;
        }

    }
}
