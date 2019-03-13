using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    /// <summary>
    /// 数据转换辅助类
    /// </summary>
    public class ConvertHelper
    {
        #region 基本数据转换

        /// <summary>
        /// 默认日期
        /// </summary>
        public static DateTime DEFAULT_DATE = DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);

        /// <summary>
        /// 转换成整形
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果为空或者非数字字符串的话，则返回0</returns>
        public static int ToInt(object obj)
        {
            int result = 0;

            if (obj == null)
            {
                return result;
            }

            if (int.TryParse(obj.ToString(), out result))
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 转换成字符串
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回sting.Empty</returns>
        public static string ToActionString(object obj)
        {
            return null == obj ? string.Empty : obj.ToString();
        }

        /// <summary>
        /// 转换成双浮点
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0.0000</returns>
        public static double ToDouble(object obj)
        {
            double result = 0.0000;

            if (null == obj)
            {
                return result;
            }

            if (double.TryParse(obj.ToString(), out result))
            {
                return result;
            }


            return result;
        }

        /// <summary>
        /// 将对象转换为数值(DateTime)类型,转换失败返回1900-01-01
        /// </summary>
        /// <param name="obj">转换的对象</param>
        /// <param name="format">日期格式</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj, string format)
        {
            try
            {
                DateTime dt = DateTime.ParseExact(ToActionString(obj), format, System.Globalization.CultureInfo.CurrentCulture);
                if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                    return dt;
                return DEFAULT_DATE;
            }
            catch
            {
                return DEFAULT_DATE;
            }
        }

        /// <summary>
        /// 转成日期格式
        /// </summary>
        /// <param name="obj">日期字符串</param>
        /// <returns></returns>
        public static DateTime ToDateTime(object obj)
        {
            string str = ToActionString(obj);
            if (null == obj || string.IsNullOrEmpty(str))
            {
                return DEFAULT_DATE;
            }
            else
            {
                if (str.Contains("-"))
                {
                    DateTime dt = DateTime.Parse(str);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return dt;
                }
                else
                {
                    DateTime dt = DateTime.ParseExact(str, "yyyy-MM-dd", System.Globalization.CultureInfo.CurrentCulture);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return dt;
                }
            }

            return DEFAULT_DATE;
        }

        /// <summary>
        /// 转成单浮点数
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0.0f</returns>
        public static float ToFloat(object obj)
        {
            float result = 0.0f;

            if (null == obj)
            {
                return result;
            }

            if (float.TryParse(obj.ToString(), out result))
            {
                return result;
            }


            return result;
        }

        /// <summary>
        /// 转成小数
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果：如果转换的数据为空，则返回0</returns>
        public static decimal ToDecimal(object obj)
        {
            decimal result = 0M;

            if (null == obj)
            {
                return result;
            }

            if (decimal.TryParse(obj.ToString(), out result))
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// 转成整型long
        /// </summary>
        /// <param name="obj">转换的变量</param>
        /// <returns>转换结果；如果转换的数据为空，则返回0</returns>
        public static long ToLong(object obj)
        {
            long result = 0;
            if (null == obj)
            {
                return result;
            }

            if (long.TryParse(obj.ToString(), out result))
            {
                return result;
            }

            return result;
        }
        #endregion

        #region 将byte[]转换成int

        ///// <summary>
        ///// 将byte[]转换成int
        ///// </summary>
        ///// <param name="data">需要转换成整数的byte数组</param>
        //public static int BytesToInt32(byte[] data)
        //{
        //    //如果传入的字节数组长度小于4,则返回0
        //    if (data.Length < 4)
        //    {
        //        return 0;
        //    }

        //    //定义要返回的整数
        //    int num = 0;

        //    //如果传入的字节数组长度大于4,需要进行处理
        //    if (data.Length >= 4)
        //    {
        //        //创建一个临时缓冲区
        //        byte[] tempBuffer = new byte[4];

        //        //将传入的字节数组的前4个字节复制到临时缓冲区
        //        Buffer.BlockCopy(data, 0, tempBuffer, 0, 4);

        //        //将临时缓冲区的值转换成整数，并赋给num
        //        num = BitConverter.ToInt32(tempBuffer, 0);
        //    }

        //    //返回整数
        //    return num;
        //}

        #endregion

        #region 补足位数

        /// <summary>
        /// 往左边补足位数
        /// </summary>
        /// <param name="obj">补足的变量</param>
        /// <param name="totalLenght">总共长度</param>
        /// <param name="padSymbol">补充的符号</param>
        /// <returns></returns>
        public static string PadLeftSide(object obj, int totalLenght, char padSymbol)
        {
            string result = string.Empty;

            if (null == obj)
            {
                result = result.PadLeft(totalLenght, padSymbol);
            }
            else
            {
                result = obj.ToString();
                result = result.PadLeft(totalLenght, padSymbol);
            }

            return result;
        }

        /// <summary>
        /// 往右边补足位数
        /// </summary>
        /// <param name="obj">补足的变量</param>
        /// <param name="totalLenght">总共长度</param>
        /// <param name="padSymbol">补充的符号</param>
        /// <returns></returns>
        public static string PadRightSide(object obj, int totalLenght, char padSymbol)
        {
            string result = string.Empty;

            if (null == obj)
            {
                result = result.PadRight(totalLenght, padSymbol);
            }
            else
            {
                result = obj.ToString();
                result = result.PadRight(totalLenght, padSymbol);
            }

            return result;
        }

        #endregion

        #region 数据判断

        /// <summary>
		/// 判断对象是否为正确的日期值
		/// </summary>
		/// <param name="obj">目标数据</param>
		/// <returns>Boolean。</returns>
		public static bool IsDateTime(object obj)
        {
            DateTime DEFAULT_DATE = Convert.ToDateTime("1900-01-01");

            try
            {
                string str = ToActionString(obj);
                if (string.IsNullOrEmpty(str))
                {
                    return false;
                }

                if (str.Contains("-"))
                {
                    DateTime dt = DateTime.Parse(str);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return true;
                }
                else
                {
                    DateTime dt = DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                    if (dt > DEFAULT_DATE && DateTime.MaxValue > dt)
                        return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
