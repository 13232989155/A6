using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class ShareTopicBLL : Base.BaseBLL<ShareTopicEntity>
    {
        /// <summary>
        /// 获取进行中的列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<ShareTopicEntity> UnderwayList()
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTopicEntity>()
                     .Where( it => it.isDel == false && it.startDate <= SqlFunc.GetDate() && SqlFunc.GetDate() <= it.endDate)
                     .OrderBy(it => it.startDate, OrderByType.Desc )
                     .ToList();
        }


    }
}
