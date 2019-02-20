using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
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

            ShareBLL shareBLL = new ShareBLL();
            IPagedList<ShareEntity> shareEntities = shareBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(shareEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            ShareBLL shareBLL = new ShareBLL();
            ShareEntity shareEntity = shareBLL.GetById(id);

            return View(shareEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            AdminEntity adminEntity = ThisAdmin();

            if ( adminEntity.userId < 10000)
            {
                return View(viewName: "ErrorDisplay", model: "未关联前端用户账号！！！");
            }

            return View();


        }

    }
}