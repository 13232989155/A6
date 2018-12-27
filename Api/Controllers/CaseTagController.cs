using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CaseTagController : BaseController
    {

        /// <summary>
        /// 获取案例标签列表
        /// </summary>
        /// <param name="token">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List([FromForm] string token)
        {

            DataResult dr = new DataResult();
            try
            {
                CaseTagBLL caseTagBLL = new CaseTagBLL();

                List<CaseTagEntity> caseTagEntities = caseTagBLL.List();

                dr.code = "200";
                dr.data = caseTagEntities;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }


    }
}