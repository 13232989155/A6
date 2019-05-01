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
    public class CourseRecommendController : BaseController
    {

        /// <summary>
        /// 获取课程推荐列表
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
                CourseRecommendBLL courseRecommendBLL = new CourseRecommendBLL();
                CourseBLL courseBLL = new CourseBLL();

                List<CourseRecommendEntity> courseRecommendEntities = courseRecommendBLL.ActionDal.ActionDBAccess.Queryable<CourseRecommendEntity>().ToList();

                courseRecommendEntities.ForEach(it =>
               {
                   List<int> vs = courseRecommendBLL.ActionDal.ActionDBAccess.Queryable<CourseRecommendCorrelationEntity>()
                                .Where(itt => itt.courseRecommendId == it.courseRecommendId)
                                .Select(itt => itt.courseId)
                                .ToList();
                   if ( vs.Count > 0)
                   {
                       it.courseEntities = courseBLL.ListByIdInts(vs);
                   }

               });

                dr.code = "200";
                dr.data = courseRecommendEntities;
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