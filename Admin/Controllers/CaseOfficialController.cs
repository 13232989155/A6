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
    public class CaseOfficialController : BaseController
    {
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

            CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
            IPagedList<CaseOfficialEntity> caseOfficialEntities = caseOfficialBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(caseOfficialEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
            CaseOfficialEntity caseOfficialEntity = caseOfficialBLL.GetById(id);
            return View(caseOfficialEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            AdminBLL adminBLL = new AdminBLL();
            AdminEntity adminEntity = adminBLL.GetById(ThisAdmin().adminId);

            if ( adminEntity.userId < 10000)
            {
                return View(viewName: "ErrorDisplay", model: "未关联前端用户账号！！！");
            }

            return View();
        }


        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("title, coverImage, contents")] CaseOfficialEntity caseOfficialEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.title))
                {
                    dataResult.code = "201";
                    dataResult.msg = "标题不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "图片不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.contents))
                {
                    dataResult.code = "201";
                    dataResult.msg = "内容不能为空";
                    return dataResult;
                }
                AdminBLL adminBLL = new AdminBLL();
                AdminEntity adminEntity = adminBLL.GetById(ThisAdmin().adminId);

                CaseOfficialEntity caseOfficial = new CaseOfficialEntity()
                {
                    contents = caseOfficialEntity.contents,
                    coverImage = caseOfficialEntity.coverImage,
                    createDate = DateTime.Now,
                    integral = 0,
                    isDel = false,
                    modifyDate = DateTime.Now,
                    state = -1,
                    title = caseOfficialEntity.title,
                    userId = adminEntity.userId
                };

                int rows = adminBLL.ActionDal.ActionDBAccess.Insertable(caseOfficial).ExecuteCommand();

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
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Edit(int id)
        {
            CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
            CaseOfficialEntity caseOfficialEntity = caseOfficialBLL.GetById(id);
            return View(caseOfficialEntity);
        }


        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Edit([Bind("caseOfficialId,title, coverImage, contents")] CaseOfficialEntity caseOfficialEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.title))
                {
                    dataResult.code = "201";
                    dataResult.msg = "标题不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.coverImage))
                {
                    dataResult.code = "201";
                    dataResult.msg = "图片不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(caseOfficialEntity.contents))
                {
                    dataResult.code = "201";
                    dataResult.msg = "内容不能为空";
                    return dataResult;
                }

                CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
                CaseOfficialEntity caseOfficial = caseOfficialBLL.GetById(caseOfficialEntity.caseOfficialId);

                caseOfficial.contents = caseOfficialEntity.contents;
                caseOfficial.title = caseOfficialEntity.title;
                caseOfficial.coverImage = caseOfficialEntity.coverImage;
                caseOfficial.modifyDate = DateTime.Now;

                int rows = caseOfficialBLL.ActionDal.ActionDBAccess.Updateable(caseOfficial).ExecuteCommand();

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

            CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
            CaseOfficialEntity caseOfficialEntity = caseOfficialBLL.GetById(id);

            caseOfficialEntity.isDel = true;
            caseOfficialEntity.modifyDate = DateTime.Now;

            int rows = caseOfficialBLL.ActionDal.ActionDBAccess.Updateable(caseOfficialEntity).ExecuteCommand();

            return RedirectToAction("List");
        }
    }
}