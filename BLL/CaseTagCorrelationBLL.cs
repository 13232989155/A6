using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CaseTagCorrelationBLL : Base.BaseBLL<CaseTagCorrelationEntity>
    {

        /// <summary>
        /// 根据案例ID获取标签列表
        /// </summary>
        /// <param name="caseId"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<CaseTagEntity> CaseTagListByCaseId(int caseId, bool isDel = false)
        {
            List<CaseTagEntity> caseTagEntities = new List<CaseTagEntity>();

            caseTagEntities = ActionDal.ActionDBAccess.Queryable<CaseTagEntity, CaseTagCorrelationEntity>((ct, ctc) => new object[]
                               {
                                    JoinType.Inner, ct.caseTagId == ctc.caseTagId && ctc.caseId == caseId
                               })
                                .WhereIF(!isDel, (ct, ctc) => ct.isDel == false)
                                .OrderBy(ct => ct.createDate, OrderByType.Desc)
                                .ToList();


            return caseTagEntities;
        }
    }
}
