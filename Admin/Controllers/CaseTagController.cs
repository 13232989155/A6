using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Controllers
{
    public class CaseTagController : Controller
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

            CaseTagBLL caseTagBLL = new CaseTagBLL();
            IPagedList<CaseTagEntity> caseTagEntities = caseTagBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(caseTagEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CaseTagBLL caseTagBLL = new CaseTagBLL();
            CaseTagEntity caseTagEntity = caseTagBLL.GetById(id);

            return View(caseTagEntity);
        }
    }
}