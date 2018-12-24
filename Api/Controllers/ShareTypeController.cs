using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareTypeController : BaseController
    {

        /// <summary>
        /// 获取说说类型列表
        /// </summary>
        /// <param name="token">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List([FromForm] string token)
        {

            DataResult dr = new DataResult();
            try
            {

                ShareTypeBLL shareTypeBLL = new ShareTypeBLL();
                IEnumerable<ShareTypeEntity> shareTypeEntities = shareTypeBLL.List();

                dr.code = "200";
                dr.data = shareTypeEntities;
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