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
    public class CourseBannerController : BaseController
    {
        private CourseBannerBLL courseBannerBLL = null;

        public CourseBannerController()
        {
            courseBannerBLL = new CourseBannerBLL();
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

            IPagedList<CourseBannerEntity> courseBannerEntities = courseBannerBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(courseBannerEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CourseBannerEntity courseBannerEntity = courseBannerBLL.GetById(id);

            return View(courseBannerEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            List<CourseEntity> courseEntities = courseBannerBLL.ActionDal.ActionDBAccess.Queryable<CourseEntity>().Where(it => it.isDel == false).ToList();

            return View(courseEntities);
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="courseBannerEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("courseId, coverImage, title")] CourseBannerEntity courseBannerEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseBannerEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "图片不能为空";
                    return dataResult;
                }

                if (courseBannerEntity.courseId < 10000)
                {
                    dataResult.code = "201";
                    dataResult.msg = "课程不能为空";
                    return dataResult;
                }

                CourseBannerEntity courseBanner = new CourseBannerEntity()
                {
                    adminId = ThisAdmin().adminId,
                    courseId = courseBannerEntity.courseId,
                    coverImage = courseBannerEntity.coverImage,
                    createDate = DateTime.Now,
                    modifyDate = DateTime.Now,
                    title = courseBannerEntity.title ?? ""
                };

                int rows = courseBannerBLL.ActionDal.ActionDBAccess.Insertable(courseBanner).ExecuteCommand();

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
            CourseBannerEntity courseBannerEntity = courseBannerBLL.GetById(id);
            List<CourseEntity> courseEntities = courseBannerBLL.ActionDal.ActionDBAccess.Queryable<CourseEntity>().Where(it => it.isDel == false).ToList();
            ViewBag.courseEntities = courseEntities;
            return View(courseBannerEntity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseTypeEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("courseId, coverImage, title, courseBannerId")] CourseBannerEntity courseBannerEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseBannerEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "图片不能为空";
                    return dataResult;
                }

                if (courseBannerEntity.courseId < 10000)
                {
                    dataResult.code = "201";
                    dataResult.msg = "课程不能为空";
                    return dataResult;
                }

                CourseBannerEntity courseBanner = courseBannerBLL.GetById(courseBannerEntity.courseBannerId);
                courseBanner.adminId = ThisAdmin().adminId;
                courseBanner.courseId = courseBannerEntity.courseId;
                courseBanner.coverImage = courseBannerEntity.coverImage;
                courseBanner.modifyDate = DateTime.Now;
                courseBanner.title = courseBannerEntity.title ?? "";

                int rows = courseBannerBLL.ActionDal.ActionDBAccess.Updateable(courseBanner).ExecuteCommand();

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
            CourseBannerEntity courseBanner = courseBannerBLL.GetById(id);
            int rows = courseBannerBLL.ActionDal.ActionDBAccess.Deleteable(courseBanner).ExecuteCommand();
            return RedirectToAction("List");
        }


    }
}