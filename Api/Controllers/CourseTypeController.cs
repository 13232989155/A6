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
    public class CourseTypeController : BaseController
    {
        /// <summary>
        /// 获取课程分类列表
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
                CourseTypeBLL courseTypeBLL = new CourseTypeBLL();
                List<CourseTypeEntity> courseTypeEntities = courseTypeBLL.ActionDal.ActionDBAccess.Queryable<CourseTypeEntity>().Where(it => it.isDel == false).ToList();

                dr.code = "200";
                dr.data = courseTypeEntities;
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