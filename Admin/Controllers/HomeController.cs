using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Admin.Base;
using System.IO;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.Http;

namespace Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            if (!IsLogin())
            {
                return RedirectToAction(controllerName: "Login", actionName: "Login");
            }

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// 隐私条款
        /// </summary>
        /// <returns></returns>
        public IActionResult Notice()
        {
            return View();
        }


        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files">文件数组*</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> UploadFile(IFormFileCollection files)
        {

            DataResult dataResult = new DataResult();
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
                                newFileName = Guid.NewGuid().ToString() + suffix;
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

                    dataResult.code = "200";
                    dataResult.data = fileUrl;
                    dataResult.errno = 0;
                }
                else
                {
                    dataResult.code = "201";
                    dataResult.msg = "文件为空";
                    dataResult.errno = 201;
                }

            }
            catch (Exception ex)
            {
                dataResult.code = "999";
                dataResult.msg = ex.Message;
                dataResult.errno = 999;
            }

            return dataResult;
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

    }
}
