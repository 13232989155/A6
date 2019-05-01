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
    public class TeacherController : BaseController
    {
        private TeacherBLL teacherBLL = null;

        public TeacherController()
        {
            teacherBLL = new TeacherBLL();
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

            IPagedList<TeacherEntity> teacherEntities = teacherBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(teacherEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            TeacherEntity teacherEntity = teacherBLL.GetById(id);
            return View(teacherEntity);
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
        /// <param name="teacherEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("name, describe")] TeacherEntity teacherEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(teacherEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(teacherEntity.describe))
                {
                    dataResult.code = "201";
                    dataResult.msg = "描述不能为空";
                    return dataResult;
                }

                TeacherEntity teacher = new TeacherEntity()
                {
                    adminId = ThisAdmin().adminId,
                    createDate = DateTime.Now,
                    describe = teacherEntity.describe,
                    modifyDate = DateTime.Now,
                    name = teacherEntity.name,
                    isDel = false
                };

                int rows = teacherBLL.ActionDal.ActionDBAccess.Insertable(teacher).ExecuteCommand();

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
            TeacherEntity teacherEntity = teacherBLL.GetById(id);
            return View(teacherEntity);
        }


        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="teacherEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("name, describe, teacherId, isDel")] TeacherEntity teacherEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(teacherEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(teacherEntity.describe))
                {
                    dataResult.code = "201";
                    dataResult.msg = "描述不能为空";
                    return dataResult;
                }

                TeacherEntity teacher = teacherBLL.GetById(teacherEntity.teacherId);
                teacher.name = teacherEntity.name;
                teacher.describe = teacherEntity.describe;
                teacher.modifyDate = DateTime.Now;
                teacher.adminId = ThisAdmin().adminId;
                teacher.isDel = teacherEntity.isDel;

                int rows = teacherBLL.ActionDal.ActionDBAccess.Updateable(teacher).ExecuteCommand();

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

            TeacherEntity teacherEntity = teacherBLL.GetById(id);
            teacherEntity.isDel = true;
            teacherEntity.modifyDate = DateTime.Now;

            int rows = teacherBLL.ActionDal.ActionDBAccess.Updateable(teacherEntity).ExecuteCommand();

            return RedirectToAction("List");
        }
    }
}