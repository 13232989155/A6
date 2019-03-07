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
    public class CircleController : BaseController
    {

        /// <summary>
        /// 获取最新案例和说说
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult Newest([FromForm] string token, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {
            DataResult dr = new DataResult();
            try
            {
                CaseBLL caseBLL = new CaseBLL();
                ShareBLL shareBLL = new ShareBLL();
                CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();
                int caseCount = caseBLL.Count();
                int shareCount = shareBLL.Count();
                int caseOfficialCount = caseOfficialBLL.Count();
                int totalItemCount = caseCount + shareCount + caseOfficialCount;

                UserEntity userEntity = new UserEntity();
                if (!string.IsNullOrWhiteSpace(token))
                {
                    userEntity = this.GetUserByToken(token);
                }

                List<CircleResultHelper> circleResultHelpers = caseBLL.CaseAndShareList(pageNumber:pageNumber, pageSize:pageSize, totalCount:totalItemCount, thisUserId: userEntity.userId > 10000 ? userEntity.userId : -1);

                List<CaseEntity> caseEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.案例).Select(it => it.caseEntity).ToList();
                if (caseEntities.Count > 0)
                {
                    caseEntities = CaseListEndorseCountByList(caseEntities, userEntity.userId > 10000 ? userEntity.userId : -1);
                    caseEntities = CaseListCommentCountByList(caseEntities);

                    caseEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.caseId).caseEntity = it;
                    });
                }

                List<ShareEntity> shareEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.说说).Select(it => it.shareEntity).ToList();
                if (shareEntities.Count > 0)
                {
                    shareEntities = ShareListEndorseCountByList(shareEntities, userEntity.userId > 10000 ? userEntity.userId : -1);
                    shareEntities = ShareListCommentCountByList(shareEntities);

                    shareEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.shareId).shareEntity = it;
                    });
                }

                List<CaseOfficialEntity> caseOfficialEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.官方案例).Select(it => it.caseOfficialEntity).ToList();
                if (caseOfficialEntities.Count > 0)
                {
                    caseOfficialEntities = CaseOfficialEndorseCountByList(caseOfficialEntities, userEntity.userId > 10000 ? userEntity.userId : -1);
                    caseOfficialEntities = CaseOfficialCommentCountByList(caseOfficialEntities);

                    caseOfficialEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.caseOfficialId).caseOfficialEntity = it;
                    });
                }

                circleResultHelpers = (from crh in circleResultHelpers
                                       orderby crh.createDate descending
                                       select crh
                                       ).ToList();

                PageData pageData = new PageData(circleResultHelpers, pageNumber, pageSize, totalItemCount);

                dr.code = "200";
                dr.data = pageData;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }

        /// <summary>
        /// 获取关注的人的说说和案例
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Attention([FromForm] string token, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {
            DataResult dr = new DataResult();
            try
            {
                UserEntity userEntity = this.GetUserByToken(token);

                FansBLL fansBLL = new FansBLL();

                List<FansUserResult> fansUserResults = fansBLL.AttentionList(userEntity.userId);

                if ( fansUserResults.Count < 1)
                {
                    dr.code = "200";
                    dr.data = new PageData(null, pageNumber, pageSize, 0);
                    dr.msg = "未关注任何人";
                    return Json(dr);
                }

                CaseBLL caseBLL = new CaseBLL();
                ShareBLL shareBLL = new ShareBLL();
                CaseOfficialBLL caseOfficialBLL = new CaseOfficialBLL();

                int[] userIdInts = fansUserResults.Select(it => it.userId).ToArray();

                int caseCount = caseBLL.CountByUserIdInts(userIdInts);
                int shareCount = shareBLL.CountByUserIdInts(userIdInts);
                int caseOfficialCount = caseOfficialBLL.CountByUserIdInts(userIdInts);
                int totalItemCount = caseCount + shareCount + caseOfficialCount;

                List<CircleResultHelper> circleResultHelpers = caseBLL.CaseAndShareList(pageNumber: pageNumber, pageSize: pageSize, totalCount: totalItemCount, userIdInte:userIdInts);

                List<CaseEntity> caseEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.案例).Select(it => it.caseEntity).ToList();
                if (caseEntities.Count > 0)
                {
                    caseEntities = CaseListEndorseCountByList(caseEntities, userEntity.userId);
                    caseEntities = CaseListCommentCountByList(caseEntities);

                    caseEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.caseId).caseEntity = it;
                    });
                }

                List<ShareEntity> shareEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.说说).Select(it => it.shareEntity).ToList();
                if (shareEntities.Count > 0)
                {
                    shareEntities = ShareListEndorseCountByList(shareEntities, userEntity.userId);
                    shareEntities = ShareListCommentCountByList(shareEntities);

                    shareEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.shareId).shareEntity = it;
                    });

                }


                List<CaseOfficialEntity> caseOfficialEntities = circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.官方案例).Select(it => it.caseOfficialEntity).ToList();
                if (caseOfficialEntities.Count > 0)
                {
                    caseOfficialEntities = CaseOfficialEndorseCountByList(caseOfficialEntities, userEntity.userId);
                    caseOfficialEntities = CaseOfficialCommentCountByList(caseOfficialEntities);

                    caseOfficialEntities.ForEach(it =>
                    {
                        circleResultHelpers.Find(itt => itt.id == it.caseOfficialId).caseOfficialEntity = it;
                    });

                }


                circleResultHelpers = (from crh in circleResultHelpers
                                       orderby crh.createDate descending
                                       select crh
                                       ).ToList();

                PageData pageData = new PageData(circleResultHelpers, pageNumber, pageSize, totalItemCount);

                dr.code = "200";
                dr.data = pageData;

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