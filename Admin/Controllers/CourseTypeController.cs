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
    public class CourseTypeController : BaseController
    {
        private CourseTypeBLL courseTypeBLL = null;

        public CourseTypeController()
        {
            courseTypeBLL = new CourseTypeBLL();
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

            IPagedList<CourseTypeEntity> courseTypeEntities = courseTypeBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(courseTypeEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CourseTypeEntity courseTypeEntity = courseTypeBLL.GetById(id);

            return View(courseTypeEntity);
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
        /// <param name="courseTypeEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("name, isDel, courseTypeId")] CourseTypeEntity courseTypeEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseTypeEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                CourseTypeEntity courseType = new CourseTypeEntity()
                {
                    adminId = ThisAdmin().adminId,
                    createDate = DateTime.Now,
                    isDel = false,
                    modifyDate = DateTime.Now,
                    name = courseTypeEntity.name
                };

                int rows = courseTypeBLL.ActionDal.ActionDBAccess.Insertable(courseType).ExecuteCommand();

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
            CourseTypeEntity courseTypeEntity = courseTypeBLL.GetById(id);
            return View(courseTypeEntity);
        }

        /// <summary>
        /// 保存编辑
        /// </summary>
        /// <param name="id"></param>
        /// <param name="courseTypeEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("name, courseTypeId")] CourseTypeEntity courseTypeEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseTypeEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                CourseTypeEntity courseType = courseTypeBLL.GetById(courseTypeEntity.courseTypeId);
                courseType.name = courseTypeEntity.name;
                courseType.isDel = courseTypeEntity.isDel;
                courseType.modifyDate = DateTime.Now;
                courseType.adminId = ThisAdmin().adminId;

                int rows = courseTypeBLL.ActionDal.ActionDBAccess.Updateable(courseType).ExecuteCommand();

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

            CourseTypeEntity courseTypeEntity = courseTypeBLL.GetById(id);
            courseTypeEntity.isDel = true;
            courseTypeEntity.modifyDate = DateTime.Now;
            courseTypeEntity.adminId = ThisAdmin().adminId;

            int rows = courseTypeBLL.ActionDal.ActionDBAccess.Updateable(courseTypeEntity).ExecuteCommand();

            return RedirectToAction("List");
        }

    }
}