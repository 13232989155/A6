using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper
{
    /// <summary>
    /// 正则表达式操作类
    /// </summary>
    public class RegexHelper
    {
        #region 正则表达式

        /// <summary>
        /// 手机号码表达式
        /// </summary>
        public const string PATTERN_PHONE = @"^1([38][0-9]|4[579]|5[0-3,5-9]|6[6]|7[0135678]|9[89])\d{8}$";

        /// <summary>
        /// 电话号码表达式
        /// </summary>
        public const string PATTERN_TEL = @"^(\d{3,4}-)?\d{6,8}$";

        /// <summary>
        /// 邮箱表达式
        /// </summary>
        public const string PATTERN_EMAIL = @"^[a-zA-Z0-9_-]+@[a-zA-Z0-9_-]+(\.[a-zA-Z0-9_-]+)+$";

        /// <summary>
        /// 身份证表达式
        /// </summary>
        public const string PATTERN_IDCARD = @"(^\d{17}(?:\d|x)$)|(^\d{15}$)";

        /// <summary>
        /// 半角数字、符号、字母表达式
        /// </summary>
        public const string PATTERN_HALF_NUM = @"^[0-9]*$";

        /// <summary>
        /// 邮编表达式
        /// </summary>
        public const string PATTERN_POST_NUM = @"^[0-9]{6}$";
        //@"\d[6]$";@"[1-9]\d{5}(?!d)";

        /// <summary>
        /// 传真表达式
        /// </summary>
        public const string PATTERN_FAG = @"^(\d{3,4}-)?\d{7,8}$";

        #endregion

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="target">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsRegexMath(string target, string pattern)
        {
            return IsRegexMath(target, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="target">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件</param>
        public static bool IsRegexMath(string target, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(target, pattern, options);
        }
    }
}
