using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CaseBLL : Base.BaseBLL<CaseEntity>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="coverImage"></param>
        /// <param name="describe"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public CaseEntity Create(int userId, string title, string coverImage, string describe, string tips)
        {
            CaseEntity caseEntity = new CaseEntity()
            {
                coverImage = coverImage,
                createDate = DateTime.Now,
                describe = describe,
                integral = 0,
                isDel = false,
                modifyDate = DateTime.Now,
                state = -1,
                tips = tips ?? "",
                title = title,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(caseEntity).ExecuteReturnEntity();

        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public CaseEntity GetById(int caseId)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseEntity>().Where(it => it.caseId == caseId).First();
        }
    }
}
