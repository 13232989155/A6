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
    public class CaseController : Controller
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

            CaseBLL caseBLL = new CaseBLL();
            IPagedList<CaseEntity> caseEntities = caseBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(caseEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            CaseBLL caseBLL = new CaseBLL();
            CaseEntity caseEntity = caseBLL.GetById(id);

            CaseTagCorrelationBLL caseTagCorrelationBLL = new CaseTagCorrelationBLL();
            caseEntity.caseTagEntities = caseTagCorrelationBLL.CaseTagListByCaseId(caseEntity.caseId);

            CaseStepBLL caseStepBLL = new CaseStepBLL();
            caseEntity.caseStepEntities = caseStepBLL.ListByCaseId(caseEntity.caseId);

            return View(caseEntity);
        }
    }
}