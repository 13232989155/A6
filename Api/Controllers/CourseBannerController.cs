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

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseBannerController : BaseController
    {

        /// <summary>
        /// 获取课程Banner列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult List([FromForm] string token)
        {

            DataResult dr = new DataResult();
            try
            {
                CourseBannerBLL courseBannerBLL = new CourseBannerBLL();
                List<CourseBannerEntity> courseBannerEntities = courseBannerBLL.ActionDal.ActionDBAccess.Queryable<CourseBannerEntity>().ToList();

                dr.code = "200";
                dr.data = courseBannerEntities;
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