using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace QiNiu
{
    public class DataEncrypt
    {
        #region 非对称加密

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr">需加密的字符串</param>
        /// <returns></returns>
        public static string DataMd5(string encypStr)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] inputBye = Encoding.ASCII.GetBytes(encypStr);
            byte[] outputBye = md5.ComputeHash(inputBye);

            string retStr = Convert.ToBase64String(outputBye);

            return retStr;
        }

        #endregion

        #region 对称加密

        /// <summary>
        /// 用于对称算法的密钥
        /// </summary>
        private static byte[] _arrDESKey = new byte[] { 42, 16, 93, 156, 78, 4, 218, 108 };

        /// <summary>
        /// 用于对称算法的初始化向量
        /// </summary>
        private static byte[] _arrDESIV = new byte[] { 55, 103, 246, 79, 36, 99, 167, 99 };

        /// <summary>
        /// 对数据进行编码
        /// </summary>
        /// <param name="encodeStr">需要加密的字符串</param>
        /// <param name="arrDESKey">用于对称算法的密钥</param>
        /// <param name="arrDESIV">用于对称算法的初始化向量</param>
        /// <returns></returns>
        public static string Encode(string encodeStr, byte[] arrDESKey = null, byte[] arrDESIV = null)
        {
            if (string.IsNullOrEmpty(encodeStr))
            {
                throw new Exception("Error: 源字符串为空！！");
            }

            // 如果没传的话，使用默认
            if (arrDESIV == null || arrDESKey == null)
            {
                arrDESIV = _arrDESIV;
                arrDESKey = _arrDESKey;
            }

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV), CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(encodeStr);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int)objMemoryStream.Length);
        }

        /// <summary>
        /// 对数据解码
        /// </summary>
        /// <param name="decodeStr">需要解码的字符串</param>
        /// <param name="arrDESKey">用于对称算法的密钥</param>
        /// <param name="arrDESIV">用于对称算法的初始化向量</param>
        /// <returns></returns>
        public static string Decode(string decodeStr, byte[] arrDESKey = null, byte[] arrDESIV = null)
        {
            if (decodeStr == null)
            {
                throw new Exception("Error: 源字符串为空！！");
            }

            // 如果没传的话，使用默认
            if (arrDESIV == null || arrDESKey == null)
            {
                arrDESIV = _arrDESIV;
                arrDESKey = _arrDESKey;
            }

            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(decodeStr);
            MemoryStream objMemoryStream = new MemoryStream(arrInput);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV), CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }

        #endregion
    }
}
