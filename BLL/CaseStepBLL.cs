using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CaseStepBLL : Base.BaseBLL<CaseStepEntity>
    {

        /// <summary>
        /// 根据案例ID获取步骤列表
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public List<CaseStepEntity> ListByCaseId(int caseId)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseStepEntity>().Where(it => it.caseId == caseId).ToList();
        }
    }
}
