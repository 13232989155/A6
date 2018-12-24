using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ShareTypeBLL : Base.BaseBLL<ShareTypeEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public IEnumerable<ShareTypeEntity> List( bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTypeEntity>()
                     .WhereIF(!isDel, it => it.isDel == false)
                     .OrderBy(it => it.createDate)
                     .ToList();
        }
    }
}
