using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="files">文件数组</param>
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

                            string fileName = file.FileName;/*获取文件名*/
                            string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1);/*获取后缀名*/

                            string newFileName = Guid.NewGuid().ToString() + "." + suffix;

                            fileUrl[i] = Helper.QiNiuHelper.UploadData(newFileName, bytes);
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
    }
}