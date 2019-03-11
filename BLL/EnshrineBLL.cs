using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class EnshrineBLL : Base.BaseBLL<EnshrineEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objId"></param>
        /// <param name="typeId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public EnshrineEntity GetByIdAndTypeIdAndUserId(int objId, int typeId, int userId)
        {
            return ActionDal.ActionDBAccess.Queryable<EnshrineEntity>().Where(it => it.objId == objId && it.typeId == typeId && it.userId == userId).First();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public List<EnshrineEntity> ListByUserId(int userId, int typeId)
        {
            return ActionDal.ActionDBAccess.Queryable<EnshrineEntity>()
                    .WhereIF(typeId > 1, it => it.typeId == typeId)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }
    }
}
