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
    public class EnshrineController : BaseController
    {
        /// <summary>
        /// 获取收藏列表
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult List([FromForm] string token, [FromForm] TypeEnumEntity.TypeEnum typeId)
        {
            DataResult dr = new DataResult();
            try
            {
                EnshrineBLL enshrineBLL = new EnshrineBLL();
                List<EnshrineEntity> enshrineEntities = new List<EnshrineEntity>();

                UserEntity userEntity = this.GetUserByToken(token);

                enshrineEntities = enshrineBLL.ListByUserId(userEntity.userId, (int)typeId);
                CaseBLL caseBLL = new CaseBLL();
                int[] caseIdInts = enshrineEntities.Where(it => it.typeId == (int)TypeEnumEntity.TypeEnum.案例).Select(it => it.objId).ToArray();
                if (caseIdInts.Count() > 0)
                {

                    List<CaseEntity> caseEntities = caseBLL.ListByCaseIdInts(caseIdInts);

                    if (caseEntities.Count > 0)
                    {
                        caseEntities = CaseListEndorseCountByList(caseEntities, userEntity.userId);
                        caseEntities = CaseListCommentCountByList(caseEntities);

                        caseEntities.ForEach(it =>
                        {
                            enshrineEntities.Find(itt => itt.typeId == (int)TypeEnumEntity.TypeEnum.案例 && itt.objId == it.caseId).caseEntity = it;
                        });
                    }

                }

                int[] caseOfficialIdInts = enshrineEntities.Where(it => it.typeId == (int)TypeEnumEntity.TypeEnum.官方案例).Select(it => it.objId).ToArray();
                if (caseOfficialIdInts.Count() > 0)
                {

                    List<CaseOfficialEntity> caseOfficialEntities = caseBLL.ListByCaseOfficialIdInts(caseOfficialIdInts);

                    if (caseOfficialEntities.Count > 0)
                    {
                        caseOfficialEntities = CaseOfficialEndorseCountByList(caseOfficialEntities, userEntity.userId);
                        caseOfficialEntities = CaseOfficialCommentCountByList(caseOfficialEntities);

                        caseOfficialEntities.ForEach(it =>
                        {
                            enshrineEntities.Find(itt => itt.typeId == (int)TypeEnumEntity.TypeEnum.官方案例 && itt.objId == it.caseOfficialId).caseOfficialEntity = it;
                        });
                    }

                }



                dr.code = "200";
                dr.data = enshrineEntities;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }

        /// <summary>
        /// 收藏
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="objId">*</param>
        /// <param name="typeId">案例：2；官方案例：3；课程：4；</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] int objId, [FromForm] TypeEnumEntity.TypeEnum typeId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000 || (int)typeId < 2)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                EnshrineBLL enshrineBLL = new EnshrineBLL();
                EnshrineEntity enshrineEntity = enshrineBLL.GetByIdAndTypeIdAndUserId(objId, (int)typeId, userEntity.userId);

                if ( enshrineEntity != null)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                    return Json(dr);
                }

                EnshrineEntity enshrine = new EnshrineEntity()
                {
                    createDate = DateTime.Now,
                    objId = objId,
                    typeId = (int)typeId,
                    userId = userEntity.userId
                };

                int rows = enshrineBLL.ActionDal.ActionDBAccess.Insertable(enshrine).ExecuteCommand();

                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="objId">*</param>
        /// <param name="typeId">案例：2；官方案例：3；课程：4；</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int objId, [FromForm] TypeEnumEntity.TypeEnum typeId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000 || (int)typeId < 2)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);


                EnshrineBLL enshrineBLL = new EnshrineBLL();
                EnshrineEntity enshrineEntity = enshrineBLL.GetByIdAndTypeIdAndUserId(objId, (int)typeId, userEntity.userId);

                int rows = enshrineBLL.ActionDal.ActionDBAccess.Deleteable(enshrineEntity).ExecuteCommand();

                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

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