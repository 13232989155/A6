using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CaseTagBLL : Base.BaseBLL<CaseTagEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public IEnumerable<CaseTagEntity> List( bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseTagEntity>()
                    .WhereIF(!isDel, it => it.isDel == false)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }
    }
}
