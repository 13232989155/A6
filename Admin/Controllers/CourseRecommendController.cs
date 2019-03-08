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
    public class CourseRecommendController : BaseController
    {
        private CourseRecommendBLL courseRecommendBLL = null;

        public CourseRecommendController()
        {
            courseRecommendBLL = new CourseRecommendBLL();
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

            IPagedList<CourseRecommendEntity> courseRecommendEntities = courseRecommendBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(courseRecommendEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CourseRecommendEntity courseRecommendEntity = courseRecommendBLL.GetById(id);
            courseRecommendEntity.courseRecommendCorrelationEntities = courseRecommendBLL.ActionDal.ActionDBAccess.Queryable<CourseRecommendCorrelationEntity>()
                                                                            .Where(it => it.courseRecommendId == courseRecommendEntity.courseRecommendId).ToList();
            return View(courseRecommendEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="courseRecommendEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("name")] CourseRecommendEntity courseRecommendEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseRecommendEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                CourseRecommendEntity courseRecommend = new CourseRecommendEntity()
                {
                    adminId = ThisAdmin().adminId,
                    createDate = DateTime.Now,
                    modifyDate = DateTime.Now,
                    name = courseRecommendEntity.name
                };

                int rows = courseRecommendBLL.ActionDal.ActionDBAccess.Insertable(courseRecommend).ExecuteCommand();

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
        public IActionResult Edit(int id)
        {
            CourseRecommendEntity courseRecommendEntity = courseRecommendBLL.GetById(id);
            return View(courseRecommendEntity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseRecommendEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("name, courseRecommendId")] CourseRecommendEntity courseRecommendEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseRecommendEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                CourseRecommendEntity courseRecommend = courseRecommendBLL.GetById(courseRecommendEntity.courseRecommendId);

                courseRecommend.name = courseRecommendEntity.name;
                courseRecommend.modifyDate = DateTime.Now;
                courseRecommend.adminId = ThisAdmin().adminId;

                int rows = courseRecommendBLL.ActionDal.ActionDBAccess.Updateable(courseRecommend).ExecuteCommand();

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
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Delete(int id)
        {

            CourseRecommendEntity courseRecommendEntity = courseRecommendBLL.GetById(id);

            int rows = courseRecommendBLL.ActionDal.ActionDBAccess.Deleteable(courseRecommendEntity).ExecuteCommand();

            return RedirectToAction("List");
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult CourseList(int id)
        {
            CourseRecommendEntity courseRecommendEntity = courseRecommendBLL.GetById(id);

            return View(courseRecommendEntity);
        }

        /// <summary>
        /// 保存关联课程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> CourseList( int courseRecommendId, List<int> courseIdInts)
        {

            DataResult dataResult = new DataResult();

            try
            {

                int rows = 0;
                CourseRecommendCorrelationBLL courseRecommendCorrelationBLL = new CourseRecommendCorrelationBLL();

                List<CourseRecommendCorrelationEntity> courseRecommendCorrelationEntities = courseRecommendCorrelationBLL.ListByCourseRecommendId(courseRecommendId);

                if (courseRecommendCorrelationEntities.Count > 0 || courseIdInts.Count > 0)
                {

                    if (courseRecommendCorrelationEntities.Count > 0)
                    {

                        rows = courseRecommendCorrelationBLL.ActionDal.ActionDBAccess.Deleteable(courseRecommendCorrelationEntities).ExecuteCommand();
                    }
                    if (courseIdInts != null)
                    {

                        List<CourseRecommendCorrelationEntity> courseRecommendCorrelations = new List<CourseRecommendCorrelationEntity>();
                        courseIdInts.ForEach(it =>
                        {
                            courseRecommendCorrelations.Add(new CourseRecommendCorrelationEntity()
                            {
                                courseId = it,
                                courseRecommendId = courseRecommendId,
                                createDate = DateTime.Now
                            });
                        });
                        rows = courseRecommendCorrelationBLL.ActionDal.ActionDBAccess.Insertable(courseRecommendCorrelations).ExecuteCommand();
                    }

                }

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
        /// 根据id获取关联课程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> ListById(int id)
        {
            DataResult dataResult = new DataResult();

            try
            {
                CourseRecommendCorrelationBLL courseRecommendCorrelationBLL = new CourseRecommendCorrelationBLL();

                List<CourseRecommendCorrelationEntity> courseRecommendCorrelationEntities = courseRecommendCorrelationBLL.ListByCourseRecommendId( id); 

                dataResult.data = courseRecommendCorrelationEntities;
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