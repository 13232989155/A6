using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using Api.Base;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using BLL;

namespace Api.Controllers
{
    /// <summary>
    /// 帮助
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelperController : BaseController
    {

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">文件数组*</param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult UploadFile(IFormFileCollection files)
        {

            DataResult dr = new DataResult();
            try
            {
                if (files != null && files.Count > 0)
                {
                    string[] fileUrl = new string[files.Count];

                    for (int i = 0; i < files.Count; i++)
                    {
                        var file = files[i];

                        if (file.Length > 0)
                        {
                            Stream stream = file.OpenReadStream();
                            byte[] bytes = new byte[stream.Length];
                            stream.Read(bytes, 0, bytes.Length);
                            stream.Seek(0, SeekOrigin.Begin);

                            string suffix = string.Empty;

                            suffix = GetImageFormat(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            //新文件名
                            string newFileName = string.Empty;

                            if (!string.IsNullOrWhiteSpace(suffix))
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                                newFileName = Guid.NewGuid().ToString() + suffix ;
                                stream.Seek(0, SeekOrigin.Begin);
                                fileUrl[i] = Helper.QiNiuHelper.UploadData(newFileName, bytes) + "?" + img.Width.ToString() + "x" + img.Height.ToString(); 
                            }
                            else
                            {

                                string fileName = file.FileName;/*获取文件名*/
                                suffix = fileName.Substring(fileName.LastIndexOf(".") + 1);/*获取后缀名*/
                                newFileName = Guid.NewGuid().ToString() + "." + suffix;
                                fileUrl[i] = Helper.QiNiuHelper.UploadData(newFileName, bytes);
                            }

                           
                        }

                    }

                    dr.code = "200";
                    dr.data = fileUrl;
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "文件为空";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }


        /// <summary>
        /// 获取Image图片格式
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string GetImageFormat(Stream file)
        {
            string format = string.Empty;

            byte[] sb = new byte[2];  //这次读取的就是直接0-1的位置长度了.
            file.Read(sb, 0, sb.Length);
            //根据文件头判断
            string strFlag = sb[0].ToString() + sb[1].ToString();
            //察看格式类型
            switch (strFlag)
            {
                //JPG格式
                case "255216":
                    format = ".jpg";
                    return format;
                //GIF格式
                case "7173":
                    format = ".gif";
                    return format;
                //BMP格式
                case "6677":
                    format = ".bmp";
                    return format;
                //PNG格式
                case "13780":
                    format = ".png";
                    return format;
                //其他格式
                default:
                    return format;
            }
        }

        /// <summary>
        /// 发送验证码
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult SendCode([FromForm] string phone)
        {
            DataResult dr = new DataResult();
            try
            {
                if ( string.IsNullOrWhiteSpace(phone) || phone.Length != 11 || Regex.IsMatch(phone, Helper.RegexHelper.PATTERN_PHONE))
                {
                    dr.code = "201";
                    dr.msg = "手机号码错误";
                    return Json(dr);
                }

                Random rd = new Random();
                string code = rd.Next(100000, 999999).ToString();

                bool rs = Aliyun.MessageHeiper.SendCode(phone, code);

                if (!rs)
                {
                    dr.code = "201";
                    dr.msg = "验证码发送失败";
                    return Json(dr);
                }

                PhoneCodeBLL phoneCodeBLL = new PhoneCodeBLL();

                int rows = phoneCodeBLL.Delete(phone);

                if (phoneCodeBLL.Create(phone, code) > 0)
                {
                    dr.code = "200";
                    dr.msg = "验证码发送成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "验证码发送成功，但保存失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }

    }
}