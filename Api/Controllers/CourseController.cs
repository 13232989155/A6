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
    public class CourseController : BaseController
    {

        /// <summary>
        /// 获取课程列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="courseTypeId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult PageList([FromForm] string token, [FromForm] int courseTypeId = -1, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {

            DataResult dr = new DataResult();
            try
            {
                CourseBLL courseBLL = new CourseBLL();

                int totalItemCount = courseBLL.Count(courseTypeId);
                List<CourseEntity> courseEntities = courseBLL.List(courseTypeId, pageNumber: pageNumber, pageSize: pageSize, totalCount: totalItemCount);

                PageData pageData = new PageData(courseEntities, pageNumber, pageSize, totalItemCount);

                dr.code = "200";
                dr.data = pageData;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 根据课程ID获取介绍
        /// </summary>
        /// <param name="courseId">*</param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult GetById([FromForm] int courseId)
        {
            DataResult dr = new DataResult();
            try
            {
                CourseBLL courseBLL = new CourseBLL();
                CourseEntity courseEntity = courseBLL.GetById(courseId);
                courseEntity.videoUrl = "";

                dr.code = "200";
                dr.data = courseEntity;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }


        /// <summary>
        /// 根据课程获取详细内容
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="courseId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetDetailById([FromForm] string token, [FromForm] int courseId)
        {
            DataResult dr = new DataResult();
            try
            {
                UserEntity userEntity = this.GetUserByToken(token);
                //CourseOrderBLL courseOrderBLL = new CourseOrderBLL();
                //CourseOrderEntity courseOrderEntity = courseOrderBLL.GetByCourseAndUserId(courseId, userEntity.userId);

                //if (courseOrderEntity == null)
                //{
                //    dr.code = "201";
                //    dr.msg = "未购买该课程";
                //    return Json(dr);
                //}

                CourseBLL courseBLL = new CourseBLL();
                CourseEntity courseEntity = courseBLL.GetById(courseId);

                CourseSectionBLL courseSectionBLL = new CourseSectionBLL();
                courseEntity.courseSectionEntities = courseSectionBLL.ListByCourseId(courseId);

                dr.code = "200";
                dr.data = courseEntity;
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