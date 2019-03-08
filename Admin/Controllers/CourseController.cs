using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Controllers
{
    public class CourseController : BaseController
    {
        private CourseBLL courseBLL = null;

        public CourseController()
        {
            courseBLL = new CourseBLL();
        }

        /// <summary>
        /// 列表页面
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult List(string searchString, int? page)
        {
            ViewBag.searchString = string.IsNullOrWhiteSpace(searchString) ? "" : searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 15;

            IPagedList<CourseEntity> courseEntities = courseBLL.AdminPageList(pageNumber, pageSize, searchString);
            return View(courseEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {

            CourseEntity courseEntity = courseBLL.GetById(id);

            return View(courseEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            List<CourseTypeEntity> courseTypeEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseTypeEntity>().Where(it => it.isDel == false).ToList();
            return View(courseTypeEntities);
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="courseEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("courseTypeId, name, describe, coverImage, coverVideo, teacherDescribe, videoUrl, tips, price")] CourseEntity courseEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "标题不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(courseEntity.videoUrl))
                {
                    dataResult.code = "201";
                    dataResult.msg = "视频不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(courseEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "封面图片不能为空";
                    return dataResult;
                }

                CourseEntity course = new CourseEntity()
                {
                    adminId = ThisAdmin().adminId,
                    courseTypeId = courseEntity.courseTypeId == 0 ? -1 : courseEntity.courseTypeId,
                    coverImage = courseEntity.coverImage,
                    coverVideo = courseEntity.coverVideo ?? "",
                    createDate = DateTime.Now,
                    describe = courseEntity.describe ?? "",
                    isDel = false,
                    modifyDate = DateTime.Now,
                    price = courseEntity.price == 0 ? -1 : courseEntity.price,
                    state = 1,
                    teacherDescribe = courseEntity.teacherDescribe ?? "",
                    tips = courseEntity.tips ?? "",
                    name = courseEntity.name,
                    videoUrl = courseEntity.videoUrl
                };

                int rows = courseBLL.ActionDal.ActionDBAccess.Insertable(course).ExecuteCommand();

                if (rows > 0)
                {
                    dataResult.code = "200";
                    dataResult.msg = "成功";
                }
                else
                {
                    dataResult.code = "201";
                    dataResult.msg = "失败";
                }

            }
            catch (Exception e)
            {
                dataResult.code = "999";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit( int id)
        {
            CourseEntity courseEntity = courseBLL.GetById(id);
            List<CourseTypeEntity> courseTypeEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseTypeEntity>().Where(it => it.isDel == false).ToList();
            ViewBag.courseTypeEntities = courseTypeEntities;
            return View(courseEntity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("courseTypeId, name, describe, coverImage, coverVideo, teacherDescribe, videoUrl, tips, price, isDel, courseId")] CourseEntity courseEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {
                if (string.IsNullOrWhiteSpace(courseEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "标题不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(courseEntity.videoUrl))
                {
                    dataResult.code = "201";
                    dataResult.msg = "视频不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(courseEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "封面图片不能为空";
                    return dataResult;
                }

                CourseEntity course = courseBLL.GetById(courseEntity.courseId);
                course.adminId = ThisAdmin().adminId;
                course.courseTypeId = courseEntity.courseTypeId == 0 ? -1 : courseEntity.courseTypeId;
                course.coverImage = courseEntity.coverImage ?? "";
                course.coverVideo = courseEntity.coverVideo ?? "";
                course.describe = courseEntity.describe ?? "";
                course.isDel = courseEntity.isDel;
                course.modifyDate = DateTime.Now;
                course.price = courseEntity.price == 0 ? -1 : courseEntity.price;
                course.teacherDescribe = courseEntity.teacherDescribe ?? "";
                course.tips = courseEntity.tips ?? "";
                course.name = courseEntity.name ?? "";
                course.videoUrl = courseEntity.videoUrl;

                int rows = courseBLL.ActionDal.ActionDBAccess.Updateable(course).ExecuteCommand();

                if (rows > 0)
                {
                    dataResult.code = "200";
                    dataResult.msg = "成功";
                }
                else
                {
                    dataResult.code = "201";
                    dataResult.msg = "失败";
                }

            }
            catch (Exception e)
            {
                dataResult.code = "202";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;

        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {

            CourseEntity course = courseBLL.GetById(id);
            course.isDel = true;
            course.modifyDate = DateTime.Now;

            int rows = courseBLL.ActionDal.ActionDBAccess.Updateable(course).ExecuteCommand();

            return RedirectToAction("List");
        }



        /// <summary>
        /// 视频章节列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult SectionList(int id)
        {
            return View(id);
        }


        /// <summary>
        /// 获取全部课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> ListCourse()
        {
            DataResult dataResult = new DataResult();

            try
            {
                List<CourseEntity> courseEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                                                    .Where(it => it.isDel == false)
                                                    .ToList();

                dataResult.data = courseEntities;
                dataResult.code = "200";
                dataResult.msg = "成功";
            }
            catch (Exception e)
            {
                dataResult.code = "999";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;

        }

    }
}