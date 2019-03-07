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
    /// 官方案例
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CaseOfficialController : BaseController
    {

        private CaseOfficialBLL caseOfficialBLL = null;

        /// <summary>
        /// 
        /// </summary>
        public CaseOfficialController()
        {
            this.caseOfficialBLL = new CaseOfficialBLL();
        }


        /// <summary>
        /// 根据ID获取详细内容
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="caseOfficialId">*</param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult GetById([FromForm] string token, [FromForm] int caseOfficialId)
        {
            DataResult dr = new DataResult();
            try
            {
                CaseOfficialEntity caseOfficialEntity = caseOfficialBLL.GetById(caseOfficialId);

                CommentBLL commentBLL = new CommentBLL();
                caseOfficialEntity.commentCount = commentBLL.ListByTypeAndObjId((int)Entity.TypeEnumEntity.TypeEnum.官方案例, caseOfficialEntity.caseOfficialId).Count();

                EndorseBLL endorseBLL = new EndorseBLL();
                List<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjId((int)Entity.TypeEnumEntity.TypeEnum.官方案例, caseOfficialEntity.caseOfficialId);

                caseOfficialEntity.endorseCount = endorseEntities.Count();

                UserEntity userEntity = new UserEntity();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    userEntity = this.GetUserByToken(token);
                    if (endorseEntities.ToList().Exists(it => it.userId == userEntity.userId))
                    {
                        caseOfficialEntity.isEndorse = true;
                    }

                    //增加阅读记录
                    ReadBLL readBLL = new ReadBLL();
                    readBLL.Create(userEntity.userId, (int)Entity.TypeEnumEntity.TypeEnum.官方案例, caseOfficialId);
                }

                dr.code = "200";
                dr.data = caseOfficialEntity;
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