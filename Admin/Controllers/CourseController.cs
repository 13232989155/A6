using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using X.PagedList;
using static Admin.Models.EChartsResult;

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
            List<TeacherEntity> teacherEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<TeacherEntity>().Where(it => it.isDel == false).ToList();
            ViewBag.teacherEntities = teacherEntities;
            return View(courseTypeEntities);
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="courseEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("courseTypeId, name, describe, coverImage, coverVideo, teacherId, videoUrl, tips, price")] CourseEntity courseEntity)
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

                if ( courseEntity.teacherId < 10000)
                {
                    dataResult.code = "201";
                    dataResult.msg = "教师不能为空";
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
                    teacherId = courseEntity.teacherId,
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

            List<TeacherEntity> teacherEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<TeacherEntity>().Where(it => it.isDel == false).ToList();
            ViewBag.teacherEntities = teacherEntities;
            return View(courseEntity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("courseTypeId, name, describe, coverImage, coverVideo, teacherId, videoUrl, tips, price, isDel, courseId")] CourseEntity courseEntity)
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

                if (courseEntity.teacherId < 10000)
                {
                    dataResult.code = "201";
                    dataResult.msg = "教师不能为空";
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
                course.teacherId = courseEntity.teacherId;
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


        /// <summary>
        /// 每节汇总统计
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> CourseSectionStatistics()
        {
            DataResult dataResult = new DataResult();

            try
            {
                List<int> courseEntities = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                                                    .Select(it => it.courseId)
                                                    .ToList();

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                courseEntities.ForEach( it => 
                {
                    keyValuePairs.Add(it.ToString(), "0");
                });


                List<KeyValuePair<string, string>> keyValues = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseOrderEntity>()
                            .Where(it => it.state == 2 )
                            .GroupBy(it => it.courseId)
                            .Select<KeyValuePair<string, string>>("courseId, SUM(realTotal) as realTotal")
                            .ToList();

                keyValues.ForEach(it =>
               {
                   keyValuePairs[it.Key] = Math.Round(Convert.ToDouble(it.Value), 2).ToString();
               });

                LineResult lineResult = new LineResult();
                lineResult.xAxis = keyValuePairs.Keys.ToList();
                lineResult.yAxis = keyValuePairs.Values.ToList();
                dataResult.data = lineResult;

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

        /// <summary>
        /// 每天汇总统计
        /// </summary>
        /// <param name="staDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> CourseDayStatistics( DateTime staDate, DateTime endDate)
        {
            DataResult dataResult = new DataResult();

            try
            {
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                for ( var d = staDate; d <= endDate;)
                {
                    keyValuePairs.Add(d.ToString("MM/dd/yyyy"), "0");
                    d = d.AddDays(1);
                }

                List<KeyValuePair<string, string>> keyValues = courseBLL.ActionDal.ActionDBAccess.Queryable<CourseOrderEntity>()
                            .Where(it => it.state == 2 && SqlFunc.Between(it.payDate, staDate, endDate.AddDays(1)))
                            .GroupBy(it => it.payDate)
                            .Select<KeyValuePair<string, string>>("payDate, SUM(realTotal) as realTotal")
                            .ToList();
      
                keyValues.ForEach(it =>
               {
                   keyValuePairs[Convert.ToDateTime(it.Key).ToString("MM/dd/yyyy")] = Math.Round(Convert.ToDouble( it.Value), 2).ToString();

               });

                LineResult lineResult = new LineResult();
                lineResult.xAxis = keyValuePairs.Keys.ToList();
                lineResult.yAxis = keyValuePairs.Values.ToList();
                dataResult.data = lineResult;
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